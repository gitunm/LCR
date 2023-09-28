using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeftCenterRight.DataModel
{
    public class PresetGame
    {
        public PresetGame(int numberofplayers, int numberofgames)
        {
            NumberOfPlayers = numberofplayers;
            NumberOfGames = numberofgames;
        }

        public int NumberOfPlayers { get; set; }
        public int NumberOfGames { get; set; }

        public override string ToString()
        {
            return $"{NumberOfPlayers} Players x {NumberOfGames} Games";
        }

    }
}
