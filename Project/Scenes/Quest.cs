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


        private Map map;

        public bool IsNear(Player player)
        {
            return Math.Abs(PositionX - player.LOCALX) <= 2 && Math.Abs(PositionY - player.LOCALY) <= 2;
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



            string questText = @"
 ██████╗ ██╗   ██╗███████╗████████╗███████╗███████╗
██╔═══██╗██║   ██║██╔════╝╚══██╔══╝██╔════╝██╔════╝
██║   ██║██║   ██║█████╗     ██║   █████╗  ███████╗
██║▄▄ ██║██║   ██║██╔══╝     ██║   ██╔══╝  ╚════██║
╚██████╔╝╚██████╔╝███████╗   ██║   ███████╗███████║
 ╚══▀▀═╝  ╚═════╝ ╚══════╝   ╚═╝   ╚══════╝╚══════╝
                                                   

          
     O
    /|\
    / \

Ah, vous voilà ! Je suis dans une situation délicate.
Ma maison a été gravement endommagée lors d'une attaque inattendue d'un sanglier furieux.
Je dois la réparer au plus vite pour protéger ma famille, mais il me manque des matériaux essentiels.
Vous, un courageux pirate, pourriez-vous m'aider à trouver 5 morceaux de bois ?
Ils seraient parfaits pour réparer les dégâts.

";

            string fullPrompt = questText + "\nAcceptez-vous la quête ?";
            var menu = new Menu(fullPrompt, new string[] { "Oui", "Non" });

            int selectedIndex = menu.Run();

            if (selectedIndex == 0)
            {
                QuestAccepted = true;
                Console.WriteLine("Quête acceptée!");
                Console.WriteLine("Bonne chance dans votre quête!");
                Console.WriteLine("Appuyez sur n'importe quelle touche pour continuer...");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Quête refusée.");
            }
        }


        public class Npc
        {
            public int PositionX { get; private set; }
            public int PositionY { get; private set; }


        }
        public void UpdatePlayerActions(Player player, Map map, WoodCollector woodCollector)
        {
            foreach (var questNpc in map.questNPCs)
            {
                if (questNpc.IsNear(player))
                {
                    Console.WriteLine("NPC de quête à proximité, déclenchement de l'interaction");
                    questNpc.Interact(woodCollector,player); 
                    break;
                }
            }
        }
    }
}