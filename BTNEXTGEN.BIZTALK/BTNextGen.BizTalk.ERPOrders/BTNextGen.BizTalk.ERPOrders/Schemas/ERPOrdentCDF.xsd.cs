namespace BTNextGen.BizTalk.ERPOrders.Schemas {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"BTNextGen.BizTalk.ERPOrders.Schemas.ERPOrdentCDF",@"ORDENT")]
    [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.String), "HeadRefInfo.RefInfo.REFNUM.Value", XPath = @"/*[local-name()='ORDENT' and namespace-uri()='BTNextGen.BizTalk.ERPOrders.Schemas.ERPOrdentCDF']/*[local-name()='HeadRefInfo' and namespace-uri()='']/*[local-name()='RefInfo' and namespace-uri()='']/*[local-name()='REFNUM' and namespace-uri()='']/*[local-name()='Value' and namespace-uri()='']", XsdType = @"string")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"ORDENT"})]
    public sealed class ERPOrdentCDF : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns=""BTNextGen.BizTalk.ERPOrders.Schemas.ERPOrdentCDF"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" targetNamespace=""BTNextGen.BizTalk.ERPOrders.Schemas.ERPOrdentCDF"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:annotation>
    <xs:appinfo>
      <schemaEditorExtension:schemaInfo namespaceAlias=""b"" extensionClass=""Microsoft.BizTalk.FlatFileExtension.FlatFileExtension"" standardName=""Flat File"" xmlns:schemaEditorExtension=""http://schemas.microsoft.com/BizTalk/2003/SchemaEditorExtensions"" />
      <b:schemaInfo standard=""Flat File"" codepage=""65001"" default_pad_char="" "" pad_char_type=""char"" count_positions_by_byte=""false"" parser_optimization=""speed"" lookahead_depth=""3"" suppress_empty_nodes=""false"" generate_empty_nodes=""true"" allow_early_termination=""false"" early_terminate_optional_fields=""false"" allow_message_breakup_of_infix_root=""false"" compile_parse_tables=""false"" root_reference=""ORDENT"" />
    </xs:appinfo>
  </xs:annotation>
  <xs:element name=""ORDENT"">
    <xs:annotation>
      <xs:appinfo>
        <b:recordInfo structure=""delimited"" child_delimiter_type=""hex"" child_delimiter=""0x0D 0x0A"" child_order=""postfix"" sequence_number=""1"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" />
        <b:properties>
          <b:property distinguished=""true"" xpath=""/*[local-name()='ORDENT' and namespace-uri()='BTNextGen.BizTalk.ERPOrders.Schemas.ERPOrdentCDF']/*[local-name()='HeadRefInfo' and namespace-uri()='']/*[local-name()='RefInfo' and namespace-uri()='']/*[local-name()='REFNUM' and namespace-uri()='']/*[local-name()='Value' and namespace-uri()='']"" />
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
        <xs:element name=""Begin"">
          <xs:annotation>
            <xs:appinfo>
              <b:recordInfo structure=""delimited"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""1"" />
            </xs:appinfo>
          </xs:annotation>
          <xs:complexType>
            <xs:sequence>
              <xs:annotation>
                <xs:appinfo>
                  <b:groupInfo sequence_number=""0"" />
                </xs:appinfo>
              </xs:annotation>
              <xs:element default=""BEGIN"" name=""BEGIN"" type=""xs:string"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:fieldInfo sequence_number=""1"" justification=""left"" />
                  </xs:appinfo>
                </xs:annotation>
              </xs:element>
              <xs:element default=""ORDENT"" name=""TxnType"" type=""xs:string"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:fieldInfo sequence_number=""2"" justification=""left"" />
                  </xs:appinfo>
                </xs:annotation>
              </xs:element>
              <xs:element name=""TransID"" type=""xs:string"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:fieldInfo sequence_number=""3"" justification=""left"" />
                  </xs:appinfo>
                </xs:annotation>
              </xs:element>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element name=""FileType"">
          <xs:annotation>
            <xs:appinfo>
              <b:recordInfo structure=""delimited"" child_delimiter_type=""char"" child_delimiter="";"" child_order=""infix"" sequence_number=""2"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" />
            </xs:appinfo>
          </xs:annotation>
          <xs:complexType>
            <xs:sequence>
              <xs:annotation>
                <xs:appinfo>
                  <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                </xs:appinfo>
              </xs:annotation>
              <xs:element minOccurs=""0"" name=""LineNumber"" type=""xs:string"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:fieldInfo justification=""right"" sequence_number=""1"" pad_char_type=""char"" pad_char=""0"" min_length_with_pad=""6"" />
                  </xs:appinfo>
                </xs:annotation>
              </xs:element>
              <xs:element minOccurs=""0"" name=""Qualifier"" type=""xs:string"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:fieldInfo justification=""left"" sequence_number=""2"" />
                  </xs:appinfo>
                </xs:annotation>
              </xs:element>
              <xs:element default=""ORDENT"" name=""FileType"" type=""xs:string"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:fieldInfo justification=""left"" sequence_number=""3"" />
                  </xs:appinfo>
                </xs:annotation>
              </xs:element>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element name=""FileHeader"">
          <xs:annotation>
            <xs:appinfo>
              <b:recordInfo structure=""delimited"" child_delimiter_type=""char"" child_delimiter="";"" child_order=""infix"" sequence_number=""3"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" />
            </xs:appinfo>
          </xs:annotation>
          <xs:complexType>
            <xs:sequence>
              <xs:annotation>
                <xs:appinfo>
                  <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                </xs:appinfo>
              </xs:annotation>
              <xs:element name=""LineNumber"" type=""xs:string"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:fieldInfo justification=""right"" sequence_number=""1"" pad_char_type=""char"" pad_char=""0"" min_length_with_pad=""6"" />
                  </xs:appinfo>
                </xs:annotation>
              </xs:element>
              <xs:element name=""HeaderInfo"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:recordInfo structure=""delimited"" child_delimiter_type=""char"" child_delimiter=""/"" child_order=""infix"" sequence_number=""2"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" />
                  </xs:appinfo>
                </xs:annotation>
                <xs:complexType>
                  <xs:sequence>
                    <xs:annotation>
                      <xs:appinfo>
                        <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                      </xs:appinfo>
                    </xs:annotation>
                    <xs:element name=""EDIID"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:recordInfo structure=""delimited"" child_delimiter_type=""char"" child_delimiter=""="" child_order=""infix"" sequence_number=""1"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" />
                        </xs:appinfo>
                      </xs:annotation>
                      <xs:complexType>
                        <xs:sequence>
                          <xs:annotation>
                            <xs:appinfo>
                              <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                            </xs:appinfo>
                          </xs:annotation>
                          <xs:element default=""~EDIID"" name=""Name"" type=""xs:string"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:fieldInfo justification=""left"" sequence_number=""1"" />
                              </xs:appinfo>
                            </xs:annotation>
                          </xs:element>
                          <xs:element name=""Value"" type=""xs:string"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:fieldInfo justification=""left"" sequence_number=""2"" />
                              </xs:appinfo>
                            </xs:annotation>
                          </xs:element>
                        </xs:sequence>
                      </xs:complexType>
                    </xs:element>
                    <xs:element name=""BO"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:recordInfo structure=""delimited"" child_delimiter_type=""char"" child_delimiter=""="" child_order=""infix"" sequence_number=""2"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" />
                        </xs:appinfo>
                      </xs:annotation>
                      <xs:complexType>
                        <xs:sequence>
                          <xs:annotation>
                            <xs:appinfo>
                              <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                            </xs:appinfo>
                          </xs:annotation>
                          <xs:element default=""BO"" name=""Name"" type=""xs:string"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:fieldInfo justification=""left"" sequence_number=""1"" />
                              </xs:appinfo>
                            </xs:annotation>
                          </xs:element>
                          <xs:element minOccurs=""0"" name=""Value"" type=""xs:string"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:fieldInfo justification=""left"" sequence_number=""2"" />
                              </xs:appinfo>
                            </xs:annotation>
                          </xs:element>
                        </xs:sequence>
                      </xs:complexType>
                    </xs:element>
                    <xs:element name=""SHPVIA"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:recordInfo structure=""delimited"" child_delimiter_type=""char"" child_delimiter=""="" child_order=""infix"" sequence_number=""3"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" />
                        </xs:appinfo>
                      </xs:annotation>
                      <xs:complexType>
                        <xs:sequence>
                          <xs:annotation>
                            <xs:appinfo>
                              <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                            </xs:appinfo>
                          </xs:annotation>
                          <xs:element default=""SHPVIA"" name=""Name"" type=""xs:string"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:fieldInfo justification=""left"" sequence_number=""1"" />
                              </xs:appinfo>
                            </xs:annotation>
                          </xs:element>
                          <xs:element name=""Value"" type=""xs:string"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:fieldInfo justification=""left"" sequence_number=""2"" />
                              </xs:appinfo>
                            </xs:annotation>
                          </xs:element>
                        </xs:sequence>
                      </xs:complexType>
                    </xs:element>
                    <xs:element name=""CHGVIA"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:recordInfo structure=""delimited"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""4"" child_delimiter_type=""char"" child_delimiter=""="" child_order=""infix"" />
                        </xs:appinfo>
                      </xs:annotation>
                      <xs:complexType>
                        <xs:sequence>
                          <xs:annotation>
                            <xs:appinfo>
                              <b:groupInfo sequence_number=""0"" />
                            </xs:appinfo>
                          </xs:annotation>
                          <xs:element default=""CHGVIA"" name=""Name"" type=""xs:string"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:fieldInfo sequence_number=""1"" justification=""left"" />
                              </xs:appinfo>
                            </xs:annotation>
                          </xs:element>
                          <xs:element name=""Value"" type=""xs:string"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:fieldInfo sequence_number=""2"" justification=""left"" />
                              </xs:appinfo>
                            </xs:annotation>
                          </xs:element>
                        </xs:sequence>
                      </xs:complexType>
                    </xs:element>
                    <xs:element name=""PONUM"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:recordInfo structure=""delimited"" child_delimiter_type=""char"" child_delimiter=""="" child_order=""infix"" sequence_number=""5"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" />
                        </xs:appinfo>
                      </xs:annotation>
                      <xs:complexType>
                        <xs:sequence>
                          <xs:annotation>
                            <xs:appinfo>
                              <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                            </xs:appinfo>
                          </xs:annotation>
                          <xs:element default=""PONUM"" name=""Name"" type=""xs:string"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:fieldInfo justification=""left"" sequence_number=""1"" />
                              </xs:appinfo>
                            </xs:annotation>
                          </xs:element>
                          <xs:element name=""Value"" type=""xs:string"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:fieldInfo justification=""left"" sequence_number=""2"" />
                              </xs:appinfo>
                            </xs:annotation>
                          </xs:element>
                        </xs:sequence>
                      </xs:complexType>
                    </xs:element>
                    <xs:element name=""POCODE"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:recordInfo structure=""delimited"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""6"" child_delimiter_type=""char"" child_delimiter=""="" />
                        </xs:appinfo>
                      </xs:annotation>
                      <xs:complexType>
                        <xs:sequence>
                          <xs:annotation>
                            <xs:appinfo>
                              <b:groupInfo sequence_number=""0"" />
                            </xs:appinfo>
                          </xs:annotation>
                          <xs:element default=""POCODE"" name=""Name"" type=""xs:string"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:fieldInfo sequence_number=""1"" justification=""left"" />
                              </xs:appinfo>
                            </xs:annotation>
                          </xs:element>
                          <xs:element default=""NE"" name=""Value"" type=""xs:string"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:fieldInfo sequence_number=""2"" justification=""left"" />
                              </xs:appinfo>
                            </xs:annotation>
                          </xs:element>
                        </xs:sequence>
                      </xs:complexType>
                    </xs:element>
                    <xs:element name=""BLK"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:recordInfo structure=""delimited"" child_delimiter_type=""char"" child_delimiter=""="" child_order=""infix"" sequence_number=""7"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" />
                        </xs:appinfo>
                      </xs:annotation>
                      <xs:complexType>
                        <xs:sequence>
                          <xs:annotation>
                            <xs:appinfo>
                              <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                            </xs:appinfo>
                          </xs:annotation>
                          <xs:element default=""BLK"" name=""Name"" type=""xs:string"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:fieldInfo justification=""left"" sequence_number=""1"" />
                              </xs:appinfo>
                            </xs:annotation>
                          </xs:element>
                          <xs:element default=""N"" name=""Value"" type=""xs:string"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:fieldInfo justification=""left"" sequence_number=""2"" />
                              </xs:appinfo>
                            </xs:annotation>
                          </xs:element>
                        </xs:sequence>
                      </xs:complexType>
                    </xs:element>
                    <xs:element name=""EDIADR"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:recordInfo structure=""delimited"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" child_delimiter_type=""char"" child_delimiter=""="" child_order=""infix"" sequence_number=""8"" />
                        </xs:appinfo>
                      </xs:annotation>
                      <xs:complexType>
                        <xs:sequence>
                          <xs:annotation>
                            <xs:appinfo>
                              <b:groupInfo sequence_number=""0"" />
                            </xs:appinfo>
                          </xs:annotation>
                          <xs:element default=""EDIADR"" name=""Name"" type=""xs:string"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:fieldInfo sequence_number=""1"" justification=""left"" />
                              </xs:appinfo>
                            </xs:annotation>
                          </xs:element>
                          <xs:element default=""Y"" name=""Value"" type=""xs:string"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:fieldInfo sequence_number=""2"" justification=""left"" />
                              </xs:appinfo>
                            </xs:annotation>
                          </xs:element>
                        </xs:sequence>
                      </xs:complexType>
                    </xs:element>
                    <xs:element name=""TRANSID"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:recordInfo structure=""delimited"" child_delimiter_type=""char"" child_delimiter=""="" child_order=""infix"" sequence_number=""9"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" />
                        </xs:appinfo>
                      </xs:annotation>
                      <xs:complexType>
                        <xs:sequence>
                          <xs:annotation>
                            <xs:appinfo>
                              <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                            </xs:appinfo>
                          </xs:annotation>
                          <xs:element default=""TRANSID"" name=""Name"" type=""xs:string"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:fieldInfo justification=""left"" sequence_number=""1"" />
                              </xs:appinfo>
                            </xs:annotation>
                          </xs:element>
                          <xs:element name=""Value"" type=""xs:string"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:fieldInfo justification=""left"" sequence_number=""2"" />
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
        <xs:element name=""Date1"">
          <xs:annotation>
            <xs:appinfo>
              <b:recordInfo structure=""delimited"" child_delimiter_type=""char"" child_delimiter="";"" child_order=""infix"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""4"" />
            </xs:appinfo>
          </xs:annotation>
          <xs:complexType>
            <xs:sequence>
              <xs:annotation>
                <xs:appinfo>
                  <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                </xs:appinfo>
              </xs:annotation>
              <xs:element name=""LineNumber"" type=""xs:string"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:fieldInfo justification=""right"" sequence_number=""1"" pad_char_type=""char"" pad_char=""0"" min_length_with_pad=""6"" />
                  </xs:appinfo>
                </xs:annotation>
              </xs:element>
              <xs:element name=""DateInfo"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:recordInfo structure=""delimited"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""2"" child_delimiter_type=""char"" child_delimiter=""/"" />
                  </xs:appinfo>
                </xs:annotation>
                <xs:complexType>
                  <xs:sequence>
                    <xs:annotation>
                      <xs:appinfo>
                        <b:groupInfo sequence_number=""0"" />
                      </xs:appinfo>
                    </xs:annotation>
                    <xs:element name=""DATECD1"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:recordInfo structure=""delimited"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" child_delimiter_type=""char"" child_delimiter=""="" sequence_number=""1"" />
                        </xs:appinfo>
                      </xs:annotation>
                      <xs:complexType>
                        <xs:sequence>
                          <xs:annotation>
                            <xs:appinfo>
                              <b:groupInfo sequence_number=""0"" />
                            </xs:appinfo>
                          </xs:annotation>
                          <xs:element default=""DATECD1"" name=""Name"" type=""xs:string"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:fieldInfo sequence_number=""1"" justification=""left"" />
                              </xs:appinfo>
                            </xs:annotation>
                          </xs:element>
                          <xs:element minOccurs=""0"" name=""Value"" nillable=""true"" type=""xs:string"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:fieldInfo sequence_number=""2"" justification=""left"" />
                              </xs:appinfo>
                            </xs:annotation>
                          </xs:element>
                        </xs:sequence>
                      </xs:complexType>
                    </xs:element>
                    <xs:element name=""DATE1"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:recordInfo structure=""delimited"" child_delimiter_type=""char"" child_delimiter=""="" child_order=""infix"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""2"" />
                        </xs:appinfo>
                      </xs:annotation>
                      <xs:complexType>
                        <xs:sequence>
                          <xs:annotation>
                            <xs:appinfo>
                              <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                            </xs:appinfo>
                          </xs:annotation>
                          <xs:element default=""DATE1"" name=""Name"" type=""xs:string"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:fieldInfo justification=""left"" sequence_number=""1"" />
                              </xs:appinfo>
                            </xs:annotation>
                          </xs:element>
                          <xs:element name=""Value"" type=""xs:string"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:fieldInfo justification=""left"" sequence_number=""2"" />
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
        <xs:element name=""HeadRefInfo"">
          <xs:annotation>
            <xs:appinfo>
              <b:recordInfo structure=""delimited"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""5"" child_delimiter_type=""char"" child_delimiter="";"" />
            </xs:appinfo>
          </xs:annotation>
          <xs:complexType>
            <xs:sequence>
              <xs:annotation>
                <xs:appinfo>
                  <b:groupInfo sequence_number=""0"" />
                </xs:appinfo>
              </xs:annotation>
              <xs:element name=""LineNumber"" type=""xs:string"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:fieldInfo sequence_number=""1"" justification=""right"" pad_char_type=""char"" pad_char=""0"" min_length_with_pad=""6"" />
                  </xs:appinfo>
                </xs:annotation>
              </xs:element>
              <xs:element name=""RefInfo"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:recordInfo sequence_number=""2"" structure=""delimited"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" child_delimiter_type=""char"" child_delimiter=""/"" />
                  </xs:appinfo>
                </xs:annotation>
                <xs:complexType>
                  <xs:sequence>
                    <xs:annotation>
                      <xs:appinfo>
                        <b:groupInfo sequence_number=""0"" />
                      </xs:appinfo>
                    </xs:annotation>
                    <xs:element name=""REFCODE"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:recordInfo sequence_number=""1"" structure=""delimited"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" child_delimiter_type=""char"" child_delimiter=""="" />
                        </xs:appinfo>
                      </xs:annotation>
                      <xs:complexType>
                        <xs:sequence>
                          <xs:annotation>
                            <xs:appinfo>
                              <b:groupInfo sequence_number=""0"" />
                            </xs:appinfo>
                          </xs:annotation>
                          <xs:element default=""REFCODE"" name=""Name"" type=""xs:string"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:fieldInfo sequence_number=""1"" justification=""left"" />
                              </xs:appinfo>
                            </xs:annotation>
                          </xs:element>
                          <xs:element default=""CO"" name=""Value"" type=""xs:string"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:fieldInfo sequence_number=""2"" justification=""left"" />
                              </xs:appinfo>
                            </xs:annotation>
                          </xs:element>
                        </xs:sequence>
                      </xs:complexType>
                    </xs:element>
                    <xs:element name=""REFNUM"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:recordInfo sequence_number=""2"" structure=""delimited"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" child_delimiter_type=""char"" child_delimiter=""="" />
                        </xs:appinfo>
                      </xs:annotation>
                      <xs:complexType>
                        <xs:sequence>
                          <xs:annotation>
                            <xs:appinfo>
                              <b:groupInfo sequence_number=""0"" />
                            </xs:appinfo>
                          </xs:annotation>
                          <xs:element default=""REFNUM"" name=""Name"" type=""xs:string"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:fieldInfo sequence_number=""1"" justification=""left"" />
                              </xs:appinfo>
                            </xs:annotation>
                          </xs:element>
                          <xs:element name=""Value"" type=""xs:string"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:fieldInfo sequence_number=""2"" justification=""left"" />
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
        <xs:element name=""Address0"">
          <xs:annotation>
            <xs:appinfo>
              <b:recordInfo structure=""delimited"" child_delimiter_type=""char"" child_delimiter="";"" child_order=""infix"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""6"" />
            </xs:appinfo>
          </xs:annotation>
          <xs:complexType>
            <xs:sequence>
              <xs:annotation>
                <xs:appinfo>
                  <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                </xs:appinfo>
              </xs:annotation>
              <xs:element name=""LineNumber"" type=""xs:string"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:fieldInfo justification=""right"" sequence_number=""1"" pad_char_type=""char"" pad_char=""0"" min_length_with_pad=""6"" />
                  </xs:appinfo>
                </xs:annotation>
              </xs:element>
              <xs:element name=""EDIENT"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:recordInfo structure=""delimited"" child_delimiter_type=""char"" child_delimiter=""="" child_order=""infix"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""2"" />
                  </xs:appinfo>
                </xs:annotation>
                <xs:complexType>
                  <xs:sequence>
                    <xs:annotation>
                      <xs:appinfo>
                        <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                      </xs:appinfo>
                    </xs:annotation>
                    <xs:element default=""EDIENT"" name=""Name"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""1"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element default=""ST"" name=""Value"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""2"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element name=""Address1"">
          <xs:annotation>
            <xs:appinfo>
              <b:recordInfo structure=""delimited"" child_delimiter_type=""char"" child_delimiter="";"" child_order=""infix"" sequence_number=""7"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" />
            </xs:appinfo>
          </xs:annotation>
          <xs:complexType>
            <xs:sequence>
              <xs:annotation>
                <xs:appinfo>
                  <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                </xs:appinfo>
              </xs:annotation>
              <xs:element name=""LineNumber"" type=""xs:string"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:fieldInfo justification=""right"" sequence_number=""1"" pad_char_type=""char"" min_length_with_pad=""6"" pad_char=""0"" />
                  </xs:appinfo>
                </xs:annotation>
              </xs:element>
              <xs:element name=""EDINAM"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:recordInfo structure=""delimited"" child_delimiter_type=""char"" child_delimiter=""="" child_order=""infix"" sequence_number=""2"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" />
                  </xs:appinfo>
                </xs:annotation>
                <xs:complexType>
                  <xs:sequence>
                    <xs:annotation>
                      <xs:appinfo>
                        <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                      </xs:appinfo>
                    </xs:annotation>
                    <xs:element default=""EDINAM"" name=""Name"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""1"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""Value"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""2"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element name=""Address2"">
          <xs:annotation>
            <xs:appinfo>
              <b:recordInfo structure=""delimited"" child_delimiter_type=""char"" child_delimiter="";"" child_order=""infix"" sequence_number=""8"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" />
            </xs:appinfo>
          </xs:annotation>
          <xs:complexType>
            <xs:sequence>
              <xs:annotation>
                <xs:appinfo>
                  <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                </xs:appinfo>
              </xs:annotation>
              <xs:element name=""LineNumber"" type=""xs:string"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:fieldInfo justification=""right"" sequence_number=""1"" pad_char_type=""char"" min_length_with_pad=""6"" pad_char=""0"" />
                  </xs:appinfo>
                </xs:annotation>
              </xs:element>
              <xs:element name=""EDIAD1"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:recordInfo structure=""delimited"" child_delimiter_type=""char"" child_delimiter=""="" child_order=""infix"" sequence_number=""2"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" />
                  </xs:appinfo>
                </xs:annotation>
                <xs:complexType>
                  <xs:sequence>
                    <xs:annotation>
                      <xs:appinfo>
                        <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                      </xs:appinfo>
                    </xs:annotation>
                    <xs:element default=""EDIAD1"" name=""Name"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""1"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""Value"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""2"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element name=""Address3"">
          <xs:annotation>
            <xs:appinfo>
              <b:recordInfo structure=""delimited"" child_delimiter_type=""char"" child_delimiter="";"" child_order=""infix"" sequence_number=""9"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" />
            </xs:appinfo>
          </xs:annotation>
          <xs:complexType>
            <xs:sequence>
              <xs:annotation>
                <xs:appinfo>
                  <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                </xs:appinfo>
              </xs:annotation>
              <xs:element name=""LineNumber"" type=""xs:string"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:fieldInfo justification=""right"" sequence_number=""1"" pad_char_type=""char"" pad_char=""0"" min_length_with_pad=""6"" />
                  </xs:appinfo>
                </xs:annotation>
              </xs:element>
              <xs:element name=""EDIAD2"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:recordInfo structure=""delimited"" child_delimiter_type=""char"" child_delimiter=""="" child_order=""infix"" sequence_number=""2"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" />
                  </xs:appinfo>
                </xs:annotation>
                <xs:complexType>
                  <xs:sequence>
                    <xs:annotation>
                      <xs:appinfo>
                        <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                      </xs:appinfo>
                    </xs:annotation>
                    <xs:element default=""EDIAD2"" name=""Name"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""1"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element minOccurs=""0"" name=""Value"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""2"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element name=""Address5"">
          <xs:annotation>
            <xs:appinfo>
              <b:recordInfo structure=""delimited"" child_delimiter_type=""char"" child_delimiter="";"" child_order=""infix"" sequence_number=""10"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" />
            </xs:appinfo>
          </xs:annotation>
          <xs:complexType>
            <xs:sequence>
              <xs:annotation>
                <xs:appinfo>
                  <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                </xs:appinfo>
              </xs:annotation>
              <xs:element name=""LineNumber"" type=""xs:string"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:fieldInfo justification=""right"" sequence_number=""1"" pad_char_type=""char"" pad_char=""0"" min_length_with_pad=""6"" />
                  </xs:appinfo>
                </xs:annotation>
              </xs:element>
              <xs:element name=""CitStZipCou"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:recordInfo sequence_number=""2"" structure=""delimited"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" child_delimiter_type=""char"" child_delimiter=""/"" />
                  </xs:appinfo>
                </xs:annotation>
                <xs:complexType>
                  <xs:sequence>
                    <xs:annotation>
                      <xs:appinfo>
                        <b:groupInfo sequence_number=""0"" />
                      </xs:appinfo>
                    </xs:annotation>
                    <xs:element name=""EDICIT"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:recordInfo structure=""delimited"" child_delimiter_type=""char"" child_delimiter=""="" child_order=""infix"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""1"" />
                        </xs:appinfo>
                      </xs:annotation>
                      <xs:complexType>
                        <xs:sequence>
                          <xs:annotation>
                            <xs:appinfo>
                              <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                            </xs:appinfo>
                          </xs:annotation>
                          <xs:element default=""EDICIT"" name=""Name"" type=""xs:string"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:fieldInfo justification=""left"" sequence_number=""1"" />
                              </xs:appinfo>
                            </xs:annotation>
                          </xs:element>
                          <xs:element name=""Value"" type=""xs:string"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:fieldInfo justification=""left"" sequence_number=""2"" />
                              </xs:appinfo>
                            </xs:annotation>
                          </xs:element>
                        </xs:sequence>
                      </xs:complexType>
                    </xs:element>
                    <xs:element name=""EDISTA"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:recordInfo structure=""delimited"" child_delimiter_type=""char"" child_delimiter=""="" child_order=""infix"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""2"" />
                        </xs:appinfo>
                      </xs:annotation>
                      <xs:complexType>
                        <xs:sequence>
                          <xs:annotation>
                            <xs:appinfo>
                              <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                            </xs:appinfo>
                          </xs:annotation>
                          <xs:element default=""EDISTA"" name=""Name"" type=""xs:string"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:fieldInfo justification=""left"" sequence_number=""1"" />
                              </xs:appinfo>
                            </xs:annotation>
                          </xs:element>
                          <xs:element name=""Value"" type=""xs:string"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:fieldInfo justification=""left"" sequence_number=""2"" />
                              </xs:appinfo>
                            </xs:annotation>
                          </xs:element>
                        </xs:sequence>
                      </xs:complexType>
                    </xs:element>
                    <xs:element name=""EDIZIP"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:recordInfo structure=""delimited"" child_delimiter_type=""char"" child_delimiter=""="" child_order=""infix"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""3"" />
                        </xs:appinfo>
                      </xs:annotation>
                      <xs:complexType>
                        <xs:sequence>
                          <xs:annotation>
                            <xs:appinfo>
                              <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                            </xs:appinfo>
                          </xs:annotation>
                          <xs:element default=""EDIZIP"" name=""Name"" type=""xs:string"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:fieldInfo justification=""left"" sequence_number=""1"" />
                              </xs:appinfo>
                            </xs:annotation>
                          </xs:element>
                          <xs:element name=""Value"" type=""xs:string"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:fieldInfo justification=""left"" sequence_number=""2"" />
                              </xs:appinfo>
                            </xs:annotation>
                          </xs:element>
                        </xs:sequence>
                      </xs:complexType>
                    </xs:element>
                    <xs:element name=""EDICOU"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:recordInfo structure=""delimited"" child_delimiter_type=""char"" child_delimiter=""="" child_order=""infix"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""4"" />
                        </xs:appinfo>
                      </xs:annotation>
                      <xs:complexType>
                        <xs:sequence>
                          <xs:annotation>
                            <xs:appinfo>
                              <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                            </xs:appinfo>
                          </xs:annotation>
                          <xs:element default=""EDICOU"" name=""Name"" type=""xs:string"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:fieldInfo justification=""left"" sequence_number=""1"" />
                              </xs:appinfo>
                            </xs:annotation>
                          </xs:element>
                          <xs:element name=""Value"" type=""xs:string"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:fieldInfo justification=""left"" sequence_number=""2"" />
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
        <xs:element maxOccurs=""unbounded"" name=""Details"">
          <xs:annotation>
            <xs:appinfo>
              <b:recordInfo structure=""delimited"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""11"" child_delimiter_type=""hex"" child_delimiter=""0x0D 0x0A"" />
            </xs:appinfo>
          </xs:annotation>
          <xs:complexType>
            <xs:sequence minOccurs=""0"" maxOccurs=""unbounded"">
              <xs:annotation>
                <xs:appinfo>
                  <b:groupInfo sequence_number=""0"" />
                </xs:appinfo>
              </xs:annotation>
              <xs:element name=""LineItemUPC"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:recordInfo structure=""delimited"" child_delimiter_type=""char"" child_delimiter="";"" child_order=""infix"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""1"" />
                  </xs:appinfo>
                </xs:annotation>
                <xs:complexType>
                  <xs:sequence>
                    <xs:annotation>
                      <xs:appinfo>
                        <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                      </xs:appinfo>
                    </xs:annotation>
                    <xs:element name=""LineNumber"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""right"" sequence_number=""1"" pad_char_type=""char"" pad_char=""0"" min_length_with_pad=""6"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""LineItem"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:recordInfo structure=""delimited"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""2"" child_delimiter_type=""char"" child_delimiter=""/"" />
                        </xs:appinfo>
                      </xs:annotation>
                      <xs:complexType>
                        <xs:sequence>
                          <xs:annotation>
                            <xs:appinfo>
                              <b:groupInfo sequence_number=""0"" />
                            </xs:appinfo>
                          </xs:annotation>
                          <xs:element name=""CUSLIN"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:recordInfo structure=""delimited"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""1"" child_delimiter_type=""char"" child_delimiter=""="" />
                              </xs:appinfo>
                            </xs:annotation>
                            <xs:complexType>
                              <xs:sequence>
                                <xs:annotation>
                                  <xs:appinfo>
                                    <b:groupInfo sequence_number=""0"" />
                                  </xs:appinfo>
                                </xs:annotation>
                                <xs:element default=""CUSLIN"" name=""Name"" type=""xs:string"">
                                  <xs:annotation>
                                    <xs:appinfo>
                                      <b:fieldInfo sequence_number=""1"" justification=""left"" />
                                    </xs:appinfo>
                                  </xs:annotation>
                                </xs:element>
                                <xs:element name=""Value"" type=""xs:string"">
                                  <xs:annotation>
                                    <xs:appinfo>
                                      <b:fieldInfo sequence_number=""2"" justification=""left"" />
                                    </xs:appinfo>
                                  </xs:annotation>
                                </xs:element>
                              </xs:sequence>
                            </xs:complexType>
                          </xs:element>
                          <xs:element name=""QTY"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:recordInfo structure=""delimited"" child_delimiter_type=""char"" child_delimiter=""="" child_order=""infix"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""2"" />
                              </xs:appinfo>
                            </xs:annotation>
                            <xs:complexType>
                              <xs:sequence>
                                <xs:annotation>
                                  <xs:appinfo>
                                    <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                                  </xs:appinfo>
                                </xs:annotation>
                                <xs:element default=""QTY"" name=""Name"" type=""xs:string"">
                                  <xs:annotation>
                                    <xs:appinfo>
                                      <b:fieldInfo justification=""left"" sequence_number=""1"" />
                                    </xs:appinfo>
                                  </xs:annotation>
                                </xs:element>
                                <xs:element name=""Value"" type=""xs:string"">
                                  <xs:annotation>
                                    <xs:appinfo>
                                      <b:fieldInfo justification=""left"" sequence_number=""2"" />
                                    </xs:appinfo>
                                  </xs:annotation>
                                </xs:element>
                              </xs:sequence>
                            </xs:complexType>
                          </xs:element>
                          <xs:element name=""UPC"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:recordInfo structure=""delimited"" child_delimiter_type=""char"" child_delimiter=""="" child_order=""infix"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""3"" />
                              </xs:appinfo>
                            </xs:annotation>
                            <xs:complexType>
                              <xs:sequence>
                                <xs:annotation>
                                  <xs:appinfo>
                                    <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                                  </xs:appinfo>
                                </xs:annotation>
                                <xs:element default=""EDIITM"" name=""Name"" type=""xs:string"">
                                  <xs:annotation>
                                    <xs:appinfo>
                                      <b:fieldInfo justification=""left"" sequence_number=""1"" />
                                    </xs:appinfo>
                                  </xs:annotation>
                                </xs:element>
                                <xs:element minOccurs=""0"" name=""Value"" type=""xs:string"">
                                  <xs:annotation>
                                    <xs:appinfo>
                                      <b:fieldInfo justification=""left"" sequence_number=""2"" />
                                    </xs:appinfo>
                                  </xs:annotation>
                                </xs:element>
                              </xs:sequence>
                            </xs:complexType>
                          </xs:element>
                          <xs:element name=""PO"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:recordInfo structure=""delimited"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""4"" child_delimiter_type=""char"" child_delimiter=""="" />
                              </xs:appinfo>
                            </xs:annotation>
                            <xs:complexType>
                              <xs:sequence>
                                <xs:annotation>
                                  <xs:appinfo>
                                    <b:groupInfo sequence_number=""0"" />
                                  </xs:appinfo>
                                </xs:annotation>
                                <xs:element default=""PO"" name=""Name"" type=""xs:string"">
                                  <xs:annotation>
                                    <xs:appinfo>
                                      <b:fieldInfo sequence_number=""1"" justification=""left"" />
                                    </xs:appinfo>
                                  </xs:annotation>
                                </xs:element>
                                <xs:element name=""Value"" type=""xs:string"">
                                  <xs:annotation>
                                    <xs:appinfo>
                                      <b:fieldInfo sequence_number=""2"" justification=""left"" />
                                    </xs:appinfo>
                                  </xs:annotation>
                                </xs:element>
                              </xs:sequence>
                            </xs:complexType>
                          </xs:element>
                          <xs:element name=""DETPRICE"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:recordInfo structure=""delimited"" child_delimiter_type=""char"" child_delimiter=""="" child_order=""infix"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""5"" />
                              </xs:appinfo>
                            </xs:annotation>
                            <xs:complexType>
                              <xs:sequence>
                                <xs:annotation>
                                  <xs:appinfo>
                                    <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                                  </xs:appinfo>
                                </xs:annotation>
                                <xs:element default=""DETPRICE"" name=""Name"" type=""xs:string"">
                                  <xs:annotation>
                                    <xs:appinfo>
                                      <b:fieldInfo justification=""left"" sequence_number=""1"" />
                                    </xs:appinfo>
                                  </xs:annotation>
                                </xs:element>
                                <xs:element name=""Value"" type=""xs:string"">
                                  <xs:annotation>
                                    <xs:appinfo>
                                      <b:fieldInfo justification=""left"" sequence_number=""2"" />
                                    </xs:appinfo>
                                  </xs:annotation>
                                </xs:element>
                              </xs:sequence>
                            </xs:complexType>
                          </xs:element>
                          <xs:element name=""BO"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:recordInfo structure=""delimited"" child_delimiter_type=""char"" child_delimiter=""="" child_order=""infix"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""6"" />
                              </xs:appinfo>
                            </xs:annotation>
                            <xs:complexType>
                              <xs:sequence>
                                <xs:annotation>
                                  <xs:appinfo>
                                    <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                                  </xs:appinfo>
                                </xs:annotation>
                                <xs:element default=""BO"" name=""Name"" type=""xs:string"">
                                  <xs:annotation>
                                    <xs:appinfo>
                                      <b:fieldInfo justification=""left"" sequence_number=""1"" />
                                    </xs:appinfo>
                                  </xs:annotation>
                                </xs:element>
                                <xs:element minOccurs=""0"" name=""Value"" type=""xs:string"">
                                  <xs:annotation>
                                    <xs:appinfo>
                                      <b:fieldInfo justification=""left"" sequence_number=""2"" />
                                    </xs:appinfo>
                                  </xs:annotation>
                                </xs:element>
                              </xs:sequence>
                            </xs:complexType>
                          </xs:element>
                          <xs:element name=""PROMOCD"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:recordInfo sequence_number=""7"" structure=""delimited"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" child_delimiter_type=""char"" child_delimiter=""="" />
                              </xs:appinfo>
                            </xs:annotation>
                            <xs:complexType>
                              <xs:sequence>
                                <xs:annotation>
                                  <xs:appinfo>
                                    <b:groupInfo sequence_number=""0"" />
                                  </xs:appinfo>
                                </xs:annotation>
                                <xs:element default=""PROMOCD"" name=""Name"" type=""xs:string"">
                                  <xs:annotation>
                                    <xs:appinfo>
                                      <b:fieldInfo sequence_number=""1"" justification=""left"" />
                                    </xs:appinfo>
                                  </xs:annotation>
                                </xs:element>
                                <xs:element name=""Value"" type=""xs:string"">
                                  <xs:annotation>
                                    <xs:appinfo>
                                      <b:fieldInfo sequence_number=""2"" justification=""left"" />
                                    </xs:appinfo>
                                  </xs:annotation>
                                </xs:element>
                              </xs:sequence>
                            </xs:complexType>
                          </xs:element>
                          <xs:element name=""PROMOPR"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:recordInfo sequence_number=""8"" structure=""delimited"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" child_delimiter_type=""char"" child_delimiter=""="" />
                              </xs:appinfo>
                            </xs:annotation>
                            <xs:complexType>
                              <xs:sequence>
                                <xs:annotation>
                                  <xs:appinfo>
                                    <b:groupInfo sequence_number=""0"" />
                                  </xs:appinfo>
                                </xs:annotation>
                                <xs:element default=""PROMOPR"" name=""Name"" type=""xs:string"">
                                  <xs:annotation>
                                    <xs:appinfo>
                                      <b:fieldInfo sequence_number=""1"" justification=""left"" />
                                    </xs:appinfo>
                                  </xs:annotation>
                                </xs:element>
                                <xs:element name=""Value"" type=""xs:string"">
                                  <xs:annotation>
                                    <xs:appinfo>
                                      <b:fieldInfo sequence_number=""2"" justification=""left"" />
                                    </xs:appinfo>
                                  </xs:annotation>
                                </xs:element>
                              </xs:sequence>
                            </xs:complexType>
                          </xs:element>
                          <xs:element name=""WRPCOD"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:recordInfo sequence_number=""9"" structure=""delimited"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" child_delimiter_type=""char"" child_delimiter=""="" />
                              </xs:appinfo>
                            </xs:annotation>
                            <xs:complexType>
                              <xs:sequence>
                                <xs:annotation>
                                  <xs:appinfo>
                                    <b:groupInfo sequence_number=""0"" />
                                  </xs:appinfo>
                                </xs:annotation>
                                <xs:element default=""WRPCOD"" name=""Name"" type=""xs:string"">
                                  <xs:annotation>
                                    <xs:appinfo>
                                      <b:fieldInfo sequence_number=""1"" justification=""left"" />
                                    </xs:appinfo>
                                  </xs:annotation>
                                </xs:element>
                                <xs:element name=""Value"" type=""xs:string"">
                                  <xs:annotation>
                                    <xs:appinfo>
                                      <b:fieldInfo sequence_number=""2"" justification=""left"" />
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
              <xs:element name=""Reference"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:recordInfo structure=""delimited"" child_delimiter_type=""char"" child_delimiter="";"" child_order=""infix"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""2"" />
                  </xs:appinfo>
                </xs:annotation>
                <xs:complexType>
                  <xs:sequence>
                    <xs:annotation>
                      <xs:appinfo>
                        <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                      </xs:appinfo>
                    </xs:annotation>
                    <xs:element name=""LineNumber"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""right"" sequence_number=""1"" pad_char_type=""char"" pad_char=""0"" min_length_with_pad=""6"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""RefQualifiers"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:recordInfo sequence_number=""2"" structure=""delimited"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" child_delimiter_type=""char"" child_delimiter=""/"" />
                        </xs:appinfo>
                      </xs:annotation>
                      <xs:complexType>
                        <xs:sequence>
                          <xs:annotation>
                            <xs:appinfo>
                              <b:groupInfo sequence_number=""0"" />
                            </xs:appinfo>
                          </xs:annotation>
                          <xs:element name=""DETCHGIND"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:recordInfo structure=""delimited"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""1"" child_delimiter_type=""char"" child_delimiter=""="" />
                              </xs:appinfo>
                            </xs:annotation>
                            <xs:complexType>
                              <xs:sequence>
                                <xs:annotation>
                                  <xs:appinfo>
                                    <b:groupInfo sequence_number=""0"" />
                                  </xs:appinfo>
                                </xs:annotation>
                                <xs:element default=""DETCHGIND"" name=""Name"" type=""xs:string"">
                                  <xs:annotation>
                                    <xs:appinfo>
                                      <b:fieldInfo sequence_number=""1"" justification=""left"" />
                                    </xs:appinfo>
                                  </xs:annotation>
                                </xs:element>
                                <xs:element default=""N"" name=""Value"" type=""xs:string"">
                                  <xs:annotation>
                                    <xs:appinfo>
                                      <b:fieldInfo sequence_number=""2"" justification=""left"" />
                                    </xs:appinfo>
                                  </xs:annotation>
                                </xs:element>
                              </xs:sequence>
                            </xs:complexType>
                          </xs:element>
                          <xs:element name=""DETREFQLR"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:recordInfo structure=""delimited"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" child_delimiter_type=""char"" child_delimiter=""="" child_order=""infix"" sequence_number=""2"" />
                              </xs:appinfo>
                            </xs:annotation>
                            <xs:complexType>
                              <xs:sequence>
                                <xs:annotation>
                                  <xs:appinfo>
                                    <b:groupInfo sequence_number=""0"" />
                                  </xs:appinfo>
                                </xs:annotation>
                                <xs:element default=""DETREFQLR"" name=""Name"" type=""xs:string"">
                                  <xs:annotation>
                                    <xs:appinfo>
                                      <b:fieldInfo sequence_number=""1"" justification=""left"" />
                                    </xs:appinfo>
                                  </xs:annotation>
                                </xs:element>
                                <xs:element default=""QY"" name=""Value"" type=""xs:string"">
                                  <xs:annotation>
                                    <xs:appinfo>
                                      <b:fieldInfo sequence_number=""2"" justification=""left"" />
                                    </xs:appinfo>
                                  </xs:annotation>
                                </xs:element>
                              </xs:sequence>
                            </xs:complexType>
                          </xs:element>
                          <xs:element name=""DETREFID"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:recordInfo structure=""delimited"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" child_delimiter_type=""char"" child_delimiter=""="" child_order=""infix"" sequence_number=""3"" />
                              </xs:appinfo>
                            </xs:annotation>
                            <xs:complexType>
                              <xs:sequence>
                                <xs:annotation>
                                  <xs:appinfo>
                                    <b:groupInfo sequence_number=""0"" />
                                  </xs:appinfo>
                                </xs:annotation>
                                <xs:element default=""DETREFID"" name=""Name"" type=""xs:string"">
                                  <xs:annotation>
                                    <xs:appinfo>
                                      <b:fieldInfo sequence_number=""1"" justification=""left"" />
                                    </xs:appinfo>
                                  </xs:annotation>
                                </xs:element>
                                <xs:element default=""VGP"" name=""Value"" type=""xs:string"">
                                  <xs:annotation>
                                    <xs:appinfo>
                                      <b:fieldInfo sequence_number=""2"" justification=""left"" />
                                    </xs:appinfo>
                                  </xs:annotation>
                                </xs:element>
                              </xs:sequence>
                            </xs:complexType>
                          </xs:element>
                          <xs:element name=""DETREFDSC"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:recordInfo structure=""delimited"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" child_delimiter_type=""char"" child_delimiter=""="" child_order=""infix"" sequence_number=""4"" />
                              </xs:appinfo>
                            </xs:annotation>
                            <xs:complexType>
                              <xs:sequence>
                                <xs:annotation>
                                  <xs:appinfo>
                                    <b:groupInfo sequence_number=""0"" />
                                  </xs:appinfo>
                                </xs:annotation>
                                <xs:element default=""DETREFDSC"" name=""Name"" type=""xs:string"">
                                  <xs:annotation>
                                    <xs:appinfo>
                                      <b:fieldInfo sequence_number=""1"" justification=""left"" />
                                    </xs:appinfo>
                                  </xs:annotation>
                                </xs:element>
                                <xs:element minOccurs=""0"" name=""Value"" type=""xs:string"">
                                  <xs:annotation>
                                    <xs:appinfo>
                                      <b:fieldInfo sequence_number=""2"" justification=""left"" />
                                    </xs:appinfo>
                                  </xs:annotation>
                                </xs:element>
                              </xs:sequence>
                            </xs:complexType>
                          </xs:element>
                          <xs:element name=""DETREFIDF"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:recordInfo structure=""delimited"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" child_delimiter_type=""char"" child_delimiter=""="" child_order=""infix"" sequence_number=""5"" />
                              </xs:appinfo>
                            </xs:annotation>
                            <xs:complexType>
                              <xs:sequence>
                                <xs:annotation>
                                  <xs:appinfo>
                                    <b:groupInfo sequence_number=""0"" />
                                  </xs:appinfo>
                                </xs:annotation>
                                <xs:element default=""DETREFIDF"" name=""Name"" type=""xs:string"">
                                  <xs:annotation>
                                    <xs:appinfo>
                                      <b:fieldInfo sequence_number=""1"" justification=""left"" />
                                    </xs:appinfo>
                                  </xs:annotation>
                                </xs:element>
                                <xs:element default=""2"" name=""Value"" type=""xs:string"">
                                  <xs:annotation>
                                    <xs:appinfo>
                                      <b:fieldInfo sequence_number=""2"" justification=""left"" />
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
              <xs:element name=""GiftMessage"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:recordInfo structure=""delimited"" child_delimiter_type=""char"" child_delimiter="";"" child_order=""infix"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""3"" />
                  </xs:appinfo>
                </xs:annotation>
                <xs:complexType>
                  <xs:sequence>
                    <xs:annotation>
                      <xs:appinfo>
                        <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                      </xs:appinfo>
                    </xs:annotation>
                    <xs:element name=""LineNumber"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""right"" sequence_number=""1"" pad_char_type=""char"" pad_char=""0"" min_length_with_pad=""6"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""DETREFMSG"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:recordInfo structure=""delimited"" child_delimiter_type=""char"" child_delimiter=""="" child_order=""infix"" sequence_number=""2"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" />
                        </xs:appinfo>
                      </xs:annotation>
                      <xs:complexType>
                        <xs:sequence>
                          <xs:annotation>
                            <xs:appinfo>
                              <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                            </xs:appinfo>
                          </xs:annotation>
                          <xs:element default=""DETREFMSG"" name=""Name"" type=""xs:string"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:fieldInfo justification=""left"" sequence_number=""1"" />
                              </xs:appinfo>
                            </xs:annotation>
                          </xs:element>
                          <xs:element name=""Value"" type=""xs:string"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:fieldInfo justification=""left"" sequence_number=""2"" />
                              </xs:appinfo>
                            </xs:annotation>
                          </xs:element>
                        </xs:sequence>
                      </xs:complexType>
                    </xs:element>
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
              <xs:element name=""GiftMessage2"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:recordInfo sequence_number=""4"" structure=""delimited"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" child_delimiter_type=""char"" child_delimiter="";"" child_order=""infix"" />
                  </xs:appinfo>
                </xs:annotation>
                <xs:complexType>
                  <xs:sequence>
                    <xs:annotation>
                      <xs:appinfo>
                        <b:groupInfo sequence_number=""0"" />
                      </xs:appinfo>
                    </xs:annotation>
                    <xs:element name=""LineNumber"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""right"" pad_char_type=""char"" pad_char=""0"" min_length_with_pad=""6"" sequence_number=""1"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""DETREFMSG"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:recordInfo structure=""delimited"" child_delimiter_type=""char"" child_delimiter=""="" child_order=""infix"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""2"" />
                        </xs:appinfo>
                      </xs:annotation>
                      <xs:complexType>
                        <xs:sequence>
                          <xs:annotation>
                            <xs:appinfo>
                              <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                            </xs:appinfo>
                          </xs:annotation>
                          <xs:element default=""DETREFMSG"" name=""Name"" type=""xs:string"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:fieldInfo justification=""left"" sequence_number=""1"" />
                              </xs:appinfo>
                            </xs:annotation>
                          </xs:element>
                          <xs:element name=""Value"" type=""xs:string"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:fieldInfo justification=""left"" sequence_number=""2"" />
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
        <xs:element name=""FileFooter"">
          <xs:annotation>
            <xs:appinfo>
              <b:recordInfo structure=""delimited"" child_delimiter_type=""char"" child_delimiter="";"" child_order=""infix"" sequence_number=""12"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" />
            </xs:appinfo>
          </xs:annotation>
          <xs:complexType>
            <xs:sequence>
              <xs:annotation>
                <xs:appinfo>
                  <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                </xs:appinfo>
              </xs:annotation>
              <xs:element minOccurs=""0"" default=""999999"" name=""LineNumber"" type=""xs:string"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:fieldInfo justification=""right"" sequence_number=""1"" pad_char_type=""char"" pad_char=""0"" min_length_with_pad=""6"" />
                  </xs:appinfo>
                </xs:annotation>
              </xs:element>
              <xs:element name=""TotalLines"" type=""xs:string"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:fieldInfo justification=""right"" sequence_number=""2"" pad_char_type=""char"" pad_char=""0"" min_length_with_pad=""6"" />
                  </xs:appinfo>
                </xs:annotation>
              </xs:element>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element name=""End"">
          <xs:annotation>
            <xs:appinfo>
              <b:recordInfo sequence_number=""13"" structure=""delimited"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" />
            </xs:appinfo>
          </xs:annotation>
          <xs:complexType>
            <xs:sequence>
              <xs:annotation>
                <xs:appinfo>
                  <b:groupInfo sequence_number=""0"" />
                </xs:appinfo>
              </xs:annotation>
              <xs:element default=""END"" name=""END"" type=""xs:string"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:fieldInfo sequence_number=""1"" justification=""left"" />
                  </xs:appinfo>
                </xs:annotation>
              </xs:element>
              <xs:element default=""ORDENT"" name=""TxnType"" type=""xs:string"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:fieldInfo sequence_number=""2"" justification=""left"" />
                  </xs:appinfo>
                </xs:annotation>
              </xs:element>
              <xs:element name=""TransID"" type=""xs:string"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:fieldInfo sequence_number=""3"" justification=""left"" />
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
        
        public ERPOrdentCDF() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "ORDENT";
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
