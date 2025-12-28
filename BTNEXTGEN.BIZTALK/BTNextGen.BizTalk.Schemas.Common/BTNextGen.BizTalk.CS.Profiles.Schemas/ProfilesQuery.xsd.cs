namespace BTNextGen.BizTalk.CS.Profiles.Schemas {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"",@"CommerceServerProfilesQuery")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"CommerceServerProfilesQuery"})]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.BizTalk.CS.Profiles.Schemas.ProfilesQueryExpr", typeof(global::BTNextGen.BizTalk.CS.Profiles.Schemas.ProfilesQueryExpr))]
    public sealed class ProfilesQuery : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" attributeFormDefault=""unqualified"" elementFormDefault=""qualified"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:import schemaLocation=""BTNextGen.BizTalk.CS.Profiles.Schemas.ProfilesQueryExpr"" namespace=""http://schemas.microsoft.com/CommerceServer/2004/02/Expressions"" />
  <xs:annotation>
    <xs:appinfo>
      <b:references>
        <b:reference targetNamespace=""http://schemas.microsoft.com/CommerceServer/2004/02/Expressions"" />
      </b:references>
    </xs:appinfo>
  </xs:annotation>
  <xs:element name=""CommerceServerProfilesQuery"">
    <xs:complexType>
      <xs:sequence>
        <xs:element xmlns:q1=""http://schemas.microsoft.com/CommerceServer/2004/02/Expressions"" ref=""q1:CLAUSE"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public ProfilesQuery() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "CommerceServerProfilesQuery";
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
