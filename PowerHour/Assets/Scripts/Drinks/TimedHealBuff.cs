using UnityEngine;

public class TimedHealBuff : TimedBuff
{
    public PlayerController component;
    public TimedHealBuff(ScriptableBuff buff, GameObject obj) : base(buff, obj)
    {
        //Getting MovementComponent, replace with your own implementation
        component = obj.GetComponent<PlayerController>();
    }

    protected override void ApplyEffect()
    {
        //Add speed increase to MovementComponent
        component.Heal(((HealBuff)Buff).HealAmount);
    }

    public override void End()
    {
        //Revert speed increase
        EffectStacks = 0;
    }
    protected override void ApplyTick()
    {
        //Do nothing
    }
}