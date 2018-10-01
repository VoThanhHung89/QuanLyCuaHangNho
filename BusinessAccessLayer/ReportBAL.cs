using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DataAccessLayer;

namespace BusinessAccessLayer
{
    public class ReportBAL
    {
        MyMeThod myme = new MyMeThod();
        public DataTable KhachAll()
        {
            try
            {
                using (QLCuaHangDataContext cuahang = new QLCuaHangDataContext())
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("MaKhach");
                    dt.Columns.Add("TenKhach");
                    dt.Columns.Add("SDT");
                    dt.Columns.Add("DiaChi");
                    dt.Columns.Add("GhiChu");
                    DataRow dr;
                    foreach (Khach k in cuahang.Khaches)
                    {
                        dr = dt.NewRow();
                        dr["MaKhach"] = k.MaKhach;
                        dr["TenKhach"] = k.TenKhach;
                        dr["SDT"] = k.SDT;
                        dr["DiaChi"] = k.DiaChi;
                        dr["GhiChu"] = k.GhiChu;
                        dt.Rows.Add(dr);
                    }
                    return dt;
                }
            }
            catch (Exception ex)
            {
                myme.ShowError(ex);
                return null;
            }
        }
        public DataTable KhachGiaoDich(string makhach, char loai)
        {
            try
            {
                using (QLCuaHangDataContext cuahang = new QLCuaHangDataContext())
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("MaKhach");
                    dt.Columns.Add("TenKhach");
                    dt.Columns.Add("SDT");
                    dt.Columns.Add("DiaChi");
                    dt.Columns.Add("GhiChuKhach");
                    dt.Columns.Add("MaNX");
                    dt.Columns.Add("Ngay");
                    dt.Columns["Ngay"].DataType = typeof(DateTime);
                    dt.Columns.Add("GhiChuNX");
                    DataRow dr;
                    foreach (var kgd in cuahang.SP_XemKhachGiaoDich(makhach, loai))
                    {
                        dr = dt.NewRow();
                        dr["MaKhach"] = kgd.MaKhach;
                        dr["TenKhach"] = kgd.TenKhach;
                        dr["SDT"] = kgd.SDT;
                        dr["DiaChi"] = kgd.DiaChi;
                        dr["GhiChuKhach"] = kgd.GhiChuKhach;
                        dr["MaNX"] = kgd.MaNX;
                        if (kgd.Ngay != null) dr["Ngay"] = kgd.Ngay;
                        else dr["Ngay"] = DBNull.Value;
                        dr["GhiChuNX"] = kgd.GhiChuNX;
                        dt.Rows.Add(dr);
                    }
                    return dt;
                }
            }
            catch (Exception ex)
            {
                myme.ShowError(ex);
                return null;
            }
        }
        public DataTable ChiTietByMaNX(string manx)
        {
            try
            {
                using (QLCuaHangDataContext cuahang = new QLCuaHangDataContext())
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("STT");
                    dt.Columns["STT"].DataType = typeof(Int64);
                    dt.Columns.Add("MaHang");
                    dt.Columns.Add("TenHang");
                    dt.Columns.Add("SoLuong");
                    dt.Columns["STT"].DataType = typeof(Int64);
                    dt.Columns.Add("DonGia");
                    dt.Columns["DonGia"].DataType = typeof(double);
                    dt.Columns.Add("ThanhTien");
                    dt.Columns["ThanhTien"].DataType = typeof(double);
                    DataRow dr;
                    foreach (var ct in cuahang.SP_XemCTNX(manx))
                    {
                        dr = dt.NewRow();
                        dr["STT"] =ct.STT;
                        dr["MaHang"] =ct.MaHang;
                        dr["TenHang"] =ct.TenHang;
                        dr["SoLuong"] =ct.SoLuong;
                        dr["DonGia"] =ct.DonGia;
                        dr["ThanhTien"] =ct.ThanhTien;
                        dt.Rows.Add(dr);
                    }
                    return dt;
                }
            }
            catch(Exception ex) { myme.ShowError(ex); return null; }
        }
        public DataTable HangAll()
        {
            try
            {
                using(QLCuaHangDataContext cuahang = new QLCuaHangDataContext())
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("MaHang");
                    dt.Columns.Add("TenHang");
                    dt.Columns.Add("SoLuong");
                    dt.Columns["SoLuong"].DataType = typeof(Int64);
                    dt.Columns.Add("DonViTinh");
                    dt.Columns.Add("DonGiaNhap");
                    dt.Columns["DonGiaNhap"].DataType = typeof(double);
                    dt.Columns.Add("DonGiaXuat");
                    dt.Columns["DonGiaXuat"].DataType = typeof(double);
                    dt.Columns.Add("GhiChu");
                    DataRow dr;
                    foreach(Hang h in cuahang.Hangs)
                    {
                        dr = dt.NewRow();
                        dr["MaHang"] = h.MaHang;
                        dr["TenHang"] = h.TenHang;
                        dr["SoLuong"] = h.SoLuong;
                        dr["DonViTinh"] = h.DonViTinh;
                        dr["DonGiaNhap"] = h.DonGiaNhap;
                        dr["DonGiaXuat"] = h.DonGiaXuat;
                        dr["GhiChu"] = h.GhiChu;
                        dt.Rows.Add(dr);
                    }
                    return dt;
                }
            }catch(Exception ex) { myme.ShowError(ex); return null; }
        }
        public DataTable HangNX(string mahang, string ngaybd, string ngaykt)
        {
            try
            {
                using (QLCuaHangDataContext cuahang = new QLCuaHangDataContext())
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("MaHang");
                    dt.Columns.Add("TenHang");
                    dt.Columns.Add("SoLuongTon");
                    dt.Columns["SoLuongTon"].DataType = typeof(Int64);
                    dt.Columns.Add("DonViTinh");
                    dt.Columns.Add("DonGiaNhap");
                    dt.Columns["DonGiaNhap"].DataType = typeof(double);
                    dt.Columns.Add("DonGiaXuat");
                    dt.Columns["DonGiaXuat"].DataType = typeof(double);
                    dt.Columns.Add("GhiChu");
                    dt.Columns.Add("MaNX");
                    dt.Columns.Add("Ngay");
                    dt.Columns["Ngay"].DataType = typeof(DateTime);
                    dt.Columns.Add("SoLuongNX");
                    dt.Columns["SoLuongNX"].DataType = typeof(Int64);
                    dt.Columns.Add("DonGia");
                    dt.Columns["DonGia"].DataType = typeof(double);
                    DataRow dr;
                    foreach (var hnx in cuahang.SP_XemHangNX(mahang, ngaybd, ngaykt))
                    {
                        dr = dt.NewRow();
                        dr["MaHang"] = hnx.MaHang;
                        dr["TenHang"] = hnx.TenHang;
                        dr["SoLuongTon"] = hnx.SoLuongTon;
                        dr["DonViTinh"] = hnx.DonViTinh;
                        dr["DonGiaNhap"] = hnx.DonGiaNhap;
                        dr["DonGiaXuat"] = hnx.DonGiaXuat;
                        dr["GhiChu"] = hnx.GhiChu;
                        dr["MaNX"] = hnx.MaNX;
                        dr["Ngay"] = hnx.Ngay;
                        dr["SoLuongNX"] = hnx.SoLuongNX;
                        dr["DonGia"] = hnx.DonGia;
                        dt.Rows.Add(dr);
                    }
                    return dt;
                }
            }catch(Exception ex) { myme.ShowError(ex); return null; }
        }
        /// <summary>
        /// Danh sách Nhập Xuất xuất theo loại.
        /// </summary>
        /// <param name="loai">A:All - N:Nhập - X:Xuất</param>
        /// <returns></returns>
        public DataTable NhapXuat(string loai, DateTime? ngaybd, DateTime? ngaykt)
        {
            try
            {
                using (QLCuaHangDataContext cuahang = new QLCuaHangDataContext())
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("MaNX");
                    dt.Columns.Add("MaKhach");
                    dt.Columns.Add("TenKhach");
                    dt.Columns.Add("SDT");
                    dt.Columns.Add("DiaChi");
                    dt.Columns.Add("Ngay");
                    dt.Columns["Ngay"].DataType = typeof(DateTime);
                    dt.Columns.Add("GhiChu");
                    dt.Columns.Add("TongTien");
                    dt.Columns["TongTien"].DataType = typeof(double);
                    DataRow dr;
                    foreach(var nx in cuahang.SP_XemNhapXuat())
                    {
                        //Tất cả các loại phiếu.
                        if (loai == "A")
                        {
                            //Tất cả.
                            if (ngaybd == null && ngaykt == null)
                            {
                                dr = dt.NewRow();
                                dr["MaNX"] = nx.MaNX;
                                dr["MaKhach"] = nx.MaKhach;
                                dr["TenKhach"] = nx.TenKhach;
                                dr["SDT"] = nx.SDT;
                                dr["DiaChi"] = nx.DiaChi;
                                dr["Ngay"] = nx.Ngay;
                                dr["GhiChu"] = nx.GhiChu;
                                dr["TongTien"] = nx.TongTien;
                                dt.Rows.Add(dr);
                            }
                            //Tất cả theo ngày.
                            else
                            {
                                DateTime ngay1 = Convert.ToDateTime(ngaybd.ToString().Substring(0, 10));
                                DateTime ngay2 = Convert.ToDateTime(nx.Ngay.ToString().Substring(0, 10));
                                DateTime ngay3 = Convert.ToDateTime(ngaykt.ToString().Substring(0, 10));
                                if (ngay1 <= ngay2 && ngay2 <= ngay3)
                                {
                                    dr = dt.NewRow();
                                    dr["MaNX"] = nx.MaNX;
                                    dr["MaKhach"] = nx.MaKhach;
                                    dr["TenKhach"] = nx.TenKhach;
                                    dr["SDT"] = nx.SDT;
                                    dr["DiaChi"] = nx.DiaChi;
                                    dr["Ngay"] = nx.Ngay;
                                    dr["GhiChu"] = nx.GhiChu;
                                    dr["TongTien"] = nx.TongTien;
                                    dt.Rows.Add(dr);
                                }
                            }
                        }
                        //Tất cả theo Nhập hoặc Xuất.
                        else
                        {
                            if (nx.MaNX.ToString().Substring(0, 1) == loai)
                            { 
                                //Không theo thời gian.
                                if (ngaybd != null && ngaykt != null)
                                {
                                    DateTime ngay1 = Convert.ToDateTime(ngaybd.ToString().Substring(0, 10));
                                    DateTime ngay2 = Convert.ToDateTime(nx.Ngay.ToString().Substring(0, 10));
                                    DateTime ngay3 = Convert.ToDateTime(ngaykt.ToString().Substring(0, 10));
                                    if (ngay1 <= ngay2 && ngay2 <= ngay3)
                                    {
                                        dr = dt.NewRow();
                                        dr["MaNX"] = nx.MaNX;
                                        dr["MaKhach"] = nx.MaKhach;
                                        dr["TenKhach"] = nx.TenKhach;
                                        dr["SDT"] = nx.SDT;
                                        dr["DiaChi"] = nx.DiaChi;
                                        dr["Ngay"] = nx.Ngay;
                                        dr["GhiChu"] = nx.GhiChu;
                                        dr["TongTien"] = nx.TongTien;
                                        dt.Rows.Add(dr);
                                    }
                                }
                                //Theo thời gian.
                                else
                                {
                                    dr = dt.NewRow();
                                    dr["MaNX"] = nx.MaNX;
                                    dr["MaKhach"] = nx.MaKhach;
                                    dr["TenKhach"] = nx.TenKhach;
                                    dr["SDT"] = nx.SDT;
                                    dr["DiaChi"] = nx.DiaChi;
                                    dr["Ngay"] = nx.Ngay;
                                    dr["GhiChu"] = nx.GhiChu;
                                    dr["TongTien"] = nx.TongTien;
                                    dt.Rows.Add(dr);
                                }
                            }
                        }
                    }
                    return dt;
                }
            }catch(Exception ex) { myme.ShowError(ex); return null; }
        }
    }
}
