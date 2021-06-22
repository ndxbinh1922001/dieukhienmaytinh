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
    public partial class Form1 : Form
    {
        public int  port;
        public Form1(int port)
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            this.port = port;
        }

        //IPEndPoint ip;
        TcpListener server;
        List<TcpClient> clientList;
        public void Connect()
        {
            clientList = new List<TcpClient>();
            server = new TcpListener(IPAddress.Any, port);
            server.Start();
            Thread listen = new Thread(() =>
            { 
                    while (true)
                    {
                        TcpClient client = server.AcceptTcpClient();
                        clientList.Add(client);
                    Thread rec = new Thread(()=>display(client,server,port));
                    rec.IsBackground = true;
                    rec.Start();                        
                    
                    //display(client, server, port);
                }
                
            });
            listen.IsBackground = true;
            listen.Start();

        }
        public void display(object obj1,object obj2,int port)
        {
            TcpClient cli = obj1 as TcpClient;
            TcpListener ser = obj2 as TcpListener;

            displayscreen f=new displayscreen(cli,ser,port);
            f.ShowDialog();
          
            
        }
        private void btnListen_Click(object sender, EventArgs e)
        {
            //int port = int.Parse(txtPort.Text);
            //new displayscreen(port).Show();
            //this.Hide();
            Connect();
        }
    }
}
