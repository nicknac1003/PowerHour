using UnityEngine;
public abstract class TimedBuff
{
    // How often ApplyTick() is called (in seconds)
    protected float TickRate = 0.5f;
    protected float Duration;
    protected int EffectStacks;
    public ScriptableBuff Buff { get; }
    protected readonly GameObject Obj;
    public bool IsFinished;

    private float _timeSinceLastTick;

    public TimedBuff(ScriptableBuff buff, GameObject obj)
    {
        Buff = buff;
        Obj = obj;
        if (Buff.IsPermanent)
        {
            Duration = 1;
        }
    }

    public void Tick(float delta)
    {
        if (!Buff.IsPermanent)
        {
            Duration -= delta;
        }
        
        if (_timeSinceLastTick >= TickRate)
        {
            ApplyTick();
            _timeSinceLastTick = 0;
        }
        if (Duration <= 0)
        {
            End();
            IsFinished = true;
        }
    }

    /**
     * Activates buff or extends duration if ScriptableBuff has IsDurationStacked or IsEffectStacked set to true.
     */
    public void Activate()
    {
        if (Buff.IsPermanent)
        {
            ApplyEffect();
            return;
        }
        if (Buff.IsEffectStacked || Duration <= 0)
        {
            ApplyEffect();
            EffectStacks++;
        }
        
        if (Buff.IsDurationStacked || Duration <= 0)
        {
            Duration += Buff.Duration;
        }
    }
    protected abstract void ApplyEffect();
     // Called every TickRate seconds. Can be used for things such as damage over time or healing over time.
    protected abstract void ApplyTick();
    public abstract void End();
}