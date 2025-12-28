using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BTNextGen.BizTalk.ViewCache
{
    public partial class ViewCache : Form
    {
        public ViewCache()
        {
            InitializeComponent();
        }

        private void btnGCache_Click(object sender, EventArgs e)
        {
            BTNextGen.BizTalk.SSO.Utility.CacheManger.Instance.PopulateCacheFromSSO();
            System.Diagnostics.EventLog.WriteEntry("SSO data is cached", "SSO data is cached");

            DataSet ds = new DataSet();
            ds = BTNextGen.BizTalk.SSO.Utility.CacheManger.Instance.ViewSSOCache();

            dataGridView1. = ds.Tables.;

        }
    }
}
