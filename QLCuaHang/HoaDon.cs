using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BusinessAccessLayer;

namespace QLCuaHang
{
    public partial class HoaDon : Form
    {
        public HoaDon()
        {
            InitializeComponent();
        }
        public static string manx;
        ReportBAL rpBAL = new ReportBAL();
        private void HoaDon_Load(object sender, EventArgs e)
        {
            try
            {
                if(manx != string.Empty)
                {
                    HoaDonRPV.Reset();
                    HoaDonRPV.LocalReport.ReportEmbeddedResource = "QLCuaHang.HoaDonRP.rdlc";
                    ReportDataSource rps = new ReportDataSource("dsHoaDon", rpBAL.ChiTietByMaNX(manx));
                    foreach (DataRow dr in rpBAL.NhapXuat(manx.Substring(0, 1), null, null).Rows)
                    {
                        if (dr["MaNX"].ToString() == manx)
                        {
                            ReportParameter[] para = new ReportParameter[7];
                            para[0] = new ReportParameter("paraMaNX", dr["MaNX"].ToString());
                            para[1] = new ReportParameter("paraNgay", dr["Ngay"].ToString());
                            para[2] = new ReportParameter("paraMaKhach", dr["MaKhach"].ToString());
                            para[3] = new ReportParameter("paraTenKhach", dr["TenKhach"].ToString());
                            para[4] = new ReportParameter("paraSDT", dr["SDT"].ToString());
                            para[5] = new ReportParameter("paraDiaChi", dr["DiaChi"].ToString());
                            para[6] = new ReportParameter("paraGhiChu", dr["GhiChu"].ToString());
                            HoaDonRPV.LocalReport.SetParameters(para);
                        }
                    }
                    HoaDonRPV.LocalReport.DataSources.Add(rps);
                    HoaDonRPV.RefreshReport();
                }
            }
            catch(Exception ex)
            {
                MyMeThod myme = new MyMeThod();
                myme.ShowError(ex);
            }
        }
    }
}
