using System.Collections.Generic;
using System;
using System.Linq;

namespace PokerLogic.Models
{
    public class PokerCards
    {
        private static readonly string[] _cardNumber = {"2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A"};
        private static readonly string[] _cardSymbol = {"spades", "hearts", "clubs", "diamonds"};
        private Queue<int> _deskCard;


        public PokerCards()
        {
            SufferDeskCark();
        }

        public void SufferDeskCark()
        {
            // create new set of card if it is a new game
            // else using those on the deskCard
            int[] cardIndex;
            if(_deskCard == null)
            {
                cardIndex = Enumerable.Range(0, 52).ToArray();
                _deskCard = new Queue<int>();
            }
            else
            {
                cardIndex = _deskCard.ToArray();
                _deskCard.Clear();
            }
            
            // Start suffering
            var rand = new Random();
            var numberOfCardIndex = cardIndex.Count();
            for(var i = 0 ; i < numberOfCardIndex ; i++)
            {
                int randomNumber = rand.Next(numberOfCardIndex - i);

                var temp = cardIndex[randomNumber];
                cardIndex[randomNumber] = cardIndex[i];
                cardIndex[i] = temp;
            }

            // Add back to deskCard
            foreach(var index in cardIndex)
            {
                _deskCard.Enqueue(index);
            }
        }

        public int DrawCard()
        {
            return _deskCard.Dequeue();
        }

        public static string ShowCard(int cardIndex)
        {
            if (cardIndex < 0 || cardIndex > 51)
            {
                throw new Exception("Index out of bound");
            }

            var cardNumberIndex = (int)cardIndex / 4;
            var cardShapeIndex = cardIndex - (cardNumberIndex * 4);

            return $"{_cardNumber[cardNumberIndex]}_{_cardSymbol[cardShapeIndex]}";
        }
    }    
}