namespace BTNextGen.BizTalk.CS.Inventory.Schemas {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"",@"MSInventoryCollection")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"MSInventoryCollection"})]
    public sealed class BTInventory : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" attributeFormDefault=""unqualified"" elementFormDefault=""unqualified"" version=""1.0"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:simpleType name=""versionType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""3"" />
      <xs:pattern value=""1.0"" />
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
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""MaxValueType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""4000"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""idType"">
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
  <xs:simpleType name=""CurrencyType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""128"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BuiltInType"">
    <xs:restriction base=""xs:boolean"" />
  </xs:simpleType>
  <xs:simpleType name=""DisplayAsBaseType"">
    <xs:restriction base=""xs:boolean"" />
  </xs:simpleType>
  <xs:simpleType name=""InventoryCatalogNameType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""85"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""InventoryCatalogDescriptionType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""256"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""DateCreatedType"">
    <xs:restriction base=""xs:dateTime"">
      <xs:minInclusive value=""1753-01-01T00:00:00"" />
      <xs:maxInclusive value=""9999-12-31T23:59:59"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""LastModifiedType"">
    <xs:restriction base=""xs:dateTime"">
      <xs:minInclusive value=""1753-01-01T00:00:00"" />
      <xs:maxInclusive value=""9999-12-31T23:59:59"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""ProductCatalogNameType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""85"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""ProductIdType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""256"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""VariantIdType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""256"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BackorderableType"">
    <xs:restriction base=""xs:boolean"" />
  </xs:simpleType>
  <xs:simpleType name=""PreorderableType"">
    <xs:restriction base=""xs:boolean"" />
  </xs:simpleType>
  <xs:simpleType name=""StatusType"">
    <xs:restriction base=""xs:integer"">
      <xs:minInclusive value=""-2147483648"" />
      <xs:maxInclusive value=""2147483647"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""OnHandQuantityType"">
    <xs:restriction base=""xs:double"">
      <xs:minInclusive value=""-3.402823E+38"" />
      <xs:maxInclusive value=""3.402823E+38"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""ExcessOnHandQuantityType"">
    <xs:restriction base=""xs:double"">
      <xs:minInclusive value=""-3.402823E+38"" />
      <xs:maxInclusive value=""3.402823E+38"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""ReorderPointType"">
    <xs:restriction base=""xs:double"">
      <xs:minInclusive value=""-3.402823E+38"" />
      <xs:maxInclusive value=""3.402823E+38"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""TargetQuantityType"">
    <xs:restriction base=""xs:double"">
      <xs:minInclusive value=""-3.402823E+38"" />
      <xs:maxInclusive value=""3.402823E+38"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BackorderLimitType"">
    <xs:restriction base=""xs:double"">
      <xs:minInclusive value=""-3.402823E+38"" />
      <xs:maxInclusive value=""3.402823E+38"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""PreorderLimitType"">
    <xs:restriction base=""xs:double"">
      <xs:minInclusive value=""-3.402823E+38"" />
      <xs:maxInclusive value=""3.402823E+38"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""StockOutThresholdType"">
    <xs:restriction base=""xs:double"">
      <xs:minInclusive value=""-3.402823E+38"" />
      <xs:maxInclusive value=""3.402823E+38"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""PreorderAvailabilityDateType"">
    <xs:restriction base=""xs:dateTime"">
      <xs:minInclusive value=""1753-01-01T00:00:00"" />
      <xs:maxInclusive value=""9999-12-31T23:59:59"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BackorderAvailabilityDateType"">
    <xs:restriction base=""xs:dateTime"">
      <xs:minInclusive value=""1753-01-01T00:00:00"" />
      <xs:maxInclusive value=""9999-12-31T23:59:59"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""LastRestockedType"">
    <xs:restriction base=""xs:dateTime"">
      <xs:minInclusive value=""1753-01-01T00:00:00"" />
      <xs:maxInclusive value=""9999-12-31T23:59:59"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""UnitOfMeasureType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""20"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""MemoType"">
    <xs:restriction base=""xs:string"">
      <xs:minLength value=""0"" />
      <xs:maxLength value=""20"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""BackorderedQuantityType"">
    <xs:restriction base=""xs:double"">
      <xs:minInclusive value=""-3.402823E+38"" />
      <xs:maxInclusive value=""3.402823E+38"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""PreorderedQuantityType"">
    <xs:restriction base=""xs:double"">
      <xs:minInclusive value=""-3.402823E+38"" />
      <xs:maxInclusive value=""3.402823E+38"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Warehouse1_InventoryTypeType"">
    <xs:restriction base=""xs:string"">
      <xs:enumeration value=""ReserveNumber123:400"" />
      <xs:enumeration value=""W:20"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Warehouse1_LEQuantityType"">
    <xs:restriction base=""xs:integer"">
      <xs:minInclusive value=""-2147483648"" />
      <xs:maxInclusive value=""2147483647"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Warehouse1_OnHandQuantityType"">
    <xs:restriction base=""xs:integer"">
      <xs:minInclusive value=""-2147483648"" />
      <xs:maxInclusive value=""2147483647"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Warehouse1_OnOrderQuantityType"">
    <xs:restriction base=""xs:integer"">
      <xs:minInclusive value=""-2147483648"" />
      <xs:maxInclusive value=""2147483647"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Warehouse2_InventoryTypeType"">
    <xs:restriction base=""xs:string"">
      <xs:enumeration value=""ReserveNumber123:400"" />
      <xs:enumeration value=""W:20"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Warehouse2_LEQuantityType"">
    <xs:restriction base=""xs:integer"">
      <xs:minInclusive value=""-2147483648"" />
      <xs:maxInclusive value=""2147483647"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Warehouse2_OnHandQuantityType"">
    <xs:restriction base=""xs:integer"">
      <xs:minInclusive value=""-2147483648"" />
      <xs:maxInclusive value=""2147483647"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Warehouse2_OnOrderQuantityType"">
    <xs:restriction base=""xs:integer"">
      <xs:minInclusive value=""-2147483648"" />
      <xs:maxInclusive value=""2147483647"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Warehouse3_InventoryTypeType"">
    <xs:restriction base=""xs:string"">
      <xs:enumeration value=""ReserveNumber123:400"" />
      <xs:enumeration value=""W:20"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Warehouse3_LEQuantityType"">
    <xs:restriction base=""xs:integer"">
      <xs:minInclusive value=""-2147483648"" />
      <xs:maxInclusive value=""2147483647"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Warehouse3_OnhandQuantityType"">
    <xs:restriction base=""xs:integer"">
      <xs:minInclusive value=""-2147483648"" />
      <xs:maxInclusive value=""2147483647"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Warehouse3_OnOrderQuantityType"">
    <xs:restriction base=""xs:integer"">
      <xs:minInclusive value=""-2147483648"" />
      <xs:maxInclusive value=""2147483647"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Warehouse4_InventoryTypeType"">
    <xs:restriction base=""xs:string"">
      <xs:enumeration value=""ReserveNumber123:400"" />
      <xs:enumeration value=""W:20"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Warehouse4_LEQuantityType"">
    <xs:restriction base=""xs:integer"">
      <xs:minInclusive value=""-2147483648"" />
      <xs:maxInclusive value=""2147483647"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Warehouse4_OnhandQuantityType"">
    <xs:restriction base=""xs:integer"">
      <xs:minInclusive value=""-2147483648"" />
      <xs:maxInclusive value=""2147483647"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Warehouse4_OnOrderQuantityType"">
    <xs:restriction base=""xs:integer"">
      <xs:minInclusive value=""-2147483648"" />
      <xs:maxInclusive value=""2147483647"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Warehouse5_InventoryTypeType"">
    <xs:restriction base=""xs:string"">
      <xs:enumeration value=""ReserveNumber123:400"" />
      <xs:enumeration value=""W:50"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Warehouse5_LEQuantityType"">
    <xs:restriction base=""xs:integer"">
      <xs:minInclusive value=""-2147483648"" />
      <xs:maxInclusive value=""2147483647"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Warehouse5_OnHandQuantityType"">
    <xs:restriction base=""xs:integer"">
      <xs:minInclusive value=""-2147483648"" />
      <xs:maxInclusive value=""2147483647"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Warehouse5_OnOrderQuantityType"">
    <xs:restriction base=""xs:integer"">
      <xs:minInclusive value=""-2147483648"" />
      <xs:maxInclusive value=""2147483647"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Warehouse6_InventoryTypeType"">
    <xs:restriction base=""xs:string"">
      <xs:enumeration value=""ReserveNumber123:400"" />
      <xs:enumeration value=""W:50"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Warehouse6_LEQuantityType"">
    <xs:restriction base=""xs:integer"">
      <xs:minInclusive value=""-2147483648"" />
      <xs:maxInclusive value=""2147483647"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Warehouse6_OnHandQuantityType"">
    <xs:restriction base=""xs:integer"">
      <xs:minInclusive value=""-2147483648"" />
      <xs:maxInclusive value=""2147483647"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""Warehouse6_OnOrderQuantityType"">
    <xs:restriction base=""xs:integer"">
      <xs:minInclusive value=""-2147483648"" />
      <xs:maxInclusive value=""2147483647"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:complexType name=""PropertyDisplayNameType"">
    <xs:attribute name=""Value"" type=""ValueType"" use=""required"" />
    <xs:attribute name=""language"" type=""languageType"" use=""optional"" />
  </xs:complexType>
  <xs:complexType name=""DefaultValueType"">
    <xs:attribute name=""Value"" type=""ValueType"" use=""required"" />
    <xs:attribute name=""language"" type=""languageType"" use=""optional"" />
  </xs:complexType>
  <xs:complexType name=""PropertyValueType"">
    <xs:attribute name=""DisplayName"" type=""ValueType"" use=""required"" />
    <xs:attribute name=""language"" type=""languageType"" use=""optional"" />
    <xs:attribute name=""rank"" type=""rankType"" use=""optional"" />
  </xs:complexType>
  <xs:complexType name=""ProductCatalogType"">
    <xs:attribute name=""name"" type=""ProductCatalogNameType"" use=""required"" />
  </xs:complexType>
  <xs:complexType name=""PropertyType"">
    <xs:choice minOccurs=""0"" maxOccurs=""unbounded"">
      <xs:element name=""DisplayName"" type=""PropertyDisplayNameType"" />
      <xs:element name=""DefaultValue"" type=""DefaultValueType"" />
      <xs:element name=""PropertyValue"" type=""PropertyValueType"" />
    </xs:choice>
    <xs:attribute name=""MinValue"" type=""MinValueType"" use=""optional"" />
    <xs:attribute name=""MaxValue"" type=""MaxValueType"" use=""optional"" />
    <xs:attribute name=""id"" type=""idType"" use=""optional"" />
    <xs:attribute name=""dataType"" type=""dataTypeType"" use=""optional"" />
    <xs:attribute name=""name"" type=""nameType"" use=""required"" />
    <xs:attribute name=""IsFreeTextSearchable"" type=""IsFreeTextSearchableType"" use=""optional"" />
    <xs:attribute name=""IncludeInSpecSearch"" type=""IncludeInSpecSearchType"" use=""optional"" />
    <xs:attribute name=""DisplayOnSite"" type=""DisplayOnSiteType"" use=""optional"" />
    <xs:attribute name=""AssignAll"" type=""AssignAllType"" use=""optional"" />
    <xs:attribute name=""ExportToDW"" type=""ExportToDWType"" use=""optional"" />
    <xs:attribute name=""DisplayInProductsList"" type=""DisplayInProductsListType"" use=""optional"" />
    <xs:attribute name=""Multilingual"" type=""MultilingualType"" use=""optional"" />
    <xs:attribute name=""IsRequired"" type=""IsRequiredType"" use=""optional"" />
    <xs:attribute name=""Currency"" type=""CurrencyType"" use=""optional"" />
    <xs:attribute name=""BuiltIn"" type=""BuiltInType"" use=""optional"" />
    <xs:attribute name=""DisplayAsBase"" type=""DisplayAsBaseType"" use=""optional"" />
  </xs:complexType>
  <xs:complexType name=""MSInventoryCollectionType"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""1"" name=""InventorySchema"" type=""InventorySchemaType"" />
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""InventoryCatalog"" type=""InventoryCatalogType"" />
    </xs:sequence>
    <xs:attribute name=""version"" type=""versionType"" use=""required"" />
  </xs:complexType>
  <xs:complexType name=""InventorySchemaType"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""Property"" type=""PropertyType"" />
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""EntityExtensions"" type=""EntityExtensionsType"" />
    </xs:sequence>
  </xs:complexType>
  <xs:complexType name=""EntityExtensionsType"">
    <xs:choice minOccurs=""0"" maxOccurs=""unbounded"">
      <xs:element name=""PropertyDefinition"" type=""xs:string"" />
      <xs:element name=""InventoryCatalog"" type=""xs:string"" />
      <xs:element name=""InventorySku"" type=""xs:string"" />
    </xs:choice>
  </xs:complexType>
  <xs:complexType name=""InventoryCatalogType"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""ProductCatalog"" type=""ProductCatalogType"" />
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""InventorySku"" type=""InventorySkuType"" />
    </xs:sequence>
    <xs:attribute name=""name"" type=""InventoryCatalogNameType"" use=""required"" />
    <xs:attribute name=""description"" type=""InventoryCatalogDescriptionType"" use=""optional"" />
    <xs:attribute name=""InventoryCatalogDescription"" type=""InventoryCatalogDescriptionType"" use=""optional"" />
    <xs:attribute name=""DateCreated"" type=""DateCreatedType"" use=""optional"" />
    <xs:attribute name=""LastModified"" type=""LastModifiedType"" use=""optional"" />
  </xs:complexType>
  <xs:complexType name=""InventorySkuType"">
    <xs:attribute name=""InventoryCatalogName"" type=""InventoryCatalogNameType"" use=""optional"" />
    <xs:attribute name=""LastModified"" type=""LastModifiedType"" use=""optional"" />
    <xs:attribute name=""ProductCatalogName"" type=""ProductCatalogNameType"" use=""optional"" />
    <xs:attribute name=""ProductId"" type=""ProductIdType"" use=""optional"" />
    <xs:attribute name=""VariantId"" type=""VariantIdType"" use=""optional"" />
    <xs:attribute name=""Backorderable"" type=""BackorderableType"" use=""optional"" />
    <xs:attribute name=""Preorderable"" type=""PreorderableType"" use=""optional"" />
    <xs:attribute name=""Status"" type=""StatusType"" use=""optional"" />
    <xs:attribute name=""OnHandQuantity"" type=""OnHandQuantityType"" use=""optional"" />
    <xs:attribute name=""ExcessOnHandQuantity"" type=""ExcessOnHandQuantityType"" use=""optional"" />
    <xs:attribute name=""ReorderPoint"" type=""ReorderPointType"" use=""optional"" />
    <xs:attribute name=""TargetQuantity"" type=""TargetQuantityType"" use=""optional"" />
    <xs:attribute name=""BackorderLimit"" type=""BackorderLimitType"" use=""optional"" />
    <xs:attribute name=""PreorderLimit"" type=""PreorderLimitType"" use=""optional"" />
    <xs:attribute name=""StockOutThreshold"" type=""StockOutThresholdType"" use=""optional"" />
    <xs:attribute name=""PreorderAvailabilityDate"" type=""PreorderAvailabilityDateType"" use=""optional"" />
    <xs:attribute name=""BackorderAvailabilityDate"" type=""BackorderAvailabilityDateType"" use=""optional"" />
    <xs:attribute name=""LastRestocked"" type=""LastRestockedType"" use=""optional"" />
    <xs:attribute name=""UnitOfMeasure"" type=""UnitOfMeasureType"" use=""optional"" />
    <xs:attribute name=""Memo"" type=""MemoType"" use=""optional"" />
    <xs:attribute name=""BackorderedQuantity"" type=""BackorderedQuantityType"" use=""optional"" />
    <xs:attribute name=""PreorderedQuantity"" type=""PreorderedQuantityType"" use=""optional"" />
    <xs:attribute name=""Warehouse1_InventoryType"" type=""Warehouse1_InventoryTypeType"" use=""optional"" />
    <xs:attribute name=""Warehouse1_LEQuantity"" type=""Warehouse1_LEQuantityType"" use=""optional"" />
    <xs:attribute name=""Warehouse1_OnHandQuantity"" type=""Warehouse1_OnHandQuantityType"" use=""optional"" />
    <xs:attribute name=""Warehouse1_OnOrderQuantity"" type=""Warehouse1_OnOrderQuantityType"" use=""optional"" />
    <xs:attribute name=""Warehouse2_InventoryType"" type=""Warehouse2_InventoryTypeType"" use=""optional"" />
    <xs:attribute name=""Warehouse2_LEQuantity"" type=""Warehouse2_LEQuantityType"" use=""optional"" />
    <xs:attribute name=""Warehouse2_OnHandQuantity"" type=""Warehouse2_OnHandQuantityType"" use=""optional"" />
    <xs:attribute name=""Warehouse2_OnOrderQuantity"" type=""Warehouse2_OnOrderQuantityType"" use=""optional"" />
    <xs:attribute name=""Warehouse3_InventoryType"" type=""Warehouse3_InventoryTypeType"" use=""optional"" />
    <xs:attribute name=""Warehouse3_LEQuantity"" type=""Warehouse3_LEQuantityType"" use=""optional"" />
    <xs:attribute name=""Warehouse3_OnhandQuantity"" type=""Warehouse3_OnhandQuantityType"" use=""optional"" />
    <xs:attribute name=""Warehouse3_OnOrderQuantity"" type=""Warehouse3_OnOrderQuantityType"" use=""optional"" />
    <xs:attribute name=""Warehouse4_InventoryType"" type=""Warehouse4_InventoryTypeType"" use=""optional"" />
    <xs:attribute name=""Warehouse4_LEQuantity"" type=""Warehouse4_LEQuantityType"" use=""optional"" />
    <xs:attribute name=""Warehouse4_OnhandQuantity"" type=""Warehouse4_OnhandQuantityType"" use=""optional"" />
    <xs:attribute name=""Warehouse4_OnOrderQuantity"" type=""Warehouse4_OnOrderQuantityType"" use=""optional"" />
    <xs:attribute name=""Warehouse5_InventoryType"" type=""Warehouse5_InventoryTypeType"" use=""optional"" />
    <xs:attribute name=""Warehouse5_LEQuantity"" type=""Warehouse5_LEQuantityType"" use=""optional"" />
    <xs:attribute name=""Warehouse5_OnHandQuantity"" type=""Warehouse5_OnHandQuantityType"" use=""optional"" />
    <xs:attribute name=""Warehouse5_OnOrderQuantity"" type=""Warehouse5_OnOrderQuantityType"" use=""optional"" />
    <xs:attribute name=""Warehouse6_InventoryType"" type=""Warehouse6_InventoryTypeType"" use=""optional"" />
    <xs:attribute name=""Warehouse6_LEQuantity"" type=""Warehouse6_LEQuantityType"" use=""optional"" />
    <xs:attribute name=""Warehouse6_OnHandQuantity"" type=""Warehouse6_OnHandQuantityType"" use=""optional"" />
    <xs:attribute name=""Warehouse6_OnOrderQuantity"" type=""Warehouse6_OnOrderQuantityType"" use=""optional"" />
  </xs:complexType>
  <xs:element name=""MSInventoryCollection"" type=""MSInventoryCollectionType"" />
</xs:schema>";
        
        public BTInventory() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "MSInventoryCollection";
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
