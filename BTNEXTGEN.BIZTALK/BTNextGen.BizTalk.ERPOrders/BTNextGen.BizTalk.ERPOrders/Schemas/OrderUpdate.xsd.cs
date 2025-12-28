namespace BTNextGen.BizTalk.ERPOrders.Schemas {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"",@"PurchaseOrderUpdates")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"PurchaseOrderUpdates"})]
    public sealed class OrderUpdate : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" elementFormDefault=""qualified"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""PurchaseOrderUpdates"" type=""PurchaseOrderUpdates"" />
  <xs:complexType name=""PurchaseOrderUpdates"">
    <xs:choice minOccurs=""0"" maxOccurs=""unbounded"">
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""PurchaseOrderEx"" type=""PurchaseOrder"" />
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""OrderAddress"" type=""OrderAddress"" />
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""OrderFormEx"" type=""OrderForm"" />
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""LineItemEx"" type=""LineItem"" />
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""DiscountApplicationRecord"" type=""DiscountApplicationRecord"" />
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""CashCardPayment"" type=""CashCardPayment"" />
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""CreditCardPayment"" type=""CreditCardPayment"" />
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""GiftCertificatePayment"" type=""GiftCertificatePayment"" />
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""PurchaseOrderPayment"" type=""PurchaseOrderPayment"" />
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""PromoCodeRecord"" type=""PromoCodeRecord"" />
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""Shipment"" type=""Shipment"" />
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""ShippingDiscountRecord"" type=""ShippingDiscountRecord"" />
    </xs:choice>
  </xs:complexType>
  <xs:complexType name=""PurchaseOrder"">
    <xs:attribute name=""OrderGroupId"" type=""Guid"" />
    <xs:attribute name=""PropertyName"" type=""xs:string"" />
    <xs:attribute name=""PropertyValue"" type=""xs:string"" />
  </xs:complexType>
  <xs:complexType name=""OrderAddress"">
    <xs:attribute name=""OrderAddressId"" type=""xs:string"" />
    <xs:attribute name=""OrderGroupId"" type=""Guid"" />
    <xs:attribute name=""PropertyName"" type=""xs:string"" />
    <xs:attribute name=""PropertyValue"" type=""xs:string"" />
  </xs:complexType>
  <xs:complexType name=""OrderForm"">
    <xs:attribute name=""OrderFormId"" type=""Guid"" />
    <xs:attribute name=""PropertyName"" type=""xs:string"" />
    <xs:attribute name=""PropertyValue"" type=""xs:string"" />
  </xs:complexType>
  <xs:complexType name=""LineItem"">
    <xs:attribute name=""LineItemId"" type=""Guid"" />
    <xs:attribute name=""PropertyName"" type=""xs:string"" />
    <xs:attribute name=""PropertyValue"" type=""xs:string"" />
  </xs:complexType>
  <xs:complexType name=""DiscountApplicationRecord"">
    <xs:attribute name=""DiscountId"" type=""xs:int"" />
    <xs:attribute name=""LineItemId"" type=""Guid"" />
    <xs:attribute name=""PropertyName"" type=""xs:string"" />
    <xs:attribute name=""PropertyValue"" type=""xs:string"" />
  </xs:complexType>
  <xs:complexType name=""CashCardPayment"">
    <xs:attribute name=""PaymentId"" type=""Guid"" />
    <xs:attribute name=""PropertyName"" type=""xs:string"" />
    <xs:attribute name=""PropertyValue"" type=""xs:string"" />
  </xs:complexType>
  <xs:complexType name=""CreditCardPayment"">
    <xs:attribute name=""PaymentId"" type=""Guid"" />
    <xs:attribute name=""PropertyName"" type=""xs:string"" />
    <xs:attribute name=""PropertyValue"" type=""xs:string"" />
  </xs:complexType>
  <xs:complexType name=""GiftCertificatePayment"">
    <xs:attribute name=""PaymentId"" type=""Guid"" />
    <xs:attribute name=""PropertyName"" type=""xs:string"" />
    <xs:attribute name=""PropertyValue"" type=""xs:string"" />
  </xs:complexType>
  <xs:complexType name=""PurchaseOrderPayment"">
    <xs:attribute name=""PaymentId"" type=""Guid"" />
    <xs:attribute name=""PropertyName"" type=""xs:string"" />
    <xs:attribute name=""PropertyValue"" type=""xs:string"" />
  </xs:complexType>
  <xs:complexType name=""PromoCodeRecord"">
    <xs:attribute name=""OrderFormId"" type=""Guid"" />
    <xs:attribute name=""PromoCode"" type=""xs:string"" />
    <xs:attribute name=""PropertyName"" type=""xs:string"" />
    <xs:attribute name=""PropertyValue"" type=""xs:string"" />
  </xs:complexType>
  <xs:complexType name=""Shipment"">
    <xs:attribute name=""ShipmentId"" type=""Guid"" />
    <xs:attribute name=""PropertyName"" type=""xs:string"" />
    <xs:attribute name=""PropertyValue"" type=""xs:string"" />
  </xs:complexType>
  <xs:complexType name=""ShippingDiscountRecord"">
    <xs:attribute name=""DiscountId"" type=""xs:int"" />
    <xs:attribute name=""ShipmentId"" type=""Guid"" />
    <xs:attribute name=""PropertyName"" type=""xs:string"" />
    <xs:attribute name=""PropertyValue"" type=""xs:string"" />
  </xs:complexType>
  <xs:simpleType name=""Guid"">
    <xs:restriction base=""xs:string"">
      <xs:pattern value=""[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}"" />
    </xs:restriction>
  </xs:simpleType>
</xs:schema>";
        
        public OrderUpdate() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "PurchaseOrderUpdates";
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
