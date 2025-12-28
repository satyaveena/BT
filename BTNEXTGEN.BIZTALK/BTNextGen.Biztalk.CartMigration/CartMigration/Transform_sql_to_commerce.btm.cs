namespace CartMigration {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"CartMigration.xml_cartmigration_sp", typeof(global::CartMigration.xml_cartmigration_sp))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"CartMigration.IOrder_tempuri_org+CreateLegacyBaskets", typeof(global::CartMigration.IOrder_tempuri_org.CreateLegacyBaskets))]
    public sealed class Transform_sql_to_commerce : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0"" version=""1.0"" xmlns:ns0=""http://tempuri.org/"" xmlns:s0=""BTNextGen.Biztalk.CartsMigration"" xmlns:ns1=""http://schemas.datacontract.org/2004/07/BTNextGen.Services.IOrders"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:Carts"" />
  </xsl:template>
  <xsl:template match=""/s0:Carts"">
    <ns0:CreateLegacyBaskets>
      <ns0:basketList>
        <xsl:for-each select=""s0:CartListing"">
          <xsl:for-each select=""s0:Cart"">
            <ns1:BasketDetails>
              <ns1:AccountInventoryReserveNumber>
                <xsl:value-of select=""s0:AccountInventoryReserveNumber/text()"" />
              </ns1:AccountInventoryReserveNumber>
              <ns1:AccountInventoryType>
                <xsl:value-of select=""s0:AccountInventoryType/text()"" />
              </ns1:AccountInventoryType>
              <ns1:AccountWarehouseList>
                <xsl:value-of select=""s0:AccountWarehouseList/text()"" />
              </ns1:AccountWarehouseList>
              <ns1:BasketCreatedDate>
                <xsl:value-of select=""s0:BasketCreatedDate/text()"" />
              </ns1:BasketCreatedDate>
              <ns1:BasketIsBTCart>
                <xsl:value-of select=""s0:BasketIsBTCart/text()"" />
              </ns1:BasketIsBTCart>
              <ns1:BasketIsTolas>
                <xsl:value-of select=""s0:BasketIsTolas/text()"" />
              </ns1:BasketIsTolas>
              <ns1:BasketModifiedDate>
                <xsl:value-of select=""s0:BasketModifiedDate/text()"" />
              </ns1:BasketModifiedDate>
              <ns1:BasketNote>
                <xsl:value-of select=""s0:BasketNote/text()"" />
              </ns1:BasketNote>
              <ns1:BasketPONumber>
                <xsl:value-of select=""s0:BasketPONumber/text()"" />
              </ns1:BasketPONumber>
              <ns1:BasketSpecialInstructions>
                <xsl:value-of select=""s0:BasketSpecialInstructions/text()"" />
              </ns1:BasketSpecialInstructions>
              <ns1:BasketStatus>
                <xsl:value-of select=""s0:BasketStatus/text()"" />
              </ns1:BasketStatus>
              <ns1:BasketStoreGiftWrapFee>
                <xsl:value-of select=""s0:BasketStoreGiftWrapFee/text()"" />
              </ns1:BasketStoreGiftWrapFee>
              <ns1:BasketStoreOrderFee>
                <xsl:value-of select=""s0:BasketStoreOrderFee/text()"" />
              </ns1:BasketStoreOrderFee>
              <ns1:BasketStoreProcessingFee>
                <xsl:value-of select=""s0:BasketStoreProcessingFee/text()"" />
              </ns1:BasketStoreProcessingFee>
              <ns1:BasketStoreShippingFee>
                <xsl:value-of select=""s0:BasketStoreShippingFee/text()"" />
              </ns1:BasketStoreShippingFee>
              <ns1:BasketTotal>
                <xsl:value-of select=""s0:BasketTotal/text()"" />
              </ns1:BasketTotal>
              <ns1:CSBookAccountId>
                <xsl:value-of select=""s0:LegacyBookAccountId/text()"" />
              </ns1:CSBookAccountId>
              <ns1:CSEntertainmentAccountId>
                <xsl:value-of select=""s0:LegacyEntertainmentAccountId/text()"" />
              </ns1:CSEntertainmentAccountId>
              <ns1:CSUserId>
                <xsl:value-of select=""s0:CSUserID/text()"" />
              </ns1:CSUserId>
              <ns1:HomeDeliveryAccountId>
                <xsl:value-of select=""s0:HomeDeliveryAccountId/text()"" />
              </ns1:HomeDeliveryAccountId>
              <ns1:HomeDeliveryAddressType>
                <xsl:value-of select=""s0:HomeDeliveryAddressType/text()"" />
              </ns1:HomeDeliveryAddressType>
              <ns1:IsHomeDeliveryIndicator>
                <xsl:value-of select=""s0:IsHomeDeliveryIndicator/text()"" />
              </ns1:IsHomeDeliveryIndicator>
              <ns1:LegacyBasketId>
                <xsl:value-of select=""s0:LegacyBasketId/text()"" />
              </ns1:LegacyBasketId>
              <ns1:LegacyBasketName>
                <xsl:value-of select=""s0:LegacyBasketName/text()"" />
              </ns1:LegacyBasketName>
              <ns1:LegacySourceSystem>
                <xsl:value-of select=""s0:LegacySourceSystem/text()"" />
              </ns1:LegacySourceSystem>
              <ns1:LineItemList>
                <xsl:for-each select=""s0:CartDetails/s0:CartDetail"">
                  <ns1:LineItemDetails>
                    <ns1:BTBibNumber>
                      <xsl:value-of select=""s0:BTBibNumber/text()"" />
                    </ns1:BTBibNumber>
                    <ns1:BTGiftWrapCode>
                      <xsl:value-of select=""s0:BTGiftWrapCode/text()"" />
                    </ns1:BTGiftWrapCode>
                    <ns1:BTGiftWrapMessage>
                      <xsl:value-of select=""s0:BTGiftWrapMessage/text()"" />
                    </ns1:BTGiftWrapMessage>
                    <ns1:BTItemType>
                      <xsl:value-of select=""s0:BTItemType/text()"" />
                    </ns1:BTItemType>
                    <ns1:BTKey>
                      <xsl:value-of select=""s0:Btkey/text()"" />
                    </ns1:BTKey>
                    <ns1:BTTitle>
                      <xsl:value-of select=""s0:BTTitle/text()"" />
                    </ns1:BTTitle>
                    <ns1:BTTitleEditionVersion>
                      <xsl:value-of select=""s0:BTTitleEditionVersion/text()"" />
                    </ns1:BTTitleEditionVersion>
                    <ns1:BTVolumeSet>
                      <xsl:value-of select=""s0:BTVolumSet/text()"" />
                    </ns1:BTVolumeSet>
                    <ns1:LegacyBasketId>
                      <xsl:value-of select=""s0:LegacyBasketId/text()"" />
                    </ns1:LegacyBasketId>
                    <ns1:LegacyCreatedDate>
                      <xsl:value-of select=""s0:LegacyCreatedDate/text()"" />
                    </ns1:LegacyCreatedDate>
                    <ns1:LegacyLineItemId>
                      <xsl:value-of select=""s0:LegacyLineItmId/text()"" />
                    </ns1:LegacyLineItemId>
                    <ns1:LegacyModifiedDate>
                      <xsl:value-of select=""s0:LegacyModifiedDate/text()"" />
                    </ns1:LegacyModifiedDate>
                    <ns1:LinePONumber>
                      <xsl:value-of select=""s0:LinePONumber/text()"" />
                    </ns1:LinePONumber>
                    <ns1:ListPrice>
                      <xsl:value-of select=""s0:ListPrice/text()"" />
                    </ns1:ListPrice>
                    <ns1:PlacedPrice>
                      <xsl:value-of select=""s0:PlacedPrice/text()"" />
                    </ns1:PlacedPrice>
                    <ns1:ProductCatalog>
                      <xsl:value-of select=""s0:ProductCatalog/text()"" />
                    </ns1:ProductCatalog>
                    <ns1:ProductType>
                      <xsl:value-of select=""s0:ProductType/text()"" />
                    </ns1:ProductType>
                    <ns1:Quantity>
                      <xsl:value-of select=""s0:Quantity/text()"" />
                    </ns1:Quantity>
                  </ns1:LineItemDetails>
                </xsl:for-each>
              </ns1:LineItemList>
            </ns1:BasketDetails>
          </xsl:for-each>
        </xsl:for-each>
      </ns0:basketList>
    </ns0:CreateLegacyBaskets>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"CartMigration.xml_cartmigration_sp";
        
        private const string _strTrgSchemasList0 = @"CartMigration.IOrder_tempuri_org+CreateLegacyBaskets";
        
        public override string XmlContent {
            get {
                return _strMap;
            }
        }
        
        public override string XsltArgumentListContent {
            get {
                return _strArgList;
            }
        }
        
        public override string[] SourceSchemas {
            get {
                string[] _SrcSchemas = new string [1];
                _SrcSchemas[0] = @"CartMigration.xml_cartmigration_sp";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"CartMigration.IOrder_tempuri_org+CreateLegacyBaskets";
                return _TrgSchemas;
            }
        }
    }
}
