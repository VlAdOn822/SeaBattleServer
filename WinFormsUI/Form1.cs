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
        int numOfEnemy;
        int numOfPlayer;
        string name;
        string gameCode;
        private IFieldReader fieldReader = new FileFieldReader("field.txt");
        Label[,] myField = new Label[10, 10];
        Label[,] enemyField = new Label[10, 10];
        List<Label[,]> fields;
        Panel pnlMy = new Panel();
        Panel pnlEnemy = new Panel();
        Panel[] panels;
        string code;

        public Form1()
        {
            InitializeComponent();
            //Connect();
            Uri tcpUri = new Uri("http://localhost:9000/SeaBattleServer");
            // Создаём сетевой адрес, с которым клиент будет взаимодействовать
            EndpointAddress address = new EndpointAddress(tcpUri);
            BasicHttpBinding binding = new BasicHttpBinding();
            // Данный класс используется клиентами для отправки сообщений
            ChannelFactory<ISeaBattle> factory = new ChannelFactory<ISeaBattle>(binding, address);
            // Открываем канал для общения клиента с со службой
            ISeaBattle service = factory.CreateChannel();
            game = service;
        }

        private void CreateFields()
        {
            panels = new Panel[] { pnlMy, pnlEnemy };
            fields = new List<Label[,]>() { myField, enemyField };
            for (int i = 0; i < 2; i++)
            {
                panels[i].Location = new Point(20 + i * 400, 50);
                panels[i].Size = new Size(360, 360);
                panels[i].BorderStyle = BorderStyle.Fixed3D;
                panels[i].Click += L_Click;
                Controls.Add(panels[i]);

                Label fieldLabel = new Label();
                if (i == 0)
                    fieldLabel.Text = $"Your panel";
                else
                    fieldLabel.Text = "Enemy's field";
                fieldLabel.Location = new Point(panels[i].Location.X, 18);
                Controls.Add(fieldLabel);

                int mySize = panels[i].Width / 10;
                for (int y = 0; y < 10; y++)
                {
                    for (int x = 0; x < 10; x++)
                    {
                        Label label = new Label();
                        label.Size = new Size(mySize, mySize);
                        label.Location = new Point(x * mySize, y * mySize);
                        label.BorderStyle = BorderStyle.FixedSingle;
                        label.TextAlign = ContentAlignment.MiddleCenter;
                        if (i == 0)
                            myField[x, y] = label;
                        else
                            enemyField[x, y] = label;
                        panels[i].Controls.Add(label);
                        label.MouseClick += new MouseEventHandler(this.L_Click);
                    }
                }
            }
        }

        private void L_Click(object sender, EventArgs e)
        {
            Label l = sender as Label;
            l.BackColor = Color.Gray;

            for (int i = 0; i < enemyField.GetLength(0); i++)
            {
                for (int j = 0; j < enemyField.GetLength(1); j++)
                {
                    if (l == enemyField[i, j])
                    {
                        code = GetCode(i, j);
                        return;
                    }
                }
            }
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
            tbCode.Hide();
            lInfo.Location = new Point(100, 421);
            name = tbName.Text;
            tbName.Hide();
            numOfEnemy = game.JoinGame(gameCode, name, fieldReader.ReadField());
            if (numOfEnemy == 0) numOfPlayer = 1;
            else numOfPlayer = 0;
            CreateFields();
            mainTimer.Start();
        }

        //private static void Connect()
        //{
        //    Uri tcpUri = new Uri("http://localhost:8000/SeaBattleServer");
        //    Создаём сетевой адрес, с которым клиент будет взаимодействовать
        //   EndpointAddress address = new EndpointAddress(tcpUri);
        //    BasicHttpBinding binding = new BasicHttpBinding();
        //    Данный класс используется клиентами для отправки сообщений
        //    ChannelFactory<ISeaBattle> factory = new ChannelFactory<ISeaBattle>(binding, address);
        //    Открываем канал для общения клиента с со службой
        //    ISeaBattle service = factory.CreateChannel();
        //    game = service;

        //}

        private void button1_Click(object sender, EventArgs e)
        {
            gameCode = tbCode.Text;
            JoinGame();
        }

        private void mainTimer_Tick(object sender, EventArgs e)
        {
            
            string res;
            res = game.Update(gameCode, numOfEnemy);
            lInfo.Text = res;
            if (res == "Not Ready")
            {
                bMove.Enabled = false;
                lInfo.Text = res + $". Your game code is {gameCode}.";
            }

            else if (res == "Another player is moving now")
            {
                bMove.Enabled = false;
                lInfo.Text = res;
                PrintEnemyBoard(game.GetBoard(gameCode, numOfEnemy));
                PrintPlayerBoard(game.GetBoard(gameCode, numOfPlayer));
                if (game.CheckLooser(gameCode) == numOfPlayer)
                {
                    lInfo.Font = new Font("Tobota", 36, FontStyle.Italic);
                    lInfo.Location = new Point(190, 187);
                    lInfo.BackColor = Color.Red;
                    lInfo.Text = $"You are looser!";
                    bMove.Enabled = false;
                    mainTimer.Stop();
                }
            }

            else if (res == "Ready" && game.CheckLooser(gameCode) == -1)
            {
                PrintEnemyBoard(game.GetBoard(gameCode, numOfEnemy));
                PrintPlayerBoard(game.GetBoard(gameCode, numOfPlayer));
                bMove.Enabled = true;
                lInfo.Text = "Your move";
            }

            else if (game.CheckLooser(gameCode) != -1)
            {
                if (game.CheckLooser(gameCode) == numOfEnemy)
                {
                    lInfo.Font = new Font("Tobota", 36, FontStyle.Italic);
                    lInfo.Location = new Point(190, 187);
                    lInfo.BackColor = Color.LightGreen;
                    lInfo.Text = "You have won!!!";
                    bMove.Enabled = false;
                }
                mainTimer.Stop();
            }
        }

        private void PrintPlayerBoard(string[] field)
        {
            for (int i = 0; i < field.Length; i++)
            {
                for (int j = 0; j < field[i].Length; j++)
                {
                    myField[j, i].Text = field[i][j].ToString();
                    if (myField[j, i].Text == "X")
                    {
                        myField[j, i].ForeColor = Color.Red;
                        myField[j, i].BackColor = Color.Black;
                    }
                    else if (myField[j, i].Text == "-")
                    {

                        myField[j, i].ForeColor = Color.White;
                        myField[j, i].BackColor = Color.LightGreen;
                    }
                    else
                    {
                        myField[j, i].ForeColor = Color.LightBlue;
                        myField[j, i].BackColor = Color.LightBlue;
                    }

                }
            }
        }

        private void PrintEnemyBoard(string[] field)
        {
            for (int i = 0; i < field.Length; i++)
            {
                for (int j = 0; j < field[i].Length; j++)
                {
                    enemyField[j, i].Text = field[i][j].ToString();
                    if (enemyField[j, i].Text == "X")
                    {
                        enemyField[j, i].ForeColor = Color.Black;
                        enemyField[j, i].BackColor = Color.Red;
                    }
                    else if (enemyField[j, i].Text == "-")
                    {
                        enemyField[j, i].ForeColor = Color.White;
                        enemyField[j, i].BackColor = Color.Black;
                    }
                    else if (enemyField[j, i].BackColor != Color.Gray)
                    {
                        enemyField[j, i].ForeColor = Color.LightBlue;
                        enemyField[j, i].BackColor = Color.LightBlue;
                    }
                }
            }
        }

        private void bMove_Click(object sender, EventArgs e)
        {
            game.Shot(gameCode, numOfEnemy, code);
            PrintEnemyBoard(game.GetBoard(gameCode, numOfEnemy));
            PrintPlayerBoard(game.GetBoard(gameCode, numOfPlayer));
        }

    }
}
