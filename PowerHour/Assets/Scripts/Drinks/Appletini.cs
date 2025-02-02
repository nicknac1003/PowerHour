using UnityEngine;

[CreateAssetMenu(menuName = "Buffs/Appletini", fileName="Appletini", order = 0)]
public class Appletini : ScriptableBuff
{
    public override TimedBuff InitializeBuff(GameObject obj)
    {
       return new TimedAppletini(this, obj);
    }
}