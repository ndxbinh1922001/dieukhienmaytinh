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
using System.Security.Cryptography;
using System.IO;
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
            string query_username = "Select * from USERNAME Where dangnhap = " + tbTK.Text;
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
                port_str = port.Next(2000, 20000).ToString();
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
            string query = "INSERT INTO USERNAME VALUES(" + tbTK.Text + "," + tbMK.Text + ","+ port_str + ")";
            SqlCommand command = new SqlCommand(query, sqlcon);
            command.Connection.Open();
            command.ExecuteNonQuery();
            MessageBox.Show("Đã đăng ký");
        }
        static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }
        private void signup_Load(object sender, EventArgs e)
        {

        }
        
    }
}
