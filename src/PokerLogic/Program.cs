using System.Collections.Generic;
using System;
using PokerLogic.Models;
using PokerLogic.GameRules;
using System.Linq;

namespace PokerLogic
{
    class Program
    {
        static void Main(string[] args)
        {
            GameRuleStrategy gameStrategy;

            Console.WriteLine("Select game");
            Console.WriteLine("=================");
            Console.WriteLine("1. Blackjack");


            var testPlayer = new List<Player>()
            {
                new Player("Wan 01"),
                new Player("Wan 02")
            };
            gameStrategy = new BlackjackStrategy(testPlayer);
            
            gameStrategy.StartGame();

            Console.WriteLine("End of the game");
            Console.Read();

            // Console.Write("Any new player ? (Y/N): ");
            // var selection = Console.ReadKey().KeyChar;
            // Console.WriteLine();
            // Console.WriteLine(char.ToUpper(selection));
        }
    }
}
