using System.Collections.ObjectModel;

namespace AdventOfCode.Puzzles._2023.Day07
{
    internal abstract class CamelCardBase : BaseSolution<List<CamelCardHand>>
    {
        private readonly bool _jokerMode;

        public CamelCardBase(bool jokerMode)
        {
            _jokerMode = jokerMode;
        }

        internal override List<CamelCardHand> ReadInputFromFile()
        {
            var lines = File.ReadAllLines(@"./Puzzles/2023/Day07/Input.txt");
            var hands = new List<CamelCardHand>();
            foreach (var line in lines)
            {
                hands.Add(new CamelCardHand(line[..5].ToCharArray(), int.Parse(line[5..]), _jokerMode));
            }
            return hands;
        }
    }

    public class CamelCardHand : IComparable
    {
        /// <summary>
        /// The default value of each card.
        /// </summary>
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

        private const char JOKER = 'J';

        /// <summary>
        /// When enabled, 'J' is treated as a wildcard and can be used to complete any hand.
        /// </summary>
        private readonly bool _jokerMode;

        public CamelCardHand(char[] cards, int bid, bool jokerMode)
        {
            Cards = cards;
            Bid = bid;
            _jokerMode = jokerMode;
        }

        /// <summary>
        /// Base number of points bid for this hand.
        /// </summary>
        public int Bid { get; set; }
        /// <summary>
        /// Collection of cards in the hand.
        /// </summary>
        public char[] Cards { get; set; }

        /// <summary>
        /// the hand's strength for comparing to other hands.
        /// </summary>
        private Strength _strength = Strength.None;

        public Strength Strength
        {
            get
            {
                // Lazy load the strength.
                if (_strength == Strength.None) { _strength = GetStrength(); }
                return _strength;
            }
            private set { _strength = value; }
        }

        /// <summary>
        /// Return value of card. If in joker mode, 'J' is treated as a wildcard and returns 1.
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        private int GetCardValue(char card) => _jokerMode && card == JOKER ? 1 : _cardValues[card];

        /// <summary>
        /// Custom CompareTo override thand handles comparing hands based on strength and card values.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object? obj)
        {
            if (obj is CamelCardHand compHand)
            {
                var handComp = Strength.CompareTo(compHand.Strength);
                if (handComp == 0)
                {
                    for (int i = 0; i < Cards.Length; i++)
                    {
                        var comp = GetCardValue(Cards[i]).CompareTo(GetCardValue(compHand.Cards[i]));
                        if (comp != 0)
                        {
                            return comp;
                        }
                    }
                }
                return handComp;
            }
            return 1;
        }

        /// <summary>
        /// Gets the strength of the hand.
        /// </summary>
        /// <returns></returns>
        private Strength GetStrength()
        {
            // Group cards by value.
            var groups = Cards.GroupBy(x => x);
            if (groups.Any(x => x.Count() == 5))
            {
                return Strength.FiveOfAKind;
            }
            else if (groups.Any(x => x.Count() == 4))
            {
                // if joker mode is enabled, and there's at least one joker, 4 of a kind is actually 5 of a kind.
                if (_jokerMode && Cards.Any(c => c == JOKER))
                {
                    return Strength.FiveOfAKind;
                }
                return Strength.FourOfAKind;
            }
            else if (groups.Any(x => x.Count() == 3) && groups.Any(x => x.Count() == 2))
            {
                // if joker mode is enabled, and there's at least one joker, full house is actually 5 of a kind.
                // because 444JJ or 44JJJ would both be 5 of a kind.
                if (_jokerMode && Cards.Any(c => c == JOKER))
                {
                    return Strength.FiveOfAKind;
                }
                return Strength.FullHouse;
            }
            else if (groups.Any(x => x.Count() == 3))
            {
                // if joker mode is enabled
                if (_jokerMode)
                {
                    var jokerCount = Cards.Count(x => x == JOKER);
                    // if there's 3 jokers the only options is four of a kind.
                    if (jokerCount == 3)
                    {
                        return Strength.FourOfAKind;
                    }
                    //else if (jokerCount == 2) // not possible, would fall in full house logic
                    //{
                    //    return Strength.FiveOfAKind;
                    //}
                    else if (jokerCount == 1)
                    {
                        return Strength.FourOfAKind;
                    }
                }
                return Strength.ThreeOfAKind;
            }
            else if (groups.Count(x => x.Count() == 2) == 2)
            {
                // if joker mode is enabled
                if (_jokerMode)
                {
                    var jokerCount = Cards.Count(x => x == JOKER);
                    // if there's 3 jokers the only options is four of a kind, because one of the pairs is a joker pair.
                    if (jokerCount == 2)
                    {
                        return Strength.FourOfAKind;
                    }
                    else if (jokerCount == 1) // if it's 1, then it's a full house, because there's 2 pairs and 1 joker.
                    {
                        return Strength.FullHouse;
                    }
                }
                return Strength.TwoPair;
            }
            else if (groups.Any(x => x.Count() == 2))
            {
                // if joker mode is enabled
                if (_jokerMode)
                {
                    // if there are any jokers, the only remaining option is three of a kind.
                    if (Cards.Any(x => x == JOKER))
                    {
                        return Strength.ThreeOfAKind;
                    }
                }
                return Strength.OnePair;
            }
            else
            {
                // if joker mode is enabled
                if (_jokerMode)
                {
                    // any jokers means it's a pair.
                    if (Cards.Count(x => x == JOKER) > 0)
                    {
                        return Strength.OnePair;
                    }
                }
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
