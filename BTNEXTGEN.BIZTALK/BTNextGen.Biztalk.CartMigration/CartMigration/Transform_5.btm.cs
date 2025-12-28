namespace CartMigration {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"CartMigration.IOrder_tempuri_org+CreateLegacyBaskets", typeof(global::CartMigration.IOrder_tempuri_org.CreateLegacyBaskets))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"CartMigration.CartMigrationRequestBackup", typeof(global::CartMigration.CartMigrationRequestBackup))]
    public sealed class Transform_5 : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0 s1"" version=""1.0"" xmlns:s0=""http://schemas.datacontract.org/2004/07/BTNextGen.Services.IOrders"" xmlns:ns0=""http://CartMigration.CartMigrationRequestBackup"" xmlns:s1=""http://tempuri.org/"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s1:CreateLegacyBaskets"" />
  </xsl:template>
  <xsl:template match=""/s1:CreateLegacyBaskets"">
    <ns0:CartMigrationRequestBackup>
      <BasketList>
        <xsl:for-each select=""s1:basketList"">
          <xsl:for-each select=""s0:BasketDetails"">
            <BasketDetails>
              <xsl:if test=""s0:AccountInventoryReserveNumber"">
                <AcctInventoryReserveNumber>
                  <xsl:value-of select=""s0:AccountInventoryReserveNumber/text()"" />
                </AcctInventoryReserveNumber>
              </xsl:if>
              <xsl:if test=""s0:AccountInventoryType"">
                <AcctInventoryType>
                  <xsl:value-of select=""s0:AccountInventoryType/text()"" />
                </AcctInventoryType>
              </xsl:if>
              <xsl:if test=""s0:AccountWarehouseList"">
                <AcctWarehouseList>
                  <xsl:value-of select=""s0:AccountWarehouseList/text()"" />
                </AcctWarehouseList>
              </xsl:if>
              <xsl:if test=""s0:BasketCreatedDate"">
                <BasketCreateDate>
                  <xsl:value-of select=""s0:BasketCreatedDate/text()"" />
                </BasketCreateDate>
              </xsl:if>
              <xsl:if test=""s0:BasketIsBTCart"">
                <BasketIsBTCart>
                  <xsl:value-of select=""s0:BasketIsBTCart/text()"" />
                </BasketIsBTCart>
              </xsl:if>
              <xsl:if test=""s0:BasketIsTolas"">
                <BasketIsTolas>
                  <xsl:value-of select=""s0:BasketIsTolas/text()"" />
                </BasketIsTolas>
              </xsl:if>
              <xsl:if test=""s0:BasketModifiedDate"">
                <BasketModifyDate>
                  <xsl:value-of select=""s0:BasketModifiedDate/text()"" />
                </BasketModifyDate>
              </xsl:if>
              <xsl:if test=""s0:BasketNote"">
                <BasketNote>
                  <xsl:value-of select=""s0:BasketNote/text()"" />
                </BasketNote>
              </xsl:if>
              <xsl:if test=""s0:BasketPONumber"">
                <BasketPONumber>
                  <xsl:value-of select=""s0:BasketPONumber/text()"" />
                </BasketPONumber>
              </xsl:if>
              <xsl:if test=""s0:BasketSpecialInstructions"">
                <BasketSpecialInstructions>
                  <xsl:value-of select=""s0:BasketSpecialInstructions/text()"" />
                </BasketSpecialInstructions>
              </xsl:if>
              <xsl:if test=""s0:BasketStatus"">
                <BasketStatus>
                  <xsl:value-of select=""s0:BasketStatus/text()"" />
                </BasketStatus>
              </xsl:if>
              <xsl:if test=""s0:BasketStoreGiftWrapFee"">
                <BasketGiftFee>
                  <xsl:value-of select=""s0:BasketStoreGiftWrapFee/text()"" />
                </BasketGiftFee>
              </xsl:if>
              <xsl:if test=""s0:BasketStoreOrderFee"">
                <BasketOrderFee>
                  <xsl:value-of select=""s0:BasketStoreOrderFee/text()"" />
                </BasketOrderFee>
              </xsl:if>
              <xsl:if test=""s0:BasketStoreProcessingFee"">
                <BasketProcessingFee>
                  <xsl:value-of select=""s0:BasketStoreProcessingFee/text()"" />
                </BasketProcessingFee>
              </xsl:if>
              <xsl:if test=""s0:BasketStoreShippingFee"">
                <BasketShipFee>
                  <xsl:value-of select=""s0:BasketStoreShippingFee/text()"" />
                </BasketShipFee>
              </xsl:if>
              <xsl:if test=""s0:BasketTotal"">
                <BasketTotal>
                  <xsl:value-of select=""s0:BasketTotal/text()"" />
                </BasketTotal>
              </xsl:if>
              <xsl:if test=""s0:CSBookAccountId"">
                <BasketBookID>
                  <xsl:value-of select=""s0:CSBookAccountId/text()"" />
                </BasketBookID>
              </xsl:if>
              <xsl:if test=""s0:CSEntertainmentAccountId"">
                <BasketEntID>
                  <xsl:value-of select=""s0:CSEntertainmentAccountId/text()"" />
                </BasketEntID>
              </xsl:if>
              <xsl:if test=""s0:CSUserId"">
                <BasketUser>
                  <xsl:value-of select=""s0:CSUserId/text()"" />
                </BasketUser>
              </xsl:if>
              <xsl:if test=""s0:HomeDeliveryAccountId"">
                <BasketHomeDeliveryAcct>
                  <xsl:value-of select=""s0:HomeDeliveryAccountId/text()"" />
                </BasketHomeDeliveryAcct>
              </xsl:if>
              <xsl:if test=""s0:HomeDeliveryAddressType"">
                <BasketHomeDeliveryAcctType>
                  <xsl:value-of select=""s0:HomeDeliveryAddressType/text()"" />
                </BasketHomeDeliveryAcctType>
              </xsl:if>
              <xsl:if test=""s0:IsHomeDeliveryIndicator"">
                <IsHomeDeliveryIndicator>
                  <xsl:value-of select=""s0:IsHomeDeliveryIndicator/text()"" />
                </IsHomeDeliveryIndicator>
              </xsl:if>
              <xsl:if test=""s0:LegacyBasketId"">
                <LegacyBasketID>
                  <xsl:value-of select=""s0:LegacyBasketId/text()"" />
                </LegacyBasketID>
              </xsl:if>
              <xsl:if test=""s0:LegacyBasketName"">
                <LegacyBasketName>
                  <xsl:value-of select=""s0:LegacyBasketName/text()"" />
                </LegacyBasketName>
              </xsl:if>
              <xsl:if test=""s0:LegacySourceSystem"">
                <LegacySourceSystem>
                  <xsl:value-of select=""s0:LegacySourceSystem/text()"" />
                </LegacySourceSystem>
              </xsl:if>
              <DetailLists>
                <xsl:for-each select=""s0:LineItemList"">
                  <xsl:for-each select=""s0:LineItemDetails"">
                    <BasketItemDetail>
                      <xsl:if test=""s0:BTBibNumber"">
                        <BTBib>
                          <xsl:value-of select=""s0:BTBibNumber/text()"" />
                        </BTBib>
                      </xsl:if>
                      <xsl:if test=""s0:BTGiftWrapCode"">
                        <BTGwcode>
                          <xsl:value-of select=""s0:BTGiftWrapCode/text()"" />
                        </BTGwcode>
                      </xsl:if>
                      <xsl:if test=""s0:BTGiftWrapMessage"">
                        <BTGwmsg>
                          <xsl:value-of select=""s0:BTGiftWrapMessage/text()"" />
                        </BTGwmsg>
                      </xsl:if>
                      <xsl:if test=""s0:BTItemType"">
                        <BTItemType>
                          <xsl:value-of select=""s0:BTItemType/text()"" />
                        </BTItemType>
                      </xsl:if>
                      <xsl:if test=""s0:BTKey"">
                        <BTkey>
                          <xsl:value-of select=""s0:BTKey/text()"" />
                        </BTkey>
                      </xsl:if>
                      <xsl:if test=""s0:BTTitle"">
                        <BTTitle>
                          <xsl:value-of select=""s0:BTTitle/text()"" />
                        </BTTitle>
                      </xsl:if>
                      <xsl:if test=""s0:BTTitleEditionVersion"">
                        <BTEdition>
                          <xsl:value-of select=""s0:BTTitleEditionVersion/text()"" />
                        </BTEdition>
                      </xsl:if>
                      <xsl:if test=""s0:BTVolumeSet"">
                        <BTVolume>
                          <xsl:value-of select=""s0:BTVolumeSet/text()"" />
                        </BTVolume>
                      </xsl:if>
                      <xsl:if test=""s0:LegacyBasketId"">
                        <DETLegacyBasketID>
                          <xsl:value-of select=""s0:LegacyBasketId/text()"" />
                        </DETLegacyBasketID>
                      </xsl:if>
                      <xsl:if test=""s0:LegacyCreatedDate"">
                        <LegacyCreateDate>
                          <xsl:value-of select=""s0:LegacyCreatedDate/text()"" />
                        </LegacyCreateDate>
                      </xsl:if>
                      <xsl:if test=""s0:LegacyLineItemId"">
                        <LegacyLineItemID>
                          <xsl:value-of select=""s0:LegacyLineItemId/text()"" />
                        </LegacyLineItemID>
                      </xsl:if>
                      <xsl:if test=""s0:LegacyModifiedDate"">
                        <LegacyModifyDate>
                          <xsl:value-of select=""s0:LegacyModifiedDate/text()"" />
                        </LegacyModifyDate>
                      </xsl:if>
                      <xsl:if test=""s0:LinePONumber"">
                        <LegacyPONumber>
                          <xsl:value-of select=""s0:LinePONumber/text()"" />
                        </LegacyPONumber>
                      </xsl:if>
                      <xsl:if test=""s0:ListPrice"">
                        <ListPrice>
                          <xsl:value-of select=""s0:ListPrice/text()"" />
                        </ListPrice>
                      </xsl:if>
                      <xsl:if test=""s0:PlacedPrice"">
                        <PlacePrice>
                          <xsl:value-of select=""s0:PlacedPrice/text()"" />
                        </PlacePrice>
                      </xsl:if>
                      <xsl:if test=""s0:ProductCatalog"">
                        <ProductCatalog>
                          <xsl:value-of select=""s0:ProductCatalog/text()"" />
                        </ProductCatalog>
                      </xsl:if>
                      <xsl:if test=""s0:ProductType"">
                        <ProductType>
                          <xsl:value-of select=""s0:ProductType/text()"" />
                        </ProductType>
                      </xsl:if>
                      <xsl:if test=""s0:Quantity"">
                        <Qty>
                          <xsl:value-of select=""s0:Quantity/text()"" />
                        </Qty>
                      </xsl:if>
                    </BasketItemDetail>
                  </xsl:for-each>
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
        
        private const string _strSrcSchemasList0 = @"CartMigration.IOrder_tempuri_org+CreateLegacyBaskets";
        
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
                _SrcSchemas[0] = @"CartMigration.IOrder_tempuri_org+CreateLegacyBaskets";
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
