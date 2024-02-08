
using MapEntities;
using Newtonsoft.Json;
using System.IO;
using System.IO.Pipes;

public abstract class EntityAbstract
{
    public string name;
    public string type;
    public int maxhealth;
    public float health;
    public float stamina;
    public int maxStamina;
    public float currentStamina;
    public int speed;
    public float resistanceFeu;
    public float resistanceEau;
    public float resistanceVent;
    public float resistancePhysique;
    public float boostDamage;
    public int level;
    public int experience;
    public int maxExerience;
    public bool gameReset = false;
    public bool gameStart = true;

    public List<EntitiesCapacities> listCapacities { get; set; }
    public EntityContainer entitiesContainer;

    public int currentLevel { get; set; }

    public abstract void DisplayDetails();
    public abstract void AddHealth(int add);
    public abstract void AddStamina(int add);
    public abstract void LessStamina(float less);
    public abstract void TakeDamage(float less);
    public abstract void AddLevel();
    public abstract void AddExperience(int add);
    public abstract void Loot(Player p);

    public void CreateEntity(string path, EntityContainer entities)
    {
        Allies allies = new Allies
        {
            name = "Ace",
            type = "Logia",
            maxhealth = 300,
            health = 300.0f,
            stamina = 200.0f,
            maxStamina = 200,
            speed = 40,
            resistanceFeu = 2.0f,
            resistanceEau = 0.5f,
            resistanceVent = 0.75f,
            resistancePhysique = 1.5f,
            boostDamage = 1.0f,
            level = 1,
            listCapacities = new List<EntitiesCapacities>
            {
                new EntitiesCapacities
                {
                    name = "Chopped",
                    type = "Physique",
                    damage = 20.0f,
                    stamina = 10.0f,
                    speed = 5,
                    boostDamage = 1.0f,
                    criticalChance = 1.4f,
                    level = 0
                },
                 new EntitiesCapacities
                {
                    name = "Fire Punch",
                    type = "Feu",
                    damage = 40.0f,
                    stamina = 10.0f,
                    speed = 5,
                    boostDamage = 1.0f,
                    criticalChance = 1.3f,
                    level = 2
                },
                new EntitiesCapacities
                {
                    name = "Great Ring of Fire: Horse of the Sun",
                    type = "Feu",
                    damage = 80.0f,
                    stamina = 10.0f,
                    speed = 5,
                    boostDamage = 1.0f,
                    criticalChance = 1.2f,
                    level = 3
                }
            },
            currentLevel = 1,
            currentStamina = 200,
        };

        Allies allies2 = new Allies
        {
            name = "Monkey D.Luffy",
            type = "Paramecia",
            maxhealth = 300,
            health = 300.0f,
            stamina = 300.0f,
            maxStamina = 300,
            speed = 50,
            resistanceFeu = 1.5f,
            resistanceEau = 0.5f,
            resistanceVent = 1.25f,
            resistancePhysique = 2.0f,
            boostDamage = 1.0f,
            level = 1,
            listCapacities = new List<EntitiesCapacities>
            {
                new EntitiesCapacities
                {
                    name = "Jet Pistol",
                    type = "Physique",
                    damage = 20.0f,
                    stamina = 10.0f,
                    speed = 20,
                    boostDamage = 1.0f,
                    criticalChance = 1.4f,
                    level = 0
                },
                new EntitiesCapacities
                {
                    name = "Red Hawk",
                    type = "Physique",
                    damage = 50.0f,
                    stamina = 100.0f,
                    speed = 70,
                    boostDamage = 1.0f,
                    criticalChance = 1.2f,
                    level = 2
                },
                new EntitiesCapacities
                {
                    name = "Gear 5",
                    type = "Physique",
                    damage = 60.0f,
                    stamina = 20.0f,
                    speed = 20,
                    boostDamage = 1.0f,
                    criticalChance = 1.3f,
                    level = 3
                },
                new EntitiesCapacities
                {
                    name = "Haki",
                    type = "Physique",
                    damage = 70.0f,
                    stamina = 20.0f,
                    speed = 20,
                    boostDamage = 1.0f,
                    criticalChance = 1.3f,
                    level = 5
                }
            },
            currentLevel = 1,
            currentStamina = 300,
        };

        Allies allies3 = new Allies
        {
            name = "Jimbey",
            type = "Logia",
            maxhealth = 400,
            health = 400.0f,
            stamina = 300.0f,
            maxStamina = 300,
            speed = 50,
            resistanceFeu = 0.5f,
            resistanceEau = 2.0f,
            resistanceVent = 0.75f,
            resistancePhysique = 1.0f,
            boostDamage = 1.0f,
            level = 1,
            listCapacities = new List<EntitiesCapacities>
            {
                new EntitiesCapacities
                {
                    name = "Fish-Man Karate",
                    type = "Physique",
                    damage = 20.0f,
                    stamina = 10.0f,
                    speed = 20,
                    boostDamage = 1.0f,
                    criticalChance = 1.2f,
                    level = 0
                },
                new EntitiesCapacities
                {
                    name = "Water Shot",
                    type = "Eau",
                    damage = 30.0f,
                    stamina = 100.0f,
                    speed = 70,
                    boostDamage = 1.0f,
                    criticalChance = 1.2f,
                    level = 1
                },
                new EntitiesCapacities
                {
                    name = "Heart of Water",
                    type = "Eau",
                    damage = 50.0f,
                    stamina = 100.0f,
                    speed = 70,
                    boostDamage = 1.0f,
                    criticalChance = 1.2f,
                    level = 4
                }
            },
            currentLevel = 1,
            currentStamina = 300,
        };

        Enemy enemy = new Enemy
        {
            name = "Marine",
            type = "Humain",
            difficultyIA = "Normal",
            maxhealth = 500,
            health = 500.0f,
            stamina = 300.0f,
            maxStamina = 300,
            speed = 50,
            resistanceFeu = 0.5f,
            resistanceEau = 0.5f,
            resistanceVent = 0.5f,
            resistancePhysique = 0.75f,
            boostDamage = 1.0f,
            level = 1,
            listCapacities = new List<EntitiesCapacities>
                    {
                        new EntitiesCapacities
                        {
                            name = "Punch",
                            type = "Physique",
                            damage = 5.0f,
                            stamina = 20.0f,
                            speed = 20,
                            boostDamage = 1.0f,
                            criticalChance = 1.2f,
                            level = 0
                        },
                        new EntitiesCapacities
                        {
                            name = "Epee",
                            type = "Physique",
                            damage = 20.0f,
                            stamina = 20.0f,
                            speed = 20,
                            boostDamage = 1.0f,
                            criticalChance = 1.2f,
                            level = 0
                        }
                    },
            currentLevel = 1,
            currentStamina = 300,
        };

        Enemy enemy2 = new Enemy
        {
            name = "Kobby",
            type = "Paramecia",
            difficultyIA = "Hard",
            maxhealth = 500,
            health = 500.0f,
            stamina = 300.0f,
            maxStamina = 300,
            speed = 50,
            resistanceFeu = 0.75f,
            resistanceEau = 0.75f,
            resistanceVent = 0.75f,
            resistancePhysique = 1.25f,
            boostDamage = 1.0f,
            level = 1,
            listCapacities = new List<EntitiesCapacities>
            {
                new EntitiesCapacities
                {
                    name = "Six Powers",
                    type = "Physique",
                    damage = 20.0f,
                    stamina = 20.0f,
                    speed = 20,
                    criticalChance = 1.2f,
                    level = 0
                },
                new EntitiesCapacities
                {
                    name = "Honesty Impact",
                    type = "Eau",
                    damage = 50.0f,
                    stamina = 100.0f,
                    speed = 70,
                    criticalChance = 1.2f,
                    level = 0
                }
            },
            currentLevel = 1,
            currentStamina = 300,
        };

        Enemy enemy3 = new Enemy
        {
            name = "Doflamingo",
            type = "Paramecia",
            difficultyIA = "Hard",
            maxhealth = 500,
            health = 500.0f,
            stamina = 300.0f,
            maxStamina = 300,
            speed = 50,
            resistanceFeu = 1.10f,
            resistanceEau = 1.10f,
            resistanceVent = 2.0f,
            resistancePhysique = 1.5f,
            boostDamage = 1.0f,
            level = 1,
            listCapacities = new List<EntitiesCapacities>
            {
                new EntitiesCapacities
                {
                    name = "Ball of yarn",
                    type = "Vent",
                    damage = 40.0f,
                    stamina = 20.0f,
                    speed = 20,
                    criticalChance = 1.2f,
                    level = 0
                },
                new EntitiesCapacities
                {
                    name = "Bird Cage",
                    type = "Vent",
                    damage = 70.0f,
                    stamina = 100.0f,
                    speed = 70,
                    criticalChance = 1.2f,
                    level = 0
                }
            },
            currentLevel = 1,
            currentStamina = 300,
        };

        Enemy enemy4 = new Enemy
        {
            name = "Sanglier",
            type = "Humain",
            difficultyIA = "Normal",
            maxhealth = 100,
            health = 100.0f,
            stamina = 300.0f,
            maxStamina = 300,
            speed = 50,
            resistanceFeu = 1.0f,
            resistanceEau = 1.0f,
            resistanceVent = 1.0f,
            resistancePhysique = 1.0f,
            boostDamage = 1.0f,
            level = 1,
            listCapacities = new List<EntitiesCapacities>
            {
                new EntitiesCapacities
                {
                    name = "Morsure",
                    type = "Physique",
                    damage = 5.0f,
                    stamina = 20.0f,
                    speed = 20,
                    criticalChance = 1.2f,
                    level = 0
                },
                new EntitiesCapacities
                {
                    name = "Charge",
                    type = "Physique",
                    damage = 10.0f,
                    stamina = 100.0f,
                    speed = 70,
                    criticalChance = 1.4f,
                    level = 0
                }
            },
            currentLevel = 1,
            currentStamina = 300,
        };

        entitiesContainer = new EntityContainer
        {
            alliesList = new List<Allies> { allies, allies2, allies3 },
            enemiesList = new List<Enemy> { enemy, enemy2, enemy3, enemy4 },
        };

        // Check le nb d'allié pour l'hud
        foreach (var addAllies in entitiesContainer.alliesList)
        {
            if (!entities.alliesList.Any(noDoublons => noDoublons.name == addAllies.name))
            {
                entities.alliesList.Add(addAllies);
            }
        }


        if (gameReset || !GetExistsJson(path) || GetEmptyJson(path))
        {
            string json = JsonConvert.SerializeObject(entitiesContainer);
            File.WriteAllText(path, json);
            Console.WriteLine("Données sauvegardées dans le fichier : " + path);
        }

    }

    private bool GetExistsJson(string path)
    {
        return File.Exists(path);
    }

    private bool GetEmptyJson(string path)
    {
        try
        {
            string json = File.ReadAllText(path);
            return string.IsNullOrEmpty(json);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error reading JSON file: " + ex.Message);
            return true; 
        }
    }

    public void GetInfoEntity(string path)
    {
        string json = File.ReadAllText(path);
        entitiesContainer = JsonConvert.DeserializeObject<EntityContainer>(json);
    }

    public EntityContainer GetInfoEntityUpdateLevel(string path)
    {
        string json = File.ReadAllText(path);
        EntityContainer entities = JsonConvert.DeserializeObject<EntityContainer>(json);

        foreach (var ally in entities.alliesList)
        {
            ally.currentLevel = ally.level;
        }

        return entities;
    }

    public EntityContainer GetInfoEntityUpdateStamina(string path)
    {
        string json = File.ReadAllText(path);
        EntityContainer entities = JsonConvert.DeserializeObject<EntityContainer>(json);

        foreach (var ally in entities.alliesList)
        {
            ally.currentStamina = ally.stamina;
        }

        return entities;
    }

    public void UpdateJsonLevel(EntityContainer entities, string path)
    {
        var targetAlliesUpdate = entities.alliesList.FirstOrDefault(a => a.name.Equals(this.name, StringComparison.OrdinalIgnoreCase));

        if (targetAlliesUpdate != null)
        {
            targetAlliesUpdate.currentLevel = level;
        }

        // Maj du JSON avec la modif du level
        using (StreamWriter writer = File.CreateText(path))
        {
            string updatedJson = JsonConvert.SerializeObject(entities);
            writer.Write(updatedJson);
            writer.Close();
        }
    }

    public void UpdateJsonStamina(EntityContainer entities, string path)
    {
        var targetAlliesUpdate = entities.alliesList.FirstOrDefault(a => a.name.Equals(this.name, StringComparison.OrdinalIgnoreCase));

        if (targetAlliesUpdate != null)
        {
            targetAlliesUpdate.currentStamina = stamina;
        }

        using (StreamWriter writer = File.CreateText(path))
        {
            string updatedJson = JsonConvert.SerializeObject(entities);
            writer.Write(updatedJson);
            writer.Close();
        }
        
    }

}