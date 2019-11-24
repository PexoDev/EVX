using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assets.Scripts.Attacks;
using Assets.Scripts.Units;
using Assets.Scripts.Units.Enemy;
using Assets.Scripts.Units.Soldier;
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

        [SerializeField] private Button _choiceLeft;
        [SerializeField] private Button _choiceMid;
        [SerializeField] private Button _choiceRight;

        [SerializeField] private Text _nameText;
        [SerializeField] private Text _describText;
        [SerializeField] private Button _hqSoldiersTilesButton;

        [SerializeField] private Vector2 _mapSize;
        [SerializeField] private Sprite _mapFieldSprite;

        [SerializeField] private Sprite[] _soldierSprites;
        [SerializeField] private Sprite _enemySprite;

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
            EnemiesController = new EnemiesController(this, _enemyPrefab, _enemiesParentTransform, _enemySprite, _enemySprite, _enemySprite);
            SoldiersController = new SoldiersController(this, EnemiesController, _soldierSprites);
            UIController = new UIController(this, _choiceModalCanvas, _choiceText, _choiceLeft, _choiceMid, _choiceRight, _hqSoldiersTilesButton, _nameText, _describText);
            UIController.Instantiate();

            Map = new MapGrid(((int)_mapSize.x, (int)_mapSize.y), _mapFieldObjectPrefab, _mapParentTransform, this, _mapFieldSprite, _pathLineRenderer);

            ProjectilesController = new ProjectilesController(_plasMaterial,_laserMaterial, _ballisticMaterial, _projectileMesh);

            EnemiesController.ParentCanvas = _mainCanvas;
        }

        private void Update()
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
            if (Input.GetKeyDown(KeyCode.X)) _eqCanvasController.AddNewItem(ConsumableItemsList.AllConsumableItems[RandomGenerator.Next(0,ConsumableItemsList.AllConsumableItems.Length)], this);
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

            ProcessGameLoop();
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

        private float _enemySpawnCooldown = 3f;
        private float _currentEnemySpawnCooldown;

        private void GenerateWave()
        {

            if (_currentEnemySpawnCooldown > 0)
            {
                _currentEnemySpawnCooldown -= Time.deltaTime;
                return;
            }

            SpawnEnemy();
            _currentEnemySpawnCooldown = _enemySpawnCooldown;
        }

        private float _difficulty = 0.001f;
        private void SpawnEnemy()
        {
            _difficulty += 0.005f;

            UnitParameters defaultEnemySettings;
            if (RandomGenerator.Next(0, 101) < 25)
            {
                defaultEnemySettings = new UnitParameters { MovementSpeed = 0.0012f, DefaultDamage = 0, AttackRange = 3, Health = 100, AttacksPerSecond = 0.5f, ClipSize = 10};
                _enemySpawnCooldown = 1f;
            }
            else 
            {
                defaultEnemySettings = new UnitParameters { MovementSpeed = 0.010f, DefaultDamage = 0, AttackRange = 3, Health = 10, AttacksPerSecond = 2f, ClipSize = 10 };
                _enemySpawnCooldown = 0.25f;
            }

            defaultEnemySettings *= 1 + _difficulty;

            for(int i = 0; i < 2; i++)
                EnemiesController.SpawnEnemy(new DummyEnemy(Map.Path.ToArray(), SoldiersController, EnemiesController, PlayerBase, (DamageType)RandomGenerator.Next(1,4), (HealthType)RandomGenerator.Next(1, 4), defaultEnemySettings, 1, _enemySprite));
        }
    }

    public enum GameMode
    {
        Play = 0,
        Building = 1,
        Upgrading = 2,
        Pause = 3
    }
}