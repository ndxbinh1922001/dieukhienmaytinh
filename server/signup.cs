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
using MongoDB.Bson;
using MongoDB.Driver;
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

            //SqlConnection sqlcon_test_username = new SqlConnection(@"Data Source=DESKTOP-1A1OR0G;Initial Catalog=SIGNIN;Integrated Security=True");
            var client = new MongoClient("mongodb+srv://bang:bang123@cluster0.aacbw.mongodb.net/myFirstDatabase?retryWrites=true&w=majority");
            IMongoDatabase db = client.GetDatabase("NT216");
            var coll = db.GetCollection<BsonDocument>("NT216_case_study");
            try
            {
                var filter = Builders<BsonDocument>.Filter.Eq("USERNAME", tbTK.Text);
                var doc = coll.Find(filter).FirstOrDefault();
                if (doc!=null)
                {
                    MessageBox.Show("Tên đăng nhập đã tồn tại. Hãy sử dụng tên khác.");
                    return;
                }                                    
            }
            catch(Exception)
            {
               
            }
               

            var doc1 = new BsonDocument
            {
                {"USERNAME", tbTK.Text.Trim()},
                {"PASSWORD", tbMK.Text.Trim()}
            };
            coll.InsertOne(doc1);
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
