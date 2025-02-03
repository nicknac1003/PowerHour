using UnityEngine;

[CreateAssetMenu(menuName = "Buffs/DamageBuff")]
public class DamageBuff: ScriptableBuff
{
    public float DamageIncrease;
    public override TimedBuff InitializeBuff(GameObject obj)
    {
        return new TimedDamageBuff(this, obj);
    }
}