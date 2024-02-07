using MapEntities;
using MapGame;
using MenuPr;

namespace Project.Quest
{
    public class QuestNPC
    {
        public int PositionX { get; private set; }
        public int PositionY { get; private set; }
        public string QuestText { get; private set; }
        public bool QuestAccepted { get; set; }
        public int WoodCollected { get; private set; }

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
            WoodCollected = 0;
            this.map = map;
        }
        public void CollectWood()
        {
            if (WoodCollected < 5) // Supposons que le joueur ne peut ramasser que 5 morceaux de bois
            {
                WoodCollected++;
                Console.WriteLine($"Morceau de bois ramassé. Vous avez maintenant {WoodCollected}/5.");
                map.ClearWoodPiecePosition(PositionX, PositionY);
            }
            else
            {
                Console.WriteLine("Vous avez déjà ramassé suffisamment de bois.");
            }
        }

        public void Interact()
        {

            if (QuestAccepted && WoodCollected < 5)
            {
                Console.WriteLine($"{WoodCollected}/5 morceaux de bois ramassés.");
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
        public void UpdatePlayerActions(Player player, Map map)
        {
            foreach (var questNpc in map.questNPCs)
            {
                if (questNpc.IsNear(player))
                {
                    Console.WriteLine("NPC de quête à proximité, déclenchement de l'interaction");
                    questNpc.Interact();
                    break;
                }
            }
        }


        public class WoodPiece
        {
            public int PositionX { get; private set; }
            public int PositionY { get; private set; }

            public WoodPiece(int x, int y)
            {
                PositionX = x;
                PositionY = y;
            }
        }



    }
}