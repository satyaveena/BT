using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TS360Constants
{
    public class HTMLTemplate
    {
        public const string InventoryItemHTMLTemplate = @"<div class='{OddRow}'>
                <div style='padding-left: 7px;' class='col1 fl col' title='{WarehouseToolTip}'>
                    {WareHouse}
                </div>
                <div style='white-space: nowrap; overflow: hidden;text-align: right;' title='{OnHandInventory}' class='col2 fl col'>
                   {OnHandInventory}
                </div>
                <div class='col3 fl col' style='text-align: right;'>
                     {OnOrderQuantity}
                </div>
                <div class='cb'>
                    <!-- -->
                </div>
            </div>";
    }
}
