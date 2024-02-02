
using Newtonsoft.Json;

public abstract class EntityAbstract
{
    public string _name;
    public string _type;
    public int _maxhealth;
    public float _health;
    public int _stamina;
    public int _speed;
    public float _resistance;
    public float _boostDamage;
    public int _level;
    public int _experience;
    public int _maxExerience;
    public bool gameReset = false;

    public List<EntitiesCapacities> _ListCapacities { get; set; }
    public EntityContainer entitiesContainer;
    public int _currentLevel { get; set; }

    public abstract void DisplayDetails();
    public abstract void AddHealth(int add);
    public abstract void AddStamina(int add);
    public abstract void LessStamina(int less);
    public abstract void TakeDamage(float less);
    public abstract void AddLevel();
    public abstract void AddExperience(int add);

    public void CreateEntity(string path)
    {
        Allies allies = new Allies
        {
            _name = "Ace", // Type Logia
            _type = "Logia",
            _maxhealth = 300,
            _health = 300.0f,
            _stamina = 200,
            _speed = 40,
            _resistance = 1.0f,
            _boostDamage = 1.0f,
            _level = 1,
            _ListCapacities = new List<EntitiesCapacities>
            {
                new EntitiesCapacities
                {
                    _name = "Haki",
                    _damage = 70.0f,
                    _stamina = 10,
                    _speed = 5,
                    _resistance = 1.0f,
                    _boostDamage = 1.0f,
                    _level = 2
                },
                new EntitiesCapacities
                {
                    _name = "Paladin",
                    _damage = 20.0f,
                     _stamina = 10,
                    _speed = 5,
                    _resistance = 1.0f,
                    _boostDamage = 1.0f,
                    _level = 0
                }
            },
            _currentLevel = 1,
        };

        Allies allies2 = new Allies
        {
            _name = "Luffy",
            _type = "Paramecia",
            _maxhealth = 500,
            _health = 500.0f,
            _stamina = 300,
            _speed = 50,
            _resistance = 1.0f,
            _boostDamage = 1.0f,
            _level = 1,
            _ListCapacities = new List<EntitiesCapacities>
            {
                new EntitiesCapacities
                {
                    _name = "Haki",
                    _damage = 70.0f,
                    _stamina = 20,
                    _speed = 20,
                    _resistance = 1.0f,
                    _boostDamage = 1.0f,
                    _level = 0
                },
                new EntitiesCapacities
                {
                    _name = "Gear 5",
                    _damage = 100.0f,
                    _stamina = 100,
                    _speed = 70,
                    _resistance = 1.0f,
                    _boostDamage = 1.0f,
                    _level = 5
                }
            },
            _currentLevel = 1,
        };

        Enemy enemy = new Enemy
        {
            _name = "Marine",
            _type = "Humain",
            _maxhealth = 500,
            _health = 500.0f,
            _stamina = 300,
            _speed = 50,
            _resistance = 1.0f,
            _boostDamage = 1.0f,
            _level = 1,
            _ListCapacities = new List<EntitiesCapacities>
            {
                new EntitiesCapacities
                {
                    _name = "Haki",
                    _damage = 20.0f,
                    _stamina = 20,
                    _speed = 20,
                    _resistance = 1.0f,
                    _boostDamage = 1.0f,
                    _level = 2
                },
                new EntitiesCapacities
                {
                    _name = "Gear 5",
                    _damage = 30.0f,
                    _stamina = 100,
                    _speed = 70,
                    _resistance = 1.0f,
                    _boostDamage = 1.0f,
                    _level = 5
                }
            },
            _currentLevel = 1,
        };

        Enemy enemy2 = new Enemy
        {
            _name = "Amarial",
            _type = "Zoan",
            _maxhealth = 500,
            _health = 500.0f,
            _stamina = 300,
            _speed = 50,
            _resistance = 1.0f,
            _boostDamage = 1.0f,
            _level = 1,
            _ListCapacities = new List<EntitiesCapacities>
            {
                new EntitiesCapacities
                {
                    _name = "Haki",
                    _damage = 20.0f,
                    _stamina = 20,
                    _speed = 20,
                    _level = 2
                },
                new EntitiesCapacities
                {
                    _name = "Gear 5",
                    _damage = 30.0f,
                    _stamina = 100,
                    _speed = 70,
                    _level = 5
                }
            },
            _currentLevel = 1,
        };

        entitiesContainer = new EntityContainer
        {
            AlliesList = new List<Allies> { allies, allies2 },
            EnemiesList = new List<Enemy> { enemy, enemy2 },
        };

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

        foreach (var ally in entities.AlliesList)
        {
            ally._currentLevel = ally._level;
        }

        return entities;
    }


    public void UpdateJsonLevel(EntityContainer entities, string path)
    {
        var targetAlliesUpdate = entities.AlliesList.FirstOrDefault(a => a._name.Equals(this._name, StringComparison.OrdinalIgnoreCase));

        if (targetAlliesUpdate != null)
        {
            targetAlliesUpdate._currentLevel = _level;
        }

        // Maj du JSON avec la modif du level
        string updatedJson = JsonConvert.SerializeObject(entities);
        File.WriteAllText(path, updatedJson);
    }

}