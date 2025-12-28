namespace PromotionsExtract {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"ArrayOfPromotion", @"Promotion"})]
    public sealed class PromotionsService_schemas_datacontract_org_2004_07_BT_NextGen_Promotions : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" xmlns:tns=""http://schemas.datacontract.org/2004/07/BT.NextGen.Promotions"" elementFormDefault=""qualified"" targetNamespace=""http://schemas.datacontract.org/2004/07/BT.NextGen.Promotions"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:complexType name=""ArrayOfPromotion"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""Promotion"" nillable=""true"" type=""tns:Promotion"" />
    </xs:sequence>
  </xs:complexType>
  <xs:element name=""ArrayOfPromotion"" nillable=""true"" type=""tns:ArrayOfPromotion"" />
  <xs:complexType name=""Promotion"">
    <xs:sequence>
      <xs:element minOccurs=""0"" name=""PromoDescription"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""PromoEndDate"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""PromoName"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""PromoStartDate"" nillable=""true"" type=""xs:string"" />
      <xs:element minOccurs=""0"" name=""PromoType"" nillable=""true"" type=""xs:string"" />
    </xs:sequence>
  </xs:complexType>
  <xs:element name=""Promotion"" nillable=""true"" type=""tns:Promotion"" />
</xs:schema>";
        
        public PromotionsService_schemas_datacontract_org_2004_07_BT_NextGen_Promotions() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [2];
                _RootElements[0] = "ArrayOfPromotion";
                _RootElements[1] = "Promotion";
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
        
        [Schema(@"http://schemas.datacontract.org/2004/07/BT.NextGen.Promotions",@"ArrayOfPromotion")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"ArrayOfPromotion"})]
        public sealed class ArrayOfPromotion : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public ArrayOfPromotion() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "ArrayOfPromotion";
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
        
        [Schema(@"http://schemas.datacontract.org/2004/07/BT.NextGen.Promotions",@"Promotion")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"Promotion"})]
        public sealed class Promotion : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public Promotion() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "Promotion";
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
}
