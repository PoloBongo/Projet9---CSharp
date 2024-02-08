
using MapEntities;
using MapGame;
namespace Csharp.test
{
    public class Tests
    {
        [Test]
        public void VerifyTheMapIsNotNull()
        {
            var world = new World();

            var map = world.GetMapAt(0, 0);

            Assert.IsNotNull(map);
        }
        [Test]
        public void VerrifyIfContenteAGreatEllement()
        {
            World world = new World();
            char[,] layout = world.CreateRandomLayout(false, false, false, 20, 20);

            Assert.IsTrue(ContainsElement(layout, '.')); 
        }

        [Test]
        public void VerifyIfOneStructurAreCreate()
        {
            World world = new World();

            char[,] layout = world.CreateRandomLayout(false, true, false, 20, 20);

            Assert.IsTrue(ContainsElement(layout, 'H')); 
            Assert.IsTrue(ContainsElement(layout, ']') || ContainsElement(layout, '[') || ContainsElement(layout, '―'));
        }

        private bool ContainsElement(char[,] layout, char element)
        {
            for (int i = 0; i < layout.GetLength(0); i++)
            {
                for (int j = 0; j < layout.GetLength(1); j++)
                {
                    if (layout[i, j] == element)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        [Test]
        public void VerifyIfTheListAddElementNotNull() 
        {
            var entities = new EntityContainer();
            Allies allie = new Allies
            {
                _name = "Ace",
                _type = "Logia",
                _blocked = 0,
                _currentBlocked = 0,
                _maxhealth = 300,
                _health = 300.0f,
                _stamina = 200.0f,
                _maxStamina = 200,
                _speed = 40,
                _resistanceFeu = 2.0f,
                _resistanceEau = 0.5f,
                _resistanceVent = 0.75f,
                _resistancePhysique = 1.5f,
                _boostDamage = 1.0f,
                _level = 1,
                _ListCapacities = new List<EntitiesCapacities>
            {
                new EntitiesCapacities
                {
                    _name = "Chopped",
                    _type = "Physique",
                    _damage = 20.0f,
                    _stamina = 10.0f,
                    _speed = 5,
                    _boostDamage = 1.0f,
                    _criticalChance = 1.4f,
                    _level = 0
                },
                 new EntitiesCapacities
                {
                    _name = "Fire Punch",
                    _type = "Feu",
                    _damage = 40.0f,
                    _stamina = 10.0f,
                    _speed = 5,
                    _boostDamage = 1.0f,
                    _criticalChance = 1.3f,
                    _level = 0
                },
                new EntitiesCapacities
                {
                    _name = "Great Ring of Fire: Horse of the Sun",
                    _type = "Feu",
                    _damage = 80.0f,
                     _stamina = 10.0f,
                    _speed = 5,
                    _boostDamage = 1.0f,
                    _criticalChance = 1.2f,
                    _level = 0
                }
            },
                _currentLevel = 1,
                _currentStamina = 200,
            };
            Enemy enemie = new Enemy
            {
                _name = "Marine",
                _type = "Humain",
                _difficultyIA = "Normal",
                _blocked = 0,
                _currentBlocked = 0,
                _maxhealth = 500,
                _health = 500.0f,
                _stamina = 300.0f,
                _maxStamina = 300,
                _speed = 50,
                _resistanceFeu = 0.5f,
                _resistanceEau = 0.5f,
                _resistanceVent = 0.5f,
                _resistancePhysique = 0.75f,
                _boostDamage = 1.0f,
                _level = 1,
                _ListCapacities = new List<EntitiesCapacities>
                    {
                        new EntitiesCapacities
                        {
                            _name = "Punch",
                            _type = "Physique",
                            _damage = 5.0f,
                            _stamina = 20.0f,
                            _speed = 20,
                            _boostDamage = 1.0f,
                            _criticalChance = 1.2f,
                            _level = 0
                        },
                        new EntitiesCapacities
                        {
                            _name = "Epee",
                            _type = "Physique",
                            _damage = 20.0f,
                            _stamina = 20.0f,
                            _speed = 20,
                            _boostDamage = 1.0f,
                            _criticalChance = 1.2f,
                            _level = 0
                        }
                    },
                _currentLevel = 1,
                _currentStamina = 300,
            };

            entities.AlliesList = new List<Allies> { allie };
            entities.EnemiesList = new List<Enemy> { enemie };

            Assert.IsNotNull(entities.AlliesList);
            Assert.IsNotNull(entities.EnemiesList);

        }
        [Test]
        public void VerifyIfTakeDamage()
        {
            Allies allie = new Allies
            {
                _name = "Ace",
                _type = "Logia",
                _blocked = 0,
                _currentBlocked = 0,
                _maxhealth = 300,
                _health = 300.0f,
                _stamina = 200.0f,
                _maxStamina = 200,
                _speed = 40,
                _resistanceFeu = 2.0f,
                _resistanceEau = 0.5f,
                _resistanceVent = 0.75f,
                _resistancePhysique = 1.5f,
                _boostDamage = 1.0f,
                _level = 1,
                _ListCapacities = new List<EntitiesCapacities>
            {
                new EntitiesCapacities
                {
                    _name = "Chopped",
                    _type = "Physique",
                    _damage = 20.0f,
                    _stamina = 10.0f,
                    _speed = 5,
                    _boostDamage = 1.0f,
                    _criticalChance = 1.4f,
                    _level = 0
                },
                 new EntitiesCapacities
                {
                    _name = "Fire Punch",
                    _type = "Feu",
                    _damage = 40.0f,
                    _stamina = 10.0f,
                    _speed = 5,
                    _boostDamage = 1.0f,
                    _criticalChance = 1.3f,
                    _level = 0
                },
                new EntitiesCapacities
                {
                    _name = "Great Ring of Fire: Horse of the Sun",
                    _type = "Feu",
                    _damage = 80.0f,
                     _stamina = 10.0f,
                    _speed = 5,
                    _boostDamage = 1.0f,
                    _criticalChance = 1.2f,
                    _level = 0
                }
            },
                _currentLevel = 1,
                _currentStamina = 200,
            };

            allie.TakeDamage(20);
            Assert.AreEqual(280, allie._health);
        }
        [Test]
        public void VerifyIfGetHealth()
        {
            var entities = new EntityContainer();
            Allies allie1 = new Allies
            {
                _name = "Ace",
                _type = "Logia",
                _blocked = 0,
                _currentBlocked = 0,
                _maxhealth = 300,
                _health = 300.0f,
                _stamina = 200.0f,
                _maxStamina = 200,
                _speed = 40,
                _resistanceFeu = 2.0f,
                _resistanceEau = 0.5f,
                _resistanceVent = 0.75f,
                _resistancePhysique = 1.5f,
                _boostDamage = 1.0f,
                _level = 1,
                _ListCapacities = new List<EntitiesCapacities>
            {
                new EntitiesCapacities
                {
                    _name = "Chopped",
                    _type = "Physique",
                    _damage = 20.0f,
                    _stamina = 10.0f,
                    _speed = 5,
                    _boostDamage = 1.0f,
                    _criticalChance = 1.4f,
                    _level = 0
                },
                 new EntitiesCapacities
                {
                    _name = "Fire Punch",
                    _type = "Feu",
                    _damage = 40.0f,
                    _stamina = 10.0f,
                    _speed = 5,
                    _boostDamage = 1.0f,
                    _criticalChance = 1.3f,
                    _level = 0
                },
                new EntitiesCapacities
                {
                    _name = "Great Ring of Fire: Horse of the Sun",
                    _type = "Feu",
                    _damage = 80.0f,
                     _stamina = 10.0f,
                    _speed = 5,
                    _boostDamage = 1.0f,
                    _criticalChance = 1.2f,
                    _level = 0
                }
            },
                _currentLevel = 1,
                _currentStamina = 200,
            };
            Allies allies2 = new Allies
            {
                _name = "Monkey D.Luffy",
                _type = "Paramecia",
                _blocked = 0,
                _currentBlocked = 0,
                _maxhealth = 500,
                _health = 500.0f,
                _stamina = 300.0f,
                _maxStamina = 300,
                _speed = 50,
                _resistanceFeu = 1.5f,
                _resistanceEau = 0.5f,
                _resistanceVent = 1.25f,
                _resistancePhysique = 2.0f,
                _boostDamage = 1.0f,
                _level = 1,
                _ListCapacities = new List<EntitiesCapacities>
            {
                new EntitiesCapacities
                {
                    _name = "Jet Pistol",
                    _type = "Physique",
                    _damage = 20.0f,
                    _stamina = 20.0f,
                    _speed = 20,
                    _boostDamage = 1.0f,
                    _criticalChance = 1.4f,
                    _level = 0
                },
                new EntitiesCapacities
                {
                    _name = "Red Hawk",
                    _type = "Physique",
                    _damage = 50.0f,
                    _stamina = 100.0f,
                    _speed = 70,
                    _boostDamage = 1.0f,
                    _criticalChance = 1.2f,
                    _level = 2
                },
                new EntitiesCapacities
                {
                    _name = "Gear 5",
                    _type = "Physique",
                    _damage = 60.0f,
                    _stamina = 20.0f,
                    _speed = 20,
                    _boostDamage = 1.0f,
                    _criticalChance = 1.3f,
                    _level = 3
                },
                new EntitiesCapacities
                {
                    _name = "Haki",
                    _type = "Physique",
                    _damage = 70.0f,
                    _stamina = 20.0f,
                    _speed = 20,
                    _boostDamage = 1.0f,
                    _criticalChance = 1.3f,
                    _level = 5
                }
            },
                _currentLevel = 1,
                _currentStamina = 300,
            };

            entities.AlliesList = new List<Allies> { allie1, allies2  };

            entities.AlliesList[1].AddHealth(20);
            Assert.AreEqual(520, entities.AlliesList[1]._health);
        }
    }
}