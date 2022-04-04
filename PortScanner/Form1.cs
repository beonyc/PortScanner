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
using System.Net.NetworkInformation;

namespace PortScanner
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        public static ManualResetEvent connectDone = new ManualResetEvent(false);

       
        private void button1_Click(object sender, EventArgs e)
        {
            int BeginigofPort = Convert.ToInt32(numericUpDown1.Value);
            int EndingofPort = Convert.ToInt32(numericUpDown2.Value);

            int i;

            progressBar1.Maximum = EndingofPort - BeginigofPort + 1;

            progressBar1.Value = 0;
            listView1.Items.Clear();

            IPAddress addr = IPAddress.Parse(textBox1.Text);

            for (i = BeginigofPort; i <= EndingofPort; i++) //Цикл для портов
            {
                // Создание и иницилизация сокета
                IPEndPoint ep = new IPEndPoint(addr, i);
                Socket soc = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // Попытка соединения с сервером
                IAsyncResult asyncResult = soc.BeginConnect(ep, new AsyncCallback(AnswerBack), soc);

                if (!asyncResult.AsyncWaitHandle.WaitOne(30, false))
                {
                    soc.Close();
                    listView1.Items.Add("Порт " + i.ToString());
                    listView1.Items[i - BeginigofPort].SubItems.Add("");
                    listView1.Items[i - BeginigofPort].SubItems.Add("Закрыт");
                    listView1.Items[i - BeginigofPort].BackColor = Color.White;
                    listView1.Refresh();
                    progressBar1.Value += 1;
                }
                else
                {
                    soc.Close();
                    listView1.Items.Add("Порт " + i.ToString());
                    listView1.Items[i - BeginigofPort].SubItems.Add("Открыт");
                    listView1.Items[i - BeginigofPort].BackColor = Color.LightGreen;
                    progressBar1.Value += 1;
                }
            }
            progressBar1.Value = 0;
        }


        private static void AnswerBack(IAsyncResult joker) // метод для сокета
        {
            try
            {
                Socket client = (Socket)joker.AsyncState;
                client.EndConnect(joker);
                connectDone.Set();
            }
            catch
            {

            }
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

      

    }
}
