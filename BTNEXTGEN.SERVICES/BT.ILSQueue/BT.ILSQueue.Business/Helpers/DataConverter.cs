using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;

using Microsoft.SqlServer.Server;
using System.Data.SqlClient;

using BT.TS360API.ServiceContracts;
using BT.ILSQueue.Business.Constants;
using BT.ILSQueue.Business.Models;

namespace BT.ILSQueue.Business.Helpers
{
    public class DataConverter
    {
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

        public static List<SqlDataRecord> ConvertILSLineItemDetailToDataTable(List<ILSLineItemDetail> lstILSLineItemDetail)
        { /*
                [BasketLineItemID] [nvarchar](50) NULL,
	            [ILSOrderNumber] [nvarchar](100) NULL,
	            [ILSBIBNumber] [nvarchar](100) NULL,
	            [LocationCode] [varchar](255) NULL,
	            [FundCode] [varchar](255) NULL,
	            [CollectionCode] [varchar](255) NULL,
	            [AdditionalField1] [varchar](50) NULL*/

            var columns = new SqlMetaData[7];
            columns[0] = new SqlMetaData("BasketLineItemID", SqlDbType.NVarChar, 50);
            columns[1] = new SqlMetaData("ILSOrderNumber", SqlDbType.NVarChar, 100);
            columns[2] = new SqlMetaData("ILSBIBNumber", SqlDbType.NVarChar, 100);
            columns[3] = new SqlMetaData("LocationCode", SqlDbType.VarChar, 255);
            columns[4] = new SqlMetaData("FundCode", SqlDbType.VarChar, 255);
            columns[5] = new SqlMetaData("CollectionCode", SqlDbType.VarChar, 255);
            columns[6] = new SqlMetaData("AdditionalField1", SqlDbType.VarChar, 50);
            List<SqlDataRecord> dataRecords = new List<SqlDataRecord>();

            lstILSLineItemDetail.ForEach(item =>
            {
                SqlDataRecord record = new SqlDataRecord(columns);
                record.SetString(0, item.BasketLineItemID);
                record.SetString(1, item.ILSOrderNumber);
                record.SetString(2, item.ILSBIBNumber);
                record.SetString(3, item.LocationCode);
                record.SetString(4, item.FundCode);
                record.SetString(5, item.CollectionCode);
                record.SetString(6, item.ILSPolySegID);
                dataRecords.Add(record);
            });

            return dataRecords;


        }
        public static T ConvertTo<T>(DataRow row, string columnName)
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
    }
}
