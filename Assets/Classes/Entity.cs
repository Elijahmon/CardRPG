using System.Collections.Generic;

public class Entity
{
    protected bool isDead;
    protected Health health;
    protected Mana mana;
    protected Action action;

    protected Dictionary<Enums.AppliedEffect, int> appliedEffects;

    public virtual void Init()
    {
        health = new Health();
        health.Init();
        mana = new Mana();
        mana.Init();
        action = new Action();
        action.Init();

    }

    public virtual void Tick()
    {

    }

    #region DamageHandlers
    public virtual void TakePhysicalDamage(int amount)
    {
        UnityEngine.Debug.Log("Taking Physical Damage " + amount);
        int delta = amount;
        //Apply armor or other effects
        TakeDamage(delta);
    }

    public virtual void TakeMagicalDamage(int amount)
    {
        int delta = amount;
        //Effects
        TakeDamage(delta);
    }

    protected virtual bool TakeDamage(int delta)
    {
        if (health.GetCurrentHealth() - delta > 0)
        {
            health.ModifyCurrentHealth(-delta);
        }
        else
        {
            health.ModifyCurrentHealth(-health.GetCurrentHealth());
            UnityEngine.Debug.Log("Dead");
            //Dead
            return true;
        }

        return false;
    }
    #endregion

    #region AppliedEffectsHandlers
    public virtual void ApplyEffect(Enums.AppliedEffect effectType, int amount)
    {
        if (appliedEffects.ContainsKey(effectType) == true)
        {
            appliedEffects[effectType] += amount;
        }
        else
        {
            appliedEffects.Add(effectType, amount);
        }
    }
    #endregion

    public Health GetHealth()
    {
        return health;
    }

    public Mana GetMana()
    {
        return mana;
    }

    public Action GetAction()
    {
        return action;
    }

    #region Stat Gets
    public int GetCurrentHealth()
    {
        return health.GetCurrentHealth();
    }

    public int GetMaxHealth()
    {
        return health.GetMaxHealth();
    }

    public int GetCurrentMana()
    {
        return mana.GetCurrentMana();
    }

    public int GetMaxMana()
    {
        return mana.GetMaxMana();
    }

    public int GetCurrentAction()
    {
        return action.GetCurrentAction();
    }
    #endregion

    public bool IsDead()
    {
        return isDead;
    }
}
