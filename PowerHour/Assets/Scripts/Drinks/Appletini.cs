using UnityEngine;

[CreateAssetMenu(menuName = "Buffs/Appletini")]
public class Appletini : ScriptableBuff
{
    public override TimedBuff InitializeBuff(GameObject obj)
    {
       return new TimedAppletini(this, obj);
    }
}