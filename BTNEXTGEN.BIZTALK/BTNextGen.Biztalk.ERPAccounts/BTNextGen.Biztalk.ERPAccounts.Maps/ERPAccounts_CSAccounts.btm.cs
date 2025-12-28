namespace BTNextGen.Biztalk.ERPAccounts.Maps {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.Biztalk.ERPAccounts.Schemas.ProcedureResultSet_dbo_procCommerceServerGetAccounts+StoredProcedureResultSet0", typeof(global::BTNextGen.Biztalk.ERPAccounts.Schemas.ProcedureResultSet_dbo_procCommerceServerGetAccounts.StoredProcedureResultSet0))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"BTNextGen.BizTalk.CS.Profiles.Schemas.BTProfile", typeof(global::BTNextGen.BizTalk.CS.Profiles.Schemas.BTProfile))]
    public sealed class ERPAccounts_CSAccounts : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0"" version=""1.0"" xmlns:s0=""http://schemas.microsoft.com/Sql/2008/05/ProceduresResultSets/dbo/procCommerceServerGetAccounts"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:StoredProcedureResultSet0"" />
  </xsl:template>
  <xsl:template match=""/s0:StoredProcedureResultSet0"">
    <ProfileDocument>
      <BTAccount>
        <BTNextGen>
          <xsl:if test=""s0:u_bt_account_id"">
            <account_id>
              <xsl:value-of select=""s0:u_bt_account_id/text()"" />
            </account_id>
          </xsl:if>
          <xsl:if test=""s0:u_account_type"">
            <account_type>
              <xsl:value-of select=""s0:u_account_type/text()"" />
            </account_type>
          </xsl:if>
          <xsl:if test=""s0:u_billing_address"">
            <billing_address>
              <xsl:value-of select=""s0:u_billing_address/text()"" />
            </billing_address>
          </xsl:if>
          <xsl:if test=""s0:u_primary_warehouse"">
            <primary_warehouse>
              <xsl:value-of select=""s0:u_primary_warehouse/text()"" />
            </primary_warehouse>
          </xsl:if>
          <product_type>
            <xsl:value-of select=""s0:u_product_type/text()"" />
          </product_type>
          <xsl:if test=""s0:u_san_suffix"">
            <san_suffix>
              <xsl:value-of select=""s0:u_san_suffix/text()"" />
            </san_suffix>
          </xsl:if>
          <xsl:if test=""s0:u_shipping_address"">
            <shipping_address>
              <xsl:value-of select=""s0:u_shipping_address/text()"" />
            </shipping_address>
          </xsl:if>
          <xsl:if test=""s0:u_secondary_warehouse"">
            <secondary_warehouse>
              <xsl:value-of select=""s0:u_secondary_warehouse/text()"" />
            </secondary_warehouse>
          </xsl:if>
          <xsl:if test=""s0:b_is_billing_account"">
            <is_billing_account>
              <xsl:value-of select=""s0:b_is_billing_account/text()"" />
            </is_billing_account>
          </xsl:if>
          <xsl:if test=""s0:b_is_shipping_account"">
            <is_shipping_account>
              <xsl:value-of select=""s0:b_is_shipping_account/text()"" />
            </is_shipping_account>
          </xsl:if>
          <xsl:if test=""s0:b_is_home_delivery"">
            <is_home_delivery>
              <xsl:value-of select=""s0:b_is_home_delivery/text()"" />
            </is_home_delivery>
          </xsl:if>
          <xsl:if test=""s0:u_account_name"">
            <account_name>
              <xsl:value-of select=""s0:u_account_name/text()"" />
            </account_name>
          </xsl:if>
          <xsl:if test=""s0:u_bill_to_account_number"">
            <bill_to_account_number>
              <xsl:value-of select=""s0:u_bill_to_account_number/text()"" />
            </bill_to_account_number>
          </xsl:if>
          <xsl:if test=""s0:u_inventory_type"">
            <inventory_type>
              <xsl:value-of select=""s0:u_inventory_type/text()"" />
            </inventory_type>
          </xsl:if>
          <xsl:if test=""s0:u_reserve_inventory_number"">
            <reserve_inventory_number>
              <xsl:value-of select=""s0:u_reserve_inventory_number/text()"" />
            </reserve_inventory_number>
          </xsl:if>
          <xsl:if test=""s0:b_check_reserve_flag"">
            <check_reserve_flag>
              <xsl:value-of select=""s0:b_check_reserve_flag/text()"" />
            </check_reserve_flag>
          </xsl:if>
          <xsl:if test=""s0:u_exclusive_item_group"">
            <exclusive_item_group>
              <xsl:value-of select=""s0:u_exclusive_item_group/text()"" />
            </exclusive_item_group>
          </xsl:if>
          <xsl:if test=""s0:u_erp_account_number"">
            <erp_account_number>
              <xsl:value-of select=""s0:u_erp_account_number/text()"" />
            </erp_account_number>
          </xsl:if>
          <xsl:if test=""s0:b_is_TOLAS"">
            <is_TOLAS>
              <xsl:value-of select=""s0:b_is_TOLAS/text()"" />
            </is_TOLAS>
          </xsl:if>
          <xsl:if test=""s0:u_sop_price_plan_list"">
            <sop_price_plan_list>
              <xsl:value-of select=""s0:u_sop_price_plan_list/text()"" />
            </sop_price_plan_list>
          </xsl:if>
          <xsl:if test=""s0:u_primary_payment_option"">
            <primary_payment_option>
              <xsl:value-of select=""s0:u_primary_payment_option/text()"" />
            </primary_payment_option>
          </xsl:if>
          <xsl:if test=""s0:u_warehouses"">
            <warehouses>
              <xsl:value-of select=""s0:u_warehouses/text()"" />
            </warehouses>
          </xsl:if>
          <xsl:if test=""s0:u_accounts"">
            <accounts>
              <xsl:value-of select=""s0:u_accounts/text()"" />
            </accounts>
          </xsl:if>
          <account8_id>
            <xsl:value-of select=""s0:u_account8_id/text()"" />
          </account8_id>
        </BTNextGen>
        <PricingSystem>
          <xsl:if test=""s0:i_minimum_margin"">
            <minimum_margin>
              <xsl:value-of select=""s0:i_minimum_margin/text()"" />
            </minimum_margin>
          </xsl:if>
        </PricingSystem>
      </BTAccount>
    </ProfileDocument>
  </xsl:template>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"BTNextGen.Biztalk.ERPAccounts.Schemas.ProcedureResultSet_dbo_procCommerceServerGetAccounts+StoredProcedureResultSet0";
        
        private const global::BTNextGen.Biztalk.ERPAccounts.Schemas.ProcedureResultSet_dbo_procCommerceServerGetAccounts.StoredProcedureResultSet0 _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"BTNextGen.BizTalk.CS.Profiles.Schemas.BTProfile";
        
        private const global::BTNextGen.BizTalk.CS.Profiles.Schemas.BTProfile _trgSchemaTypeReference0 = null;
        
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
                _SrcSchemas[0] = @"BTNextGen.Biztalk.ERPAccounts.Schemas.ProcedureResultSet_dbo_procCommerceServerGetAccounts+StoredProcedureResultSet0";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"BTNextGen.BizTalk.CS.Profiles.Schemas.BTProfile";
                return _TrgSchemas;
            }
        }
    }
}
