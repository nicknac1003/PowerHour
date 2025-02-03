using UnityEngine;

public class TimedDamageBuff : TimedBuff
{
    public PlayerController component;
    public TimedDamageBuff(ScriptableBuff buff, GameObject obj) : base(buff, obj)
    {
        //Getting MovementComponent, replace with your own implementation
        component = obj.GetComponent<PlayerController>();
    }

    protected override void ApplyEffect()
    {
        //Add speed increase to MovementComponent
        PlayerAttackScript[] playerAttackScript = component.GetComponentsInChildren<PlayerAttackScript>();
        foreach (PlayerAttackScript playerAttack in playerAttackScript)
        {
            playerAttack.increaseDamage(((DamageBuff)Buff).DamageIncrease);
        }
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