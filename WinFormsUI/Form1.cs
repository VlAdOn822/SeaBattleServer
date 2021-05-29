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
        private ISeaBattle game;
        int numOfPlayer;
        string name;
        Label ulljoingame;
        string gameCode;
        private IFieldReader fieldReader = new FileFieldReader("field.txt");
        Label[,] myField = new Label[10, 10];
        Label[,] anamiesField = new Label[10, 10];
        List<Label[,]> fields;
        Panel pnlMy = new Panel();
        Panel pnlEnemy = new Panel();
        Panel[] panels;
        string code;
        public Form1()
        {
            InitializeComponent();
            Connect();
            CreateFields();
            game = new Game();
        }

        private void CreateFields()
        {
            panels = new Panel[] { pnlMy, pnlEnemy };
            fields = new List<Label[,]>() { myField, anamiesField };
            for (int i = 0; i < 2; i++)
            {
                //panels[i].Location = new Point( i* 10*20)
                Label l = new Label();
                l.Click += L_Click;
                panels[i].Controls.Add(l);

                Controls.Add(panels[i]);
            }
        }

        private void L_Click(object sender, EventArgs e)
        {
            Label l = sender as Label;

            for (int i = 0; i < anamiesField.GetLength(0); i++)
            {
                for (int j = 0; j < anamiesField.GetLength(1); j++)
                {
                    if(l == anamiesField[i, j])
                    {
                        code = GetCode(i, j);
                        return;
                    }
                }
            }
            //найти номер лабел, записать в переменную ход. code (char)((int)('a') + i) 

        }

        private string GetCode(int i, int j)
        {
            return $"{(char)('a' + i)}{j}";
        }

        private void button2_Click(object sender, EventArgs e)
        {

            gameCode = game.StartGame();

            JoinGame();

        }

        private void JoinGame()
        {
            label1.Hide();
            button1.Hide();
            button2.Hide();

            name = tbName.Text;
            tbName.Hide();

            numOfPlayer = game.JoinGame(gameCode, name, fieldReader.ReadField());
            mainTimer.Start();
        }

        private static void Connect()
        {
            Uri tcpUri = new Uri("http://localhost:8000/SeaBattleServer");
            // Создаём сетевой адрес, с которым клиент будет взаимодействовать
            EndpointAddress address = new EndpointAddress(tcpUri);
            BasicHttpBinding binding = new BasicHttpBinding();
            // Данный класс используется клиентами для отправки сообщений
            ChannelFactory<ISeaBattle> factory = new ChannelFactory<ISeaBattle>(binding, address);
            // Открываем канал для общения клиента с со службой
            ISeaBattle service = factory.CreateChannel();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            gameCode = tbCode.Text;
            JoinGame();
        }

        private void mainTimer_Tick(object sender, EventArgs e)
        {
            string res;
            res = game.Update(gameCode, numOfPlayer);
            lInfo.Text = res;
            if (res != "Ready")
            {
                bMove.Enabled = false;
            }

            if (res == "Ready" && game.CheckWinner(gameCode) == -1)
            {
                PrintBoard(game.GetBoard(gameCode, numOfPlayer));
                bMove.Enabled = true;
                lInfo.Text = "Your move";
            }
            if (game.CheckWinner(gameCode) != -1)
            {
                if (game.CheckWinner(gameCode) == numOfPlayer)
                {
                    lInfo.Text = "You have won!!!";
                    bMove.Enabled = false;
                }
                else
                {
                    lInfo.Text = "You r looser";
                    bMove.Enabled = false;
                }
                mainTimer.Stop();
            }
        }

        private void PrintBoard(string[] field)
        {
            for (int i = 0; i < field.Length; i++)
            {
                for (int j = 0; j < field[i].Length; j++)
                {
                    myField[j, i].Text = field[i][j].ToString();
                }
            }
        }

        private void bMove_Click(object sender, EventArgs e)
        {
            game.Shot(gameCode, numOfPlayer, code);
        }
    }
}
