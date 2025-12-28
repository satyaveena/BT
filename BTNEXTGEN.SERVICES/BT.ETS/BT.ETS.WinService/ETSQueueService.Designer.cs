namespace BT.ETS.WinService
{
    partial class ETSQueueService
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Diagnostics.EventLog etsServiceLog;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.etsServiceLog = new System.Diagnostics.EventLog();
            ((System.ComponentModel.ISupportInitialize)(this.etsServiceLog)).BeginInit();
            // 
            // repriceServiceLog
            // 
            this.etsServiceLog.Log = "BTNG ETS Service Log";
            this.etsServiceLog.Source = "BTNextGen ETS Source";
            // 
            // ServiceHandler
            // 
            components = new System.ComponentModel.Container();
            this.ServiceName = "BTNextGen ETS Service";
            ((System.ComponentModel.ISupportInitialize)(this.etsServiceLog)).EndInit();
        }

        #endregion
    }
}
