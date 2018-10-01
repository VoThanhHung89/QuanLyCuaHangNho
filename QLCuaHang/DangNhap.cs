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
    public partial class DangNhap : Form
    {
        public DangNhap()
        {
            InitializeComponent();
        }
        TaiKhoanBAL tkBal = new TaiKhoanBAL();
        LsTaiKhoanBAL lstkBal = new LsTaiKhoanBAL();
        public static int mals; 
        public static string tendn;
        private void DangNhap_Load(object sender, EventArgs e)
        {
            //Load combobox server.
            cboServer.Items.Add(".");
            cboServer.Items.Add("(local)");
            cboServer.Items.Add(@".\SQLEXPRESS");
            cboServer.Items.Add(string.Format(@"{0}\SQLEXPRESS", Environment.MachineName));
            //Load thông tin nếu có lưu đăng nhập.
            tkBal.LoadThongTinDangNhap(chbLuuDN, txtTen, txtMatKhau, cboServer, txtDatabase);
        }
        private void btnDN_Click(object sender, EventArgs e)
        {
            try { CheckLogin(); }
            catch (Exception ex)
            {
                if (DialogResult.Yes == MessageBox.Show("Đã xảy ra lỗi!\nBạn có muốn xem lỗi?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk))
                    MessageBox.Show(ex.ToString());
            }
        }
        private void btnThoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void btnXoaTT_Click(object sender, EventArgs e)
        {
            txtTen.Text = txtMatKhau.Text = cboServer.Text = txtDatabase.Text = string.Empty;
            chbLuuDN.Checked = false;
        }
        //Sự kiện ấn phím trên Form. (Setting property của Form KeyPreview = True).
        private void DangNhap_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter) CheckLogin(); //Sự kiện ấn nút Enter.
            if(e.KeyCode == Keys.Escape) Application.Exit(); // Sự kiện ấn nút Escape.
        }
        //Kiểm tra đăng nhập.
        private void CheckLogin()
        {   //Kiểm tra kết nối.
            if (tkBal.CheckConnect(cboServer, txtDatabase))
            {
                //Lưu thông tin đăng nhập. Ở Property - Settings của DatAccesslayer hoặc ở App.Config.
                //??????Kiểm tra Database, nếu chưa có thì attach vào.?????????
                string connecttionstring = string.Format("Data Source={0};Initial Catalog={1};Integrated Security=True", 
                                                            cboServer.Text.Trim(), txtDatabase.Text.Trim());
                if (chbLuuDN.Checked) tkBal.LuuThongTinDangNhap(true, txtTen.Text, txtMatKhau.Text, connecttionstring);
                else tkBal.LuuThongTinDangNhap(false, string.Empty, string.Empty, string.Empty);
                //Kiểm tra đăng nhập.
                if (tkBal.Loggin(txtTen.Text.Trim(), txtMatKhau.Text.Trim()))
                {
                    MessageBox.Show("Đăng nhập thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //Tạo thông tin LsTaiKhoan.
                    tendn = txtTen.Text.Trim();
                    lstkBal.AddLsTaiKhoan(tendn);
                    mals = lstkBal.GetMaLsTK(tendn);
                    //Lưu thời gian đăng nhập.
                    lstkBal.EditTimeIn(mals, tendn, DateTime.Now);
                    //Ẩn Form.
                    this.Hide();
                    FormChinh frm = new FormChinh();
                    frm.Show();
                }
                else
                {
                    MessageBox.Show("Đăng nhập thất bại. Tên đăng nhập hoặc Mật khẩu không trùng khớp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    txtTen.Text = txtMatKhau.Text = string.Empty;
                    txtTen.Focus();
                }
            }
        }
    }
}
