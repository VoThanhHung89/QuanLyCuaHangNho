using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataAccessLayer;

namespace BusinessAccessLayer
{
    public class LsTaiKhoanBAL
    {
        MyMeThod myme = new MyMeThod();
        public List<LsTaiKhoan> GetLsTaiKhoan(string tendn)
        {
            try
            {
                using (QLCuaHangDataContext cuahang = new QLCuaHangDataContext())
                {
                    return cuahang.LsTaiKhoans.Where(ls => ls.TenDangNhap == tendn).ToList();
                }
            }
            catch (Exception ex) { myme.ShowError(ex); return null; }
        }
        public void LoadGridView(DataGridView dgv, string tendn)
        {
            try
            {
                using (QLCuaHangDataContext cuahang = new QLCuaHangDataContext())
                {
                    var lstk = from ls in cuahang.LsTaiKhoans
                               where ls.TenDangNhap == tendn
                               orderby ls.MaLS descending
                               select new { SignIn = ls.SignIn, SignOut = ls.SignOut };
                    dgv.DataSource = lstk;
                }
            }
            catch (Exception ex) { myme.ShowError(ex); }
        }
        public LsTaiKhoan DetailLsTK(int mals, string tendn)
        {
            LsTaiKhoan lstk = new LsTaiKhoan();
            foreach (LsTaiKhoan ls in GetLsTaiKhoan(tendn))
            {
                if (ls.MaLS == mals)
                {
                    lstk = ls;
                }
            }
            return lstk;
        }
        /// <summary>
        /// Lấy mã để tiến hành thêm thông tin thời gian đăng nhập.
        /// </summary>
        /// <param name="tendn"></param>
        /// <returns></returns>
        public int GetMaLsTK(string tendn)
        {
            int mals = 0;
            foreach (LsTaiKhoan ls in GetLsTaiKhoan(tendn))
            {
                if (ls.SignIn == null && ls.SignOut == null) mals = ls.MaLS;
            }
            return mals;
        }
        //Do mã tự tăng nên không cần thêm thông tin MaLSTaiKhoan.
        public void AddLsTaiKhoan(string tendn)
        {
            try
            {
                using (QLCuaHangDataContext cuahang = new QLCuaHangDataContext())
                {
                    LsTaiKhoan lstk = new LsTaiKhoan() { TenDangNhap = tendn, SignIn = null, SignOut = null };
                    cuahang.LsTaiKhoans.InsertOnSubmit(lstk);
                    cuahang.SubmitChanges();
                }
            }
            catch (Exception ex) { myme.ShowError(ex); }
        }
        public void EditTimeIn(int mals, string tendn, DateTime intime)
        {
            try
            {
                using (QLCuaHangDataContext cuahang = new QLCuaHangDataContext())
                {
                    LsTaiKhoan lstk = cuahang.LsTaiKhoans.Single(ls => ls.MaLS == mals && ls.TenDangNhap == tendn);
                    lstk.SignIn = intime;
                    cuahang.SubmitChanges();
                }
            }
            catch (Exception ex) { myme.ShowError(ex); }
        }
        public void EditTimeOut(int mals, string tendn, DateTime outtime)
        {
            try
            {
                using (QLCuaHangDataContext cuahang = new QLCuaHangDataContext())
                {
                    LsTaiKhoan lstk = cuahang.LsTaiKhoans.Single(ls => ls.MaLS == mals && ls.TenDangNhap == tendn);
                    lstk.SignOut = outtime;
                    cuahang.SubmitChanges();
                }
            }
            catch (Exception ex) { myme.ShowError(ex); }
        }
    }
}
