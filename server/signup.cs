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
            SqlConnection sqlcon = new SqlConnection(@"Data Source=DESKTOP-1A1OR0G;Initial Catalog=SIGNIN;Integrated Security=True");
            string query = "INSERT INTO USERNAME VALUES(" + tbTK.Text.Trim() + "," + tbMK.Text.Trim() + ","+tbPort.Text.Trim()+ ")";
            SqlCommand command = new SqlCommand(query, sqlcon);
            command.Connection.Open();
            command.ExecuteNonQuery();
            MessageBox.Show("Đã đăng ký");
        }
    }
}
