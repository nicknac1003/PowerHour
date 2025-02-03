using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;

    public static GameManager instance;

    public int currentWave = 0;

    public bool waveStarted;
    public bool drinking;

    public EnemySpawnController enemySpawnController;

    public DrinkSelector drinkSelector;

    public void Start()
    {
        instance = this;
        waveStarted = false;
        drinking = false;
        //call function after 5 seconds
        Invoke("StartWave", 5);
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

    }
    public void EndDrinking()
    {
        drinkSelector.hideDrinks();
        StartWave();
        
    }

    public void StartWave()
    {
        if (!waveStarted)
        {
            Debug.Log("Starting wave");
            waveStarted = true;
            enemySpawnController.startWave();
            drinking = false;
        }
    }
}