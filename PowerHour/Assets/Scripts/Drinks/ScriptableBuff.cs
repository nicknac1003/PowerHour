using UnityEngine;
public abstract class ScriptableBuff : ScriptableObject
{
    public bool IsPermanent;

    //Time duration of the buff in seconds.
    public float Duration;

    //Duration is increased each time the buff is applied.

    public bool IsDurationStacked;

    //Effect value is increased each time the buff is applied.
    public bool IsEffectStacked;

    public string BuffName;

    public Texture Icon;

    public string Description;

    public abstract TimedBuff InitializeBuff(GameObject obj);

}