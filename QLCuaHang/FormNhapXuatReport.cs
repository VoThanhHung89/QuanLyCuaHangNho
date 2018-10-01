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
    public partial class FormNhapXuatReport : Form
    {
        public FormNhapXuatReport()
        {
            InitializeComponent();
        }
        ReportBAL rpBAL = new ReportBAL();
        /// <summary>
        /// Loại Report:
        /// A - Tất cả.
        /// N - Tất cả phiếu Nhập.
        /// X - Tất cả phiếu Xuất.
        /// Hoặc riêng từng mã phiếu.(Là mã của phiếu)
        /// </summary>
        public static string LoaiReport;
        /// <summary>
        /// Dùng để lấy kết quả trong khoảng thoài gian hoặc từng ngày.
        /// </summary>
        public static DateTime? NgayBD, NgayKT;
        private void FormNhapXuatReport_Load(object sender, EventArgs e)
        {
            NhapXuatRPV.Reset();
            //Report Tất cả.
            if(LoaiReport == "A" && NgayBD == null && NgayKT == null) 
            {
                NhapXuatRPV.LocalReport.ReportEmbeddedResource = "QLCuaHang.NhapXuatAllRP.rdlc";
                ReportDataSource rps = new ReportDataSource("dsNXAll", rpBAL.NhapXuat(LoaiReport, null, null));
                NhapXuatRPV.LocalReport.DataSources.Add(rps);
            }
            //Report Tất cả nhưng tùy thuộc vào loại phiếu.
            if((LoaiReport == "N" || LoaiReport == "X" ) && NgayBD == null && NgayKT == null)
            {
                NhapXuatRPV.LocalReport.ReportEmbeddedResource = "QLCuaHang.NhapXuatTheoLoaiRP.rdlc";
                ReportDataSource rps = new ReportDataSource("dsNhapXuatTheoLoai", rpBAL.NhapXuat(LoaiReport, null, null));
                ReportParameter para = new ReportParameter("paraNhapXuatTheoLoai", LoaiReport);
                NhapXuatRPV.LocalReport.SetParameters(para);
                NhapXuatRPV.LocalReport.DataSources.Add(rps);
            }
            //Report Thông tin của Phiếu dựa trên mã phiếu. LoaiReport = Mã Phiếu.
            if(LoaiReport.Length > 1 && NgayBD == null && NgayKT == null)
            {
                NhapXuatRPV.LocalReport.ReportEmbeddedResource = "QLCuaHang.NhapXuatTheoMaRP.rdlc";
                ReportDataSource rps = new ReportDataSource("dsNhapXuatTheoMa", rpBAL.ChiTietByMaNX(LoaiReport));
                foreach (DataRow dr in rpBAL.NhapXuat(LoaiReport.Substring(0, 1), null, null).Rows)
                {
                    if(dr["MaNX"].ToString() == LoaiReport)
                    {
                        ReportParameter[] para = new ReportParameter[7];
                        para[0] = new ReportParameter("paraMaNX", dr["MaNX"].ToString());
                        para[1] = new ReportParameter("paraNgay", dr["Ngay"].ToString());
                        para[2] = new ReportParameter("paraMaKhach", dr["MaKhach"].ToString());
                        para[3] = new ReportParameter("paraTenKhach", dr["TenKhach"].ToString());
                        para[4] = new ReportParameter("paraSDT", dr["SDT"].ToString());
                        para[5] = new ReportParameter("paraDiaChi", dr["DiaChi"].ToString());
                        para[6] = new ReportParameter("paraGhiChu", dr["GhiChu"].ToString());
                        NhapXuatRPV.LocalReport.SetParameters(para);
                    }
                }
                NhapXuatRPV.LocalReport.DataSources.Add(rps);
            }
            //Report Danh sách Phiếu Nhập - Phiếu Xuất trong khoảng thời gian.
            if(LoaiReport == "A" && NgayBD != null && NgayKT != null)
            {
                //Sử dụng lại mẫu Report của NhapXuatAllRP do cùng thiết kế.
                NhapXuatRPV.LocalReport.ReportEmbeddedResource = "QLCuaHang.NhapXuatAllRP.rdlc";
                ReportDataSource rps = new ReportDataSource("dsNXAll", rpBAL.NhapXuat(LoaiReport, NgayBD, NgayKT));
                ReportParameter[] para = new ReportParameter[]
                {
                    new ReportParameter("paraNgayBD", NgayBD.Value.ToString()),
                    new ReportParameter("paraNgayKT", NgayKT.Value.ToString())
                };
                NhapXuatRPV.LocalReport.SetParameters(para);
                NhapXuatRPV.LocalReport.DataSources.Add(rps);
            }
            //Report Danh sách Phiếu Nhập hoặc Phiếu Xuất trong khoảng thời gian.
            if ((LoaiReport == "N" || LoaiReport == "X") && NgayBD != null && NgayKT != null)
            {
                //Sử dụng lại mẫu Report của NhapXuatTheoLoaiRP do cùng thiết kế.
                NhapXuatRPV.LocalReport.ReportEmbeddedResource = "QLCuaHang.NhapXuatTheoLoaiRP.rdlc";
                ReportDataSource rps = new ReportDataSource("dsNhapXuatTheoLoai", rpBAL.NhapXuat(LoaiReport, NgayBD, NgayKT));
                ReportParameter[] para = new ReportParameter[]
                {
                    new ReportParameter("paraNgayBD", NgayBD.Value.ToString()),
                    new ReportParameter("paraNgayKT", NgayKT.Value.ToString()),
                    new ReportParameter("paraNhapXuatTheoLoai", LoaiReport)
                };
                NhapXuatRPV.LocalReport.SetParameters(para);
                NhapXuatRPV.LocalReport.DataSources.Add(rps);
            }
            this.NhapXuatRPV.RefreshReport();
        }
    }
}
