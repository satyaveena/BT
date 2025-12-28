namespace BTNextGen.BizTalk.ERPOrders.Schemas {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"BTNextGen.BizTalk.ERPOrders.Schemas.POAck855",@"POAck")]
    [Microsoft.XLANGs.BaseTypes.DistinguishedFieldAttribute(typeof(System.String), "Header.BAK.BAK08TransmissionID", XPath = @"/*[local-name()='POAck' and namespace-uri()='BTNextGen.BizTalk.ERPOrders.Schemas.POAck855']/*[local-name()='Header' and namespace-uri()='']/*[local-name()='BAK' and namespace-uri()='']/*[local-name()='BAK08TransmissionID' and namespace-uri()='']", XsdType = @"string")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"POAck"})]
    public sealed class POAck855 : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns=""BTNextGen.BizTalk.ERPOrders.Schemas.POAck855"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" targetNamespace=""BTNextGen.BizTalk.ERPOrders.Schemas.POAck855"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:annotation>
    <xs:appinfo>
      <schemaEditorExtension:schemaInfo namespaceAlias=""b"" extensionClass=""Microsoft.BizTalk.FlatFileExtension.FlatFileExtension"" standardName=""Flat File"" xmlns:schemaEditorExtension=""http://schemas.microsoft.com/BizTalk/2003/SchemaEditorExtensions"" />
      <b:schemaInfo standard=""Flat File"" codepage=""65001"" default_pad_char="" "" pad_char_type=""char"" count_positions_by_byte=""false"" parser_optimization=""speed"" lookahead_depth=""3"" suppress_empty_nodes=""false"" generate_empty_nodes=""true"" allow_early_termination=""false"" early_terminate_optional_fields=""false"" allow_message_breakup_of_infix_root=""false"" compile_parse_tables=""false"" root_reference=""POAck"" />
    </xs:appinfo>
  </xs:annotation>
  <xs:element name=""POAck"">
    <xs:annotation>
      <xs:appinfo>
        <b:recordInfo structure=""delimited"" child_delimiter_type=""hex"" child_delimiter=""0x0D 0x0A"" child_order=""infix"" sequence_number=""1"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" />
        <b:properties>
          <b:property distinguished=""true"" xpath=""/*[local-name()='POAck' and namespace-uri()='BTNextGen.BizTalk.ERPOrders.Schemas.POAck855']/*[local-name()='Header' and namespace-uri()='']/*[local-name()='BAK' and namespace-uri()='']/*[local-name()='BAK08TransmissionID' and namespace-uri()='']"" />
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
              <xs:element name=""BEGIN"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:recordInfo structure=""positional"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""1"" tag_name=""BEGIN"" />
                  </xs:appinfo>
                </xs:annotation>
                <xs:complexType>
                  <xs:sequence>
                    <xs:annotation>
                      <xs:appinfo>
                        <b:groupInfo sequence_number=""0"" />
                      </xs:appinfo>
                    </xs:annotation>
                    <xs:element name=""BEGINTag"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo sequence_number=""1"" justification=""left"" pos_length=""5"" pos_offset=""0"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""TxnType"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo sequence_number=""2"" justification=""left"" pos_length=""3"" pos_offset=""0"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""TransmissionID"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo sequence_number=""3"" justification=""left"" pos_length=""7"" pos_offset=""0"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""Unused"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""4"" pos_length=""8"" pos_offset=""0"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""Date"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo sequence_number=""5"" justification=""left"" pos_length=""8"" pos_offset=""0"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""Time"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo sequence_number=""6"" justification=""left"" pos_length=""8"" pos_offset=""0"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""Filler"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo sequence_number=""7"" justification=""left"" pos_length=""8"" pos_offset=""0"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
              <xs:element name=""ISA"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:recordInfo tag_name=""ISA"" structure=""delimited"" child_delimiter_type=""char"" child_delimiter=""*"" child_order=""prefix"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""2"" />
                  </xs:appinfo>
                </xs:annotation>
                <xs:complexType>
                  <xs:sequence>
                    <xs:annotation>
                      <xs:appinfo>
                        <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                      </xs:appinfo>
                    </xs:annotation>
                    <xs:element name=""ISA01"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""1"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""ISA02"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""2"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""ISA03"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""3"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""ISA04"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""4"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""ISA05"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""5"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""ISA06EDIID"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""6"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""ISA07"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""7"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""ISA08Sender"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""8"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""ISA09Date"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""9"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""ISA10Time"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""10"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""ISA11"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""11"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""ISA12"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""12"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""ISA13"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""13"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""ISA14"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""14"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""ISA15"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""15"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""ISA16"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""16"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
              <xs:element name=""GS"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:recordInfo tag_name=""GS"" structure=""delimited"" child_delimiter_type=""char"" child_delimiter=""*"" child_order=""prefix"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""3"" />
                  </xs:appinfo>
                </xs:annotation>
                <xs:complexType>
                  <xs:sequence>
                    <xs:annotation>
                      <xs:appinfo>
                        <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                      </xs:appinfo>
                    </xs:annotation>
                    <xs:element name=""GS01"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""1"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""GS02EDIAccount"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""2"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""GS03"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""3"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""GS04Date"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""4"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""GS05Time"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""5"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""GS06"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""6"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""GS07"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""7"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""GS08"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""8"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
              <xs:element name=""ST"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:recordInfo tag_name=""ST"" structure=""delimited"" child_delimiter_type=""char"" child_delimiter=""*"" child_order=""prefix"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""4"" />
                  </xs:appinfo>
                </xs:annotation>
                <xs:complexType>
                  <xs:sequence>
                    <xs:annotation>
                      <xs:appinfo>
                        <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                      </xs:appinfo>
                    </xs:annotation>
                    <xs:element name=""ST01"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""1"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""ST02"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""2"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
              <xs:element name=""BAK"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:recordInfo tag_name=""BAK"" structure=""delimited"" child_delimiter_type=""char"" child_delimiter=""*"" child_order=""prefix"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""5"" />
                  </xs:appinfo>
                </xs:annotation>
                <xs:complexType>
                  <xs:sequence>
                    <xs:annotation>
                      <xs:appinfo>
                        <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                      </xs:appinfo>
                    </xs:annotation>
                    <xs:element name=""BAK01ReasonCode"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""1"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""BAK02StatusCode"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""2"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""BAK03PONumber"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""3"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""BAK04ActionCode"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""4"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""BAK05InventoryQty"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""5"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""BAK06ExpectedShipDate"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""6"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""BAK07"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""7"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""BAK08TransmissionID"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""8"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""BAK09"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""9"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
              <xs:element minOccurs=""0"" maxOccurs=""2"" name=""N1"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:recordInfo tag_name=""N1"" structure=""delimited"" child_delimiter_type=""char"" child_delimiter=""*"" child_order=""prefix"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""6"" />
                  </xs:appinfo>
                </xs:annotation>
                <xs:complexType>
                  <xs:sequence>
                    <xs:annotation>
                      <xs:appinfo>
                        <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                      </xs:appinfo>
                    </xs:annotation>
                    <xs:element minOccurs=""0"" name=""N101BTST"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""1"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element minOccurs=""0"" name=""N102Name"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""2"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
              <xs:element minOccurs=""0"" maxOccurs=""2"" name=""N2"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:recordInfo tag_name=""N2"" structure=""delimited"" child_delimiter_type=""char"" child_delimiter=""*"" child_order=""prefix"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""7"" />
                  </xs:appinfo>
                </xs:annotation>
                <xs:complexType>
                  <xs:sequence>
                    <xs:annotation>
                      <xs:appinfo>
                        <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                      </xs:appinfo>
                    </xs:annotation>
                    <xs:element minOccurs=""0"" name=""N201Addr1"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""1"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
              <xs:element minOccurs=""0"" maxOccurs=""2"" name=""N3"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:recordInfo structure=""delimited"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""8"" child_delimiter_type=""char"" child_delimiter=""*"" tag_name=""N3"" child_order=""prefix"" />
                  </xs:appinfo>
                </xs:annotation>
                <xs:complexType>
                  <xs:sequence>
                    <xs:annotation>
                      <xs:appinfo>
                        <b:groupInfo sequence_number=""0"" />
                      </xs:appinfo>
                    </xs:annotation>
                    <xs:element minOccurs=""0"" name=""N301Addr2"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""1"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
              <xs:element minOccurs=""0"" maxOccurs=""2"" name=""N4"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:recordInfo tag_name=""N4"" structure=""delimited"" child_delimiter_type=""char"" child_delimiter=""*"" child_order=""prefix"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""9"" />
                  </xs:appinfo>
                </xs:annotation>
                <xs:complexType>
                  <xs:sequence>
                    <xs:annotation>
                      <xs:appinfo>
                        <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                      </xs:appinfo>
                    </xs:annotation>
                    <xs:element minOccurs=""0"" name=""N401City"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""1"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element minOccurs=""0"" name=""N402State"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""2"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element minOccurs=""0"" name=""N403Zip"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""3"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element minOccurs=""0"" name=""N404Country"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""4"" />
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
              <xs:element maxOccurs=""1"" name=""PO1"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:recordInfo tag_name=""PO1"" structure=""delimited"" child_delimiter_type=""char"" child_delimiter=""*"" child_order=""prefix"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""1"" />
                  </xs:appinfo>
                </xs:annotation>
                <xs:complexType>
                  <xs:sequence>
                    <xs:annotation>
                      <xs:appinfo>
                        <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                      </xs:appinfo>
                    </xs:annotation>
                    <xs:element name=""PO101MerchLineNumer"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""1"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""PO102OrderedQTY"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""2"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""PO103"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""3"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""PO104Price"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""4"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""PO105"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""5"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""PO106SKUQualifier"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""6"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""PO107SKUId"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""7"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""PO108"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""8"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""PO109Description"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""9"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""PO110"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""10"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""PO111"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""11"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""PO112NumQualifier"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""12"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""PO113PONumber"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""13"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""PO114NumQualifer"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo sequence_number=""14"" justification=""left"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""PO115OSN-OLN-LAN"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo sequence_number=""15"" justification=""left"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""PO116Empty"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo sequence_number=""16"" justification=""left"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
              <xs:element maxOccurs=""1"" name=""Status"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:recordInfo structure=""delimited"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" child_delimiter_type=""hex"" child_delimiter=""0x0D 0x0A"" sequence_number=""2"" />
                  </xs:appinfo>
                </xs:annotation>
                <xs:complexType>
                  <xs:sequence>
                    <xs:annotation>
                      <xs:appinfo>
                        <b:groupInfo sequence_number=""0"" />
                      </xs:appinfo>
                    </xs:annotation>
                    <xs:element maxOccurs=""unbounded"" name=""ACK"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:recordInfo tag_name=""ACK"" structure=""delimited"" child_delimiter_type=""char"" child_delimiter=""*"" child_order=""prefix"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""1"" />
                        </xs:appinfo>
                      </xs:annotation>
                      <xs:complexType>
                        <xs:sequence>
                          <xs:annotation>
                            <xs:appinfo>
                              <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                            </xs:appinfo>
                          </xs:annotation>
                          <xs:element name=""ACK01StatusCode"" type=""xs:string"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:fieldInfo justification=""left"" sequence_number=""1"" />
                              </xs:appinfo>
                            </xs:annotation>
                          </xs:element>
                          <xs:element name=""ACK02StatusQTY"" type=""xs:string"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:fieldInfo justification=""left"" sequence_number=""2"" />
                              </xs:appinfo>
                            </xs:annotation>
                          </xs:element>
                          <xs:element name=""ACK03"" type=""xs:string"">
                            <xs:annotation>
                              <xs:appinfo>
                                <b:fieldInfo justification=""left"" sequence_number=""3"" />
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
        <xs:element name=""Trailer"">
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
              <xs:element name=""CTT"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:recordInfo tag_name=""CTT"" structure=""delimited"" child_delimiter_type=""char"" child_delimiter=""*"" child_order=""prefix"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""1"" />
                  </xs:appinfo>
                </xs:annotation>
                <xs:complexType>
                  <xs:sequence>
                    <xs:annotation>
                      <xs:appinfo>
                        <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                      </xs:appinfo>
                    </xs:annotation>
                    <xs:element name=""CTT01"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""1"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""CTT02"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""2"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""CTT03"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""3"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""CTT04"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""4"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""CTT05"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""5"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""CTT06"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo sequence_number=""6"" justification=""left"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
              <xs:element name=""SE"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:recordInfo tag_name=""SE"" structure=""delimited"" child_delimiter_type=""char"" child_delimiter=""*"" child_order=""prefix"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""2"" />
                  </xs:appinfo>
                </xs:annotation>
                <xs:complexType>
                  <xs:sequence>
                    <xs:annotation>
                      <xs:appinfo>
                        <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                      </xs:appinfo>
                    </xs:annotation>
                    <xs:element name=""SE01"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""1"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""SE02"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""2"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
              <xs:element name=""GE"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:recordInfo tag_name=""GE"" structure=""delimited"" child_delimiter_type=""char"" child_delimiter=""*"" child_order=""prefix"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""3"" />
                  </xs:appinfo>
                </xs:annotation>
                <xs:complexType>
                  <xs:sequence>
                    <xs:annotation>
                      <xs:appinfo>
                        <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                      </xs:appinfo>
                    </xs:annotation>
                    <xs:element name=""GE01"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""1"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""GE02"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""2"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
              <xs:element name=""IEA"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:recordInfo tag_name=""IEA"" structure=""delimited"" child_delimiter_type=""char"" child_delimiter=""*"" child_order=""prefix"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" sequence_number=""4"" />
                  </xs:appinfo>
                </xs:annotation>
                <xs:complexType>
                  <xs:sequence>
                    <xs:annotation>
                      <xs:appinfo>
                        <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
                      </xs:appinfo>
                    </xs:annotation>
                    <xs:element name=""IEA01"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""1"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""IEA02"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""2"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
              <xs:element name=""END"">
                <xs:annotation>
                  <xs:appinfo>
                    <b:recordInfo structure=""positional"" tag_name=""END"" tag_offset=""0"" sequence_number=""5"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" />
                  </xs:appinfo>
                </xs:annotation>
                <xs:complexType>
                  <xs:sequence>
                    <xs:annotation>
                      <xs:appinfo>
                        <b:groupInfo sequence_number=""0"" />
                      </xs:appinfo>
                    </xs:annotation>
                    <xs:element name=""EndTag"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""3"" sequence_number=""1"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""TxnType"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""3"" sequence_number=""2"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""TransmissionNumber"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""7"" sequence_number=""3"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""Unused"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" sequence_number=""4"" pos_length=""8"" pos_offset=""0"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""Date"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""8"" sequence_number=""5"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""Time"" type=""xs:string"">
                      <xs:annotation>
                        <xs:appinfo>
                          <b:fieldInfo justification=""left"" pos_offset=""0"" pos_length=""8"" sequence_number=""6"" />
                        </xs:appinfo>
                      </xs:annotation>
                    </xs:element>
                    <xs:element name=""Filler"" type=""xs:string"">
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
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public POAck855() {
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
