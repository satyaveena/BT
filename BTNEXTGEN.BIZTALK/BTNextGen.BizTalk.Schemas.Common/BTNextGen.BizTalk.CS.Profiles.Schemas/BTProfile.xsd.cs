namespace BTNextGen.BizTalk.CS.Profiles.Schemas {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"",@"ProfileDocument")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"ProfileDocument"})]
    public sealed class BTProfile : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" elementFormDefault=""qualified"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:simpleType name=""bit"">
    <xs:restriction base=""xs:integer"">
      <xs:minInclusive value=""0"" />
      <xs:maxInclusive value=""1"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_org_idType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""50"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_user_catalog_setType"">
    <xs:restriction base=""xs:string"">
      <xs:maxLength value=""50"" />
      <xs:minLength value=""0"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_access_levelType"">
    <xs:restriction base=""xs:integer"">
      <xs:maxInclusive value=""2147483647"" />
      <xs:minInclusive value=""0"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_account_create_cartsType"">
    <xs:restriction base=""xs:string"">
      <xs:maxLength value=""4000"" />
      <xs:minLength value=""0"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_account_view_ordersType"">
    <xs:restriction base=""xs:string"">
      <xs:maxLength value=""4000"" />
      <xs:minLength value=""0"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_account_statusType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""1"" />
      <xs:maxLength value=""50"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:complexType name=""UserObject_AccountInfoType"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""org_id"" type=""UserObject_org_idType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""user_catalog_set"" type=""UserObject_user_catalog_setType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""date_registered"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""access_level"" type=""UserObject_access_levelType"" />
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""account_create_carts"" type=""UserObject_account_create_cartsType"" />
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""account_view_orders"" type=""UserObject_account_view_ordersType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""account_status"" type=""UserObject_account_statusType"" />
    </xs:sequence>
  </xs:complexType>
  <xs:simpleType name=""UserObject_campaign_historyType"">
    <xs:restriction base=""xs:string"">
      <xs:maxLength value=""50"" />
      <xs:minLength value=""0"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:complexType name=""UserObject_AdvertisingType"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""campaign_history"" type=""UserObject_campaign_historyType"" />
    </xs:sequence>
  </xs:complexType>
  <xs:simpleType name=""UserObject_user_id_changed_byType"">
    <xs:restriction base=""xs:string"">
      <xs:maxLength value=""50"" />
      <xs:minLength value=""0"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_application_nameType"">
    <xs:restriction base=""xs:string"">
      <xs:maxLength value=""256"" />
      <xs:minLength value=""0"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_KeyIndexType"">
    <xs:restriction base=""xs:integer"">
      <xs:minInclusive value=""1"" />
      <xs:maxInclusive value=""2"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_logon_error_datesType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""255"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_password_answer_error_datesType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""255"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:complexType name=""UserObject_ProfileSystemType"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""date_last_changed"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""date_created"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""user_id_changed_by"" type=""UserObject_user_id_changed_byType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""csadapter_date_last_changed"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""application_name"" type=""UserObject_application_nameType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""date_address_list_last_changed"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""date_credit_card_list_last_changed"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""KeyIndex"" type=""UserObject_KeyIndexType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""change_password"" type=""bit"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""last_lockedout_date"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""date_last_password_changed"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""date_last_logon"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""logon_error_dates"" type=""UserObject_logon_error_datesType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""last_activity_date"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""password_answer_error_dates"" type=""UserObject_password_answer_error_datesType"" />
    </xs:sequence>
  </xs:complexType>
  <xs:simpleType name=""UserObject_user_idType"">
    <xs:restriction base=""xs:string"">
      <xs:maxLength value=""50"" />
      <xs:minLength value=""1"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_preferred_addressType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""50"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_address_listType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""255"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_preferred_credit_cardType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""50"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_credit_card_listType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""255"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_user_typeType"">
    <xs:restriction base=""xs:string"">
      <xs:maxLength value=""50"" />
      <xs:minLength value=""0"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_tel_numberType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""32"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_tel_extensionType"">
    <xs:restriction base=""xs:string"">
      <xs:maxLength value=""50"" />
      <xs:minLength value=""0"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_fax_numberType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""32"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_fax_extensionType"">
    <xs:restriction base=""xs:string"">
      <xs:maxLength value=""50"" />
      <xs:minLength value=""0"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_languageType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""128"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_preferred_shipping_methodType"">
    <xs:restriction base=""xs:string"">
      <xs:maxLength value=""4000"" />
      <xs:minLength value=""0"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_default_shopper_listType"">
    <xs:restriction base=""xs:string"">
      <xs:maxLength value=""4000"" />
      <xs:minLength value=""0"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_user_live_idType"">
    <xs:restriction base=""xs:string"">
      <xs:maxLength value=""50"" />
      <xs:minLength value=""0"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_email_addressType"">
    <xs:restriction base=""xs:string"">
      <xs:maxLength value=""64"" />
      <xs:minLength value=""1"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_user_security_passwordType"">
    <xs:restriction base=""xs:string"">
      <xs:maxLength value=""100"" />
      <xs:minLength value=""1"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_last_nameType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""50"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_first_nameType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""50"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_password_questionType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""1"" />
      <xs:maxLength value=""255"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_password_answerType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""255"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:complexType name=""UserObject_GeneralInfoType"">
    <xs:sequence>
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""user_id"" type=""UserObject_user_idType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""preferred_address"" type=""UserObject_preferred_addressType"" />
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""address_list"" type=""UserObject_address_listType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""preferred_credit_card"" type=""UserObject_preferred_credit_cardType"" />
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""credit_card_list"" type=""UserObject_credit_card_listType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""user_type"" type=""UserObject_user_typeType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""tel_number"" type=""UserObject_tel_numberType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""tel_extension"" type=""UserObject_tel_extensionType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""fax_number"" type=""UserObject_fax_numberType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""fax_extension"" type=""UserObject_fax_extensionType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""language"" type=""UserObject_languageType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""direct_mail_opt_out"" type=""bit"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""express_checkout"" type=""bit"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""preferred_shipping_method"" type=""UserObject_preferred_shipping_methodType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""default_shopper_list"" type=""UserObject_default_shopper_listType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""user_live_id"" type=""UserObject_user_live_idType"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""email_address"" type=""UserObject_email_addressType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""user_security_password"" type=""UserObject_user_security_passwordType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""last_name"" type=""UserObject_last_nameType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""first_name"" type=""UserObject_first_nameType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""password_question"" type=""UserObject_password_questionType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""password_answer"" type=""UserObject_password_answerType"" />
    </xs:sequence>
  </xs:complexType>
  <xs:complexType name=""UserObject_SSOInfoType"">
    <xs:sequence />
  </xs:complexType>
  <xs:simpleType name=""UserObject_invalid_login_attemptsType"">
    <xs:restriction base=""xs:string"">
      <xs:maxLength value=""4000"" />
      <xs:minLength value=""0"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_user_nameType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_user_mobileType"">
    <xs:restriction base=""xs:string"">
      <xs:maxLength value=""50"" />
      <xs:minLength value=""0"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_default_book_accountType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""50"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_default_entertainment_accountType"">
    <xs:restriction base=""xs:string"">
      <xs:maxLength value=""50"" />
      <xs:minLength value=""0"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_default_billing_accountType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""50"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_default_shipping_accountType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""50"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_podType"">
    <xs:restriction base=""xs:string"">
      <xs:maxLength value=""50"" />
      <xs:minLength value=""0"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_roletypesType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""255"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_audience_typesType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_garnerType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""50"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_saved_searchesType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_my_preferencesType"">
    <xs:restriction base=""xs:string"">
      <xs:maxLength value=""4000"" />
      <xs:minLength value=""0"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_default_quantityType"">
    <xs:restriction base=""xs:integer"">
      <xs:maxInclusive value=""2147483647"" />
      <xs:minInclusive value=""-2147483648"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_cart_folder_listType"">
    <xs:restriction base=""xs:string"">
      <xs:maxLength value=""4000"" />
      <xs:minLength value=""0"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_product_type_listType"">
    <xs:restriction base=""xs:string"">
      <xs:maxLength value=""4000"" />
      <xs:minLength value=""0"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_function_listType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_product_lookup_listType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_u_alertsType"">
    <xs:restriction base=""xs:string"">
      <xs:maxLength value=""4000"" />
      <xs:minLength value=""0"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_bt_statusType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""1"" />
      <xs:maxLength value=""50"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_cust_user_nameType"">
    <xs:restriction base=""xs:string"">
      <xs:maxLength value=""4000"" />
      <xs:minLength value=""0"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_cybersource_subscription_idType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_default_home_delivery_account_idType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_default_home_delivery_summary_viewType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_legacy_source_systemType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_legacy_user_idType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_user_aliasType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_u_pigType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_default_esuppliers_accountType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_sales_repType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:complexType name=""UserObject_BTNextGenType"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""accept_agreement"" type=""bit"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""first_logon_date"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""invalid_login_attempts"" type=""UserObject_invalid_login_attemptsType"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""user_name"" type=""UserObject_user_nameType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""password_changed"" type=""bit"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""user_mobile"" type=""UserObject_user_mobileType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""default_book_account"" type=""UserObject_default_book_accountType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""default_entertainment_account"" type=""UserObject_default_entertainment_accountType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""default_billing_account"" type=""UserObject_default_billing_accountType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""default_shipping_account"" type=""UserObject_default_shipping_accountType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""pod"" type=""UserObject_podType"" />
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""roletypes"" type=""UserObject_roletypesType"" />
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""audience_types"" type=""UserObject_audience_typesType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""garner"" type=""UserObject_garnerType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""default_purchase_qty"" type=""xs:integer"" />
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""saved_searches"" type=""UserObject_saved_searchesType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""my_preferences"" type=""UserObject_my_preferencesType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""default_quantity"" type=""UserObject_default_quantityType"" />
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""cart_folder_list"" type=""UserObject_cart_folder_listType"" />
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""product_type_list"" type=""UserObject_product_type_listType"" />
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""function_list"" type=""UserObject_function_listType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""is_bt_employee"" type=""bit"" />
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""product_lookup_list"" type=""UserObject_product_lookup_listType"" />
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""u_alerts"" type=""UserObject_u_alertsType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""bt_status"" type=""UserObject_bt_statusType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""cust_user_name"" type=""UserObject_cust_user_nameType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""cybersource_subscription_id"" type=""UserObject_cybersource_subscription_idType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""default_home_delivery_account_id"" type=""UserObject_default_home_delivery_account_idType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""default_home_delivery_summary_view"" type=""UserObject_default_home_delivery_summary_viewType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""legacy_source_system"" type=""UserObject_legacy_source_systemType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""legacy_user_id"" type=""UserObject_legacy_user_idType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""user_alias"" type=""UserObject_user_aliasType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""legacy_created_date"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""legacy_updated_date"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""u_pig"" type=""UserObject_u_pigType"" />
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""default_esuppliers_account"" type=""UserObject_default_esuppliers_accountType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""sales_rep"" type=""UserObject_sales_repType"" />
    </xs:sequence>
  </xs:complexType>
  <xs:simpleType name=""UserObject_cart_formatType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_cart_sort_byType"">
    <xs:restriction base=""xs:string"">
      <xs:maxLength value=""4000"" />
      <xs:minLength value=""0"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_cart_sort_orderType"">
    <xs:restriction base=""xs:string"">
      <xs:maxLength value=""4000"" />
      <xs:minLength value=""0"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_default_duplicate_cartsType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_default_duplicate_ordersType"">
    <xs:restriction base=""xs:string"">
      <xs:maxLength value=""4000"" />
      <xs:minLength value=""0"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_my_review_type_listType"">
    <xs:restriction base=""xs:string"">
      <xs:maxLength value=""4000"" />
      <xs:minLength value=""0"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_product_type_filterType"">
    <xs:restriction base=""xs:string"">
      <xs:maxLength value=""4000"" />
      <xs:minLength value=""0"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_result_formatType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_result_sort_byType"">
    <xs:restriction base=""xs:string"">
      <xs:maxLength value=""4000"" />
      <xs:minLength value=""0"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_result_sort_orderType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserObject_default_marc_profileType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:complexType name=""UserObject_MyPreferencesType"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""cart_format"" type=""UserObject_cart_formatType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""cart_sort_by"" type=""UserObject_cart_sort_byType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""cart_sort_order"" type=""UserObject_cart_sort_orderType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""default_duplicate_carts"" type=""UserObject_default_duplicate_cartsType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""default_duplicate_orders"" type=""UserObject_default_duplicate_ordersType"" />
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""my_review_type_list"" type=""UserObject_my_review_type_listType"" />
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""product_type_filter"" type=""UserObject_product_type_filterType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""result_format"" type=""UserObject_result_formatType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""result_sort_by"" type=""UserObject_result_sort_byType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""result_sort_order"" type=""UserObject_result_sort_orderType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""home_delivery_allowed"" type=""bit"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""warning_delete_cart"" type=""bit"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""warning_delete_folder"" type=""bit"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""warning_delete_lineItem"" type=""bit"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""remove_share_carts_folder_when_empty"" type=""bit"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""default_marc_profile"" type=""UserObject_default_marc_profileType"" />
    </xs:sequence>
  </xs:complexType>
  <xs:complexType name=""UserObjectType"">
    <xs:sequence>
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""AccountInfo"" type=""UserObject_AccountInfoType"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""Advertising"" type=""UserObject_AdvertisingType"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""ProfileSystem"" type=""UserObject_ProfileSystemType"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""GeneralInfo"" type=""UserObject_GeneralInfoType"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""SSOInfo"" type=""UserObject_SSOInfoType"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""BTNextGen"" type=""UserObject_BTNextGenType"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""MyPreferences"" type=""UserObject_MyPreferencesType"" />
    </xs:sequence>
  </xs:complexType>
  <xs:simpleType name=""Address_address_idType"">
    <xs:restriction base=""xs:string"">
      <xs:maxLength value=""50"" />
      <xs:minLength value=""1"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Address_last_nameType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""64"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Address_first_nameType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""64"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Address_address_nameType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""50"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Address_address_typeType"">
    <xs:restriction base=""xs:integer"">
      <xs:minInclusive value=""0"" />
      <xs:maxInclusive value=""2147483647"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Address_descriptionType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""50"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Address_address_line1Type"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""80"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Address_address_line2Type"">
    <xs:restriction base=""xs:string"">
      <xs:maxLength value=""80"" />
      <xs:minLength value=""0"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Address_cityType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""64"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Address_region_codeType"">
    <xs:restriction base=""xs:string"">
      <xs:maxLength value=""50"" />
      <xs:minLength value=""0"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Address_region_nameType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""64"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Address_postal_codeType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""20"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Address_country_codeType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""50"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Address_country_nameType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""50"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Address_tel_numberType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""32"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Address_tel_extensionType"">
    <xs:restriction base=""xs:string"">
      <xs:maxLength value=""50"" />
      <xs:minLength value=""0"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Address_localeType"">
    <xs:restriction base=""xs:integer"">
      <xs:minInclusive value=""0"" />
      <xs:maxInclusive value=""2147483647"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:complexType name=""Address_GeneralInfoType"">
    <xs:sequence>
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""address_id"" type=""Address_address_idType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""last_name"" type=""Address_last_nameType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""first_name"" type=""Address_first_nameType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""address_name"" type=""Address_address_nameType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""address_type"" type=""Address_address_typeType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""description"" type=""Address_descriptionType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""address_line1"" type=""Address_address_line1Type"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""address_line2"" type=""Address_address_line2Type"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""city"" type=""Address_cityType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""region_code"" type=""Address_region_codeType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""region_name"" type=""Address_region_nameType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""postal_code"" type=""Address_postal_codeType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""country_code"" type=""Address_country_codeType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""country_name"" type=""Address_country_nameType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""tel_number"" type=""Address_tel_numberType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""tel_extension"" type=""Address_tel_extensionType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""locale"" type=""Address_localeType"" />
    </xs:sequence>
  </xs:complexType>
  <xs:simpleType name=""Address_user_id_changed_byType"">
    <xs:restriction base=""xs:string"">
      <xs:maxLength value=""50"" />
      <xs:minLength value=""0"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:complexType name=""Address_ProfileSystemType"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""user_id_changed_by"" type=""Address_user_id_changed_byType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""date_last_changed"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""date_created"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""csadapter_date_last_changed"" type=""xs:dateTime"" />
    </xs:sequence>
  </xs:complexType>
  <xs:simpleType name=""Address_address_line3Type"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Address_address_line4Type"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Address_email_addressType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:complexType name=""Address_BTNextGenType"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""address_line3"" type=""Address_address_line3Type"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""address_line4"" type=""Address_address_line4Type"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""is_po_box"" type=""bit"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""email_address"" type=""Address_email_addressType"" />
    </xs:sequence>
  </xs:complexType>
  <xs:complexType name=""AddressType"">
    <xs:sequence>
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""GeneralInfo"" type=""Address_GeneralInfoType"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""ProfileSystem"" type=""Address_ProfileSystemType"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""BTNextGen"" type=""Address_BTNextGenType"" />
    </xs:sequence>
  </xs:complexType>
  <xs:simpleType name=""BTAccount_account_idType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTAccount_account_typeType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTAccount_acquisition_typesType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTAccount_billing_addressType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTAccount_disable_reason_codeType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTAccount_primary_warehouseType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTAccount_account8_idType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTAccount_product_typeType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTAccount_san_suffixType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTAccount_shipping_addressType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTAccount_secondary_warehouseType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTAccount_max_copy_per_lineType"">
    <xs:restriction base=""xs:integer"">
      <xs:maxInclusive value=""2147483647"" />
      <xs:minInclusive value=""-2147483648"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTAccount_account_aliasType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTAccount_account_nameType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTAccount_disable_user_idType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTAccount_users_create_cartType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTAccount_users_view_orderType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTAccount_bill_to_account_numberType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTAccount_org_idType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTAccount_fee_listType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTAccount_inventory_typeType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTAccount_reserve_inventory_numberType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTAccount_exclusive_item_groupType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTAccount_erp_account_numberType"">
    <xs:restriction base=""xs:string"">
      <xs:maxLength value=""4000"" />
      <xs:minLength value=""0"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTAccount_sop_price_plan_listType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTAccount_warehousesType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTAccount_accountsType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTAccount_legacy_source_systemType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTAccount_processing_typesType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTAccount_legacy_account_idType"">
    <xs:restriction base=""xs:integer"">
      <xs:minInclusive value=""-2147483648"" />
      <xs:maxInclusive value=""2147483647"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTAccount_legacy_org_idType"">
    <xs:restriction base=""xs:integer"">
      <xs:minInclusive value=""-2147483648"" />
      <xs:maxInclusive value=""2147483647"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTAccount_eSupplierType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTAccount_esupplier_market_typeType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTAccount_esupplier_market_tierType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTAccount_sales_repType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:complexType name=""BTAccount_BTNextGenType"">
    <xs:sequence>
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""account_id"" type=""BTAccount_account_idType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""account_type"" type=""BTAccount_account_typeType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""acquisition_types"" type=""BTAccount_acquisition_typesType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""billing_address"" type=""BTAccount_billing_addressType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""disable_reason_code"" type=""BTAccount_disable_reason_codeType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""primary_warehouse"" type=""BTAccount_primary_warehouseType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""account8_id"" type=""BTAccount_account8_idType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""product_type"" type=""BTAccount_product_typeType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""san_suffix"" type=""BTAccount_san_suffixType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""shipping_address"" type=""BTAccount_shipping_addressType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""secondary_warehouse"" type=""BTAccount_secondary_warehouseType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""disable_date"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""max_copy_per_line"" type=""BTAccount_max_copy_per_lineType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""is_billing_account"" type=""bit"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""is_shipping_account"" type=""bit"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""is_home_delivery"" type=""bit"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""is_disabled"" type=""bit"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""account_alias"" type=""BTAccount_account_aliasType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""account_name"" type=""BTAccount_account_nameType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""disable_user_id"" type=""BTAccount_disable_user_idType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""default_account"" type=""bit"" />
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""users_create_cart"" type=""BTAccount_users_create_cartType"" />
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""users_view_order"" type=""BTAccount_users_view_orderType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""bill_to_account_number"" type=""BTAccount_bill_to_account_numberType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""org_id"" type=""BTAccount_org_idType"" />
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""fee_list"" type=""BTAccount_fee_listType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""preferred_credit_card"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""date_credit_card_list_last_changed"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""cc_transmitted_to_erp"" type=""bit"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""inventory_type"" type=""BTAccount_inventory_typeType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""reserve_inventory_number"" type=""BTAccount_reserve_inventory_numberType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""check_reserve_flag"" type=""bit"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""exclusive_item_group"" type=""BTAccount_exclusive_item_groupType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""erp_account_number"" type=""BTAccount_erp_account_numberType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""is_TOLAS"" type=""bit"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""sop_price_plan_list"" type=""BTAccount_sop_price_plan_listType"" />
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""credit_cards"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""primary_payment_option"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""warehouses"" type=""BTAccount_warehousesType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""is_library_system_account"" type=""bit"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""accounts"" type=""BTAccount_accountsType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""legacy_source_system"" type=""BTAccount_legacy_source_systemType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""processing_types"" type=""BTAccount_processing_typesType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""legacy_created_date"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""legacy_updated_date"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""is_internal"" type=""bit"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""is_entertainment_grid_account"" type=""bit"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""legacy_account_id"" type=""BTAccount_legacy_account_idType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""legacy_org_id"" type=""BTAccount_legacy_org_idType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""check_le_reserve"" type=""bit"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""eSupplier"" type=""BTAccount_eSupplierType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""esupplier_market_type"" type=""BTAccount_esupplier_market_typeType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""esupplier_market_tier"" type=""BTAccount_esupplier_market_tierType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""sales_rep"" type=""BTAccount_sales_repType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""is_online_bill_pay_enabled"" type=""bit"" />
    </xs:sequence>
  </xs:complexType>
  <xs:simpleType name=""BTAccount_user_id_changed_byType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:complexType name=""BTAccount_ProfileSystemType"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""user_id_changed_by"" type=""BTAccount_user_id_changed_byType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""csadapter_date_last_changed"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""date_created"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""date_last_changed"" type=""xs:dateTime"" />
    </xs:sequence>
  </xs:complexType>
  <xs:simpleType name=""BTAccount_minimum_marginType"">
    <xs:restriction base=""xs:integer"">
      <xs:minInclusive value=""-2147483648"" />
      <xs:maxInclusive value=""2147483647"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTAccount_itemprc_lib_numberType"">
    <xs:restriction base=""xs:string"">
      <xs:maxLength value=""4000"" />
      <xs:minLength value=""0"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:complexType name=""BTAccount_PricingSystemType"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""best_pricing_flag"" type=""bit"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""blowout_pricing_flag"" type=""bit"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""contract_code_flag"" type=""bit"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""firm_pricing_flag"" type=""bit"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""independents_day_program_flag"" type=""bit"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""item_special_pricing_flag"" type=""bit"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""MSRP_pricing_flag"" type=""bit"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""skip_generic_pricing_flag"" type=""bit"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""surcharge"" type=""bit"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""minimum_margin"" type=""BTAccount_minimum_marginType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""itemprc_lib_number"" type=""BTAccount_itemprc_lib_numberType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""flat_fee_pricing_flag"" type=""bit"" />
    </xs:sequence>
  </xs:complexType>
  <xs:complexType name=""BTAccountType"">
    <xs:sequence>
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""BTNextGen"" type=""BTAccount_BTNextGenType"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""ProfileSystem"" type=""BTAccount_ProfileSystemType"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""PricingSystem"" type=""BTAccount_PricingSystemType"" />
    </xs:sequence>
  </xs:complexType>
  <xs:simpleType name=""BTFee_fee_idType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTFee_fee_descriptionType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTFee_fee_labelType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTFee_fee_amountType"">
    <xs:restriction base=""xs:float"">
      <xs:minInclusive value=""-2147483648"" />
      <xs:maxInclusive value=""2147483647"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTFee_fee_typeType"">
    <xs:restriction base=""xs:string"">
      <xs:maxLength value=""4000"" />
      <xs:minLength value=""0"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:complexType name=""BTFee_BTNextGenType"">
    <xs:sequence>
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""fee_id"" type=""BTFee_fee_idType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""fee_description"" type=""BTFee_fee_descriptionType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""fee_label"" type=""BTFee_fee_labelType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""fee_amount"" type=""BTFee_fee_amountType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""fee_type"" type=""BTFee_fee_typeType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""is_active"" type=""bit"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""percent_indicator"" type=""bit"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""bt_indicator"" type=""bit"" />
    </xs:sequence>
  </xs:complexType>
  <xs:simpleType name=""BTFee_user_id_changed_byType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:complexType name=""BTFee_ProfileSystemType"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""user_id_changed_by"" type=""BTFee_user_id_changed_byType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""csadapter_date_last_changed"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""date_created"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""date_last_changed"" type=""xs:dateTime"" />
    </xs:sequence>
  </xs:complexType>
  <xs:complexType name=""BTFeeType"">
    <xs:sequence>
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""BTNextGen"" type=""BTFee_BTNextGenType"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""ProfileSystem"" type=""BTFee_ProfileSystemType"" />
    </xs:sequence>
  </xs:complexType>
  <xs:simpleType name=""BTInvalidLoginAttempts_login_attempt_idType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTInvalidLoginAttempts_user_nameType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTInvalidLoginAttempts_ip_addressType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:complexType name=""BTInvalidLoginAttempts_BTNextGenType"">
    <xs:sequence>
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""login_attempt_id"" type=""BTInvalidLoginAttempts_login_attempt_idType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""user_name"" type=""BTInvalidLoginAttempts_user_nameType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""login_attempt_date"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""ip_address"" type=""BTInvalidLoginAttempts_ip_addressType"" />
    </xs:sequence>
  </xs:complexType>
  <xs:simpleType name=""BTInvalidLoginAttempts_user_id_changed_byType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:complexType name=""BTInvalidLoginAttempts_ProfileSystemType"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""user_id_changed_by"" type=""BTInvalidLoginAttempts_user_id_changed_byType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""date_last_changed"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""date_created"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""csadapter_date_last_changed"" type=""xs:dateTime"" />
    </xs:sequence>
  </xs:complexType>
  <xs:complexType name=""BTInvalidLoginAttemptsType"">
    <xs:sequence>
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""BTNextGen"" type=""BTInvalidLoginAttempts_BTNextGenType"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""ProfileSystem"" type=""BTInvalidLoginAttempts_ProfileSystemType"" />
    </xs:sequence>
  </xs:complexType>
  <xs:simpleType name=""BTMyPreferences_u_user_idType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:complexType name=""BTMyPreferences_GeneralInfoType"">
    <xs:sequence>
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""u_user_id"" type=""BTMyPreferences_u_user_idType"" />
    </xs:sequence>
  </xs:complexType>
  <xs:simpleType name=""BTMyPreferences_u_email_addressType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTMyPreferences_u_result_formatType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTMyPreferences_u_result_sort_byType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTMyPreferences_functionsType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTMyPreferences_u_result_sort_orderType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTMyPreferences_my_review_typeType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTMyPreferences_u_cart_formatType"">
    <xs:restriction base=""xs:string"">
      <xs:maxLength value=""4000"" />
      <xs:minLength value=""0"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTMyPreferences_u_cart_sort_byType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTMyPreferences_u_cart_sort_orderType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTMyPreferences_u_product_typeType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTMyPreferences_u_audience_typeType"">
    <xs:restriction base=""xs:string"">
      <xs:maxLength value=""4000"" />
      <xs:minLength value=""0"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTMyPreferences_u_default_duplicate_cartsType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTMyPreferences_u_default_duplicate_ordersType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTMyPreferences_u_product_type_filterType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTMyPreferences_u_default_entertainment_accountType"">
    <xs:restriction base=""xs:string"">
      <xs:maxLength value=""4000"" />
      <xs:minLength value=""0"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTMyPreferences_u_default_book_accountType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:complexType name=""BTMyPreferences_BTNextGenType"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""u_email_address"" type=""BTMyPreferences_u_email_addressType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""u_result_format"" type=""BTMyPreferences_u_result_formatType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""u_result_sort_by"" type=""BTMyPreferences_u_result_sort_byType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""home_delivery_allowed"" type=""bit"" />
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""functions"" type=""BTMyPreferences_functionsType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""u_result_sort_order"" type=""BTMyPreferences_u_result_sort_orderType"" />
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""my_review_type"" type=""BTMyPreferences_my_review_typeType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""u_cart_format"" type=""BTMyPreferences_u_cart_formatType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""u_cart_sort_by"" type=""BTMyPreferences_u_cart_sort_byType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""u_cart_sort_order"" type=""BTMyPreferences_u_cart_sort_orderType"" />
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""u_product_type"" type=""BTMyPreferences_u_product_typeType"" />
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""u_audience_type"" type=""BTMyPreferences_u_audience_typeType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""u_default_duplicate_carts"" type=""BTMyPreferences_u_default_duplicate_cartsType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""u_default_duplicate_orders"" type=""BTMyPreferences_u_default_duplicate_ordersType"" />
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""u_product_type_filter"" type=""BTMyPreferences_u_product_type_filterType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""u_default_entertainment_account"" type=""BTMyPreferences_u_default_entertainment_accountType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""u_default_book_account"" type=""BTMyPreferences_u_default_book_accountType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""warning_delete_cart"" type=""bit"" />
    </xs:sequence>
  </xs:complexType>
  <xs:complexType name=""BTMyPreferences_ProfileSystemType"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""date_created"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""date_last_changed"" type=""xs:dateTime"" />
    </xs:sequence>
  </xs:complexType>
  <xs:complexType name=""BTMyPreferencesType"">
    <xs:sequence>
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""GeneralInfo"" type=""BTMyPreferences_GeneralInfoType"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""BTNextGen"" type=""BTMyPreferences_BTNextGenType"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""ProfileSystem"" type=""BTMyPreferences_ProfileSystemType"" />
    </xs:sequence>
  </xs:complexType>
  <xs:complexType name=""BTProductInterestGroup_ProfileSystemType"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""date_last_changed"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""date_created"" type=""xs:dateTime"" />
    </xs:sequence>
  </xs:complexType>
  <xs:simpleType name=""BTProductInterestGroup_pig_idType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTProductInterestGroup_nameType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTProductInterestGroup_DescriptionType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTProductInterestGroup_MarketTypeType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTProductInterestGroup_ProductTypeType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:complexType name=""BTProductInterestGroup_BTNGType"">
    <xs:sequence>
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""pig_id"" type=""BTProductInterestGroup_pig_idType"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""name"" type=""BTProductInterestGroup_nameType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""Description"" type=""BTProductInterestGroup_DescriptionType"" />
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""MarketType"" type=""BTProductInterestGroup_MarketTypeType"" />
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""ProductType"" type=""BTProductInterestGroup_ProductTypeType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""u_display_sequence"" type=""xs:string"" />
    </xs:sequence>
  </xs:complexType>
  <xs:complexType name=""BTProductInterestGroupType"">
    <xs:sequence>
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""ProfileSystem"" type=""BTProductInterestGroup_ProfileSystemType"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""BTNG"" type=""BTProductInterestGroup_BTNGType"" />
    </xs:sequence>
  </xs:complexType>
  <xs:simpleType name=""BTProductLookup_product_lookup_idType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTProductLookup_isbn_link_displayedType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTProductLookup_isbn_lookup_codeType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTProductLookup_personal_prod_urlType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTProductLookup_prod_lookup_indexType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTProductLookup_prod_suffix_lookupType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:complexType name=""BTProductLookup_BTNextGenType"">
    <xs:sequence>
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""product_lookup_id"" type=""BTProductLookup_product_lookup_idType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""isbn_link_displayed"" type=""BTProductLookup_isbn_link_displayedType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""isbn_lookup_code"" type=""BTProductLookup_isbn_lookup_codeType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""personal_prod_url"" type=""BTProductLookup_personal_prod_urlType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""prod_lookup_index"" type=""BTProductLookup_prod_lookup_indexType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""prod_suffix_lookup"" type=""BTProductLookup_prod_suffix_lookupType"" />
    </xs:sequence>
  </xs:complexType>
  <xs:simpleType name=""BTProductLookup_user_id_changed_byType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:complexType name=""BTProductLookup_ProfileSystemType"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""user_id_changed_by"" type=""BTProductLookup_user_id_changed_byType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""csadapter_date_last_changed"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""date_created"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""date_last_changed"" type=""xs:dateTime"" />
    </xs:sequence>
  </xs:complexType>
  <xs:complexType name=""BTProductLookupType"">
    <xs:sequence>
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""BTNextGen"" type=""BTProductLookup_BTNextGenType"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""ProfileSystem"" type=""BTProductLookup_ProfileSystemType"" />
    </xs:sequence>
  </xs:complexType>
  <xs:complexType name=""BTSavedSearch_ProfileSystemType"">
    <xs:sequence>
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""date_created"" type=""xs:dateTime"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""date_last_changed"" type=""xs:dateTime"" />
    </xs:sequence>
  </xs:complexType>
  <xs:simpleType name=""BTSavedSearch_saved_search_idType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTSavedSearch_nameType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTSavedSearch_searchfromType"">
    <xs:restriction base=""xs:integer"">
      <xs:minInclusive value=""-2147483648"" />
      <xs:maxInclusive value=""2147483647"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:complexType name=""BTSavedSearch_BTNextGenType"">
    <xs:sequence>
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""saved_search_id"" type=""BTSavedSearch_saved_search_idType"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""name"" type=""BTSavedSearch_nameType"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""search_criteria"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""searchfrom"" type=""BTSavedSearch_searchfromType"" />
    </xs:sequence>
  </xs:complexType>
  <xs:complexType name=""BTSavedSearchType"">
    <xs:sequence>
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""ProfileSystem"" type=""BTSavedSearch_ProfileSystemType"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""BTNextGen"" type=""BTSavedSearch_BTNextGenType"" />
    </xs:sequence>
  </xs:complexType>
  <xs:simpleType name=""BTShippingMethod_shipping_method_idType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTShippingMethod_shipping_method_nameType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTShippingMethod_address_typeType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTShippingMethod_carrier_codeType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTShippingMethod_first_unit_costType"">
    <xs:restriction base=""xs:float"">
      <xs:minInclusive value=""-2147483648"" />
      <xs:maxInclusive value=""2147483647"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTShippingMethod_additional_unit_costType"">
    <xs:restriction base=""xs:float"">
      <xs:minInclusive value=""-2147483648"" />
      <xs:maxInclusive value=""2147483647"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:complexType name=""BTShippingMethod_BTNextGenType"">
    <xs:sequence>
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""shipping_method_id"" type=""BTShippingMethod_shipping_method_idType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""shipping_method_name"" type=""BTShippingMethod_shipping_method_nameType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""address_type"" type=""BTShippingMethod_address_typeType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""carrier_code"" type=""BTShippingMethod_carrier_codeType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""first_unit_cost"" type=""BTShippingMethod_first_unit_costType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""additional_unit_cost"" type=""BTShippingMethod_additional_unit_costType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""is_active"" type=""bit"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""b_is_allowed_po_box"" type=""bit"" />
    </xs:sequence>
  </xs:complexType>
  <xs:simpleType name=""BTShippingMethod_user_id_changed_byType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:complexType name=""BTShippingMethod_ProfileSystemType"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""user_id_changed_by"" type=""BTShippingMethod_user_id_changed_byType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""csadapter_date_last_changed"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""date_created"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""date_last_changed"" type=""xs:dateTime"" />
    </xs:sequence>
  </xs:complexType>
  <xs:complexType name=""BTShippingMethodType"">
    <xs:sequence>
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""BTNextGen"" type=""BTShippingMethod_BTNextGenType"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""ProfileSystem"" type=""BTShippingMethod_ProfileSystemType"" />
    </xs:sequence>
  </xs:complexType>
  <xs:complexType name=""BTSiteBranding_ProfileSystemType"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""date_created"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""date_last_changed"" type=""xs:dateTime"" />
    </xs:sequence>
  </xs:complexType>
  <xs:simpleType name=""BTSiteBranding_sitebranding_idType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTSiteBranding_nameType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTSiteBranding_search_stringType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTSiteBranding_HeaderImageUrlType"">
    <xs:restriction base=""xs:string"">
      <xs:maxLength value=""4000"" />
      <xs:minLength value=""0"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTSiteBranding_FooterImageUrlType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:complexType name=""BTSiteBranding_BTNGType"">
    <xs:sequence>
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""sitebranding_id"" type=""BTSiteBranding_sitebranding_idType"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""name"" type=""BTSiteBranding_nameType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""search_string"" type=""BTSiteBranding_search_stringType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""is_default"" type=""bit"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""HeaderImageUrl"" type=""BTSiteBranding_HeaderImageUrlType"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""FooterImageUrl"" type=""BTSiteBranding_FooterImageUrlType"" />
    </xs:sequence>
  </xs:complexType>
  <xs:complexType name=""BTSiteBrandingType"">
    <xs:sequence>
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""ProfileSystem"" type=""BTSiteBranding_ProfileSystemType"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""BTNG"" type=""BTSiteBranding_BTNGType"" />
    </xs:sequence>
  </xs:complexType>
  <xs:simpleType name=""BTSubscription_subscription_idType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTSubscription_org_idType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTSubscription_review_type_listType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTSubscription_content_typesType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTSubscription_subscription_descriptionType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTSubscription_fulltext_reviewsType"">
    <xs:restriction base=""xs:integer"">
      <xs:minInclusive value=""-2147483648"" />
      <xs:maxInclusive value=""2147483647"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:complexType name=""BTSubscription_BTNextGenType"">
    <xs:sequence>
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""subscription_id"" type=""BTSubscription_subscription_idType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""org_id"" type=""BTSubscription_org_idType"" />
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""review_type_list"" type=""BTSubscription_review_type_listType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""subscription_start_date"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""subscription_end_date"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""entertainment_product"" type=""bit"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""full_txt_review_enabled"" type=""bit"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""content_types"" type=""BTSubscription_content_typesType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""subscription_description"" type=""BTSubscription_subscription_descriptionType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""table_of_contents"" type=""bit"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""fulltext_reviews"" type=""BTSubscription_fulltext_reviewsType"" />
    </xs:sequence>
  </xs:complexType>
  <xs:simpleType name=""BTSubscription_user_id_changed_byType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:complexType name=""BTSubscription_ProfileSystemType"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""user_id_changed_by"" type=""BTSubscription_user_id_changed_byType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""date_created"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""date_last_changed"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""cs_adapter_date_last_changed"" type=""xs:dateTime"" />
    </xs:sequence>
  </xs:complexType>
  <xs:complexType name=""BTSubscriptionType"">
    <xs:sequence>
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""BTNextGen"" type=""BTSubscription_BTNextGenType"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""ProfileSystem"" type=""BTSubscription_ProfileSystemType"" />
    </xs:sequence>
  </xs:complexType>
  <xs:simpleType name=""BTUserAlert_u_user_id_changed_byType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:complexType name=""BTUserAlert_ProfileSystemType"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""u_user_id_changed_by"" type=""BTUserAlert_u_user_id_changed_byType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""dt_date_created"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""dt_date_last_changed"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""dt_csadapter_date_last_changed"" type=""xs:dateTime"" />
    </xs:sequence>
  </xs:complexType>
  <xs:simpleType name=""BTUserAlert_u_alert_idType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTUserAlert_u_user_keyType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTUserAlert_u_alert_messageType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTUserAlert_u_alert_titleType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTUserAlert_u_alert_priorityType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTUserAlert_u_alert_typeType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:complexType name=""BTUserAlert_BTNextGenType"">
    <xs:sequence>
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""u_alert_id"" type=""BTUserAlert_u_alert_idType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""u_user_key"" type=""BTUserAlert_u_user_keyType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""b_is_shown"" type=""bit"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""u_alert_message"" type=""BTUserAlert_u_alert_messageType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""u_alert_title"" type=""BTUserAlert_u_alert_titleType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""u_alert_priority"" type=""BTUserAlert_u_alert_priorityType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""u_alert_type"" type=""BTUserAlert_u_alert_typeType"" />
    </xs:sequence>
  </xs:complexType>
  <xs:complexType name=""BTUserAlertType"">
    <xs:sequence>
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""ProfileSystem"" type=""BTUserAlert_ProfileSystemType"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""BTNextGen"" type=""BTUserAlert_BTNextGenType"" />
    </xs:sequence>
  </xs:complexType>
  <xs:simpleType name=""BTUserReviewType_userReview_idType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:complexType name=""BTUserReviewType_GeneralInfoType"">
    <xs:sequence>
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""userReview_id"" type=""BTUserReviewType_userReview_idType"" />
    </xs:sequence>
  </xs:complexType>
  <xs:simpleType name=""BTUserReviewType_user_idType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTUserReviewType_review_type_idType"">
    <xs:restriction base=""xs:string"">
      <xs:maxLength value=""4000"" />
      <xs:minLength value=""0"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTUserReviewType_ordinalType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:complexType name=""BTUserReviewType_BTNextGenType"">
    <xs:sequence>
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""user_id"" type=""BTUserReviewType_user_idType"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""review_type_id"" type=""BTUserReviewType_review_type_idType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""ordinal"" type=""BTUserReviewType_ordinalType"" />
    </xs:sequence>
  </xs:complexType>
  <xs:simpleType name=""BTUserReviewType_user_id_changed_byType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:complexType name=""BTUserReviewType_ProfileSystemType"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""user_id_changed_by"" type=""BTUserReviewType_user_id_changed_byType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""csadapter_date_last_changed"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""date_created"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""date_last_changed"" type=""xs:dateTime"" />
    </xs:sequence>
  </xs:complexType>
  <xs:complexType name=""BTUserReviewTypeType"">
    <xs:sequence>
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""GeneralInfo"" type=""BTUserReviewType_GeneralInfoType"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""BTNextGen"" type=""BTUserReviewType_BTNextGenType"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""ProfileSystem"" type=""BTUserReviewType_ProfileSystemType"" />
    </xs:sequence>
  </xs:complexType>
  <xs:simpleType name=""BTWarehouse_warehouse_idType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTWarehouse_warehouse_erp_codeType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTWarehouse_warehouse_descriptionType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTWarehouse_btwarehouse_idType"">
    <xs:restriction base=""xs:integer"">
      <xs:maxInclusive value=""2147483647"" />
      <xs:minInclusive value=""-2147483648"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTWarehouse_warehouse_inventory_nameType"">
    <xs:restriction base=""xs:string"">
      <xs:maxLength value=""4000"" />
      <xs:minLength value=""0"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:complexType name=""BTWarehouse_BTNextGenType"">
    <xs:sequence>
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""warehouse_id"" type=""BTWarehouse_warehouse_idType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""warehouse_erp_code"" type=""BTWarehouse_warehouse_erp_codeType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""warehouse_description"" type=""BTWarehouse_warehouse_descriptionType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""btwarehouse_id"" type=""BTWarehouse_btwarehouse_idType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""warehouse_inventory_name"" type=""BTWarehouse_warehouse_inventory_nameType"" />
    </xs:sequence>
  </xs:complexType>
  <xs:simpleType name=""BTWarehouse_user_id_changed_byType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:complexType name=""BTWarehouse_ProfileSystemType"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""date_created"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""date_last_changed"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""csadapter_date_last_changed"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""user_id_changed_by"" type=""BTWarehouse_user_id_changed_byType"" />
    </xs:sequence>
  </xs:complexType>
  <xs:complexType name=""BTWarehouseType"">
    <xs:sequence>
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""BTNextGen"" type=""BTWarehouse_BTNextGenType"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""ProfileSystem"" type=""BTWarehouse_ProfileSystemType"" />
    </xs:sequence>
  </xs:complexType>
  <xs:simpleType name=""CreditCard_idType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""1"" />
      <xs:maxLength value=""50"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""CreditCard_payment_group_idType"">
    <xs:restriction base=""xs:string"">
      <xs:maxLength value=""50"" />
      <xs:minLength value=""1"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""CreditCard_cc_numberType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""1"" />
      <xs:maxLength value=""40"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""CreditCard_last_4_digitsType"">
    <xs:restriction base=""xs:string"">
      <xs:maxLength value=""4"" />
      <xs:minLength value=""1"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""CreditCard_billing_addressType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""1"" />
      <xs:maxLength value=""50"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""CreditCard_expiration_monthType"">
    <xs:restriction base=""xs:integer"">
      <xs:minInclusive value=""-2147483648"" />
      <xs:maxInclusive value=""2147483647"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""CreditCard_expiration_yearType"">
    <xs:restriction base=""xs:integer"">
      <xs:minInclusive value=""-2147483648"" />
      <xs:maxInclusive value=""2147483647"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:complexType name=""CreditCard_GeneralInfoType"">
    <xs:sequence>
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""id"" type=""CreditCard_idType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""payment_group_id"" type=""CreditCard_payment_group_idType"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""cc_number"" type=""CreditCard_cc_numberType"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""last_4_digits"" type=""CreditCard_last_4_digitsType"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""billing_address"" type=""CreditCard_billing_addressType"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""expiration_month"" type=""CreditCard_expiration_monthType"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""expiration_year"" type=""CreditCard_expiration_yearType"" />
    </xs:sequence>
  </xs:complexType>
  <xs:simpleType name=""CreditCard_KeyIndexType"">
    <xs:restriction base=""xs:integer"">
      <xs:maxInclusive value=""2"" />
      <xs:minInclusive value=""1"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""CreditCard_user_id_changed_byType"">
    <xs:restriction base=""xs:string"">
      <xs:maxLength value=""50"" />
      <xs:minLength value=""0"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:complexType name=""CreditCard_ProfileSystemType"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""KeyIndex"" type=""CreditCard_KeyIndexType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""user_id_changed_by"" type=""CreditCard_user_id_changed_byType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""date_last_changed"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""date_created"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""csadapter_date_last_changed"" type=""xs:dateTime"" />
    </xs:sequence>
  </xs:complexType>
  <xs:simpleType name=""CreditCard_aliasType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""CreditCard_credit_card_typeType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:complexType name=""CreditCard_BTNextGenType"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""credit_card_token"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""alias"" type=""CreditCard_aliasType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""primary_indicator"" type=""bit"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""credit_card_type"" type=""CreditCard_credit_card_typeType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""transmitted_to_erp"" type=""bit"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""payment_status"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""notification_status"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""card_contact_user"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""date_last_failed_notification"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""date_last_preemptive_notification"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""u_subscription_id"" type=""xs:string"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""u_subscription_id_public_signature"" type=""xs:string"" />
    </xs:sequence>
  </xs:complexType>
  <xs:complexType name=""CreditCardType"">
    <xs:sequence>
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""GeneralInfo"" type=""CreditCard_GeneralInfoType"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""ProfileSystem"" type=""CreditCard_ProfileSystemType"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""BTNextGen"" type=""CreditCard_BTNextGenType"" />
    </xs:sequence>
  </xs:complexType>
  <xs:simpleType name=""Currency_currency_codeType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""1"" />
      <xs:maxLength value=""50"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Currency_currency_cultureType"">
    <xs:restriction base=""xs:string"">
      <xs:maxLength value=""50"" />
      <xs:minLength value=""1"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Currency_conversion_factorType"">
    <xs:restriction base=""xs:float"">
      <xs:minInclusive value=""0"" />
      <xs:maxInclusive value=""2147483647"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:complexType name=""Currency_GeneralInfoType"">
    <xs:sequence>
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""currency_code"" type=""Currency_currency_codeType"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""currency_culture"" type=""Currency_currency_cultureType"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""conversion_factor"" type=""Currency_conversion_factorType"" />
    </xs:sequence>
  </xs:complexType>
  <xs:simpleType name=""Currency_user_id_changed_byType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""50"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:complexType name=""Currency_ProfileSystemType"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""user_id_changed_by"" type=""Currency_user_id_changed_byType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""date_last_changed"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""date_created"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""csadapter_date_last_changed"" type=""xs:dateTime"" />
    </xs:sequence>
  </xs:complexType>
  <xs:complexType name=""CurrencyType"">
    <xs:sequence>
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""GeneralInfo"" type=""Currency_GeneralInfoType"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""ProfileSystem"" type=""Currency_ProfileSystemType"" />
    </xs:sequence>
  </xs:complexType>
  <xs:simpleType name=""Organization_org_idType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""1"" />
      <xs:maxLength value=""50"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Organization_nameType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""1"" />
      <xs:maxLength value=""255"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Organization_trading_partner_numberType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""255"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Organization_user_id_admin_contactType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""50"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Organization_user_id_receiverType"">
    <xs:restriction base=""xs:string"">
      <xs:maxLength value=""50"" />
      <xs:minLength value=""0"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Organization_preferred_addressType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""50"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Organization_address_listType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""255"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Organization_org_catalog_setType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""255"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Organization_purchasingType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""255"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:complexType name=""Organization_GeneralInfoType"">
    <xs:sequence>
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""org_id"" type=""Organization_org_idType"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""name"" type=""Organization_nameType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""trading_partner_number"" type=""Organization_trading_partner_numberType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""user_id_admin_contact"" type=""Organization_user_id_admin_contactType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""user_id_receiver"" type=""Organization_user_id_receiverType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""preferred_address"" type=""Organization_preferred_addressType"" />
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""address_list"" type=""Organization_address_listType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""org_catalog_set"" type=""Organization_org_catalog_setType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""purchasing"" type=""Organization_purchasingType"" />
    </xs:sequence>
  </xs:complexType>
  <xs:simpleType name=""Organization_user_id_changed_byType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""50"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:complexType name=""Organization_ProfileSystemType"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""user_id_changed_by"" type=""Organization_user_id_changed_byType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""date_last_changed"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""date_created"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""csadapter_date_last_changed"" type=""xs:dateTime"" />
    </xs:sequence>
  </xs:complexType>
  <xs:simpleType name=""Organization_bt_org_idType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Organization_erp_market_typeType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Organization_web_market_typeType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Organization_allowed_users_countType"">
    <xs:restriction base=""xs:integer"">
      <xs:minInclusive value=""-2147483648"" />
      <xs:maxInclusive value=""2147483647"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Organization_users_use_countType"">
    <xs:restriction base=""xs:integer"">
      <xs:minInclusive value=""-2147483648"" />
      <xs:maxInclusive value=""2147483647"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Organization_credit_card_listType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Organization_aliasType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Organization_contact_faxType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Organization_contact_emailType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Organization_unit_typeType"">
    <xs:restriction base=""xs:string"">
      <xs:maxLength value=""4000"" />
      <xs:minLength value=""0"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Organization_fee_listType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Organization_default_book_account_idType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Organization_default_entertainment_account_idType"">
    <xs:restriction base=""xs:string"">
      <xs:maxLength value=""4000"" />
      <xs:minLength value=""0"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Organization_address_idType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Organization_account_listType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Organization_fulltext_reviewsType"">
    <xs:restriction base=""xs:integer"">
      <xs:minInclusive value=""-2147483648"" />
      <xs:maxInclusive value=""2147483647"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Organization_review_type_listType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Organization_admin_contactType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Organization_statusType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Organization_contact_phoneType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Organization_user_listType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Organization_account_countType"">
    <xs:restriction base=""xs:integer"">
      <xs:minInclusive value=""-2147483648"" />
      <xs:maxInclusive value=""2147483647"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Organization_user_countType"">
    <xs:restriction base=""xs:integer"">
      <xs:minInclusive value=""-2147483648"" />
      <xs:maxInclusive value=""2147483647"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Organization_shipping_method_listType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Organization_personal_prod_urlType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Organization_prod_lookup_indexType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Organization_isbn_lookup_codeType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Organization_isbn_link_displayedType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Organization_prod_suffix_lookupType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Organization_payment_option_listType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Organization_returns_messageType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""80"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Organization_marketing_messageType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""80"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Organization_reserve_numberType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Organization_subscription_listType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Organization_product_lookup_listType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Organization_u_contact_nameType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Organization_legacy_org_idType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Organization_org_typeType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Organization_library_membershipType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Organization_library_system_typeType"">
    <xs:restriction base=""xs:string"">
      <xs:maxLength value=""4000"" />
      <xs:minLength value=""0"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Organization_country_codeType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Organization_geo_codeType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Organization_market_typesType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Organization_partial_ship_codeType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Organization_partnerType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Organization_preferred_carrier_codeType"">
    <xs:restriction base=""xs:string"">
      <xs:maxLength value=""4000"" />
      <xs:minLength value=""0"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Organization_sanType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Organization_legacy_sanType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Organization_legacy_source_systemType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Organization_warehousesType"">
    <xs:restriction base=""xs:string"">
      <xs:maxLength value=""4000"" />
      <xs:minLength value=""0"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Organization_SiteBrandingType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Organization_default_esuppliers_accountType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Organization_e_suppliersType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Organization_sales_rep_listType"">
    <xs:restriction base=""xs:string"">
      <xs:maxLength value=""4000"" />
      <xs:minLength value=""0"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:complexType name=""Organization_BTNextGenType"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""bt_org_id"" type=""Organization_bt_org_idType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""erp_market_type"" type=""Organization_erp_market_typeType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""web_market_type"" type=""Organization_web_market_typeType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""allowed_users_count"" type=""Organization_allowed_users_countType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""users_use_count"" type=""Organization_users_use_countType"" />
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""credit_card_list"" type=""Organization_credit_card_listType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""alias"" type=""Organization_aliasType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""contact_fax"" type=""Organization_contact_faxType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""contact_email"" type=""Organization_contact_emailType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""all_site_access"" type=""bit"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""all_warehouse"" type=""bit"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""gen_aff_sig"" type=""bit"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""is_active"" type=""bit"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""unit_type"" type=""Organization_unit_typeType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""web_order"" type=""bit"" />
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""fee_list"" type=""Organization_fee_listType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""active_date"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""default_book_account_id"" type=""Organization_default_book_account_idType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""default_entertainment_account_id"" type=""Organization_default_entertainment_account_idType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""address_id"" type=""Organization_address_idType"" />
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""account_list"" type=""Organization_account_listType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""entertainment_product"" type=""bit"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""table_of_contents"" type=""bit"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""subscription_start_date"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""subscription_end_date"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""fulltext_reviews"" type=""Organization_fulltext_reviewsType"" />
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""review_type_list"" type=""Organization_review_type_listType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""home_delivery_allowed"" type=""bit"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""admin_contact"" type=""Organization_admin_contactType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""status"" type=""Organization_statusType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""contact_phone"" type=""Organization_contact_phoneType"" />
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""user_list"" type=""Organization_user_listType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""account_count"" type=""Organization_account_countType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""user_count"" type=""Organization_user_countType"" />
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""shipping_method_list"" type=""Organization_shipping_method_listType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""personal_prod_url"" type=""Organization_personal_prod_urlType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""prod_lookup_index"" type=""Organization_prod_lookup_indexType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""isbn_lookup_code"" type=""Organization_isbn_lookup_codeType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""isbn_link_displayed"" type=""Organization_isbn_link_displayedType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""prod_suffix_lookup"" type=""Organization_prod_suffix_lookupType"" />
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""payment_option_list"" type=""Organization_payment_option_listType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""returns_message"" type=""Organization_returns_messageType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""marketing_message"" type=""Organization_marketing_messageType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""muze"" type=""bit"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""is_bt_employee"" type=""bit"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""has_reserve"" type=""bit"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""reserve_number"" type=""Organization_reserve_numberType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""subscription_list"" type=""Organization_subscription_listType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""product_lookup_list"" type=""Organization_product_lookup_listType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""u_contact_name"" type=""Organization_u_contact_nameType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""legacy_org_id"" type=""Organization_legacy_org_idType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""org_type"" type=""Organization_org_typeType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""library_membership"" type=""Organization_library_membershipType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""library_system_type"" type=""Organization_library_system_typeType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""country_code"" type=""Organization_country_codeType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""geo_code"" type=""Organization_geo_codeType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""market_types"" type=""Organization_market_typesType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""partial_ship_code"" type=""Organization_partial_ship_codeType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""partner"" type=""Organization_partnerType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""preferred_carrier_code"" type=""Organization_preferred_carrier_codeType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""san"" type=""Organization_sanType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""legacy_san"" type=""Organization_legacy_sanType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""legacy_source_system"" type=""Organization_legacy_source_systemType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""warehouses"" type=""Organization_warehousesType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""legacy_created_date"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""legacy_updated_date"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""SiteBranding"" type=""Organization_SiteBrandingType"" />
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""default_esuppliers_account"" type=""Organization_default_esuppliers_accountType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""original_entry"" type=""bit"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""grid_enabled"" type=""bit"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""shared_cart_workflow_enabled"" type=""bit"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""marc_profiler_enabled"" type=""bit"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""oclc_cataloging_plus_enabled"" type=""bit"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""show_bib_number"" type=""bit"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""is_full_marc_profile"" type=""bit"" />
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""e_suppliers"" type=""Organization_e_suppliersType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""slip_report"" type=""bit"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""library_system_handling"" type=""bit"" />
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""sales_rep_list"" type=""Organization_sales_rep_listType"" />
    </xs:sequence>
  </xs:complexType>
  <xs:complexType name=""OrganizationType"">
    <xs:sequence>
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""GeneralInfo"" type=""Organization_GeneralInfoType"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""ProfileSystem"" type=""Organization_ProfileSystemType"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""BTNextGen"" type=""Organization_BTNextGenType"" />
    </xs:sequence>
  </xs:complexType>
  <xs:simpleType name=""UserCredentials_user_idType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserCredentials_user_nameType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserCredentials_email_addressType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserCredentials_inactive_indicatorType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserCredentials_user_aliasType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserCredentials_user_passwordType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserCredentials_password_questionType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserCredentials_password_answerType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserCredentials_first_nameType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserCredentials_last_nameType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:complexType name=""UserCredentials_BTNextGenType"">
    <xs:sequence>
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""user_id"" type=""UserCredentials_user_idType"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""user_name"" type=""UserCredentials_user_nameType"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""email_address"" type=""UserCredentials_email_addressType"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""inactive_indicator"" type=""UserCredentials_inactive_indicatorType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""user_alias"" type=""UserCredentials_user_aliasType"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""user_password"" type=""UserCredentials_user_passwordType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""password_question"" type=""UserCredentials_password_questionType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""password_answer"" type=""UserCredentials_password_answerType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""first_name"" type=""UserCredentials_first_nameType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""last_name"" type=""UserCredentials_last_nameType"" />
    </xs:sequence>
  </xs:complexType>
  <xs:simpleType name=""UserCredentials_user_id_changed_byType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserCredentials_created_byType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UserCredentials_KeyIndexType"">
    <xs:restriction base=""xs:integer"">
      <xs:minInclusive value=""1"" />
      <xs:maxInclusive value=""2"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:complexType name=""UserCredentials_ProfileSystemType"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""user_id_changed_by"" type=""UserCredentials_user_id_changed_byType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""date_last_changed"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""created_by"" type=""UserCredentials_created_byType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""date_created"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""KeyIndex"" type=""UserCredentials_KeyIndexType"" />
    </xs:sequence>
  </xs:complexType>
  <xs:complexType name=""UserCredentialsType"">
    <xs:sequence>
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""BTNextGen"" type=""UserCredentials_BTNextGenType"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""ProfileSystem"" type=""UserCredentials_ProfileSystemType"" />
    </xs:sequence>
  </xs:complexType>
  <xs:simpleType name=""SalesRep_sales_rep_parent_idType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""SalesRep_source_systemType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""SalesRep_user_id_changed_byType"">
    <xs:restriction base=""xs:string"">
      <xs:maxLength value=""4000"" />
      <xs:minLength value=""0"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:complexType name=""SalesRep_ProfileSystemType"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""sales_rep_parent_id"" type=""SalesRep_sales_rep_parent_idType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""source_system"" type=""SalesRep_source_systemType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""date_last_changed"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""date_created"" type=""xs:dateTime"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""user_id_changed_by"" type=""SalesRep_user_id_changed_byType"" />
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""csadapter_date_last_changed"" type=""xs:dateTime"" />
    </xs:sequence>
  </xs:complexType>
  <xs:simpleType name=""SalesRep_sales_rep_idType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""SalesRep_sales_rep_nameType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:complexType name=""SalesRep_BTNextGenType"">
    <xs:sequence>
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""sales_rep_id"" type=""SalesRep_sales_rep_idType"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""sales_rep_name"" type=""SalesRep_sales_rep_nameType"" />
    </xs:sequence>
  </xs:complexType>
  <xs:complexType name=""SalesRepType"">
    <xs:sequence>
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""ProfileSystem"" type=""SalesRep_ProfileSystemType"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""BTNextGen"" type=""SalesRep_BTNextGenType"" />
    </xs:sequence>
  </xs:complexType>
  <xs:element name=""ProfileDocument"">
    <xs:complexType>
      <xs:sequence>
        <xs:element name=""UserObject"" type=""UserObjectType"" />
        <xs:element name=""Address"" type=""AddressType"" />
        <xs:element name=""BTAccount"" type=""BTAccountType"" />
        <xs:element name=""BTFee"" type=""BTFeeType"" />
        <xs:element name=""BTInvalidLoginAttempts"" type=""BTInvalidLoginAttemptsType"" />
        <xs:element name=""BTMyPreferences"" type=""BTMyPreferencesType"" />
        <xs:element name=""BTProductInterestGroup"" type=""BTProductInterestGroupType"" />
        <xs:element name=""BTProductLookup"" type=""BTProductLookupType"" />
        <xs:element name=""BTSavedSearch"" type=""BTSavedSearchType"" />
        <xs:element name=""BTShippingMethod"" type=""BTShippingMethodType"" />
        <xs:element name=""BTSiteBranding"" type=""BTSiteBrandingType"" />
        <xs:element name=""BTSubscription"" type=""BTSubscriptionType"" />
        <xs:element name=""BTUserAlert"" type=""BTUserAlertType"" />
        <xs:element name=""BTUserReviewType"" type=""BTUserReviewTypeType"" />
        <xs:element name=""BTWarehouse"" type=""BTWarehouseType"" />
        <xs:element name=""CreditCard"" type=""CreditCardType"" />
        <xs:element name=""Currency"" type=""CurrencyType"" />
        <xs:element name=""Organization"" type=""OrganizationType"" />
        <xs:element name=""UserCredentials"" type=""UserCredentialsType"" />
        <xs:element name=""SalesRep"" type=""SalesRepType"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public BTProfile() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "ProfileDocument";
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
