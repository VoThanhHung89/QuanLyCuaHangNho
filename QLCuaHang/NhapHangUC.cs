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

namespace QLCuaHang
{
    public partial class NhapHangUC : UserControl
    {
        public NhapHangUC()
        {
            InitializeComponent();
        }
        private static NhapHangUC _instance;
        public static NhapHangUC Instance
        {
            get
            {
                if (_instance == null) _instance = new NhapHangUC();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        NhapXuatBAL nxBAL = new NhapXuatBAL();
        MyMeThod myme = new MyMeThod();
        private void NhapBindingData() {
            lblMaN.Text = lblNgayN.Text = lblMaKhachN.Text = lblTenKhachN.Text = lblSDTN.Text = txtDiaChiN.Text = txtGhiChuN.Text = txtTongTienN.Text = string.Empty;
            nxBAL.LoadGridViewCT(dgvChiTietN, null);
            if (dgvNhap.SelectedRows.Count!=0) {
                string manhap = dgvNhap.CurrentRow.Cells["man"].Value.ToString();
                nxBAL.LoadGridViewCT(dgvChiTietN, manhap);
                lblMaN.Text = manhap;
                lblNgayN.Text = string.Format("{0:d}", dgvNhap.CurrentRow.Cells["ngayn"].Value);
                lblMaKhachN.DataBindings.Clear();
                lblMaKhachN.DataBindings.Add("Text", dgvNhap.DataSource, "makhach");
                lblTenKhachN.DataBindings.Clear();
                lblTenKhachN.DataBindings.Add("Text", dgvNhap.DataSource, "tenkhach");
                lblSDTN.DataBindings.Clear();
                lblSDTN.DataBindings.Add("Text", dgvNhap.DataSource, "sdt");
                txtDiaChiN.DataBindings.Clear();
                txtDiaChiN.DataBindings.Add("Text", dgvNhap.DataSource, "diachi");
                txtTongTienN.Text = string.Format("{0:N0}", dgvNhap.CurrentRow.Cells["tongtienn"].Value);
                txtGhiChuN.Text = dgvNhap.CurrentRow.Cells["ghichun"].Value.ToString();
                dgvChiTietN.ClearSelection();
            }
        }
        private void XuatBindingData() {
            lblMaX.Text = lblNgayX.Text = lblMaKhachX.Text = lblTenKhachX.Text = lblSDTX.Text= txtDiaChiX.Text = txtGhiChuX.Text = txtTongTienX.Text = string.Empty;
            nxBAL.LoadGridViewCT(dgvChiTietX, null);
            if (dgvXuat.SelectedRows.Count != 0) {
                string maxuat = dgvXuat.CurrentRow.Cells["max"].Value.ToString();
                nxBAL.LoadGridViewCT(dgvChiTietX, maxuat);
                lblMaX.Text = maxuat;
                lblNgayX.Text = string.Format("{0:d}", dgvXuat.CurrentRow.Cells["ngayX"].Value);
                lblMaKhachX.DataBindings.Clear();
                lblMaKhachX.DataBindings.Add("Text", dgvXuat.DataSource, "makhach");
                lblTenKhachX.DataBindings.Clear();
                lblTenKhachX.DataBindings.Add("Text", dgvXuat.DataSource, "tenkhach");
                lblSDTX.DataBindings.Clear();
                lblSDTX.DataBindings.Add("Text", dgvXuat.DataSource, "sdt");
                txtDiaChiX.DataBindings.Clear();
                txtDiaChiX.DataBindings.Add("Text", dgvXuat.DataSource, "diachi");
                txtTongTienX.Text = string.Format("{0:N0}", dgvXuat.CurrentRow.Cells["tongtienx"].Value);
                txtGhiChuX.Text = dgvXuat.CurrentRow.Cells["ghichux"].Value.ToString();
                dgvChiTietX.ClearSelection();
            }
        }
        private void ChonDenDong(DataGridView dgv, string giatrima) {
            for(int i=0; i< dgv.RowCount; i++) {
                if (dgv.Rows[i].Cells[0].Value.ToString() == giatrima) //Lấy Cell là 0 vì ở 2 gridview mã hàng ở cell 0.
                    dgv.CurrentCell = dgv.Rows[i].Cells[0];
            }
        }
        private void NhapHangUC_Load(object sender, EventArgs e)
        {
            cboCachTimN.SelectedIndex = cboCachTimX.SelectedIndex = 0;
            nxBAL.LoadGridView(dgvNhap, "N");
            nxBAL.LoadGridView(dgvXuat, "X");
            NhapBindingData();
            XuatBindingData();
        }
        //Tab Nhập Hàng.
        #region Tab Nhập
        private void dgvNhap_SelectionChanged(object sender, EventArgs e)
        {
            NhapBindingData();
        }
        private void btnThemN_Click(object sender, EventArgs e)
        {
            FormNhapXuat.manx = myme.MaTuTang("N", nxBAL.MaCuoiCung("N"), 15);
            FormNhapXuat frm = new FormNhapXuat();
            frm.ShowDialog();
            nxBAL.LoadGridView(dgvNhap, "N");
            ChonDenDong(dgvNhap, FormNhapXuat.manx);
            NhapBindingData();
        }
        private void btnSuaN_Click(object sender, EventArgs e)
        {
            FormNhapXuat.manx = dgvNhap.CurrentRow.Cells["man"].Value.ToString();
            FormNhapXuat frm = new FormNhapXuat();
            frm.ShowDialog();
            nxBAL.LoadGridView(dgvNhap, "N");
            ChonDenDong(dgvNhap, FormNhapXuat.manx);
            NhapBindingData();
        }
        private void btnXoaN_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == MessageBox.Show("Bạn có muốn xóa thông tin trên?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
            { nxBAL.DeleteNX(dgvNhap.CurrentRow.Cells["man"].Value.ToString());
                nxBAL.LoadGridView(dgvNhap, "N");
                NhapBindingData(); 
            }
        }
        private void chbHienTCN_Click(object sender, EventArgs e)
        {
            chbHienTCN.Checked = true;
            chbHienTKN.Checked = false;
            cboCachTimN.SelectedIndex = 0;
            txtTuTimN.Text = string.Empty;
            foreach(DataGridViewRow dgr in dgvNhap.Rows) dgr.Visible = true;
        }
        private void chbHienTKN_Click(object sender, EventArgs e)
        {
            chbHienTCN.Checked = false;
            chbHienTKN.Checked = true;
        }
        private void btnTimN_Click(object sender, EventArgs e)
        {
            if(btnTimN.Text =="CHỌN NGÀY")
            {
                TimNgayNhapXuat.ngay = string.Empty;
                TimNgayNhapXuat frm = new TimNgayNhapXuat();
                frm.ShowDialog();
                txtTuTimN.Text = TimNgayNhapXuat.ngay;
            }
            chbHienTCN.Checked = false;
            chbHienTKN.Checked = true;
            nxBAL.TimKiemPhieu(cboCachTimN.Text.Trim(), txtTuTimN.Text.Trim(), dgvNhap);
            dgvNhap.ClearSelection();
        }
        private void cboCachTimN_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cboCachTimN.Text == "Ngày Lập Phiếu")
            {
                btnTimN.Text = "CHỌN NGÀY";
                TimNgayNhapXuat.ngay = string.Empty;
                TimNgayNhapXuat frm = new TimNgayNhapXuat();
                frm.ShowDialog();
                txtTuTimN.Text = TimNgayNhapXuat.ngay;
                chbHienTCN.Checked = false;
                chbHienTKN.Checked = true;
                nxBAL.TimKiemPhieu(cboCachTimN.Text.Trim(), txtTuTimN.Text.Trim(), dgvNhap);
                dgvNhap.ClearSelection();
            }
            else btnTimN.Text = "TÌM KIẾM";
        }
        private void btnXuatHD_Click(object sender, EventArgs e)
        {
            HoaDon.manx = lblMaN.Text.Trim();
            HoaDon frm = new HoaDon();
            frm.ShowDialog();
        }
        #endregion
        //Tab Xuất Hàng.
        #region Tab Xuất
        private void dgvXuat_SelectionChanged(object sender, EventArgs e)
        {
            XuatBindingData();
        }
        private void btnXoaX_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == MessageBox.Show("Bạn có muốn xóa thông tin trên?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
            {
                nxBAL.DeleteNX(dgvXuat.CurrentRow.Cells["max"].Value.ToString());
                nxBAL.LoadGridView(dgvXuat, "X");
                XuatBindingData();
            }
        }
        private void btnThemX_Click(object sender, EventArgs e)
        {
            FormNhapXuat.manx = myme.MaTuTang("X", nxBAL.MaCuoiCung("X"), 15);
            FormNhapXuat frm = new FormNhapXuat();
            frm.ShowDialog();
            nxBAL.LoadGridView(dgvXuat, "X");
            ChonDenDong(dgvXuat, FormNhapXuat.manx);
            XuatBindingData();
        }
        private void btnSuaX_Click(object sender, EventArgs e)
        {
            FormNhapXuat.manx = dgvXuat.CurrentRow.Cells["max"].Value.ToString();
            FormNhapXuat frm = new FormNhapXuat();
            frm.ShowDialog();
            nxBAL.LoadGridView(dgvXuat, "X");
            ChonDenDong(dgvXuat, FormNhapXuat.manx);
            XuatBindingData();
        }
        private void chbHienTCX_Click(object sender, EventArgs e)
        {
            chbHienTCX.Checked = true;
            chbHienTKX.Checked = false; 
            cboCachTimX.SelectedIndex = 0;
            txtTuTimX.Text = string.Empty;
            foreach (DataGridViewRow dgr in dgvXuat.Rows) dgr.Visible = true;
        }
        private void chbHienTKX_Click(object sender, EventArgs e)
        {
            chbHienTCX.Checked = false;
            chbHienTKX.Checked = true;
        }
        private void cboCachTimX_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cboCachTimX.Text == "Ngày Lập Phiếu")
            {
                btnTimX.Text = "CHỌN NGÀY";
                TimNgayNhapXuat.ngay = string.Empty;
                TimNgayNhapXuat frm = new TimNgayNhapXuat();
                frm.ShowDialog();
                txtTuTimX.Text = TimNgayNhapXuat.ngay;
                chbHienTCX.Checked = false;
                chbHienTKX.Checked = true;
                nxBAL.TimKiemPhieu(cboCachTimX.Text.Trim(), txtTuTimX.Text.Trim(), dgvXuat);
                dgvXuat.ClearSelection();
            }
            else btnTimX.Text = "TÌM KIẾM";
        }
        private void btnTimX_Click(object sender, EventArgs e)
        {
            if (btnTimX.Text == "CHỌN NGÀY")
            {
                TimNgayNhapXuat.ngay = string.Empty;
                TimNgayNhapXuat frm = new TimNgayNhapXuat();
                frm.ShowDialog();
                txtTuTimX.Text = TimNgayNhapXuat.ngay;
            }
            chbHienTCX.Checked = false;
            chbHienTKX.Checked = true;
            nxBAL.TimKiemPhieu(cboCachTimX.Text.Trim(), txtTuTimX.Text.Trim(), dgvXuat);
            dgvXuat.ClearSelection();
        }
        private void btnXuatHDX_Click(object sender, EventArgs e)
        {
            HoaDon.manx = lblMaX.Text;
            HoaDon frm = new HoaDon();
            frm.ShowDialog();
        }
        #endregion
        //Tab Report.
        #region Report
        //Checkbox
        private void chbNhapAll_Click(object sender, EventArgs e)
        {
            chbNhapAll.Checked = true;
            chbXuatAll.Checked = false;
        }
        private void chbXuatAll_Click(object sender, EventArgs e)
        {
            chbNhapAll.Checked = false;
            chbXuatAll.Checked = true;
        }
        private void chbNhapTheoMa_Click(object sender, EventArgs e)
        {
            chbNhapTheoMa.Checked = true;
            nxBAL.LoadComboboxMaPhieu(cboMaPhieu, "N");
            chbXuatTheoMa.Checked = false;
        }
        private void chbXuatTheoMa_Click(object sender, EventArgs e)
        {
            chbNhapTheoMa.Checked = false;
            chbXuatTheoMa.Checked = true;
            nxBAL.LoadComboboxMaPhieu(cboMaPhieu, "X");
        }
        private void chbNhapTheoTG_Click(object sender, EventArgs e)
        {
            chbNhapTheoTG.Checked = true;
            chbXuatTheoTG.Checked = false;
        }
        private void chbXuatTheoTG_Click(object sender, EventArgs e)
        {
            chbNhapTheoTG.Checked = false;
            chbXuatTheoTG.Checked = true;
        }
        //Radiobutton
        private void rdbAll_Click(object sender, EventArgs e)
        {
            rdbAll.Checked = btnReportAll.Enabled = true;
            rdbTheoLoai.Checked = chbNhapAll.Enabled = chbXuatAll.Enabled = btnReportTheoLoai.Enabled = false;
            rdbTheoMa.Checked = cboMaPhieu.Enabled = btnReportTheoMa.Enabled = chbNhapTheoMa.Enabled = chbXuatTheoMa.Enabled = false;
            rdbTCTheoTG.Checked = dtpNgayBDAll.Enabled = dtpNgayKTAll.Enabled = btnReportTCTheoTG.Enabled = false;
            rdbLoaiTheoTG.Checked = chbNhapTheoTG.Enabled = chbXuatTheoTG.Enabled = dtpNgayBDTheoTG.Enabled = dtpNgayKTTheoTG.Enabled = btnReportLoaiTheoTG.Enabled = false;
        }
        private void rdbTheoLoai_Click(object sender, EventArgs e)
        {
            rdbAll.Checked = btnReportAll.Enabled = false;
            rdbTheoLoai.Checked = chbNhapAll.Enabled = chbXuatAll.Enabled = btnReportTheoLoai.Enabled = true;
            rdbTheoMa.Checked = cboMaPhieu.Enabled = btnReportTheoMa.Enabled = chbNhapTheoMa.Enabled = chbXuatTheoMa.Enabled = false;
            rdbTCTheoTG.Checked = dtpNgayBDAll.Enabled = dtpNgayKTAll.Enabled = btnReportTCTheoTG.Enabled = false;
            rdbLoaiTheoTG.Checked = chbNhapTheoTG.Enabled = chbXuatTheoTG.Enabled = dtpNgayBDTheoTG.Enabled = dtpNgayKTTheoTG.Enabled = btnReportLoaiTheoTG.Enabled = false;
        }
        private void rdbTheoMa_Click(object sender, EventArgs e)
        {
            rdbAll.Checked = btnReportAll.Enabled = false;
            rdbTheoLoai.Checked = chbNhapAll.Enabled = chbXuatAll.Enabled = btnReportTheoLoai.Enabled = false;
            rdbTheoMa.Checked = cboMaPhieu.Enabled = btnReportTheoMa.Enabled = chbNhapTheoMa.Enabled = chbXuatTheoMa.Enabled = true;
            rdbTCTheoTG.Checked = dtpNgayBDAll.Enabled = dtpNgayKTAll.Enabled = btnReportTCTheoTG.Enabled = false;
            rdbLoaiTheoTG.Checked = chbNhapTheoTG.Enabled = chbXuatTheoTG.Enabled = dtpNgayBDTheoTG.Enabled = dtpNgayKTTheoTG.Enabled = btnReportLoaiTheoTG.Enabled = false;
            if(chbNhapTheoMa.Checked) nxBAL.LoadComboboxMaPhieu(cboMaPhieu, "N");
            else nxBAL.LoadComboboxMaPhieu(cboMaPhieu, "X");
        }
        private void rdbTCTheoTG_Click(object sender, EventArgs e)
        {
            rdbAll.Checked = btnReportAll.Enabled = false;
            rdbTheoLoai.Checked = chbNhapAll.Enabled = chbXuatAll.Enabled = btnReportTheoLoai.Enabled = false;
            rdbTheoMa.Checked = cboMaPhieu.Enabled = btnReportTheoMa.Enabled = chbNhapTheoMa.Enabled = chbXuatTheoMa.Enabled = false;
            rdbTCTheoTG.Checked = dtpNgayBDAll.Enabled = dtpNgayKTAll.Enabled = btnReportTCTheoTG.Enabled = true;
            rdbLoaiTheoTG.Checked = chbNhapTheoTG.Enabled = chbXuatTheoTG.Enabled = dtpNgayBDTheoTG.Enabled = dtpNgayKTTheoTG.Enabled = btnReportLoaiTheoTG.Enabled = false;
        }
        private void rdbLoaiTheoTG_Click(object sender, EventArgs e)
        {
            rdbAll.Checked = btnReportAll.Enabled = false;
            rdbTheoLoai.Checked = chbNhapAll.Enabled = chbXuatAll.Enabled = btnReportTheoLoai.Enabled = false;
            rdbTheoMa.Checked = cboMaPhieu.Enabled = btnReportTheoMa.Enabled = chbNhapTheoMa.Enabled = chbXuatTheoMa.Enabled = false;
            rdbTCTheoTG.Checked = dtpNgayBDAll.Enabled = dtpNgayKTAll.Enabled = btnReportTCTheoTG.Enabled = false;
            rdbLoaiTheoTG.Checked = chbNhapTheoTG.Enabled = chbXuatTheoTG.Enabled = dtpNgayBDTheoTG.Enabled = dtpNgayKTTheoTG.Enabled = btnReportLoaiTheoTG.Enabled = true;
        }
        //Button
        private void btnReportAll_Click(object sender, EventArgs e)
        {
            FormNhapXuatReport.LoaiReport = "A";
            FormNhapXuatReport.NgayBD = FormNhapXuatReport.NgayKT = null;
            FormNhapXuatReport frm = new FormNhapXuatReport();
            frm.Show();
        }
        private void btnReportTheoLoai_Click(object sender, EventArgs e)
        {
            if (chbNhapAll.Checked) FormNhapXuatReport.LoaiReport = "N";
            if (chbXuatAll.Checked) FormNhapXuatReport.LoaiReport = "X";
            FormNhapXuatReport.NgayBD = FormNhapXuatReport.NgayKT = null;
            FormNhapXuatReport frm = new FormNhapXuatReport();
            frm.Show();
        }
        private void btnReportTheoMa_Click(object sender, EventArgs e)
        {
            FormNhapXuatReport.LoaiReport = cboMaPhieu.Text.Trim();
            FormNhapXuatReport.NgayBD = FormNhapXuatReport.NgayKT = null;
            FormNhapXuatReport frm = new FormNhapXuatReport();
            frm.Show();
        }
        private void btnReportTCTheoTG_Click(object sender, EventArgs e)
        {
            FormNhapXuatReport.LoaiReport = "A";
            FormNhapXuatReport.NgayBD = dtpNgayBDAll.Value;
            FormNhapXuatReport.NgayKT = dtpNgayKTAll.Value;
            FormNhapXuatReport frm = new FormNhapXuatReport();
            frm.Show();
        }
        private void btnReportLoaiTheoTG_Click(object sender, EventArgs e)
        {
            if (chbNhapTheoTG.Checked) FormNhapXuatReport.LoaiReport = "N";
            if (chbXuatTheoTG.Checked) FormNhapXuatReport.LoaiReport = "X";
            FormNhapXuatReport.NgayBD = dtpNgayBDTheoTG.Value;
            FormNhapXuatReport.NgayKT = dtpNgayKTTheoTG.Value;
            FormNhapXuatReport frm = new FormNhapXuatReport();
            frm.Show();
        }
        #endregion
    }
}
