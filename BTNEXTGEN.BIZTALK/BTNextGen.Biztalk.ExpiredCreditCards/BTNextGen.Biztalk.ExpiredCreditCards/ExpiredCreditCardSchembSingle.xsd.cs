namespace BTNextGen.Biztalk.ExpiredCreditCards {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"",@"CreditCardData")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"CreditCardData"})]
    public sealed class ExpiredCreditCardSchembSingle : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""CreditCardData"">
    <xs:complexType>
      <xs:sequence>
        <xs:element name=""id"" type=""xs:string"" />
        <xs:element name=""paymentgroupid"" type=""xs:string"" />
        <xs:element name=""ccnumber"" type=""xs:string"" />
        <xs:element name=""last4digits"" type=""xs:string"" />
        <xs:element name=""billingaddress"" type=""xs:string"" />
        <xs:element name=""expirationmonth"" type=""xs:string"" />
        <xs:element name=""expirationyear"" type=""xs:string"" />
        <xs:element name=""cardcontactuser"" type=""xs:string"" />
        <xs:element name=""cardtoken"" type=""xs:string"" />
        <xs:element name=""paymentstatus"" type=""xs:string"" />
        <xs:element name=""notificationstatus"" type=""xs:string"" />
        <xs:element name=""datelastfailednotification"" type=""xs:string"" />
        <xs:element name=""datelastpreemptivenotification"" type=""xs:string"" />
        <xs:element name=""alias"" type=""xs:string"" />
        <xs:element name=""primaryindicator"" type=""xs:string"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public ExpiredCreditCardSchembSingle() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "CreditCardData";
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
