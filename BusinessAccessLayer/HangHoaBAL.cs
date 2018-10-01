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
    public class HangHoaBAL
    {
        MyMeThod myme = new MyMeThod();
        public List<Hang> GetAll()
        {
            try
            {
                using (QLCuaHangDataContext cuahang = new QLCuaHangDataContext())
                {
                    return cuahang.Hangs.ToList();
                }
            }
            catch (Exception ex) { myme.ShowError(ex); return null; }
        }
        public void LoadGridView(DataGridView dgv)
        {
            dgv.DataSource = GetAll();
        }
        public void LoadComboboxHang(ComboBox cbomahang, ComboBox cbotenhang)
        {
            try
            {
                cbomahang.DataSource = cbotenhang.DataSource = GetAll();
                cbomahang.DisplayMember = cbotenhang.ValueMember = "MaHang";
                cbomahang.ValueMember = cbotenhang.DisplayMember = "TenHang";
            }catch(Exception ex) { myme.ShowError(ex); }
        }
        public string MaCuoiCung()
        {
            try
            {
                using (QLCuaHangDataContext cuahang = new QLCuaHangDataContext())
                {
                    return cuahang.Hangs.Max(h => h.MaHang);
                }
            }
            catch (Exception ex) { myme.ShowError(ex); return null; }
        }
        private bool CheckMa(string ma)
        {
            try
            {
                using (QLCuaHangDataContext cuahang = new QLCuaHangDataContext())
                {
                    if (cuahang.Hangs.SingleOrDefault(h => h.MaHang == ma) != null) return true;
                    else return false;
                }
            }
            catch (Exception ex) { myme.ShowError(ex); return false; }
        }
        public void AddOrEditHang(string ma, string ten, int soluong, string donvitinh, double gianhap, double giaxuat, string ghichu)
        {
            try
            {
                using (QLCuaHangDataContext cuahang = new QLCuaHangDataContext())
                {
                    if (!CheckMa(ma))// Thêm mới Hàng.
                        cuahang.SP_ThemHang(ma, ten, soluong, donvitinh, gianhap, giaxuat, ghichu);
                    else // Sửa Hàng.
                        cuahang.SP_SuaHang(ma, ten, soluong, donvitinh, gianhap, giaxuat, ghichu);
                    cuahang.SubmitChanges();
                    MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo");
                }
            }
            catch (Exception ex) { myme.ShowError(ex); }
        }
        public void DeleteHang(string ma)
        {
            try
            {
                using (QLCuaHangDataContext cuahang = new QLCuaHangDataContext())
                {
                    // Cập nhật thông tin từ Chi Tiết vào Ghi Chú của Nhập Xuất
                    Hang h = cuahang.Hangs.Single(hh => hh.MaHang == ma);
                    List<ChiTiet> ct = cuahang.ChiTiets.Where(c => c.MaHang == ma).ToList();
                    foreach(ChiTiet c in ct) {
                        string thongtin = DateTime.Now.ToShortDateString() + " xoá Hàng: " + h.TenHang + " -sl: " + c.SoLuong + " -giá: " + c.DonGia;
                        NhapXuat nx = cuahang.NhapXuats.SingleOrDefault( n => n.MaNX == c.MaNX);
                        nx.GhiChu += Environment.NewLine + thongtin; 
                    }
                    cuahang.SP_XoaHang(ma);
                    cuahang.SubmitChanges();
                    MessageBox.Show("Xoá thông tin thành công.", "Thông báo");
                }
            }
            catch (Exception ex) { myme.ShowError(ex); }
        }
        public void TimKiemHangHoa(string cachtim, string tutim, DataGridView dgv)
        {
            try
            {
                using (QLCuaHangDataContext cuahang = new QLCuaHangDataContext())
                {
                    switch (cachtim)
                    {
                        case ("Mã Hàng Hoá"):
                            {
                                dgv.DataSource = cuahang.Hangs.Where(h => h.MaHang.Contains(tutim));
                                break;
                            }
                        case ("Tên Hàng Hoá"):
                            {
                                dgv.DataSource = cuahang.Hangs.Where(h => h.TenHang.Contains(tutim));
                                break;
                            }
                        case ("Đơn Giá Nhập"):
                            {
                                dgv.DataSource = cuahang.Hangs.Where(h => h.DonGiaNhap == Convert.ToDouble(tutim));
                                break;
                            }
                        case ("Đơn Giá Xuất"):
                            {
                                dgv.DataSource = cuahang.Hangs.Where(h => h.DonGiaXuat == Convert.ToDouble(tutim));
                                break;
                            }
                        default:
                            {
                                dgv.DataSource = cuahang.Hangs.Where(h => h.GhiChu.Contains(tutim));
                                break;
                            }
                    }
                }
            }
            catch (Exception ex) { myme.ShowError(ex); }
        }
    }
}
