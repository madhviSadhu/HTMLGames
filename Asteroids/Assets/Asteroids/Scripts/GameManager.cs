using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
    public class GameManager : MonoBehaviour
    {
        public PowerupManager powerupManager;
        public Text LevelText;
        public Text HighscoreText;
        public Text scoreText;
        public Text inGameMsg;
        public GameObject HUD;
        static GameManager instance;
        GameObject ShipPrefab;
        GameObject AsteroidBigPrefab;
        GameObject AsteroidSmallPrefab;
        ObjectPool bigAsteroidPool;
        ObjectPool smallAsteroidPool;
        SpaceshipBehaviour ship;
        const string cleared = "Level Cleared!";
        const string gameOverMsg = "GAME OVER";
        const string fmtLevel = "Level ";
        int level = 1;
        int numAsteroidsForLevel = 2;
        bool requestTitleScreen = true;

        void Awake()
        {

            ShipPrefab = Resources.Load<GameObject>("SpaceShip");
            AsteroidBigPrefab = Resources.Load<GameObject>("AsteroidBig");
            AsteroidSmallPrefab = Resources.Load<GameObject>("AsteroidSmall");
            SingletonInstance();
            bigAsteroidPool = ObjectPool.Build(AsteroidBigPrefab, 25, 50);
            smallAsteroidPool = ObjectPool.Build(AsteroidSmallPrefab, 25, 50);
        }

        void Start()
        {
            ship = SpaceshipBehaviour.Spawn(ShipPrefab);
            ship.Remove();
            StartCoroutine(GameLoop());
            StartCoroutine(powerupManager.SpawnPowerupsFor(ship.gameObject));
        }

        void OnEnable()
        {
            instance = this;
        }

        void SingletonInstance()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                throw new SingletonException("Only allowed one instance");
            }
        }

        IEnumerator GameLoop()
        {
            NewGame();
            while (true)
            {
                if (requestTitleScreen)
                {
                    requestTitleScreen = false;
                    yield return StartCoroutine(ShowTitleScreen());
                }
                yield return StartCoroutine(LevelStart());
                yield return StartCoroutine(LevelPlay());
                yield return StartCoroutine(LevelEnd());
                GC.Collect();
            }
        }

        IEnumerator ShowTitleScreen()
        {
            HighscoreText.text = "Score\n" + PlayerPrefs.GetString("Score", HighscoreText.text);
            while (!Input.GetKeyDown(KeyCode.Space)) yield return null;
        }

        IEnumerator LevelStart()
        {
            ship.Recover();
            LevelText.text = fmtLevel + level.ToString();
            yield return Pause.Long();
            SpawnAsteroids(numAsteroidsForLevel);
            HUD.SetActive(false);
        }

        IEnumerator LevelPlay()
        {
            inGameMsg.text = "";
            while (ship.IsAlive && AsteroidBehaviour.Any) yield return null;
        }

        IEnumerator LevelEnd()
        {
            PlayerPrefs.SetString("Score", scoreText.text);
            bool gameover = !ship.IsAlive;
            if (gameover)
            {
                inGameMsg.text = gameOverMsg;
                yield return Pause.Brief();
                Score.Tally();
                yield return Pause.Brief();
                Score.Reset();
                RemoveRemainingObject();
                powerupManager.DenyAllPower();
                NewGame();
            }
            else
            {
                inGameMsg.text = cleared;
                yield return Pause.Brief();
                Score.LevelCleared(level);
                yield return Pause.Brief();
                AdvanceLevel();
            }
            yield return Pause.Long();
        }

        void NewGame()
        {
            HUD.SetActive(true);
            level = 1;
            numAsteroidsForLevel = 2;
            requestTitleScreen = true;
        }

        void AdvanceLevel()
        {
            level++;
            numAsteroidsForLevel += level;
        }

        void SpawnAsteroids(int count)
        {
            for (int i = 0; i < count; i++)
            {
                ObjectPool bigOrSmall = i % 2 == 0 ? bigAsteroidPool : smallAsteroidPool;
                var asteroid = bigOrSmall.GetReusable<AsteroidBehaviour>();
                asteroid.Spawn();
            }
        }

        public static void SpawnSmallAsteroid(Vector3 position)
        {
            AsteroidBehaviour asteroid = instance.smallAsteroidPool.GetReusable<AsteroidBehaviour>();
            asteroid.SpawnAt(position);
        }

        void RemoveRemainingObject()
        {
            foreach (var a in FindObjectsOfType<RandomizeGameObject>())
                a.Remove();
        }
    }

    public static class Pause
    {
        public static WaitForSeconds Long()
        {
            return new WaitForSeconds(2f);
        }

        public static WaitForSeconds Brief()
        {
            return new WaitForSeconds(1f);
        }
    }