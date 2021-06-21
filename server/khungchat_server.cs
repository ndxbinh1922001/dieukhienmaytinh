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
namespace server
{
    public partial class khungchat_server : Form
    {
        IPEndPoint ip;
        Socket server;
        Thread receiveMess,sendMess;
        public int port;
        public khungchat_server(int port)
        {
            InitializeComponent();
            this.port = port;
            CheckForIllegalCrossThreadCalls = false;
            ip = new IPEndPoint(IPAddress.Any, port+1);
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.Bind(ip);
            server.Listen(-1);
            server = server.Accept();
            receiveMess = new Thread(receive);
            receiveMess.Start();
            receiveMess.IsBackground = true;
        }
        void send()
        {
            byte[] data = new byte[1024];
            data = System.Text.Encoding.UTF8.GetBytes("Server: " + textBox1.Text);
            Addmessager("Server: " + textBox1.Text);     
            if (textBox1.Text!=string.Empty)
            {
                server.Send(data);
            }
            textBox1.Text = "";
            sendMess.Abort();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            sendMess = new Thread(send);
            sendMess.Start();
        }
        void receive()
        {
            while (server.Connected)
            {
                byte[] data = new byte[1024];
                server.Receive(data);
                string mess = System.Text.Encoding.UTF8.GetString(data);
                Addmessager(mess);
            }
        }
        void Addmessager(string mess)
        {
            listView1.Items.Add(new ListViewItem() { Text=mess} );
        }
    }
}
