using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLCuaHang
{
    public partial class TimNgayNhapXuat : Form
    {
        public TimNgayNhapXuat()
        {
            InitializeComponent();
        }
        public static string ngay;
        private void TimNgayNhapXuat_Load(object sender, EventArgs e)
        {
            chbNgay.Checked = dtpNgay.Enabled = btnNgay.Enabled = true;
            chbKhoangTG.Checked = dtpNgayBD.Enabled = dtpNgayKT.Enabled = btnKhoangTG.Enabled = false;
        }

        private void chbNgay_Click(object sender, EventArgs e)
        {
            chbNgay.Checked = dtpNgay.Enabled = btnNgay.Enabled = true;
            chbKhoangTG.Checked = dtpNgayBD.Enabled = dtpNgayKT.Enabled = btnKhoangTG.Enabled = false;
        }

        private void chbKhoangTG_Click(object sender, EventArgs e)
        {
            chbNgay.Checked = dtpNgay.Enabled = btnNgay.Enabled = false;
            chbKhoangTG.Checked = dtpNgayBD.Enabled = dtpNgayKT.Enabled = btnKhoangTG.Enabled = true;
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            ngay = string.Empty;
            this.Close();
        }

        private void btnNgay_Click(object sender, EventArgs e)
        {
            ngay = dtpNgay.Value.ToShortDateString();
            this.Close();
        }

        private void btnKhoangTG_Click(object sender, EventArgs e)
        {
            ngay = dtpNgayBD.Value.ToShortDateString() +" => "+ dtpNgayKT.Value.ToShortDateString();
            this.Close();
        }
    }
}
