using BEER2023.Enemy;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDirector : MonoBehaviour
{
    private static GameDirector __inst__;
    public static GameDirector Instance
    {
        get
        {
            return __inst__;
        }

        set
        {
            if (__inst__ != null)
            {
                Destroy(value);
                Debug.Log("Instance already created!");
            }
            else
            {
                __inst__ = value;
            }
        }
    }

    [Header("Components")]
    public EnemyController enemyPrefab;

    public List<SpawnPoint> spawnPoints = new List<SpawnPoint>();
    public List<SMBox> objectiveBoxes = new List<SMBox>();
    public List<Container> containers = new List<Container>();

    private List<EnemyController> enemies = new List<EnemyController>();

    public MenuManager prefabMenu;
    private MenuManager menuMan;

    // Have a list of pickable targets. If there's a new dropped one, we could analyze if its closer than others.
    // 
    //

    private void Awake()
    {
        Instance = this;
        menuMan = Instantiate(prefabMenu);
        menuMan.ToggleHide(false);
        // Populate all spawnpoints.
        var spawnPointList = FindObjectsOfType<SpawnPoint>();
        foreach (var item in spawnPointList)
        {
            spawnPoints.Add(item);
        }

        var boxesFound = FindObjectsOfType<SMBox>();
        foreach (var item in boxesFound)
        {
            if (item.addToListAuto) objectiveBoxes.Add(item);
        }

        var ctrs = FindObjectsOfType<Container>();
        foreach (var item in ctrs)
        {
            if (item.enabled) containers.Add(item);
        }
    }

    private void Start()
    {
        currentLives = maxLives;
        UpdateBoxesAmount(currentLives);
        StartSpawning();
    }

    private void Update()
    {
        // Open menu with ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Toggle pause menu.
            if (menuMan.isCanvasOn)
            {
                isSpawning = true;
                menuMan.BackToGame();
            }
            else
            {
                isSpawning = false;
                menuMan.OpenAsPause();
            }
        }
    }

    /// <summary>
    /// Boxes area
    /// </summary>
    #region Boxes and Objectives
    public void AddBox(SMBox box)
    {
        if (!box.enabled)
        {
            Debug.Log("Box is not enabled! " + box.gameObject.name);
            return;
        }
        objectiveBoxes.Add(box);
    }

    public void RemoveBox(SMBox box)
    {
        objectiveBoxes.Remove(box);
    }

    public int maxLives = 3;
    private int currentLives = 3;
    public void BoxScored()
    {
        currentLives--;
        Debug.Log("LOST A LIFE!");
        UpdateBoxesAmount(currentLives);

        if (currentLives <= 0)
        {
            // Absolute loss.
            Debug.Log("AND YOU LOST");
            TriggerLoss();
        }
    }

    private void TriggerWin()
    {
        // What win?
        // Save score somewhere in ONLINE?
        Debug.Log("YOU HAVE WON!");
    }

    private void TriggerLoss()
    {
        isSpawning = false;
        StopCoroutine(spawningFunction);
        // Show some kind of menu animation.
        Invoke("ReturnToMenu", 3f);
    }

    void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    #endregion

    #region Wave Spawner

    // Spawn bichos
    public WaveData currentWave;
    public int currentWaveInstructionIdx = 0;

    private IEnumerator spawningFunction;
    private bool isSpawning = true;
    public void SetIsSpawning(bool enable)
    {
        isSpawning = enable;
    }
    public void StartSpawning()
    {
        // We don't want to interrupt spawning in any way.
        // Unless we pause it.
        if (spawningFunction != null) return;

        spawningFunction = SpawnCoroutine();
        isSpawning = true;

        StartCoroutine(spawningFunction);
    }

    private IEnumerator SpawnCoroutine()
    {
        // Gather an instruction
        // Execute.
        // Wait a frame
        // Execute again
        WaveDataEntry nextInstruction;
        while (currentWave.waveInstructions.Count > currentWaveInstructionIdx)
        {
            while (!isSpawning) { yield return new WaitForEndOfFrame(); }
            nextInstruction = currentWave.waveInstructions[currentWaveInstructionIdx];
            bool waitInstruction = InterpretWaveDataEntry(nextInstruction);
            currentWaveInstructionIdx++;
            if (waitInstruction) { yield return new WaitForSecondsRealtime(nextInstruction.amount); }
        }
        Debug.Log("Stopped THE SUMMONING!");
        TriggerWin();
    }

    private bool InterpretWaveDataEntry(WaveDataEntry entry)
    {
        int amount = entry.amount;
        string info = entry.information;

        // There we go
        switch (entry.verb)
        {
            case Verbs.None:
                // DUH.
                break;
            case Verbs.Spawn:
                // SPAWN THERE!
                // For now don't use pooler? Bah use it anyway.
                for (int i = 0; i < amount; i++)
                {
                    Transform spawnPoint = GetRandomSpawnPoint<Transform>(entry.side); // Gather random spawnpoint.
                    EnemyController enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
                    var a = GetRandomFromList<SMBox>(objectiveBoxes);
                    if (a != null)
                    {
                        enemy.SetAttackTarget(a.transform);
                        enemy.SwapState(EnemyState.Startup);
                        enemies.Add(enemy);
                    }
                }
                break;
            case Verbs.ContainerRandom:
                // From the list of containers, gather one and open it.
                Debug.Log("Opening a random container!");
                for (int i = 0; i < amount; i++)
                {
                    Container chosenOne = GetRandomContainer(entry.side); // Gather random spawnpoint.
                    chosenOne.Open();
                    SpawnDanger(chosenOne.transform);
                }
                break;
            case Verbs.Container:
                var container = containers.Find(x => x.ID == amount);
                if (container == null) return false;
                if (entry.doorStatus == DoorStatus.Open) container.Open();
                else if (entry.doorStatus == DoorStatus.Close) container.Close();
                SpawnDanger(container.transform);
                break;
            case Verbs.Wait:
                // Wait command!
                Debug.Log("Wait command issued");
                return true;
            case Verbs.ContainerCloseAll:
                Debug.Log("Closing all containers");
                foreach (Container asd in containers)
                {
                    asd.Close();
                }
                break;
            case Verbs.PlaySound:
                AudioManager.Instance.PlaySound(info);
                break;
        }
        return false;
    }

    public DangerAlert prefab;
    public Canvas root;
    public void SpawnDanger(Transform danger)
    {
        var a = Instantiate(prefab, root.transform);
        a.Attach(danger);
    }

    public Transform GetObjective()
    {
        return GetRandomFromList<SMBox>(objectiveBoxes).transform;
    }

    private T GetRandomSpawnPoint<T>(AreaGroup side)
    {
        var sps = spawnPoints.FindAll(x => x.side == side);
        T point = sps[Random.Range(0, sps.Count)].GetComponent<T>();
        return point;
    }

    private T GetRandomFromList<T>(List<T> list) where T : MonoBehaviour
    {
        if (list.Count == 0) return null;
        return list[Random.Range(0, list.Count)];
    }

    private Container GetRandomContainer(AreaGroup side)
    {
        var sps = containers.FindAll(x => x.side == side);
        Container point = sps[Random.Range(0, sps.Count)];
        return point;
    }

    public Transform RequestExit()
    {
        // TODO not scalable. Max range is not generic.
        return GetRandomSpawnPoint<Transform>((AreaGroup)Random.Range(0, 5));
    }

    #endregion

    //private void OnValidate()
    //{
    //    if (updatePath)
    //    {
    //        debugEnemy.SetAttackTarget(testTarget);
    //        updatePath = false;
    //    }
    //    else if (spawnEnemy)
    //    {
    //        var newEnemy = Instantiate(enemyPrefab, spawnPoints[Random.Range(0, spawnPoints.Count)].transform.position, Quaternion.identity);
    //        // newEnemy.SwapState(EnemyState.Attacking);
    //        newEnemy.SetAttackTarget(testTarget);
    //        spawnEnemy = false;
    //    }
    //}

    #region Handle Canvas
    [Header("Canvas")]
    public TextMeshProUGUI boxesDisplay;

    public void UpdateBoxesAmount(int amountRemaining)
    {
        boxesDisplay.text = amountRemaining.ToString();
    }


    #endregion
}
