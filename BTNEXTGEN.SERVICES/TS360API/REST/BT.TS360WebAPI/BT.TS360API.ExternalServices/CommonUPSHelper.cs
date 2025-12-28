using BT.TS360API.ExternalServices.UPSTrackingReference;
using BT.TS360API.Logging;
using BT.TS360API.ServiceContracts;
using BT.TS360Constants;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BT.TS360API.ExternalServices
{
    public class CommonUPSHelper
    {
        private static volatile CommonUPSHelper _instance;
        private static TrackPortTypeClient _serviceClient;
        private static readonly object SyncRoot = new Object();

        private readonly string UPSAPIUserName = ConfigurationManager.AppSettings["UPSAPIUserName"];
        private readonly string UPSAPIPassword = ConfigurationManager.AppSettings["UPSAPIPassword"];
        private readonly string UPSAPIAccessLicenseKey = ConfigurationManager.AppSettings["UPSAPIAccessLicenseKey"];

        private CommonUPSHelper()
        { // prevent init object outside
        }

        public static CommonUPSHelper Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                    {
                        _serviceClient = new TrackPortTypeClient(); 
                        _instance = new CommonUPSHelper();
                    }
                }

                return _instance;
            }
        }

        public async Task<UPSResponse> GetUPSData(string ShipTrackingNumber)
        {
            var result = new UPSResponse();

            try
            {
                TrackRequest tractRequest = new TrackRequest();

                UPSSecurity upss = new UPSSecurity();

                UPSSecurityServiceAccessToken upssSvcAccessToken = new UPSSecurityServiceAccessToken();
                upssSvcAccessToken.AccessLicenseNumber = UPSAPIAccessLicenseKey;

                upss.ServiceAccessToken = upssSvcAccessToken;

                UPSSecurityUsernameToken upssUsrNameToken = new UPSSecurityUsernameToken();
                upssUsrNameToken.Username = UPSAPIUserName;
                upssUsrNameToken.Password = UPSAPIPassword;

                upss.UsernameToken = upssUsrNameToken;

                RequestType request = new RequestType();
                String[] requestOption = { "15" };
                request.RequestOption = requestOption;
                tractRequest.Request = request;
                tractRequest.InquiryNumber = ShipTrackingNumber;

                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls | System.Net.SecurityProtocolType.Tls11; //This line will ensure the latest security protocol for consuming the web service call.

                _serviceClient = new TrackPortTypeClient();
                TrackResponse trackResponse = _serviceClient.ProcessTrack(upss, tractRequest);

                if (trackResponse.Shipment != null)
                {
                    if (trackResponse.Shipment[0].Package != null && trackResponse.Shipment[0].Package[0].Activity != null && string.Equals(trackResponse.Shipment[0].Package[0].Activity[0].Status.Description, "Delivered", StringComparison.OrdinalIgnoreCase))
                    {
                        
                        result.ShipmentStatus = UPSConstants.DELIVERED;
                        result.DeliveryDate = DateTime.ParseExact(trackResponse.Shipment[0].Package[0].Activity[0].Date, "yyyyddmm", CultureInfo.InvariantCulture).ToString("dd/mm/yyyy");
                    }
                    else if (trackResponse.Shipment[0].DeliveryDetail != null)
                    {
                        result.ShipmentStatus = UPSConstants.SHIPPED;
                        result.ExpectedDeliveryDate = DateTime.ParseExact(trackResponse.Shipment[0].DeliveryDetail[0].Date, "yyyyddmm", CultureInfo.InvariantCulture).ToString("dd/mm/yyyy");
                    }
                    else if (trackResponse.Shipment[0].Package != null && trackResponse.Shipment[0].Package[0].DeliveryDetail != null && trackResponse.Shipment[0].Package[0].DeliveryDetail[0].Type != null && string.Equals(trackResponse.Shipment[0].Package[0].DeliveryDetail[0].Type.Description, "Rescheduled Delivery", StringComparison.OrdinalIgnoreCase))
                    {
                        result.ShipmentStatus = UPSConstants.SHIPPED;
                        result.ExpectedDeliveryDate = DateTime.ParseExact(trackResponse.Shipment[0].Package[0].DeliveryDetail[0].Date, "yyyyddmm", CultureInfo.InvariantCulture).ToString("dd/mm/yyyy");
                    }
                    else
                    {
                        result.ShipmentStatus = UPSConstants.NO_TRACKING_AVAILABLE;
                    }
                }
                else
                {
                    result.ShipmentStatus = UPSConstants.NO_TRACKING_AVAILABLE;
                }
            }
            catch (FaultException<BT.TS360API.ExternalServices.UPSTrackingReference.ErrorDetailType[]> ex)
            {
                if (ex.Message.Equals("An exception has been raised as a result of client data.")
                    && ex.Code != null && ex.Code.Name == "Client")
                {
                    result.ShipmentStatus = UPSConstants.INVALID_TRACKING_NUMBER;
                }
                else
                    throw;
            }

            return result;
        }
    }
}
