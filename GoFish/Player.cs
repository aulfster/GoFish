﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoFish
{
    class Player
    {
        private string name;
        public string Name { get { return name; } }
        private Random random;
        private Deck cards;
        private TextBox textBoxOnForm;

        public Player(String name, Random random, TextBox textBoxOnForm)
        {
            // The constructor for the Player class initializes four private fields, and then
            // adds a line to the TextBox control on the form that says, "Joe has just 
            // joined the game"—but use the name in the private field, and don't forget to
            // add a line break at the end of every line you add to the TextBox.

            this.name = name;
            this.random = random;
            this.cards = new Deck(0);
            this.textBoxOnForm = textBoxOnForm;

            this.textBoxOnForm.Text += this.name + " has just joined the game" + Environment.NewLine;
        }

        public IEnumerable<Values> PullOutBooks() {
            List<Values> books = new List<Values>();
            for (int i = 1; i <= 13; i++)
            {
                Values value = (Values)i;
                int howMany = 0;
                for (int card = 0; card < cards.Count; card++)
                    if (cards.Peek(card).Value == value)
                        howMany++;
                if (howMany == 4)
                {
                    books.Add(value);
                    cards.PullOutValues(value);
                }
            }
            return books;
        } 

        public Values GetRandomValue()
        {
            // This method gets a random value—but it has to be a value that's in the deck!

            Random r = new Random();
            int temp = r.Next(1, 14);
            return (Values)temp;
        }

        public Deck DoYouHaveAny(Values value)
        {
            // This is where an opponent asks if I have any cards of a certain value
            // Use Deck.PullOutValues() to pull out the values. Add a line to the TextBox
            // that says, "Joe has 3 sixes"— use the new Card.Plural() static method

            Deck temp = cards.PullOutValues(value);
            textBoxOnForm.Text += this.name + " has " + temp.Count + " " + Card.Plural(value) + Environment.NewLine;
            return temp;
        }

        public void AskForACard(List<Player> players, int myIndex, Deck stock)
        {
            // Here's an overloaded version of AskForACard()—choose a random value
            // from the deck using GetRandomValue() and ask for it using AskForACard()

            AskForACard(players, myIndex, stock, GetRandomValue());
        }
        public void AskForACard(List<Player> players, int myIndex, Deck stock, Values value)
        {
            // Ask the other players for a value. First add a line to the TextBox: "Joe asks
            // if anyone has a Queen". Then go through the list of players that was passed in
            // as a parameter and ask each player if he has any of the value (using his 
            // DoYouHaveAny() method). He'll pass you a deck of cards—add them to my deck.
            // Keep track of how many cards were added. If there weren't any, you'll need
            // to deal yourself a card from the stock (which was also passed as a parameter),
            // and you'll have to add a line to the TextBox: "Joe had to draw from the stock"

            textBoxOnForm.Text += this.name + " asks if anyone has a " + value.ToString() + Environment.NewLine;
            int numberOfCards = 0;
            int flag = 0;

            int counter = 0;
            foreach(Player player in players)
            {
                if (counter != myIndex)
                {
                    Deck temp = player.DoYouHaveAny(value);
                    if (temp.Count > 0)
                    {
                        flag = 1;
                        numberOfCards += temp.Count;
                        for (int i = 0; i < temp.Count; i++)
                        {
                            cards.Add(temp.Deal(i));
                        }
                    }
                }
                counter++;
            }

            if (flag == 0)
            {
                cards.Add(stock.Deal());
                numberOfCards += stock.Count;
                textBoxOnForm.Text += this.name + " had to draw from the stock" + Environment.NewLine;
            }

        }
        // Here's a property and a few short methods that were already written for you

        public int CardCount { get { return cards.Count; } }

        public void TakeCard(Card card) { cards.Add(card); }

        public IEnumerable<string> GetCardNames() { return cards.GetCardNames(); }

        public Card Peek(int cardNumber) { return cards.Peek(cardNumber); }

        public void SortHand() { cards.SortByValue(); }

    }
}
