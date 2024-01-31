using MenuPr;
using static System.Console;

namespace ShopDemo
{
    class Shop
    {
        private decimal OrderTotal;
        private Dictionary<string, decimal> ItemPrices = new Dictionary<string, decimal>
        {
            { "Heal", 10m },
            { "jsp", 20m },
            { "Exit", 0m }
        };

        public Shop()
        {
            OrderTotal = 100;
        }

        public void Run()
        {
            DisplayIntro();
            WriteLine();

            string selectedItem;
            do
            {
                selectedItem = DisplayMenu();
                if (selectedItem != "Exit")
                {
                    ProcessItemPurchase(selectedItem);
                    DisplayOrderTotal();
                    WriteLine();
                }
            } while (selectedItem != "Exit");

            DisplayOutro();
        }

        private string DisplayMenu()
        {
            Menu menu = new Menu("Choisissez un article à acheter:", ItemPrices.Keys.ToArray());
            int selectedIndex = menu.Run();
            return ItemPrices.Keys.ElementAt(selectedIndex);
        }

        private void ProcessItemPurchase(string itemName)
        {
            WriteLine($"Tu veux acheter {itemName} pour {ItemPrices[itemName]}?");
            WriteLine("Combien en veux-tu?");
            int quantity = GetUserInputAsInteger();
            decimal itemTotal = ItemPrices[itemName] * quantity;
            OrderTotal += itemTotal;
            WriteLine($"Okay, {quantity}x {itemName} te fait {itemTotal}");
        }

        private int GetUserInputAsInteger()
        {
            while (true)
            {
                if (int.TryParse(ReadLine(), out int quantity) && quantity >= 0)
                {
                    return quantity;
                }
                WriteLine("Erreur: Veuillez entrer un nombre entier positif.");
            }
        }

        private void DisplayOrderTotal()
        {
            SetConsoleColor(ConsoleColor.Green);
            WriteLine($"Tu as {OrderTotal}");
            SetConsoleColor(ConsoleColor.White);
        }

        private void SetConsoleColor(ConsoleColor color)
        {
            ForegroundColor = color;
        }

        private void DisplayIntro()
        {
            WriteLine("==============");
            WriteLine("==Item 4 d'or==");
            WriteLine("==============");
        }

        private void DisplayOutro()
        {
            WriteLine("Merci !");
            WriteLine("Touche n'importe quel touche pour partir");
            ReadKey();
        }
    }
}
