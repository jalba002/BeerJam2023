using BEER2023.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public bool updatePath = false;
    public bool spawnEnemy = false;
    [Header("Debug Components")]
    public EnemyController debugEnemy;
    public Transform testTarget;

    public EnemyController enemyPrefab;

    public List<SpawnPoint> spawnPoints = new List<SpawnPoint>();
    public List<SMBox> objectiveBoxes = new List<SMBox>();

    // Have a list of pickable targets. If there's a new dropped one, we could analyze if its closer than others.
    // 

    private void Awake()
    {
        Instance = this;
        // Populate all spawnpoints.
        var spawnPointList = FindObjectsOfType<SpawnPoint>();
        foreach (var item in spawnPointList)
        {
            spawnPoints.Add(item);
        }

        var boxesFound = FindObjectsOfType<SMBox>();
        foreach (var item in boxesFound)
        {
            if (item.enabled) objectiveBoxes.Add(item);
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
    #endregion

    private void OnValidate()
    {
        if (updatePath)
        {
            debugEnemy.SetAttackTarget(testTarget);
            updatePath = false;
        }
        else if (spawnEnemy)
        {
            var newEnemy = Instantiate(enemyPrefab, spawnPoints[Random.Range(0, spawnPoints.Count)].transform.position, Quaternion.identity);
            // newEnemy.SwapState(EnemyState.Attacking);
            newEnemy.SetAttackTarget(testTarget);
            spawnEnemy = false;
        }
    }
}
