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
    public partial class HangHoaUC : UserControl
    {
        public HangHoaUC()
        {
            InitializeComponent();
        }
        private static HangHoaUC _instance;

        public static HangHoaUC Instance
        {
            get {
                if (_instance == null) _instance = new HangHoaUC();
                return _instance;
            }

            set {
                _instance = value;
            }
        }
        HangHoaBAL hhBAL = new HangHoaBAL();
        ReportBAL rpBAL = new ReportBAL();
        MyMeThod myme = new MyMeThod();
        private void BlindingData() {
            txtMa.Text = txtTen.Text = txtDVT.Text = txtGhiChu.Text = string.Empty;
            nmrSL.Value = nmrGN.Value = nmrGX.Value = 0;
            txtMa.DataBindings.Clear();
            txtTen.DataBindings.Clear();
            nmrSL.DataBindings.Clear();
            txtDVT.DataBindings.Clear();
            nmrGN.DataBindings.Clear();
            nmrGX.DataBindings.Clear();
            txtGhiChu.DataBindings.Clear();
            txtMa.DataBindings.Add("Text", dgvHangHoa.DataSource, "mahang");
            txtTen.DataBindings.Add("Text", dgvHangHoa.DataSource, "tenhang");
            nmrSL.DataBindings.Add("Value", dgvHangHoa.DataSource, "soluong");
            txtDVT.DataBindings.Add("Text", dgvHangHoa.DataSource, "donvitinh");
            nmrGN.DataBindings.Add("Value", dgvHangHoa.DataSource, "dongianhap"); 
            nmrGX.DataBindings.Add("Value", dgvHangHoa.DataSource, "dongiaxuat");
            txtGhiChu.DataBindings.Add("Text", dgvHangHoa.DataSource, "ghichu");
        }
        private void ManagerControl(string trangthai) {
            if(trangthai == "binhthuong")
            {
                dgvHangHoa.Enabled = true;
                txtTen.ReadOnly = nmrSL.ReadOnly = txtDVT.ReadOnly = nmrGN.ReadOnly = nmrGX.ReadOnly = txtGhiChu.ReadOnly = true;
                nmrSL.Increment = nmrGN.Increment = nmrGX.Increment = 0;
                btnThem.Visible = btnSua.Visible = true;
                btnLuu.Enabled = false;
                btnXoa.Text = "XÓA";
            }
            if (trangthai == "them")
            {
                dgvHangHoa.Enabled = false;
                txtTen.ReadOnly = nmrSL.ReadOnly = txtDVT.ReadOnly = nmrGN.ReadOnly = nmrGX.ReadOnly = txtGhiChu.ReadOnly = false;
                nmrSL.Increment = 1;
                nmrGN.Increment = nmrGX.Increment = 100;
                txtTen.Text = txtDVT.Text = txtGhiChu.Text = string.Empty;
                nmrSL.Value = nmrGN.Value = nmrGX.Value = 0;
                txtMa.Text = myme.MaTuTang("H", hhBAL.MaCuoiCung(), 10);
                btnThem.Visible = btnSua.Visible = false;
                btnLuu.Enabled = true;
                btnXoa.Text = "HỦY";
            }
            if (trangthai == "sua")
            {
                dgvHangHoa.Enabled = false;
                txtTen.ReadOnly = nmrSL.ReadOnly = txtDVT.ReadOnly = nmrGN.ReadOnly = nmrGX.ReadOnly = txtGhiChu.ReadOnly = false;
                nmrSL.Increment = 1;
                nmrGN.Increment = nmrGX.Increment = 100;
                btnThem.Visible = btnSua.Visible = false;
                btnLuu.Enabled = true;
                btnXoa.Text = "HỦY";
            }
        }
        private void HangHoaUC_Load(object sender, EventArgs e)
        {
            hhBAL.LoadGridView(dgvHangHoa);
            BlindingData();
            cboCachTim.SelectedIndex = 0;
            ManagerControl("binhthuong");
            chbHienTC.Checked = true;
            chbHienTK.Checked = false;
        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            ManagerControl("them");
            nmrSL.DataBindings.Clear();
            nmrGN.DataBindings.Clear();
            nmrGX.DataBindings.Clear();
        }
        private void btnSua_Click(object sender, EventArgs e)
        {
            ManagerControl("sua");
            nmrSL.DataBindings.Clear();
            nmrGN.DataBindings.Clear();
            nmrGX.DataBindings.Clear();
        }
        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (txtTen.Text != string.Empty)
            {
                string mahang = txtMa.Text.Trim();
                hhBAL.AddOrEditHang(mahang, txtTen.Text.Trim(), Convert.ToInt32(nmrSL.Value), txtDVT.Text.Trim(), Convert.ToDouble(nmrGN.Value), 
                                                Convert.ToDouble(nmrGX.Value), txtGhiChu.Text.Trim());
                hhBAL.LoadGridView(dgvHangHoa);
                BlindingData();
                ManagerControl("binhthuong");
                //Chuyển đến dòng vừa thực thi.
                foreach (DataGridViewRow dgr in dgvHangHoa.Rows)
                {
                    if (dgr.Cells["makhach"].Value.ToString() == mahang)
                    {
                        dgr.Selected = true;
                    }
                    else
                        dgr.Selected = false;
                }
            }
        }
        private void btnXoa_Click(object sender, EventArgs e)
        {
            int thutuhang = 0;
            if(btnXoa.Text == "XÓA" && txtMa.Text != string.Empty) {
                if (DialogResult.OK == MessageBox.Show("Bạn muốn xoá thông tin trên?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
                {
                    thutuhang = dgvHangHoa.CurrentRow.Index;
                    hhBAL.DeleteHang(txtMa.Text.Trim());
                }
            }
            ManagerControl("binhthuong");
            hhBAL.LoadGridView(dgvHangHoa);
            BlindingData();
            if (thutuhang > 1)
            {
                dgvHangHoa.ClearSelection();
                dgvHangHoa.Rows[thutuhang - 1].Selected = true;
            }
        }
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            chbHienTK.Checked = true;
            chbHienTC.Checked = false;
            hhBAL.TimKiemHangHoa(cboCachTim.Text.Trim(), txtTuTim.Text.Trim(), dgvHangHoa);
            if (dgvHangHoa.RowCount == 0) MessageBox.Show("Không có kết quả nào trùng khớp với dữ kiện tìm kiếm.", "Thông báo");
            BlindingData();
        }
        private void chbHienTC_Click(object sender, EventArgs e)
        {
            chbHienTC.Checked = true;
            chbHienTK.Checked = false;
            txtTuTim.Text = string.Empty;
            cboCachTim.SelectedIndex = 0;
            hhBAL.LoadGridView(dgvHangHoa);
            BlindingData();
        }
        private void chbHienTK_Click(object sender, EventArgs e)
        {
            chbHienTK.Checked = true;
            chbHienTC.Checked = false;
        }
        #region Report
        private void rdbHangAll_Click(object sender, EventArgs e)
        {
            rdbHangAll.Checked = true;
            rdbHangNX.Checked = false;
            cboMaHang.Enabled = cboTenHang.Enabled = chbTG.Enabled = chbTG.Checked = false;
        }
        private void rdbHangNX_Click(object sender, EventArgs e)
        {
            rdbHangAll.Checked = false;
            rdbHangNX.Checked = true;
            cboMaHang.Enabled = cboTenHang.Enabled = chbTG.Enabled = true;
            hhBAL.LoadComboboxHang(cboMaHang, cboTenHang);
        }
        private void cboMaHang_SelectedValueChanged(object sender, EventArgs e)
        {
            cboTenHang.Text = cboMaHang.SelectedValue.ToString().Trim();
        }
        private void cboTenHang_SelectedValueChanged(object sender, EventArgs e)
        {
            cboMaHang.Text = cboTenHang.SelectedValue.ToString().Trim();
        }
        private void chbTG_CheckedChanged(object sender, EventArgs e)
        {
            if (chbTG.Checked) dtpBD.Enabled = dtpKT.Enabled = true;
            else dtpBD.Enabled = dtpKT.Enabled = false;
        }
        private void btnReport_Click(object sender, EventArgs e)
        {
            if (rdbHangAll.Checked)
            {
                HangRPV.Reset();
                HangRPV.LocalReport.ReportEmbeddedResource = "QLCuaHang.HangAllRP.rdlc";
                ReportDataSource rps = new ReportDataSource("dsHangAll", rpBAL.HangAll());
                HangRPV.LocalReport.DataSources.Add(rps);
                HangRPV.RefreshReport();
            }
            if(rdbHangNX.Checked && chbTG.Checked == false) //Không có thời gian.
            {
                HangRPV.Reset();
                HangRPV.LocalReport.ReportEmbeddedResource = "QLCuaHang.HangNXRP.rdlc";
                ReportDataSource rps = new ReportDataSource("dsHangNX", rpBAL.HangNX(cboMaHang.Text.Trim(), null, null));
                HangRPV.LocalReport.DataSources.Add(rps);
                HangRPV.RefreshReport();
            }
            if (rdbHangNX.Checked && chbTG.Checked) //Có thời gian.
            {
                HangRPV.Reset();
                HangRPV.LocalReport.ReportEmbeddedResource = "QLCuaHang.HangNXTheoNgayRP.rdlc";
                ReportDataSource rps = new ReportDataSource("dsHangNXTheoNgayRP", rpBAL.HangNX(cboMaHang.Text.Trim(), dtpBD.Value.ToString("MM/dd/yyyy"), dtpKT.Value.ToString("MM/dd/yyyy")));
                ReportParameter[] rpp = new ReportParameter[]
                {
                    new ReportParameter("paraNgayBD", dtpBD.Value.ToString("dd/MM/yyyy")),
                    new ReportParameter("paraNgayKT", dtpKT.Value.ToString("dd/MM/yyyy"))
                };
                HangRPV.LocalReport.SetParameters(rpp);
                HangRPV.LocalReport.DataSources.Add(rps);
                HangRPV.RefreshReport();
            }
        }
        #endregion
    }
}
