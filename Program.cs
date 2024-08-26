using FlashCards.Helpers;

namespace FlashCards;

internal class Program
{
    static void Main(string[] args)
    {
        FlashCardsHelper flashCardsHelper = new FlashCardsHelper();
        flashCardsHelper.Run();
    }
}
