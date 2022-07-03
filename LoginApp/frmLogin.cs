using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dapper;

namespace LoginApp
{
    public partial class frmLogin : MetroFramework.Forms.MetroForm
    {
        public frmLogin()
        {
            InitializeComponent();
        }
        private void frmLogin_Load(object sender, EventArgs e)
        {
            this.StyleManager = metroStyleManager1;
            metroStyleManager1.Theme = MetroFramework.MetroThemeStyle.Light;
            metroStyleManager1.Style = MetroFramework.MetroColorStyle.Blue;

            if (Properties.Settings.Default.RememberMe)
            {
                txtUserName.Text = Properties.Settings.Default.UserName;
                txtPassword.Text = Properties.Settings.Default.Password;

            }
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUserName.Text))
            {
                MetroFramework.MetroMessageBox.Show(this, "Lütfen kullanıcıadınızı giriniz.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUserName.Focus();
                return;
            }
            try
            {
                using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["cn"].ConnectionString))
                {
                    if (db.State == ConnectionState.Closed)
                        db.Open();
                    User obj = db.Query<User>($"select *from Users where UserName = '{txtUserName.Text}'", commandType: CommandType.Text).SingleOrDefault();
                    if (obj != null)
                    {
                        if (obj.Password == txtPassword.Text)
                        {
                            using (frmMain frm = new frmMain())
                            {
                                this.Hide();
                                frm.ShowDialog();
                            }
                        }
                        else
                            MetroFramework.MetroMessageBox.Show(this, "Lütfen kullanıcıadınızı giriniz.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                        MetroFramework.MetroMessageBox.Show(this, "Kullanıcı adı veya şifre hatalı.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {

                MetroFramework.MetroMessageBox.Show(this, ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void chkRememberMe_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRememberMe.Checked)
            {
                Properties.Settings.Default.UserName = txtUserName.Text;
                Properties.Settings.Default.Password = txtPassword.Text;
            }
            else
            {
                Properties.Settings.Default.UserName = null;
                Properties.Settings.Default.Password = null;
            }
            Properties.Settings.Default.RememberMe = chkRememberMe.Checked;
            Properties.Settings.Default.Save();
        }

        
    }
    
}
