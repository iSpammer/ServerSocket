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
using System.Net.Sockets;
using System.Threading;
namespace ServerGUI
{
    public partial class Form1 : Form
    {
        Socket sock;
        Socket acc;
        public Form1()
        {
            InitializeComponent();
        }

        Socket socket() {
            return new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            sock = socket();
            sock.Bind(new IPEndPoint(0, 3));
            sock.Listen(0);

            new Thread(delegate() 
            {
                acc = sock.Accept();
                MessageBox.Show("Connection Accepted");

                while (true) {
                    try
                    {
                        byte[] buffer = new byte[255];
                        int rec = acc.Receive(buffer, 0, buffer.Length, 0);

                        if (rec <= 0)
                        {
                            throw new SocketException();
                        }

                        Array.Resize(ref buffer, rec);

                        Invoke((MethodInvoker)delegate
                        {
                            listBox1.Items.Add("Client 1:"+Encoding.Default.GetString(buffer));
                        });
                        send_6();
                    }
                    catch {
                        MessageBox.Show("Disconnected");
                        Application.Exit();

                        break;
                    }
                 }
                sock.Close();
            }).Start();
        }

        private void send_6() {
            byte[] data = Encoding.Default.GetBytes("6");
            acc.Send(data, 0, data.Length, 0);
        }


        private void button2_Click(object sender, EventArgs e)
        {
            byte[] data = Encoding.Default.GetBytes(textBox1.Text);
            acc.Send(data, 0, data.Length, 0);
        }
    }
}
