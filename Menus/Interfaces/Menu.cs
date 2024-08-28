namespace FlashCards.Menus.Interfaces;

internal interface IMenu
{
    void Run();

    List<string> GetMenuChoices();

    void RouteToOption(char option);

    string GetOption();
}
