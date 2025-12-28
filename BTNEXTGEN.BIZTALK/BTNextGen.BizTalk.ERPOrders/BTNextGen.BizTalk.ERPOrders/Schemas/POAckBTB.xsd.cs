namespace BTNextGen.BizTalk.ERPOrders.Schemas {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"BTNextGen.BizTalk.ERPOrders.Schemas.POAckBTB",@"POAckBTB")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"POAckBTB"})]
    public sealed class POAckBTB : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns=""BTNextGen.BizTalk.ERPOrders.Schemas.POAckBTB"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" targetNamespace=""BTNextGen.BizTalk.ERPOrders.Schemas.POAckBTB"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:annotation>
    <xs:appinfo>
      <schemaEditorExtension:schemaInfo namespaceAlias=""b"" extensionClass=""Microsoft.BizTalk.FlatFileExtension.FlatFileExtension"" standardName=""Flat File"" xmlns:schemaEditorExtension=""http://schemas.microsoft.com/BizTalk/2003/SchemaEditorExtensions"" />
      <b:schemaInfo standard=""Flat File"" codepage=""65001"" default_pad_char="" "" pad_char_type=""char"" count_positions_by_byte=""false"" parser_optimization=""speed"" lookahead_depth=""3"" suppress_empty_nodes=""false"" generate_empty_nodes=""true"" allow_early_termination=""false"" early_terminate_optional_fields=""false"" allow_message_breakup_of_infix_root=""false"" compile_parse_tables=""false"" root_reference=""POAckBTB"" child_delimiter_type=""hex"" default_child_delimiter=""0x0D 0x0A"" />
    </xs:appinfo>
  </xs:annotation>
  <xs:element name=""POAckBTB"">
    <xs:annotation>
      <xs:appinfo>
        <b:recordInfo structure=""delimited"" child_delimiter_type=""hex"" child_delimiter=""0xD 0xA"" child_order=""postfix"" sequence_number=""1"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" />
      </xs:appinfo>
    </xs:annotation>
    <xs:complexType>
      <xs:sequence>
        <xs:annotation>
          <xs:appinfo>
            <b:groupInfo sequence_number=""0"" />
          </xs:appinfo>
        </xs:annotation>
        <xs:element name=""Header"">
          <xs:annotation>
            <xs:appinfo>
              <b:recordInfo sequence_number=""1"" structure=""delimited"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" child_delimiter_type=""default"" />
            </xs:appinfo>
          </xs:annotation>
          <xs:complexType>
            <xs:sequence>
              <xs:annotation>
                <xs:appinfo>
                  <b:groupInfo sequence_number=""0"" />
                </xs:appinfo>
              </xs:annotation>
              <xs:element name=""FileHeader02"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:recordInfo structure=""positional"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""1"" />
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
                    <xs:element name=""FileArchiveDate"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""6"" sequence_number=""6"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""NameOfFile"" type=""xs:string"">
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
                    <xs:element name=""DestinationSAN02"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""7"" sequence_number=""9"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""DestinationSuffix02"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""5"" sequence_number=""10"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""AckTypeFlag"" type=""xs:string"">
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
                    <b:recordInfo structure=""positional"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""2"" />
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
                    <xs:element name=""CustomerSAN"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""7"" sequence_number=""5"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""CustomerSuffix"" type=""xs:string"">
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
                    <xs:element name=""POAckDate"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""6"" sequence_number=""9"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""Currency"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""3"" sequence_number=""10"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""PODate"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""6"" sequence_number=""11"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""POCancelDate"" type=""xs:string"">
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
                    <b:recordInfo structure=""positional"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""3"" />
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
                    <xs:element name=""CustomerNuber"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""12"" sequence_number=""4"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""Filler"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""48"" sequence_number=""5"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
              <xs:element name=""POHeader21"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:recordInfo structure=""positional"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""4"" />
                  </xs:appinfo>
                </xs:annotation>
                <xs:complexType>
                  <xs:sequence>
                    <xs:annotation>
                      <xs:appinfo>
                        <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                      </xs:appinfo>
                    </xs:annotation>
                    <xs:element name=""Tag21"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""2"" sequence_number=""1"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""SeqNumber21"" type=""xs:string"">
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
                    <xs:element name=""Message"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""60"" sequence_number=""4"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
              <xs:element name=""POHeader25"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:recordInfo structure=""positional"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""5"" />
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
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""25"" sequence_number=""5"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""Filler"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""10"" sequence_number=""6"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
              <xs:element name=""POHeader26"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:recordInfo structure=""positional"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""6"" />
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
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""25"" sequence_number=""5"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""Filler"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""10"" sequence_number=""6"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
              <xs:element name=""POHeader27"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:recordInfo structure=""positional"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""7"" />
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
                    <xs:element name=""BTBorderNumber"" type=""xs:string"">
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
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""25"" sequence_number=""6"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""Filler27"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""10"" sequence_number=""7"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
              <xs:element name=""POHeader30"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:recordInfo structure=""positional"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""8"" />
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
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""25"" sequence_number=""5"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""Filler30"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""10"" sequence_number=""6"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
              <xs:element name=""POHeader31"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:recordInfo structure=""positional"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""9"" />
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
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""25"" sequence_number=""5"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""Filler31"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""10"" sequence_number=""6"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
              <xs:element name=""POHeader32"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:recordInfo structure=""positional"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""10"" />
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
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""25"" sequence_number=""6"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""Filler32"" type=""xs:string"">
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
        <xs:element name=""Details"">
          <xs:annotation>
            <xs:appinfo>
              <b:recordInfo structure=""delimited"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" child_delimiter_type=""default"" sequence_number=""2"" />
            </xs:appinfo>
          </xs:annotation>
          <xs:complexType>
            <xs:sequence>
              <xs:annotation>
                <xs:appinfo>
                  <b:groupInfo sequence_number=""0"" />
                </xs:appinfo>
              </xs:annotation>
              <xs:sequence>
                <xs:annotation>
                  <xs:appinfo>
                    <b:groupInfo sequence_number=""1"" />
                  </xs:appinfo>
                </xs:annotation>
                <xs:element name=""POLine40"">
                  <xs:annotation>
                    <xs:appinfo>
                      <b:recordInfo structure=""positional"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""1"" />
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
                      <xs:element name=""LineNumber"" type=""xs:string"">
                        <xs:annotation>
                          <xs:appinfo>
                            <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""20"" sequence_number=""4"" />
                          </xs:appinfo>
                        </xs:annotation>
                      </xs:element>
                      <xs:element name=""OrderQty"" type=""xs:string"">
                        <xs:annotation>
                          <xs:appinfo>
                            <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""5"" sequence_number=""5"" />
                          </xs:appinfo>
                        </xs:annotation>
                      </xs:element>
                      <xs:element name=""ListPrice"" type=""xs:string"">
                        <xs:annotation>
                          <xs:appinfo>
                            <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""8"" sequence_number=""6"" />
                          </xs:appinfo>
                        </xs:annotation>
                      </xs:element>
                      <xs:element name=""NetPrice"" type=""xs:string"">
                        <xs:annotation>
                          <xs:appinfo>
                            <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""9"" sequence_number=""7"" />
                          </xs:appinfo>
                        </xs:annotation>
                      </xs:element>
                      <xs:element name=""ListNetFlag"" type=""xs:string"">
                        <xs:annotation>
                          <xs:appinfo>
                            <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""1"" sequence_number=""8"" />
                          </xs:appinfo>
                        </xs:annotation>
                      </xs:element>
                      <xs:element name=""SpecialPriceIndicator"" type=""xs:string"">
                        <xs:annotation>
                          <xs:appinfo>
                            <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""1"" sequence_number=""9"" />
                          </xs:appinfo>
                        </xs:annotation>
                      </xs:element>
                      <xs:element name=""DiscountPercent"" type=""xs:string"">
                        <xs:annotation>
                          <xs:appinfo>
                            <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""5"" sequence_number=""10"" />
                          </xs:appinfo>
                        </xs:annotation>
                      </xs:element>
                      <xs:element name=""ShipQty"" type=""xs:string"">
                        <xs:annotation>
                          <xs:appinfo>
                            <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""5"" sequence_number=""11"" />
                          </xs:appinfo>
                        </xs:annotation>
                      </xs:element>
                      <xs:element name=""StatusCode"" type=""xs:string"">
                        <xs:annotation>
                          <xs:appinfo>
                            <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""2"" sequence_number=""12"" />
                          </xs:appinfo>
                        </xs:annotation>
                      </xs:element>
                      <xs:element name=""StatusWHSCode"" type=""xs:string"">
                        <xs:annotation>
                          <xs:appinfo>
                            <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""2"" sequence_number=""13"" />
                          </xs:appinfo>
                        </xs:annotation>
                      </xs:element>
                      <xs:element name=""Filler40"" type=""xs:string"">
                        <xs:annotation>
                          <xs:appinfo>
                            <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""2"" sequence_number=""14"" />
                          </xs:appinfo>
                        </xs:annotation>
                      </xs:element>
                    </xs:sequence>
                  </xs:complexType>
                </xs:element>
                <xs:element name=""POLine41"">
                  <xs:annotation>
                    <xs:appinfo>
                      <b:recordInfo structure=""positional"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""2"" />
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
                      <xs:element name=""ShipQty"" type=""xs:string"">
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
                      <xs:element name=""DateAvailable"" type=""xs:string"">
                        <xs:annotation>
                          <xs:appinfo>
                            <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""6"" sequence_number=""6"" />
                          </xs:appinfo>
                        </xs:annotation>
                      </xs:element>
                      <xs:element name=""StatusCode"" type=""xs:string"">
                        <xs:annotation>
                          <xs:appinfo>
                            <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""3"" sequence_number=""7"" />
                          </xs:appinfo>
                        </xs:annotation>
                      </xs:element>
                      <xs:element name=""ISBNOrdered"" type=""xs:string"">
                        <xs:annotation>
                          <xs:appinfo>
                            <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""9"" sequence_number=""8"" />
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
                      <xs:element name=""FreeForm"" type=""xs:string"">
                        <xs:annotation>
                          <xs:appinfo>
                            <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""27"" sequence_number=""10"" />
                          </xs:appinfo>
                        </xs:annotation>
                      </xs:element>
                      <xs:element name=""QtySubstitutionFlag"" type=""xs:string"">
                        <xs:annotation>
                          <xs:appinfo>
                            <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""1"" sequence_number=""11"" />
                          </xs:appinfo>
                        </xs:annotation>
                      </xs:element>
                      <xs:element name=""Filler41"" type=""xs:string"">
                        <xs:annotation>
                          <xs:appinfo>
                            <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""4"" sequence_number=""12"" />
                          </xs:appinfo>
                        </xs:annotation>
                      </xs:element>
                    </xs:sequence>
                  </xs:complexType>
                </xs:element>
                <xs:element name=""POLine42"">
                  <xs:annotation>
                    <xs:appinfo>
                      <b:recordInfo structure=""positional"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""3"" />
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
                <xs:element name=""POLine43"">
                  <xs:annotation>
                    <xs:appinfo>
                      <b:recordInfo structure=""positional"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""4"" />
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
                <xs:element name=""POLine45"">
                  <xs:annotation>
                    <xs:appinfo>
                      <b:recordInfo structure=""positional"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""5"" />
                    </xs:appinfo>
                  </xs:annotation>
                  <xs:complexType>
                    <xs:sequence>
                      <xs:annotation>
                        <xs:appinfo>
                          <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                        </xs:appinfo>
                      </xs:annotation>
                      <xs:element name=""Tag45"" type=""xs:string"">
                        <xs:annotation>
                          <xs:appinfo>
                            <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""2"" sequence_number=""1"" />
                          </xs:appinfo>
                        </xs:annotation>
                      </xs:element>
                      <xs:element name=""SeqNumber45"" type=""xs:string"">
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
                      <xs:element name=""CPQPriceArea"" type=""xs:string"">
                        <xs:annotation>
                          <xs:appinfo>
                            <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""20"" sequence_number=""4"" />
                          </xs:appinfo>
                        </xs:annotation>
                      </xs:element>
                      <xs:element name=""GTINQualifierP"" type=""xs:string"">
                        <xs:annotation>
                          <xs:appinfo>
                            <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""4"" sequence_number=""5"" />
                          </xs:appinfo>
                        </xs:annotation>
                      </xs:element>
                      <xs:element name=""GTINPrimary"" type=""xs:string"">
                        <xs:annotation>
                          <xs:appinfo>
                            <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""16"" sequence_number=""6"" />
                          </xs:appinfo>
                        </xs:annotation>
                      </xs:element>
                      <xs:element name=""GTINQualifierS"" type=""xs:string"">
                        <xs:annotation>
                          <xs:appinfo>
                            <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""4"" sequence_number=""7"" />
                          </xs:appinfo>
                        </xs:annotation>
                      </xs:element>
                      <xs:element name=""GTINSecondary"" type=""xs:string"">
                        <xs:annotation>
                          <xs:appinfo>
                            <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""16"" sequence_number=""8"" />
                          </xs:appinfo>
                        </xs:annotation>
                      </xs:element>
                    </xs:sequence>
                  </xs:complexType>
                </xs:element>
                <xs:element minOccurs=""19"" maxOccurs=""19"" name=""POLine46"">
                  <xs:annotation>
                    <xs:appinfo>
                      <b:recordInfo structure=""positional"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""6"" />
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
                      <xs:element name=""BTKeyReplacement"" type=""xs:string"">
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
              </xs:sequence>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element name=""Footer"">
          <xs:annotation>
            <xs:appinfo>
              <b:recordInfo sequence_number=""3"" structure=""delimited"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" child_delimiter_type=""default"" />
            </xs:appinfo>
          </xs:annotation>
          <xs:complexType>
            <xs:sequence>
              <xs:annotation>
                <xs:appinfo>
                  <b:groupInfo sequence_number=""0"" />
                </xs:appinfo>
              </xs:annotation>
              <xs:element name=""POTrailer59"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:recordInfo structure=""positional"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""1"" />
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
                    <xs:element name=""RecordCount"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""5"" sequence_number=""4"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""LineCount"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""10"" sequence_number=""5"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""UnitsShippable"" type=""xs:string"">
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
                    <xs:element name=""UnitsBackordered"" type=""xs:string"">
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
              <xs:element name=""FileTrailer91"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:recordInfo structure=""positional"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""2"" />
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
                    <xs:element name=""TotalLines"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""13"" sequence_number=""3"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""TotalPOs"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""5"" sequence_number=""4"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""TotalUnitsShipable"" type=""xs:string"">
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
                    <xs:element name=""Filler"" type=""xs:string"">
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
        
        public POAckBTB() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "POAckBTB";
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
