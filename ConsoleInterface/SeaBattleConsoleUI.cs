using SeaBattleServer;
using System;
using System.Reflection.Emit;

namespace ConsoleInterface
{
    class SeaBattleConsoleUI
    {
        private ISeaBattle game;
        private IFieldReader fieldReader = new FileFieldReader("field.txt");
        string gameCode;
        int num;
        public SeaBattleConsoleUI(ISeaBattle game)
        {
            this.game = game;
        }
        public void Start()
        {
            Console.WriteLine("Enter your name");
            string name = Console.ReadLine();
            string[] field = fieldReader.ReadField();
            Console.WriteLine("Start new game? Or join exiting game? (S/J)");
            string answer;
            answer = Console.ReadLine().ToLower();
            while (answer != "s" && answer != "j")
            {
                Console.WriteLine("Wrong answer, fool. Start new game? Or join exiting game? (S/J)");
                answer = Console.ReadLine().ToLower();
            }
            if (answer == "s")
                StartGame(name, field);
            else JoinGame(name, field);
            Console.WriteLine($"Your num is {num}");
            string res;

            do
            {
                res = game.Update(gameCode, num);
                while (res != "Ready")
                {
                    Console.WriteLine(res);
                    Console.WriteLine("Try again later");
                    Console.ReadLine();
                    res = game.Update(gameCode, num);
                }
                while (res == "Ready"&& game.CheckWinner(gameCode) == -1)
                {
                    PrintBoard(game.GetBoard(gameCode, num));
                    Console.WriteLine("Your move?");
                    string code = Console.ReadLine();
                    Console.WriteLine(game.Shot(gameCode, num, code));
                    res = game.Update(gameCode, num);
                }
            } while (game.CheckWinner(gameCode) == -1);
            if (game.CheckWinner(gameCode) == num)
            {
                Console.WriteLine("You have won!!!");
            }
            else
            {
                Console.WriteLine("You r looser");
            }
        }


        private void PrintBoard(string[] board)
        {
            Console.Write("    ");
            for (int i = 0; i < 10; i++)
            {
                Console.Write($"{(char)(i + 'a')} ");
            }
            Console.WriteLine();
            Console.WriteLine("  ----------------------");
            for (int i = 0; i < 10; i++)
            {
                Console.Write($"{i}|  ");
                for (int j = 0; j < 10; j++)
                {
                    Console.Write($"{board[i][j]}{board[i][j]}");
                }
                Console.WriteLine();
            }
        }

        private void JoinGame(string name, string[] field)
        {
            Console.WriteLine("Enter game code");
            gameCode = Console.ReadLine();
            num = game.JoinGame(gameCode, name, field);
            while (num == -1)
            {
                Console.WriteLine("This game isn't exist or has no free places! Enter another game code.");
                gameCode = Console.ReadLine();
                if (gameCode.ToLower() == "exit")
                    return;
                num = game.JoinGame(gameCode, name, field);
            }
        }

        private void StartGame(string name, string[] field)
        {
            gameCode = game.StartGame();
            Console.WriteLine($"This game has created. Your game code is {gameCode}.");
            num = game.JoinGame(gameCode, name, field);
        }
    }

}
