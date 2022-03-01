using System;

namespace QWIXX
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("LETS START RECORDING A GAME OF QWIXX");
            Console.Write("Enter Your Name: ");
            string name = Console.ReadLine();

            ScoreCard scoreCard = new ScoreCard(name);
            scoreCard.Run();
        }
    }
}
