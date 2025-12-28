namespace CartMigration {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"CartMigration.xml_cartmigration_sp", typeof(global::CartMigration.xml_cartmigration_sp))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"CartMigration.CartMigrationRequestBackup", typeof(global::CartMigration.CartMigrationRequestBackup))]
    public sealed class Transform_4 : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0"" version=""1.0"" xmlns:ns0=""http://CartMigration.CartMigrationRequestBackup"" xmlns:s0=""BTNextGen.Biztalk.CartsMigration"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:Carts"" />
  </xsl:template>
  <xsl:template match=""/s0:Carts"">
    <ns0:CartMigrationRequestBackup>
      <BasketList>
        <xsl:for-each select=""s0:CartListing"">
          <xsl:for-each select=""s0:Cart"">
            <BasketDetails>
              <BasketCreateDate>
                <xsl:value-of select=""s0:BasketCreatedDate/text()"" />
              </BasketCreateDate>
              <BasketModifyDate>
                <xsl:value-of select=""s0:BasketModifiedDate/text()"" />
              </BasketModifyDate>
              <BasketPONumber>
                <xsl:value-of select=""s0:BasketPONumber/text()"" />
              </BasketPONumber>
              <BasketSpecialInstructions>
                <xsl:value-of select=""s0:BasketSpecialInstructions/text()"" />
              </BasketSpecialInstructions>
              <BasketGiftFee>
                <xsl:value-of select=""s0:BasketStoreGiftWrapFee/text()"" />
              </BasketGiftFee>
              <BasketOrderFee>
                <xsl:value-of select=""s0:BasketStoreOrderFee/text()"" />
              </BasketOrderFee>
              <BasketProcessingFee>
                <xsl:value-of select=""s0:BasketStoreProcessingFee/text()"" />
              </BasketProcessingFee>
              <BasketShipFee>
                <xsl:value-of select=""s0:BasketStoreShippingFee/text()"" />
              </BasketShipFee>
              <IsHomeDeliveryIndicator>
                <xsl:value-of select=""s0:IsHomeDeliveryIndicator/text()"" />
              </IsHomeDeliveryIndicator>
              <LegacyBasketID>
                <xsl:value-of select=""s0:LegacyBasketId/text()"" />
              </LegacyBasketID>
              <LegacyBasketName>
                <xsl:value-of select=""s0:LegacyBasketName/text()"" />
              </LegacyBasketName>
              <LegacySourceSystem>
                <xsl:value-of select=""s0:LegacySourceSystem/text()"" />
              </LegacySourceSystem>
              <DetailLists>
                <xsl:for-each select=""s0:CartDetails/s0:CartDetail"">
                  <BasketItemDetail>
                    <BTkey>
                      <xsl:value-of select=""s0:Btkey/text()"" />
                    </BTkey>
                    <LegacyLineItemID>
                      <xsl:value-of select=""s0:LegacyLineItmId/text()"" />
                    </LegacyLineItemID>
                    <LegacyPONumber>
                      <xsl:value-of select=""s0:LinePONumber/text()"" />
                    </LegacyPONumber>
                    <ProductType>
                      <xsl:value-of select=""s0:ProductType/text()"" />
                    </ProductType>
                    <Qty>
                      <xsl:value-of select=""s0:Quantity/text()"" />
                    </Qty>
                  </BasketItemDetail>
                </xsl:for-each>
              </DetailLists>
            </BasketDetails>
          </xsl:for-each>
        </xsl:for-each>
      </BasketList>
    </ns0:CartMigrationRequestBackup>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"CartMigration.xml_cartmigration_sp";
        
        private const string _strTrgSchemasList0 = @"CartMigration.CartMigrationRequestBackup";
        
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
                _TrgSchemas[0] = @"CartMigration.CartMigrationRequestBackup";
                return _TrgSchemas;
            }
        }
    }
}
