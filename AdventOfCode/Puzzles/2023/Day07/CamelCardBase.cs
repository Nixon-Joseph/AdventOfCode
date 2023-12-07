using System.Collections.ObjectModel;

namespace AdventOfCode.Puzzles._2023.Day07
{
    internal abstract class CamelCardBase : BaseSolution<List<CamelCardHand>>
    {
        internal override List<CamelCardHand> ReadInputFromFile()
        {
            var lines = File.ReadAllLines(@"./Puzzles/2023/Day07/Input.txt");
            var hands = new List<CamelCardHand>();
            foreach (var line in lines)
            {
                hands.Add(new CamelCardHand(line[..5].ToCharArray(), int.Parse(line[5..])));
            }
            return hands;
        }
    }

    public class CamelCardHand : IComparable
    {
        private static ReadOnlyDictionary<char, int> _cardValues = new ReadOnlyDictionary<char, int>(new Dictionary<char, int>
        {
            { 'A', 14 },
            { 'K', 13 },
            { 'Q', 12 },
            { 'J', 11 },
            { 'T', 10 },
            { '9', 9 },
            { '8', 8 },
            { '7', 7 },
            { '6', 6 },
            { '5', 5 },
            { '4', 4 },
            { '3', 3 },
            { '2', 2 },
        });

        public CamelCardHand(char[] cards, int bid)
        {
            Cards = cards;
            Bid = bid;
        }

        public int Bid { get; set; }
        public char[] Cards { get; set; }

        private Strength _strength = Strength.None;

        public Strength Strength
        {
            get
            {
                if (_strength == Strength.None)
                {
                    _strength = GetStrength();
                }
                return _strength;
            }
            private set { _strength = value; }
        }

        public int CompareTo(object? obj)
        {
            if (obj is CamelCardHand compHand)
            {
                if (compHand.Strength == Strength)
                {
                    for (int i = 0; i < Cards.Length; i++)
                    {
                        if (_cardValues[Cards[i]] > _cardValues[compHand.Cards[i]])
                        {
                            return 1;
                        }
                        else if (_cardValues[Cards[i]] < _cardValues[compHand.Cards[i]])
                        {
                            return -1;
                        }
                    }
                    return 0; // if all are the same.
                }
                else if (compHand.Strength > Strength)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            } 
            else 
            { 
                return 1;
            }
        }

        private Strength GetStrength()
        {
            var groups = Cards.GroupBy(x => x);
            if (groups.Any(x => x.Count() == 5))
            {
                return Strength.FiveOfAKind;
            }
            else if (groups.Any(x => x.Count() == 4))
            {
                return Strength.FourOfAKind;
            }
            else if (groups.Any(x => x.Count() == 3) && groups.Any(x => x.Count() == 2))
            {
                return Strength.FullHouse;
            }
            else if (groups.Any(x => x.Count() == 3))
            {
                return Strength.ThreeOfAKind;
            }
            else if (groups.Count(x => x.Count() == 2) == 2)
            {
                return Strength.TwoPair;
            }
            else if (groups.Any(x => x.Count() == 2))
            {
                return Strength.OnePair;
            }
            else
            {
                return Strength.HighCard;
            }
        }
    }

    public enum Strength
    {
        None = 0,
        HighCard = 1,
        OnePair = 2,
        TwoPair = 3,
        ThreeOfAKind = 4,
        FullHouse = 5,
        FourOfAKind = 6,
        FiveOfAKind = 7,
    }
}
