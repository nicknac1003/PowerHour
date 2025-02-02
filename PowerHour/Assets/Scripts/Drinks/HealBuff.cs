using UnityEngine;

[CreateAssetMenu(menuName = "Buffs/HealBuff")]
public class HealBuff: ScriptableBuff
{
    public float HealAmount;
    public override TimedBuff InitializeBuff(GameObject obj)
    {
        return new TimedHealBuff(this, obj);
    }
}