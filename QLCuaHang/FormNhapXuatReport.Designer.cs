namespace QLCuaHang
{
    partial class FormNhapXuatReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormNhapXuatReport));
            this.NhapXuatRPV = new Microsoft.Reporting.WinForms.ReportViewer();
            this.SuspendLayout();
            // 
            // NhapXuatRPV
            // 
            this.NhapXuatRPV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NhapXuatRPV.Location = new System.Drawing.Point(0, 0);
            this.NhapXuatRPV.Name = "NhapXuatRPV";
            this.NhapXuatRPV.ServerReport.BearerToken = null;
            this.NhapXuatRPV.Size = new System.Drawing.Size(1050, 629);
            this.NhapXuatRPV.TabIndex = 0;
            // 
            // FormNhapXuatReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1050, 629);
            this.Controls.Add(this.NhapXuatRPV);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormNhapXuatReport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BÁO CÁO THÔNG TIN NHẬP XUẤT";
            this.Load += new System.EventHandler(this.FormNhapXuatReport_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer NhapXuatRPV;
    }
}