using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BT.TS360API.Common.DataAccess;
using BT.TS360API.ServiceContracts;
using BT.TS360Constants;
using BT.TS360API.Common.Models;

namespace BT.TS360API.Common.Helpers
{
    public class GridHelper
    {
        public static List<DCGridTitleProperty> RemoveDuplicateDCGridLines(List<DCGridTitleProperty> dcGridTitleProperties)
        {
            for (var i = 0; i < dcGridTitleProperties.Count; i++)
            {
                List<string> forceDeletedItems;
                dcGridTitleProperties[i] = RemoveDuplicateDCGridLines(dcGridTitleProperties[i], out forceDeletedItems);
            }
            return dcGridTitleProperties;
        }

        /// <summary>
        /// Remove all duplicate gridlines
        /// </summary>
        /// <param name="dcGridTitleProperty"></param>
        /// <returns>dcGridTitleProperty</returns>
        public static DCGridTitleProperty RemoveDuplicateDCGridLines(DCGridTitleProperty dcGridTitleProperty, out List<string> forceDeletedItems)
        {
            var duplicatedItems = new List<DCGridLine>();
            forceDeletedItems = null;
            if (dcGridTitleProperty == null) return null;
            var dcGridLines = dcGridTitleProperty.DCGridLines;
            if (dcGridLines == null)
            {
                return dcGridTitleProperty;
            }

            for (var i = 0; i < dcGridLines.Count - 1; i++)
            {
                for (var j = dcGridLines.Count - 1; j > i; j--)
                {
                    if (IsDuplicated(dcGridLines[i], dcGridLines[j]))
                    {
                        var removedLine = CompareLastModifiedTime(dcGridLines[i].LastModifiedTime, dcGridLines[j].LastModifiedTime) ?
                            dcGridLines[i] : dcGridLines[j];

                        if (!duplicatedItems.Contains(removedLine))
                        {
                            duplicatedItems.Add(removedLine);
                        }

                        if (string.Compare(removedLine.LineStatus, LineStatus.Add.ToString(),
                            StringComparison.OrdinalIgnoreCase) != 0)
                        {
                            if (dcGridTitleProperty.DeletedItems == null)
                            {
                                dcGridTitleProperty.DeletedItems = new List<string>();
                            }
                            //
                            if (!dcGridTitleProperty.DeletedItems.Contains(removedLine.ID))
                            {
                                dcGridTitleProperty.DeletedItems.Add(removedLine.ID);
                            }

                            if (forceDeletedItems == null)
                            {
                                forceDeletedItems = new List<string>();
                            }

                            if (!forceDeletedItems.Contains(removedLine.ID))
                            {
                                forceDeletedItems.Add(removedLine.ID);
                            }
                        }
                    }
                }
            }

            if (duplicatedItems.Count > 0)
            {
                foreach (var duplicatedItem in duplicatedItems)
                {
                    dcGridTitleProperty.DCGridLines.Remove(duplicatedItem);
                }
            }
            return dcGridTitleProperty;
        }

        public static void RemoveDuplicateDCGridLines(List<DCGridInputData> dcGridTitleProperties)
        {
            foreach (DCGridInputData t in dcGridTitleProperties)
            {
                RemoveDuplicateDCGridLines(t);
            }
        }

        public static void RemoveDuplicateDCGridLines(DCGridInputData dcGridTitleProperty)
        {
            if (dcGridTitleProperty == null) return;
            var dcGridLines = dcGridTitleProperty.DCGridLines;
            if (dcGridLines == null) return;

            for (var i = 0; i < dcGridLines.Count - 1; i++)
            {
                var templateLine = dcGridLines[i];
                var lastModifiedTime = templateLine.LastModifiedTime;
                for (var j = dcGridLines.Count - 1; j > i; j--)
                {
                    var compareLine = dcGridLines[j];
                    var lastModifedTimeCompare = compareLine.LastModifiedTime;
                    var flag = false;
                    for (var k = 0; k < dcGridLines[i].DCGridFieldCodes.Count; k++)
                    {
                        flag = (templateLine.DCGridFieldCodes[k].GridCode == compareLine.DCGridFieldCodes[k].GridCode)
                               && (templateLine.DCGridFieldCodes[k].GridCodeId == compareLine.DCGridFieldCodes[k].GridCodeId)
                               && (templateLine.DCGridFieldCodes[k].GridCodeText == compareLine.DCGridFieldCodes[k].GridCodeText)
                               && (templateLine.DCGridFieldCodes[k].GridFieldId == compareLine.DCGridFieldCodes[k].GridFieldId)
                               && (templateLine.DCGridFieldCodes[k].GridFieldIdToName == compareLine.DCGridFieldCodes[k].GridFieldIdToName);
                        if (!flag)
                        {
                            break;
                        }
                    }
                    if (flag)
                    {
                        DCGridLine removedLine;
                        if (CompareLastModifiedTime(lastModifiedTime, lastModifedTimeCompare))
                        {
                            removedLine = templateLine;
                            dcGridTitleProperty.DCGridLines.RemoveAt(i);
                            templateLine = dcGridLines[i];
                            //reset for loop
                            j = dcGridLines.Count;

                        }
                        else
                        {
                            removedLine = compareLine;
                            dcGridTitleProperty.DCGridLines.RemoveAt(j);
                        }

                        if (removedLine.LineStatus != "ADD")
                        {
                            if (dcGridTitleProperty.DeletedItems == null)
                                dcGridTitleProperty.DeletedItems = new List<string>();
                            //
                            dcGridTitleProperty.DeletedItems.Add(removedLine.ID);
                        }
                    }
                }
            }
        }

        public static List<CommonCartGridLine> AddDCGridLinesToCartGridLines(List<DCGridLine> dcGridLines, string lineItemID)
        {
            var cartGridLines = new List<CommonCartGridLine>();
            // Change Order for fixing issue Order
            if (dcGridLines == null || dcGridLines.Count == 0)
                return null;
            var statusAdd = LineStatus.Add.ToString().ToUpper();
            int count = dcGridLines.Count;
            //for (var i = count - 1; i >= 0; i--)
            for (var i = 0; i < count; i++)
            {
                var dcGridLine = dcGridLines[i];

                if (dcGridLine.LineStatus == statusAdd)
                {
                    var dcGridFieldCodes = dcGridLine.DCGridFieldCodes;
                    var gridLineFieldCodeList = new List<CartGridLineFieldCode>();

                    // Reset new ID
                    dcGridLine.ID = Guid.NewGuid().ToString("b");

                    foreach (var dcGridFieldCode in dcGridFieldCodes)
                    {
                        var gridLineFieldCode = new CartGridLineFieldCode()
                        {
                            GridCodeId = dcGridFieldCode.GridCodeId,
                            GridFieldId = dcGridFieldCode.GridFieldId,
                            GridTextValue = dcGridFieldCode.GridCodeText,
                            GridCodeValue = dcGridFieldCode.GridCode,
                            IsAuthorized = (!string.IsNullOrEmpty(dcGridFieldCode.IsAuthorized) && dcGridFieldCode.IsAuthorized.ToUpper() == "TRUE"),
                            IsFreeText = (!string.IsNullOrEmpty(dcGridFieldCode.IsFreeText) && dcGridFieldCode.IsFreeText.ToUpper() == "TRUE"),
                            GridFieldType = ConvertToGridFieldType(dcGridFieldCode.GridFieldType)
                        };
                        gridLineFieldCodeList.Add(gridLineFieldCode);
                    }
                    var cartGridLine = new CommonCartGridLine()
                    {
                        CartGridLineId = dcGridLine.ID,
                        GridFieldCodeList = gridLineFieldCodeList,
                        LastModifiedByUserId = dcGridLine.LastModifiedByUserId,
                        Quantity =
                            string.IsNullOrEmpty(dcGridLine.Quantity)
                                ? 0
                                : int.Parse(dcGridLine.Quantity),
                        LineItemId = lineItemID,
                        Sequence = string.IsNullOrEmpty(dcGridLine.Sequence)
                                ? 0
                                : int.Parse(dcGridLine.Sequence)
                    };

                    cartGridLines.Add(cartGridLine);
                }
            }
            return cartGridLines;
        }

        public static int GetGridLineCount(string cartId, string lineItemId, bool isGridEnabled)
        {
            if (!isGridEnabled)
                return 0;

            var gridLineCount = 0;
            List<GridNoteCount> gridNoteCounts = null;

            if (!string.IsNullOrEmpty(lineItemId))
            {
                var lineItems = new List<string>() { lineItemId };
                gridNoteCounts = GridDAOManager.Instance.GetCountForLineItem(cartId, lineItems);// CartGridContext.Current.CartGridManager.GetCountForLineItem(cartId, lineItems);
            }

            if (gridNoteCounts != null && gridNoteCounts.Count > 0)
            {
                var gridNoteCount = gridNoteCounts.First();
                if (gridNoteCount != null)
                {
                    gridLineCount = gridNoteCount.GridLineCount;
                }
            }

            return gridLineCount;
        }

        public static async Task<List<NoteClientObject>> GetNotes(string cartId, string userId, List<string> btKeys, List<string> lineItemIds = null)
        {
            return await GridDAOManager.Instance.GetNotesByBTKeysAsync(cartId, userId, btKeys, lineItemIds);
        }

        private static GridFieldType ConvertToGridFieldType(string gridFieldType)
        {
            if (string.IsNullOrEmpty(gridFieldType)) return GridFieldType.Unknown;

            try
            {
                var result = (GridFieldType)Enum.Parse(typeof(GridFieldType), gridFieldType);
                return result;
            }
            catch
            {

            }
            return GridFieldType.Unknown;
        }

        private static bool IsDuplicated(DCGridLine gridLine1, DCGridLine gridLine2)
        {
            var isDuplicated = false;
            for (var i = 0; i < gridLine2.DCGridFieldCodes.Count; i++)
            {
                isDuplicated = (gridLine1.DCGridFieldCodes[i].GridCode == gridLine2.DCGridFieldCodes[i].GridCode)
                       && (gridLine1.DCGridFieldCodes[i].GridCodeId == gridLine2.DCGridFieldCodes[i].GridCodeId)
                       && (gridLine1.DCGridFieldCodes[i].GridCodeText == gridLine2.DCGridFieldCodes[i].GridCodeText)
                       && (gridLine1.DCGridFieldCodes[i].GridFieldId == gridLine2.DCGridFieldCodes[i].GridFieldId)
                       && (gridLine1.DCGridFieldCodes[i].GridFieldIdToName == gridLine2.DCGridFieldCodes[i].GridFieldIdToName);

                if (!isDuplicated)
                {
                    break;
                }
            }
            return isDuplicated;
        }

        private static bool CompareLastModifiedTime(string value, string comparedValue)
        {
            if (string.IsNullOrEmpty(value) && string.IsNullOrEmpty(comparedValue))
                return false;
            return String.CompareOrdinal(value, comparedValue) < 0;
        }
        public static List<DCGridLine> ConvertCartGridLinesToDCGridLines(List<CommonCartGridLine> cartGridLines)
        {
            var dcGridLines = new List<DCGridLine>();

            if (cartGridLines == null)
                return dcGridLines;

            foreach (var cartGridLine in cartGridLines)
            {
                var fieldCodeList = cartGridLine.GridFieldCodeList;
                var dcGridFieldCodes = new List<DCGridFieldCode>();

                foreach (var fieldCode in fieldCodeList)
                {
                    var dcGridFieldCode = new DCGridFieldCode()
                    {
                        GridCode = fieldCode.GridCodeValue,
                        GridCodeId = fieldCode.GridCodeId,
                        GridFieldId = fieldCode.GridFieldId,
                        GridCodeText = fieldCode.GridTextValue,
                        IsFreeText = fieldCode.IsFreeText.ToString(),
                        IsAuthorized = fieldCode.IsAuthorized.ToString(),
                        IsFutureDate = fieldCode.IsFutureDate.ToString(),
                        IsExpired = fieldCode.IsExpired.ToString(),
                        IsDisabled = fieldCode.IsDisabled.ToString(),
                        GridFieldType = fieldCode.GridFieldType.ToString()
                    };
                    dcGridFieldCodes.Add(dcGridFieldCode);
                }

                dcGridLines.Add(new DCGridLine()
                {
                    ID = cartGridLine.CartGridLineId,
                    Quantity = cartGridLine.Quantity.ToString(),
                    DCGridFieldCodes = dcGridFieldCodes,
                    LineStatus = LineStatus.View.ToString().ToUpper(),
                    Sequence = cartGridLine.Sequence.ToString(),
                    IsAuthorized = cartGridLine.IsAuthorized.ToString(),
                    LastModifiedByUserId = cartGridLine.LastModifiedByUserId,
                    LastModifiedByUserName = cartGridLine.LastModifiedByUserName
                });

            }

            return dcGridLines;
        }

    }
}
