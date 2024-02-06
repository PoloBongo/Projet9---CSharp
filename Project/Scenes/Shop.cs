using MapEntities;
using static System.Console;


namespace ShopDemo
{
    class Shop
    {
        private static Dictionary<string, double> produits = new Dictionary<string, double>()
        {
            { "Viande", 5.00 },
            { "Alcool", 10.00 },
        };

        private static bool running = true;
        private static int SelectedIndex = 0;
        private static Player player;

        public static void Run(Player currentPlayer)
        {
            player = currentPlayer;

            running = true;
            while (running)
            {
                Clear();
                DisplayProducts();

                int produitChoisi = ChooseProduct();

                // Sortir de la boutique
                if (produitChoisi == produits.Count)
                {
                    running = false;
                    continue;
                }

                string articleChoisi = produits.Keys.ElementAt(produitChoisi);
                Write($"\nEntrez la quantité que vous souhaitez acheter de {articleChoisi} : ");
                int quantite;
                if (int.TryParse(ReadLine(), out quantite) && quantite > 0)
                {
                    double prixTotal = produits[articleChoisi] * quantite;
                    if (player.NBGold >= prixTotal)
                    {
                        player.NBGold -= (int)prixTotal;
                        Clear();
                        DisplayProducts(); // Afficher les informations mises à jour


                        if (articleChoisi == "Viande")
                        {
                            player.AddViande(quantite);
                        }
                        else if (articleChoisi == "Alcool")
                        {
                            player.AddAlcool(quantite);
                        }
                        WriteLine($"\n\tLe prix total pour {quantite} {articleChoisi} est : {prixTotal} pièces d'or.");
                        WriteLine($"\tIl vous reste {player.NBGold} pièces d'or.");
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.DarkRed;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine("\n\t\tFonds insuffisants pour cet achat");
                        Console.ResetColor();
                    }
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine("\n\t\t Choix Incorrect");
                    Console.ResetColor();
                }

                Console.BackgroundColor = ConsoleColor.DarkYellow;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("\t     Appuyez sur une touche pour Continuer     ");
                Console.ResetColor();
                ReadKey();
            }
        }




        private static int ChooseProduct()
        {
            ConsoleKey keyPressed;
            do
            {
                Clear();
                DisplayProducts();

                ConsoleKeyInfo keyInfo = ReadKey(true);
                keyPressed = keyInfo.Key;

                if (keyPressed == ConsoleKey.UpArrow)
                {
                    SelectedIndex--;
                    if (SelectedIndex < 0)
                    {
                        SelectedIndex = produits.Count;
                    }
                }
                else if (keyPressed == ConsoleKey.DownArrow)
                {
                    SelectedIndex++;
                    if (SelectedIndex > produits.Count)
                    {
                        SelectedIndex = 0;
                    }
                }

            } while (keyPressed != ConsoleKey.Enter);

            return SelectedIndex;
        }


        private static void DisplayProducts()
        {


            WriteLine(@"
            ███████ ██   ██  ██████  ██████
            ██      ██   ██ ██    ██ ██   ██
            ███████ ███████ ██    ██ ██████
                 ██ ██   ██ ██    ██ ██
            ███████ ██   ██  ██████  ██
                                 
                                 
                                 
                                 
            ");


            ForegroundColor = ConsoleColor.Green;
            WriteLine($"\t\tMontant restant : {player.NBGold} piece(s)");
            ResetColor();

            WriteLine("\n\tVoici nos produits disponibles :");

            int index = 0;

            foreach (var produit in produits)
            {
                string prefix;

                if (index == SelectedIndex)
                {
                    prefix = ">>";
                    ForegroundColor = ConsoleColor.Black;
                    BackgroundColor = ConsoleColor.White;
                }
                else
                {
                    prefix = "  ";
                    ForegroundColor = ConsoleColor.White;
                    BackgroundColor = ConsoleColor.Black;
                }

                WriteLine($"   {prefix} {produit.Key}: {produit.Value}  ");
                index++;
            }

            if (SelectedIndex == produits.Count)
            {
                ForegroundColor = ConsoleColor.Black;
                BackgroundColor = ConsoleColor.DarkRed;
            }
            else
            {
                ForegroundColor = ConsoleColor.White;
                BackgroundColor = ConsoleColor.Black;
            }
            WriteLine(" < [ QUITTER ]   ");

            ResetColor();
        }
    }
}
