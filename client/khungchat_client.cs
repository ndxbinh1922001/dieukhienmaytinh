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
using System.Security.Cryptography;
using System.IO;
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
                string plaintext = "";
                string key = "nguyendoanxuanbinh19220011952126";
                string iv = "1234567890abcdef";
                byte[] key_byte = System.Text.Encoding.UTF8.GetBytes(key);
                byte[] iv_byte = System.Text.Encoding.UTF8.GetBytes(iv);
                byte[] data = new byte[1024];                                                
                client.Receive(data);
                byte[] cipher = remove_padding_array(data);
                plaintext = DecryptStringFromBytes_Aes(cipher, key_byte, iv_byte);
                //string mess = System.Text.Encoding.UTF8.GetString(data);
                Addmessager(plaintext);
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
            string plaintext = "Client: " + textBox1.Text;
            string key = "nguyendoanxuanbinh19220011952126";
            string iv = "1234567890abcdef";
            byte[] key_byte = System.Text.Encoding.UTF8.GetBytes(key);
            byte[] iv_byte = System.Text.Encoding.UTF8.GetBytes(iv);
            byte[] encrypted = EncryptStringToBytes_Aes(plaintext, key_byte, iv_byte);
            //data = System.Text.Encoding.UTF8.GetBytes("Client: " + textBox1.Text);
            client.Send(encrypted);            
            Addmessager("Client: " + textBox1.Text);
            textBox1.Text = "";
            sendMess.Abort();
        }

        private void khungchatclient_load(object sender, EventArgs e)
        {
           
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
        static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            // Declare the string used to hold the decrypted text.
            string plaintext;
            // Create an Aes object with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;
                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }
        static byte[] remove_padding_array(byte[] a)
        {
            int length = 0;
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] == 0)
                {
                    length = i;
                    break;
                }

            }
            byte[] b = new byte[length];
            for (int i = 0; i < length; i++)
            {
                b[i] = a[i];
            }
            return b;
        }
    }
}
