﻿using static System.Console;


namespace ShopDemo
{
    public class Shop
    {
        private Dictionary<string, double> produits = new Dictionary<string, double>()
        {
            { "Pommes", 1.50 },
            { "Bananes", 18.00 },
            { "Oranges", 2.00 },
            { "Fraises", 3.50 },
            { "Pêches", 2.25 },
        };

        private bool running = true;
        private int SelectedIndex = 0;
        private double money = 100.00;

        public void Run()
        {
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
                    if (money >= prixTotal)
                    {
                        money -= prixTotal;
                        Clear();
                        DisplayProducts(); // Afficher les informations mises à jour
                        WriteLine($"\n\tLe prix total pour {quantite} {articleChoisi} est : {prixTotal} pieces");
                        WriteLine($"\tIl vous reste {money} piece(s).");
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
                Console.WriteLine("\t     Appuyer sur une touche pour Continuer     ");
                Console.ResetColor();
                ReadKey();
            }
        }




        public int ChooseProduct()
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


        private void DisplayProducts()
        {


            WriteLine(@"
            ███████ ██   ██  ██████  ██████
            ██      ██   ██ ██    ██ ██   ██
            ███████ ███████ ██    ██ ██████
                 ██ ██   ██ ██    ██ ██
            ███████ ██   ██  ██████  ██
                                 
                                 
                                 
                                 
            ");


            ForegroundColor = ConsoleColor.Green;
            WriteLine($"\t\tMontant restant : {money} piece(s)");
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
