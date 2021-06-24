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
    public partial class displayscreen : Form
    {
        public TcpClient client;
        public TcpListener server;
        public Thread listening;
        public Thread getImage;
        public Thread sendData;
        public Thread receiveMessage;
        public NetworkStream nSteam;
        public int port;
        public displayscreen(TcpClient obj1,TcpListener obj2,int port)
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            this.client=obj1 as TcpClient;
            this.server = obj2 as TcpListener;
            this.port = port;
            //client = new TcpClient();
            //tạo luồng để lắng nghe kết nối từ client
            listening = new Thread(startListening);
            //tạo luồng nhận dữ liệu màn hình 
            getImage = new Thread(receiveImage);
            //tạo luồng nhận dữ liệu tin nhắn từ client
           
            //this.port = port;
            //server = new TcpListener(IPAddress.Any, port);
            listening.Start();
        }  
        //hàm lắng nghe kết nối
        private void startListening()
        { 
            //while (!client.Connected)
            //{
            //    server.Start();
            //    client = server.AcceptTcpClient();
            //}
            getImage.Start();//bắt đầu nhận dữ liệu hình ảnh
            getImage.Join();
        }
        //hàm nhận dữ liệu hình ảnh
        private void receiveImage()
        {
            BinaryFormatter bin = new BinaryFormatter(); 
                while (client.Connected)
                {
                    nSteam = client.GetStream();
                while(true)
                {
                    try { pictureBox1.Image = (Image)bin.Deserialize(nSteam); }

                    catch
                    {
                        break;
                    }
                }
                }                                           
        }
        //khi form displayscreen đóng thì gọi hàm stopListening để đóng kết nối socket
        private void Displayscreen_closing(object sender, FormClosingEventArgs e)
        {
            stopListening();
        }
        //Hàm đóng kết nối socket
        private void stopListening()
        {
            server.Stop();
            client = null;
            if (listening.IsAlive)
            {
                listening.Abort();
            }
            if (getImage.IsAlive)
            {
                getImage.Abort();
            }
            
        }
        //button tạo thread để gửi thông báo cho client tắt máy
        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("client is shutdowning.");
            sendData = new Thread(sendshutdown);
            sendData.Start();
        }
        //hàm gửi lệnh tắt máy
        void sendshutdown()
        {
            Byte[] data = System.Text.Encoding.UTF8.GetBytes("!@#$%^&*()");
            nSteam.Write(data, 0, data.Length);
        }
        //hàm gửi lệnh restart
        void sendrestart()
        {
            Byte[] data = System.Text.Encoding.ASCII.GetBytes("+_)(*&^%$#@!");
            nSteam.Write(data, 0, data.Length);
        }
        //button gửi lệnh restart cho client
        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("client is restarting.");
            sendData = new Thread(sendrestart);
            sendData.Start();
        }
        
        //button gửi lệnh sleep cho client
        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("client is sleeping.");
            sendData = new Thread(sendsleep);
            sendData.Start();
        }
        //hàm gửi lệnh sleep cho client
        void sendsleep()
        {
            Byte[] data = System.Text.Encoding.ASCII.GetBytes("<>?:{}|");
            nSteam.Write(data, 0, data.Length);
        }   
        private void btnChat_Click(object sender, EventArgs e)
        {
            sendChat();
            khungchat_server f = new khungchat_server(port);
            f.Show();
           
        }
        
        void sendChat()
        {
            Byte[] data = System.Text.Encoding.UTF8.GetBytes("__________");
            nSteam.Write(data, 0, data.Length);
        }
        private void displayscreen_Load(object sender, EventArgs e)
        {

        }        
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
       
        
    }
}
