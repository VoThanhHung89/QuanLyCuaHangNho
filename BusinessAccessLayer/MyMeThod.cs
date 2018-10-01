using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BusinessAccessLayer
{
    public class MyMeThod
    {
        /// <summary>
        /// Tự tạo mã mới tăng dần.
        /// </summary>
        /// <param name="kytuma">Ký tự đầu của Mã</param>
        /// <param name="macuoicung">Mã lớn nhất trong List</param>
        /// <param name="chieudaima">Chiều dài của mã</param>
        /// <returns></returns>
        public string MaTuTang(string kytuma, string macuoicung, int chieudaima)
        {
            string mamoi = string.Empty;
            int socu, somoi = 0;
            if (!string.IsNullOrEmpty(macuoicung))
                socu = Convert.ToInt32(macuoicung.Substring(kytuma.Length, chieudaima - kytuma.Length));
            else socu = 0;
            somoi = socu + 1;
            mamoi = kytuma;
            while ((mamoi.Length + somoi.ToString().Length) < chieudaima)
            {
                mamoi += "0";
            }
            return mamoi + somoi.ToString();
        }
        public void ShowError(Exception ex)
        {
            String errorMessage;
            errorMessage = "Lỗi: ";
            errorMessage = String.Concat(errorMessage, ex.Message);
            errorMessage = String.Concat(errorMessage, "\n" + ex.StackTrace);
            if (DialogResult.Yes == MessageBox.Show("Không thành công. Quá trình phát sinh lỗi.\n Bạn có muốn xem lỗi?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Error))
            {
                MessageBox.Show(errorMessage, "Lỗi");
            }
        }
    }
}
