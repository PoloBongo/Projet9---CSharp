using MenuPr;
using static System.Console;
using System.Collections.Generic;

namespace ShopDemo
{
    class Shop
    {
        private decimal OrderTotal;
        private Dictionary<string, decimal> Items = new Dictionary<string, decimal>
        {
            {"Heal", 10m},
            {"JSP", 20m}
        };

        public Shop()
        {
            OrderTotal = 100;
        }

        public void Run()
        {
            DisplayIntro();

            string selectedItem;
            do
            {
                selectedItem = DisplayMenu();
                if (selectedItem != "Exit")
                {
                    SellItem(selectedItem, Items[selectedItem]);
                    DisplayOrderTotal();
                }
            } while (selectedItem != "Exit");

            DisplayOutro();
        }

        private string DisplayMenu()
        {
            List<string> options = new List<string>(Items.Keys) { "Exit" };
            Menu menu = new Menu("Choisissez un article à acheter:", options.ToArray());
            int selectedIndex = menu.Run();
            return options[selectedIndex];
        }

        private void SellItem(string itemName, decimal cost)
        {
            WriteLine($"Tu veux acheter {itemName} pour {cost}?");
            WriteLine("Combien en veux-tu?");
            int quantity = ReadQuantity();
            decimal itemTotal = cost * quantity;
            OrderTotal += itemTotal;
            WriteLine($"Okay, {quantity}x {itemName} te fait {itemTotal}");
        }


        private void DisplayOrderTotal()
        {
            ForegroundColor = ConsoleColor.Green;
            WriteLine($"Tu as {OrderTotal}");
            ForegroundColor = ConsoleColor.White;
        }

        private void DisplayIntro()
        {
            WriteLine("==============");
            WriteLine("==Item 4 d'or==");
            WriteLine("==============");
            Console.Out.Flush(); // Force l'affichage du contenu en tampon
        }

        private void DisplayOutro()
        {
            WriteLine("Merci !");
            WriteLine("Touche n'importe quel touche pour partir");
            ReadKey();
        }

        private void FlushInputBuffer()
        {
            while (Console.KeyAvailable)
            {
                Console.ReadKey(true);
            }
        }

        private int ReadQuantity()
        {
            FlushInputBuffer(); // S'assurer que le tampon d'entrée est vide

            while (true)
            {
                string input = Console.ReadLine();

                // Vérifier et ignorer un éventuel caractère de saut de ligne
                if (!string.IsNullOrEmpty(input) && input[input.Length - 1] == '\n')
                {
                    input = input.Substring(0, input.Length - 1);
                }

                if (int.TryParse(input, out int quantity) && quantity >= 0)
                {
                    return quantity;
                }
                else
                {
                    WriteLine("Erreur: Veuillez entrer un nombre entier positif.");
                }
            }
        }
    }
}