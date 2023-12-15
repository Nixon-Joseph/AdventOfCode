namespace AdventOfCode.Puzzles._2023.Day04
{
    internal class ScratchSolution2 : ScratchBase
    {
        protected override object DoSolve()
        {
            var cards = ReadInputFromFile();
            for (var cardIndex = 0; cardIndex < cards.Count; cardIndex++)
            {
                var card = cards[cardIndex];
                if (card.GetNumCardsToCopy() is var cardsToCopy and > 0)
                {
                    var copyIndex = cardIndex + 1;
                    while (copyIndex <= cardIndex + cardsToCopy)
                    {
                        cards[copyIndex].Copies += card.Copies;
                        copyIndex++;
                    }
                }
            }
            return cards.Sum(c => c.Copies);
        }
    }
}
