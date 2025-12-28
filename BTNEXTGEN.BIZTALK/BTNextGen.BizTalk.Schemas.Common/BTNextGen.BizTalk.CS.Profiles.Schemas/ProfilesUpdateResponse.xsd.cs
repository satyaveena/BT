namespace BTNextGen.BizTalk.CS.Profiles.Schemas {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"",@"CommerceServerProfilesUpdateResponse")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"CommerceServerProfilesUpdateResponse"})]
    public sealed class ProfilesUpdateResponse : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" attributeFormDefault=""unqualified"" elementFormDefault=""qualified"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""CommerceServerProfilesUpdateResponse"">
    <xs:complexType>
      <xs:sequence>
        <xs:element name=""InvalidProfileUpdateMessage"">
          <xs:complexType />
        </xs:element>
        <xs:element name=""Address"">
          <xs:complexType>
            <xs:attribute name=""GeneralInfo.address_id"" type=""xs:string"" use=""required"" />
          </xs:complexType>
        </xs:element>
        <xs:element name=""BlanketPO"">
          <xs:complexType>
            <xs:attribute name=""GeneralInfo.po_id"" type=""xs:string"" use=""required"" />
          </xs:complexType>
        </xs:element>
        <xs:element name=""CreditCard"">
          <xs:complexType>
            <xs:attribute name=""GeneralInfo.id"" type=""xs:string"" use=""required"" />
          </xs:complexType>
        </xs:element>
        <xs:element name=""Currency"">
          <xs:complexType>
            <xs:attribute name=""GeneralInfo.currency_code"" type=""xs:string"" use=""required"" />
          </xs:complexType>
        </xs:element>
        <xs:element name=""Organization"">
          <xs:complexType>
            <xs:attribute name=""GeneralInfo.org_id"" type=""xs:string"" use=""required"" />
          </xs:complexType>
        </xs:element>
        <xs:element name=""UserObject"">
          <xs:complexType>
            <xs:attribute name=""GeneralInfo.user_id"" type=""xs:string"" use=""required"" />
          </xs:complexType>
        </xs:element>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public ProfilesUpdateResponse() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "CommerceServerProfilesUpdateResponse";
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
