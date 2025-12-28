namespace BTNextGen.BizTalk.PaymentProcessing.Schemas {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"BTNextGen.BizTalk.PaymentProcessing.ERPTXNs.ERPccRequest",@"ERPccRequestRoot")]
    [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.String), "merchantReferenceCode.merchantReferenceCode_Value", XPath = @"/*[local-name()='ERPccRequestRoot' and namespace-uri()='BTNextGen.BizTalk.PaymentProcessing.ERPTXNs.ERPccRequest']/*[local-name()='merchantReferenceCode' and namespace-uri()='']/*[local-name()='merchantReferenceCode_Value' and namespace-uri()='']", XsdType = @"string")]
    [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.String), "notificationEmail.notificationEmail_Value", XPath = @"/*[local-name()='ERPccRequestRoot' and namespace-uri()='BTNextGen.BizTalk.PaymentProcessing.ERPTXNs.ERPccRequest']/*[local-name()='notificationEmail' and namespace-uri()='']/*[local-name()='notificationEmail_Value' and namespace-uri()='']", XsdType = @"string")]
    [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.String), "subscriptionID.subscriptionID_Value", XPath = @"/*[local-name()='ERPccRequestRoot' and namespace-uri()='BTNextGen.BizTalk.PaymentProcessing.ERPTXNs.ERPccRequest']/*[local-name()='subscriptionID' and namespace-uri()='']/*[local-name()='subscriptionID_Value' and namespace-uri()='']", XsdType = @"string")]
    [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.String), "ccAuthService.ccAuthService_Value", XPath = @"/*[local-name()='ERPccRequestRoot' and namespace-uri()='BTNextGen.BizTalk.PaymentProcessing.ERPTXNs.ERPccRequest']/*[local-name()='ccAuthService' and namespace-uri()='']/*[local-name()='ccAuthService_Value' and namespace-uri()='']", XsdType = @"string")]
    [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.String), "ccCreditService.ccCreditService_Value", XPath = @"/*[local-name()='ERPccRequestRoot' and namespace-uri()='BTNextGen.BizTalk.PaymentProcessing.ERPTXNs.ERPccRequest']/*[local-name()='ccCreditService' and namespace-uri()='']/*[local-name()='ccCreditService_Value' and namespace-uri()='']", XsdType = @"string")]
    [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.String), "merchantID.merchantID_Value", XPath = @"/*[local-name()='ERPccRequestRoot' and namespace-uri()='BTNextGen.BizTalk.PaymentProcessing.ERPTXNs.ERPccRequest']/*[local-name()='merchantID' and namespace-uri()='']/*[local-name()='merchantID_Value' and namespace-uri()='']", XsdType = @"string")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"ERPccRequestRoot"})]
    public sealed class ERPccRequest : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns=""BTNextGen.BizTalk.PaymentProcessing.ERPTXNs.ERPccRequest"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" targetNamespace=""BTNextGen.BizTalk.PaymentProcessing.ERPTXNs.ERPccRequest"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:annotation>
    <xs:appinfo>
      <schemaEditorExtension:schemaInfo namespaceAlias=""b"" extensionClass=""Microsoft.BizTalk.FlatFileExtension.FlatFileExtension"" standardName=""Flat File"" xmlns:schemaEditorExtension=""http://schemas.microsoft.com/BizTalk/2003/SchemaEditorExtensions"" />
      <b:schemaInfo standard=""Flat File"" codepage=""65001"" default_pad_char="" "" pad_char_type=""char"" count_positions_by_byte=""false"" parser_optimization=""complexity"" lookahead_depth=""0"" suppress_empty_nodes=""false"" generate_empty_nodes=""true"" allow_early_termination=""false"" early_terminate_optional_fields=""false"" allow_message_breakup_of_infix_root=""false"" compile_parse_tables=""false"" root_reference=""ERPccRequestRoot"" />
    </xs:appinfo>
  </xs:annotation>
  <xs:element name=""ERPccRequestRoot"">
    <xs:annotation>
      <xs:appinfo>
        <b:recordInfo structure=""delimited"" child_delimiter_type=""hex"" child_delimiter=""0xD 0xA"" child_order=""postfix"" sequence_number=""1"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" />
        <b:properties>
          <b:property distinguished=""true"" xpath=""/*[local-name()='ERPccRequestRoot' and namespace-uri()='BTNextGen.BizTalk.PaymentProcessing.ERPTXNs.ERPccRequest']/*[local-name()='merchantReferenceCode' and namespace-uri()='']/*[local-name()='merchantReferenceCode_Value' and namespace-uri()='']"" />
          <b:property distinguished=""true"" xpath=""/*[local-name()='ERPccRequestRoot' and namespace-uri()='BTNextGen.BizTalk.PaymentProcessing.ERPTXNs.ERPccRequest']/*[local-name()='notificationEmail' and namespace-uri()='']/*[local-name()='notificationEmail_Value' and namespace-uri()='']"" />
          <b:property distinguished=""true"" xpath=""/*[local-name()='ERPccRequestRoot' and namespace-uri()='BTNextGen.BizTalk.PaymentProcessing.ERPTXNs.ERPccRequest']/*[local-name()='subscriptionID' and namespace-uri()='']/*[local-name()='subscriptionID_Value' and namespace-uri()='']"" />
          <b:property distinguished=""true"" xpath=""/*[local-name()='ERPccRequestRoot' and namespace-uri()='BTNextGen.BizTalk.PaymentProcessing.ERPTXNs.ERPccRequest']/*[local-name()='ccAuthService' and namespace-uri()='']/*[local-name()='ccAuthService_Value' and namespace-uri()='']"" />
          <b:property distinguished=""true"" xpath=""/*[local-name()='ERPccRequestRoot' and namespace-uri()='BTNextGen.BizTalk.PaymentProcessing.ERPTXNs.ERPccRequest']/*[local-name()='ccCreditService' and namespace-uri()='']/*[local-name()='ccCreditService_Value' and namespace-uri()='']"" />
          <b:property distinguished=""true"" xpath=""/*[local-name()='ERPccRequestRoot' and namespace-uri()='BTNextGen.BizTalk.PaymentProcessing.ERPTXNs.ERPccRequest']/*[local-name()='merchantID' and namespace-uri()='']/*[local-name()='merchantID_Value' and namespace-uri()='']"" />
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
        <xs:element minOccurs=""0"" name=""ccAuthService"">
          <xs:annotation>
            <xs:appinfo>
              <b:recordInfo tag_name=""ccAuthService"" structure=""delimited"" child_delimiter_type=""char"" child_delimiter=""="" child_order=""prefix"" sequence_number=""1"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" />
            </xs:appinfo>
          </xs:annotation>
          <xs:complexType>
            <xs:sequence>
              <xs:annotation>
                <xs:appinfo>
                  <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                </xs:appinfo>
              </xs:annotation>
              <xs:element minOccurs=""0"" name=""ccAuthService_Value"" type=""xs:string"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:fieldInfo justification=""left"" sequence_number=""1"" />
                  </xs:appinfo>
                </xs:annotation>
              </xs:element>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element minOccurs=""0"" name=""ccCaptureService"">
          <xs:annotation>
            <xs:appinfo>
              <b:recordInfo tag_name=""ccCaptureService"" structure=""delimited"" child_delimiter_type=""char"" child_delimiter=""="" child_order=""prefix"" sequence_number=""2"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" />
            </xs:appinfo>
          </xs:annotation>
          <xs:complexType>
            <xs:sequence>
              <xs:annotation>
                <xs:appinfo>
                  <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                </xs:appinfo>
              </xs:annotation>
              <xs:element minOccurs=""0"" name=""ccCaptureService_Value"" type=""xs:string"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:fieldInfo justification=""left"" sequence_number=""1"" />
                  </xs:appinfo>
                </xs:annotation>
              </xs:element>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element minOccurs=""0"" name=""ccAuthReversalService"">
          <xs:annotation>
            <xs:appinfo>
              <b:recordInfo tag_name=""ccAuthReversalService"" structure=""delimited"" child_delimiter_type=""char"" child_delimiter=""="" child_order=""prefix"" sequence_number=""3"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" />
            </xs:appinfo>
          </xs:annotation>
          <xs:complexType>
            <xs:sequence>
              <xs:annotation>
                <xs:appinfo>
                  <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                </xs:appinfo>
              </xs:annotation>
              <xs:element minOccurs=""0"" name=""ccAuthReversalService_Value"" type=""xs:string"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:fieldInfo justification=""left"" sequence_number=""1"" />
                  </xs:appinfo>
                </xs:annotation>
              </xs:element>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element minOccurs=""0"" name=""ccCreditService"">
          <xs:annotation>
            <xs:appinfo>
              <b:recordInfo tag_name=""ccCreditService"" structure=""delimited"" child_delimiter_type=""char"" child_delimiter=""="" child_order=""prefix"" sequence_number=""4"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" />
            </xs:appinfo>
          </xs:annotation>
          <xs:complexType>
            <xs:sequence>
              <xs:annotation>
                <xs:appinfo>
                  <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                </xs:appinfo>
              </xs:annotation>
              <xs:element minOccurs=""0"" name=""ccCreditService_Value"" type=""xs:string"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:fieldInfo justification=""left"" sequence_number=""1"" />
                  </xs:appinfo>
                </xs:annotation>
              </xs:element>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element minOccurs=""0"" name=""purchasingLevel"">
          <xs:annotation>
            <xs:appinfo>
              <b:recordInfo structure=""delimited"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""5"" child_delimiter_type=""char"" child_delimiter=""="" tag_name=""purchasingLevel"" />
            </xs:appinfo>
          </xs:annotation>
          <xs:complexType>
            <xs:sequence>
              <xs:annotation>
                <xs:appinfo>
                  <b:groupInfo sequence_number=""0"" />
                </xs:appinfo>
              </xs:annotation>
              <xs:element minOccurs=""0"" name=""purchasingLevel_Value"" type=""xs:string"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:fieldInfo sequence_number=""1"" justification=""left"" />
                  </xs:appinfo>
                </xs:annotation>
              </xs:element>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element name=""merchantID"">
          <xs:annotation>
            <xs:appinfo>
              <b:recordInfo tag_name=""merchantID"" structure=""delimited"" child_delimiter_type=""char"" child_delimiter=""="" child_order=""prefix"" sequence_number=""6"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" />
            </xs:appinfo>
          </xs:annotation>
          <xs:complexType>
            <xs:sequence>
              <xs:annotation>
                <xs:appinfo>
                  <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                </xs:appinfo>
              </xs:annotation>
              <xs:element name=""merchantID_Value"" type=""xs:string"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:fieldInfo justification=""left"" sequence_number=""1"" />
                  </xs:appinfo>
                </xs:annotation>
              </xs:element>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element name=""merchantReferenceCode"">
          <xs:annotation>
            <xs:appinfo>
              <b:recordInfo tag_name=""merchantReferenceCode"" structure=""delimited"" child_delimiter_type=""char"" child_delimiter=""="" child_order=""prefix"" sequence_number=""7"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" />
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
                    <b:fieldInfo justification=""left"" sequence_number=""1"" />
                  </xs:appinfo>
                </xs:annotation>
              </xs:element>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element minOccurs=""0"" name=""subscriptionID"">
          <xs:annotation>
            <xs:appinfo>
              <b:recordInfo tag_name=""subscriptionID"" structure=""delimited"" child_delimiter_type=""char"" child_delimiter=""="" child_order=""prefix"" sequence_number=""8"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" />
            </xs:appinfo>
          </xs:annotation>
          <xs:complexType>
            <xs:sequence>
              <xs:annotation>
                <xs:appinfo>
                  <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                </xs:appinfo>
              </xs:annotation>
              <xs:element minOccurs=""0"" name=""subscriptionID_Value"" type=""xs:string"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:fieldInfo justification=""left"" sequence_number=""1"" />
                  </xs:appinfo>
                </xs:annotation>
              </xs:element>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element minOccurs=""0"" name=""requestID"">
          <xs:annotation>
            <xs:appinfo>
              <b:recordInfo tag_name=""requestID"" structure=""delimited"" child_delimiter_type=""char"" child_delimiter=""="" child_order=""prefix"" sequence_number=""9"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" />
            </xs:appinfo>
          </xs:annotation>
          <xs:complexType>
            <xs:sequence>
              <xs:annotation>
                <xs:appinfo>
                  <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                </xs:appinfo>
              </xs:annotation>
              <xs:element minOccurs=""0"" name=""requestID_Value"" type=""xs:string"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:fieldInfo justification=""left"" sequence_number=""1"" />
                  </xs:appinfo>
                </xs:annotation>
              </xs:element>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element minOccurs=""0"" name=""requestToken"">
          <xs:annotation>
            <xs:appinfo>
              <b:recordInfo tag_name=""requestToken"" structure=""delimited"" child_delimiter_type=""char"" child_delimiter=""="" child_order=""prefix"" sequence_number=""10"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" />
            </xs:appinfo>
          </xs:annotation>
          <xs:complexType>
            <xs:sequence>
              <xs:annotation>
                <xs:appinfo>
                  <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                </xs:appinfo>
              </xs:annotation>
              <xs:element minOccurs=""0"" name=""requestToken_Value"" type=""xs:string"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:fieldInfo justification=""left"" sequence_number=""1"" />
                  </xs:appinfo>
                </xs:annotation>
              </xs:element>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element minOccurs=""0"" name=""shipFromPostal"">
          <xs:annotation>
            <xs:appinfo>
              <b:recordInfo structure=""delimited"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""11"" child_delimiter_type=""char"" child_delimiter=""="" child_order=""prefix"" tag_name=""shipFromPostal"" />
            </xs:appinfo>
          </xs:annotation>
          <xs:complexType>
            <xs:sequence>
              <xs:annotation>
                <xs:appinfo>
                  <b:groupInfo sequence_number=""0"" />
                </xs:appinfo>
              </xs:annotation>
              <xs:element minOccurs=""0"" name=""shipFromPostal_Value"" type=""xs:string"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:fieldInfo sequence_number=""1"" justification=""left"" />
                  </xs:appinfo>
                </xs:annotation>
              </xs:element>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element minOccurs=""0"" name=""shipToCountry"">
          <xs:annotation>
            <xs:appinfo>
              <b:recordInfo structure=""delimited"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""12"" child_delimiter_type=""char"" child_delimiter=""="" child_order=""prefix"" tag_name=""shipToCountry"" />
            </xs:appinfo>
          </xs:annotation>
          <xs:complexType>
            <xs:sequence>
              <xs:annotation>
                <xs:appinfo>
                  <b:groupInfo sequence_number=""0"" />
                </xs:appinfo>
              </xs:annotation>
              <xs:element minOccurs=""0"" name=""shipToCountry_Value"" type=""xs:string"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:fieldInfo sequence_number=""1"" justification=""left"" />
                  </xs:appinfo>
                </xs:annotation>
              </xs:element>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element minOccurs=""0"" name=""shipToPostal"">
          <xs:annotation>
            <xs:appinfo>
              <b:recordInfo structure=""delimited"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""13"" child_delimiter_type=""char"" child_delimiter=""="" tag_name=""shipToPostal"" />
            </xs:appinfo>
          </xs:annotation>
          <xs:complexType>
            <xs:sequence>
              <xs:annotation>
                <xs:appinfo>
                  <b:groupInfo sequence_number=""0"" />
                </xs:appinfo>
              </xs:annotation>
              <xs:element minOccurs=""0"" name=""shipToPostal_Value"" type=""xs:string"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:fieldInfo sequence_number=""1"" justification=""left"" />
                  </xs:appinfo>
                </xs:annotation>
              </xs:element>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element minOccurs=""0"" name=""shipToState"">
          <xs:annotation>
            <xs:appinfo>
              <b:recordInfo structure=""delimited"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""14"" child_delimiter_type=""char"" child_delimiter=""="" tag_name=""shipToState"" />
            </xs:appinfo>
          </xs:annotation>
          <xs:complexType>
            <xs:sequence>
              <xs:annotation>
                <xs:appinfo>
                  <b:groupInfo sequence_number=""0"" />
                </xs:appinfo>
              </xs:annotation>
              <xs:element minOccurs=""0"" name=""shipToState_Value"" type=""xs:string"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:fieldInfo sequence_number=""1"" justification=""left"" />
                  </xs:appinfo>
                </xs:annotation>
              </xs:element>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element name=""item_0_unitprice"">
          <xs:annotation>
            <xs:appinfo>
              <b:recordInfo tag_name=""item_0_unitprice"" structure=""delimited"" child_delimiter_type=""char"" child_delimiter=""="" child_order=""prefix"" sequence_number=""15"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" />
            </xs:appinfo>
          </xs:annotation>
          <xs:complexType>
            <xs:sequence>
              <xs:annotation>
                <xs:appinfo>
                  <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                </xs:appinfo>
              </xs:annotation>
              <xs:element name=""item_0_unitprice_Value"" type=""xs:string"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:fieldInfo justification=""left"" sequence_number=""1"" />
                  </xs:appinfo>
                </xs:annotation>
              </xs:element>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element minOccurs=""0"" name=""item_0_quantity"">
          <xs:annotation>
            <xs:appinfo>
              <b:recordInfo tag_name=""item_0_quantity"" structure=""delimited"" child_delimiter_type=""char"" child_delimiter=""="" child_order=""prefix"" sequence_number=""16"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" />
            </xs:appinfo>
          </xs:annotation>
          <xs:complexType>
            <xs:sequence>
              <xs:annotation>
                <xs:appinfo>
                  <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                </xs:appinfo>
              </xs:annotation>
              <xs:element minOccurs=""0"" name=""item_0_quantity_Value"" type=""xs:string"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:fieldInfo justification=""left"" sequence_number=""1"" />
                  </xs:appinfo>
                </xs:annotation>
              </xs:element>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element minOccurs=""0"" name=""item_0_taxamount"">
          <xs:annotation>
            <xs:appinfo>
              <b:recordInfo tag_name=""item_0_taxamount"" structure=""delimited"" child_delimiter_type=""char"" child_delimiter=""="" child_order=""prefix"" sequence_number=""17"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" />
            </xs:appinfo>
          </xs:annotation>
          <xs:complexType>
            <xs:sequence>
              <xs:annotation>
                <xs:appinfo>
                  <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                </xs:appinfo>
              </xs:annotation>
              <xs:element minOccurs=""0"" name=""item_0_taxamount_Value"" type=""xs:string"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:fieldInfo justification=""left"" sequence_number=""1"" />
                  </xs:appinfo>
                </xs:annotation>
              </xs:element>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element minOccurs=""0"" name=""item_0_taxrate"">
          <xs:annotation>
            <xs:appinfo>
              <b:recordInfo structure=""delimited"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""18"" child_delimiter_type=""char"" child_delimiter=""="" tag_name=""item_0_taxrate"" child_order=""prefix"" />
            </xs:appinfo>
          </xs:annotation>
          <xs:complexType>
            <xs:sequence>
              <xs:annotation>
                <xs:appinfo>
                  <b:groupInfo sequence_number=""0"" />
                </xs:appinfo>
              </xs:annotation>
              <xs:element minOccurs=""0"" name=""item_0_taxrate_Value"" type=""xs:string"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:fieldInfo sequence_number=""1"" justification=""left"" />
                  </xs:appinfo>
                </xs:annotation>
              </xs:element>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element name=""purchaseTotals_currency"">
          <xs:annotation>
            <xs:appinfo>
              <b:recordInfo tag_name=""purchaseTotals_currency"" structure=""delimited"" child_delimiter_type=""char"" child_delimiter=""="" child_order=""prefix"" sequence_number=""19"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" />
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
                    <b:fieldInfo justification=""left"" sequence_number=""1"" />
                  </xs:appinfo>
                </xs:annotation>
              </xs:element>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element minOccurs=""0"" name=""invoiceHeader_userPO"">
          <xs:annotation>
            <xs:appinfo>
              <b:recordInfo tag_name=""invoiceHeader_userPO"" structure=""delimited"" child_delimiter_type=""char"" child_delimiter=""="" child_order=""prefix"" sequence_number=""20"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" />
            </xs:appinfo>
          </xs:annotation>
          <xs:complexType>
            <xs:sequence>
              <xs:annotation>
                <xs:appinfo>
                  <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                </xs:appinfo>
              </xs:annotation>
              <xs:element minOccurs=""0"" name=""invoiceHeader_userPO_Value"" type=""xs:string"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:fieldInfo justification=""left"" sequence_number=""1"" />
                  </xs:appinfo>
                </xs:annotation>
              </xs:element>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element minOccurs=""0"" name=""invoiceHeader_amexDataTAA1"">
          <xs:annotation>
            <xs:appinfo>
              <b:recordInfo structure=""delimited"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""21"" child_delimiter_type=""char"" child_delimiter=""="" tag_name=""invoiceHeader_amexDataTAA1"" />
            </xs:appinfo>
          </xs:annotation>
          <xs:complexType>
            <xs:sequence>
              <xs:annotation>
                <xs:appinfo>
                  <b:groupInfo sequence_number=""0"" />
                </xs:appinfo>
              </xs:annotation>
              <xs:element minOccurs=""0"" name=""invoiceHeader_amexDataTAA1_Value"" type=""xs:string"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:fieldInfo sequence_number=""1"" justification=""left"" />
                  </xs:appinfo>
                </xs:annotation>
              </xs:element>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element minOccurs=""0"" name=""invoiceHeader_merchantDescriptor"">
          <xs:annotation>
            <xs:appinfo>
              <b:recordInfo structure=""delimited"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""22"" tag_name=""invoiceHeader_merchantDescriptor"" child_delimiter_type=""char"" child_delimiter=""="" />
            </xs:appinfo>
          </xs:annotation>
          <xs:complexType>
            <xs:sequence>
              <xs:annotation>
                <xs:appinfo>
                  <b:groupInfo sequence_number=""0"" />
                </xs:appinfo>
              </xs:annotation>
              <xs:element minOccurs=""0"" name=""invoiceHeader_merchantDescriptor_Value"" type=""xs:string"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:fieldInfo sequence_number=""1"" justification=""left"" />
                  </xs:appinfo>
                </xs:annotation>
              </xs:element>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element minOccurs=""0"" name=""invoiceHeader_merchantDescriptorContact"">
          <xs:annotation>
            <xs:appinfo>
              <b:recordInfo tag_name=""invoiceHeader_merchantDescriptorContact"" structure=""delimited"" child_delimiter_type=""char"" child_delimiter=""="" child_order=""prefix"" sequence_number=""23"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" />
            </xs:appinfo>
          </xs:annotation>
          <xs:complexType>
            <xs:sequence>
              <xs:annotation>
                <xs:appinfo>
                  <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                </xs:appinfo>
              </xs:annotation>
              <xs:element minOccurs=""0"" name=""invoiceHeader_merchantDescriptorContact_Value"" type=""xs:string"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:fieldInfo justification=""left"" sequence_number=""1"" />
                  </xs:appinfo>
                </xs:annotation>
              </xs:element>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""notificationEmail"">
          <xs:annotation>
            <xs:appinfo>
              <b:recordInfo sequence_number=""24"" structure=""delimited"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" tag_name=""notificationEmail"" child_delimiter_type=""char"" child_delimiter=""="" child_order=""prefix"" />
            </xs:appinfo>
          </xs:annotation>
          <xs:complexType>
            <xs:sequence>
              <xs:annotation>
                <xs:appinfo>
                  <b:groupInfo sequence_number=""0"" />
                </xs:appinfo>
              </xs:annotation>
              <xs:element name=""notificationEmail_Value"" type=""xs:string"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:fieldInfo sequence_number=""1"" justification=""left"" />
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
        
        public ERPccRequest() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "ERPccRequestRoot";
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
