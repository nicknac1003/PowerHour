using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class DrinkSelector : MonoBehaviour
{
    [SerializeField] private List<ScriptableBuff> drinkList;
    public List<GameObject> drinkOptions = new List<GameObject>();


    void Start()
    {
        updateDrinks();
    }

    public void updateDrinks(){

        List<ScriptableBuff> selectedDrinks = GetRandomDistinctElements(drinkList, drinkOptions.Count);

        for (int i = 0; i < drinkOptions.Count; i++)
        {
            ScriptableBuff drink = selectedDrinks[i];
            SelectDrink drinkComponent = drinkOptions[i].GetComponent<SelectDrink>();
            drinkComponent.setAssets(drink.BuffName, drink.Description, drink.Icon);
            drinkComponent.drink = drink;
        }
    }

    public void showDrinks(){
        Debug.Log("Showing drinks");
        gameObject.SetActive(true);
    }
    public void hideDrinks(){
        Debug.Log("Hiding drinks");
        gameObject.SetActive(false);
    }

    public List<T> GetRandomDistinctElements<T>(List<T> list, int n)
    {
        List<T> shuffledList = new List<T>(list);
        for (int i = 0; i < shuffledList.Count; i++)
        {
            int randomIndex = Random.Range(i, shuffledList.Count);
            T temp = shuffledList[i];
            shuffledList[i] = shuffledList[randomIndex];
            shuffledList[randomIndex] = temp;
        }
        return shuffledList.GetRange(0, n);
    }



}