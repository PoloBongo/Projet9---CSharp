public class Initialization
{
    /*    public void creationEntity(EntityAbstract marine, EntityAbstract jimbey)
        {
            marine._name = "marine";
            marine._health = 100;
            marine._stamina = 50;
            marine._speed = 60;
            marine._level = 5;

            jimbey._name = "jimbey";
            jimbey._health = 100;
            jimbey._stamina = 50;
            jimbey._speed = 60;
            jimbey._level = 5;
        }*/

        public List<EntityAbstract> LoadEntityStats(string fileName)
        {
            List<EntityAbstract> entities = new List<EntityAbstract>();

            try
            {
                using (StreamReader sr = new StreamReader(fileName))
                {
                    while (!sr.EndOfStream)
                    {
                        string entityType = sr.ReadLine();
                        EntityAbstract entity = EntityAbstract.CreateEntity(entityType);

                        entity._name = entityType;
                        entity._health = int.Parse(sr.ReadLine());
                        entity._stamina = int.Parse(sr.ReadLine());
                        entity._speed = int.Parse(sr.ReadLine());
                        entity._level = int.Parse(sr.ReadLine());

                        entities.Add(entity);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Une erreur s'est produite lors de la lecture du fichier : " + ex.Message);
            }

            return entities;
        }
    }
