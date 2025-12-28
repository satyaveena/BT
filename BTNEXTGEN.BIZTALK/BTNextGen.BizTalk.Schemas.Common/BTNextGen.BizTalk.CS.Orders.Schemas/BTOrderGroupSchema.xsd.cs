namespace BTNextGen.BizTalk.CS.Orders.Schemas {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"",@"OrderGroups")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"OrderGroups"})]
    public sealed class BTOrderGroupSchema : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" elementFormDefault=""qualified"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""OrderGroups"" type=""OrderGroups"" />
  <xs:complexType name=""OrderGroups"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""PurchaseOrderEx"" type=""PurchaseOrderEx"" />
    </xs:sequence>
  </xs:complexType>
  <xs:complexType name=""PurchaseOrderEx"">
    <xs:sequence>
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""OrderForms"" type=""ArrayOfOrderFormEx"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""Addresses"" type=""ArrayOfOrderAddressEx"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""WeaklyTypedProperties"" type=""ArrayOfWeaklyTypedProperties"" />
    </xs:sequence>
    <xs:attribute name=""Status"" type=""xs:string"" />
    <xs:attribute name=""SoldToId"" type=""Guid"" />
    <xs:attribute name=""TaxTotal"" type=""xs:decimal"" />
    <xs:attribute name=""LastModified"" type=""xs:dateTime"" />
    <xs:attribute name=""Created"" type=""xs:dateTime"" />
    <xs:attribute name=""StatusCode"" type=""StatusCodes"" />
    <xs:attribute name=""SoldToAddressId"" type=""xs:string"" />
    <xs:attribute name=""TrackingNumber"" type=""xs:string"" />
    <xs:attribute name=""SubTotal"" type=""xs:decimal"" />
    <xs:attribute name=""IsDirty"" type=""xs:boolean"" />
    <xs:attribute name=""LineItemCount"" type=""xs:int"" />
    <xs:attribute name=""HandlingTotal"" type=""xs:decimal"" />
    <xs:attribute name=""BasketId"" type=""Guid"" />
    <xs:attribute name=""ShippingTotal"" type=""xs:decimal"" />
    <xs:attribute name=""ModifiedBy"" type=""xs:string"" />
    <xs:attribute name=""SoldToName"" type=""xs:string"" />
    <xs:attribute name=""Total"" type=""xs:decimal"" />
    <xs:attribute name=""OrderGroupId"" type=""Guid"" />
    <xs:attribute name=""Name"" type=""xs:string"" />
    <xs:attribute name=""BillingCurrency"" type=""xs:string"" />
  </xs:complexType>
  <xs:complexType name=""ArrayOfOrderFormEx"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""OrderFormEx"" type=""OrderFormEx"" />
    </xs:sequence>
  </xs:complexType>
  <xs:complexType name=""OrderFormEx"">
    <xs:sequence>
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""PromoCodeRecords"" type=""ArrayOfPromoCodeRecord"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""Shipments"" type=""ArrayOfShipment"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""PromoCodes"" type=""ArrayOfString"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""Payments"" type=""ArrayOfPayment"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""LineItems"" type=""ArrayOfLineItemEx"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""WeaklyTypedProperties"" type=""ArrayOfWeaklyTypedProperties"" />
    </xs:sequence>
    <xs:attribute name=""CostSummaryByIntAdmin"" type=""xs:string"" />
    <xs:attribute name=""BTNote"" type=""xs:string"" />
    <xs:attribute name=""CostSummaryByExtAdmin"" type=""xs:string"" />
    <xs:attribute name=""IsTolas"" type=""xs:boolean"" />
    <xs:attribute name=""IsHomeDeliveryIndicator"" type=""xs:boolean"" />
    <xs:attribute name=""LegacyBasketId"" type=""xs:string"" />
    <xs:attribute name=""Name"" type=""xs:string"" />
    <xs:attribute name=""HomeDeliveryAccountId"" type=""xs:string"" />
    <xs:attribute name=""LastModified"" type=""xs:dateTime"" />
    <xs:attribute name=""ShippingTotal"" type=""xs:decimal"" />
    <xs:attribute name=""CheckReserveFlag"" type=""xs:boolean"" />
    <xs:attribute name=""LegacySource"" type=""xs:string"" />
    <xs:attribute name=""BTGiftWrapCode"" type=""xs:string"" />
    <xs:attribute name=""SubTotal"" type=""xs:decimal"" />
    <xs:attribute name=""OrderGroupId"" type=""Guid"" />
    <xs:attribute name=""HandlingTotal"" type=""xs:decimal"" />
    <xs:attribute name=""FolderId"" type=""xs:string"" />
    <xs:attribute name=""EntertainmentAccountId"" type=""xs:string"" />
    <xs:attribute name=""BookAccountId"" type=""xs:string"" />
    <xs:attribute name=""InventoryReserveNumber"" type=""xs:string"" />
    <xs:attribute name=""BillingAddressId"" type=""xs:string"" />
    <xs:attribute name=""OrderFormId"" type=""Guid"" />
    <xs:attribute name=""PriorStatus"" type=""xs:string"" />
    <xs:attribute name=""IsBTCart"" type=""xs:int"" />
    <xs:attribute name=""TransmissionNumber"" type=""xs:int"" />
    <xs:attribute name=""BTStatus"" type=""xs:string"" />
    <xs:attribute name=""AccountType"" type=""xs:string"" />
    <xs:attribute name=""AccountInventoryType"" type=""xs:string"" />
    <xs:attribute name=""StoreProccessingFee"" type=""xs:boolean"" />
    <xs:attribute name=""OriginalBasketId"" type=""xs:string"" />
    <xs:attribute name=""BTGiftWrapMessage"" type=""xs:string"" />
    <xs:attribute name=""StoreOrderFee"" type=""xs:boolean"" />
    <xs:attribute name=""Created"" type=""xs:dateTime"" />
    <xs:attribute name=""IsBackOrder"" type=""xs:boolean"" />
    <xs:attribute name=""PromoUserIdentity"" type=""xs:string"" />
    <xs:attribute name=""StoreGiftWrapFee"" type=""xs:boolean"" />
    <xs:attribute name=""HomeDeliveryAddressType"" type=""xs:string"" />
    <xs:attribute name=""PONumber"" type=""xs:string"" />
    <xs:attribute name=""StoreShippingFee"" type=""xs:boolean"" />
    <xs:attribute name=""ModifiedBy"" type=""xs:string"" />
    <xs:attribute name=""BTTargetERP"" type=""xs:string"" />
    <xs:attribute name=""Status"" type=""xs:string"" />
    <xs:attribute name=""TaxTotal"" type=""xs:decimal"" />
    <xs:attribute name=""WarehouseList"" type=""xs:string"" />
    <xs:attribute name=""Total"" type=""xs:decimal"" />
    <xs:attribute name=""IsArchived"" type=""xs:int"" />
    <xs:attribute name=""IsPrimary"" type=""xs:int"" />
    <xs:attribute name=""SpecialInstructions"" type=""xs:string"" />
    <xs:attribute name=""ShippingMethodExId"" type=""xs:string"" />
  </xs:complexType>
  <xs:complexType name=""ArrayOfOrderAddressEx"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""OrderAddressEx"" type=""OrderAddressEx"" />
    </xs:sequence>
  </xs:complexType>
  <xs:complexType name=""OrderAddressEx"">
    <xs:sequence>
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""WeaklyTypedProperties"" type=""ArrayOfWeaklyTypedProperties"" />
    </xs:sequence>
    <xs:attribute name=""RegionCode"" type=""xs:string"" />
    <xs:attribute name=""IsPOBox"" type=""xs:boolean"" />
    <xs:attribute name=""Email"" type=""xs:string"" />
    <xs:attribute name=""CountryCode"" type=""xs:string"" />
    <xs:attribute name=""EveningPhoneNumber"" type=""xs:string"" />
    <xs:attribute name=""PostalCode"" type=""xs:string"" />
    <xs:attribute name=""Name"" type=""xs:string"" />
    <xs:attribute name=""RegionName"" type=""xs:string"" />
    <xs:attribute name=""State"" type=""xs:string"" />
    <xs:attribute name=""LastName"" type=""xs:string"" />
    <xs:attribute name=""OrderGroupId"" type=""Guid"" />
    <xs:attribute name=""DaytimePhoneNumber"" type=""xs:string"" />
    <xs:attribute name=""CountryName"" type=""xs:string"" />
    <xs:attribute name=""OrderAddressId"" type=""xs:string"" />
    <xs:attribute name=""City"" type=""xs:string"" />
    <xs:attribute name=""FirstName"" type=""xs:string"" />
    <xs:attribute name=""Line4"" type=""xs:string"" />
    <xs:attribute name=""Organization"" type=""xs:string"" />
    <xs:attribute name=""Line1"" type=""xs:string"" />
    <xs:attribute name=""Line2"" type=""xs:string"" />
    <xs:attribute name=""Line3"" type=""xs:string"" />
    <xs:attribute name=""FaxNumber"" type=""xs:string"" />
  </xs:complexType>
  <xs:complexType name=""ArrayOfPromoCodeRecord"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""PromoCodeRecord"" type=""PromoCodeRecord"" />
    </xs:sequence>
  </xs:complexType>
  <xs:complexType name=""PromoCodeRecord"">
    <xs:sequence>
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""WeaklyTypedProperties"" type=""ArrayOfWeaklyTypedProperties"" />
    </xs:sequence>
    <xs:attribute name=""OrderGroupId"" type=""Guid"" />
    <xs:attribute name=""PromoCode"" type=""xs:string"" />
    <xs:attribute name=""PromoCodeLookupDate"" type=""xs:dateTime"" />
    <xs:attribute name=""PromoCodeStatus"" type=""PromoCodeState"" />
    <xs:attribute name=""PromoCodeDefinitionId"" type=""xs:int"" />
    <xs:attribute name=""PromoCodeReserved"" type=""xs:boolean"" />
    <xs:attribute name=""PromoApplied"" type=""xs:boolean"" />
    <xs:attribute name=""OrderFormId"" type=""Guid"" />
  </xs:complexType>
  <xs:complexType name=""ArrayOfShipment"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""Shipment"" type=""Shipment"" />
    </xs:sequence>
  </xs:complexType>
  <xs:complexType name=""Shipment"">
    <xs:sequence>
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""LineItemIndexes"" type=""ArrayOfAnyType"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""ShippingDiscounts"" type=""ArrayOfShippingDiscountRecord"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""WeaklyTypedProperties"" type=""ArrayOfWeaklyTypedProperties"" />
    </xs:sequence>
    <xs:attribute name=""Status"" type=""xs:string"" />
    <xs:attribute name=""ShippingMethodName"" type=""xs:string"" />
    <xs:attribute name=""ShipmentTrackingNumber"" type=""xs:string"" />
    <xs:attribute name=""ShipmentTotal"" type=""xs:decimal"" />
    <xs:attribute name=""ShippingAddressId"" type=""xs:string"" />
    <xs:attribute name=""ShipmentId"" type=""Guid"" />
    <xs:attribute name=""ShippingDiscountAmount"" type=""xs:decimal"" />
    <xs:attribute name=""OrderGroupId"" type=""Guid"" />
    <xs:attribute name=""OrderFormId"" type=""Guid"" />
    <xs:attribute name=""ShippingMethodId"" type=""Guid"" />
  </xs:complexType>
  <xs:complexType name=""ArrayOfPayment"">
    <xs:choice minOccurs=""0"" maxOccurs=""unbounded"">
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""ACHPayment"" type=""ACHPayment"" />
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""PurchaseOrderPayment"" type=""PurchaseOrderPayment"" />
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""CreditCardPaymentEx"" type=""CreditCardPaymentEx"" />
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""GiftCertificatePayment"" type=""GiftCertificatePayment"" />
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""CashCardPayment"" type=""CashCardPayment"" />
    </xs:choice>
  </xs:complexType>
  <xs:complexType name=""ACHPayment"">
    <xs:sequence>
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""WeaklyTypedProperties"" type=""ArrayOfWeaklyTypedProperties"" />
    </xs:sequence>
    <xs:attribute name=""Status"" type=""xs:string"" />
    <xs:attribute name=""NameOnBankAccount"" type=""xs:string"" />
    <xs:attribute name=""OrderGroupId"" type=""Guid"" />
    <xs:attribute name=""PaymentType"" type=""PaymentMethodTypes"" />
    <xs:attribute name=""CustomerNameOnPayment"" type=""xs:string"" />
    <xs:attribute name=""PaymentMethodName"" type=""xs:string"" />
    <xs:attribute name=""ABARoutingNumber"" type=""xs:string"" />
    <xs:attribute name=""PaymentMethodId"" type=""Guid"" />
    <xs:attribute name=""DerivedClassName"" type=""xs:string"" />
    <xs:attribute name=""PaymentId"" type=""Guid"" />
    <xs:attribute name=""BillingAddressId"" type=""xs:string"" />
    <xs:attribute name=""Amount"" type=""xs:decimal"" />
    <xs:attribute name=""OrderFormId"" type=""Guid"" />
    <xs:attribute name=""CheckNumber"" type=""xs:string"" />
    <xs:attribute name=""BankAccountType"" type=""xs:string"" />
  </xs:complexType>
  <xs:complexType name=""PurchaseOrderPayment"">
    <xs:sequence>
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""WeaklyTypedProperties"" type=""ArrayOfWeaklyTypedProperties"" />
    </xs:sequence>
    <xs:attribute name=""Status"" type=""xs:string"" />
    <xs:attribute name=""OrderGroupId"" type=""Guid"" />
    <xs:attribute name=""PaymentType"" type=""PaymentMethodTypes"" />
    <xs:attribute name=""CustomerNameOnPayment"" type=""xs:string"" />
    <xs:attribute name=""PurchaseOrderPaymentNumber"" type=""xs:string"" />
    <xs:attribute name=""PaymentMethodName"" type=""xs:string"" />
    <xs:attribute name=""DerivedClassName"" type=""xs:string"" />
    <xs:attribute name=""PaymentId"" type=""Guid"" />
    <xs:attribute name=""BillingAddressId"" type=""xs:string"" />
    <xs:attribute name=""Amount"" type=""xs:decimal"" />
    <xs:attribute name=""OrderFormId"" type=""Guid"" />
    <xs:attribute name=""PaymentMethodId"" type=""Guid"" />
  </xs:complexType>
  <xs:complexType name=""CreditCardPaymentEx"">
    <xs:sequence>
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""WeaklyTypedProperties"" type=""ArrayOfWeaklyTypedProperties"" />
    </xs:sequence>
    <xs:attribute name=""ExpirationYear"" type=""xs:int"" />
    <xs:attribute name=""BTCreditCardToken"" type=""xs:string"" />
    <xs:attribute name=""Status"" type=""xs:string"" />
    <xs:attribute name=""ValidationCode"" type=""xs:string"" />
    <xs:attribute name=""PaymentType"" type=""PaymentMethodTypes"" />
    <xs:attribute name=""BillingAddressId"" type=""xs:string"" />
    <xs:attribute name=""DerivedClassName"" type=""xs:string"" />
    <xs:attribute name=""AuthorizationCode"" type=""xs:string"" />
    <xs:attribute name=""CustomerNameOnPayment"" type=""xs:string"" />
    <xs:attribute name=""PaymentMethodName"" type=""xs:string"" />
    <xs:attribute name=""CardType"" type=""xs:string"" />
    <xs:attribute name=""OrderGroupId"" type=""Guid"" />
    <xs:attribute name=""PaymentId"" type=""Guid"" />
    <xs:attribute name=""CreditCardIdentifier"" type=""xs:string"" />
    <xs:attribute name=""ExpirationMonth"" type=""xs:int"" />
    <xs:attribute name=""Amount"" type=""xs:decimal"" />
    <xs:attribute name=""CreditCardNumber"" type=""xs:string"" />
    <xs:attribute name=""OrderFormId"" type=""Guid"" />
    <xs:attribute name=""PaymentMethodId"" type=""Guid"" />
  </xs:complexType>
  <xs:complexType name=""GiftCertificatePayment"">
    <xs:sequence>
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""WeaklyTypedProperties"" type=""ArrayOfWeaklyTypedProperties"" />
    </xs:sequence>
    <xs:attribute name=""Status"" type=""xs:string"" />
    <xs:attribute name=""OrderGroupId"" type=""Guid"" />
    <xs:attribute name=""PaymentType"" type=""PaymentMethodTypes"" />
    <xs:attribute name=""AuthorizationCode"" type=""xs:string"" />
    <xs:attribute name=""ExpirationDate"" type=""xs:dateTime"" />
    <xs:attribute name=""CustomerNameOnPayment"" type=""xs:string"" />
    <xs:attribute name=""PaymentMethodName"" type=""xs:string"" />
    <xs:attribute name=""PaymentMethodId"" type=""Guid"" />
    <xs:attribute name=""DerivedClassName"" type=""xs:string"" />
    <xs:attribute name=""PaymentId"" type=""Guid"" />
    <xs:attribute name=""BillingAddressId"" type=""xs:string"" />
    <xs:attribute name=""Amount"" type=""xs:decimal"" />
    <xs:attribute name=""GiftCertificateNumber"" type=""xs:string"" />
    <xs:attribute name=""Pin"" type=""xs:string"" />
    <xs:attribute name=""OrderFormId"" type=""Guid"" />
  </xs:complexType>
  <xs:complexType name=""CashCardPayment"">
    <xs:sequence>
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""WeaklyTypedProperties"" type=""ArrayOfWeaklyTypedProperties"" />
    </xs:sequence>
    <xs:attribute name=""Status"" type=""xs:string"" />
    <xs:attribute name=""OrderGroupId"" type=""Guid"" />
    <xs:attribute name=""PaymentType"" type=""PaymentMethodTypes"" />
    <xs:attribute name=""CustomerNameOnPayment"" type=""xs:string"" />
    <xs:attribute name=""PaymentMethodName"" type=""xs:string"" />
    <xs:attribute name=""PaymentMethodId"" type=""Guid"" />
    <xs:attribute name=""DerivedClassName"" type=""xs:string"" />
    <xs:attribute name=""CashCardNumber"" type=""xs:string"" />
    <xs:attribute name=""PaymentId"" type=""Guid"" />
    <xs:attribute name=""BillingAddressId"" type=""xs:string"" />
    <xs:attribute name=""Amount"" type=""xs:decimal"" />
    <xs:attribute name=""Pin"" type=""xs:string"" />
    <xs:attribute name=""OrderFormId"" type=""Guid"" />
  </xs:complexType>
  <xs:complexType name=""ArrayOfLineItemEx"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""LineItemEx"" type=""LineItemEx"" />
    </xs:sequence>
  </xs:complexType>
  <xs:complexType name=""LineItemEx"">
    <xs:sequence>
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""ItemLevelDiscountsApplied"" type=""ArrayOfDiscountApplicationRecord"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""OrderLevelDiscountsApplied"" type=""ArrayOfDiscountApplicationRecord"" />
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""WeaklyTypedProperties"" type=""ArrayOfWeaklyTypedProperties"" />
    </xs:sequence>
    <xs:attribute name=""LineItemDiscountAmount"" type=""xs:decimal"" />
    <xs:attribute name=""ProductCatalog"" type=""xs:string"" />
    <xs:attribute name=""LineItemNumberId"" type=""xs:string"" />
    <xs:attribute name=""BTCancelQuantity"" type=""xs:int"" />
    <xs:attribute name=""LegacyBasketId"" type=""xs:string"" />
    <xs:attribute name=""LineItemId"" type=""Guid"" />
    <xs:attribute name=""BTGTIN"" type=""xs:string"" />
    <xs:attribute name=""LastModified"" type=""xs:dateTime"" />
    <xs:attribute name=""BackorderQuantity"" type=""xs:decimal"" />
    <xs:attribute name=""Status"" type=""xs:string"" />
    <xs:attribute name=""PreorderQuantity"" type=""xs:decimal"" />
    <xs:attribute name=""OrderGroupId"" type=""Guid"" />
    <xs:attribute name=""Description"" type=""xs:string"" />
    <xs:attribute name=""BTPublisher"" type=""xs:string"" />
    <xs:attribute name=""InventoryCondition"" type=""InventoryCondition"" />
    <xs:attribute name=""AllowBackordersAndPreorders"" type=""xs:boolean"" />
    <xs:attribute name=""BTUPC"" type=""xs:string"" />
    <xs:attribute name=""OrderFormId"" type=""Guid"" />
    <xs:attribute name=""LegacyCartLineId"" type=""xs:string"" />
    <xs:attribute name=""BTVolumeSet"" type=""xs:string"" />
    <xs:attribute name=""ShippingMethodId"" type=""Guid"" />
    <xs:attribute name=""BTBackorderQuantity"" type=""xs:int"" />
    <xs:attribute name=""BTTitleEditionVersion"" type=""xs:string"" />
    <xs:attribute name=""BTISBN"" type=""xs:string"" />
    <xs:attribute name=""ExtendedPrice"" type=""xs:decimal"" />
    <xs:attribute name=""OrderLevelDiscountAmount"" type=""xs:decimal"" />
    <xs:attribute name=""ProductCategory"" type=""xs:string"" />
    <xs:attribute name=""ModifiedBy"" type=""xs:string"" />
    <xs:attribute name=""BTAuthorOrArtist"" type=""xs:string"" />
    <xs:attribute name=""ProductId"" type=""xs:string"" />
    <xs:attribute name=""ListPrice"" type=""xs:decimal"" />
    <xs:attribute name=""Created"" type=""xs:dateTime"" />
    <xs:attribute name=""BTItemType"" type=""xs:string"" />
    <xs:attribute name=""ShippingAddressId"" type=""xs:string"" />
    <xs:attribute name=""Quantity"" type=""xs:decimal"" />
    <xs:attribute name=""ShippingMethodName"" type=""xs:string"" />
    <xs:attribute name=""BTKey"" type=""xs:string"" />
    <xs:attribute name=""InStockQuantity"" type=""xs:decimal"" />
    <xs:attribute name=""PlacedPrice"" type=""xs:decimal"" />
    <xs:attribute name=""POLineItemNumber"" type=""xs:string"" />
    <xs:attribute name=""BTLineItemNote"" type=""xs:string"" />
    <xs:attribute name=""BTPriceKey"" type=""xs:string"" />
    <xs:attribute name=""ProductVariantId"" type=""xs:string"" />
    <xs:attribute name=""DisplayName"" type=""xs:string"" />
    <xs:attribute name=""BTBibNumber"" type=""xs:string"" />
    <xs:attribute name=""BTCatalogCode"" type=""xs:string"" />
  </xs:complexType>
  <xs:complexType name=""ArrayOfShippingDiscountRecord"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""ShippingDiscountRecord"" type=""ShippingDiscountRecord"" />
    </xs:sequence>
  </xs:complexType>
  <xs:complexType name=""ShippingDiscountRecord"">
    <xs:sequence>
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""WeaklyTypedProperties"" type=""ArrayOfWeaklyTypedProperties"" />
    </xs:sequence>
    <xs:attribute name=""DiscountId"" type=""xs:int"" />
    <xs:attribute name=""LastModified"" type=""xs:dateTime"" />
    <xs:attribute name=""Priority"" type=""xs:int"" />
    <xs:attribute name=""DiscountAmount"" type=""xs:decimal"" />
    <xs:attribute name=""DiscountValue"" type=""xs:decimal"" />
    <xs:attribute name=""TypeOfDiscount"" type=""DiscountType"" />
    <xs:attribute name=""BasketDisplayMessage"" type=""xs:string"" />
    <xs:attribute name=""ShipmentId"" type=""Guid"" />
    <xs:attribute name=""OrderGroupId"" type=""Guid"" />
    <xs:attribute name=""PromoCode"" type=""xs:string"" />
    <xs:attribute name=""DiscountName"" type=""xs:string"" />
    <xs:attribute name=""PromoCodeDefinitionId"" type=""xs:int"" />
  </xs:complexType>
  <xs:complexType name=""ArrayOfDiscountApplicationRecord"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""DiscountApplicationRecord"" type=""DiscountApplicationRecord"" />
    </xs:sequence>
  </xs:complexType>
  <xs:complexType name=""DiscountApplicationRecord"">
    <xs:sequence>
      <xs:element minOccurs=""1"" maxOccurs=""1"" name=""WeaklyTypedProperties"" type=""ArrayOfWeaklyTypedProperties"" />
    </xs:sequence>
    <xs:attribute name=""DiscountId"" type=""xs:int"" />
    <xs:attribute name=""LastModified"" type=""xs:dateTime"" />
    <xs:attribute name=""Priority"" type=""xs:int"" />
    <xs:attribute name=""DiscountAmount"" type=""xs:decimal"" />
    <xs:attribute name=""DiscountValue"" type=""xs:decimal"" />
    <xs:attribute name=""TypeOfDiscount"" type=""DiscountType"" />
    <xs:attribute name=""BasketDisplayMessage"" type=""xs:string"" />
    <xs:attribute name=""LineItemId"" type=""Guid"" />
    <xs:attribute name=""PromoCode"" type=""xs:string"" />
    <xs:attribute name=""OrderGroupId"" type=""Guid"" />
    <xs:attribute name=""DiscountName"" type=""xs:string"" />
    <xs:attribute name=""PromoCodeDefinitionId"" type=""xs:int"" />
  </xs:complexType>
  <xs:complexType name=""ArrayOfString"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""String"" type=""xs:anyType"" />
    </xs:sequence>
  </xs:complexType>
  <xs:complexType name=""ArrayOfAnyType"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""AnyType"" type=""AnyType"" />
    </xs:sequence>
  </xs:complexType>
  <xs:complexType name=""AnyType"">
    <xs:attribute name=""Type"" type=""xs:string"" />
    <xs:attribute name=""Value"" type=""xs:string"" />
  </xs:complexType>
  <xs:simpleType name=""StatusCodes"">
    <xs:restriction base=""xs:string"">
      <xs:enumeration value=""Basket"" />
      <xs:enumeration value=""OrderTemplate"" />
      <xs:enumeration value=""PurchaseOrder"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""PromoCodeState"">
    <xs:restriction base=""xs:string"">
      <xs:enumeration value=""OK"" />
      <xs:enumeration value=""Invalid"" />
      <xs:enumeration value=""LimitReached"" />
      <xs:enumeration value=""IdentityMismatch"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""PaymentMethodTypes"">
    <xs:restriction base=""xs:string"">
      <xs:enumeration value=""CreditCard"" />
      <xs:enumeration value=""GiftCertificate"" />
      <xs:enumeration value=""PurchaseOrder"" />
      <xs:enumeration value=""CashCard"" />
      <xs:enumeration value=""Custom"" />
      <xs:enumeration value=""Custom1"" />
      <xs:enumeration value=""Custom2"" />
      <xs:enumeration value=""Custom3"" />
      <xs:enumeration value=""Custom4"" />
      <xs:enumeration value=""Custom5"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""InventoryCondition"">
    <xs:restriction base=""xs:string"">
      <xs:enumeration value=""InStock"" />
      <xs:enumeration value=""Preordered"" />
      <xs:enumeration value=""Backordered"" />
      <xs:enumeration value=""OutOfStock"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name=""DiscountType"">
    <xs:restriction base=""xs:string"">
      <xs:enumeration value=""CurrencyValue"" />
      <xs:enumeration value=""Percentage"" />
    </xs:restriction>
  </xs:simpleType>
  <xs:complexType name=""ArrayOfWeaklyTypedProperties"">
    <xs:sequence>
      <xs:element minOccurs=""0"" maxOccurs=""unbounded"" name=""WeaklyTypedProperty"" type=""WeaklyTypedProperty"" />
    </xs:sequence>
  </xs:complexType>
  <xs:complexType name=""WeaklyTypedProperty"">
    <xs:attribute name=""Name"" type=""xs:string"" />
    <xs:attribute name=""Type"" type=""xs:string"" />
    <xs:attribute name=""Value"" type=""xs:string"" />
  </xs:complexType>
  <xs:simpleType name=""Char"">
    <xs:restriction base=""xs:unsignedShort"" />
  </xs:simpleType>
  <xs:simpleType name=""Guid"">
    <xs:restriction base=""xs:string"">
      <xs:pattern value=""[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}"" />
    </xs:restriction>
  </xs:simpleType>
</xs:schema>";
        
        public BTOrderGroupSchema() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "OrderGroups";
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
