using BT.ETS.Business;
using BT.ETS.Business.Constants;
using BT.ETS.Business.Exceptions;
using BT.ETS.Business.Helpers;
using BT.ETS.Business.Manager;
using BT.ETS.Business.Manager.Interface;
using BT.ETS.Business.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;


namespace BT.ETS.API.Services
{
    public class OrganizationServices
    {
        #region Constructor
        public OrganizationServices()
        {

        }
        #endregion

        #region Public Method

        internal async Task<List<OrganizationInfo>> GetEspOrgsByDate(string since)
        {
            DateTime? inputDate;
            try
            {
                
                if (!string.IsNullOrEmpty(since))
                {
                    CultureInfo cultures = new CultureInfo("en-US");
                    inputDate = Convert.ToDateTime(since, cultures);

                    if (inputDate < SqlDateTime.MinValue.Value || inputDate > SqlDateTime.MaxValue.Value)
                    {
                        throw new BusinessException(110);
                    }
                }
                else
                {
                    inputDate = null;
                }
            }
            catch (FormatException)
            {
                // may throw from Convert.ToDateTime(string value, IFormatProvider provider);
                var msg = string.Format(BusinessExceptionConstants.Message(100), since);
                throw new BusinessException(100, msg);
            }


            return await OrganizationManager.Instance.GetEspOrgsByDate(inputDate);
        }

        internal async Task<List<UserInfo>> GetLoginIDsByOrgId(string orgId)
        {
            if (string.IsNullOrEmpty(orgId))
                throw new BusinessException(101);

            return await OrganizationManager.Instance.GetLoginIDsByOrgId(orgId);
        }

        #endregion
    }
}