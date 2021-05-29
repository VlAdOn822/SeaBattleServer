using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattleServer
{
    class TxtFileRepository : IRepository

    {
        private string filename;

        public TxtFileRepository(string filename)
        {
            this.filename = filename;
        }

        public int AddPlayer(Player player)
        {
            Player[] players = ReadPlayers();
            int count = players.Count(p => p!= null &&p.Name!= null);
            if(count<2)
            {
                int num =players[0] == null|| players[0].Name == null ? 0 : 1;
                players[num] = player;
                WritePlayers(players);
                WriteNumActivePlayer(num);
                return num;
            }
            return -1;
        }

        public string NewGame()
        {
            try
            {
                if (File.Exists(filename))
                    return "Fail";
                File.Create(filename).Close();
                return "Ok";
            }
            catch(Exception e)
            {
                return e.Message;
            }
        }

        public int ReadNumActivePlayer()
        {
            StreamReader sr = new StreamReader(filename);
            string[] all = sr.ReadToEnd().Split('\n');
            sr.Close();
            foreach (var item in all)
            {
                if (item.StartsWith("NumActivePlayer"))
                    return Convert.ToInt32(item.Trim().Substring(15));
            }
            return -1;
        }

        public Player[] ReadPlayers()
        {
            Player[] players = new Player[2];
                StreamReader sr = new StreamReader(filename);
                string[] all = sr.ReadToEnd().Split('\n');
                sr.Close();
                foreach (var item in all)
                {
                    if (item.StartsWith("Player"))
                    {
                        int num = Convert.ToInt32(item.Substring(6, 1));
                        if(players[num]==null)
                        players[num] = new Player();
                        string key = item.Substring(7, 5);
                        if (key == "Field")
                        {
                            int ind = Convert.ToInt32(item.Substring(12, 1));
                            players[num].Field[ind] = item.Trim().Substring(13);
                        }
                        if (key == "Board")
                        {
                            int ind = Convert.ToInt32(item.Substring(12, 1));
                            players[num].Board[ind] = item.Trim().Substring(13);
                        }
                        if (key == "Name:")
                        {
                            players[num].Name = item.Trim().Substring(12);
                        }
                    }
                }
                return players;
        }

        public void WriteNumActivePlayer(int num)
        {
            bool found = false;
            string[] all = { };
            if (File.Exists(filename))
            {
                StreamReader sr = new StreamReader(filename);
                all = sr.ReadToEnd().Split('\n');
                sr.Close();
                for (int i = 0; i < all.Length; i++)
                {
                    if (all[i].StartsWith("NumActivePlayer"))
                    {
                        all[i] = $"NumActivePlayer{num}";
                        found = true;
                    }

                }
            }
                StreamWriter sw = new StreamWriter(filename);
                if (!found)
                    sw.WriteLine($"NumActivePlayer{num}");
                foreach (var item in all)
                {
                    sw.WriteLine(item.Trim());
                }
                sw.Close();
            
        }

        public void WritePlayers(Player[] players)
        {
            int numActive = ReadNumActivePlayer();
            StreamWriter sw = new StreamWriter(filename);
            sw.WriteLine($"NumActivePlayer{numActive}");
            for(int j=0; j<players.Length; j++)
            {
                Player player = players[j];
                if (player != null)
                {
                    sw.WriteLine($"Player{j}Name:{player.Name}");
                    for (int i = 0; i < player.Field.Length; i++)
                    {
                        sw.WriteLine($"Player{j}Field{i}{player.Field[i]}");
                    }
                    for (int i = 0; i < player.Board.Length; i++)
                    {
                        sw.WriteLine($"Player{j}Board{i}{player.Board[i]}");
                    }
                }
            }
            sw.Close();

        }
    }
}
