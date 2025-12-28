using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BT.TS360API.Logging;
using BT.TS360API.Marketing;
using BT.TS360API.ServiceContracts;
using BT.TS360Constants;
using Microsoft.SharePoint.Client;

namespace BT.TS360SP
{
    /// <summary>
    /// Base Content Management List
    /// </summary>
    /// <typeparam name="T">DTO</typeparam>
    public abstract class CMListBase<T> : IDisposable where T : ICMListItem, new()
    {
        //private static IList<T> _storage = new List<T>();

        //private static readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        public bool HasDisplayOrder = false;
        public bool HasIsDefault = true;
        public bool IsGettingById = false;

        public int RowLimit = 200;
        public bool HasAdName = false;
        private bool _alreadyGettingIsDefault = false;

        /// <summary>
        /// SharePoint Web
        /// </summary>        
        protected string CurrentWebUrl;
        public TargetingParam Targeting { get; set; }
        private ICMListSite _site;
        protected CMListBase(string site="")
        {
            switch (site)
            {
                case "Home":
                    _site = new CMListBaseHome();
                    break;
                case "Promotion":
                    _site = new CMListBasePromotion();
                    break;
                case "Preview":
                    _site = new CMListBasePreview();
                    break;
                default:
                    _site = new CMListBasePublishing();
                    break;

            }
            Initialize();
        }

        private void Initialize()
        {

            CurrentWebUrl = _site.GetWebUrl();
        }
        protected virtual string GetViewFields()
        {
            return string.Empty;
        }
        protected virtual string GetSortFields()
        {
            return string.Empty;
        }
        protected virtual string GetGroupBy()
        {
            return string.Empty;
        }
        /// <summary>
        /// Get SharePoint List name.
        /// </summary>
        /// <returns></returns>
        protected abstract string GetListName();
        public IList<T> Get()
        {
            var items = GetListItems();
            items = Filter(items);
            items = PrioritizeTargeting(items);
            Refine(items);
            return items;
        }
        private IList<T> GetListItems()
        {
            var listName = GetListName();

            var storage = new List<T>();

            using (var clientContext = new ClientContext(CurrentWebUrl))
            {
                try
                {
                    var list = clientContext.Web.Lists.GetByTitle(listName);

                    if (list == null) return storage;
                    ListItemCollection listItems = null;

                    var strQuery=CreateQuery(listName, true);
                    if (!string.IsNullOrEmpty(strQuery))
                        listItems = RunQuery(strQuery, list, clientContext);

                    if (listItems==null||listItems.Count == 0)
                    {
                        strQuery = CreateQuery(listName,false);
                        listItems = RunQuery(strQuery, list, clientContext);
                    }

                    foreach (ListItem listItem in listItems)
                    {
                        var item = new T();
                        item.Initialize(listItem);
                        storage.Add(item);
                    }
                }
                catch (Exception ex)
                {
                    //Logger.LogException(ex);
                    Logger.Write("CMListBase", string.Format("Message: {0}, List: {1} Trace: {2}", ex.Message, listName, ex.StackTrace));
                }
            }
            return storage;
        }

        private ListItemCollection RunQuery(string strQuery, List list, ClientContext clientContext)
        {
            var query = new CamlQuery();
            query.ViewXml = strQuery;
            ListItemCollection listItems = list.GetItems(query);
            IncludeFields(clientContext, listItems);
            clientContext.ExecuteQuery();
            return listItems;
        }

        protected virtual void IncludeFields(ClientContext clientContext,ListItemCollection listItems)
        {
        }
        /// <summary>
        /// Fresh load ListItems of T
        /// </summary>
        /// <returns></returns>
        public IList<T> LoadPreviewListItems()
        {
            var result = new List<T>();
            //SPSecurity.RunWithElevatedPrivileges(delegate()
            //{
            //    using (var site = new SPSite(GetCollaborationWebUrl()))
            //    {
            //        using (var web = site.OpenWeb())
            //        {
            //            var listName = GetListName();

            //            try
            //            {
            //                SPList list = web.Lists[listName];

            //                if (list != null)
            //                {
            //                    var query = new SPQuery
            //                    {
            //                        RowLimit = (uint)RowLimit,
            //                        Query = CreateQuery(list, false, false)
            //                    };

            //                    var items = list.GetItems(query);
            //                    if (items.Count == 0 && !_alreadyGettingIsDefault)
            //                    {
            //                        SPQuery query1 = new SPQuery
            //                        {
            //                            RowLimit = (uint)RowLimit,
            //                            Query = CreateQuery(list, true, false)
            //                        };
            //                        items = list.GetItems(query1);
            //                    }
            //                    foreach (SPListItem listItem in items)
            //                    {
            //                        var item = new T();
            //                        item.Initialize(listItem);
            //                        result.Add(item);
            //                    }
            //                }
            //            }
            //            catch (Exception ex)
            //            {
            //                //Logger.Write("CMListBase", string.Format("LoadPreviewListItems: {0}, List: {1} Trace: {2}", ex.Message, listName, ex.StackTrace));
            //            }
            //        }
            //    }
            //});
            return result;
        }

        private string CreateQuery(string listName,bool withMarketing)
        {
            var query = new StringBuilder();

            var whereString = string.Empty;
            if (withMarketing)
            {
                if (HasAdName)
                {
                    // Get Ad Name from CS MM
                    var listAdName = Targeting != null ?
                        MarketingController.GetCampaignAdItem(listName, Targeting) :
                        MarketingController.GetCampaignAdItem(listName);
                    //var listAdName = Targeting != null ?
                    //    MarketingController.GetCampaignAdItem(listName, Targeting) :
                    //    MarketingController.GetCampaignAdItem(listName);
                    if(listAdName != null && listAdName.Count > 0)
                    whereString = ContentManagementHelper.BuildCamlString(listAdName);
                }
                else
                    return string.Empty;
            }
            else if (HasIsDefault)
            {
                whereString = ContentManagementHelper.CreateCamlEqualBoolValue(CMFieldNameConstants.IsDefault, true);
            }

            query.Append("<View>");
            query.Append(GetViewFields());
            query.AppendFormat("<RowLimit>{0}</RowLimit>", RowLimit);

            query.Append("<Query>");
            // to be override in the inherit class
            whereString = AddWhereCamlTo(whereString);
            whereString = _site.QueryCondition(whereString);
            if (!string.IsNullOrEmpty(whereString))
                query.Append(ContentManagementHelper.CreateCamlWhereExpression(whereString));

            query.Append(GetOrderBy());
            query.Append("</Query>");
            query.Append("</View>");
            return query.ToString();
        }

        private string GetOrderBy()
        {
            string displayOrderCamlSortString = GetSortFields();
            //if (string.IsNullOrEmpty(displayOrderCamlSortString) && HasDisplayOrder)
            //    displayOrderCamlSortString = string.Format("<FieldRef Name=\"{0}\" Ascending=\"TRUE\" />", CMFieldNameConstants.DisplayOrder);
            
            if (!string.IsNullOrEmpty(displayOrderCamlSortString))
                displayOrderCamlSortString = ContentManagementHelper.CreateCamlOrderExpression(displayOrderCamlSortString);
            else if (HasDisplayOrder)
                displayOrderCamlSortString = ContentManagementHelper.CreateCamlOrderExpression(string.Format("<FieldRef Name=\"{0}\" Ascending=\"TRUE\" />", CMFieldNameConstants.DisplayOrder));

            return displayOrderCamlSortString;
        }

        //private readonly Dictionary<string, string> _cacheListName = new Dictionary<string, string>()
        //                                                          {
        //                                                              {"GeneralInformation", "GeneralInformation"},
        //                                                              {"TitleBar", "TitleBar"},
        //                                                              {"HeaderTitles", "HeaderTitles"}
        //                                                          };

        //private bool CanBeCache(string listName)
        //{
        //    return _cacheListName.ContainsKey(listName);
        //}

        protected virtual IList<T> PrioritizeTargeting(IList<T> items)
        {
            if (this.HasAdName)
                return items.OrderBy(x => x.GetAdName()).ToList();//.CompareTo(y.AdName));
            return items;
        }

        /// <summary>
        /// Clear CM Cache of a SPListBase<T>
        /// </summary>
        //public void ClearCache()
        //{
        //    _lock.EnterWriteLock();
        //    try
        //    {
        //        _storage.Clear();
        //    }
        //    finally
        //    {
        //        _lock.ExitWriteLock();
        //    }
        //}

        /// <summary>
        /// Refine the CM ListItem to afford a business rule
        /// </summary>
        /// <param name="items"></param>
        protected virtual void Refine(IList<T> items)
        {

        }

        protected virtual string AddWhereCamlTo(string currentCaml)
        {
            return currentCaml;
        }

        protected virtual IList<T> Filter(IList<T> item)
        {
            return item;
        }

        

        public virtual void Dispose()
        {
            //ClearCache();
        }
    }

}
