using SeaBattleServer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsUI
{
    class SeaBattleWFUI
    {
        private ISeaBattle game;
        public string name;
        private IFieldReader fieldReader = new FileFieldReader("field.txt");
        string gameCode;
        int num;
        Label infocode;
        Timer aTimer;
        public SeaBattleWFUI(ISeaBattle game, string name, string gameCode)
        {
            this.game = game;
            this.name = name;
            this.gameCode = gameCode;
        }
        public SeaBattleWFUI(ISeaBattle game, string name)
        {
            this.game = game;
            this.name = name;
        }

        public void Start()
        {
            string[] field = fieldReader.ReadField();
            if (gameCode == null)
                StartGame(name, field);
            else JoinGame(name, field, gameCode);
            aTimer = new Timer();
            aTimer.Interval = 2000;
            aTimer.Start();
            PrintBoard(game.GetBoard(gameCode, num));
            aTimer.Tick += new EventHandler(this.aTimer_Tick);
            infocode.Text = "";
        }

        private void aTimer_Tick(object sender, EventArgs e)
        {

            string res;
            res = game.Update(gameCode, num);
            if (game.CheckWinner(gameCode) == -1)
            {
                if (res != "Ready")
                {
                    infocode.Text = "You have not any enemy now or its not your turn to move.";
                    infocode.Size = new Size(infocode.Text.Length * 6, 15);
                    res = game.Update(gameCode, num);
                }
                if (res == "Ready")
                {
                    PrintBoard(game.GetBoard(gameCode, num));
                    infocode.Text = "Your move";
                    infocode.Size = new Size(infocode.Text.Length * 6, 15);
                    res = game.Update(gameCode, num);
                }
            }
            if (game.CheckWinner(gameCode) != -1)
            {
                if (game.CheckWinner(gameCode) == num)
                {
                    Console.WriteLine("You have won!!!");
                }
                else
                {
                    Console.WriteLine("You r looser");
                }
            }
        }



        private void PrintBoard(string[] board)
        {
            Panel myPanel = new Panel();
            myPanel.Location = new Point(20, 50);
            myPanel.Size = new Size(360, 360);
            myPanel.BorderStyle = BorderStyle.Fixed3D;
            Form.ActiveForm.Controls.Add(myPanel);
            Label myLabel = new Label();
            myLabel.Text = $"Your panel";
            myLabel.Location = new Point(16, 18);
            Form.ActiveForm.Controls.Add(myLabel);

            Panel enemyPanel = new Panel();
            enemyPanel.Location = new Point(427, 50);
            enemyPanel.Size = new Size(360, 360);
            enemyPanel.BorderStyle = BorderStyle.Fixed3D;
            Form.ActiveForm.Controls.Add(enemyPanel);
            Label enemyLabel = new Label();
            enemyLabel.Text = $"Your enemy panel";
            enemyLabel.Location = new Point(423, 18);
            Form.ActiveForm.Controls.Add(enemyLabel);

            int mySize = myPanel.Width / 10;
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    Label label = new Label();
                    label.Size = new Size(mySize, mySize);
                    label.Location = new Point(x * mySize, y * mySize);
                    label.BorderStyle = BorderStyle.FixedSingle;
                    myPanel.Controls.Add(label);
                }
            }

            int enemySize = enemyPanel.Width / 10;
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    Label label = new Label();
                    label.Size = new Size(enemySize, enemySize);
                    label.Location = new Point(x * enemySize, y * enemySize);
                    label.BorderStyle = BorderStyle.FixedSingle;
                    enemyPanel.Controls.Add(label);
                    label.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PlayerShoot);
                }
            }
        }

        private void PlayerShoot(object sender, MouseEventArgs e)
        {
            if(infocode.Text == "Your move")
            {

            }

        }

        private void JoinGame(string name, string[] field, string gameCode)
        {
            
            num = game.JoinGame(gameCode, name, field);
            while (num == -1)
            {
                Label errorLabel = new Label();
                errorLabel.Text = "This game isn't exist or has no free places! Enter another game code.";
                errorLabel.Location = new Point(423, 18);
                errorLabel.Size = new Size(errorLabel.Text.Length * 6, 15);
                Form.ActiveForm.Controls.Add(errorLabel);

            }
        }

        private void StartGame(string name, string[] field)
        {
            gameCode = game.StartGame();
            infocode = new Label();
            infocode.Text = $"This game has created. Your game code is {gameCode}.";
            infocode.Location = new Point(320, 5);
            infocode.Size = new Size(infocode.Text.Length * 6, 15);
            Form.ActiveForm.Controls.Add(infocode);
            num = game.JoinGame(gameCode, name, field);
        }
    }
    
}
