using UnityEngine;

[CreateAssetMenu(menuName = "Buffs/SpeedBuff", fileName="SpeedBuff", order = 0)]
public class SpeedBuff: ScriptableBuff
{
    public float SpeedIncrease;
    public float duration;
    public override TimedBuff InitializeBuff(GameObject obj)
    {
       return new TimedSpeedBuff(this, obj);
    }
}