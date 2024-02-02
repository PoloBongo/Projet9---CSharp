
public class Fight
{
        private bool tourAlier = false;
        Random random = new Random();
        public void startCombat(EntityContainer entities)
        {
            Console.WriteLine(entities.AlliesList[0]._name);
            EntityAbstract allie = entities.AlliesList[0];
            EntityAbstract enemie = entities.EnemiesList[0];
            List<string> displayedCapacities = new List<string>();
            ConsoleKeyInfo keyInfo;

            while (allie._health >= 0 || enemie._health >= 0)
            {
                if (allie._speed > enemie._speed || tourAlier)
                    {
                        tourAlier = true;
                    }
                    else
                    {
                        tourAlier = false;
                    }

                    if (tourAlier)
                    {
                        Console.WriteLine($"Voulez-vous changer d'allier[A] ou attaquer[B] ?");
                        keyInfo = Console.ReadKey();
                        switch (keyInfo.Key)
                        {
                            case ConsoleKey.A:
                                for (int i = 0; i < entities.AlliesList.Count(); i++)
                                {
                                    Console.WriteLine($"- {entities.AlliesList[i]._name}\n");
                                }
                                Console.Write("Entrez le nom de l'allié que vous voulez choisir : ");
                                string chooseAllies = Console.ReadLine();
                                Allies newAllies = entities.AlliesList.FirstOrDefault(a => a._name.Equals(chooseAllies, StringComparison.OrdinalIgnoreCase));
                                if (newAllies != null)
                                {
                                    Console.WriteLine($"Vous avez choisi l'allié : {newAllies._name}");
                                    allie = newAllies;
                                }
                                else
                                {
                                    Console.WriteLine("Allié non trouvé. Veuillez choisir un allié valide.");
                                }
                            break;
                            case ConsoleKey.B:
                                Console.WriteLine($"Choisi la capacité que tu veux utilisé pour attaquer :\n ");
                                for (int i = 0; i < entities.AlliesList.Count(); i++)
                                {
                                    for (int j = 0; j < entities.AlliesList[i]._ListCapacities.Count(); j++)
                                    {
                                        if (allie._level >= allie._ListCapacities[j]._level && !displayedCapacities.Contains(allie._ListCapacities[j]._name))
                                        {
                                            Console.WriteLine($"- {allie._ListCapacities[j]._name}");
                                            displayedCapacities.Add(allie._ListCapacities[j]._name);
                                        }
                                    }
                                }
                                /* Dès que l'utilisateur à choisis sont attaque grâce à la tabulation, il faudra passé l'index de tabulation en paramètre de _ListCapacities[index] ( au lieu du 0 )*/
                                enemie.TakeDamage(allie._ListCapacities[0]._damage);

                                Console.WriteLine($"vie {allie._name} : {allie._health} ");
                                Console.WriteLine($"vie {enemie._name} : {enemie._health} ");
                            break;
                        default:
                            break;

                        }

                        if (enemie._health <= 0)
                        {
                            Console.WriteLine("Tu as gagner");
                            int nombreAleatoire = random.Next(2, 10 * enemie._level);
                            allie.AddExperience(nombreAleatoire);
                            break;
                        }
                        tourAlier = false;
                    }
                    if (!tourAlier)
                    {

                        int nombreAleatoire = random.Next(1, 3);
                        if (nombreAleatoire == 1) allie.TakeDamage(50);
                        if (nombreAleatoire == 2) enemie.AddHealth(10);
                        if (nombreAleatoire == 3) enemie.AddStamina(10);
                        Console.WriteLine($"vie {allie._name} : {allie._health} ");
                        Console.WriteLine($"vie {enemie._name} : {enemie._health} ");
                        if (allie._health <= 0)
                        {
                            Console.WriteLine("Tu as perdu");
                            break;
                        }
                        tourAlier = true;
                    }
            }
        }

 }

