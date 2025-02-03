using UnityEngine;
using System.Collections.Generic;
using System.Linq;
public class EnemySpawnController : MonoBehaviour
{
    public GameObject player;

    public List<EnemyCost> enemies = new List<EnemyCost>();

    private int currentEnemies = 0;
    private float lastSpawnTime;


    private List<Transform> spawnPoints = new List<Transform>();

    private bool waveStarted;
    public bool waveEnded;
    public List<GameObject> enemiesToSpawn = new List<GameObject>();

    public List<Wave> waves = new List<Wave>();

    public int currentWave = 0;

    void Start()
    {
        //get all gameobjects with tag EnemySpawner
        spawnPoints = GameObject.FindGameObjectsWithTag("EnemySpawnLocation").Select(x => x.transform).ToList();
        lastSpawnTime = Time.time;
        waveStarted = false;
        generateWave();

    }

    void Update()
    {

        if (waveStarted)
        {
            Wave currWave = waves[currentWave];
            if (currentEnemies < currWave.maxEnemies && Time.time - lastSpawnTime > currWave.spawnInterval)
            {
                SpawnEnemy();
                lastSpawnTime = Time.time;
            }
            if (currentEnemies == 0 && enemiesToSpawn.Count == 0)
            {
                endWave();
            }
        }
    }

    public void startWave(){
        if (!waveStarted)
        {
            waveEnded = false;
            waveStarted = true;
        }
    }

    public void endWave()
    {
        waveStarted = false;
        waveEnded = true;
        currentWave++;
        generateWave();
    }

    public void generateWave()
    {
        int waveBudget = waves[currentWave].cost;
        //get min cost in enemies
        int minCost = enemies.Min(x => x.cost);
        while (waveBudget >= minCost)
        {
            int enemyIndex = Random.Range(0, enemies.Count);
            if (enemies[enemyIndex].cost <= waveBudget)
            {
                enemiesToSpawn.Add(enemies[enemyIndex].enemyPrefab);
                waveBudget -= enemies[enemyIndex].cost;
            }
        }
    }

    void SpawnEnemy()
    {
        if (enemiesToSpawn.Count == 0)
        {
            return;
        }
        int spawnPointIndex = Random.Range(0, spawnPoints.Count);

        GameObject enemy = Instantiate(enemiesToSpawn[0], spawnPoints[spawnPointIndex].position, Quaternion.identity);
        enemy.SetActive(true);
        enemy.GetComponent<Enemy>().target = player;
        Debug.Log("Enemy " + enemy.GetComponent<Enemy>().id + " spawned");
        Debug.Log("Enemy spawned at " + spawnPoints[spawnPointIndex].position);
        currentEnemies++;
        enemiesToSpawn.RemoveAt(0);
    }

    public void RemoveEnemy()
    {
        currentEnemies--;
    }
}

[System.Serializable]
public class EnemyCost
{
    public GameObject enemyPrefab;
    public int cost;
}

[System.Serializable]
public class Wave {
    public int cost;
    public int maxEnemies;
    public float spawnInterval;

}