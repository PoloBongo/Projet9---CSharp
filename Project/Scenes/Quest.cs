﻿using MapGame;


namespace Project.Quest
{
    public class QuestNPC
    {
        public int PositionX { get; private set; }
        public int PositionY { get; private set; }
        public string QuestText { get; private set; }
        public bool QuestAccepted { get; set; }

        public QuestNPC(int x, int y, string questText)
        {
            PositionX = x; 
            PositionY = y; 
            QuestText = questText;
            QuestAccepted = false;
        }

        public void Interact()
        {
            Console.WriteLine(QuestText);
            Console.WriteLine("Accepter la quête? (O/N)");
            var response = Console.ReadKey();
            if (response.Key == ConsoleKey.O)
            {
                QuestAccepted = true;
                Console.WriteLine("Quête acceptée!");
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

            public bool IsNear(int x, int y)
            {
                return Math.Abs(PositionX - x) <= 2 && Math.Abs(PositionY - y) <= 2;
            }

            public void UpdatePlayerActions(Npc npc, Map map)
            {
                foreach (var questNpc in map.questNPCs)
                {
                    if (npc.IsNear(questNpc.PositionX, questNpc.PositionY))
                    {
                        questNpc.Interact();
                        break;
                    }
                }
            }
        }
    }
}