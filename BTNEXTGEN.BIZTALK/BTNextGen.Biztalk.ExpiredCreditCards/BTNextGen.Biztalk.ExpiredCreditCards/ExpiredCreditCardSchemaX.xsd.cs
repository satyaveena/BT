namespace BTNextGen.Biztalk.ExpiredCreditCards {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"http://BTNextGen.Biztalk.ExpiredCreditCards.ExpiredCreditCardSchema",@"ExpiredCreditCardsList")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"ExpiredCreditCardsList"})]
    public sealed class ExpiredCreditCardSchemaX : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns=""http://BTNextGen.Biztalk.ExpiredCreditCards.ExpiredCreditCardSchemaX"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" targetNamespace=""http://BTNextGen.Biztalk.ExpiredCreditCards.ExpiredCreditCardSchema"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""ExpiredCreditCardsList"">
    <xs:complexType>
      <xs:sequence>
        <xs:element maxOccurs=""unbounded"" name=""CreditCardsDoc"">
          <xs:complexType>
            <xs:sequence>
              <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""CreditCardData"">
                <xs:complexType>
                  <xs:sequence>
                    <xs:element name=""id"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" maxOccurs=""1"" name=""paymentgroupid"" type=""xs:string"" />
                    <xs:element name=""ccnumber"" type=""xs:string"" />
                    <xs:element name=""last4digits"" type=""xs:string"" />
                    <xs:element name=""billingaddress"" type=""xs:string"" />
                    <xs:element name=""expirationmonth"" type=""xs:string"" />
                    <xs:element name=""expirationyear"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" maxOccurs=""1"" name=""cardcontactuser"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" maxOccurs=""1"" name=""cardtoken"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" maxOccurs=""1"" name=""paymentstatus"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" maxOccurs=""1"" name=""notificationstatus"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" maxOccurs=""1"" name=""datelastfailednotification"" type=""xs:string"" />
                    <xs:element minOccurs=""0"" maxOccurs=""1"" name=""datelastpreemptivenotification"" type=""xs:string"" />
                    <xs:element name=""alias"" type=""xs:string"" />
                    <xs:element name=""primaryindicator"" type=""xs:string"" />
                    <xs:element name=""firstname"" type=""xs:string"" />
                    <xs:element name=""lastname"" type=""xs:string"" />
                    <xs:element name=""emailaddress"" type=""xs:string"" />
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
        
        public ExpiredCreditCardSchemaX() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "ExpiredCreditCardsList";
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
