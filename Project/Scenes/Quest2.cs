using System;
using MapEntities;
using MapGame;
using MenuPr;
using Wood;

namespace Project.Quest2
{
    public class QuestNPC2
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
            Console.WriteLine(PositionX);
            return Math.Abs(PositionX - player.LOCALX) <= 1 && Math.Abs(PositionY - player.LOCALY) <= 1;
        }

        public QuestNPC2(int x, int y, string questText, Map map)
        {
            PositionX = x;
            PositionY = y;
            QuestText = questText;
            QuestAccepted = false;
            this.map = map;
        }

        public void Interact2(Player player)
        {
            if (HasInteracted)
            {
                Console.WriteLine("Vous avez déjà interagi avec ce NPC pour cette quête.");
                return;
            }

            if (QuestAccepted && !QuestCompleted && player.SangliersTues < 5)
            {
                Console.WriteLine($"{player.SangliersTues}/5 sangliers tués.");
                return;
            }

            if (player.SangliersTues >= 5)
            {
                Console.WriteLine("Félicitations ! Vous avez tué suffisamment de sangliers pour aider le NPC.");
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
   


Ah, vous voilà ! J'ai un problème qui requiert votre aide.
Des sangliers sauvages ont envahi ma ferme et causent des ravages.
Je dois les chasser pour protéger mes cultures, mais je ne suis pas équipé pour le faire.
Vous, un chasseur habile, pourriez-vous tuer 5 sangliers pour moi ?
Cela aiderait grandement à sauver ma récolte.

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

        private bool IsInteractionKeyPressed()
        {
            return Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.F;
        }


        public void UpdatePlayerActions(Player player ,List<QuestNPC2> questNPCs)
        {
            // Vérifier si le joueur est proche d'un NPC et si la touche F est enfoncée
            if (IsNear(player) && IsInteractionKeyPressed())
            {
                Console.WriteLine("NPC de quête à proximité. Appuyez sur F pour interagir.");
                // Interagir avec le NPC si la touche F est enfoncée
                Console.WriteLine("Interagir avec le NPC de quête...");
                Interact2( player);
                return;
            }
        }

    }
}
