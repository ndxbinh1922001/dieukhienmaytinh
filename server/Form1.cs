using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace server
{
    public partial class Form1 : Form
    {
        public int  port;
        public Form1(int port)
        {
            InitializeComponent();
            this.port = port;
        }
        

        private void btnListen_Click(object sender, EventArgs e)
        {
            //int port = int.Parse(txtPort.Text);
            new displayscreen(port).Show();
            this.Hide();
        }
    }
}
