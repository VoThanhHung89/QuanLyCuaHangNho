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
    public partial class TaiKhoanUC : UserControl
    {
        public TaiKhoanUC()
        {
            InitializeComponent();
        }
        private static TaiKhoanUC _inStance;
        public static TaiKhoanUC Instance
        {
            get
            {
                if (_inStance == null)
                    _inStance = new TaiKhoanUC();
                return _inStance;
            }
            set
            {
                _inStance = value;
            }
        }
        TaiKhoanBAL tkBal = new TaiKhoanBAL();
        LsTaiKhoanBAL lstkBal = new LsTaiKhoanBAL();
        private void BlindingData()
        {
            txtTen.Text = txtMatKhau.Text = txtMatKhau2.Text = string.Empty;
            txtTen.DataBindings.Clear();
            txtTen.DataBindings.Add("Text", dgvTaiKhoan.DataSource, "tendangnhap");
            txtMatKhau.DataBindings.Clear();
            txtMatKhau.DataBindings.Add("Text", dgvTaiKhoan.DataSource, "matkhau");
            txtMatKhau2.DataBindings.Clear();
            txtMatKhau2.DataBindings.Add("Text", dgvTaiKhoan.DataSource, "matkhau");
            HidePassGridView();
            //Load lịch sử đăng nhập.
            lblTenDN.DataBindings.Clear();
            lblTenDN.DataBindings.Add("Text", dgvTaiKhoan.DataSource, "tendangnhap");
            lstkBal.LoadGridView(dgvLS, txtTen.Text.Trim());
        }
        private void ManagerControl(string trangthai) {
            if(trangthai =="binhthuong") {
                txtTen.ReadOnly = txtMatKhau.ReadOnly = txtMatKhau2.ReadOnly = true;
                btnThem.Visible = btnSua.Visible = true;
                btnLuu.Enabled = false;
                btnXoa.Text = "XÓA";
                chbXemMK.Visible = false;
            }
            if(trangthai =="them") {
                txtTen.ReadOnly = txtMatKhau.ReadOnly = txtMatKhau2.ReadOnly = false;
                btnThem.Visible = btnSua.Visible = false;
                btnLuu.Enabled = true;
                btnXoa.Text = "HỦY";
                chbXemMK.Visible = true;
                txtMatKhau.UseSystemPasswordChar = txtMatKhau2.UseSystemPasswordChar = true;
                txtTen.Text = txtMatKhau.Text = txtMatKhau2.Text = string.Empty;
            }
            if(trangthai =="sua") {
                txtMatKhau.ReadOnly = txtMatKhau2.ReadOnly = false;
                btnThem.Visible = btnSua.Visible = false;
                btnLuu.Enabled = true;
                btnXoa.Text = "HỦY";
                chbXemMK.Visible = true;
                txtMatKhau.UseSystemPasswordChar = txtMatKhau2.UseSystemPasswordChar = true;
                txtMatKhau.Text = txtMatKhau2.Text = string.Empty;
            }
        }
        private void HidePassGridView()
        {
            for (int i = 0; i < dgvTaiKhoan.RowCount; i++)
            {
                string pass = dgvTaiKhoan.Rows[i].Cells["matkhau"].Value.ToString().Trim();
                dgvTaiKhoan.Rows[i].Cells["matkhau"].Value = new string('*', pass.Length);
            }
        }
        private void TaiKhoanUC_Load(object sender, EventArgs e)
        {
            tkBal.LoadGirdview(dgvTaiKhoan);
            BlindingData();
            ManagerControl("binhthuong");
        }
        private void dgvTaiKhoan_SelectionChanged(object sender, EventArgs e)
        {
            lstkBal.LoadGridView(dgvLS, txtTen.Text.Trim());
        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            if (DangNhap.tendn.Trim() == "admin")
            {
                ManagerControl("them");
            }
            else MessageBox.Show("Chỉ có ADMIN mới có quyền thêm tài khoản mới.", "Thông báo");
        }
        private void btnSua_Click(object sender, EventArgs e)
        {
            if (DangNhap.tendn == txtTen.Text.Trim() || DangNhap.tendn == "admin")
            {
                ManagerControl("sua");
            }
            else MessageBox.Show("Bạn không có quyền chỉnh sửa mật khẩu của tài khoản này.", "Thông báo");
        }
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (btnXoa.Text == "XOÁ")//Trường hợp xoá tài khoản.
            {
                if (DangNhap.tendn.Trim() == "admin" && txtTen.Text.Trim() != "admin")
                {
                    if (DialogResult.OK == MessageBox.Show("Bạn muốn xoá thông tin trên?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
                    {
                        tkBal.DeleteTaiKhoan(txtTen.Text.Trim());
                    }
                }
                else MessageBox.Show("Chỉ có ADMIN mới có quyền xoá tài khoản.", "Thông báo");
            }
            tkBal.LoadGirdview(dgvTaiKhoan);
            BlindingData();
            ManagerControl("binhthuong");
        }
        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtTen.Text) && !string.IsNullOrEmpty(txtMatKhau.Text) && !string.IsNullOrEmpty(txtMatKhau2.Text))
            { //Không trống dữ liệu.
                if (txtMatKhau.Text == txtMatKhau2.Text)//Mật mã nhập lại phải trùng.
                    tkBal.AddOrEditTaiKhoan(txtTen.Text.Trim(), txtMatKhau.Text.Trim());
                else MessageBox.Show("Không thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                MessageBox.Show("Không thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            tkBal.LoadGirdview(dgvTaiKhoan);
            BlindingData();
            ManagerControl("binhthuong");
        }
        private void chbXemMK_CheckedChanged(object sender, EventArgs e)
        {
            if (chbXemMK.Checked)
            {
                txtMatKhau.UseSystemPasswordChar = false;
                txtMatKhau2.UseSystemPasswordChar = false;
            }
            else
            {
                txtMatKhau.UseSystemPasswordChar = true;
                txtMatKhau2.UseSystemPasswordChar = true;
            }
        }
    }
}
