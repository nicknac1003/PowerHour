using UnityEngine;
using System.Collections.Generic;

public class EnemySpawnController : MonoBehaviour
{
    public GameObject player;

    [SerializeField]
    List<GameObject> enemyPrefabs = new List<GameObject>();

    public float spawnInterval = 5.0f;
    public int maxEnemies = 10;
    public int currentEnemies = 0;
    private float lastSpawnTime;

    [SerializeField]
    List<Transform> spawnPoints = new List<Transform>();

    void Start()
    {
        lastSpawnTime = Time.time;
    }

    void Update()
    {
        if (currentEnemies < maxEnemies && Time.time - lastSpawnTime > spawnInterval)
        {
            SpawnEnemy();
            lastSpawnTime = Time.time;
        }
    }

    void SpawnEnemy()
    {
        int enemyPrefabIndex = Random.Range(0, enemyPrefabs.Count); 
        int spawnPointIndex = Random.Range(0, spawnPoints.Count);
        
        GameObject enemy = Instantiate(enemyPrefabs[enemyPrefabIndex], spawnPoints[spawnPointIndex].position, Quaternion.identity);
        enemy.SetActive(true);
        enemy.GetComponent<Enemy>().target = player;
        Debug.Log("Enemy " + enemy.GetComponent<Enemy>().id + " spawned");
        Debug.Log("Enemy spawned at " + spawnPoints[spawnPointIndex].position);
        currentEnemies++;
    }

    public void RemoveEnemy(GameObject enemy)
    {
        currentEnemies--;
    }
}