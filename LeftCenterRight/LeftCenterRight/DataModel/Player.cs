using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeftCenterRight.DataModel
{
    public class Player
    {
        public Player(int id, int chipCount)
        {
            ID = id;
            ChipCount = chipCount;
        }

        public int ID { get; set; }
        public int ChipCount { get; set; }

        public int WinCount {  get; set; }
        public bool IsWinner {  get; set; }
        public string DisplayImage
        { get => IsWinner ? "/LeftCenterRight;component/Image/winner.png" : "/LeftCenterRight;component/Image/nonwinner.png"; }
    }
}
