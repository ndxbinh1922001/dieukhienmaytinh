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
using MongoDB.Bson;
using MongoDB.Driver;
namespace client
{
    public partial class signin : Form
    {
        public signin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            var client = new MongoClient("mongodb+srv://bang:bang123@cluster0.aacbw.mongodb.net/myFirstDatabase?retryWrites=true&w=majority");
            IMongoDatabase db = client.GetDatabase("NT216");
            var coll = db.GetCollection<BsonDocument>("NT216_case_study");
            var filter = Builders<BsonDocument>.Filter.Eq("USERNAME", txtUsername.Text.Trim());

            var doc = coll.Find(filter).FirstOrDefault();
            if (doc == null)
            {
                MessageBox.Show("Tên đăng nhập và mật khẩu không tồn tại.");
                return;
            }
            string username = doc["USERNAME"].AsString;
            string password = doc["PASSWORD"].AsString;
            if (username == txtUsername.Text.Trim() && password == txtPassword.Text.Trim())
            {
                new Form1().Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Mật khẩu của bạn không đúng.");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new signup().Show();
        }
    }
}
