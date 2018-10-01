using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BusinessAccessLayer;
using Microsoft.Reporting.WinForms;

namespace QLCuaHang
{
    public partial class KhachHangUC : UserControl
    {
        public KhachHangUC()
        {
            InitializeComponent();
        }
        private static KhachHangUC _inStance;
        public static KhachHangUC Instance
        {
            get
            {
                if (_inStance == null)
                    _inStance = new KhachHangUC();
                return _inStance;
            }
            set
            {
                _inStance = value;
            }
        }
        MyMeThod myme = new MyMeThod();
        KhachHangBAL khBAL = new KhachHangBAL();
        ReportBAL rpBAL = new ReportBAL();
        private void BlindingData()
        {
            txtMa.Text = txtTen.Text = txtSDT.Text = txtDiaChi.Text = txtGhiChu.Text = string.Empty; txtMa.DataBindings.Clear();
            txtTen.DataBindings.Clear();
            txtSDT.DataBindings.Clear();
            txtDiaChi.DataBindings.Clear();
            txtGhiChu.DataBindings.Clear();
            txtMa.DataBindings.Add("Text", dgvKhachHang.DataSource, "makhach");
            txtTen.DataBindings.Add("Text", dgvKhachHang.DataSource, "tenkhach");
            txtSDT.DataBindings.Add("Text", dgvKhachHang.DataSource, "sdt");
            txtDiaChi.DataBindings.Add("Text", dgvKhachHang.DataSource, "diachi");
            txtGhiChu.DataBindings.Add("Text", dgvKhachHang.DataSource, "ghichu");
        }
        /// <summary>
        /// Trạng thái của các buuton và textbox.
        /// </summary>
        /// <param name="trangthai">binhthuong/them/sua</param>
        private void ManagerControl(string trangthai)
        {
            if (trangthai == "binhthuong")
            {
                dgvKhachHang.Enabled = true;
                txtTen.ReadOnly = txtSDT.ReadOnly = txtDiaChi.ReadOnly = txtGhiChu.ReadOnly = true;
                btnThem.Visible = btnSua.Visible = true;
                btnLuu.Enabled = false;
                btnXoa.Text = "XÓA";
            }
            if (trangthai == "them")
            {
                dgvKhachHang.Enabled = false;
                txtTen.ReadOnly = txtSDT.ReadOnly = txtDiaChi.ReadOnly = txtGhiChu.ReadOnly = false;
                txtMa.Text = myme.MaTuTang("K", khBAL.MaCuoiCung(), 10);
                txtTen.Text = txtSDT.Text = txtDiaChi.Text = txtGhiChu.Text = string.Empty;
                btnThem.Visible = btnSua.Visible = false;
                btnLuu.Enabled = true;
                btnXoa.Text = "HỦY";
            }
            if (trangthai == "sua")
            {
                dgvKhachHang.Enabled = false;
                txtTen.ReadOnly = txtSDT.ReadOnly = txtDiaChi.ReadOnly = txtGhiChu.ReadOnly = false;
                btnThem.Visible = btnSua.Visible = false;
                btnLuu.Enabled = true;
                btnXoa.Text = "HỦY";
            }
        }
        private void KhachHangUC_Load(object sender, EventArgs e)
        {
            khBAL.LoadGridViewAll(dgvKhachHang);
            BlindingData();
            ManagerControl("binhthuong");
            cboCachTim.SelectedIndex = 0;
            chbHienTC.Checked = true;
            chbHienTK.Checked = false;
        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            ManagerControl("them");
        }
        private void btnSua_Click(object sender, EventArgs e)
        {
            if(txtMa.Text.Trim() != string.Empty)
                ManagerControl("sua");
        }
        private void btnXoa_Click(object sender, EventArgs e)
        {
            int thutukhach = 0;
            if (btnXoa.Text == "XÓA" && txtMa.Text != string.Empty)
            {
                if (DialogResult.OK == MessageBox.Show("Bạn muốn xoá thông tin trên?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
                {
                    thutukhach = dgvKhachHang.CurrentRow.Index;
                    khBAL.DeleteKhach(txtMa.Text.Trim());
                }
            }
            ManagerControl("binhthuong");
            khBAL.LoadGridViewAll(dgvKhachHang);
            BlindingData();
            if (thutukhach > 1)
            {
                dgvKhachHang.ClearSelection();
                dgvKhachHang.Rows[thutukhach - 1].Selected = true;
            }
        }
        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (txtTen.Text != string.Empty)
            {
                string makhach = txtMa.Text.Trim();
                khBAL.AddOrEditKhach(makhach, txtTen.Text.Trim(), txtSDT.Text.Trim(), txtDiaChi.Text.Trim(), txtGhiChu.Text.Trim());
                khBAL.LoadGridViewAll(dgvKhachHang);
                ManagerControl("binhthuong");
                BlindingData();
                //Chuyển đến dòng vừa thực thi.
                foreach(DataGridViewRow dgr in dgvKhachHang.Rows) {
                    if (dgr.Cells["makhach"].Value.ToString() == makhach)
                    {
                        dgr.Selected = true;
                        //dgr.Cells["makhach"].Selected = true;
                    }
                    else
                        dgr.Selected = false;
                }
            }
            else MessageBox.Show("Tên Khách Hàng không được để trống.", "Thông báo");
        }
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            chbHienTK.Checked = true;
            chbHienTC.Checked = false;
            khBAL.TimKiemKhachHang(cboCachTim.Text, txtTuTim.Text.Trim(), dgvKhachHang);
            if (dgvKhachHang.RowCount == 0) MessageBox.Show("Không có kết quả nào khớp với dữ liệu tìm kiếm.", "Thông báo");
            BlindingData();
        }
        private void chbHienTC_Click(object sender, EventArgs e)
        {
            chbHienTC.Checked = true;
            chbHienTK.Checked = false;
            cboCachTim.SelectedIndex = 0;
            txtTuTim.Text = string.Empty;
            khBAL.LoadGridViewAll(dgvKhachHang);
            BlindingData();
        }
        private void chbHienTK_Click(object sender, EventArgs e)
        {
            chbHienTK.Checked = true;
            chbHienTC.Checked = false;
        }
        #region report
        private void btnReport_Click(object sender, EventArgs e)
        {
            if (rdbTCKhach.Checked)//Report Danh sách toàn bộ khách hàng.
            {
                
                KhachRPV.Reset();
                KhachRPV.LocalReport.ReportEmbeddedResource = "QLCuaHang.KhachAllRP.rdlc";
                ReportDataSource rps = new ReportDataSource("dsKhachAll", rpBAL.KhachAll());
                KhachRPV.LocalReport.DataSources.Add(rps);
                KhachRPV.RefreshReport();
            }
            if (rdbKhachGD.Checked && khBAL.CheckMa(cboMaKhach.Text.Trim()))//Report Danh sách giao dịch của khách hàng tương ứng.
            {
                KhachRPV.Reset();
                KhachRPV.LocalReport.ReportEmbeddedResource = "QLCuaHang.KhachGiaoDichRP.rdlc";
                ReportDataSource rpsn = new ReportDataSource("dsKhachGDNhap", rpBAL.KhachGiaoDich(cboMaKhach.Text.Trim(), Convert.ToChar("N")));
                ReportDataSource rpsx = new ReportDataSource("dsKhachGDXuat", rpBAL.KhachGiaoDich(cboMaKhach.Text.Trim(), Convert.ToChar("X")));
                KhachRPV.LocalReport.DataSources.Add(rpsn);
                KhachRPV.LocalReport.DataSources.Add(rpsx);
                KhachRPV.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(SubChiTietSPProcessing);
                KhachRPV.RefreshReport();
            }
        }
        private void SubChiTietSPProcessing(object sender, SubreportProcessingEventArgs e)
        {
            string manx = e.Parameters["MaNX"].Values[0].ToString().Trim();
            ReportDataSource dts = new ReportDataSource("dsKhachSubCTRP", rpBAL.ChiTietByMaNX(manx));
            e.DataSources.Add(dts);
        }
        private void rdbTCKhach_Click(object sender, EventArgs e)
        {
            rdbTCKhach.Checked = true;
            rdbKhachGD.Checked = false;
            cboMaKhach.Enabled = cboTenKhach.Enabled = false;
        }
        private void rdbKhachGD_Click(object sender, EventArgs e)
        {
            rdbTCKhach.Checked = false;
            rdbKhachGD.Checked = true;
            cboMaKhach.Enabled = cboTenKhach.Enabled = true;
            //Load combobox.
            khBAL.LoadComboboxMaKhach(cboMaKhach);
            khBAL.LoadComboboxTenKhach(cboTenKhach);
        }
        private void cboMaKhach_SelectedValueChanged(object sender, EventArgs e)
        {
            cboTenKhach.Text = cboMaKhach.SelectedValue.ToString().Trim();
        }
        private void cboTenKhach_SelectedValueChanged(object sender, EventArgs e)
        {
            cboMaKhach.Text = cboTenKhach.SelectedValue.ToString().Trim();
        }
        #endregion
    }
}
