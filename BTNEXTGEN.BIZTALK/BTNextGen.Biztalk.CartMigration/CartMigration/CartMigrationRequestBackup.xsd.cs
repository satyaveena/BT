namespace CartMigration {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"http://CartMigration.CartMigrationRequestBackup",@"CartMigrationRequestBackup")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"CartMigrationRequestBackup"})]
    public sealed class CartMigrationRequestBackup : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns=""http://CartMigration.CartMigrationRequestBackup"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" targetNamespace=""http://CartMigration.CartMigrationRequestBackup"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""CartMigrationRequestBackup"">
    <xs:complexType>
      <xs:sequence>
        <xs:element minOccurs=""0"" name=""BasketList"">
          <xs:complexType>
            <xs:sequence>
              <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""BasketDetails"">
                <xs:complexType>
                  <xs:sequence>
                    <xs:element minOccurs=""0"" name=""AcctInventoryReserveNumber"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""AcctInventoryType"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""AcctWarehouseList"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""BasketCreateDate"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""BasketIsBTCart"" type=""xs:boolean"" />
                    <xs:element minOccurs=""0"" name=""BasketIsTolas"" type=""xs:boolean"" />
                    <xs:element minOccurs=""0"" name=""BasketModifyDate"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""BasketNote"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""BasketPONumber"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""BasketSpecialInstructions"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""BasketStatus"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""BasketGiftFee"" type=""xs:decimal"" />
                    <xs:element minOccurs=""0"" name=""BasketOrderFee"" type=""xs:decimal"" />
                    <xs:element minOccurs=""0"" name=""BasketProcessingFee"" type=""xs:decimal"" />
                    <xs:element minOccurs=""0"" name=""BasketShipFee"" type=""xs:decimal"" />
                    <xs:element minOccurs=""0"" name=""BasketTotal"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""BasketBookID"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""BasketEntID"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""BasketUser"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""BasketHomeDeliveryAcct"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""BasketHomeDeliveryAcctType"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""IsHomeDeliveryIndicator"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""LegacyBasketID"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""LegacyBasketName"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" name=""LegacySourceSystem"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""DetailLists"">
                      <xs:complexType>
                        <xs:sequence>
                          <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""BasketItemDetail"">
                            <xs:complexType>
                              <xs:sequence>
                                <xs:element minOccurs=""0"" name=""BTBib"" type=""xs:string"" />
                                <xs:element minOccurs=""0"" name=""BTGwcode"" type=""xs:string"" />
                                <xs:element minOccurs=""0"" name=""BTGwmsg"" type=""xs:string"" />
                                <xs:element minOccurs=""0"" name=""BTItemType"" type=""xs:string"" />
                                <xs:element minOccurs=""0"" name=""BTkey"" type=""xs:string"" />
                                <xs:element minOccurs=""0"" name=""BTTitle"" type=""xs:string"" />
                                <xs:element minOccurs=""0"" name=""BTEdition"" type=""xs:string"" />
                                <xs:element minOccurs=""0"" name=""BTVolume"" type=""xs:string"" />
                                <xs:element minOccurs=""0"" name=""DETLegacyBasketID"" type=""xs:string"" />
                                <xs:element minOccurs=""0"" name=""LegacyCreateDate"" type=""xs:string"" />
                                <xs:element minOccurs=""0"" name=""LegacyLineItemID"" type=""xs:string"" />
                                <xs:element minOccurs=""0"" name=""LegacyModifyDate"" type=""xs:string"" />
                                <xs:element minOccurs=""0"" name=""LegacyPONumber"" type=""xs:string"" />
                                <xs:element minOccurs=""0"" name=""ListPrice"" type=""xs:string"" />
                                <xs:element minOccurs=""0"" name=""PlacePrice"" type=""xs:string"" />
                                <xs:element minOccurs=""0"" name=""ProductCatalog"" type=""xs:string"" />
                                <xs:element minOccurs=""0"" name=""ProductType"" type=""xs:string"" />
                                <xs:element minOccurs=""0"" name=""Qty"" type=""xs:string"" />
                              </xs:sequence>
                            </xs:complexType>
                          </xs:element>
                        </xs:sequence>
                      </xs:complexType>
                    </xs:element>
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public CartMigrationRequestBackup() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "CartMigrationRequestBackup";
                return _RootElements;
            }
        }
        
        protected override object RawSchema {
            get {
                return _rawSchema;
            }
            set {
                _rawSchema = value;
            }
        }
    }
}
