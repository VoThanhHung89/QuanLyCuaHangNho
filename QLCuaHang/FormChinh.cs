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
    public partial class FormChinh : Form
    { 
        public FormChinh()
        {
            InitializeComponent();
        }
        private void FormChinh_Load(object sender, EventArgs e)
        {

        }
        //Đóng Form.
        private void FormChinh_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Sự kiện lưu thông tin thời gian out Tài Khoản.
            LsTaiKhoanBAL lstkBal = new LsTaiKhoanBAL();
            lstkBal.EditTimeOut(DangNhap.mals, DangNhap.tendn, DateTime.Now);
            pnlUserControl.Controls.Clear();
            KhachHangUC.Instance = null;
            TaiKhoanUC.Instance = null;
            HangHoaUC.Instance = null;
            NhapHangUC.Instance = null;
            DangNhap dn = new DangNhap();
            dn.Show();
        }
        //Khách.
        private void btnKhach_Click(object sender, EventArgs e)
        {
            if (!pnlUserControl.Contains(KhachHangUC.Instance))
                pnlUserControl.Controls.Add(KhachHangUC.Instance);
            KhachHangUC.Instance.BringToFront();
            //Đổi màu button vừa ấn.
            btnKhach.BackColor = Color.Gray;
            btnHang.BackColor = Color.Transparent;
            btnGiaoDich.BackColor = Color.Transparent;
            btnTaiKhoan.BackColor = Color.Transparent;
        }
        //Hàng hóa.
        private void btnHang_Click(object sender, EventArgs e)
        {
            if (!pnlUserControl.Contains(HangHoaUC.Instance))
                pnlUserControl.Controls.Add(HangHoaUC.Instance);
            HangHoaUC.Instance.BringToFront();
            //Đổi màu button vừa ấn.
            btnKhach.BackColor = Color.Transparent;
            btnHang.BackColor = Color.Gray;
            btnGiaoDich.BackColor = Color.Transparent;
            btnTaiKhoan.BackColor = Color.Transparent;
        }
        //Nhập xuất.
        private void btnGiaoDich_Click(object sender, EventArgs e)
        {
            if (!pnlUserControl.Contains(NhapHangUC.Instance))
                pnlUserControl.Controls.Add(NhapHangUC.Instance);
            NhapHangUC.Instance.BringToFront();
            //Đổi màu button vừa ấn.
            btnKhach.BackColor = Color.Transparent;
            btnHang.BackColor = Color.Transparent;
            btnGiaoDich.BackColor = Color.Gray;
            btnTaiKhoan.BackColor = Color.Transparent;
        }
        //Tài Khoản.
        private void btnTaiKhoan_Click(object sender, EventArgs e)
        {
            if (!pnlUserControl.Contains(TaiKhoanUC.Instance))
                pnlUserControl.Controls.Add(TaiKhoanUC.Instance);
            TaiKhoanUC.Instance.BringToFront();
            //Đổi màu button vừa ấn.
            btnKhach.BackColor = Color.Transparent;
            btnHang.BackColor = Color.Transparent;
            btnGiaoDich.BackColor = Color.Transparent;
            btnTaiKhoan.BackColor = Color.Gray;
        }
        //Escape để đóng Form.
        private void FormChinh_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if(DialogResult.Yes == MessageBox.Show("Bạn có chắc muốn đóng ứng dụng?","Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    this.Close();
                }
            }
        }
    }
}
