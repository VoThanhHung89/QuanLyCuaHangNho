using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataAccessLayer;

namespace BusinessAccessLayer
{
    public class TaiKhoanBAL
    {
        MyMeThod myme = new MyMeThod();
        LoginData log = new LoginData();
        public List<TaiKhoan> GetAll()
        {
            try
            {
                using (QLCuaHangDataContext cuahang = new QLCuaHangDataContext())
                {
                    return cuahang.TaiKhoans.ToList();
                }
            }
            catch (Exception ex) { myme.ShowError(ex); return null; }
        }
        public void LoadGirdview(DataGridView dgv)
        {
            dgv.DataSource = GetAll();
        }
        public bool Loggin(string ten, string mk)
        {
            try
            {
                using (QLCuaHangDataContext cuahang = new QLCuaHangDataContext())
                {
                    TaiKhoan tk = cuahang.TaiKhoans.SingleOrDefault(tks => tks.TenDangNhap == ten && tks.MatKhau == EncryptPass(mk));
                    if (tk != null) return true;
                    else return false;
                }
            }
            catch (Exception ex) { myme.ShowError(ex); return false; }
        }
        public bool CheckTen(string ten)
        {
            try
            {
                using (QLCuaHangDataContext cuahang = new QLCuaHangDataContext())
                {
                    TaiKhoan tk = cuahang.TaiKhoans.SingleOrDefault(tks => tks.TenDangNhap == ten);
                    if (tk != null) return true;
                    else return false;
                }
            }
            catch (Exception ex) { myme.ShowError(ex); return false; }
        }
        public void AddOrEditTaiKhoan(string ten, string mk)
        {
            try
            {
                using (QLCuaHangDataContext cuahang = new QLCuaHangDataContext())
                {
                    if (!CheckTen(ten))// Thêm mới Tài Khoản
                        cuahang.SP_ThemTK(ten, EncryptPass(mk));
                    else//Sửa tài khoản.
                        cuahang.SP_SuaTK(ten, EncryptPass(mk));
                    cuahang.SubmitChanges();
                    MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo");
                }
            }
            catch (Exception ex) { myme.ShowError(ex); }
        }
        public void DeleteTaiKhoan(string ten)
        {
            try
            {
                using (QLCuaHangDataContext cuahang = new QLCuaHangDataContext())
                {
                    cuahang.SP_XoaTK(ten);
                    cuahang.SubmitChanges();
                    MessageBox.Show("Xoá thông tin thành công!", "Thông báo");
                }
            }
            catch (Exception ex) { myme.ShowError(ex); }
        }
        public string EncryptPass(string pass)
        {
            try
            {
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(pass));
                byte[] result = md5.Hash;
                StringBuilder str = new StringBuilder();
                for (int i = 1; i < result.Length; i++)
                {
                    str.Append(result[i].ToString("x2"));
                }
                return str.ToString();
            }
            catch (Exception ex) { myme.ShowError(ex); return string.Empty; }
        }
        public void LoadThongTinDangNhap(CheckBox chb, TextBox user, TextBox password, ComboBox cboServer, TextBox txtData)
        {
            try
            {
                if (log.CheckLogin)//Trường hợp có lưu.
                {
                    chb.Checked = log.CheckLogin;
                    user.Text = log.User;
                    password.Text = log.Password;
                    string connecttionstring = log.ConnecttionString;
                    string[] chuoiconnect = connecttionstring.Split(';');
                    string[] chuoiserver = chuoiconnect[0].Split('=');
                    cboServer.Text = chuoiserver[1];
                    string[] chuoidata = chuoiconnect[1].Split('=');
                    txtData.Text = chuoidata[1];
                }
                else
                {
                    chb.Checked = log.CheckLogin;
                    user.Text = password.Text = cboServer.Text = txtData.Text = string.Empty;
                }
            }
            catch (Exception ex) { myme.ShowError(ex); }
        }
        public bool CheckConnect(ComboBox cboServer, TextBox txtData) {
            try {
                string connectionstring = string.Format("Data Source={0};Initial Catalog={1};Integrated Security=True;", cboServer.Text.Trim(), txtData.Text.Trim());
                if (log.CheckConnectData(connectionstring)) return true;
                else {
                    MessageBox.Show("Kết nối với Dữ Liệu thất bại! Bạn hãy kiểm tra lại Server và Database!", "Thông báo");
                    return false;
                }
            }catch (Exception ex) { myme.ShowError(ex); return false; }
        }
        public void LuuThongTinDangNhap(bool check, string user, string pass, string connectionstring) {
            try {
                if(check) {
                    log.CheckLogin = true;
                    log.User = user;
                    log.Password = pass;
                    log.SaveConnectionString(connectionstring);
                }
                else {
                    log.CheckLogin = false;
                    log.User = log.Password = string.Empty;
                    log.SaveConnectionString(connectionstring);
                }
            }catch (Exception ex) { myme.ShowError(ex); }
        }

    }
}
