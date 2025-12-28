using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using BT.TS360API.ServiceContracts;
using BT.TS360Constants;
using Microsoft.SqlServer.Server;
using System.Data.SqlClient;
using BT.TS360API.Common.Models;
using BT.TS360API.ServiceContracts.Request;

namespace BT.TS360API.Common.Helpers
{
    public class DataConverter
    {
        private const int MaxLength = 8000;

        internal static T ConvertTo<T>(DataRow row, string columnName)
        {
            if (row.Table.Columns.Contains(columnName))
            {
                if (!row.IsNull(columnName))
                {
                    return row.Field<T>(columnName);
                }
            }
            return default(T);
        }

        public static int GetOldAccountTypeID(int newAccounTypeId)
        {
            switch (newAccounTypeId)
            {
                case 1:
                    return (int)AccountType.OE;
                case 2: //book
                    return (int)AccountType.Book;
                case 3:
                    return (int)AccountType.Entertainment;
                case 9:
                    return (int)AccountType.OneBox;
                default:
                    return newAccounTypeId;
            }
        }

        public static List<CartAccount> ConvertListAccountSummaryToListCartAccount(List<AccountSummary> accountSummaries)
        {
            if (accountSummaries == null || accountSummaries.Count < 1)
            {
                return null;
            }
            var cartAccounts = new List<CartAccount>();
            foreach (var accountSummary in accountSummaries)
            {
                //Remove OE account
                if (accountSummary.AccountType == (int)AccountType.OE)
                {
                    continue;
                }
                var cartAccount = new CartAccount
                {
                    AccountID = accountSummary.AccountID,
                    AccountAlias = accountSummary.AccountAlias,
                    AccountERPNumber = accountSummary.AccountERPNumber,
                    AccountType = accountSummary.AccountType,
                    ESupplierID = accountSummary.ESupplierID,
                    EstimatedProcessingCharges = accountSummary.EstimateProcessingChange,
                    EstimatedTotalCartPrice = accountSummary.EstimateTotalCartPrice,
                    PONumber = accountSummary.PONumber,
                    TotalQuantity = accountSummary.TotalItems,
                    LineItemCount = accountSummary.TotalLines,

                    TotalListPrice = accountSummary.TotalListPrice,
                    TotalNetPrice = accountSummary.TotalNetPrice,
                    ERPAccountGUID = accountSummary.ERPAccountGUID,
                    IsHomeDelivery = accountSummary.IsHomeDelivery
                };
                cartAccounts.Add(cartAccount);
            }
            return cartAccounts;
        }

        /// <summary>  
        ///<para>This method takes a column name, type and maximum length, returning the column definition as SqlMetaData.</para>  
        /// </summary>
        /// <param name="System.String">A column name to be used in the returned Microsoft.SqlServer.Server.SqlMetaData.</param>
        /// <param name="System.Type">A column data type to be used in the returned Microsoft.SqlServer.Server.SqlMetaData.</param>
        /// <param name="System.Int32">The maximum length of the column to be used in the returned Microsoft.SqlServer.Server.SqlMetaData.</param>
        ///<param name="columnName"></param>
        ///<param name="type"></param>
        ///<param name="maxLength"></param>
        private static SqlMetaData ParseSqlMetaData(String columnName, Type type, Int64 maxLength)
        {
            var sqlParameter = new SqlParameter();
            sqlParameter.DbType = (DbType)TypeDescriptor.GetConverter(sqlParameter.DbType).ConvertFrom(type.Name);

            if (sqlParameter.SqlDbType == SqlDbType.Char || sqlParameter.SqlDbType == SqlDbType.NChar ||
                sqlParameter.SqlDbType == SqlDbType.NVarChar || sqlParameter.SqlDbType == SqlDbType.VarChar)
            {
                if (maxLength > MaxLength)
                {
                    maxLength = -1;
                }
                return new SqlMetaData(columnName, sqlParameter.SqlDbType, maxLength);
            }
            if (sqlParameter.SqlDbType == SqlDbType.Text || sqlParameter.SqlDbType == SqlDbType.NText)
            {
                return new SqlMetaData(columnName, sqlParameter.SqlDbType, -1);
            }
            return new SqlMetaData(columnName, sqlParameter.SqlDbType);



        }

        /// <summary>
        /// Generate a list of sql data record from a dataset
        /// </summary>
        /// <param name="dataSet"></param>
        /// <returns></returns>
        public static List<SqlDataRecord> GenerateDataRecords(DataSet dataSet)
        {
            if (dataSet == null)
                return null;

            var dataTable = dataSet.Tables[0];
            if (dataTable == null) return null;
            //create meta data
            var sqlMetaDatas = (from DataColumn col in dataTable.Columns
                                select ParseSqlMetaData(col.ColumnName, col.DataType, col.MaxLength)).ToList();
            var sqlMetaDataArray = sqlMetaDatas.ToArray();
            //fill data from dataset to data record
            var dataRecords = new List<SqlDataRecord>();
            foreach (DataRow row in dataTable.Rows)
            {
                var dataRecord = new SqlDataRecord(sqlMetaDataArray);
                dataRecord.SetValues(row.ItemArray);
                dataRecords.Add(dataRecord);
            }
            return dataRecords;
        }

        /// <summary>
        /// Generate a list of sql data record from list of data item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataList"></param>
        /// <param name="columnName"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static List<SqlDataRecord> GenerateDataRecords<T>(List<T> dataList, string columnName,
            int maxLength = 255)
        {
            if (dataList == null)
                return null;
            if (dataList.Count == 0)
                return null;

            var type = typeof(T);
            var sqlMetaDatas = new SqlMetaData[1];
            sqlMetaDatas[0] = ParseSqlMetaData(columnName, type, maxLength);
            //fill data from dataset to data record
            var dataRecords = new List<SqlDataRecord>();
            foreach (var item in dataList)
            {
                var dataRecord = new SqlDataRecord(sqlMetaDatas);
                dataRecord.SetValue(0, item);
                dataRecords.Add(dataRecord);
            }
            return dataRecords;
        }


        public static List<SqlDataRecord> GenerateDataRecords(Dictionary<string, Dictionary<string, string>> dict)
        {
            if (dict == null) return null;

            var dataRecords = new List<SqlDataRecord>();

            var firstRow = dict.Values.First();
            if (firstRow == null) return null;

            var sqlMetaDatas = new SqlMetaData[firstRow.Keys.Count];
            var listColumnNames = new Dictionary<string, int>();

            //Generating collumns metadata
            var colIndex = 0;
            foreach (var columnName in firstRow.Keys)
            {
                //var dataValue = firstRow[columnName];

                //var dataType = typeof(string);

                //if (dataValue != null)
                //    dataType = dataValue.GetType();

                sqlMetaDatas[colIndex] = ParseSqlMetaData(columnName, typeof(string), 255);
                listColumnNames.Add(columnName, colIndex);
                colIndex++;
            }

            //Binding rows
            foreach (var row in dict.Values)
            {
                var dataRecord = new SqlDataRecord(sqlMetaDatas);

                foreach (var cell in row)
                {
                    var ordinary = listColumnNames[cell.Key];

                    if (cell.Value != null)
                        dataRecord.SetString(ordinary, cell.Value);
                }

                dataRecords.Add(dataRecord);
            }

            return dataRecords;
        }

        public static DataSet ConvertCartLineItemsToDataset(List<LineItem> lineItems)
        {
            const string BTKEY_COLUMN = "BTKey";
            const string QUANTITY_COLUMN = "quantity";
            const string BASKETLINEITEMID_COLUMN = "BasketLineItemID";
            const string PONUMBER_COLUMN = "POLineItemNumber";
            const string BIB_COLUMN = "BibNumber";
            const string NOTE_COLUMN = "Note";
            const string RSP_COLUMN = "PrimaryResponsiblePartyRedundant";
            const string TITLE_COLUMN = "ShortTitleRedundant";
            DataSet ds = new DataSet();
            DataTable t = new DataTable();
            ds.Tables.Add(t);
            t.Columns.Add(BASKETLINEITEMID_COLUMN, typeof(string));
            t.Columns.Add(BTKEY_COLUMN, typeof(string));
            t.Columns.Add(QUANTITY_COLUMN, typeof(int));
            t.Columns.Add(PONUMBER_COLUMN, typeof(string));
            t.Columns.Add(NOTE_COLUMN, typeof(string));
            t.Columns.Add(BIB_COLUMN, typeof(string));
            t.Columns.Add(RSP_COLUMN, typeof(string));
            t.Columns.Add(TITLE_COLUMN, typeof(string));
            foreach (var item in lineItems)
            {
                DataRow row = t.NewRow();
                row[BASKETLINEITEMID_COLUMN] = item.Id;
                row[BTKEY_COLUMN] = item.BTKey;
                if (item.Quantity >= 0)
                    row[QUANTITY_COLUMN] = item.Quantity;
                else
                    row[QUANTITY_COLUMN] = DBNull.Value;
                row[PONUMBER_COLUMN] = item.PONumber;
                row[BIB_COLUMN] = item.Bib;
                row[NOTE_COLUMN] = item.BTLineItemNote;
                if (string.IsNullOrEmpty(item.Author))
                {
                    row[RSP_COLUMN] = DBNull.Value;
                }
                else
                {
                    row[RSP_COLUMN] = item.Author;
                }

                if (string.IsNullOrEmpty(item.Title))
                {
                    row[TITLE_COLUMN] = DBNull.Value;
                }
                else
                {
                    row[TITLE_COLUMN] = item.Title;
                }
                t.Rows.Add(row);
            }
            return ds;
        }

        public static List<SqlDataRecord> ConvertProductsToMergeCartLineItem(List<ProductLineItem> products)
        {
            var columns = new SqlMetaData[8];
            columns[0] = new SqlMetaData("BasketLineItemID", SqlDbType.NVarChar, 50);
            columns[1] = new SqlMetaData("BTKey", SqlDbType.Char, 10);
            columns[2] = new SqlMetaData("quantity", SqlDbType.Int);
            columns[3] = new SqlMetaData("POLineItemNumber", SqlDbType.NVarChar, 50);
            columns[4] = new SqlMetaData("Note", SqlDbType.Text);
            columns[5] = new SqlMetaData("BibNumber", SqlDbType.NVarChar, 25);
            columns[6] = new SqlMetaData("PrimaryResponsiblePartyRedundant", SqlDbType.NVarChar, 255);
            columns[7] = new SqlMetaData("ShortTitleRedundant", SqlDbType.NVarChar, 256);
            List<SqlDataRecord> dataRecords = new List<SqlDataRecord>();
            foreach (var item in products)
            {
                SqlDataRecord record = new SqlDataRecord(columns);
                if (!string.IsNullOrEmpty(item.LineItemId))
                    record.SetString(0, item.LineItemId);
                record.SetString(1, item.BTKey);
                if (item.Quantity >= 0)
                    record.SetInt32(2, item.Quantity);
                else
                {
                    record.SetDBNull(2);
                }
                if (!string.IsNullOrEmpty(item.PONumber))
                {
                    record.SetString(3, item.PONumber);
                }
                if (!string.IsNullOrEmpty(item.Note))
                {
                    record.SetString(4, item.Note);
                }
                if (!string.IsNullOrEmpty(item.BibNumber))
                {
                    record.SetString(5, item.BibNumber);
                }
                if (!string.IsNullOrEmpty(item.Author))
                {
                    record.SetString(6, item.Author);
                }
                else
                {
                    record.SetValue(6, DBNull.Value);
                }

                if (!string.IsNullOrEmpty(item.Title))
                {
                    record.SetString(7, item.Title);
                }
                else
                {
                    record.SetValue(7, DBNull.Value);
                }
                dataRecords.Add(record);
            }
            return dataRecords;
        }

        public static List<SqlDataRecord> ConvertCartGridLinesToDataSet(Dictionary<string, List<CommonCartGridLine>> cartGridLines)
        {
            if (cartGridLines == null || cartGridLines.Count == 0) return null;



            var dataRecords = new List<SqlDataRecord>();
            var sqlMetaDatas = new SqlMetaData[13];

            int colNum = 0;
            sqlMetaDatas[colNum++] = new SqlMetaData("AgencyCodeID", SqlDbType.VarChar, 50);
            sqlMetaDatas[colNum++] = new SqlMetaData("ItemTypeID", SqlDbType.VarChar, 50);
            sqlMetaDatas[colNum++] = new SqlMetaData("CollectionID", SqlDbType.VarChar, 50);

            for (int i = 1; i < 7; i++)
            {
                string userCodeID = "UserCode" + i + "ID";
                sqlMetaDatas[colNum] = new SqlMetaData(userCodeID, SqlDbType.VarChar, 50);
                colNum++;
            }
            sqlMetaDatas[colNum++] = new SqlMetaData("CallNumberText", SqlDbType.VarChar, 26);
            sqlMetaDatas[colNum++] = new SqlMetaData("Quantity", SqlDbType.Int);//colNum= 10
            sqlMetaDatas[colNum++] = new SqlMetaData("Sequence", SqlDbType.Int);//colNum= 11
            sqlMetaDatas[colNum++] = new SqlMetaData("BTKey", SqlDbType.NVarChar, 10);//colNum= 12
            foreach (KeyValuePair<string, List<CommonCartGridLine>> keyValuePair in cartGridLines)
            {
                var gridLineList = keyValuePair.Value;
                if (gridLineList == null || gridLineList.Count == 0) continue;
                gridLineList = gridLineList.OrderBy(item => item.Sequence).ToList();
                //var sequence = 1;
                foreach (CommonCartGridLine cartGridLine in gridLineList)
                {
                    var dataRecord = new SqlDataRecord(sqlMetaDatas);
                    var gridCodeList = cartGridLine.GridFieldCodeList;
                    for (var i = 0; i < gridCodeList.Count; i++)
                    {
                        var position = 0;
                        var gridFc = gridCodeList[i];
                        switch (gridFc.GridFieldType)
                        {
                            case GridFieldType.AgencyCode:
                                position = 0;
                                break;
                            case GridFieldType.ItemType:
                                position = 1;
                                break;
                            case GridFieldType.Collection:
                                position = 2;
                                break;
                            case GridFieldType.UserCode1:
                                position = 3;
                                break;
                            case GridFieldType.UserCode2:
                                position = 4;
                                break;
                            case GridFieldType.UserCode3:
                                position = 5;
                                break;
                            case GridFieldType.UserCode4:
                                position = 6;
                                break;
                            case GridFieldType.UserCode5:
                                position = 7;
                                break;
                            case GridFieldType.UserCode6:
                                position = 8;
                                break;
                            case GridFieldType.CallNumber:
                                position = 9;
                                break;
                        }
                        if (gridFc.GridFieldType != GridFieldType.CallNumber)
                        {
                            if (!string.IsNullOrEmpty(gridFc.GridCodeId))
                                dataRecord.SetString(position, gridFc.GridCodeId);
                            else
                                dataRecord.SetDBNull(position);
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(gridFc.GridTextValue))
                                dataRecord.SetString(position, gridFc.GridTextValue);
                            else
                                dataRecord.SetDBNull(position);
                        }

                    }
                    //TFS 10170: Add Sequence
                    //if (!string.IsNullOrEmpty(cartGridLine.AgencyCodeID))
                    //    dataRecord.SetString(0, cartGridLine.AgencyCodeID);
                    //else
                    //    dataRecord.SetDBNull(0);
                    //if (!string.IsNullOrEmpty(cartGridLine.ItemTypeID))
                    //    dataRecord.SetString(1, cartGridLine.ItemTypeID);
                    //else
                    //    dataRecord.SetDBNull(1);
                    //if (!string.IsNullOrEmpty(cartGridLine.CollectionID))
                    //    dataRecord.SetString(2, cartGridLine.CollectionID);
                    //else
                    //    dataRecord.SetDBNull(2); 
                    //if (!string.IsNullOrEmpty(cartGridLine.UserCode1ID))
                    //    dataRecord.SetString(3, cartGridLine.UserCode1ID);
                    //else
                    //    dataRecord.SetDBNull(3); 
                    //if (!string.IsNullOrEmpty(cartGridLine.UserCode2ID))
                    //    dataRecord.SetString(4, cartGridLine.UserCode2ID);
                    //else
                    //    dataRecord.SetDBNull(4); 
                    //if (!string.IsNullOrEmpty(cartGridLine.UserCode3ID))
                    //    dataRecord.SetString(5, cartGridLine.UserCode3ID);
                    //else
                    //    dataRecord.SetDBNull(5); 
                    //if (!string.IsNullOrEmpty(cartGridLine.UserCode4ID))
                    //    dataRecord.SetString(6, cartGridLine.UserCode4ID);
                    //else
                    //    dataRecord.SetDBNull(6); 
                    //if (!string.IsNullOrEmpty(cartGridLine.UserCode5ID))
                    //    dataRecord.SetString(7, cartGridLine.UserCode5ID);
                    //else
                    //    dataRecord.SetDBNull(7); 
                    //if (!string.IsNullOrEmpty(cartGridLine.UserCode6ID))
                    //    dataRecord.SetString(8, cartGridLine.UserCode6ID);
                    //else
                    //    dataRecord.SetDBNull(8); 
                    //if (!string.IsNullOrEmpty(cartGridLine.CallNumberText))
                    //    dataRecord.SetString(9, cartGridLine.CallNumberText);
                    //else
                    //    dataRecord.SetDBNull(9); 
                    if (cartGridLine.Quantity > 0)
                        dataRecord.SetInt32(10, cartGridLine.Quantity);
                    else
                        dataRecord.SetDBNull(10);
                    dataRecord.SetInt32(11, cartGridLine.Sequence);
                    dataRecord.SetString(12, keyValuePair.Key);
                    //int i = 0;
                    //foreach (GridTemplateFieldCode fieldCode in cartGridLine.GridFieldCodeList)
                    //{
                    //    dataRecord.SetString(i, fieldCode.GridFieldId);
                    //    if (!string.IsNullOrEmpty(fieldCode.GridCodeId))
                    //        dataRecord.SetString(i + 10, fieldCode.GridCodeId);
                    //    if (!string.IsNullOrEmpty(fieldCode.GridTextId))
                    //        dataRecord.SetString(i + 20, fieldCode.GridTextId);
                    //    i++;
                    //}
                    dataRecords.Add(dataRecord);
                }
            }

            return dataRecords;
        }

        public static List<SqlDataRecord> ConverUserIDsToDataSet(List<string> userIds)
        {
            var dataRecords = new List<SqlDataRecord>();
            var sqlMetaDatas = new SqlMetaData("GUID", SqlDbType.NVarChar, 50);
            foreach (string userId in userIds)
            {
                if (!string.IsNullOrEmpty(userId))
                {
                    var record = new SqlDataRecord(sqlMetaDatas);
                    record.SetString(0, userId);
                    dataRecords.Add(record);
                }
            }
            return dataRecords;
        }
        public static List<SqlDataRecord> ConvertUIUserGridFieldToDataSet(List<CommonBaseGridUserControl.UIUserGridField> userGridFieldObjects)
        {
            if (userGridFieldObjects == null || userGridFieldObjects.Count == 0)
            {
                return null;
            }

            var dataRecords = new List<SqlDataRecord>();
            var sqlMetaDatas = new SqlMetaData[5];

            //Generating collumns metadata
            sqlMetaDatas[0] = new SqlMetaData("UserGridFieldID", SqlDbType.NVarChar, 50);
            sqlMetaDatas[1] = new SqlMetaData("GridFieldID", SqlDbType.NVarChar, 50);
            sqlMetaDatas[2] = new SqlMetaData("DisplayType", SqlDbType.VarChar, 7);
            sqlMetaDatas[3] = new SqlMetaData("DefaultGridCodeID", SqlDbType.NVarChar, 50);
            sqlMetaDatas[4] = new SqlMetaData("DefaultGridText", SqlDbType.NVarChar, 26);

            //Binding rows
            foreach (var uiUserGridField in userGridFieldObjects)
            {
                var dataRecord = new SqlDataRecord(sqlMetaDatas);

                dataRecord.SetString(0, uiUserGridField.UserGridFieldID);
                dataRecord.SetString(1, uiUserGridField.GridFieldID);
                dataRecord.SetString(2, uiUserGridField.DisplayType);

                dataRecord.SetValue(3,
                    !string.IsNullOrEmpty(uiUserGridField.DefaultGridCodeID)
                        ? uiUserGridField.DefaultGridCodeID
                        : (object)DBNull.Value);

                dataRecord.SetString(4, uiUserGridField.DefaultGridText);

                dataRecords.Add(dataRecord);
            }

            return dataRecords;
        }
        public static List<SqlDataRecord> GetUtblBasketOrderFormsDataRecords(List<CartAccountSummary> dict)
        {
            if (dict == null || dict.Count < 1)
            {
                return null;
            }

            var dataRecords = new List<SqlDataRecord>();
            var sqlMetaDatas = new SqlMetaData[3];

            //Generating collumns metadata
            sqlMetaDatas[0] = new SqlMetaData("AccountID", SqlDbType.NVarChar, 20);
            sqlMetaDatas[1] = new SqlMetaData("BasketAccountTypeID", SqlDbType.BigInt);
            sqlMetaDatas[2] = new SqlMetaData("PONumber", SqlDbType.NVarChar, 50);

            //Binding rows
            foreach (var row in dict)
            {
                var dataRecord = new SqlDataRecord(sqlMetaDatas);
                if (!String.IsNullOrEmpty(row.AccountID))
                {
                    dataRecord.SetString(0, RetrieveCorrectERPValue(row.AccountID));
                }
                else
                {
                    dataRecord.SetDBNull(0);
                }

                int accountTypeId = 0;
                if (Int32.TryParse(row.BasketAccountTypeID, out accountTypeId))
                {
                    accountTypeId = GetNewAccountTypeID(accountTypeId);

                    dataRecord.SetInt64(1, accountTypeId);
                }

                if (row.PONumber != null)
                {
                    dataRecord.SetString(2, row.PONumber);
                }

                dataRecords.Add(dataRecord);
            }

            return dataRecords;
        }
        private static string RetrieveCorrectERPValue(string text)
        {
            var strText = text;
            string result;
            int start = strText.LastIndexOf("(") + 1;
            if (start > 1)
            {
                int end = strText.LastIndexOf(")");
                result = strText.Substring(start, end - start);
            }
            else
            {
                result = strText;
            }
            return result;
        }
        public static int GetNewAccountTypeID(int oldAccountTypeID)
        {
            switch (oldAccountTypeID)
            {
                case (int)AccountType.Book: //book
                    return 2;
                case (int)AccountType.Entertainment:
                    return 3;
                case (int)AccountType.OneBox:
                    return 9;
                default:
                    return oldAccountTypeID;
            }
        }
        public static List<SqlDataRecord> ConvertGridTemplateLinesToDataSetNew(List<CommonGridTemplateLine> gridTemplateLines)
        {
            if (gridTemplateLines == null || gridTemplateLines.Count == 0) return null;

            gridTemplateLines = gridTemplateLines.OrderBy(item => item.Sequence).ToList();

            var dataRecords = new List<SqlDataRecord>();
            var sqlMetaDatas = new SqlMetaData[16];
            sqlMetaDatas[0] = new SqlMetaData("GridTemplateLineID", SqlDbType.NVarChar, 50);
            sqlMetaDatas[1] = new SqlMetaData("GridTemplateID", SqlDbType.NVarChar, 50);
            sqlMetaDatas[2] = new SqlMetaData(GridFieldType.AgencyCode + "ID", SqlDbType.NVarChar, 50);
            sqlMetaDatas[3] = new SqlMetaData(GridFieldType.ItemType + "ID", SqlDbType.NVarChar, 50);
            sqlMetaDatas[4] = new SqlMetaData(GridFieldType.Collection + "ID", SqlDbType.NVarChar, 50);
            sqlMetaDatas[5] = new SqlMetaData(GridFieldType.CallNumber + "Text", SqlDbType.VarChar, 26);
            sqlMetaDatas[6] = new SqlMetaData(GridFieldType.UserCode1 + "ID", SqlDbType.NVarChar, 50);
            sqlMetaDatas[7] = new SqlMetaData(GridFieldType.UserCode2 + "ID", SqlDbType.NVarChar, 50);
            sqlMetaDatas[8] = new SqlMetaData(GridFieldType.UserCode3 + "ID", SqlDbType.NVarChar, 50);
            sqlMetaDatas[9] = new SqlMetaData(GridFieldType.UserCode4 + "ID", SqlDbType.NVarChar, 50);
            sqlMetaDatas[10] = new SqlMetaData(GridFieldType.UserCode5 + "ID", SqlDbType.NVarChar, 50);
            sqlMetaDatas[11] = new SqlMetaData(GridFieldType.UserCode6 + "ID", SqlDbType.NVarChar, 50);

            sqlMetaDatas[12] = new SqlMetaData("Quantity", SqlDbType.Int);
            sqlMetaDatas[13] = new SqlMetaData("EnabledIndicator", SqlDbType.Bit);
            sqlMetaDatas[14] = new SqlMetaData("Sequence", SqlDbType.Int);
            sqlMetaDatas[15] = new SqlMetaData("TempDisabledIndicator", SqlDbType.Bit);

            //var sequence = gridTemplateLines.Count;
            foreach (var gridLine in gridTemplateLines)
            {
                var dataRecord = new SqlDataRecord(sqlMetaDatas);
                dataRecord.SetString(0, gridLine.ID);
                dataRecord.SetDBNull(1);

                if (!string.IsNullOrEmpty(gridLine.AgencyCodeID))
                    dataRecord.SetString(2, gridLine.AgencyCodeID);
                else
                {
                    dataRecord.SetDBNull(2);
                }

                if (!string.IsNullOrEmpty(gridLine.ItemTypeID))
                    dataRecord.SetString(3, gridLine.ItemTypeID);
                else
                {
                    dataRecord.SetDBNull(3);
                }

                if (!string.IsNullOrEmpty(gridLine.CollectionID))
                    dataRecord.SetString(4, gridLine.CollectionID);
                else
                {
                    dataRecord.SetDBNull(4);
                }

                if (!string.IsNullOrEmpty(gridLine.CallNumberText))
                    dataRecord.SetString(5, gridLine.CallNumberText);
                else
                {
                    dataRecord.SetDBNull(5);
                }

                if (!string.IsNullOrEmpty(gridLine.UserCode1ID))
                    dataRecord.SetString(6, gridLine.UserCode1ID);
                else
                {
                    dataRecord.SetDBNull(6);
                }

                if (!string.IsNullOrEmpty(gridLine.UserCode2ID))
                    dataRecord.SetString(7, gridLine.UserCode2ID);
                else
                {
                    dataRecord.SetDBNull(7);
                }

                if (!string.IsNullOrEmpty(gridLine.UserCode3ID))
                    dataRecord.SetString(8, gridLine.UserCode3ID);
                else
                {
                    dataRecord.SetDBNull(8);
                }

                if (!string.IsNullOrEmpty(gridLine.UserCode4ID))
                    dataRecord.SetString(9, gridLine.UserCode4ID);
                else
                {
                    dataRecord.SetDBNull(9);
                }

                if (!string.IsNullOrEmpty(gridLine.UserCode5ID))
                    dataRecord.SetString(10, gridLine.UserCode5ID);
                else
                {
                    dataRecord.SetDBNull(10);
                }

                if (!string.IsNullOrEmpty(gridLine.UserCode6ID))
                    dataRecord.SetString(11, gridLine.UserCode6ID);
                else
                {
                    dataRecord.SetDBNull(11);
                }

                dataRecord.SetInt32(12, gridLine.Qty);
                dataRecord.SetBoolean(13, true);
                dataRecord.SetInt32(14, gridLine.Sequence);
                dataRecord.SetBoolean(15, gridLine.IsTempDisabled);
                dataRecords.Add(dataRecord);
                //sequence--;
            }
            return dataRecords;
        }
        public static List<SqlDataRecord> GetUtblProcTs360CopyLineItemsToNewCartDataRecords(Dictionary<string, Dictionary<string, string>> dict)
        {
            if (dict == null) return null;

            var dataRecords = new List<SqlDataRecord>();
            var sqlMetaDatas = new SqlMetaData[8];

            //Generating collumns metadata
            sqlMetaDatas[0] = new SqlMetaData("BasketLineItemID", SqlDbType.NVarChar, 50);
            sqlMetaDatas[1] = new SqlMetaData("BTKey", SqlDbType.Char, 10);
            sqlMetaDatas[2] = new SqlMetaData("quantity", SqlDbType.Int);
            sqlMetaDatas[3] = new SqlMetaData("POLineItemNumber", SqlDbType.NVarChar, 50);
            sqlMetaDatas[4] = new SqlMetaData("Note", SqlDbType.NVarChar, -1);
            sqlMetaDatas[5] = new SqlMetaData("BibNumber", SqlDbType.NVarChar, 25);
            sqlMetaDatas[6] = new SqlMetaData("PrimaryResponsiblePartyRedundant", SqlDbType.NVarChar, 255);
            sqlMetaDatas[7] = new SqlMetaData("ShortTitleRedundant", SqlDbType.NVarChar, 256);
            //Binding rows
            foreach (var row in dict.Values)
            {
                var dataRecord = new SqlDataRecord(sqlMetaDatas);
                dataRecord.SetString(0, row["BasketLineItemID"]);
                dataRecord.SetString(1, row["BTKey"]);
                dataRecord.SetDBNull(2);
                dataRecord.SetDBNull(3);
                dataRecord.SetDBNull(4);
                dataRecord.SetDBNull(5);
                dataRecord.SetDBNull(6);
                dataRecord.SetDBNull(7);

                dataRecords.Add(dataRecord);
            }

            return dataRecords;
        }
        public static List<SqlDataRecord> ConvertILSLineItemDetailToDataTable(List<ILSLineItemDetail> lstILSLineItemDetail)
        {
            var columns = new SqlMetaData[3];
            columns[0] = new SqlMetaData("BasketLineItemID", SqlDbType.NVarChar, 50);
            columns[1] = new SqlMetaData("ILSOrderNumber", SqlDbType.NVarChar, 100);
            columns[2] = new SqlMetaData("ILSBIBNumber", SqlDbType.NVarChar, 100);
            List<SqlDataRecord> dataRecords = new List<SqlDataRecord>();

            lstILSLineItemDetail.ForEach(item =>
            {
                SqlDataRecord record = new SqlDataRecord(columns);
                record.SetString(0, item.BasketLineItemID);
                record.SetString(1, item.ILSOrderNumber.Substring(item.ILSOrderNumber.LastIndexOf('/') + 1 ) );
                record.SetString(2, item.ILSBIBNumber.Substring(item.ILSBIBNumber.LastIndexOf('/') + 1 ) );
                dataRecords.Add(record);
            });

            return dataRecords;
        }
    }
}
