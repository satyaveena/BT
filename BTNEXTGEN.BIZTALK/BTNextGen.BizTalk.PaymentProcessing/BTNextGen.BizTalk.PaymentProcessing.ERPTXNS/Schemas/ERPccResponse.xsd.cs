namespace BTNextGen.BizTalk.PaymentProcessing.Schemas {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"BTNextGen.BizTalk.PaymentProcessing.ERPTXNs.ERPccResponse",@"ERPccResponseRoot")]
    [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.String), "merchantReferenceCode.merchantReferenceCode_Value", XPath = @"/*[local-name()='ERPccResponseRoot' and namespace-uri()='BTNextGen.BizTalk.PaymentProcessing.ERPTXNs.ERPccResponse']/*[local-name()='merchantReferenceCode' and namespace-uri()='']/*[local-name()='merchantReferenceCode_Value' and namespace-uri()='']", XsdType = @"string")]
    [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.String), "ReplyType.ReplyType_Value", XPath = @"/*[local-name()='ERPccResponseRoot' and namespace-uri()='BTNextGen.BizTalk.PaymentProcessing.ERPTXNs.ERPccResponse']/*[local-name()='ReplyType' and namespace-uri()='']/*[local-name()='ReplyType_Value' and namespace-uri()='']", XsdType = @"string")]
    [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.String), "reasonCode.reasonCode_Value", XPath = @"/*[local-name()='ERPccResponseRoot' and namespace-uri()='BTNextGen.BizTalk.PaymentProcessing.ERPTXNs.ERPccResponse']/*[local-name()='reasonCode' and namespace-uri()='']/*[local-name()='reasonCode_Value' and namespace-uri()='']", XsdType = @"string")]
    [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.String), "Reply_statusMessage.Reply_StatusMessage_Value", XPath = @"/*[local-name()='ERPccResponseRoot' and namespace-uri()='BTNextGen.BizTalk.PaymentProcessing.ERPTXNs.ERPccResponse']/*[local-name()='Reply_statusMessage' and namespace-uri()='']/*[local-name()='Reply_StatusMessage_Value' and namespace-uri()='']", XsdType = @"string")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"ERPccResponseRoot"})]
    public sealed class ERPccResponse : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns=""BTNextGen.BizTalk.PaymentProcessing.ERPTXNs.ERPccResponse"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" targetNamespace=""BTNextGen.BizTalk.PaymentProcessing.ERPTXNs.ERPccResponse"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:annotation>
    <xs:appinfo>
      <schemaEditorExtension:schemaInfo namespaceAlias=""b"" extensionClass=""Microsoft.BizTalk.FlatFileExtension.FlatFileExtension"" standardName=""Flat File"" xmlns:schemaEditorExtension=""http://schemas.microsoft.com/BizTalk/2003/SchemaEditorExtensions"" />
      <b:schemaInfo standard=""Flat File"" codepage=""65001"" default_pad_char="" "" pad_char_type=""char"" count_positions_by_byte=""false"" parser_optimization=""speed"" lookahead_depth=""3"" suppress_empty_nodes=""false"" generate_empty_nodes=""true"" allow_early_termination=""false"" early_terminate_optional_fields=""false"" allow_message_breakup_of_infix_root=""false"" compile_parse_tables=""false"" root_reference=""ERPccResponseRoot"" />
    </xs:appinfo>
  </xs:annotation>
  <xs:element name=""ERPccResponseRoot"">
    <xs:annotation>
      <xs:appinfo>
        <b:recordInfo structure=""delimited"" child_delimiter_type=""hex"" child_delimiter=""0xD 0xA"" child_order=""postfix"" sequence_number=""1"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" />
        <b:properties>
          <b:property distinguished=""true"" xpath=""/*[local-name()='ERPccResponseRoot' and namespace-uri()='BTNextGen.BizTalk.PaymentProcessing.ERPTXNs.ERPccResponse']/*[local-name()='merchantReferenceCode' and namespace-uri()='']/*[local-name()='merchantReferenceCode_Value' and namespace-uri()='']"" />
          <b:property distinguished=""true"" xpath=""/*[local-name()='ERPccResponseRoot' and namespace-uri()='BTNextGen.BizTalk.PaymentProcessing.ERPTXNs.ERPccResponse']/*[local-name()='ReplyType' and namespace-uri()='']/*[local-name()='ReplyType_Value' and namespace-uri()='']"" />
          <b:property distinguished=""true"" xpath=""/*[local-name()='ERPccResponseRoot' and namespace-uri()='BTNextGen.BizTalk.PaymentProcessing.ERPTXNs.ERPccResponse']/*[local-name()='reasonCode' and namespace-uri()='']/*[local-name()='reasonCode_Value' and namespace-uri()='']"" />
          <b:property distinguished=""true"" xpath=""/*[local-name()='ERPccResponseRoot' and namespace-uri()='BTNextGen.BizTalk.PaymentProcessing.ERPTXNs.ERPccResponse']/*[local-name()='Reply_statusMessage' and namespace-uri()='']/*[local-name()='Reply_StatusMessage_Value' and namespace-uri()='']"" />
        </b:properties>
      </xs:appinfo>
    </xs:annotation>
    <xs:complexType>
      <xs:sequence>
        <xs:annotation>
          <xs:appinfo>
            <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
          </xs:appinfo>
        </xs:annotation>
        <xs:element name=""ReplyType"">
          <xs:annotation>
            <xs:appinfo>
              <b:recordInfo tag_name=""Reply_type"" structure=""delimited"" child_delimiter_type=""char"" child_delimiter=""="" child_order=""prefix"" sequence_number=""1"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" />
            </xs:appinfo>
          </xs:annotation>
          <xs:complexType>
            <xs:sequence>
              <xs:annotation>
                <xs:appinfo>
                  <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                </xs:appinfo>
              </xs:annotation>
              <xs:element default=""     "" name=""ReplyType_Value"" type=""xs:string"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:fieldInfo justification=""left"" sequence_number=""1"" min_length_with_pad=""1"" pad_char_type=""hex"" pad_char=""0x20"" />
                  </xs:appinfo>
                </xs:annotation>
              </xs:element>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element name=""requestID"">
          <xs:annotation>
            <xs:appinfo>
              <b:recordInfo tag_name=""requestID"" structure=""delimited"" child_delimiter_type=""char"" child_delimiter=""="" child_order=""prefix"" sequence_number=""2"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" />
            </xs:appinfo>
          </xs:annotation>
          <xs:complexType>
            <xs:sequence>
              <xs:annotation>
                <xs:appinfo>
                  <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                </xs:appinfo>
              </xs:annotation>
              <xs:element name=""requestID_Value"" type=""xs:string"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:fieldInfo justification=""left"" sequence_number=""1"" pad_char_type=""hex"" pad_char=""0x20"" min_length_with_pad=""1"" />
                  </xs:appinfo>
                </xs:annotation>
              </xs:element>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element name=""requestToken"">
          <xs:annotation>
            <xs:appinfo>
              <b:recordInfo tag_name=""requestToken"" structure=""delimited"" child_delimiter_type=""char"" child_delimiter=""="" child_order=""prefix"" sequence_number=""3"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" />
            </xs:appinfo>
          </xs:annotation>
          <xs:complexType>
            <xs:sequence>
              <xs:annotation>
                <xs:appinfo>
                  <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                </xs:appinfo>
              </xs:annotation>
              <xs:element name=""requestToken_Value"" type=""xs:string"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:fieldInfo justification=""left"" sequence_number=""1"" pad_char_type=""hex"" pad_char=""0x20"" min_length_with_pad=""1"" />
                  </xs:appinfo>
                </xs:annotation>
              </xs:element>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element name=""decision"">
          <xs:annotation>
            <xs:appinfo>
              <b:recordInfo tag_name=""decision"" structure=""delimited"" child_delimiter_type=""char"" child_delimiter=""="" child_order=""prefix"" sequence_number=""4"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" />
            </xs:appinfo>
          </xs:annotation>
          <xs:complexType>
            <xs:sequence>
              <xs:annotation>
                <xs:appinfo>
                  <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                </xs:appinfo>
              </xs:annotation>
              <xs:element name=""decision_Value"" type=""xs:string"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:fieldInfo justification=""left"" sequence_number=""1"" pad_char_type=""hex"" pad_char=""0x20"" min_length_with_pad=""1"" />
                  </xs:appinfo>
                </xs:annotation>
              </xs:element>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element name=""reasonCode"">
          <xs:annotation>
            <xs:appinfo>
              <b:recordInfo tag_name=""reasonCode"" structure=""delimited"" child_delimiter_type=""char"" child_delimiter=""="" child_order=""prefix"" sequence_number=""5"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" />
            </xs:appinfo>
          </xs:annotation>
          <xs:complexType>
            <xs:sequence>
              <xs:annotation>
                <xs:appinfo>
                  <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                </xs:appinfo>
              </xs:annotation>
              <xs:element name=""reasonCode_Value"" type=""xs:string"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:fieldInfo justification=""left"" sequence_number=""1"" pad_char_type=""hex"" min_length_with_pad=""1"" pad_char=""0x20"" />
                  </xs:appinfo>
                </xs:annotation>
              </xs:element>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element name=""merchantReferenceCode"">
          <xs:annotation>
            <xs:appinfo>
              <b:recordInfo tag_name=""merchantReferenceCode"" structure=""delimited"" child_delimiter_type=""char"" child_delimiter=""="" child_order=""prefix"" sequence_number=""6"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" />
            </xs:appinfo>
          </xs:annotation>
          <xs:complexType>
            <xs:sequence>
              <xs:annotation>
                <xs:appinfo>
                  <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                </xs:appinfo>
              </xs:annotation>
              <xs:element name=""merchantReferenceCode_Value"" type=""xs:string"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:fieldInfo justification=""left"" sequence_number=""1"" pad_char_type=""hex"" min_length_with_pad=""1"" pad_char=""0x20"" />
                  </xs:appinfo>
                </xs:annotation>
              </xs:element>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element name=""purchaseTotals_currency"">
          <xs:annotation>
            <xs:appinfo>
              <b:recordInfo tag_name=""purchaseTotals_currency"" structure=""delimited"" child_delimiter_type=""char"" child_delimiter=""="" child_order=""prefix"" sequence_number=""7"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" />
            </xs:appinfo>
          </xs:annotation>
          <xs:complexType>
            <xs:sequence>
              <xs:annotation>
                <xs:appinfo>
                  <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                </xs:appinfo>
              </xs:annotation>
              <xs:element name=""purchaseTotals_currency_Value"" type=""xs:string"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:fieldInfo justification=""left"" sequence_number=""1"" pad_char_type=""hex"" min_length_with_pad=""1"" pad_char=""0x20"" />
                  </xs:appinfo>
                </xs:annotation>
              </xs:element>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element name=""Reply_reasoncode"">
          <xs:annotation>
            <xs:appinfo>
              <b:recordInfo tag_name=""Reply_reasoncode"" structure=""delimited"" child_delimiter_type=""char"" child_delimiter=""="" child_order=""prefix"" sequence_number=""8"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" />
            </xs:appinfo>
          </xs:annotation>
          <xs:complexType>
            <xs:sequence>
              <xs:annotation>
                <xs:appinfo>
                  <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                </xs:appinfo>
              </xs:annotation>
              <xs:element name=""Reply_reasoncode_Value"" type=""xs:string"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:fieldInfo justification=""left"" sequence_number=""1"" pad_char_type=""hex"" min_length_with_pad=""1"" pad_char=""0x20"" />
                  </xs:appinfo>
                </xs:annotation>
              </xs:element>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element name=""Reply_amount"">
          <xs:annotation>
            <xs:appinfo>
              <b:recordInfo tag_name=""Reply_amount"" structure=""delimited"" child_delimiter_type=""char"" child_delimiter=""="" child_order=""prefix"" sequence_number=""9"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" />
            </xs:appinfo>
          </xs:annotation>
          <xs:complexType>
            <xs:sequence>
              <xs:annotation>
                <xs:appinfo>
                  <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                </xs:appinfo>
              </xs:annotation>
              <xs:element name=""Reply_amount_Value"" type=""xs:string"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:fieldInfo justification=""left"" sequence_number=""1"" pad_char_type=""hex"" min_length_with_pad=""1"" pad_char=""0x20"" />
                  </xs:appinfo>
                </xs:annotation>
              </xs:element>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element name=""Reply_authorizedDateTime"">
          <xs:annotation>
            <xs:appinfo>
              <b:recordInfo tag_name=""Reply_authorizedDateTime"" structure=""delimited"" child_delimiter_type=""char"" child_delimiter=""="" child_order=""prefix"" sequence_number=""10"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" />
            </xs:appinfo>
          </xs:annotation>
          <xs:complexType>
            <xs:sequence>
              <xs:annotation>
                <xs:appinfo>
                  <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                </xs:appinfo>
              </xs:annotation>
              <xs:element name=""Reply_authorizedDateTime_Value"" type=""xs:string"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:fieldInfo justification=""left"" sequence_number=""1"" pad_char_type=""hex"" min_length_with_pad=""1"" pad_char=""0x20"" />
                  </xs:appinfo>
                </xs:annotation>
              </xs:element>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element name=""Reply_reconciliationID"">
          <xs:annotation>
            <xs:appinfo>
              <b:recordInfo tag_name=""Reply_reconciliationID"" structure=""delimited"" child_delimiter_type=""char"" child_delimiter=""="" child_order=""prefix"" sequence_number=""11"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" />
            </xs:appinfo>
          </xs:annotation>
          <xs:complexType>
            <xs:sequence>
              <xs:annotation>
                <xs:appinfo>
                  <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                </xs:appinfo>
              </xs:annotation>
              <xs:element name=""Reply_reconciliationID_Value"" type=""xs:string"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:fieldInfo justification=""left"" sequence_number=""1"" pad_char_type=""hex"" min_length_with_pad=""1"" pad_char=""0x20"" />
                  </xs:appinfo>
                </xs:annotation>
              </xs:element>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element name=""Reply_authorizationCode"">
          <xs:annotation>
            <xs:appinfo>
              <b:recordInfo tag_name=""Reply_authorizationCode"" structure=""delimited"" child_delimiter_type=""char"" child_delimiter=""="" child_order=""prefix"" sequence_number=""12"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" />
            </xs:appinfo>
          </xs:annotation>
          <xs:complexType>
            <xs:sequence>
              <xs:annotation>
                <xs:appinfo>
                  <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                </xs:appinfo>
              </xs:annotation>
              <xs:element name=""Reply_authorizationCode_Value"" type=""xs:string"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:fieldInfo justification=""left"" sequence_number=""1"" pad_char_type=""hex"" min_length_with_pad=""1"" pad_char=""0x20"" />
                  </xs:appinfo>
                </xs:annotation>
              </xs:element>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element name=""Reply_processorResponse"">
          <xs:annotation>
            <xs:appinfo>
              <b:recordInfo tag_name=""Reply_processorResponse"" structure=""delimited"" child_delimiter_type=""char"" child_delimiter=""="" child_order=""prefix"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""13"" />
            </xs:appinfo>
          </xs:annotation>
          <xs:complexType>
            <xs:sequence>
              <xs:annotation>
                <xs:appinfo>
                  <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                </xs:appinfo>
              </xs:annotation>
              <xs:element name=""Reply_processorResponse_Value"" type=""xs:string"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:fieldInfo justification=""left"" sequence_number=""1"" pad_char_type=""hex"" min_length_with_pad=""1"" pad_char=""0x20"" />
                  </xs:appinfo>
                </xs:annotation>
              </xs:element>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element name=""Reply_accountbalance"">
          <xs:annotation>
            <xs:appinfo>
              <b:recordInfo tag_name=""Reply_accountbalance"" structure=""delimited"" child_delimiter_type=""char"" child_delimiter=""="" child_order=""prefix"" sequence_number=""14"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" />
            </xs:appinfo>
          </xs:annotation>
          <xs:complexType>
            <xs:sequence>
              <xs:annotation>
                <xs:appinfo>
                  <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                </xs:appinfo>
              </xs:annotation>
              <xs:element name=""Reply_accountbalance_Value"" type=""xs:string"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:fieldInfo justification=""left"" sequence_number=""1"" pad_char_type=""hex"" min_length_with_pad=""1"" pad_char=""0x20"" />
                  </xs:appinfo>
                </xs:annotation>
              </xs:element>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element name=""Reply_statusMessage"">
          <xs:annotation>
            <xs:appinfo>
              <b:recordInfo tag_name=""Reply_statusMessage"" structure=""delimited"" child_delimiter_type=""char"" child_delimiter=""="" child_order=""prefix"" sequence_number=""15"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" />
            </xs:appinfo>
          </xs:annotation>
          <xs:complexType>
            <xs:sequence>
              <xs:annotation>
                <xs:appinfo>
                  <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                </xs:appinfo>
              </xs:annotation>
              <xs:element name=""Reply_StatusMessage_Value"" type=""xs:string"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:fieldInfo justification=""left"" sequence_number=""1"" pad_char_type=""hex"" min_length_with_pad=""1"" pad_char=""0x20"" />
                  </xs:appinfo>
                </xs:annotation>
              </xs:element>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public ERPccResponse() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "ERPccResponseRoot";
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
