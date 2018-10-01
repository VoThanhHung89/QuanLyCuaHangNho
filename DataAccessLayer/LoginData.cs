using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;

namespace DataAccessLayer
{
    public class LoginData
    {
        Configuration conf;
        SqlConnection connect;
        private string _connecttionString = Properties.Settings.Default.QuanLyHangConnectionString;
        private string _user = Properties.Settings.Default.Username;
        private string _password = Properties.Settings.Default.Password;
        private bool _checkLogin = Properties.Settings.Default.CheckLogin;
        public string User
        {
            get
            {
                return _user;
            }

            set
            {
                _user = value;
                Properties.Settings.Default.Username = _user;
                Properties.Settings.Default.Save();
            }
        }
        public string Password
        {
            get
            {
                return _password;
            }

            set
            {
                _password = value;
                Properties.Settings.Default.Password = _password;
                Properties.Settings.Default.Save();
            }
        }
        public bool CheckLogin
        {
            get
            {
                return _checkLogin;
            }

            set
            {
                _checkLogin = value;
                Properties.Settings.Default.CheckLogin = _checkLogin;
                Properties.Settings.Default.Save();
            }
        }
        public string ConnecttionString
        {
            get
            {
                return _connecttionString;
            }
        }

        public LoginData() { conf = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None); }
        public LoginData(bool check, string u, string p) {
            this._user = u;
            this._password = p;
            this._checkLogin = check;
        }
        public void SaveConnectionString(string value)
        {
            var contsection = (ConnectionStringsSection)conf.GetSection(value);
            if(contsection != null) {
                conf.ConnectionStrings.ConnectionStrings["DataAccessLayer.Properties.Settings.QuanLyHangConnectionString"].ConnectionString = value;
                conf.ConnectionStrings.ConnectionStrings["DataAccessLayer.Properties.Settings.QuanLyHangConnectionString"].ProviderName = "System.Data.SqlClient";
                //conf.Save(ConfigurationSaveMode.Modified);
                conf.Save();
                ConfigurationManager.RefreshSection("connectionStrings");            }
        }
        public bool CheckConnectData(string stringconnect)
        {
            connect = new SqlConnection(stringconnect);
            if (connect.State == System.Data.ConnectionState.Closed)
                connect.Open();
            return true;
        }
    }
}
