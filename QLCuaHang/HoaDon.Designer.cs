namespace QLCuaHang
{
    partial class HoaDon
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HoaDon));
            this.HoaDonRPV = new Microsoft.Reporting.WinForms.ReportViewer();
            this.SuspendLayout();
            // 
            // HoaDonRPV
            // 
            this.HoaDonRPV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HoaDonRPV.Location = new System.Drawing.Point(0, 0);
            this.HoaDonRPV.Name = "HoaDonRPV";
            this.HoaDonRPV.ServerReport.BearerToken = null;
            this.HoaDonRPV.Size = new System.Drawing.Size(491, 574);
            this.HoaDonRPV.TabIndex = 0;
            // 
            // HoaDon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(491, 574);
            this.Controls.Add(this.HoaDonRPV);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "HoaDon";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HÓA ĐƠN";
            this.Load += new System.EventHandler(this.HoaDon_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer HoaDonRPV;
    }
}