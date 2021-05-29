using SeaBattleServer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsUI
{
    public partial class Form1 : Form
    {
        int joinOrStart=-1;
        string name;
        Label ulljoingame;
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Uri tcpUri = new Uri("http://localhost:8000/SeaBattleServer");
            // Создаём сетевой адрес, с которым клиент будет взаимодействовать
            EndpointAddress address = new EndpointAddress(tcpUri);
            BasicHttpBinding binding = new BasicHttpBinding();
            // Данный класс используется клиентами для отправки сообщений
            ChannelFactory<ISeaBattle> factory = new ChannelFactory<ISeaBattle>(binding, address);
            // Открываем канал для общения клиента с со службой
            ISeaBattle service = factory.CreateChannel();
            label1.Dispose();
            button1.Dispose();
            button2.Dispose();
            if (joinOrStart == -1)
            {
                name = textBox1.Text;
                textBox1.Dispose();
                SeaBattleWFUI ui = new SeaBattleWFUI(new Game(), name);
                ui.Start();
            }
            else
            {
                int gameCode = Convert.ToInt32(textBox1.Text);
                textBox1.Dispose();
                ulljoingame.Dispose();
                SeaBattleWFUI ui = new SeaBattleWFUI(new Game(), name, gameCode.ToString());
                ui.Start();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            joinOrStart++;
            ulljoingame = new Label();
            ulljoingame.Text = "You will join someone's game";
            ulljoingame.Location = button1.Location;
            button1.Dispose();
            label1.Text = "Enter game code";
            name = textBox1.Text;
            this.Controls.Add(ulljoingame);
        }

    }
}
