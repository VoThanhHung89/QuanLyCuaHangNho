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
    public class NhapXuatBAL
    {
        MyMeThod myme = new MyMeThod();
        ChiTietBAL ctBAL = new ChiTietBAL();
        public List<NhapXuat> GetNXAll(string loai)
        {
            try
            {
                using (QLCuaHangDataContext cuahang = new QLCuaHangDataContext())
                {
                    return cuahang.NhapXuats.Where(n => n.MaNX.Substring(0, 1) == loai).ToList();
                }
            }
            catch (Exception ex) { myme.ShowError(ex); return null; }
        }
        public void LoadGridView(DataGridView dgv, string nx)
        { //Lấy mã nhập, thông tin khách, ngày nhập, tổng tiền.
            try
            {
                using (QLCuaHangDataContext cuahang = new QLCuaHangDataContext())
                {
                    dgv.DataSource = cuahang.SP_XemNhapXuat().Where(x => x.MaNX.StartsWith(nx)).ToList();
                }
            }
            catch (Exception ex) { myme.ShowError(ex); }
        }
        public void LoadGridViewCT(DataGridView dgv, string ma)
        {
            try
            {
                using (QLCuaHangDataContext cuahang = new QLCuaHangDataContext())
                {
                    dgv.DataSource = cuahang.SP_XemCTNX(ma).ToList();
                }
            }
            catch (Exception ex) { myme.ShowError(ex); }
        }
        public void LoadComboboxMaPhieu(ComboBox cbo, string loai)
        {
            try
            {
                using (QLCuaHangDataContext cuahang = new QLCuaHangDataContext())
                {
                    cbo.DataSource = cuahang.NhapXuats.Where(nx => nx.MaNX.StartsWith(loai));
                    cbo.DisplayMember = "MaNX";
                    cbo.ValueMember = "MaNX";
                }
            }catch(Exception ex) { myme.ShowError(ex); }
        }
        /// <summary>
        /// Lấy mã cuối cùng của Nhập Xuất.
        /// </summary>
        /// <param name="nhapxuat">Là ký tự N hoặc X</param>
        /// <returns></returns>
        public string MaCuoiCung(string nhapxuat)
        {
            try
            {
                using (QLCuaHangDataContext cuahang = new QLCuaHangDataContext())
                {
                    return cuahang.NhapXuats.Where(n=>n.MaNX.StartsWith(nhapxuat)).Max(n => n.MaNX);
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
                    if (cuahang.NhapXuats.SingleOrDefault(n => n.MaNX == ma) != null) return true;
                    else return false;
                }
            }
            catch (Exception ex) { myme.ShowError(ex); return false; }
        }
        /// <summary>
        /// Trả về True nếu sl được phép. False nếu vượt quá sl tồn.
        /// </summary>
        /// <param name="mahang"></param>
        /// <param name="soluong"></param>
        /// <returns></returns>
        public bool CheckSoLuongXuatHang(string mahang, int soluong) {
        try {
                using (QLCuaHangDataContext cuahang = new QLCuaHangDataContext()) {
                    if (cuahang.Hangs.SingleOrDefault(h => h.MaHang == mahang && h.SoLuong >= soluong) != null) return true;
                    else return false;
                }
        }catch(Exception ex) { myme.ShowError(ex); return false; }
        }
        public void AddOrEditNX (string ma, string mak, DateTime ngay, string ghichu) {
            try {
                using (QLCuaHangDataContext cuahang = new QLCuaHangDataContext()) {
                    if (!CheckMa(ma))//Tạo mới.
                        cuahang.SP_ThemNX(ma, mak, ngay, ghichu);
                    else
                        cuahang.SP_SuaNX(ma, mak, ngay, ghichu);
                    cuahang.SubmitChanges();
                }
            }catch (Exception ex) { myme.ShowError(ex); }
        }
        public void DeleteNX(string ma) {
            try {
                using (QLCuaHangDataContext cuahang = new QLCuaHangDataContext()) {
                    NhapXuat nx = cuahang.NhapXuats.SingleOrDefault(n => n.MaNX == ma);
                    //Tiến hành xoá Chi tiết.
                    if (MessageBox.Show("Bạn có muốn tự động cập nhật lại Số lượng hàng hoá?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        List<ChiTiet> liCT = cuahang.ChiTiets.Where(c => c.MaNX == ma).ToList();
                        foreach (ChiTiet ct in liCT)
                        {
                            ctBAL.DeleteCTUpdateHang(ma, ct.MaHang);
                        }
                    }
                    else ctBAL.DeleteCTWithMaNX(ma);
                    //Tiến hành xoá nhập xuất.
                    cuahang.NhapXuats.DeleteOnSubmit(nx);
                    cuahang.SubmitChanges();
                    MessageBox.Show("Thông tin xoá thành công.","Thông báo");
                }
            }catch(Exception ex) { myme.ShowError(ex); }
        }
        public void TimKiemPhieu(string cachtim, string tutim, DataGridView dgv) {
            try
            {
                dgv.CurrentCell = null;
                string[] tencot = new string[5];
                if (dgv.Name == "dgvNhap") { tencot[0] = "man"; tencot[1] = "makhachn"; tencot[2] = "tenkhachn"; tencot[3] = "ngayn"; tencot[4] = "ghichun"; }
                else { tencot[0] = "max"; tencot[1] = "makhachx"; tencot[2] = "tenkhachx"; tencot[3] = "ngayx"; tencot[4] = "ghichux"; }
                switch (cachtim) {
                    case "Mã Phiếu":
                        foreach(DataGridViewRow dgr in dgv.Rows) {
                            if (tutim!=string.Empty && dgr.Cells[tencot[0]].Value.ToString().Contains(tutim.Trim())) dgr.Visible = true;
                            else dgr.Visible = false;
                        } 
                        break;
                    case "Mã Khách Hàng":
                        foreach (DataGridViewRow dgr in dgv.Rows)
                        {
                            if (dgr.Cells[tencot[1]].Value == null)
                            {
                                if (tutim == string.Empty) dgr.Visible = true;
                                else dgr.Visible = false;
                            }
                            else 
                            {
                                if (tutim != string.Empty && System.Text.RegularExpressions.Regex.IsMatch(dgr.Cells[tencot[1]].Value.ToString(), tutim, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                                    dgr.Visible = true;
                                else dgr.Visible = false;
                            }
                        }
                        break;
                    case "Tên Khách Hàng": 
                        foreach (DataGridViewRow dgr in dgv.Rows)
                        {
                            if (dgr.Cells[tencot[2]].Value == null)
                            {
                                if (tutim == string.Empty) dgr.Visible = true;
                                else dgr.Visible = false;
                            }
                            else
                            {
                                if (tutim != string.Empty && System.Text.RegularExpressions.Regex.IsMatch(dgr.Cells[tencot[2]].Value.ToString(), tutim, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                                    dgr.Visible = true;
                                else dgr.Visible = false;
                            }
                        }
                        break;
                    case "Ngày Lập Phiếu":
                        foreach (DataGridViewRow dgr in dgv.Rows)
                        {
                            if(tutim.Contains(" => "))//Tìm theo khoảng thời gian.
                            {
                                DateTime ngaybd = Convert.ToDateTime(tutim.Substring(0, 10));
                                DateTime ngaykt = Convert.ToDateTime(tutim.Substring(14, 10));
                                DateTime ngayss = Convert.ToDateTime(dgr.Cells[tencot[3]].Value.ToString());
                                if (ngaybd <= ngayss && ngayss <= ngaykt) dgr.Visible = true;
                                else dgr.Visible = false;
                            }
                            else//Tìm theo ngày cố định.
                            {
                                if (dgr.Cells[tencot[3]].Value.ToString().Contains(tutim)) dgr.Visible = true;
                                else dgr.Visible = false;
                            }
                        }
                        break;
                    default: //Cell 7
                        foreach (DataGridViewRow dgr in dgv.Rows)
                        {
                            if (dgr.Cells[tencot[7]].Value == null)
                            {
                                if (tutim == string.Empty) dgr.Visible = true;
                                else dgr.Visible = false;
                            }
                            else
                            {
                                if (tutim != string.Empty && System.Text.RegularExpressions.Regex.IsMatch(dgr.Cells[tencot[7]].Value.ToString(), tutim, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                                    dgr.Visible = true;
                                else dgr.Visible = false;
                            }
                        }
                        break;
                }
            }catch (Exception ex) { myme.ShowError(ex); }
        }
        #region FormNhapXuat
        /// <summary>
        /// Thêm danh sách Hàng hóa vào gridview của FormNhapXuat
        /// </summary>
        /// <param name="dgvDC">Danh sách các mặt hàng được chọn</param>
        /// <param name="dgvCC">Danh sách các mặt hàng chưa chọn</param>
        /// <param name="manx">Mã Nhập Xuất</param>
        public void LoadGridViewChonHang(DataGridView dgvDC, DataGridView dgvCC, string manx)
        {
            try
            {
                using (QLCuaHangDataContext cuahang = new QLCuaHangDataContext())
                {
                    if (CheckMa(manx)) //Đã có thì ta phải load cả 2 dgv.
                    {
                        List<Hang> hang = new List<Hang>();//Danh sách tất cả các mặt hàng.
                        if (manx.Contains("N")) hang = cuahang.Hangs.ToList();//Nếu là phiếu nhập thì load tất cả các mặt hàng.
                        else hang = cuahang.Hangs.Where(h => h.SoLuong > 1).ToList();//Nếu là phiếu xuất thì chỉ load các hàng còn số lượng.
                        List<string> mahangct = new List<string>();//Danh sách các mã hàng đã được chọn.
                        foreach (ChiTiet ct in ctBAL.GetCTbyMaNX(manx))
                        {
                            mahangct.Add(ct.MaHang);
                            //Add các hàng đã chọn vào Grid đã chọn.
                            string tenhang = cuahang.Hangs.SingleOrDefault(h => h.MaHang == ct.MaHang).TenHang;
                            dgvDC.Rows.Add("Xóa", 0, tenhang,"+","-", ct.SoLuong, ct.DonGia, ct.MaHang);
                            dgvDC.Sort(dgvDC.Columns["tenhangDC"], System.ComponentModel.ListSortDirection.Ascending);
                            hang.RemoveAll(h => h.MaHang == ct.MaHang); // Không chọn mặt hàng đã được chọn rồi.
                        }
                        foreach(Hang h in hang) {//Add các mặt hàng chưa được chọn vào grid Chưa chọn.
                            dgvCC.Rows.Add("Chọn", h.TenHang, "+", "-", 1, h.DonGiaNhap, h.MaHang);
                            dgvCC.Sort(dgvCC.Columns["tenhangCC"], System.ComponentModel.ListSortDirection.Ascending);
                        }
                    }
                    else //Chưa có thì ta chỉ cần load dgvCC
                    {
                        List<Hang> lih = cuahang.Hangs.ToList();
                        foreach (Hang h in lih)
                        {
                            dgvCC.Rows.Add("Chọn", h.TenHang, "+", "-", 1, h.DonGiaNhap, h.MaHang);
                            dgvCC.Sort(dgvCC.Columns["tenhangCC"], System.ComponentModel.ListSortDirection.Ascending);
                        }
                    }
                }
            } catch (Exception ex) { myme.ShowError(ex); }
        }
        /// <summary>
        /// Load thông tin của Phiếu nhập xuất đã có lên các Controls.
        /// </summary>
        /// <param name="manx"></param>
        /// <param name="cboTenKhach"></param>
        /// <param name="chbTenKhach"></param>
        /// <param name="dtpNgay"></param>
        /// <param name="txtGhiChu"></param>
        public void LoadThongTinNhapXuat(string manx, ComboBox cboTenKhach, CheckBox chbTenKhach, DateTimePicker dtpNgay, TextBox txtGhiChu) {
            try
            {
                using (QLCuaHangDataContext cuahang = new QLCuaHangDataContext()) {
                    NhapXuat nx = cuahang.NhapXuats.SingleOrDefault(n => n.MaNX == manx);
                    if (nx.MaKhach == null) chbTenKhach.Checked = true;
                    else cboTenKhach.SelectedValue = nx.MaKhach;
                    dtpNgay.Value = Convert.ToDateTime(nx.Ngay);
                    txtGhiChu.Text = nx.GhiChu;
                }
            }catch (Exception ex) { myme.ShowError(ex); }
        }
        #endregion
    }
}
