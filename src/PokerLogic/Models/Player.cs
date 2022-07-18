using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace PokerLogic.Models
{
    public class Player
    {
        public string Name { get; }
        public bool IsDealer { get; set; }
        public List<int> HoldingCard {get; }
        public int Point { get; private set;}

        public Player(string name)
        {
            Name = name;
            IsDealer = false;
            HoldingCard = new List<int>();
        }

        public void SetAsDealer()
        {
            IsDealer = true;
        }

        public void AddCard(int card)
        {
            HoldingCard.Add(card);
        }

        public int NumberOfHoldingCard()
        {
            return HoldingCard.Count();
        }

        public void ModifyPoint(int point)
        {
            Point += point;
        }

        public string ShowHoldingCard()
        {
            var stringBuilder = new StringBuilder();

            foreach(var card in HoldingCard)
                stringBuilder.AppendFormat("{0}, ", PokerCards.ShowCard(card));
            
            stringBuilder.Length -= 2;

            return stringBuilder.ToString();
        }

        public override string ToString()
        {
            return Name;
        }
    }    
}