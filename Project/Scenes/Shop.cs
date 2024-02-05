using static System.Console;


namespace ShopDemo
{
    class Shop
    {
        private static Dictionary<string, double> produits = new Dictionary<string, double>()
        {
            { "Pommes", 1.50 },
            { "Bananes", 0.75 },
            { "Oranges", 2.00 },
            { "Fraises", 3.50 },
            { "Pêches", 2.25 },
        };

        private static int SelectedIndex = 0;
        private static double money = 100.00;

        public static void Run()
        {
            while (true)
            {
                Clear();
                DisplayProducts();

                int produitChoisi = ChooseProduct();

                string articleChoisi = produits.Keys.ElementAt(produitChoisi);
                Write($"Entrez la quantité que vous souhaitez acheter de {articleChoisi} : ");
                int quantite;
                if (int.TryParse(ReadLine(), out quantite) && quantite > 0)
                {
                    double prixTotal = produits[articleChoisi] * quantite;
                    if (money >= prixTotal)
                    {
                        money -= prixTotal;
                        Clear();
                        DisplayProducts(); 
                        WriteLine($"Le prix total pour {quantite} {articleChoisi} est : {prixTotal}");
                        WriteLine($"Il vous reste {money}.");
                    }
                    else
                    {
                        WriteLine("Fonds insuffisants pour cet achat.");
                    }
                }
                else
                {
                    WriteLine("La quantité doit être supérieure à zéro.");
                }

                WriteLine("Appuyez sur une touche pour continuer...");
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
                        SelectedIndex = produits.Count - 1;
                    }
                }
                else if (keyPressed == ConsoleKey.DownArrow)
                {
                    SelectedIndex++;
                    if (SelectedIndex == produits.Count)
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
            WriteLine($"Argent disponible : {money}");
            ResetColor();
            WriteLine(" ");

            WriteLine("Voici nos produits disponibles :");
            int index = 0;
            foreach (var produit in produits)
            {
                if (index == SelectedIndex)
                {
                    ForegroundColor = ConsoleColor.Black;
                    BackgroundColor = ConsoleColor.White;
                }
                else
                {
                    ForegroundColor = ConsoleColor.White;
                    BackgroundColor = ConsoleColor.Black;
                }

                WriteLine($" <<{produit.Key}: {produit.Value}>>");
                index++;
            }
            ResetColor();
        }
    }
}
