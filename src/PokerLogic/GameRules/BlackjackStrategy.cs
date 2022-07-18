using PokerLogic.Models;
using System.Collections.Generic;
using System;
using System.Linq;

namespace PokerLogic.GameRules
{
    // how to start
    // rule 1: can not over 21
    // rule 2: must be over 16
    // rule 3: if dealer card bigger than player, dealer win
    // rule 4: if player card is 21, can direct show card
    // how to end

    // Get winner
    // Get card value
    // Description
    // Set dealer & player
    public class BlackjackStrategy : GameRuleStrategy
    {
        private PokerCards _deskCard;
        public Player _dealer { get; private set;}
        public List<Player> _players { get; private set; }
        private const int _maxPlayer = 5;
        private const int _minPlayer = 2;

        public BlackjackStrategy()
        {
            _deskCard = new PokerCards();
        }

        public BlackjackStrategy(List<Player> Players)
        {
            _deskCard = new PokerCards();
            _players = Players;
        }

        public BlackjackStrategy(Player Dealer, List<Player> Players)
        {
            _deskCard = new PokerCards();
            _dealer = Dealer;
            _players = Players;
        }

        public void StartGame()
        {
            SetupGame();

            // Start the game
            // All Player play first
            foreach(var player in _players)
            {
                PlayerPlays(player);
            }

            // Dealer turn
            DealerPlay(_dealer);

            // Show game result
            var dealerCardValue = GetCardValue(_dealer.HoldingCard);
            var playerCardValue = 0;
            int gameResult;

            foreach (var player in _players)
            {
                playerCardValue = GetCardValue(player.HoldingCard);

                Console.WriteLine($"Player {player.ToString()}");
                Console.WriteLine($"Dealer Card: {dealerCardValue}, Player Card: {playerCardValue}");

                gameResult = IsPlayerWin(dealerCardValue, playerCardValue);
                Console.WriteLine(ShowGameResult(gameResult));
                CalculatePlayerPoint(player, gameResult);
            }
        }

        public void RestartGame()
        {
            _deskCard = new PokerCards();
        }
        
        public void GameDescription()
        {
            Console.WriteLine("Blackjack");
            Console.WriteLine("=====================");
            Console.WriteLine("1. Must have at lease 1 player & 1 dealer.");
            Console.WriteLine("2. The game start by giving two cards to player & dealer.");
            Console.WriteLine("3. Player can decide whether hit or stay.");
            Console.WriteLine("4. If player cards exceed 21, player lose the game.");
            Console.WriteLine("5. After all done their turn, dealer draw card until exceed 16.");
            Console.WriteLine("6. If dealer total less than player, player win, else player loss.");
        }
        
        public int GetCardValue(List<int> cards)
        {
            var cardList = new List<string>();
            var cardValue = 0;

            foreach(var card in cards)
            {
                var cardName = PokerCards.ShowCard(card);

                var cardNumber = cardName.Split("_")[0];
                if (cardNumber == "A")
                    cardList.Add(cardNumber);
                else
                    cardList.Insert(0, cardNumber);
            }

            var numberOfCard = cardList.Count();
            foreach (var card in cardList)
            {
                switch (card.ToUpper())
                {
                    case "J":
                    case "Q":
                    case "K":
                        cardValue += 10;
                        break;
                    case "A":
                        if (numberOfCard <= 2 && cardValue <= 10)
                            cardValue += 11;
                        else if (numberOfCard <= 3 & cardValue <= 11)
                            cardValue += 10;
                        else
                            cardValue += 1;
                        break;
                    default:
                        cardValue += Convert.ToInt32(card);
                        break;
                }
            }

            return cardValue;
        }

        // Setup user & player
        // Distribute card
        private void SetupGame(){

            if (_dealer == null || _players == null)
            {
                SetupPlayerAndDealer();                
            }

            // distribute cards
            DistributeCard();
        }

        // Enter dealer information
        // Enter player information
        private void SetupPlayerAndDealer()
        {
            int currentPlayer;
            if (_players == null)
            {
                _players = new List<Player>();
                currentPlayer = 0;
            }
            else
                currentPlayer = _players.Count();

            // Enter player information
            if (currentPlayer < _minPlayer)
            {
                char selection = 'Y';

                do {
                    Console.WriteLine();
                    Console.Write("Enter player name: ");
                    var playerName = Console.ReadLine();
                    _players.Add(new Player(playerName));
                    currentPlayer += 1;

                    while (currentPlayer >= _minPlayer && currentPlayer < _maxPlayer)
                    {
                        Console.Write("Any new player ? (Y/N): ");
                        selection = Console.ReadKey().KeyChar;
                        Console.WriteLine();

                        if (char.ToUpper(selection) == 'Y' || char.ToUpper(selection) == 'N')
                            break;
                        else
                            Console.WriteLine("Kindly enter Y or N only.");
                    }

                } while (currentPlayer < _maxPlayer && char.ToUpper(selection) == 'Y');

                // Next line
                Console.WriteLine();
            }

            // Enter dealer information
            if (_dealer == null)
            {
                Console.WriteLine("Kindly select one player as dealer:");
                
                var number = 1;
                foreach(var player in _players)
                {
                    Console.WriteLine($"{number}. {player}");
                    number++;
                }

                bool isDigit = false;
                int deallerNumber;
                do {
                    Console.Write(">>> ");
                    var userInput = Console.ReadLine();

                    isDigit = Int32.TryParse(userInput, out deallerNumber);

                    if (!isDigit)
                        Console.WriteLine("Kindly key in digit only.\n");
                    else if (deallerNumber <= 0 || deallerNumber >= number)
                    {
                        Console.WriteLine("Kindly key in digit from the list.\n");
                        isDigit = false;
                    }

                } while(!isDigit);

                _dealer = _players[deallerNumber - 1];
                _dealer.SetAsDealer();
                _players.Remove(_dealer);
            }
        }

        // Each player have 2 card
        // Dealer will be the last get the card
        private void DistributeCard()
        {
            for(var round = 0; round < 2; round++){
                foreach(var player in _players)
                {
                    player.AddCard(_deskCard.DrawCard());
                }

                _dealer.AddCard(_deskCard.DrawCard());
            }
        }

        private void PlayerPlays(Player player)
        {
            ShowPlayerCard(player);

            var playerChoice = 0;
            while ( playerChoice != 2 && 
                    player.NumberOfHoldingCard() < 5 && 
                    GetCardValue(player.HoldingCard) < 21)
            {
                Console.WriteLine("Player options:");
                Console.WriteLine("1. Draw card");
                Console.WriteLine("2. Pass");

                Console.Write(">>> ");
                if(!Int32.TryParse(Console.ReadLine(), out playerChoice))
                    playerChoice = 0;
                Console.WriteLine();
            
                switch (playerChoice)
                {
                    case 1:
                        player.AddCard(_deskCard.DrawCard());
                        ShowPlayerCard(player);
                        break;
                    case 2:
                        break;
                    default:
                        Console.WriteLine("Please choose from the list.");
                        break;
                }
            }
        }

        private void DealerPlay(Player dealer)
        {
            ShowPlayerCard(dealer);
            while (GetCardValue(dealer.HoldingCard) < 16)
            {
                dealer.AddCard(_deskCard.DrawCard());
                ShowPlayerCard(dealer);
            } 
        }

        private void ShowPlayerCard(Player player)
        {
            Console.WriteLine(string.Format("Show {0} card", player.IsDealer ? "dealer" : "player"));
            Console.WriteLine("===================");
            Console.WriteLine(player.ShowHoldingCard());
            Console.WriteLine($"Card total: {GetCardValue(player.HoldingCard)}");
            Console.WriteLine();
        }

        // return 1 if win
        // return 0 if draw
        // return -1 if lose
        private int IsPlayerWin(int dealerCardValue, int playerCardValue)
        {
            // winning rules            
            if (dealerCardValue > 21 && playerCardValue > 21)
                return 0;
            else if (dealerCardValue > 21)
                return 1;
            else if (playerCardValue > 21)
                return -1;
            else 
                return playerCardValue.CompareTo(dealerCardValue);;
        }

        private string ShowGameResult(int GameResult)
        {
            if (GameResult == 0)
                return "Dealer & player draw.";
            else if (GameResult >= 1)
                return "The player had win.";
            else
                return "The player lose.";
        }

        private void CalculatePlayerPoint(Player player, int GameResult)
        {
            if (GameResult <= -1)
                player.ModifyPoint(-10);
            else if (GameResult >= 1)
                player.ModifyPoint((player.NumberOfHoldingCard() == 2 && GetCardValue(player.HoldingCard) == 21) ? 15 : 10);
        }
    }
}