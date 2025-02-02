using UnityEngine;

public class TimedAppletini : TimedBuff
{
    public Transform arm;
    public TimedAppletini(ScriptableBuff buff, GameObject obj) : base(buff, obj)
    {
        arm = obj.GetComponent<BuffManager>().rightArm;
    }

    protected override void ApplyEffect()
    {
        arm.localScale = new Vector3(1.5f, 2.0f, 1.5f);
    }

    public override void End()
    {
        //Do nothing - permanent buff
        arm.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }
    protected override void ApplyTick()
    {
        //Do nothing
    }
}