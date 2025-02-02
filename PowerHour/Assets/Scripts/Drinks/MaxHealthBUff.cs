using UnityEngine;

[CreateAssetMenu(menuName = "Buffs/MaxHPBuff")]
public class MaxHealthBuff : ScriptableBuff
{
    public float HealthIncrease;
    public override TimedBuff InitializeBuff(GameObject obj)
    {
        return new TimedMaxHealthBuff(this, obj);
    }
}