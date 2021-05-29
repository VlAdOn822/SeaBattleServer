using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattleServer
{
    public class Game : ISeaBattle
    {
        public IRepository Repository(string gameCode)
        {
            return new TxtFileRepository($"game{gameCode}.txt");
        }

        public int CheckLooser(string gameCode)
        {
            Player[] players = Repository(gameCode).ReadPlayers();
            for (int i = 0; i < players.Length; i++)
            {
                int count = 0;
                foreach (var row in players[i].Board)
                {
                    foreach (var x in row)
                    {
                        if (x == 'X') count++;
                        if (count >= 20) return i;
                    }
                }
            }
            return -1;
        }

        public string[] GetBoard(string gameCode, int numPlayer)
        {

            Player[] players = Repository(gameCode).ReadPlayers();
            return players[numPlayer].Board;
        }

        public int JoinGame(string gameCode, string playerName, string[] field)
        {
            try
            {
                Player pl = new Player() { Name = playerName };
                pl.Field = field;
                return Repository(gameCode).AddPlayer(pl);
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public string Shot(string gameCode, int numPlayer, string code)
        {
            try
            {
                Player[] players = Repository(gameCode).ReadPlayers();
                string[] field = players[numPlayer == 0 ? 1 : 0].Field;
                int i = Convert.ToInt32(code[1].ToString());
                int j = code[0] - 'a';
                var old = players[numPlayer].Board[i].ToCharArray();
                old[j] = field[i][j];
                players[numPlayer].Board[i] = new string(old);
                Repository(gameCode).WritePlayers(players);
                if (field[i][j] == 'X')
                {
                    return "Hit";
                }
                else
                {
                    Repository(gameCode).WriteNumActivePlayer(numPlayer == 0 ? 1 : 0);
                    return "Miss";

                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string StartGame()
        {
            try
            {
                Random rnd = new Random();
                string gameCode;
                do
                {
                    gameCode = rnd.Next(1000).ToString();
                } while (Repository(gameCode).NewGame() != "Ok");
                return gameCode;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string Update(string gameCode, int numPlayer)
        {
            try
            {
                Player[] players = Repository(gameCode).ReadPlayers();
                for (int i = 0; i < 2; i++)
                {
                    if (players[i] == null || players[i].Name == null)
                        return "Not Ready";
                }
                int numActive = Repository(gameCode).ReadNumActivePlayer();
                if (numActive == numPlayer)
                    return "Ready";
                else
                    return "Another player is moving now";

            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
