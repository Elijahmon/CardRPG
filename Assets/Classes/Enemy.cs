using System.Collections.Generic;

public class Enemy : Entity
{
    public Enums.EnemyType enemyType;

    string name;
    bool isDead;

    CombatDeck deck;

    CombatHand currentHand;
    CombatHand discardHand;
    CombatHand exileHand;
    EnemyUIController myUIController;

    public override void Init()
    {
        health = new Health();
        health.Init();
        mana = new Mana();
        mana.Init();
        action = new Action();
        action.Init();

        deck = new CombatDeck();
        deck.Init();

        currentHand = new CombatHand();
        discardHand = new CombatHand();
        exileHand = new CombatHand();

    }

    public void Init(EnemyConfig config)
    {
        enemyType = config.type;

        name = config.name;

        health = new Health();
        health.Init(config.health);
        mana = new Mana();
        mana.Init(config.mana);
        action = new Action();
        action.Init(config.action);

        deck = new CombatDeck();
        deck.Init(config.deck);
        deck.Shuffle();

        currentHand = new CombatHand();
        discardHand = new CombatHand();
        exileHand = new CombatHand();
        appliedEffects = new Dictionary<Enums.AppliedEffect, int>();
        isDead = false;
    }

    public override void Tick()
    {

    }

    public CombatDeck GetDeck()
    {
        return deck;
    }

    public Enums.EnemyType GetEnemyType()
    {
        return enemyType;
    }

    #region DamageHandlers
    public override void TakePhysicalDamage(int amount)
    {
        //UnityEngine.Debug.Log("Taking Physical Damage " + amount);
        int delta = amount;
        //TODO: Apply armor or other effects
        TakeDamage(delta);
    }

    public override void TakeMagicalDamage(int amount)
    {
        int delta = amount;
        //TODO: Magic Resistance Effects
        TakeDamage(delta);
    }

    protected override bool TakeDamage(int delta)
    {
        isDead = base.TakeDamage(delta);

        myUIController.UpdateHealth(health.GetCurrentHealth(), health.GetMaxHealth());
        
        if(isDead == true)
        {
            myUIController.Deactivate();
        }
        return isDead;
    }
    #endregion

    #region AppliedEffectsHandlers
    public override void ApplyEffect(Enums.AppliedEffect effectType, int amount)
    {
        base.ApplyEffect(effectType, amount);

        myUIController.UpdateActiveEffects(appliedEffects);
    }
    #endregion

    public void SetUIController(EnemyUIController controller)
    {
        myUIController = controller;
    }

    public EnemyUIController GetUIController()
    {
        return myUIController;
    }

    public string GetName()
    {
        return name;
    }

    public CombatHand GetCurrentHand()
    {
        return currentHand;
    }

    public CombatHand GetDiscard()
    {
        return discardHand;
    }

}
