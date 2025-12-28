namespace BTNextGen.BizTalk.CS.Catalogue.Schemas {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"",@"MSCommerceCatalogCollection2")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"MSCommerceCatalogCollection2"})]
    public sealed class BTCatalog : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" attributeFormDefault=""unqualified"" elementFormDefault=""unqualified"" version=""1.0"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:simpleType name=""versionType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""3"" />
      <xs:pattern value=""3.0"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""ValueType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""128"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""languageType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""10"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""rankType"">
    <xs:restriction base=""xs:integer"">
      <xs:minInclusive value=""-2147483648"" />
      <xs:maxInclusive value=""2147483647"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""MinValueType"">
    <xs:restriction base=""xs:string"" />
  </xs:simpleType>
  <xs:simpleType name=""MaxValueType"">
    <xs:restriction base=""xs:string"" />
  </xs:simpleType>
  <xs:simpleType name=""PropertyNameType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""100"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""CategoryNameType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""213"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""CatalogNameType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""85"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""ProductIdType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""343"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""VariantIdType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""256"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""expressionType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""dataTypeType"">
    <xs:restriction base=""xs:string"">
      <xs:enumeration value=""number"" />
      <xs:enumeration value=""bignumber"" />
      <xs:enumeration value=""float"" />
      <xs:enumeration value=""real"" />
      <xs:enumeration value=""boolean"" />
      <xs:enumeration value=""string"" />
      <xs:enumeration value=""datetime"" />
      <xs:enumeration value=""currency"" />
      <xs:enumeration value=""filename"" />
      <xs:enumeration value=""enumeration"" />
      <xs:enumeration value=""text"" />
      <xs:enumeration value=""money"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""ruleTypeType"">
    <xs:restriction base=""xs:string"">
      <xs:enumeration value=""NoPriceChange"" />
      <xs:enumeration value=""PercentageMultiplier"" />
      <xs:enumeration value=""AddFixedAmount"" />
      <xs:enumeration value=""ReplacePrice"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""idType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""DefinitionTypeType"">
    <xs:restriction base=""xs:string"">
      <xs:enumeration value=""product"" />
      <xs:enumeration value=""category"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""DefinitionDescriptionType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""256"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""CatalogSetDescriptionType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""255"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""currencyType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""128"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""lastmodifiedType"">
    <xs:restriction base=""xs:dateTime"">
      <xs:minInclusive value=""1753-01-01T00:00:00"" />
      <xs:maxInclusive value=""9999-12-31T23:59:59"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""languagesType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""IsSearchableType"">
    <xs:restriction base=""xs:boolean"" />
  </xs:simpleType>
  <xs:simpleType name=""UseCategoryPricingType"">
    <xs:restriction base=""xs:boolean"" />
  </xs:simpleType>
  <xs:complexType name=""DisplayNameType"">
    <xs:attribute name=""Value"" type=""ValueType"" use=""required"" />
    <xs:attribute name=""language"" type=""languageType"" use=""required"" />
  </xs:complexType>
  <xs:complexType name=""PropertyDisplayNameType"">
    <xs:attribute name=""Value"" type=""ValueType"" use=""required"" />
    <xs:attribute name=""language"" type=""languageType"" use=""optional"" />
  </xs:complexType>
  <xs:complexType name=""DefaultValueType"">
    <xs:attribute name=""Value"" type=""ValueType"" use=""required"" />
    <xs:attribute name=""language"" type=""languageType"" use=""optional"" />
  </xs:complexType>
  <xs:simpleType name=""nameType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""100"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""IsFreeTextSearchableType"">
    <xs:restriction base=""xs:boolean"" />
  </xs:simpleType>
  <xs:simpleType name=""IncludeInSpecSearchType"">
    <xs:restriction base=""xs:boolean"" />
  </xs:simpleType>
  <xs:simpleType name=""DisplayOnSiteType"">
    <xs:restriction base=""xs:boolean"" />
  </xs:simpleType>
  <xs:simpleType name=""AssignAllType"">
    <xs:restriction base=""xs:boolean"" />
  </xs:simpleType>
  <xs:simpleType name=""ExportToDWType"">
    <xs:restriction base=""xs:boolean"" />
  </xs:simpleType>
  <xs:simpleType name=""DisplayInProductsListType"">
    <xs:restriction base=""xs:boolean"" />
  </xs:simpleType>
  <xs:simpleType name=""MultilingualType"">
    <xs:restriction base=""xs:boolean"" />
  </xs:simpleType>
  <xs:simpleType name=""IsRequiredType"">
    <xs:restriction base=""xs:boolean"" />
  </xs:simpleType>
  <xs:simpleType name=""BuiltInType"">
    <xs:restriction base=""xs:boolean"" />
  </xs:simpleType>
  <xs:simpleType name=""DisplayAsBaseType"">
    <xs:restriction base=""xs:boolean"" />
  </xs:simpleType>
  <xs:simpleType name=""localeType"">
    <xs:restriction base=""xs:integer"">
      <xs:minInclusive value=""-2147483648"" />
      <xs:maxInclusive value=""2147483647"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""startDateType"">
    <xs:restriction base=""xs:dateTime"">
      <xs:minInclusive value=""1753-01-01T00:00:00"" />
      <xs:maxInclusive value=""9999-12-31T23:59:59"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""endDateType"">
    <xs:restriction base=""xs:dateTime"">
      <xs:minInclusive value=""1753-01-01T00:00:00"" />
      <xs:maxInclusive value=""9999-12-31T23:59:59"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""variantUIDType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""256"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""productUIDType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""256"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""weight_measuring_unitType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""128"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""DefaultLanguageType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""10"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""ReportingLanguageType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""10"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BrandType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTAccumulatorTypeType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""20"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTAffidavitOnSaleDateType"">
    <xs:restriction base=""xs:dateTime"">
      <xs:minInclusive value=""1800-01-01T00:00:00"" />
      <xs:maxInclusive value=""9000-01-01T00:00:00"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTAudienceType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""255"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTBlowOutInactiveFlagType"">
    <xs:restriction base=""xs:boolean"" />
  </xs:simpleType>
  <xs:simpleType name=""BTBookClassificationCodeType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""10"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTBookClassificationDescType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""255"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTBoxCostType"">
    <xs:restriction base=""xs:decimal"">
      <xs:minInclusive value=""-922337203685477.5808"" />
      <xs:maxInclusive value=""922337203685477.5807"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTCatalogCodeType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""10"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTDealerCostType"">
    <xs:restriction base=""xs:decimal"">
      <xs:minInclusive value=""-922337203685477.5808"" />
      <xs:maxInclusive value=""922337203685477.5807"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTDiscountFlagType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""1"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTGroupCodeType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTInternationalListPriceType"">
    <xs:restriction base=""xs:decimal"">
      <xs:minInclusive value=""-922337203685477.5808"" />
      <xs:maxInclusive value=""922337203685477.5807"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTInternationalListPriceCurrencyCodeType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""5"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTInternationalNetPriceType"">
    <xs:restriction base=""xs:decimal"">
      <xs:minInclusive value=""-922337203685477.5808"" />
      <xs:maxInclusive value=""922337203685477.5807"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTInternationalNetPriceCurrencyCodeType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""5"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTIsActiveType"">
    <xs:restriction base=""xs:boolean"" />
  </xs:simpleType>
  <xs:simpleType name=""BTISBNType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""13"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTISBN10Type"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""10"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTItemGroupType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTKeyType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""255"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTLanguageCodeType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""10"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTManufacturerCodeType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTMarketExclusionsType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""3"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTMarketRightsType"">
    <xs:restriction base=""xs:string"" />
  </xs:simpleType>
  <xs:simpleType name=""BTMerchandiseCategoryType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTMSRPListPriceType"">
    <xs:restriction base=""xs:decimal"">
      <xs:minInclusive value=""-922337203685477.5808"" />
      <xs:maxInclusive value=""922337203685477.5807"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTMSRPListPriceCurrencyCodeType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""5"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTNumberofVolumesType"">
    <xs:restriction base=""xs:integer"">
      <xs:minInclusive value=""-2147483648"" />
      <xs:maxInclusive value=""2147483647"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTOnSaleDateType"">
    <xs:restriction base=""xs:dateTime"">
      <xs:minInclusive value=""1800-01-01T00:00:00"" />
      <xs:maxInclusive value=""2099-01-01T00:00:00"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTPartNumberType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTPreOrderDateType"">
    <xs:restriction base=""xs:dateTime"">
      <xs:minInclusive value=""1900-01-01T00:00:00"" />
      <xs:maxInclusive value=""9000-01-01T00:00:00"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTPriceCodeType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""10"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTPriceKeyType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""10"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTPricePackType"">
    <xs:restriction base=""xs:integer"">
      <xs:minInclusive value=""-2147483648"" />
      <xs:maxInclusive value=""2147483647"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTProductCodeType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTProductLineType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""255"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTProductTypeType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""255"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTPubCodeDType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTPublisherType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""255"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTPubStatusType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTQuantityPackType"">
    <xs:restriction base=""xs:integer"">
      <xs:minInclusive value=""-2147483648"" />
      <xs:maxInclusive value=""2147483647"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTReleaseDateType"">
    <xs:restriction base=""xs:dateTime"">
      <xs:minInclusive value=""1800-01-01T00:00:00"" />
      <xs:maxInclusive value=""9998-12-31T00:00:00"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTReservedInventoryFlagType"">
    <xs:restriction base=""xs:boolean"" />
  </xs:simpleType>
  <xs:simpleType name=""BTResponsiblePartyType"">
    <xs:restriction base=""xs:string"" />
  </xs:simpleType>
  <xs:simpleType name=""BTRetailPriceType"">
    <xs:restriction base=""xs:decimal"">
      <xs:minInclusive value=""-922337203685477.5808"" />
      <xs:maxInclusive value=""922337203685477.5807"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTRetailPriceCheckType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""5"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTReviewFlagType"">
    <xs:restriction base=""xs:boolean"" />
  </xs:simpleType>
  <xs:simpleType name=""BTReviewsType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTSpecialBoxCostType"">
    <xs:restriction base=""xs:decimal"">
      <xs:minInclusive value=""-922337203685477.5808"" />
      <xs:maxInclusive value=""922337203685477.5807"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTSpecialCostType"">
    <xs:restriction base=""xs:decimal"">
      <xs:minInclusive value=""-922337203685477.5808"" />
      <xs:maxInclusive value=""922337203685477.5807"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTSubTitleType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""255"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTTOLASVendorNumberType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""8"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTUnitCostType"">
    <xs:restriction base=""xs:decimal"">
      <xs:minInclusive value=""-922337203685477.5808"" />
      <xs:maxInclusive value=""922337203685477.5807"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTUPCType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""14"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BTVendorNumberType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""CategoryPage_Multilingual_Type"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""1024"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:complexType name=""CategoryPageType"">
    <xs:attribute name=""Value"" type=""CategoryPage_Multilingual_Type"" use=""required"" />
    <xs:attribute name=""language"" type=""languageType"" use=""required"" />
  </xs:complexType>
  <xs:simpleType name=""Description_Multilingual_Type"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""1"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:complexType name=""DescriptionType"">
    <xs:attribute name=""Value"" type=""Description_Multilingual_Type"" use=""required"" />
    <xs:attribute name=""language"" type=""languageType"" use=""required"" />
  </xs:complexType>
  <xs:simpleType name=""Image_filenameType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""256"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Image_heightType"">
    <xs:restriction base=""xs:integer"">
      <xs:minInclusive value=""16"" />
      <xs:maxInclusive value=""800"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Image_widthType"">
    <xs:restriction base=""xs:integer"">
      <xs:minInclusive value=""16"" />
      <xs:maxInclusive value=""1200"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""ProductTypeType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""50"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Rating_Multilingual_Type"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:complexType name=""RatingType"">
    <xs:attribute name=""Value"" type=""Rating_Multilingual_Type"" use=""required"" />
    <xs:attribute name=""language"" type=""languageType"" use=""required"" />
  </xs:complexType>
  <xs:simpleType name=""RelationshipDescriptionValueType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""256"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""DefinitionType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""128"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""PrimaryParentCategoryType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""128"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""listpriceType"">
    <xs:restriction base=""xs:decimal"">
      <xs:minInclusive value=""-922337203685477.5808"" />
      <xs:maxInclusive value=""922337203685477.5807"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:complexType name=""DefPropertyType"">
    <xs:simpleContent>
      <xs:extension base=""nameType"" />
    </xs:simpleContent>
  </xs:complexType>
  <xs:complexType name=""DefVariantPropertyType"">
    <xs:simpleContent>
      <xs:extension base=""nameType"" />
    </xs:simpleContent>
  </xs:complexType>
  <xs:complexType name=""PropertyValueType"">
    <xs:attribute name=""DisplayName"" type=""ValueType"" use=""required"" />
    <xs:attribute name=""language"" type=""languageType"" use=""optional"" />
    <xs:attribute name=""rank"" type=""rankType"" use=""optional"" />
  </xs:complexType>
  <xs:complexType name=""ParentCategoryType"">
    <xs:simpleContent>
      <xs:extension base=""CategoryNameType"">
        <xs:attribute name=""rank"" type=""rankType"" use=""optional"" />
      </xs:extension>
    </xs:simpleContent>
  </xs:complexType>
  <xs:complexType name=""RelationshipDescriptionType"">
    <xs:attribute name=""Value"" type=""RelationshipDescriptionValueType"" use=""required"" />
    <xs:attribute name=""language"" type=""languageType"" use=""required"" />
  </xs:complexType>
  <xs:complexType name=""RelationshipType"">
    <xs:choice minOccurs=""0"" maxOccurs=""unbounded"">
      <xs:element name=""RelationshipDescription"" type=""RelationshipDescriptionType"" />
    </xs:choice>
    <xs:attribute name=""name"" type=""nameType"" use=""required"" />
    <xs:attribute name=""targetCatalog"" type=""CatalogNameType"" use=""required"" />
    <xs:attribute name=""targetCategory"" type=""CategoryNameType"" use=""optional"" />
    <xs:attribute name=""targetProduct"" type=""ProductIdType"" use=""optional"" />
    <xs:attribute name=""rank"" type=""rankType"" use=""optional"" />
  </xs:complexType>
  <xs:complexType name=""MSCommerceCatalogCollection2Type"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""CatalogSchema"" type=""CatalogSchemaType"" />
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""Catalog"" type=""CatalogType"" />
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""VirtualCatalog"" type=""VirtualCatalogType"" />
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""CatalogSet"" type=""CatalogSetType"" />
    </xs:sequence>
    <xs:attribute name=""version"" type=""versionType"" use=""required"" />
  </xs:complexType>
  <xs:complexType name=""CatalogSchemaType"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""Property"" type=""PropertyType"" />
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""EntityExtensions"" type=""EntityExtensionsType"" />
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""Definition"" type=""DefinitionElementType"" />
    </xs:sequence>
  </xs:complexType>
  <xs:complexType name=""EntityExtensionsType"">
    <xs:choice minOccurs=""0"" maxOccurs=""unbounded"">
      <xs:element name=""PropertyDefinition"" type=""xs:string"" />
      <xs:element name=""ProductCatalog"" type=""xs:string"" />
    </xs:choice>
  </xs:complexType>
  <xs:complexType name=""PropertyType"">
    <xs:choice minOccurs=""0"" maxOccurs=""unbounded"">
      <xs:element name=""DisplayName"" type=""PropertyDisplayNameType"" />
      <xs:element name=""DefaultValue"" type=""DefaultValueType"" />
      <xs:element name=""PropertyValue"" type=""PropertyValueType"" />
    </xs:choice>
    <xs:attribute name=""MinValue"" type=""MinValueType"" use=""optional"" />
    <xs:attribute name=""MaxValue"" type=""MaxValueType"" use=""optional"" />
    <xs:attribute name=""dataType"" type=""dataTypeType"" use=""optional"" />
    <xs:attribute name=""id"" type=""idType"" use=""optional"" />
    <xs:attribute name=""currency"" type=""currencyType"" use=""optional"" />
    <xs:attribute name=""name"" type=""PropertyNameType"" use=""required"" />
    <xs:attribute name=""IsFreeTextSearchable"" type=""IsFreeTextSearchableType"" use=""optional"" />
    <xs:attribute name=""IncludeInSpecSearch"" type=""IncludeInSpecSearchType"" use=""optional"" />
    <xs:attribute name=""DisplayOnSite"" type=""DisplayOnSiteType"" use=""optional"" />
    <xs:attribute name=""AssignAll"" type=""AssignAllType"" use=""optional"" />
    <xs:attribute name=""ExportToDW"" type=""ExportToDWType"" use=""optional"" />
    <xs:attribute name=""DisplayInProductsList"" type=""DisplayInProductsListType"" use=""optional"" />
    <xs:attribute name=""Multilingual"" type=""MultilingualType"" use=""optional"" />
    <xs:attribute name=""IsRequired"" type=""IsRequiredType"" use=""optional"" />
    <xs:attribute name=""BuiltIn"" type=""BuiltInType"" use=""optional"" />
    <xs:attribute name=""DisplayAsBase"" type=""DisplayAsBaseType"" use=""optional"" />
  </xs:complexType>
  <xs:complexType name=""DefinitionElementType"">
    <xs:choice minOccurs=""0"" maxOccurs=""unbounded"">
      <xs:element name=""DefProperty"" type=""DefPropertyType"" />
      <xs:element name=""DefVariantProperty"" type=""DefVariantPropertyType"" />
    </xs:choice>
    <xs:attribute name=""name"" type=""nameType"" use=""required"" />
    <xs:attribute name=""DefinitionType"" type=""DefinitionTypeType"" use=""required"" />
    <xs:attribute name=""description"" type=""DefinitionDescriptionType"" use=""optional"" />
  </xs:complexType>
  <xs:complexType name=""IncludeRuleType"">
    <xs:attribute name=""baseCatalog"" type=""CatalogNameType"" use=""required"" />
    <xs:attribute name=""Category"" type=""CategoryNameType"" use=""optional"" />
    <xs:attribute name=""ProductId"" type=""ProductIdType"" use=""optional"" />
    <xs:attribute name=""VariantId"" type=""VariantIdType"" use=""optional"" />
  </xs:complexType>
  <xs:complexType name=""ExcludeRuleType"">
    <xs:attribute name=""baseCatalog"" type=""CatalogNameType"" use=""required"" />
    <xs:attribute name=""Category"" type=""CategoryNameType"" use=""optional"" />
    <xs:attribute name=""ProductId"" type=""ProductIdType"" use=""optional"" />
    <xs:attribute name=""VariantId"" type=""VariantIdType"" use=""optional"" />
  </xs:complexType>
  <xs:complexType name=""PriceRuleType"">
    <xs:attribute name=""baseCatalog"" type=""CatalogNameType"" use=""required"" />
    <xs:attribute name=""Category"" type=""CategoryNameType"" use=""optional"" />
    <xs:attribute name=""ProductId"" type=""ProductIdType"" use=""optional"" />
    <xs:attribute name=""VariantId"" type=""VariantIdType"" use=""optional"" />
    <xs:attribute name=""ruleType"" type=""ruleTypeType"" use=""required"" />
    <xs:attribute name=""amount"" type=""listpriceType"" use=""optional"" />
  </xs:complexType>
  <xs:complexType name=""CatalogType"">
    <xs:choice minOccurs=""0"" maxOccurs=""unbounded"">
      <xs:element name=""DisplayName"" type=""DisplayNameType"" />
      <xs:element name=""Category"" type=""CategoryType"" />
      <xs:element name=""Product"" type=""ProductType"" />
    </xs:choice>
    <xs:attribute name=""currency"" type=""currencyType"" use=""optional"" />
    <xs:attribute name=""lastmodified"" type=""lastmodifiedType"" use=""optional"" />
    <xs:attribute name=""languages"" type=""languagesType"" use=""optional"" />
    <xs:attribute name=""name"" type=""CatalogNameType"" use=""required"" />
    <xs:attribute name=""locale"" type=""localeType"" use=""optional"" />
    <xs:attribute name=""startDate"" type=""startDateType"" use=""optional"" />
    <xs:attribute name=""endDate"" type=""endDateType"" use=""optional"" />
    <xs:attribute name=""variantUID"" type=""PropertyNameType"" use=""optional"" />
    <xs:attribute name=""productUID"" type=""PropertyNameType"" use=""required"" />
    <xs:attribute name=""weight_measuring_unit"" type=""weight_measuring_unitType"" use=""optional"" />
    <xs:attribute name=""DefaultLanguage"" type=""languageType"" use=""required"" />
    <xs:attribute name=""ReportingLanguage"" type=""languageType"" use=""required"" />
  </xs:complexType>
  <xs:complexType name=""CategoryType"">
    <xs:choice minOccurs=""0"" maxOccurs=""unbounded"">
      <xs:element name=""DisplayName"" type=""DisplayNameType"" />
      <xs:element name=""CategoryPage"" type=""CategoryPageType"" />
      <xs:element name=""Description"" type=""DescriptionType"" />
      <xs:element name=""ParentCategory"" type=""ParentCategoryType"" />
      <xs:element name=""Relationship"" type=""RelationshipType"" />
    </xs:choice>
    <xs:attribute name=""name"" type=""CategoryNameType"" use=""required"" />
    <xs:attribute name=""rank"" type=""rankType"" use=""optional"" />
    <xs:attribute name=""id"" type=""idType"" use=""optional"" />
    <xs:attribute name=""lastmodified"" type=""lastmodifiedType"" use=""optional"" />
    <xs:attribute name=""IsSearchable"" type=""IsSearchableType"" use=""optional"" />
    <xs:attribute name=""Brand"" type=""BrandType"" use=""optional"" />
    <xs:attribute name=""Image_filename"" type=""Image_filenameType"" use=""optional"" />
    <xs:attribute name=""Image_height"" type=""Image_heightType"" use=""optional"" />
    <xs:attribute name=""Image_width"" type=""Image_widthType"" use=""optional"" />
    <xs:attribute name=""Definition"" type=""DefinitionType"" use=""optional"" />
    <xs:attribute name=""PrimaryParentCategory"" type=""PrimaryParentCategoryType"" use=""optional"" />
    <xs:attribute name=""listprice"" type=""listpriceType"" use=""optional"" />
  </xs:complexType>
  <xs:complexType name=""ProductType"">
    <xs:choice minOccurs=""0"" maxOccurs=""unbounded"">
      <xs:element name=""DisplayName"" type=""DisplayNameType"" />
      <xs:element name=""Description"" type=""DescriptionType"" />
      <xs:element name=""Rating"" type=""RatingType"" />
      <xs:element name=""ParentCategory"" type=""ParentCategoryType"" />
      <xs:element name=""Relationship"" type=""RelationshipType"" />
      <xs:element name=""ProductVariant"" type=""ProductVariantType"" />
    </xs:choice>
    <xs:attribute name=""rank"" type=""rankType"" use=""optional"" />
    <xs:attribute name=""id"" type=""idType"" use=""optional"" />
    <xs:attribute name=""lastmodified"" type=""lastmodifiedType"" use=""optional"" />
    <xs:attribute name=""UseCategoryPricing"" type=""UseCategoryPricingType"" use=""optional"" />
    <xs:attribute name=""name"" type=""nameType"" use=""optional"" />
    <xs:attribute name=""BTAccumulatorType"" type=""BTAccumulatorTypeType"" use=""optional"" />
    <xs:attribute name=""BTAffidavitOnSaleDate"" type=""BTAffidavitOnSaleDateType"" use=""optional"" />
    <xs:attribute name=""BTAudience"" type=""BTAudienceType"" use=""optional"" />
    <xs:attribute name=""BTBlowOutInactiveFlag"" type=""BTBlowOutInactiveFlagType"" use=""optional"" />
    <xs:attribute name=""BTBookClassificationCode"" type=""BTBookClassificationCodeType"" use=""optional"" />
    <xs:attribute name=""BTBookClassificationDesc"" type=""BTBookClassificationDescType"" use=""optional"" />
    <xs:attribute name=""BTBoxCost"" type=""BTBoxCostType"" use=""optional"" />
    <xs:attribute name=""BTCatalogCode"" type=""BTCatalogCodeType"" use=""optional"" />
    <xs:attribute name=""BTDealerCost"" type=""BTDealerCostType"" use=""optional"" />
    <xs:attribute name=""BTDiscountFlag"" type=""BTDiscountFlagType"" use=""optional"" />
    <xs:attribute name=""BTGroupCode"" type=""BTGroupCodeType"" use=""optional"" />
    <xs:attribute name=""BTInternationalListPrice"" type=""BTInternationalListPriceType"" use=""optional"" />
    <xs:attribute name=""BTInternationalListPriceCurrencyCode"" type=""BTInternationalListPriceCurrencyCodeType"" use=""optional"" />
    <xs:attribute name=""BTInternationalNetPrice"" type=""BTInternationalNetPriceType"" use=""optional"" />
    <xs:attribute name=""BTInternationalNetPriceCurrencyCode"" type=""BTInternationalNetPriceCurrencyCodeType"" use=""optional"" />
    <xs:attribute name=""BTIsActive"" type=""BTIsActiveType"" use=""optional"" />
    <xs:attribute name=""BTISBN"" type=""BTISBNType"" use=""optional"" />
    <xs:attribute name=""BTISBN10"" type=""BTISBN10Type"" use=""optional"" />
    <xs:attribute name=""BTItemGroup"" type=""BTItemGroupType"" use=""optional"" />
    <xs:attribute name=""BTKey"" type=""BTKeyType"" use=""optional"" />
    <xs:attribute name=""BTLanguageCode"" type=""BTLanguageCodeType"" use=""optional"" />
    <xs:attribute name=""BTManufacturerCode"" type=""BTManufacturerCodeType"" use=""optional"" />
    <xs:attribute name=""BTMarketExclusions"" type=""BTMarketExclusionsType"" use=""optional"" />
    <xs:attribute name=""BTMarketRights"" type=""BTMarketRightsType"" use=""optional"" />
    <xs:attribute name=""BTMerchandiseCategory"" type=""BTMerchandiseCategoryType"" use=""optional"" />
    <xs:attribute name=""BTMSRPListPrice"" type=""BTMSRPListPriceType"" use=""optional"" />
    <xs:attribute name=""BTMSRPListPriceCurrencyCode"" type=""BTMSRPListPriceCurrencyCodeType"" use=""optional"" />
    <xs:attribute name=""BTNumberofVolumes"" type=""BTNumberofVolumesType"" use=""optional"" />
    <xs:attribute name=""BTOnSaleDate"" type=""BTOnSaleDateType"" use=""optional"" />
    <xs:attribute name=""BTPartNumber"" type=""BTPartNumberType"" use=""optional"" />
    <xs:attribute name=""BTPreOrderDate"" type=""BTPreOrderDateType"" use=""optional"" />
    <xs:attribute name=""BTPriceCode"" type=""BTPriceCodeType"" use=""optional"" />
    <xs:attribute name=""BTPriceKey"" type=""BTPriceKeyType"" use=""optional"" />
    <xs:attribute name=""BTPricePack"" type=""BTPricePackType"" use=""optional"" />
    <xs:attribute name=""BTProductCode"" type=""BTProductCodeType"" use=""optional"" />
    <xs:attribute name=""BTProductLine"" type=""BTProductLineType"" use=""optional"" />
    <xs:attribute name=""BTProductType"" type=""BTProductTypeType"" use=""optional"" />
    <xs:attribute name=""BTPubCodeD"" type=""BTPubCodeDType"" use=""optional"" />
    <xs:attribute name=""BTPublisher"" type=""BTPublisherType"" use=""optional"" />
    <xs:attribute name=""BTPubStatus"" type=""BTPubStatusType"" use=""optional"" />
    <xs:attribute name=""BTQuantityPack"" type=""BTQuantityPackType"" use=""optional"" />
    <xs:attribute name=""BTReleaseDate"" type=""BTReleaseDateType"" use=""optional"" />
    <xs:attribute name=""BTReservedInventoryFlag"" type=""BTReservedInventoryFlagType"" use=""optional"" />
    <xs:attribute name=""BTResponsibleParty"" type=""BTResponsiblePartyType"" use=""optional"" />
    <xs:attribute name=""BTRetailPrice"" type=""BTRetailPriceType"" use=""optional"" />
    <xs:attribute name=""BTRetailPriceCheck"" type=""BTRetailPriceCheckType"" use=""optional"" />
    <xs:attribute name=""BTReviewFlag"" type=""BTReviewFlagType"" use=""optional"" />
    <xs:attribute name=""BTReviews"" type=""BTReviewsType"" use=""optional"" />
    <xs:attribute name=""BTSpecialBoxCost"" type=""BTSpecialBoxCostType"" use=""optional"" />
    <xs:attribute name=""BTSpecialCost"" type=""BTSpecialCostType"" use=""optional"" />
    <xs:attribute name=""BTSubTitle"" type=""BTSubTitleType"" use=""optional"" />
    <xs:attribute name=""BTTOLASVendorNumber"" type=""BTTOLASVendorNumberType"" use=""optional"" />
    <xs:attribute name=""BTUnitCost"" type=""BTUnitCostType"" use=""optional"" />
    <xs:attribute name=""BTUPC"" type=""BTUPCType"" use=""optional"" />
    <xs:attribute name=""BTVendorNumber"" type=""BTVendorNumberType"" use=""optional"" />
    <xs:attribute name=""ProductType"" type=""ProductTypeType"" use=""optional"" />
    <xs:attribute name=""Definition"" type=""DefinitionType"" use=""optional"" />
    <xs:attribute name=""PrimaryParentCategory"" type=""PrimaryParentCategoryType"" use=""optional"" />
    <xs:attribute name=""listprice"" type=""listpriceType"" use=""optional"" />
  </xs:complexType>
  <xs:complexType name=""ProductVariantType"">
    <xs:choice minOccurs=""0"" maxOccurs=""unbounded"">
      <xs:element name=""DisplayName"" type=""DisplayNameType"" />
    </xs:choice>
    <xs:attribute name=""rank"" type=""rankType"" use=""optional"" />
    <xs:attribute name=""VariantId"" type=""VariantIdType"" use=""optional"" />
    <xs:attribute name=""id"" type=""idType"" use=""optional"" />
    <xs:attribute name=""lastmodified"" type=""lastmodifiedType"" use=""optional"" />
    <xs:attribute name=""listprice"" type=""listpriceType"" use=""optional"" />
  </xs:complexType>
  <xs:complexType name=""VirtualCatalogType"">
    <xs:choice minOccurs=""0"" maxOccurs=""unbounded"">
      <xs:element name=""DisplayName"" type=""DisplayNameType"" />
      <xs:element name=""Category"" type=""CategoryType"" />
      <xs:element name=""IncludeRule"" type=""IncludeRuleType"" />
      <xs:element name=""ExcludeRule"" type=""ExcludeRuleType"" />
      <xs:element name=""PriceRule"" type=""PriceRuleType"" />
      <xs:element name=""VirtualCategory"" type=""CategoryType"" />
      <xs:element name=""VirtualProduct"" type=""VirtualProductType"" />
    </xs:choice>
    <xs:attribute name=""currency"" type=""currencyType"" use=""optional"" />
    <xs:attribute name=""lastmodified"" type=""lastmodifiedType"" use=""optional"" />
    <xs:attribute name=""languages"" type=""languagesType"" use=""optional"" />
    <xs:attribute name=""name"" type=""CatalogNameType"" use=""required"" />
    <xs:attribute name=""locale"" type=""localeType"" use=""optional"" />
    <xs:attribute name=""startDate"" type=""startDateType"" use=""optional"" />
    <xs:attribute name=""endDate"" type=""endDateType"" use=""optional"" />
    <xs:attribute name=""weight_measuring_unit"" type=""weight_measuring_unitType"" use=""optional"" />
    <xs:attribute name=""DefaultLanguage"" type=""languageType"" use=""required"" />
    <xs:attribute name=""ReportingLanguage"" type=""languageType"" use=""required"" />
  </xs:complexType>
  <xs:complexType name=""VirtualProductType"">
    <xs:choice minOccurs=""0"" maxOccurs=""unbounded"">
      <xs:element name=""DisplayName"" type=""DisplayNameType"" />
      <xs:element name=""Description"" type=""DescriptionType"" />
      <xs:element name=""Rating"" type=""RatingType"" />
      <xs:element name=""ParentCategory"" type=""ParentCategoryType"" />
      <xs:element name=""Relationship"" type=""RelationshipType"" />
      <xs:element name=""VirtualProductVariant"" type=""VirtualProductVariantType"" />
    </xs:choice>
    <xs:attribute name=""rank"" type=""rankType"" use=""optional"" />
    <xs:attribute name=""id"" type=""idType"" use=""optional"" />
    <xs:attribute name=""lastmodified"" type=""lastmodifiedType"" use=""optional"" />
    <xs:attribute name=""UseCategoryPricing"" type=""UseCategoryPricingType"" use=""optional"" />
    <xs:attribute name=""name"" type=""nameType"" use=""optional"" />
    <xs:attribute name=""BTAccumulatorType"" type=""BTAccumulatorTypeType"" use=""optional"" />
    <xs:attribute name=""BTAffidavitOnSaleDate"" type=""BTAffidavitOnSaleDateType"" use=""optional"" />
    <xs:attribute name=""BTAudience"" type=""BTAudienceType"" use=""optional"" />
    <xs:attribute name=""BTBlowOutInactiveFlag"" type=""BTBlowOutInactiveFlagType"" use=""optional"" />
    <xs:attribute name=""BTBookClassificationCode"" type=""BTBookClassificationCodeType"" use=""optional"" />
    <xs:attribute name=""BTBookClassificationDesc"" type=""BTBookClassificationDescType"" use=""optional"" />
    <xs:attribute name=""BTBoxCost"" type=""BTBoxCostType"" use=""optional"" />
    <xs:attribute name=""BTCatalogCode"" type=""BTCatalogCodeType"" use=""optional"" />
    <xs:attribute name=""BTDealerCost"" type=""BTDealerCostType"" use=""optional"" />
    <xs:attribute name=""BTDiscountFlag"" type=""BTDiscountFlagType"" use=""optional"" />
    <xs:attribute name=""BTGroupCode"" type=""BTGroupCodeType"" use=""optional"" />
    <xs:attribute name=""BTInternationalListPrice"" type=""BTInternationalListPriceType"" use=""optional"" />
    <xs:attribute name=""BTInternationalListPriceCurrencyCode"" type=""BTInternationalListPriceCurrencyCodeType"" use=""optional"" />
    <xs:attribute name=""BTInternationalNetPrice"" type=""BTInternationalNetPriceType"" use=""optional"" />
    <xs:attribute name=""BTInternationalNetPriceCurrencyCode"" type=""BTInternationalNetPriceCurrencyCodeType"" use=""optional"" />
    <xs:attribute name=""BTIsActive"" type=""BTIsActiveType"" use=""optional"" />
    <xs:attribute name=""BTISBN"" type=""BTISBNType"" use=""optional"" />
    <xs:attribute name=""BTISBN10"" type=""BTISBN10Type"" use=""optional"" />
    <xs:attribute name=""BTItemGroup"" type=""BTItemGroupType"" use=""optional"" />
    <xs:attribute name=""BTKey"" type=""BTKeyType"" use=""optional"" />
    <xs:attribute name=""BTLanguageCode"" type=""BTLanguageCodeType"" use=""optional"" />
    <xs:attribute name=""BTManufacturerCode"" type=""BTManufacturerCodeType"" use=""optional"" />
    <xs:attribute name=""BTMarketExclusions"" type=""BTMarketExclusionsType"" use=""optional"" />
    <xs:attribute name=""BTMarketRights"" type=""BTMarketRightsType"" use=""optional"" />
    <xs:attribute name=""BTMerchandiseCategory"" type=""BTMerchandiseCategoryType"" use=""optional"" />
    <xs:attribute name=""BTMSRPListPrice"" type=""BTMSRPListPriceType"" use=""optional"" />
    <xs:attribute name=""BTMSRPListPriceCurrencyCode"" type=""BTMSRPListPriceCurrencyCodeType"" use=""optional"" />
    <xs:attribute name=""BTNumberofVolumes"" type=""BTNumberofVolumesType"" use=""optional"" />
    <xs:attribute name=""BTOnSaleDate"" type=""BTOnSaleDateType"" use=""optional"" />
    <xs:attribute name=""BTPartNumber"" type=""BTPartNumberType"" use=""optional"" />
    <xs:attribute name=""BTPreOrderDate"" type=""BTPreOrderDateType"" use=""optional"" />
    <xs:attribute name=""BTPriceCode"" type=""BTPriceCodeType"" use=""optional"" />
    <xs:attribute name=""BTPriceKey"" type=""BTPriceKeyType"" use=""optional"" />
    <xs:attribute name=""BTPricePack"" type=""BTPricePackType"" use=""optional"" />
    <xs:attribute name=""BTProductCode"" type=""BTProductCodeType"" use=""optional"" />
    <xs:attribute name=""BTProductLine"" type=""BTProductLineType"" use=""optional"" />
    <xs:attribute name=""BTProductType"" type=""BTProductTypeType"" use=""optional"" />
    <xs:attribute name=""BTPubCodeD"" type=""BTPubCodeDType"" use=""optional"" />
    <xs:attribute name=""BTPublisher"" type=""BTPublisherType"" use=""optional"" />
    <xs:attribute name=""BTPubStatus"" type=""BTPubStatusType"" use=""optional"" />
    <xs:attribute name=""BTQuantityPack"" type=""BTQuantityPackType"" use=""optional"" />
    <xs:attribute name=""BTReleaseDate"" type=""BTReleaseDateType"" use=""optional"" />
    <xs:attribute name=""BTReservedInventoryFlag"" type=""BTReservedInventoryFlagType"" use=""optional"" />
    <xs:attribute name=""BTResponsibleParty"" type=""BTResponsiblePartyType"" use=""optional"" />
    <xs:attribute name=""BTRetailPrice"" type=""BTRetailPriceType"" use=""optional"" />
    <xs:attribute name=""BTRetailPriceCheck"" type=""BTRetailPriceCheckType"" use=""optional"" />
    <xs:attribute name=""BTReviewFlag"" type=""BTReviewFlagType"" use=""optional"" />
    <xs:attribute name=""BTReviews"" type=""BTReviewsType"" use=""optional"" />
    <xs:attribute name=""BTSpecialBoxCost"" type=""BTSpecialBoxCostType"" use=""optional"" />
    <xs:attribute name=""BTSpecialCost"" type=""BTSpecialCostType"" use=""optional"" />
    <xs:attribute name=""BTSubTitle"" type=""BTSubTitleType"" use=""optional"" />
    <xs:attribute name=""BTTOLASVendorNumber"" type=""BTTOLASVendorNumberType"" use=""optional"" />
    <xs:attribute name=""BTUnitCost"" type=""BTUnitCostType"" use=""optional"" />
    <xs:attribute name=""BTUPC"" type=""BTUPCType"" use=""optional"" />
    <xs:attribute name=""BTVendorNumber"" type=""BTVendorNumberType"" use=""optional"" />
    <xs:attribute name=""ProductType"" type=""ProductTypeType"" use=""optional"" />
    <xs:attribute name=""Definition"" type=""DefinitionType"" use=""optional"" />
    <xs:attribute name=""PrimaryParentCategory"" type=""PrimaryParentCategoryType"" use=""optional"" />
    <xs:attribute name=""listprice"" type=""listpriceType"" use=""optional"" />
  </xs:complexType>
  <xs:complexType name=""VirtualProductVariantType"">
    <xs:choice minOccurs=""0"" maxOccurs=""unbounded"">
      <xs:element name=""DisplayName"" type=""DisplayNameType"" />
    </xs:choice>
    <xs:attribute name=""rank"" type=""rankType"" use=""optional"" />
    <xs:attribute name=""VariantId"" type=""VariantIdType"" use=""optional"" />
    <xs:attribute name=""id"" type=""idType"" use=""optional"" />
    <xs:attribute name=""lastmodified"" type=""lastmodifiedType"" use=""optional"" />
    <xs:attribute name=""listprice"" type=""listpriceType"" use=""optional"" />
  </xs:complexType>
  <xs:complexType name=""ProductCatalogType"">
    <xs:simpleContent>
      <xs:extension base=""CatalogNameType"" />
    </xs:simpleContent>
  </xs:complexType>
  <xs:complexType name=""CatalogSetType"">
    <xs:choice minOccurs=""0"" maxOccurs=""unbounded"">
      <xs:element name=""ProductCatalog"" type=""ProductCatalogType"" />
    </xs:choice>
    <xs:attribute name=""name"" type=""nameType"" use=""required"" />
    <xs:attribute name=""id"" type=""idType"" use=""optional"" />
    <xs:attribute name=""expression"" type=""expressionType"" use=""optional"" />
    <xs:attribute name=""description"" type=""CatalogSetDescriptionType"" use=""optional"" />
  </xs:complexType>
  <xs:element name=""MSCommerceCatalogCollection2"" type=""MSCommerceCatalogCollection2Type"" />
</xs:schema>";
        
        public BTCatalog() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "MSCommerceCatalogCollection2";
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
