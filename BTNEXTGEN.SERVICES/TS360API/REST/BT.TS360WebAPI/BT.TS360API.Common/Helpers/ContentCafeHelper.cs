using System.Configuration;
using BT.TS360API.Common.contentcafe2;
using BT.TS360Constants;
using System.IO;
using System.Xml;
using System.Xml.Xsl;
using System;

namespace BT.TS360API.Common.Helpers
{
    public class ContentCafeHelper
    {
        public static string GetJacketImageUrl(string identifierValue, ImageSize imageSize, bool hasJacket = true)
        {
            string imgUrl;

            if (hasJacket)
            {
                var ccConfiguration = new ContentCafeConfiguration();
                var jacketSize = ccConfiguration.GetImageSize(imageSize);

                imgUrl = string.Format(ccConfiguration.ImageUrlFormat, ccConfiguration.AccessKey, identifierValue, jacketSize);
            }
            else
            {
                imgUrl = "/_layouts/IMAGES/CSDefaultSite/assets/images/common/";
                switch (imageSize)
                {
                    case ImageSize.Medium:
                        imgUrl += CommonConstants.NoImageMediumSize;
                        break;
                    case ImageSize.Large:
                        imgUrl += CommonConstants.NoImageLargeSize;
                        break;
                    default:
                        imgUrl += CommonConstants.NoImageSmallSize;
                        break;
                }
            }

            return imgUrl;
        }

      

        private static bool IsDebug
        {
            get
            {
                bool debug = false;
#if DEBUG
                debug = true;
#endif
                return debug;
            }
        }

        public static string GetTOCFromContentCafe(string btKey)
        {
            var ccXml = new ContentCafeXML();
            var key = new Key { Type = KeyType.ISBN, Value = btKey };
            
            //var requestItem = new RequestItem { Key = key, Content = new com.btol.contentcafe2.Content[1] };
            //requestItem.Content[0] = new com.btol.contentcafe2.Content { Value = com.btol.contentcafe2.ContentType.TocDetail };

            var requestItem = new RequestItem { Key = key, Content = new contentcafe2.Content[1] };
            requestItem.Content[0] = new contentcafe2.Content { Value = contentcafe2.ContentType.TocDetail };

            ccXml.RequestItems = new RequestItems();
            if (IsDebug)
            {
                ccXml.RequestItems.UserID = CommonConstants.UserForTest;
                ccXml.RequestItems.Password = CommonConstants.PwdForTest;
            }
            else
            {
                ccXml.RequestItems.UserID = CommonConstants.UserForProduction;
                ccXml.RequestItems.Password = CommonConstants.PwdForproduction;
            }

            ccXml.RequestItems.RequestItem = new RequestItem[1];
            ccXml.RequestItems.RequestItem[0] = requestItem;

            //var contentCafe = new contentcafe2.ContentCafe();
            var contentCafe = new contentcafe2.ContentCafeSoapClient();
            contentCafe.XmlClass(ref ccXml);

            string result = string.Empty;
            if (ccXml.RequestItems != null && ccXml.RequestItems.RequestItem != null && ccXml.RequestItems.RequestItem.Length > 0)
            {
                if (ccXml.RequestItems.RequestItem[0].TocItems != null && ccXml.RequestItems.RequestItem[0].TocItems.Length > 0)
                {
                    result = ccXml.RequestItems.RequestItem[0].TocItems[0].Toc;
                }
            }
            return result;
        }

        public static string GetProductMuzeFromContentCafe(string upc)
        {
            var ccXml = new ContentCafeXML();
            var key = new Key { Type = KeyType.UPC, Value = upc };
            var requestItem = new RequestItem { Key = key, Content = new contentcafe2.Content[6] };

            requestItem.Content[0] = new contentcafe2.Content();
            requestItem.Content[0].Value = contentcafe2.ContentType.MuzeVideoRelease;

            requestItem.Content[1] = new contentcafe2.Content();
            requestItem.Content[1].Value = contentcafe2.ContentType.MuzeSimilarCinema;

            requestItem.Content[2] = new contentcafe2.Content();
            requestItem.Content[2].Value = contentcafe2.ContentType.MuzePopularMusic;

            requestItem.Content[3] = new contentcafe2.Content();
            requestItem.Content[3].Value = contentcafe2.ContentType.MuzeEssentialArtists;

            requestItem.Content[4] = new contentcafe2.Content();
            requestItem.Content[4].Value = contentcafe2.ContentType.MuzeClassicalMusic;

            requestItem.Content[5] = new contentcafe2.Content();
            requestItem.Content[5].Value = contentcafe2.ContentType.MuzeGames;

            ccXml.RequestItems = new RequestItems();

            if (IsDebug)
            {
                ccXml.RequestItems.UserID = CommonConstants.UserForTest;
                ccXml.RequestItems.Password = CommonConstants.PwdForTest;
            }
            else
            {
                ccXml.RequestItems.UserID = CommonConstants.UserForProduction;
                ccXml.RequestItems.Password = CommonConstants.PwdForproduction;
            }

            ccXml.RequestItems.RequestItem = new RequestItem[1];
            ccXml.RequestItems.RequestItem[0] = requestItem;

            //using (var cc = new ContentCafe())            
            using (var cc = new contentcafe2.ContentCafeSoapClient())
            {
                cc.XmlClass(ref ccXml);
                if (ccXml.RequestItems.RequestItem.Length > 0 && ccXml.RequestItems.RequestItem[0].Muze != null
                    && !string.IsNullOrEmpty(ccXml.RequestItems.RequestItem[0].Muze.OuterXml))
                {
                    try
                    {
                        //string layoutPath = Path.Combine(SPUtility.GetGenericSetupPath("Template"), "LAYOUTS\\1033\\");
                        //string xslPath = Path.Combine(layoutPath, "muze-ORS.xsl");
                        string xslPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Template/muze-ORS.xsl");

                        var xmlInput = new StringReader(ccXml.RequestItems.RequestItem[0].Muze.OuterXml);
                        var xDoc = new XmlDocument();
                        xDoc.Load(xmlInput);
                        //
                        return TransformMuze(xDoc, xslPath, new XsltArgumentList());
                    }
                    catch (XmlException xmlEx)
                    {
                        return xmlEx.Message;
                    }
                }
            }
            return "";
        }

        private static string TransformMuze(XmlDocument inputXMLDocument, string xsltFilePath, XsltArgumentList xsltArgs)
        {
            //load the Xsl 
            var settings = new XmlReaderSettings { ProhibitDtd = false };
            var sw = new StringWriter();
            var xslTrans = new XslCompiledTransform();
            xslTrans.Load(XmlReader.Create(xsltFilePath, settings));
            xslTrans.Transform(inputXMLDocument.CreateNavigator(), xsltArgs, sw);
            return sw.ToString().Replace("[copyrightyear]", DateTime.Now.Year.ToString());
        }
    }
}
