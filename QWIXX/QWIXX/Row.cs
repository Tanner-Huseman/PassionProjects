using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWIXX
{
   public class Row
    {
        public String Color { get; }
        public Dictionary<int, string> ScoreTracker { get; set; }
        public bool IsLockedOut { get; set; }
        public int XCount
        {
            get
            {
                int result = 0;
                foreach (KeyValuePair<int, string> element in ScoreTracker)
                {
                    if(element.Value == "X")
                    {
                        result++;
                    }
                }
                return result;
            }
        
        }

        public Row(string color)
        {
            Color = color;
            if(color == "Red" || color == "Yellow")
            {
                ScoreTracker = new Dictionary<int, string>();
                ScoreTracker[2] = "2";
                ScoreTracker[3] = "3";
                ScoreTracker[4] = "4";
                ScoreTracker[5] = "5";
                ScoreTracker[6] = "6";
                ScoreTracker[7] = "7";
                ScoreTracker[8] = "8";
                ScoreTracker[9] = "9";
                ScoreTracker[10] = "10";
                ScoreTracker[11] = "11";
                ScoreTracker[12] = "12";
                ScoreTracker[00] = "00";
            }
            else if(color == "Green" || color == "Blue")
            {
                ScoreTracker = new Dictionary<int, string>();
                ScoreTracker[12] = "12";
                ScoreTracker[11] = "11";
                ScoreTracker[10] = "10";
                ScoreTracker[9] = "9";
                ScoreTracker[8] = "8";
                ScoreTracker[7] = "7";
                ScoreTracker[6] = "6";
                ScoreTracker[5] = "5";
                ScoreTracker[4] = "4";
                ScoreTracker[3] = "3";
                ScoreTracker[2] = "2";
                ScoreTracker[00] = "00";
            }
            

            IsLockedOut = false;
        }

        public int Score()
        {
            int result = 0;

            result = (XCount * (XCount + 1)) / 2;

            return result;
        }



    }
}
