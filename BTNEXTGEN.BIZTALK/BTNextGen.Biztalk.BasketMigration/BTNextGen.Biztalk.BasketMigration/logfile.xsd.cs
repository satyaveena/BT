namespace BTNextGen.Biztalk.BasketMigration {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"",@"NewDataSet")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"NewDataSet"})]
    public sealed class logfile : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:msdata=""urn:schemas-microsoft-com:xml-msdata"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" id=""NewDataSet"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element msdata:IsDataSet=""true"" msdata:UseCurrentLocale=""true"" name=""NewDataSet"">
    <xs:complexType>
      <xs:choice minOccurs=""0"" maxOccurs=""unbounded"">
        <xs:element name=""Basket"">
          <xs:complexType>
            <xs:sequence>
              <xs:element minOccurs=""0"" name=""Status"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""SoldToId"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""TaxTotal"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""LastModified"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""Created"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""StatusCode"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""SubTotal"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""IsDirty"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""LineItemCount"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""HandlingTotal"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""BasketID"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""ShippingTotal"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""ModifiedBy"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""SoldToName"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""Total"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""OrderGroupId"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""Name"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""BillingCurrency"" type=""xs:string"" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element name=""OrderForms"">
          <xs:complexType>
            <xs:sequence>
              <xs:element minOccurs=""0"" name=""Status"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""OrderGroupId"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""TaxTotal"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""LastModified"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""Created"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""PromoUserIdentity"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""SubTotal"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""HandlingTotal"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""ShippingTotal"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""ModifiedBy"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""BillingAddressId"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""Total"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""OrderFormId"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""Name"" type=""xs:string"" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element name=""LineItems"">
          <xs:complexType>
            <xs:sequence>
              <xs:element minOccurs=""0"" name=""ShippingMethodName"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""Created"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""ProductId"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""PlacedPrice"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""Description"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""InventoryCondition"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""OrderFormId"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""ShippingAddressId"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""AllowBackordersAndPreorders"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""LastModified"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""OrderGroupId"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""ListPrice"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""Quantity"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""DisplayName"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""ModifiedBy"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""ProductCatalog"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""ShippingMethodId"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""OrderLevelDiscountAmount"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""PreorderQuantity"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""ExtendedPrice"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""Status"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""BackorderQuantity"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""LineItemDiscountAmount"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""InStockQuantity"" type=""xs:string"" />
              <xs:element minOccurs=""0"" name=""LineItemId"" type=""xs:string"" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
      </xs:choice>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public logfile() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "NewDataSet";
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
