using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWIXX
{
   public class ScoreCard
    {
        string Name { get; }
        
        Row RedRow { get; set; }
        Row YellowRow { get; set; }
        Row GreenRow { get; set; }
        Row BlueRow { get; set; }

        int PassXCount { get; set; }
        bool isPassLockedOut { get; set; }

        public ScoreCard(string playerName)
        {
            Name = playerName;

            RedRow = new Row("Red");
            YellowRow = new Row("Yellow");
            GreenRow = new Row("Green");
            BlueRow = new Row("Blue");

            PassXCount = 0;
            isPassLockedOut = false;
        }

        public void Run()
        {
            Console.Clear();
            Console.WriteLine($"SCORE CARD FOR: {Name}");
            Console.WriteLine("---------------------------------------------");
            Print(RedRow);
            Print(YellowRow);
            Print(GreenRow);
            Print(BlueRow);
            Console.WriteLine($" {PassXCount} out of 4 passes used");
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine();
            //Console.WriteLine($" Red Score: {RedRow.Score()}, Yellow Score: {YellowRow.Score()}, Green Score: {GreenRow.Score()}, Blue Score: {BlueRow.Score()}");
            Console.WriteLine($" Running total: {TotalScore()}");
            Console.WriteLine();
            Console.WriteLine("Make your move.");
            Console.Write("Color? (r) (y) (g) (b) : ");
            string color = (Console.ReadLine()).ToLower();
            bool isValid = false;
            switch (color)
            {
                case "r":
                    isValid = true;
                    break;
                case "y":
                    isValid = true;
                    break;
                case "g":
                    isValid = true;
                    break;
                case "b":
                    isValid = true;
                    break;
            }

            if (isValid)
            {
                Console.Write("Number? : ");
                int number = int.Parse(Console.ReadLine());
                Mark(color, number);
            }
            else
            {
                Console.Write("invalid Color selection. please try again.");
                Console.ReadLine();
                Run();
            }        
        }

        public void Print(Row row)
        {
            Console.WriteLine($" {row.Color}: {row.Score()} pts");
            Console.Write("  ");
            foreach(KeyValuePair<int, string> variable in row.ScoreTracker)
            {
                int slot = variable.Key;
                string value = variable.Value;
                Console.Write($" {value} ");

            }
            Console.WriteLine();
            Console.WriteLine();

              
        }

        public void Mark(string color, int number)
        {
           
            Dictionary<int, string> thisScoreTracker = new Dictionary<int, string>();
           
            switch(color)
            {
                case "r":
                    thisScoreTracker = RedRow.ScoreTracker; 
                    break;
                case "y":
                    thisScoreTracker = YellowRow.ScoreTracker;
                    break;
                case "g":
                    thisScoreTracker = GreenRow.ScoreTracker;
                    break;
                case "b":
                    thisScoreTracker = BlueRow.ScoreTracker;
                    break;
            }

            bool isValid = false;
            try
            {
                isValid = thisScoreTracker[number] == number.ToString();
            }
            catch(ArgumentNullException ex)
            {
                Console.WriteLine();
                Console.WriteLine($"Invalid entry, please try again.");
                Console.WriteLine("press enter to return: ");
                Console.ReadLine();

            }
            catch(KeyNotFoundException ex)
            {
                Console.WriteLine();
                Console.WriteLine($"Invalid entry, please try again.");
                Console.WriteLine("press enter to return: ");
                Console.ReadLine();
            }

            if(isValid && (color == "r" || color == "y"))
            {
                thisScoreTracker[number] = "X";
                for (int i = number-1; i>=2; i--)
                {
                    if(!(thisScoreTracker[i] == "X" || thisScoreTracker[i] == "_"))
                    {
                        thisScoreTracker[i] = "_";
                    }
                    
                }

            }
            else if(isValid && (color == "g" || color == "b"))
            {
                thisScoreTracker[number] = "X";
                for (int i = number + 1; i <= 12; i++)
                {
                    if (!(thisScoreTracker[i] == "X" || thisScoreTracker[i] == "_")) 
                    {
                        thisScoreTracker[i] = "_";
                    }             
                }
            }
            Run();
        }

        public int TotalScore()
        {
            return (RedRow.Score() + YellowRow.Score() + GreenRow.Score() + BlueRow.Score());
        }
   }
}
