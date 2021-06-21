using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace server
{
    public partial class signin : Form
    {
        public signin()
        {
            InitializeComponent();
        }
        
        private void btnLogin_Click(object sender, EventArgs e)
        {
            SqlConnection sqlcon = new SqlConnection(@"Data Source=DESKTOP-1A1OR0G;Initial Catalog=SIGNIN;Integrated Security=True");
            string query = "Select * from USERNAME Where dangnhap = " + txtUsername.Text.Trim() + " and matkhau = " + txtPassword.Text.Trim();
            SqlDataAdapter sda = new SqlDataAdapter(query, sqlcon);
            DataTable dtbl = new DataTable();
            sda.Fill(dtbl);
            if (dtbl.Rows.Count == 1)
            {
                using (SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-1A1OR0G;Initial Catalog=SIGNIN;Integrated Security=True")) 
                {
                    string str = "select port from USERNAME where (dangnhap=" + txtUsername.Text + " AND matkhau=" + txtPassword.Text + ")";
                    SqlCommand cmd = new SqlCommand(str,connection);
                    connection.Open();
                    var sqlport = cmd.ExecuteScalar();

                    string a = sqlport.ToString();
                    int port = int.Parse(a.Trim());
                    new Form1(port).Show();
                    this.Hide();
                }
            }
            else
            {
                MessageBox.Show("Tài khoản hoặc mật khẩu của bạn không đúng.");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new signup().Show();
        }
    }
}
