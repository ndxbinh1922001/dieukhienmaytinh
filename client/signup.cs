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
                if (doc != null)
                {
                    MessageBox.Show("Tên đăng nhập đã tồn tại. Hãy sử dụng tên khác.");
                    return;
                }
            }
            catch (Exception)
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

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
