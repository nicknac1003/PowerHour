using UnityEngine;

public class TimedSpeedBuff : TimedBuff
{

    public PlayerController component;
    public TimedSpeedBuff(ScriptableBuff buff, GameObject obj) : base(buff, obj)
    {
        //Getting MovementComponent, replace with your own implementation
        component = obj.GetComponent<PlayerController>();
    }

    protected override void ApplyEffect()
    {
        //Add speed increase to MovementComponent
        component.walkSpeed += ((SpeedBuff)Buff).SpeedIncrease;
    }

    public override void End()
    {
        //Revert speed increase
        component.walkSpeed -= ((SpeedBuff)Buff).SpeedIncrease;
        EffectStacks = 0;
    }
     protected override void ApplyTick()
     {
         //Do nothing
     }
}