using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class GameManager : MonoBehaviour
{
    public GameObject player;

    public static GameManager instance;

    public int currentWave = 0;

    public bool waveStarted;
    public bool drinking;

    public EnemySpawnController enemySpawnController;

    public DrinkSelector drinkSelector;
    [SerializeField] private TextMeshProUGUI roundText;
    [SerializeField] private TextMeshProUGUI enemyCountText;


    public void Start()
    {
        instance = this;
        waveStarted = false;
        drinking = false;
        //call function after 5 seconds
        Invoke("StartWave", 5);
        roundText.text = "Round: " + (currentWave + 1);
        enemyCountText.text = "Enemies Remaining: " + enemySpawnController.enemiesToSpawn.Count;

    }

    public void Update()
    {
        if (enemySpawnController.waveEnded)
        {
            waveStarted = false;
            if (!drinking)
            {
                drinking = true;
                drinkSelector.showDrinks();
            }

        }
        if (player.GetComponent<PlayerController>().isDead)
        {
            Debug.Log("Player is dead");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        roundText.text = "Round: " + (currentWave + 1);
        enemyCountText.text = "Enemies Remaining: " + (enemySpawnController.enemiesToSpawn.Count + enemySpawnController.currentEnemies);

    }
    public void EndDrinking()
    {
        drinkSelector.hideDrinks();
        StartWave();


    }

    public void StartWave()
    {
        Debug.Log("Wave Started " +waveStarted);
        if (!waveStarted)
        {
            Debug.Log("Starting wave");
            waveStarted = true;
            enemySpawnController.startWave();
            drinking = false;
        }
    }
}