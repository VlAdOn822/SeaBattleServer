using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattleServer
{
    public interface IRepository
    {
        Player[] ReadPlayers();
        int ReadNumActivePlayer();
        void WritePlayers(Player[] players);
        void WriteNumActivePlayer(int num);
        int AddPlayer(Player player);
        /// <summary>
        /// Create new game repository
        /// </summary>
        /// <returns>if new game has created "ok" else "fail"</returns>
        string NewGame();
    }
}
