using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataAccessLayer;

namespace BusinessAccessLayer
{
    public class KhachHangBAL
    {
        MyMeThod myme = new MyMeThod();
        private List<Khach> GetAll()
        {
            try
            {
                using (QLCuaHangDataContext cuahang = new QLCuaHangDataContext())
                {
                    return cuahang.Khaches.ToList();
                }
            }
            catch (Exception ex) { myme.ShowError(ex); return null; }
        }
        public void LoadGridViewAll(DataGridView dgv)
        {
            dgv.DataSource = GetAll();
        }
        public void LoadComboboxTenKhach(ComboBox cbo) {
            try
            {
                cbo.DataSource = GetAll();
                cbo.DisplayMember = "tenkhach";
                cbo.ValueMember = "makhach";
            }
            catch(Exception ex) { myme.ShowError(ex); }
        }
        public void LoadComboboxMaKhach(ComboBox cbo)
        {
            try
            {
                cbo.DataSource = GetAll();
                cbo.DisplayMember = "makhach";
                cbo.ValueMember = "tenkhach";
            }
            catch (Exception ex) { myme.ShowError(ex); }
        }
        public string MaCuoiCung()
        {
            try
            {
                using (QLCuaHangDataContext cuahang = new QLCuaHangDataContext())
                {
                    return cuahang.Khaches.Max(k => k.MaKhach);
                }
            }
            catch (Exception ex) { myme.ShowError(ex); return null; }
        }
        public bool CheckMa(string ma)
        {
            try
            {
                using (QLCuaHangDataContext cuahang = new QLCuaHangDataContext())
                {
                    if (cuahang.Khaches.SingleOrDefault(k => k.MaKhach == ma) != null) return true;
                    else return false;
                }
            }
            catch (Exception ex) { myme.ShowError(ex); return false; }
        }
        public void AddOrEditKhach(string ma, string ten, string sdt, string diachi, string ghichu)
        {
            try
            {
                using (QLCuaHangDataContext cuahang = new QLCuaHangDataContext())
                {
                    if (diachi.Trim() == string.Empty) diachi = "(Không có)";
                    if (ghichu.Trim() == string.Empty) ghichu = "(Không có)";
                    if (!CheckMa(ma))  // Thêm mới khách.
                        cuahang.SP_ThemKhach(ma, ten, sdt, diachi, ghichu);
                    else // Sửa khách.
                        cuahang.SP_SuaKhach(ma, ten, sdt, diachi, ghichu);
                    cuahang.SubmitChanges();
                    MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo");
                }
            }
            catch (Exception ex) { myme.ShowError(ex); }
        }
        public void DeleteKhach(string ma)
        {//Chuyển mã khách ở nhập xuất thành null, cập nhật tên cho ghi chú. Sau đó xoá khách.
            try
            {
                using (QLCuaHangDataContext cuahang = new QLCuaHangDataContext())
                {
                    cuahang.SP_XoaKhach(ma);
                    cuahang.SubmitChanges();
                    MessageBox.Show("Xoá thông tin thành công!", "Thông báo");
                }
            }
            catch (Exception ex) { myme.ShowError(ex); }
        }
        public void TimKiemKhachHang(string cachtim, string tutim, DataGridView dgv)
        {
            try
            {
                using (QLCuaHangDataContext cuahang = new QLCuaHangDataContext())
                {
                    switch (cachtim)
                    {
                        case "Mã Khách Hàng":
                            {
                                dgv.DataSource = cuahang.Khaches.Where(kh => kh.MaKhach.Contains(tutim));
                                break;
                            }
                        case "Tên Khách Hàng":
                            {
                                dgv.DataSource = cuahang.Khaches.Where(kh => kh.TenKhach.Contains(tutim));
                                break;
                            }
                        case "Số Điện Thoại":
                            {
                                dgv.DataSource = cuahang.Khaches.Where(kh => kh.SDT.Contains(tutim));
                                break;
                            }
                        case "Địa Chỉ":
                            {
                                dgv.DataSource = cuahang.Khaches.Where(kh => kh.DiaChi.Contains(tutim));
                                break;
                            }
                        default:
                            {
                                dgv.DataSource = cuahang.Khaches.Where(kh => kh.GhiChu.Contains(tutim));
                                break;
                            }
                    }
                }
            }
            catch (Exception ex) { myme.ShowError(ex); }
        }
    }
}
