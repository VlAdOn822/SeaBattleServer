using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattleServer
{
    public class Player
    {
        public Player()
        {
            for(int i = 0; i<Board.Length; i++)
            {
                Board[i] = "__________";
            }
        }

        public string Name { get; set; }
        public string[] Field { get; set; } = new string[10];
        public string[] Board { get; set; } = new string[10];
    }
}
