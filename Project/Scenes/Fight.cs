
public class Fight
{
        private bool tourAlier = false;
        Random random = new Random();
        public void startCombat(EntityAbstract allie, EntityAbstract enemie)
        {
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
                    Console.WriteLine("les differentes option: \n A.Attaque B.Soin C.Stamina ");
                    ConsoleKeyInfo keyInfo = Console.ReadKey();
                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.A:
                            enemie.TakeDamage(500);
                            break;
                        case ConsoleKey.B:
                            allie.AddHealth(10);
                            break;
                        case ConsoleKey.C:
                            allie.AddStamina(50);
                            break;

                    }
                    Console.WriteLine($"vie {allie._name} : {allie._health} ");
                    Console.WriteLine($"vie {enemie._name} : {enemie._health} ");
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

