namespace CartMigration {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"BTNextGen.Biztalk.CartsMigration",@"Carts")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"Carts"})]
    public sealed class xml_cartmigration_sp : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" attributeFormDefault=""unqualified"" elementFormDefault=""qualified"" targetNamespace=""BTNextGen.Biztalk.CartsMigration"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""Carts"">
    <xs:complexType>
      <xs:sequence maxOccurs=""unbounded"">
        <xs:element name=""CartListing"">
          <xs:complexType>
            <xs:sequence maxOccurs=""unbounded"">
              <xs:element name=""Cart"">
                <xs:complexType>
                  <xs:sequence>
                    <xs:element name=""CartID"" type=""xs:unsignedInt"" />
                    <xs:element name=""CSUserID"" type=""xs:unsignedShort"" />
                    <xs:element name=""BasketStatus"" type=""xs:string"" />
                    <xs:element name=""BasketTotal"" type=""xs:decimal"" />
                    <xs:element name=""BasketCreatedDate"" type=""xs:dateTime"" />
                    <xs:element name=""BasketModifiedDate"" type=""xs:dateTime"" />
                    <xs:element name=""LegacyEntertainmentAccountId"" type=""xs:unsignedShort"" />
                    <xs:element name=""LegacyBookAccountId"" type=""xs:unsignedShort"" />
                    <xs:element name=""HomeDeliveryAddressType"" type=""xs:anyType"" />
                    <xs:element name=""BasketPONumber"" type=""xs:string"" />
                    <xs:element name=""BasketSpecialInstructions"" type=""xs:anyType"" />
                    <xs:element name=""BasketStoreShippingFee"" type=""xs:decimal"" />
                    <xs:element name=""BasketStoreGiftWrapFee"" type=""xs:decimal"" />
                    <xs:element name=""BasketStoreProcessingFee"" type=""xs:decimal"" />
                    <xs:element name=""BasketStoreOrderFee"" type=""xs:decimal"" />
                    <xs:element name=""AccountWarehouseList"" type=""xs:anyType"" />
                    <xs:element name=""BasketIsBTCart"" type=""xs:anyType"" />
                    <xs:element name=""BasketNote"" type=""xs:anyType"" />
                    <xs:element name=""AccountInventoryReserveNumber"" type=""xs:anyType"" />
                    <xs:element name=""AccountInventoryType"" type=""xs:anyType"" />
                    <xs:element name=""LegacyBasketId"" type=""xs:unsignedInt"" />
                    <xs:element name=""LegacySourceSystem"" type=""xs:string"" />
                    <xs:element name=""LegacyBasketName"" type=""xs:string"" />
                    <xs:element name=""IsHomeDeliveryIndicator"" type=""xs:string"" />
                    <xs:element name=""HomeDeliveryAccountId"" type=""xs:unsignedByte"" />
                    <xs:element name=""BasketIsTolas"" type=""xs:unsignedByte"" />
                    <xs:element name=""BasketIsSOP"" type=""xs:unsignedByte"" />
                    <xs:element name=""CartDetails"">
                      <xs:complexType>
                        <xs:sequence>
                          <xs:element maxOccurs=""unbounded"" name=""CartDetail"">
                            <xs:complexType>
                              <xs:sequence>
                                <xs:element name=""CartHdrID"" type=""xs:unsignedInt"" />
                                <xs:element name=""CartLnNum"" nillable=""true"" type=""xs:unsignedByte"" />
                                <xs:element name=""Isbn"" type=""xs:unsignedLong"" />
                                <xs:element name=""ProductCatalog"" type=""xs:anyType"" />
                                <xs:element name=""Btkey"" type=""xs:string"" />
                                <xs:element name=""Quantity"" type=""xs:unsignedByte"" />
                                <xs:element name=""ListPrice"" type=""xs:decimal"" />
                                <xs:element name=""PlacedPrice"" type=""xs:decimal"" />
                                <xs:element name=""BTGiftWrapCode"" type=""xs:anyType"" />
                                <xs:element name=""BTGiftWrapMessage"" type=""xs:anyType"" />
                                <xs:element name=""BTItemType"" type=""xs:anyType"" />
                                <xs:element name=""BTBibNumber"" type=""xs:anyType"" />
                                <xs:element name=""LinePONumber"" type=""xs:anyType"" />
                                <xs:element name=""LegacyLineItmId"" type=""xs:unsignedByte"" />
                                <xs:element name=""LegacyBasketId"" type=""xs:unsignedInt"" />
                                <xs:element name=""LegacyCreatedDate"" type=""xs:anyType"" />
                                <xs:element name=""LegacyModifiedDate"" type=""xs:dateTime"" />
                                <xs:element name=""BTLineNotes"" type=""xs:string"" />
                                <xs:element name=""BTShippedQuantity"" type=""xs:unsignedByte"" />
                                <xs:element name=""BTCancelledQuantity"" type=""xs:unsignedByte"" />
                                <xs:element name=""BTBackOrderedQuantity"" type=""xs:unsignedByte"" />
                                <xs:element name=""BTTitle"" type=""xs:string"" />
                                <xs:element name=""BTVolumSet"" type=""xs:unsignedByte"" />
                                <xs:element name=""BTTitleEditionVersion"" type=""xs:unsignedByte"" />
                                <xs:element name=""ProductType"" type=""xs:string"" />
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
        
        public xml_cartmigration_sp() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "Carts";
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
