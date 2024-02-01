using static System.Console;

namespace ShopDemo
{
    class Shop
    {
        public static void Run()
        {
            Dictionary<string, double> produits = new Dictionary<string, double>()
        {
            { "Pommes", 1.50 },
            { "Bananes", 0.75 },
            { "Oranges", 2.00 },
            { "Fraises", 3.50 },
            { "Pêches", 2.25 }
        };

            WriteLine("Bienvenue dans notre magasin !");
            WriteLine("Voici nos produits disponibles :");

            foreach (var produit in produits)
            {
                WriteLine($"{produit.Key}: {produit.Value:C}");
            }

            Write("Entrez le nom de l'article que vous souhaitez acheter : ");
            string articleChoisi = ReadLine();

            if (produits.ContainsKey(articleChoisi))
            {
                Write($"Entrez la quantité que vous souhaitez acheter de {articleChoisi} : ");
                int quantite = int.Parse(ReadLine());

                if (quantite > 0)
                {
                    double prixTotal = produits[articleChoisi] * quantite;
                    WriteLine($"Le prix total pour {quantite} {articleChoisi} est : {prixTotal:C}");
                }
                else
                {
                    WriteLine("La quantité doit être supérieure à zéro.");
                }
            }
            else
            {
                WriteLine("Article non trouvé.");
            }

            WriteLine("Merci d'avoir magasiné avec nous !");
        }
    }
}