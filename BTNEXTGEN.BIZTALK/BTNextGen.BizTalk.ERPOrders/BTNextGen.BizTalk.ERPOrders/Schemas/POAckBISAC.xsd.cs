namespace BTNextGen.BizTalk.ERPOrders.Schemas {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"BTNextGen.BizTalk.ERPOrders.Schemas.POAckBISAC",@"POAck")]
    [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.String), "Header.FileHeader.TransmissionID", XPath = @"/*[local-name()='POAck' and namespace-uri()='BTNextGen.BizTalk.ERPOrders.Schemas.POAckBISAC']/*[local-name()='Header' and namespace-uri()='']/*[local-name()='FileHeader' and namespace-uri()='']/*[local-name()='TransmissionID' and namespace-uri()='']", XsdType = @"string")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"POAck"})]
    public sealed class POAckBISAC : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns=""BTNextGen.BizTalk.ERPOrders.Schemas.POAckBISAC"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" targetNamespace=""BTNextGen.BizTalk.ERPOrders.Schemas.POAckBISAC"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:annotation>
    <xs:appinfo>
      <schemaEditorExtension:schemaInfo namespaceAlias=""b"" extensionClass=""Microsoft.BizTalk.FlatFileExtension.FlatFileExtension"" standardName=""Flat File"" xmlns:schemaEditorExtension=""http://schemas.microsoft.com/BizTalk/2003/SchemaEditorExtensions"" />
      <b:schemaInfo standard=""Flat File"" codepage=""65001"" default_pad_char="" "" pad_char_type=""char"" count_positions_by_byte=""false"" parser_optimization=""complexity"" lookahead_depth=""3"" suppress_empty_nodes=""false"" generate_empty_nodes=""true"" allow_early_termination=""false"" early_terminate_optional_fields=""false"" allow_message_breakup_of_infix_root=""false"" compile_parse_tables=""false"" root_reference=""POAck"" />
    </xs:appinfo>
  </xs:annotation>
  <xs:element name=""POAck"">
    <xs:annotation>
      <xs:appinfo>
        <b:recordInfo structure=""delimited"" child_delimiter_type=""hex"" child_delimiter=""0xD 0xA"" child_order=""postfix"" sequence_number=""1"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" />
        <b:properties>
          <b:property distinguished=""true"" xpath=""/*[local-name()='POAck' and namespace-uri()='BTNextGen.BizTalk.ERPOrders.Schemas.POAckBISAC']/*[local-name()='Header' and namespace-uri()='']/*[local-name()='FileHeader' and namespace-uri()='']/*[local-name()='TransmissionID' and namespace-uri()='']"" />
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
        <xs:element name=""Header"">
          <xs:annotation>
            <xs:appinfo>
              <recordInfo sequence_number=""1"" structure=""delimited"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" child_delimiter_type=""hex"" child_delimiter=""0x0D 0x0A"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
            </xs:appinfo>
          </xs:annotation>
          <xs:complexType>
            <xs:sequence>
              <xs:annotation>
                <xs:appinfo>
                  <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                </xs:appinfo>
              </xs:annotation>
              <xs:element name=""FileHeader"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:recordInfo structure=""positional"" tag_name=""02"" tag_offset=""0"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""1"" />
                  </xs:appinfo>
                </xs:annotation>
                <xs:complexType>
                  <xs:sequence>
                    <xs:annotation>
                      <xs:appinfo>
                        <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                      </xs:appinfo>
                    </xs:annotation>
                    <xs:element name=""Tag02"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""2"" sequence_number=""1"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""SeqNumber02"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""5"" sequence_number=""2"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""SourceSan02"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""7"" sequence_number=""3"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""SourceSuffix02"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""5"" sequence_number=""4"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""TransmissionID"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""13"" sequence_number=""5"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""FileDate"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""6"" sequence_number=""6"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""FileName"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""22"" sequence_number=""7"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""BISACVersion"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""3"" sequence_number=""8"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""VendorSAN"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""7"" sequence_number=""9"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""VendorSuffix"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""5"" sequence_number=""10"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""AckType"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""1"" sequence_number=""11"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""Filler02"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""4"" sequence_number=""12"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
              <xs:element name=""POHeader11"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:recordInfo structure=""positional"" tag_name=""11"" tag_offset=""0"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""2"" />
                  </xs:appinfo>
                </xs:annotation>
                <xs:complexType>
                  <xs:sequence>
                    <xs:annotation>
                      <xs:appinfo>
                        <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                      </xs:appinfo>
                    </xs:annotation>
                    <xs:element name=""Tag11"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""2"" sequence_number=""1"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""SeqNumber11"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""5"" sequence_number=""2"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""BTBOrderNumber"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""13"" sequence_number=""3"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""PONumber"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""13"" sequence_number=""4"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""AccountSAN"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""7"" sequence_number=""5"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""AccountSuffix"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""5"" sequence_number=""6"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""BTBSAN"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""7"" sequence_number=""7"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""BTBSuffix"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""5"" sequence_number=""8"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""AckDate"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""6"" sequence_number=""9"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""CurrencyCode"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""3"" sequence_number=""10"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""OrderDate"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""6"" sequence_number=""11"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""OrderCancelDate"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""6"" sequence_number=""12"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""Filler11"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""2"" sequence_number=""13"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
              <xs:element name=""POHeader12"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:recordInfo structure=""positional"" tag_name=""12"" tag_offset=""0"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""3"" />
                  </xs:appinfo>
                </xs:annotation>
                <xs:complexType>
                  <xs:sequence>
                    <xs:annotation>
                      <xs:appinfo>
                        <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                      </xs:appinfo>
                    </xs:annotation>
                    <xs:element name=""Tag12"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""2"" sequence_number=""1"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""SeqNumber12"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""5"" sequence_number=""2"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""BTBOrderNumber"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""13"" sequence_number=""3"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""AccountNumber"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""12"" sequence_number=""4"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""Filler12"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""48"" sequence_number=""5"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
              <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""Lines21"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:recordInfo structure=""delimited"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" child_order=""infix"" child_delimiter_type=""hex"" child_delimiter=""0x0D 0x0A"" sequence_number=""4"" />
                  </xs:appinfo>
                </xs:annotation>
                <xs:complexType>
                  <xs:sequence>
                    <xs:annotation>
                      <xs:appinfo>
                        <b:groupInfo sequence_number=""0"" />
                      </xs:appinfo>
                    </xs:annotation>
                    <xs:element maxOccurs=""unbounded"" name=""POHeader21"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:recordInfo structure=""positional"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" tag_name=""21"" tag_offset=""0"" sequence_number=""1"" />
                        </xs:appinfo>
                      </xs:annotation>
                      <xs:complexType>
                        <xs:sequence>
                          <xs:annotation>
                            <xs:appinfo>
                              <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                            </xs:appinfo>
                          </xs:annotation>
                          <xs:element maxOccurs=""unbounded"" name=""Tag21"" type=""xs:string"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""2"" sequence_number=""1"" />
                              </xs:appinfo>
                            </xs:annotation>
                          </xs:element>
                          <xs:element maxOccurs=""unbounded"" name=""SeqNumber12"" type=""xs:string"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""5"" sequence_number=""2"" />
                              </xs:appinfo>
                            </xs:annotation>
                          </xs:element>
                          <xs:element maxOccurs=""unbounded"" name=""BTBOrderNumber"" type=""xs:string"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""13"" sequence_number=""3"" />
                              </xs:appinfo>
                            </xs:annotation>
                          </xs:element>
                          <xs:element maxOccurs=""unbounded"" name=""Message"" type=""xs:string"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""60"" sequence_number=""4"" />
                              </xs:appinfo>
                            </xs:annotation>
                          </xs:element>
                        </xs:sequence>
                      </xs:complexType>
                    </xs:element>
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
              <xs:element minOccurs=""0"" maxOccurs=""1"" name=""POHeader25"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:recordInfo structure=""positional"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" tag_name=""25"" tag_offset=""0"" sequence_number=""5"" />
                  </xs:appinfo>
                </xs:annotation>
                <xs:complexType>
                  <xs:sequence>
                    <xs:annotation>
                      <xs:appinfo>
                        <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                      </xs:appinfo>
                    </xs:annotation>
                    <xs:element name=""Tag25"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""2"" sequence_number=""1"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""SeqNumber25"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""5"" sequence_number=""2"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""BTBOrderNumber"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""13"" sequence_number=""3"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""BTAddress1"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""25"" sequence_number=""4"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""BTAddress2"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""35"" sequence_number=""5"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
              <xs:element minOccurs=""0"" maxOccurs=""1"" name=""POHeader26"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:recordInfo structure=""positional"" tag_name=""26"" tag_offset=""0"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""6"" />
                  </xs:appinfo>
                </xs:annotation>
                <xs:complexType>
                  <xs:sequence>
                    <xs:annotation>
                      <xs:appinfo>
                        <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                      </xs:appinfo>
                    </xs:annotation>
                    <xs:element name=""Tag26"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""2"" sequence_number=""1"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""SeqNumber26"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""5"" sequence_number=""2"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""BTBOrderNumber"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""13"" sequence_number=""3"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""BTAddress3"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""25"" sequence_number=""4"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""BTAddress4"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""35"" sequence_number=""5"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
              <xs:element minOccurs=""0"" maxOccurs=""1"" name=""POHeader27"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:recordInfo structure=""positional"" tag_name=""27"" tag_offset=""0"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""7"" />
                  </xs:appinfo>
                </xs:annotation>
                <xs:complexType>
                  <xs:sequence>
                    <xs:annotation>
                      <xs:appinfo>
                        <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                      </xs:appinfo>
                    </xs:annotation>
                    <xs:element name=""Tag27"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""2"" sequence_number=""1"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""SeqNumber27"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""5"" sequence_number=""2"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""BTBOrderNumber"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""13"" sequence_number=""3"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""BTCity"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""22"" sequence_number=""4"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""BTState"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""3"" sequence_number=""5"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""BTZip"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""5"" sequence_number=""6"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""Filler27"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""30"" sequence_number=""7"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
              <xs:element minOccurs=""0"" maxOccurs=""1"" name=""POHeader30"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:recordInfo structure=""positional"" tag_name=""30"" tag_offset=""0"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""8"" />
                  </xs:appinfo>
                </xs:annotation>
                <xs:complexType>
                  <xs:sequence>
                    <xs:annotation>
                      <xs:appinfo>
                        <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                      </xs:appinfo>
                    </xs:annotation>
                    <xs:element name=""Tag30"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""2"" sequence_number=""1"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""SeqNumber30"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""5"" sequence_number=""2"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""BTBOrderNumber"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""13"" sequence_number=""3"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""STAddress1"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""25"" sequence_number=""4"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""STAddress2"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""35"" sequence_number=""5"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
              <xs:element minOccurs=""0"" maxOccurs=""1"" name=""POHeader31"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:recordInfo structure=""positional"" tag_name=""31"" tag_offset=""0"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""9"" />
                  </xs:appinfo>
                </xs:annotation>
                <xs:complexType>
                  <xs:sequence>
                    <xs:annotation>
                      <xs:appinfo>
                        <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                      </xs:appinfo>
                    </xs:annotation>
                    <xs:element name=""Tag31"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""2"" sequence_number=""1"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""SeqNumber31"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""5"" sequence_number=""2"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""BTBOrderNumber"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""13"" sequence_number=""3"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""STAddress3"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""25"" sequence_number=""4"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""STAddress4"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""35"" sequence_number=""5"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
              <xs:element minOccurs=""0"" maxOccurs=""1"" name=""POHeader32"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:recordInfo structure=""positional"" tag_name=""32"" tag_offset=""0"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""10"" />
                  </xs:appinfo>
                </xs:annotation>
                <xs:complexType>
                  <xs:sequence>
                    <xs:annotation>
                      <xs:appinfo>
                        <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                      </xs:appinfo>
                    </xs:annotation>
                    <xs:element name=""Tag32"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""2"" sequence_number=""1"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""SeqNumber32"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""5"" sequence_number=""2"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""BTBOrderNumber"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""13"" sequence_number=""3"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""STCity"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""22"" sequence_number=""4"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""STState"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""3"" sequence_number=""5"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""STZip"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""5"" sequence_number=""6"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""Filler32"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""30"" sequence_number=""7"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element maxOccurs=""unbounded"" name=""LineItem"">
          <xs:annotation>
            <xs:appinfo>
              <recordInfo sequence_number=""2"" structure=""delimited"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" child_delimiter_type=""hex"" child_delimiter=""0x0D 0x0A"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
            </xs:appinfo>
          </xs:annotation>
          <xs:complexType>
            <xs:sequence>
              <xs:annotation>
                <xs:appinfo>
                  <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                </xs:appinfo>
              </xs:annotation>
              <xs:element maxOccurs=""unbounded"" name=""POLine40"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:recordInfo structure=""positional"" tag_name=""40"" tag_offset=""0"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""1"" />
                  </xs:appinfo>
                </xs:annotation>
                <xs:complexType>
                  <xs:sequence>
                    <xs:annotation>
                      <xs:appinfo>
                        <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                      </xs:appinfo>
                    </xs:annotation>
                    <xs:element name=""Tag40"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""2"" sequence_number=""1"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""SeqNumber40"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""5"" sequence_number=""2"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""BTBOrderNumber"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""13"" sequence_number=""3"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""CusLineNumber"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""10"" sequence_number=""4"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""ISBN10"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""10"" sequence_number=""5"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""OrderQTY"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""5"" sequence_number=""6"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""UnitPrice"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""8"" sequence_number=""7"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""NetPrice"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""9"" sequence_number=""8"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""ListNetFlag"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""1"" sequence_number=""9"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""SpecialPriceFlag"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""1"" sequence_number=""10"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""DiscountPercent"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""5"" sequence_number=""11"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""ShippableQTY"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""5"" sequence_number=""12"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""StatusCode"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""2"" sequence_number=""13"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""StatusWHS"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""2"" sequence_number=""14"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""Filler40"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""2"" sequence_number=""15"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
              <xs:element maxOccurs=""unbounded"" name=""POLine41"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:recordInfo structure=""positional"" tag_name=""41"" tag_offset=""0"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""2"" />
                  </xs:appinfo>
                </xs:annotation>
                <xs:complexType>
                  <xs:sequence>
                    <xs:annotation>
                      <xs:appinfo>
                        <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                      </xs:appinfo>
                    </xs:annotation>
                    <xs:element name=""Tag41"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""2"" sequence_number=""1"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""SeqNumber41"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""5"" sequence_number=""2"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""BTBOrderNumber"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""13"" sequence_number=""3"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""ShippableQTY"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""5"" sequence_number=""4"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""WHSCode"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""2"" sequence_number=""5"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""AvailableDate"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""6"" sequence_number=""6"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""StatusCode"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""2"" sequence_number=""7"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""ISBNOrdered"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""10"" sequence_number=""8"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""ReportCode"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""3"" sequence_number=""9"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""ShippableQTY"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""5"" sequence_number=""10"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""OrderedQTY"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""5"" sequence_number=""11"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""BackorderedQTY"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""5"" sequence_number=""12"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""CancelledQTY"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""5"" sequence_number=""13"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""NonStockQTY"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""5"" sequence_number=""14"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""BKOCancelFlag"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""1"" sequence_number=""15"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""Filler"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""6"" sequence_number=""16"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
              <xs:element maxOccurs=""unbounded"" name=""POLine42"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:recordInfo structure=""positional"" tag_name=""42"" tag_offset=""0"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""3"" />
                  </xs:appinfo>
                </xs:annotation>
                <xs:complexType>
                  <xs:sequence>
                    <xs:annotation>
                      <xs:appinfo>
                        <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                      </xs:appinfo>
                    </xs:annotation>
                    <xs:element name=""Tag42"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""2"" sequence_number=""1"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""SeqNumber42"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""5"" sequence_number=""2"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""BTBOrderNumber"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""13"" sequence_number=""3"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""Title"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""30"" sequence_number=""4"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""Author"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""27"" sequence_number=""5"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""Edition"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""2"" sequence_number=""6"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""BindingCode"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""1"" sequence_number=""7"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
              <xs:element maxOccurs=""unbounded"" name=""POLine43"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:recordInfo structure=""positional"" tag_name=""43"" tag_offset=""0"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""4"" />
                  </xs:appinfo>
                </xs:annotation>
                <xs:complexType>
                  <xs:sequence>
                    <xs:annotation>
                      <xs:appinfo>
                        <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                      </xs:appinfo>
                    </xs:annotation>
                    <xs:element name=""Tag43"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""2"" sequence_number=""1"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""SeqNumber43"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""5"" sequence_number=""2"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""BTBOrderNumber"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""13"" sequence_number=""3"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""Publisher"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""20"" sequence_number=""4"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""Description"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""31"" sequence_number=""5"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""Filler43"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""9"" sequence_number=""6"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
              <xs:element maxOccurs=""unbounded"" name=""POLine46"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:recordInfo structure=""positional"" tag_name=""46"" tag_offset=""0"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""5"" />
                  </xs:appinfo>
                </xs:annotation>
                <xs:complexType>
                  <xs:sequence>
                    <xs:annotation>
                      <xs:appinfo>
                        <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                      </xs:appinfo>
                    </xs:annotation>
                    <xs:element name=""Tag46"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""2"" sequence_number=""1"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""SeqNumber46"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""5"" sequence_number=""2"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""BTBOrderNumber"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""13"" sequence_number=""3"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""BTKeyOrdered"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""20"" sequence_number=""4"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""BTKeyReplaced"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""20"" sequence_number=""5"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""Filler46"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""20"" sequence_number=""6"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
              <xs:element minOccurs=""0"" maxOccurs=""1"" name=""LineItem49"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:recordInfo sequence_number=""6"" structure=""delimited"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" child_delimiter_type=""hex"" child_delimiter=""0x0D 0x0A"" child_order=""infix"" />
                  </xs:appinfo>
                </xs:annotation>
                <xs:complexType>
                  <xs:sequence>
                    <xs:annotation>
                      <xs:appinfo>
                        <b:groupInfo sequence_number=""0"" />
                      </xs:appinfo>
                    </xs:annotation>
                    <xs:element minOccurs=""0"" maxOccurs=""1"" name=""POLine49"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:recordInfo structure=""positional"" tag_name=""49"" tag_offset=""0"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""1"" />
                        </xs:appinfo>
                      </xs:annotation>
                      <xs:complexType>
                        <xs:sequence>
                          <xs:annotation>
                            <xs:appinfo>
                              <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                            </xs:appinfo>
                          </xs:annotation>
                          <xs:element name=""Tag49"" type=""xs:string"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""2"" sequence_number=""1"" />
                              </xs:appinfo>
                            </xs:annotation>
                          </xs:element>
                          <xs:element name=""SeqNumber49"" type=""xs:string"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""5"" sequence_number=""2"" />
                              </xs:appinfo>
                            </xs:annotation>
                          </xs:element>
                          <xs:element name=""BTBOrderNumber"" type=""xs:string"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""13"" sequence_number=""3"" />
                              </xs:appinfo>
                            </xs:annotation>
                          </xs:element>
                          <xs:element name=""VASAvailable"" type=""xs:string"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:fieldInfo sequence_number=""4"" justification=""left"" pos_length=""1"" pos_offset=""0"" />
                              </xs:appinfo>
                            </xs:annotation>
                          </xs:element>
                          <xs:element name=""SurchargeItem"" type=""xs:string"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:fieldInfo sequence_number=""5"" justification=""left"" pos_length=""1"" pos_offset=""0"" />
                              </xs:appinfo>
                            </xs:annotation>
                          </xs:element>
                          <xs:element name=""ExtendedWithVAS"" type=""xs:string"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:fieldInfo sequence_number=""6"" justification=""left"" pos_length=""9"" pos_offset=""0"" />
                              </xs:appinfo>
                            </xs:annotation>
                          </xs:element>
                          <xs:element name=""Filler"" type=""xs:string"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""49"" sequence_number=""7"" />
                              </xs:appinfo>
                            </xs:annotation>
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
        <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""Trailer"">
          <xs:annotation>
            <xs:appinfo>
              <recordInfo sequence_number=""3"" structure=""delimited"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" child_delimiter_type=""hex"" child_delimiter=""0x0D 0x0A"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
            </xs:appinfo>
          </xs:annotation>
          <xs:complexType>
            <xs:sequence>
              <xs:annotation>
                <xs:appinfo>
                  <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                </xs:appinfo>
              </xs:annotation>
              <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""Lines58"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:recordInfo structure=""delimited"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" child_order=""infix"" child_delimiter_type=""hex"" child_delimiter=""0x0D 0x0A"" sequence_number=""1"" />
                  </xs:appinfo>
                </xs:annotation>
                <xs:complexType>
                  <xs:sequence>
                    <xs:annotation>
                      <xs:appinfo>
                        <b:groupInfo sequence_number=""0"" />
                      </xs:appinfo>
                    </xs:annotation>
                    <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""POTrailer58"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:recordInfo structure=""positional"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""1"" tag_name=""58"" />
                        </xs:appinfo>
                      </xs:annotation>
                      <xs:complexType>
                        <xs:sequence>
                          <xs:annotation>
                            <xs:appinfo>
                              <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                            </xs:appinfo>
                          </xs:annotation>
                          <xs:element default=""58"" name=""Tag58"" type=""xs:string"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""2"" sequence_number=""1"" />
                              </xs:appinfo>
                            </xs:annotation>
                          </xs:element>
                          <xs:element name=""SeqNumber58"" type=""xs:string"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""5"" sequence_number=""2"" />
                              </xs:appinfo>
                            </xs:annotation>
                          </xs:element>
                          <xs:element name=""BTBOrderNumber"" type=""xs:string"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""13"" sequence_number=""3"" />
                              </xs:appinfo>
                            </xs:annotation>
                          </xs:element>
                          <xs:element name=""VASQuantity"" type=""xs:string"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:fieldInfo sequence_number=""4"" justification=""left"" pos_offset=""0"" pos_length=""5"" />
                              </xs:appinfo>
                            </xs:annotation>
                          </xs:element>
                          <xs:element name=""VASUnitPrice"" type=""xs:string"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:fieldInfo sequence_number=""5"" justification=""left"" pos_length=""9"" pos_offset=""0"" />
                              </xs:appinfo>
                            </xs:annotation>
                          </xs:element>
                          <xs:element name=""VASDescription"" type=""xs:string"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:fieldInfo sequence_number=""6"" justification=""left"" pos_length=""36"" pos_offset=""0"" />
                              </xs:appinfo>
                            </xs:annotation>
                          </xs:element>
                          <xs:element name=""VASBTKey"" type=""xs:string"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:fieldInfo sequence_number=""7"" justification=""left"" pos_length=""10"" pos_offset=""0"" />
                              </xs:appinfo>
                            </xs:annotation>
                          </xs:element>
                        </xs:sequence>
                      </xs:complexType>
                    </xs:element>
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
              <xs:element minOccurs=""1"" maxOccurs=""1"" name=""POTrailer59"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:recordInfo structure=""positional"" tag_name=""59"" tag_offset=""0"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""2"" />
                  </xs:appinfo>
                </xs:annotation>
                <xs:complexType>
                  <xs:sequence>
                    <xs:annotation>
                      <xs:appinfo>
                        <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                      </xs:appinfo>
                    </xs:annotation>
                    <xs:element name=""Tag59"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""2"" sequence_number=""1"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""SeqNumber59"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""5"" sequence_number=""2"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""BTBOrderNumber"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""13"" sequence_number=""3"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""POsInFile"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""5"" sequence_number=""4"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""LinesInPO"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""10"" sequence_number=""5"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""UnitsShipped"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""10"" sequence_number=""6"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""UnitsOrdered"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""5"" sequence_number=""7"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""UnitsBackorderd"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""5"" sequence_number=""8"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""UnitsCancelled"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""5"" sequence_number=""9"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""UnitsNonStocked"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""5"" sequence_number=""10"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""Filler59"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""15"" sequence_number=""11"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
              <xs:element minOccurs=""1"" maxOccurs=""1"" name=""FileTrailer91"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:recordInfo structure=""positional"" tag_name=""91"" tag_offset=""0"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""3"" />
                  </xs:appinfo>
                </xs:annotation>
                <xs:complexType>
                  <xs:sequence>
                    <xs:annotation>
                      <xs:appinfo>
                        <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                      </xs:appinfo>
                    </xs:annotation>
                    <xs:element name=""Tag91"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""2"" sequence_number=""1"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""SeqNumber91"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""5"" sequence_number=""2"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""TotalLineItems"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""13"" sequence_number=""3"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""TotalPOS"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""5"" sequence_number=""4"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""TotalUnitsShipping"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""10"" sequence_number=""5"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""TagCountArea"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""35"" sequence_number=""6"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""Filler91"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""10"" sequence_number=""7"" />
                        </xs:appinfo>
                      </xs:annotation>
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
        
        public POAckBISAC() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "POAck";
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
