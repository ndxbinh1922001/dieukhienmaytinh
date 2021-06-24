using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using System.Drawing.Imaging;
using System.Runtime.Serialization.Formatters.Binary;
using System.Diagnostics;
using System.Runtime.InteropServices;
namespace client
{

    public partial class Form1 : Form
    {
        
        [DllImport("user32")]
        public static extern void LockWorkStation();
        TcpClient client = new TcpClient();
        
        NetworkStream nStream;
        public int PortNumber;
        
        public static string a = "";
        
        public Form1()
        {
            InitializeComponent();
            
            CheckForIllegalCrossThreadCalls = false;
        }
        //hàm tạo kết nối đến server và nhận dữ liệu từ server gửi về
        private void btnConnect_Click(object sender, EventArgs e)
        {
            //PortNumber = int.Parse(txtPort.Text);
            PortNumber = Int32.Parse(tbPort.Text.Trim());
            try
            {
                client.Connect(txtIP.Text, PortNumber);                
                MessageBox.Show("Connect complete");
                
                nStream = client.GetStream();
                
                Thread thread = new Thread(new ThreadStart(receivedata));
                thread.IsBackground = true;
                thread.Start();
                
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //hàm nhận dữ liệu từ server gửi đến client (dữ liệu bao gồm lệnh shutdown,restart,sleep và mess)
        void receivedata()
        {
            while (client.Connected)
            {
                byte[] myReadBuffer = new byte[1024];
                StringBuilder myCompleteMessage = new StringBuilder();
                int numberOfBytesRead = 0;
                numberOfBytesRead = nStream.Read(myReadBuffer, 0, myReadBuffer.Length);
                myCompleteMessage.AppendFormat("{0}", Encoding.UTF8.GetString(myReadBuffer, 0, numberOfBytesRead));

                string data = myCompleteMessage.ToString().Trim();

                if (data == "!@#$%^&*()")
                    Process.Start("shutdown.exe", "-s -t 0");
                else if (data == "+_)(*&^%$#@!")
                    Process.Start("shutdown.exe", "-r -t 0");
                else if (data == "<>?:{}|")
                    LockWorkStation();
                else if (data =="__________")
                {
                    a = txtIP.Text;
                    MessageBox.Show("Server muốn trò chuyện với bạn");
                }     
            }
        }
        
        //hàm cho phép server thấy được màn hình của client
        private void btnScreen_Click(object sender, EventArgs e)
        {
            if (btnScreen.Text.StartsWith("Show screen"))
            {
                timer1.Start();
                btnScreen.Text = "Stop";
            }
            else
            {
                timer1.Stop();
                btnScreen.Text = "Show screen";
            }
        }
        //hàm gửi dữ liệu màn hình theo thời gian
        private void timer1_Tick(object sender, EventArgs e)
        {
            sendDesktop();
        }
        //hàm mã hóa để gửi dữ liệu cho server
        private void sendDesktop()
        {
            BinaryFormatter bin = new BinaryFormatter();
            bin.Serialize(nStream, getDesktop());
        }
        //hàm lấy dữ liệu màn hình để gửi cho server
        private static Image getDesktop()
        {
            Rectangle bounds = Screen.PrimaryScreen.Bounds;
            Bitmap sreenshot = new Bitmap(bounds.Width, bounds.Height, PixelFormat.Format32bppArgb);
            Graphics graphic = Graphics.FromImage(sreenshot);
            graphic.CopyFromScreen(bounds.X, bounds.Y, 0, 0,bounds.Size,CopyPixelOperation.SourceCopy);
            return sreenshot;
        }

        private void btnChat_Click(object sender, EventArgs e)
        {
            new khungchat_client(PortNumber).Show();
        }

        

       
        
    }
}
