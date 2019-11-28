using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assets.Scripts.Attacks;
using Assets.Scripts.Units;
using Assets.Scripts.Units.Enemy;
using Assets.Scripts.Units.Soldier;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 649

namespace Assets.Scripts.Controllers
{
    public class GameController : MonoBehaviour
    {
        public static System.Random RandomGenerator = new System.Random();
        public MapGrid Map { get; private set; }
        public UIController UIController { get; private set; }
        public ProjectilesController ProjectilesController { get; private set; }
        public PlayerBase PlayerBase { get; private set; }
        public ScoreController ScoreController { get; private set; } = new ScoreController();
        public EconomyController EconomyController { get; set; } 

        private static GameMode _mode = GameMode.Play;

        [SerializeField] private LineRenderer _pathLineRenderer;
        [SerializeField] private EQCanvasController _eqCanvasController;

        [SerializeField] private GameObject _enemyPrefab;
        [SerializeField] private GameObject _mapFieldObjectPrefab;
        [SerializeField] private GameObject _projectilePrefab;

        [SerializeField] private Transform _mapParentTransform;
        [SerializeField] private Transform _enemiesParentTransform;
        [SerializeField] private Transform _soldiersParentTransform;
        [SerializeField] private Transform _projectilesParentTransform;

        [SerializeField] private Canvas _mainCanvas;
        [SerializeField] private Canvas _choiceModalCanvas;
        [SerializeField] private Text _choiceText;
        [SerializeField] private Text _quantsText;

        [SerializeField] private Button _choiceLeft;
        [SerializeField] private Button _choiceMid;
        [SerializeField] private Button _choiceRight;

        [SerializeField] private Text _nameText;
        [SerializeField] private Text _describText;
        [SerializeField] private Button _buyNewUnitButton;
        [SerializeField] private Button _sellUnitButton;
        [SerializeField] private Button _buyRandomItemButton;

        [SerializeField] private Vector2 _mapSize;
        [SerializeField] private Sprite _mapFieldSprite;

        [SerializeField] private Sprite[] _soldierSprites;
        [SerializeField] private Sprite[] _enemySprites;

        [SerializeField] private Material _plasMaterial;
        [SerializeField] private Material _laserMaterial;
        [SerializeField] private Material _ballisticMaterial;
        [SerializeField] private Mesh _projectileMesh;


        public EnemiesController EnemiesController { get; set; }
        public SoldiersController SoldiersController { get; set; }

        public static GameMode Mode
        {
            get => _mode;
            set => SetMode(value);
        }

        private static void SetMode(GameMode value)
        {
            _mode = value;
        }

        private void Start()
        {
            Map = new MapGrid(((int)_mapSize.x, (int)_mapSize.y), _mapFieldObjectPrefab, _mapParentTransform, this, _mapFieldSprite, _pathLineRenderer);
            ProjectilesController = new ProjectilesController(_plasMaterial,_laserMaterial, _ballisticMaterial, _projectileMesh);
            EconomyController = new EconomyController(_quantsText);
            UIController = new UIController(this, _eqCanvasController, _choiceModalCanvas, _choiceText, _choiceLeft, _choiceMid, _choiceRight, _buyNewUnitButton, _sellUnitButton, _buyRandomItemButton, _nameText, _describText);
            UIController.Instantiate();

            EnemiesController = new EnemiesController(this, _enemyPrefab, _enemiesParentTransform, _enemySprites[0], _enemySprites[1], _enemySprites[2]);
            SoldiersController = new SoldiersController(this, EnemiesController, _soldierSprites);
            EnemiesController.ParentCanvas = _mainCanvas;
        }

        private void Update()
        {
            ProcessInputs();
            ProcessGameLoop();
        }

        private void ProcessInputs()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                switch (Mode)
                {
                    case GameMode.Play:
                        Mode = GameMode.Pause;
                        break;
                    case GameMode.Pause:
                        Mode = GameMode.Play;
                        break;
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha1)) GameSpeed = 1;
            if (Input.GetKeyDown(KeyCode.Alpha2)) GameSpeed = 2;
            if (Input.GetKeyDown(KeyCode.Alpha3)) GameSpeed = 4;
            if (Input.GetKeyDown(KeyCode.Alpha4)) GameSpeed = 8;
            if (Input.GetKeyDown(KeyCode.X)) _eqCanvasController.AddNewItem(ConsumableItemsList.AllConsumableItems[RandomGenerator.Next(0, ConsumableItemsList.AllConsumableItems.Length)], this);
            if (Input.GetKeyDown(KeyCode.C))
            {
                foreach (var unit in SoldiersController.Units)
                {
                    foreach (var item in unit._equipedItems.ToArray())
                    {
                        unit.RemoveItem(item);
                    }
                }
            }
        }

        public static int GameSpeed = 1;

        private void ProcessGameLoop()
        {
            if (Mode == GameMode.Play)
            {
                for (int i = 0; i < GameSpeed; i++)
                {
                    SoldiersController.ProcessActions();
                    EnemiesController.ProcessActions();
                    CooldownController.UpdateCooldowns(GameSpeed);
                    GenerateWave();
                }
            }

            UIController.UpgradeManager.Render();
        }

        private float _enemySpawnCooldown = 0.45f;
        private float _currentEnemySpawnCooldown;
        [SerializeField]private float _powerLevel = 1;
        [SerializeField] private int _enemiesSpawned = 0;
        private void GenerateWave()
        {

            if (_currentEnemySpawnCooldown > 0)
            {
                _currentEnemySpawnCooldown -= Time.deltaTime;
                return;
            }

            EnemiesController.SpawnEnemy(_powerLevel);
            _powerLevel = 1 + ((float)math.pow(_enemiesSpawned,1.1) / 300 + math.cos(_enemiesSpawned*0.1f)*0.2f) * 0.25f;
            _enemiesSpawned++;
            if (_spawnTwo)
            {
                EnemiesController.SpawnEnemy(_powerLevel);
                _powerLevel = 1 + ((float)math.pow(_enemiesSpawned, 1.1) / 300 + math.cos(_enemiesSpawned * 0.1f) * 0.2f) * 0.25f;
                _enemiesSpawned++;
            }
            _spawnTwo = !_spawnTwo;

            _currentEnemySpawnCooldown = _enemySpawnCooldown;
        }

        private bool _spawnTwo;
    }

    public enum GameMode
    {
        Play = 0,
        Building = 1,
        Upgrading = 2,
        Pause = 3
    }
}