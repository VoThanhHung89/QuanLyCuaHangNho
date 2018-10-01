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
    public partial class FormNhapXuat : Form
    {
        public FormNhapXuat()
        {
            InitializeComponent();
        }
        NhapXuatBAL nxBAL = new NhapXuatBAL();
        ChiTietBAL ctBAL = new ChiTietBAL();
        KhachHangBAL khBAL = new KhachHangBAL();
        HangHoaBAL hangBAL = new HangHoaBAL();
        MyMeThod myme = new MyMeThod();
        public static string manx;
        private void LoadThemMoi()
        {
            txtMa.Text = manx;
            if (manx.Contains("N")) rdbPN.Checked = true;
            else rdbPX.Checked = true;
            khBAL.LoadComboboxTenKhach(cboTenKhach);
            dtpNgay.Value = DateTime.Now;
            txtGhiChu.Text = string.Empty;
            chbHienTC.Checked = true;
            nmrTongTien.Value = 0;
        }
        private void LoadChinhSua() {
            txtMa.Text = manx;
            if (manx.Contains("N")) rdbPN.Checked = true;
            else rdbPX.Checked = true;
            khBAL.LoadComboboxTenKhach(cboTenKhach);
            nxBAL.LoadThongTinNhapXuat(manx, cboTenKhach, chbKhongKhach, dtpNgay, txtGhiChu);
        }
        private void AddSTT() {
            for(int i=0; i< dgvDaChon.RowCount; i++) {
                dgvDaChon.Rows[i].Cells["stt"].Value = i + 1;
            }
        }
        /// <summary>
        /// Load danh sách các mặt hàng lên Grid
        /// </summary>
        /// <param name="tenhangCC"></param>
        /// <param name="soluong"></param>
        /// <param name="dongia"></param>
        /// <param name="mahangCC"></param>
        /// <param name="hangdachonhaychua">True: Thêm Hàng vào grid đã được chọn / False: Thêm Hàng vào grid chưa được chọn</param>
        private void FormNhapXuat_Load(object sender, EventArgs e)
        {
            //Load dữ liệu.
            if (nxBAL.CheckMa(manx))//Trường hợp đã có mã --> Chỉnh sửa.
                LoadChinhSua();
            else//Trường hợp chưa có mã --> Thêm mới.
                LoadThemMoi();
            nxBAL.LoadGridViewChonHang(dgvDaChon, dgvChuaChon, manx);
            dgvDaChon.ClearSelection();
            dgvChuaChon.ClearSelection();
            AddSTT();
        }
        private void dgvChuaChon_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow dgrCC = dgvChuaChon.CurrentRow;
            //Tăng số lượng.
            if (dgvChuaChon.Columns[e.ColumnIndex].Name == "themhang" && e.RowIndex > -1) {
                int soluongtang = Convert.ToInt32(dgrCC.Cells["soluongCC"].Value) + 1;
                if(manx.Contains("N"))dgrCC.Cells["soluongCC"].Value = soluongtang;//Là phiếu nhập thì số lượng tùy thích.
                else//Là phiếu xuất chỉ cho xuất số lượng không vượt quá lượng tồn. 
                {
                    if (nxBAL.CheckSoLuongXuatHang(dgrCC.Cells["mahangCC"].Value.ToString(), soluongtang))
                        dgrCC.Cells["soluongCC"].Value = soluongtang;
                    else MessageBox.Show("Vượt quá số lượng tồn.", "Thông báo");
                }
            }
            //Giảm số lượng.
            if (dgvChuaChon.Columns[e.ColumnIndex].Name == "giamhang" && e.RowIndex > -1)
                if(Convert.ToDouble(dgrCC.Cells["soluongCC"].Value) >1)//Chỉ cho phép trừ khi số lượng lớn hơn 1.
                    dgrCC.Cells["soluongCC"].Value = Convert.ToInt32(dgrCC.Cells["soluongCC"].Value) - 1;
            //Thêm dòng vào Grid Hàng đã chọn.
            if (dgvChuaChon.Columns[e.ColumnIndex].Name == "chon" && e.RowIndex > -1)
            {
                //Thêm dòng vào dgv Danh sách Hàng đã chọn.
                dgvDaChon.Rows.Add("Xóa", dgvDaChon.RowCount+1, dgrCC.Cells["tenhangCC"].Value, "+", "-", dgrCC.Cells["soluongCC"].Value, 
                                    dgrCC.Cells["dongiaCC"].Value, dgrCC.Cells["mahangCC"].Value);
                dgvDaChon.Sort(dgvDaChon.Columns["tenhangDC"], ListSortDirection.Ascending);
                AddSTT();
                //Tính tổng tiền.
                Int64 tien = 0;
                foreach (DataGridViewRow dgr in dgvDaChon.Rows)
                {
                    tien += (Convert.ToInt64(dgr.Cells["soluongDC"].Value) * Convert.ToInt64(dgr.Cells["dongiaDC"].Value));
                }
                nmrTongTien.Value = Convert.ToInt64(tien);
                //Xóa dòng trên dgv Hàng chưa chọn.
                dgvChuaChon.Rows.RemoveAt(dgrCC.Index);
            }
        }
        private void dgvDaChon_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow dgrDC = dgvDaChon.CurrentRow;
            if (dgvDaChon.Columns[e.ColumnIndex].Name == "themds" && e.RowIndex > -1) {
                int soluongtang = Convert.ToInt32(dgrDC.Cells["soluongDC"].Value) + 1;
                if(manx.Contains("N")) dgrDC.Cells["soluongDC"].Value = soluongtang;
                else
                {
                    if (nxBAL.CheckSoLuongXuatHang(dgrDC.Cells["mahangDC"].Value.ToString(), soluongtang))
                        dgrDC.Cells["soluongDC"].Value = soluongtang;
                    else MessageBox.Show("Vượt quá số lượng tồn.", "Thông báo");
                }
            }
            if (dgvDaChon.Columns[e.ColumnIndex].Name == "giamds" && e.RowIndex > -1)
                if (Convert.ToDouble(dgrDC.Cells["soluongDC"].Value) > 1)//Chỉ cho phép trừ khi số lượng lớn hơn 1.
                    dgrDC.Cells["soluongDC"].Value = Convert.ToInt32(dgrDC.Cells["soluongDC"].Value) - 1;
            if (dgvDaChon.Columns[e.ColumnIndex].Name == "xoa" && e.RowIndex > -1) {
                //Thêm dòng vào dgvChuaChon chưa chọn. 
                dgvChuaChon.Rows.Add("Chọn", dgrDC.Cells["tenhangDC"].Value, "+", "-", 1, dgrDC.Cells["dongiaDC"].Value, dgrDC.Cells["mahangDC"].Value);
                dgvChuaChon.Sort(dgvChuaChon.Columns["tenhangCC"], ListSortDirection.Ascending);
                //Xóa dòng trên dgv Hàng đã chọn
                dgvDaChon.Rows.RemoveAt(dgrDC.Index);
                AddSTT();
                //Cập nhật lại stt grid đã chọn.
                for (int i=0; i<dgvDaChon.RowCount; i++) {
                    dgvDaChon.Rows[i].Cells["stt"].Value = i+1;
                }
            }
            //Tính tổng tiền.
            Int64 tien = 0;
            foreach (DataGridViewRow dgr in dgvDaChon.Rows)
            {
                tien += (Convert.ToInt64(dgr.Cells["soluongDC"].Value) * Convert.ToInt64(dgr.Cells["dongiaDC"].Value));
            }
            nmrTongTien.Value = Convert.ToInt64(tien);
        }
        //Tìm kiếm
        private void chbHienTC_Click(object sender, EventArgs e)
        {
            chbHienTC.Checked = true;
            txtTimTenHang.Text = string.Empty;
            for (int i = 0; i < dgvChuaChon.RowCount; i++)
            {
                if (dgvChuaChon.Rows[i].Visible == false)
                    dgvChuaChon.Rows[i].Visible = true;
            }
        }
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            StringComparer comp = StringComparer.OrdinalIgnoreCase;
           for(int i=0; i< dgvChuaChon.RowCount; i++) {
                string tenhang = dgvChuaChon.Rows[i].Cells["tenhangCC"].Value.ToString();
                bool check = System.Text.RegularExpressions.Regex.IsMatch(tenhang, txtTimTenHang.Text, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                if (check == false)
                    dgvChuaChon.Rows[i].Visible = false;
                else dgvChuaChon.Rows[i].Visible = true;
           }
            chbHienTC.Checked = false;
        }
        private void chbKhongKhach_CheckedChanged(object sender, EventArgs e)
        {
            if (chbKhongKhach.Checked)
            {
                cboTenKhach.Text = string.Empty;
                cboTenKhach.Enabled = false;
            }
            else
            {
                cboTenKhach.SelectedIndex = 1;
                cboTenKhach.Enabled = true;
            }
        }
        //Load lại thông tin trên Form
        private void LoadDataSauLuu(bool chinhsua) //Là True nếu thêm mới, False là chỉnh sửa.
        {
            while (dgvDaChon.RowCount > 0)
            {
                dgvDaChon.Rows.RemoveAt(0);
            }
            while (dgvChuaChon.RowCount > 0)
            {
                dgvChuaChon.Rows.RemoveAt(0);
            }
            if (chinhsua) LoadChinhSua();
            else
            {
                if (manx.Contains("N")) //Phiếu nhập.
                    manx = myme.MaTuTang("N", nxBAL.MaCuoiCung("N"), 15);
                else manx = myme.MaTuTang("X", nxBAL.MaCuoiCung("X"), 15);
                LoadThemMoi();
            }
            //Load Gridview
            nxBAL.LoadGridViewChonHang(dgvDaChon, dgvChuaChon, manx);
            dgvDaChon.ClearSelection();
            dgvChuaChon.ClearSelection();
            AddSTT();
        }
        private void XuLyLuuChiTiet(bool chinhsua) //Là True nếu thêm mới, False là chỉnh sửa
        {
            if(chinhsua) ctBAL.Update(manx, dgvDaChon);
            else
            {
                for (int i = 0; i < dgvDaChon.RowCount; i++)
                {
                    string manx = txtMa.Text;
                    string mahang = dgvDaChon.Rows[i].Cells["mahangDC"].Value.ToString();
                    int sl = Convert.ToInt32(dgvDaChon.Rows[i].Cells["soluongDC"].Value);
                    double dongia = Convert.ToDouble(dgvDaChon.Rows[i].Cells["dongiaDC"].Value);
                    ctBAL.AddOrEditCT(manx, mahang, sl, dongia);
                }
            }
        }
        private void btnLuu_Click(object sender, EventArgs e)
        {
            bool chinhsua = true; //Chỉnh sửa sẽ là True. Tương ứng đã có mã rồi.
            if (!nxBAL.CheckMa(manx)) chinhsua = false; //Thêm mới là False. Xét điều kiện là chưa có mã.
            List<string> mathangvuotsoluong = new List<string>();//Tạo để kiểm tra điều kiện tạo phiếu xuất. Không cho chọn số lượng vượt quá số lượng tồn.
            if (manx.Contains("X"))//Chỉ xét với phiếu xuất. Thêm những mặt hàng có số lượng vượt lượng tồn vào danh sách loại.
            {
                foreach (DataGridViewRow dgr in dgvDaChon.Rows)
                {
                    if (!nxBAL.CheckSoLuongXuatHang(dgr.Cells["mahangDC"].Value.ToString(), Convert.ToInt32(dgr.Cells["soluongDC"].Value)))
                    {
                        mathangvuotsoluong.Add(dgr.Cells["mahangDC"].Value.ToString());
                    }
                }
            }
            //Sau khi check nếu không có mặt hàng nào vượt số lượng tồn.
            //Với phiếu nhập thì số này luôn = 0.
            if (mathangvuotsoluong.Count == 0)
            {
                //--Thêm nhập xuất.
                if (chbKhongKhach.Checked) nxBAL.AddOrEditNX(txtMa.Text, null, dtpNgay.Value, txtGhiChu.Text);
                else nxBAL.AddOrEditNX(txtMa.Text, cboTenKhach.SelectedValue.ToString(), dtpNgay.Value, txtGhiChu.Text);
                //--Thêm chi tiết.
                XuLyLuuChiTiet(chinhsua);
                MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo");
                //--Load lại dữ liệu sau khi ấn Lưu.
                LoadDataSauLuu(chinhsua);
            }
            else
            {
                MessageBox.Show("Hiện tại Phiếu Xuất có mặt hàng có số lượng vượt quá số lượng tồn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                foreach (string ma in mathangvuotsoluong)
                {
                    for (int i = 0; i < dgvDaChon.RowCount; i++)
                    {
                        if (dgvDaChon.Rows[i].Cells["mahangDC"].Value.ToString() == ma)
                        {
                            dgvDaChon.Rows[i].Cells["tenhangDC"].Selected = true;
                        }
                    }
                }
            }
        }
        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnLuuVaXuatPhieu_Click(object sender, EventArgs e)
        {
            bool chinhsua = true; //Chỉnh sửa sẽ là True. Tương ứng đã có mã rồi.
            if (!nxBAL.CheckMa(manx)) chinhsua = false; //Thêm mới là False. Xét điều kiện là chưa có mã.
            List<string> mathangvuotsoluong = new List<string>();//Tạo để kiểm tra điều kiện tạo phiếu xuất. Không cho chọn số lượng vượt quá số lượng tồn.
            if (manx.Contains("X"))//Chỉ xét với phiếu xuất. Thêm những mặt hàng có số lượng vượt lượng tồn vào danh sách loại.
            {
                foreach (DataGridViewRow dgr in dgvDaChon.Rows)
                {
                    if (!nxBAL.CheckSoLuongXuatHang(dgr.Cells["mahangDC"].Value.ToString(), Convert.ToInt32(dgr.Cells["soluongDC"].Value)))
                    {
                        mathangvuotsoluong.Add(dgr.Cells["mahangDC"].Value.ToString());
                    }
                }
            }
            //Sau khi check nếu không có mặt hàng nào vượt số lượng tồn.
            //Với phiếu nhập thì số này luôn = 0.
            if (mathangvuotsoluong.Count == 0)
            {
                //--Thêm nhập xuất.
                if (chbKhongKhach.Checked) nxBAL.AddOrEditNX(txtMa.Text, null, dtpNgay.Value, txtGhiChu.Text);
                else nxBAL.AddOrEditNX(txtMa.Text, cboTenKhach.SelectedValue.ToString(), dtpNgay.Value, txtGhiChu.Text);
                //--Thêm chi tiết.
                XuLyLuuChiTiet(chinhsua);
                MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo");
                //Hiện Hóa Đơn.
                HoaDon.manx = txtMa.Text;
                HoaDon frm = new HoaDon();
                frm.ShowDialog();
                //--Load lại dữ liệu sau khi ấn Lưu.
                LoadDataSauLuu(chinhsua);
            }
            else
            {
                MessageBox.Show("Hiện tại Phiếu Xuất có mặt hàng có số lượng vượt quá số lượng tồn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                foreach (string ma in mathangvuotsoluong)
                {
                    for (int i = 0; i < dgvDaChon.RowCount; i++)
                    {
                        if (dgvDaChon.Rows[i].Cells["mahangDC"].Value.ToString() == ma)
                        {
                            dgvDaChon.Rows[i].Cells["tenhangDC"].Selected = true;
                        }
                    }
                }
            }
        }
    }
}
