using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using System.Data;
using System.Windows.Forms;

namespace BusinessAccessLayer
{
    public class ChiTietBAL
    {
        MyMeThod myme = new MyMeThod();
        public List<ChiTiet> GetALLCT() {
            try {
                using (QLCuaHangDataContext cuahang = new QLCuaHangDataContext()) {
                    return cuahang.ChiTiets.ToList();
                }
            }catch (Exception ex) { myme.ShowError(ex); return null; }
        }
        public List<ChiTiet> GetCTbyMaNX(string manx) {
            try {
                using (QLCuaHangDataContext cuahang = new QLCuaHangDataContext()) {
                    return cuahang.ChiTiets.Where(c => c.MaNX == manx).ToList();
                }
            }catch (Exception ex) { myme.ShowError(ex); return null; }
        }
        public void AddOrEditCT(string manx, string mahang, int soluong, double dongia)
        {
            try
            {
                using (QLCuaHangDataContext cuahang = new QLCuaHangDataContext())
                {
                    if (!CheckMa(manx, mahang))//Thêm mới.
                        cuahang.SP_ThemCT(manx, mahang, soluong, dongia);
                    else cuahang.SP_SuaCT(manx, mahang, soluong, dongia);
                    cuahang.SubmitChanges();
                }
            }
            catch (Exception ex) { myme.ShowError(ex); }
        }
        public void DeleteCTUpdateHang(string manx, string mah) {
            try {
                using (QLCuaHangDataContext cuahang = new QLCuaHangDataContext()) {
                    //Cập nhật số lượng ở hàng.
                    ChiTiet c = cuahang.ChiTiets.SingleOrDefault(ct => ct.MaNX == manx && ct.MaHang == mah);
                    Hang h = cuahang.Hangs.SingleOrDefault(ha => ha.MaHang == c.MaHang);
                    if (manx.Substring(0, 1) == "N")  //Xoá ChiTiet của Nhập.
                        {
                            h.GhiChu += "\n Ngày: " + DateTime.Now.ToString("dd/MM/yyyy") + " xoá Chi Tiết phiếu Nhập thay đổi sl: " 
                                        + (h.SoLuong - c.SoLuong) + " => " + h.SoLuong;
                            h.SoLuong -= c.SoLuong;
                        }
                    else  // Xoá ChiTiet của Xuất. 
                        {
                            h.GhiChu += "\n Ngày: " + DateTime.Now.ToString("dd/MM/yyyy") + " xoá Chi Tiết phiếu Xuất thay đổi sl: "
                                        + (h.SoLuong + c.SoLuong) + " => " + h.SoLuong;
                            h.SoLuong += c.SoLuong;
                        }
                    //Tiến hành xoá chitiet.
                    cuahang.ChiTiets.DeleteOnSubmit(c);
                    cuahang.SubmitChanges();
                }
            }catch(Exception ex) { myme.ShowError(ex); }
        }
        public void DeleteCTWithMaNX(string manx)
        {
            try
            {
                using (QLCuaHangDataContext cuahang = new QLCuaHangDataContext())
                {
                    ChiTiet c = cuahang.ChiTiets.SingleOrDefault(ct => ct.MaNX == manx);
                    cuahang.ChiTiets.DeleteOnSubmit(c);
                    cuahang.SubmitChanges();
                }
            }
            catch (Exception ex) { myme.ShowError(ex); }
        }
        public void Update(string manx, DataGridView dgv) //Gridview có các columns(Xóa/TenHang/+/-/SL/DonGia/MaHang).
        {
            try {
                List<string> mahangtruoc = new List<string>(); //Danh sách các mã hàng trước khi update.
                foreach(ChiTiet ct in GetCTbyMaNX(manx)) {
                    mahangtruoc.Add(ct.MaHang);
                }
                List<string> mahangsau = new List<string>(); //Danh sách các mã hàng sau khi update.
                foreach(DataGridViewRow dgr in dgv.Rows) {
                    mahangsau.Add(dgr.Cells["mahangDC"].Value.ToString());//Cells có tên là mahangDC.
                }
                //So sách các mã hàng trước với sau nếu không còn thì xóa.
                foreach(string mht in mahangtruoc) {
                    bool vancon = false;
                    foreach(string mhs in mahangsau) {
                        if (mht == mhs) vancon = true;
                    }
                    if (!vancon) DeleteCTUpdateHang(manx, mht);
                }
                //Tiến hành cập nhật hoặc thêm mới các mặt hàng.
                foreach (DataGridViewRow dgr in dgv.Rows) {
                    AddOrEditCT(manx, dgr.Cells["mahangDC"].Value.ToString(), Convert.ToInt32(dgr.Cells["soluongDC"].Value), Convert.ToDouble(dgr.Cells["dongiaDC"].Value));
                }
            }
            catch (Exception ex) { myme.ShowError(ex); }
        }
        public bool CheckMa(string manx, string mahang)
        {
            try
            {
                using (QLCuaHangDataContext cuahang = new QLCuaHangDataContext())
                {
                    if (cuahang.ChiTiets.SingleOrDefault(ct => ct.MaNX == manx && ct.MaHang == mahang) != null) return true;
                    else return false;
                }
            }catch(Exception ex) { myme.ShowError(ex); return false; }
        }
    }
}
