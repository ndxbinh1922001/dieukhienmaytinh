using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
namespace client
{
    public partial class khungchat_client : Form
    {
        public int port;
        public khungchat_client(int port)
        {
            InitializeComponent();
            this.port = port;
            Connect();
            CheckForIllegalCrossThreadCalls = false;
        }
        IPEndPoint ip;
        Socket client;
        Thread sendMess;
        void Connect()
        {
            ip = new IPEndPoint(IPAddress.Parse(Form1.a), port+1);
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                client.Connect(ip);
            }
            catch
            {
                MessageBox.Show("Không thể kết nối đến server.");
            }
            Thread listen = new Thread(receive);
            listen.IsBackground = true;
            listen.Start();
        }
        void receive()
        {
            while(client.Connected)
            {
                byte[] data = new byte[1024];
                client.Receive(data);
                string mess = System.Text.Encoding.UTF8.GetString(data);
                Addmessager(mess);
            }    
            
        }
        void Addmessager(string mess)
        {
            listView1.Items.Add(new ListViewItem() { Text = mess });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            sendMess = new Thread(send);
            sendMess.Start();
        }
        void send()
        {
            byte[] data = new byte[1024];
            data = System.Text.Encoding.UTF8.GetBytes("Client: " + textBox1.Text);
            if (textBox1.Text != string.Empty)
            {
                client.Send(data);
            }
            Addmessager("Client: " + textBox1.Text);
            textBox1.Text = "";
            sendMess.Abort();
        }

        private void khungchatclient_load(object sender, EventArgs e)
        {
           
        }
    }
}
