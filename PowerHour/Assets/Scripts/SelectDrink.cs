using UnityEngine;
using UnityEngine.UI;
using TMPro;
class SelectDrink : MonoBehaviour
{
    public PlayerController player;
    public ScriptableBuff drink;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public RawImage icon;

    public void ApplyDrink()
    {
        player.AddBuff(drink.InitializeBuff(player.gameObject));
    }

    public void setAssets(string name, string desc, Texture img)
    {
        nameText.text = name;
        descriptionText.text = desc;
        icon.texture = img;

    }
}