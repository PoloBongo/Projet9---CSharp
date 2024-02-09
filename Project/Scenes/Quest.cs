using MapEntities;
using MapGame;
using MenuPr;
using Wood;

namespace Project.Quest
{
    public class QuestNPC
    {
        public int PositionX { get; private set; }
        public int PositionY { get; private set; }
        public string QuestText { get; private set; }
        public bool QuestAccepted { get; set; }
        public bool QuestCompleted { get; private set; }
        public bool HasInteracted { get; private set; }

        private const ConsoleKey InteractionKey = ConsoleKey.F;
        private Map map;

         public bool IsNear(Player player)
        {
            if (Math.Abs(PositionX - player.LOCALX) <= 1 && Math.Abs(PositionY - player.LOCALY) <= 1)
            {
                Console.WriteLine("NPC de quête à proximité. Appuyez sur F pour interagir.");
                return true;
            }
            return false;
        }


        public QuestNPC(int x, int y, string questText, Map map)
        {
            PositionX = x;
            PositionY = y;
            QuestText = questText;
            QuestAccepted = false;
            this.map = map;
        }


        public void Interact(WoodCollector woodCollector, Player player)
        {
            if (HasInteracted)
            {
                Console.WriteLine("Vous avez déjà interagi avec ce NPC pour cette quête.");
                return;
            }

            if (QuestAccepted && !QuestCompleted && woodCollector.WoodCollected < 5)
            {
                Console.WriteLine($"{woodCollector.WoodCollected}/5 morceaux de bois ramassés.");
                return;
            }

            if (woodCollector.WoodCollected >= 5)
            {
                Console.WriteLine("Félicitations ! Vous avez ramassé suffisamment de bois pour aider le NPC.");
                player.NBGold += 100;
                Console.WriteLine("Le NPC vous donne 100 pièces d'or pour votre aide.");
                QuestCompleted = true; // Marquer la quête comme terminée
                Console.WriteLine("Merci beaucoup de m'avoir aidé !");
                QuestAccepted = false; // Réinitialiser la quête
                HasInteracted = true; // Marquer l'interaction avec le NPC
                return;
            }



            string questArt = @"
         ██████╗ ██╗   ██╗███████╗████████╗███████╗███████╗
        ██╔═══██╗██║   ██║██╔════╝╚══██╔══╝██╔════╝██╔════╝
        ██║   ██║██║   ██║█████╗     ██║   █████╗  ███████╗
        ██║▄▄ ██║██║   ██║██╔══╝     ██║   ██╔══╝  ╚════██║
        ╚██████╔╝╚██████╔╝███████╗   ██║   ███████╗███████║
         ╚══▀▀═╝  ╚═════╝ ╚══════╝   ╚═╝   ╚══════╝╚══════╝
                                                   

          
     O
    /|\
    / \
";
            string questText = @"
    Ah, vous voilà ! Je suis dans une situation délicate.
    Ma maison a été gravement endommagée lors d'une attaque inattendue d'un sanglier furieux.
    Je dois la réparer au plus vite pour protéger ma famille, mais il me manque des matériaux essentiels.
    Vous, un courageux pirate, pourriez-vous m'aider à trouver 5 morceaux de bois ?
    Ils seraient parfaits pour réparer les dégâts.
";


            string fullPrompt = questArt + questText + "\n\t\tAcceptez -vous la quête ?";
            var menu = new Menu(fullPrompt, new string[] { "Oui", "Non" });

            int selectedIndex = menu.Run();

            if (selectedIndex == 0)
            {
                QuestAccepted = true;
                Console.BackgroundColor = ConsoleColor.DarkGreen;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("\t\tQuête acceptée!");
                Console.ResetColor();

                Console.BackgroundColor = ConsoleColor.DarkYellow;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("\t\tAppuyez sur une touche pour continuer...");
                Console.ResetColor();
                Console.ReadKey();
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("\t\tQuête refusée.");
                Console.ResetColor();

                Console.BackgroundColor = ConsoleColor.DarkYellow;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("\t\tAppuyez sur une touche pour continuer...");
                Console.ResetColor();
                Console.ReadKey();
            }
        }


        public class Npc
        {
            public int PositionX { get; private set; }
            public int PositionY { get; private set; }


        }

        private bool IsInteractionKeyPressed()
        {
            return Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.F;
        }


        public void UpdatePlayerActions(Player player, WoodCollector woodCollector, List<QuestNPC> questNPCs)
        {
            // Vérifier si le joueur est proche d'un NPC et si la touche F est enfoncée
            if (IsNear(player) && IsInteractionKeyPressed())
            {
                Console.BackgroundColor = ConsoleColor.DarkYellow;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("NPC de quête à proximité. Appuyez sur F pour interagir.");
                Console.ResetColor();
                Console.ReadKey();

                // Interagir avec le NPC si la touche F est enfoncée
                Console.WriteLine("Interagir avec le NPC de quête...");
                Interact(woodCollector, player);
                return;
            }
        }

    }
}
        