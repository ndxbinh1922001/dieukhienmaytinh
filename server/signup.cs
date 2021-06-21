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
    public partial class signup : Form
    {
        public signup()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            
            SqlConnection sqlcon_test_username = new SqlConnection(@"Data Source=DESKTOP-1A1OR0G;Initial Catalog=SIGNIN;Integrated Security=True");
            string query_username = "Select * from USERNAME Where dangnhap = " + tbTK.Text.Trim();
            SqlDataAdapter sda = new SqlDataAdapter(query_username, sqlcon_test_username);
            DataTable dtbl = new DataTable();
            sda.Fill(dtbl);
            if (dtbl.Rows.Count!=0)
            {
                MessageBox.Show("Tên đăng nhập đã tồn tại. Hãy sử dụng tên khác.");
                return;
            }    
            
            string port_str;
            bool test_port = false;
            do
            {
                Random port = new Random();
                port_str = port.Next(1024, 49151).ToString();
                SqlConnection sqlcontest = new SqlConnection(@"Data Source=DESKTOP-1A1OR0G;Initial Catalog=SIGNIN;Integrated Security=True");
                string query_test = "Select * from USERNAME Where port=" + port_str;
                SqlDataAdapter sda1 = new SqlDataAdapter(query_test, sqlcontest);
                DataTable dtbl1 = new DataTable();
                sda.Fill(dtbl1);
                if (dtbl1.Rows.Count == 0)
                {
                    test_port = true;
                }
            }
            while (!test_port);
            SqlConnection sqlcon = new SqlConnection(@"Data Source=DESKTOP-1A1OR0G;Initial Catalog=SIGNIN;Integrated Security=True");
            string query = "INSERT INTO USERNAME VALUES(" + tbTK.Text.Trim() + "," + tbMK.Text.Trim() + ","+ port_str + ")";
            SqlCommand command = new SqlCommand(query, sqlcon);
            command.Connection.Open();
            command.ExecuteNonQuery();
            MessageBox.Show("Đã đăng ký");
        }

        private void signup_Load(object sender, EventArgs e)
        {

        }
    }
}
