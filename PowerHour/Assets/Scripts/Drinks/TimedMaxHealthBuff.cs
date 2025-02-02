using UnityEngine;

public class TimedMaxHealthBuff : TimedBuff
{
    public PlayerController component;
    public TimedMaxHealthBuff(ScriptableBuff buff, GameObject obj) : base(buff, obj)
    {
        //Getting HealthComponent, replace with your own implementation
        component = obj.GetComponent<PlayerController>();
    }

    protected override void ApplyEffect()
    {
        //Add max health increase to HealthComponent
        component.maxHealth += ((MaxHealthBuff)Buff).HealthIncrease;
        component.Heal(((MaxHealthBuff)Buff).HealthIncrease);
    }

    public override void End()
    {
        //Revert max health increase
        EffectStacks = 0;
    }
    protected override void ApplyTick()
    {
        //Do nothing
    }
}