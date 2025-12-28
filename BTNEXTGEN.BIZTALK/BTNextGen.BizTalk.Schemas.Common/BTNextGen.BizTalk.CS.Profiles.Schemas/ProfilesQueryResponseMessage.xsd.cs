namespace BTNextGen.BizTalk.CS.Profiles.Schemas {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"ProfileDocumentList", @"CommerceServerProfilesQueryResponse"})]
    public sealed class ProfilesQueryResponseMessage : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" attributeFormDefault=""unqualified"" elementFormDefault=""qualified"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""ProfileDocumentList"">
    <xs:complexType>
      <xs:sequence>
        <xs:element maxOccurs=""unbounded"" name=""ProfileDocument"">
          <xs:complexType>
            <xs:sequence>
              <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""Address"">
                <xs:complexType>
                  <xs:sequence>
                    <xs:element name=""GeneralInfo"">
                      <xs:complexType>
                        <xs:sequence>
                          <xs:element name=""address_id"" type=""xs:string"" />
                          <xs:element name=""last_name"" type=""xs:string"" />
                          <xs:element name=""first_name"" type=""xs:string"" />
                          <xs:element name=""address_name"" type=""xs:string"" />
                          <xs:element name=""address_type"" type=""xs:unsignedByte"" />
                          <xs:element name=""description"" type=""xs:string"" />
                          <xs:element name=""address_line1"" type=""xs:string"" />
                          <xs:element name=""address_line2"" type=""xs:string"" />
                          <xs:element name=""city"" type=""xs:string"" />
                          <xs:element name=""region_code"" type=""xs:string"" />
                          <xs:element name=""postal_code"" type=""xs:unsignedInt"" />
                          <xs:element name=""country_code"" type=""xs:string"" />
                          <xs:element name=""tel_number"" type=""xs:string"" />
                          <xs:element name=""tel_extension"" type=""xs:unsignedShort"" />
                          <xs:element name=""locale"" type=""xs:unsignedShort"" />
                        </xs:sequence>
                      </xs:complexType>
                    </xs:element>
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
              <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""BlanketPOs"">
                <xs:complexType>
                  <xs:sequence>
                    <xs:element name=""GeneralInfo"">
                      <xs:complexType>
                        <xs:sequence>
                          <xs:element name=""po_id"" type=""xs:string"" />
                          <xs:element name=""org_id"" type=""xs:string"" />
                          <xs:element name=""po_number"" type=""xs:string"" />
                          <xs:element name=""description"" type=""xs:string"" />
                        </xs:sequence>
                      </xs:complexType>
                    </xs:element>
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
              <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""CreditCard"">
                <xs:complexType>
                  <xs:sequence>
                    <xs:element name=""GeneralInfo"">
                      <xs:complexType>
                        <xs:sequence>
                          <xs:element name=""id"" type=""xs:string"" />
                          <xs:element minOccurs=""0"" maxOccurs=""1"" name=""payment_group_id"" type=""xs:string"" />
                          <xs:element minOccurs=""1"" maxOccurs=""1"" name=""cc_number"" type=""xs:string"" />
                          <xs:element minOccurs=""1"" maxOccurs=""1"" name=""last_4_digits"" type=""xs:string"" />
                          <xs:element minOccurs=""1"" maxOccurs=""1"" name=""billing_address"" type=""xs:string"" />
                          <xs:element minOccurs=""1"" maxOccurs=""1"" name=""expiration_month"" type=""xs:int"" />
                          <xs:element minOccurs=""1"" maxOccurs=""1"" name=""expiration_year"" type=""xs:int"" />
                        </xs:sequence>
                      </xs:complexType>
                    </xs:element>
                    <xs:element minOccurs=""0"" maxOccurs=""1"" name=""ProfileSystem"">
                      <xs:complexType>
                        <xs:sequence>
                          <xs:element minOccurs=""0"" maxOccurs=""1"" name=""KeyIndex"" type=""xs:string"" />
                          <xs:element minOccurs=""0"" maxOccurs=""1"" name=""user_id_changed_by"" type=""xs:string"" />
                          <xs:element minOccurs=""0"" maxOccurs=""1"" name=""date_last_changed"" type=""xs:string"" />
                          <xs:element minOccurs=""0"" maxOccurs=""1"" name=""date_created"" type=""xs:string"" />
                          <xs:element minOccurs=""0"" maxOccurs=""1"" name=""csadapter_date_last_changed"" type=""xs:string"" />
                        </xs:sequence>
                      </xs:complexType>
                    </xs:element>
                    <xs:element minOccurs=""0"" maxOccurs=""1"" name=""BTNextGen"">
                      <xs:complexType>
                        <xs:sequence>
                          <xs:element minOccurs=""0"" maxOccurs=""1"" name=""credit_card_token"" type=""xs:string"" />
                          <xs:element minOccurs=""0"" maxOccurs=""1"" name=""alias"" type=""xs:string"" />
                          <xs:element minOccurs=""0"" maxOccurs=""1"" name=""primary_indicator"" type=""xs:string"" />
                          <xs:element minOccurs=""0"" maxOccurs=""1"" name=""credit_card_type"" type=""xs:string"" />
                          <xs:element minOccurs=""0"" maxOccurs=""1"" name=""transmitted_to_erp"" type=""xs:string"" />
                          <xs:element minOccurs=""0"" maxOccurs=""1"" name=""payment_status"" type=""xs:string"" />
                          <xs:element minOccurs=""0"" maxOccurs=""1"" name=""notification_status"" type=""xs:string"" />
                          <xs:element minOccurs=""0"" maxOccurs=""1"" name=""card_contact_user"" type=""xs:string"" />
                          <xs:element minOccurs=""0"" maxOccurs=""1"" name=""date_last_failed_notification"" type=""xs:string"" />
                          <xs:element minOccurs=""0"" maxOccurs=""1"" name=""date_last_preemptive_notification"" type=""xs:string"" />
                          <xs:element minOccurs=""0"" maxOccurs=""1"" name=""u_subscription_id"" type=""xs:string"" />
                          <xs:element minOccurs=""0"" maxOccurs=""1"" name=""u_subscription_id_public_signature"" type=""xs:string"" />
                        </xs:sequence>
                      </xs:complexType>
                    </xs:element>
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
              <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""Currency"">
                <xs:complexType>
                  <xs:sequence>
                    <xs:element name=""GeneralInfo"">
                      <xs:complexType>
                        <xs:sequence>
                          <xs:element name=""currency_code"" type=""xs:string"" />
                          <xs:element name=""currency_culture"" type=""xs:string"" />
                          <xs:element name=""conversion_factor"" type=""xs:decimal"" />
                        </xs:sequence>
                      </xs:complexType>
                    </xs:element>
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
              <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""Organization"">
                <xs:complexType>
                  <xs:sequence>
                    <xs:element name=""GeneralInfo"">
                      <xs:complexType>
                        <xs:sequence>
                          <xs:element name=""org_id"" type=""xs:string"" />
                          <xs:element name=""name"" type=""xs:string"" />
                          <xs:element name=""trading_partner_number"" type=""xs:string"" />
                          <xs:element name=""user_id_receiver"" type=""xs:string"" />
                          <xs:element name=""preferred_address"" type=""xs:string"" />
                          <xs:element name=""address_list"" type=""xs:string"" />
                          <xs:element name=""org_catalog_set"" type=""xs:string"" />
                          <xs:element name=""purchasing"" type=""xs:string"" />
                        </xs:sequence>
                      </xs:complexType>
                    </xs:element>
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
              <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""UserObject"">
                <xs:complexType>
                  <xs:sequence>
                    <xs:element name=""AccountInfo"">
                      <xs:complexType>
                        <xs:sequence>
                          <xs:element name=""org_id"" type=""xs:string"" />
                          <xs:element name=""account_status"" type=""xs:unsignedByte"" />
                          <xs:element name=""user_catalog_set"" type=""xs:string"" />
                          <xs:element name=""date_registered"" type=""xs:dateTime"" />
                        </xs:sequence>
                      </xs:complexType>
                    </xs:element>
                    <xs:element name=""GeneralInfo"">
                      <xs:complexType>
                        <xs:sequence>
                          <xs:element name=""user_id"" type=""xs:string"" />
                          <xs:element name=""user_security_password"" type=""xs:string"" />
                          <xs:element name=""email_address"" type=""xs:string"" />
                          <xs:element name=""preferred_address"" type=""xs:string"" />
                          <xs:element name=""address_list"" type=""xs:string"" />
                          <xs:element name=""credit_card_list"" type=""xs:string"" />
                          <xs:element name=""user_type"" type=""xs:unsignedByte"" />
                          <xs:element name=""last_name"" type=""xs:string"" />
                          <xs:element name=""first_name"" type=""xs:string"" />
                          <xs:element name=""tel_number"" type=""xs:string"" />
                          <xs:element name=""tel_extension"" type=""xs:string"" />
                          <xs:element name=""fax_number"" type=""xs:string"" />
                          <xs:element name=""fax_extension"" type=""xs:string"" />
                          <xs:element name=""language"" type=""xs:string"" />
                          <xs:element name=""password_question"" type=""xs:string"" />
                          <xs:element name=""password_answer"" type=""xs:string"" />
                          <xs:element name=""direct_mail_opt_out"" type=""xs:unsignedByte"" />
                          <xs:element name=""express_checkout"" type=""xs:unsignedByte"" />
                        </xs:sequence>
                      </xs:complexType>
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
  <xs:element name=""CommerceServerProfilesQueryResponse"">
    <xs:complexType>
      <xs:sequence>
        <xs:element name=""ProfileSearchResults"">
          <xs:complexType>
            <xs:sequence>
              <xs:element maxOccurs=""unbounded"" name=""SearchResults"">
                <xs:complexType>
                  <xs:sequence>
                    <xs:element name=""GeneralInfo.org_id"" type=""xs:string"" />
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
        
        public ProfilesQueryResponseMessage() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [2];
                _RootElements[0] = "ProfileDocumentList";
                _RootElements[1] = "CommerceServerProfilesQueryResponse";
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
        
        [Schema(@"",@"ProfileDocumentList")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"ProfileDocumentList"})]
        public sealed class ProfileDocumentList : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public ProfileDocumentList() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "ProfileDocumentList";
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
        
        [Schema(@"",@"CommerceServerProfilesQueryResponse")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"CommerceServerProfilesQueryResponse"})]
        public sealed class CommerceServerProfilesQueryResponse : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public CommerceServerProfilesQueryResponse() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "CommerceServerProfilesQueryResponse";
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
