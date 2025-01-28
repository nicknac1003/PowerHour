using UnityEngine;
using System.Collections.Generic;

public class EnemySpawnController : MonoBehaviour
{
    public GameObject player;

    public List<EnemyCost> enemies = new List<EnemyCost>();

    public float spawnInterval = 5.0f;
    public int maxEnemies = 10;
    public int currentEnemies = 0;
    private float lastSpawnTime;

    [SerializeField]
    List<Transform> spawnPoints = new List<Transform>();

    private bool waveStarted;
    public List<GameObject> enemiesToSpawn = new List<GameObject>();


    void Start()
    {
        lastSpawnTime = Time.time;
        waveStarted = false;
    }

    void Update()
    {
        if (!waveStarted)
        {
            generateWave();
        } else {
            if (currentEnemies < maxEnemies && Time.time - lastSpawnTime > spawnInterval)
            {
                SpawnEnemy();
                lastSpawnTime = Time.time;
            }
        }


    }
    
    public void generateWave()
    {
        waveStarted = true;
        int waveBudget = 10;
        while (waveBudget > 0)
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

    public void RemoveEnemy(GameObject enemy)
    {
        currentEnemies--;
    }
}

[System.Serializable]
public class EnemyCost {
    public GameObject enemyPrefab;
    public int cost;
}