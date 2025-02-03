using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
class SelectDrink : MonoBehaviour
{
    public PlayerController player;
    public ScriptableBuff drink;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public RawImage icon;

    public Button button;

    public void ApplyDrink()
    {
        player.AddBuff(drink.InitializeBuff(player.gameObject));
    }

    public void setAssets(string name, Texture img)
    {

        nameText.text = name;
        descriptionText.text = drink.GetFormattedDescription();
        icon.texture = img;

    }

    public void EnableButtonAfterDelay(float delay)
    {
        StartCoroutine(EnableButtonCoroutine(delay));
    }

    private IEnumerator EnableButtonCoroutine(float delay)
    {
        button.interactable = false;
        yield return new WaitForSeconds(delay);
        button.interactable = true;
    }
}