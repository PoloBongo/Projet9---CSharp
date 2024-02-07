
using MapEntities;
using MapGame;
using ShopDemo;
namespace Csharp.test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var world = new World();

            var map = world.GetMapAt(0, 0);

            Assert.IsNotNull(map);
        }
        [Test]
        public void Test2()
        {
            World world = new World();
            char[,] layout = world.CreateRandomLayout(false, false, false, 20, 20);

            Assert.IsTrue(ContainsElement(layout, '~')); 
        }

        [Test]
        public void Test3()
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
        public void Test4() 
        {
            var entities = new EntityContainer();
            var allie = new Allies
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
            var enemie = new Enemy
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

            entities.AlliesList.Add(allie);
            entities.EnemiesList.Add(enemie);
            var player = new Player(1, 1, 20, 19);
            var fight = new Fight();

            // Vérifier si les objets sont null avant d'appeler la méthode startCombat
            if (entities != null && player != null && fight != null && entities.AlliesList != null && entities.EnemiesList != null)
            {
                fight.startCombat(entities, false, player);

                Assert.IsTrue(allie._health <= 0 || enemie._health <= 0);
            }
            else
            {
                // Gérer le cas où l'un des objets est null
                Console.WriteLine("Un ou plusieurs objets ne sont pas initialisés correctement.");
            }
        }
        [Test]
        public void Test5()
        {
            var shop = new Shop();
            var consoleInput = new StringReader(Environment.NewLine);
            var consoleOutput = new StringWriter();
            Console.SetIn(consoleInput);
            Console.SetOut(consoleOutput);

            // Act
            var selectedIndex = shop.ChooseProduct();

            // Assert
            Assert.AreEqual(0, selectedIndex);
        }
    }
}