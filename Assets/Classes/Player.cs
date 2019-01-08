using System.Collections.Generic;

public class Player : Entity
{

    int level;
    bool isDead;
    Enums.Class myClass;
    CombatDeck deck;
    CombatDeck currentDeck;

    Enums.Equipment head;
    Enums.Equipment body;

    Enums.Equipment rightHand;
    Enums.Equipment leftHand;
    Enums.Equipment twoHand;
    Enums.Equipment dualWield;

    Enums.Equipment acessory1;
    Enums.Equipment acessory2;
    Enums.Equipment acessory3;

    List<Enums.Trinket> equippedTrinkets;

    List<Enums.Equipment> unEquippedItems;

    int startingCardCount = 3;

    public override void Init()
    {
        level = 1;
        health = new Health();
        health.Init();
        action = new Action();
        action.Init();
        mana = new Mana();
        mana.Init();
        UpdateStatsUI();

        deck = new CombatDeck();
        deck.Init();
        currentDeck = new CombatDeck(deck);
        equippedTrinkets = new List<Enums.Trinket>();
        unEquippedItems = new List<Enums.Equipment>();
        appliedEffects = new Dictionary<Enums.AppliedEffect, int>();
        isDead = false;
    }

    public override void Tick()
    {

    }

    public void SetClass(Enums.Class c)
    {
        myClass = c;
    }

    private void UpdateStatsUI()
    {
        Main.instance.UpdateUI(health, mana, action);
    }

    public int GetStartingCardCount()
    {
        return startingCardCount;
    }

    public CombatDeck GetCurrentCombatDeck()
    {
        return currentDeck;
    }

    public void SetPlayerLevel(int level)
    {
        this.level = level;
    }

    public void GainStat(Enums.StatType stat, int amount)
    {
        switch(stat)
        {
            case Enums.StatType.Health:
                health.IncreaseMaxHealth(amount);
                break;
            case Enums.StatType.Action:
                action.IncreaseStartingAction(amount);
                break;
            case Enums.StatType.MaxMana:
                mana.IncreaseMaxMana(amount);
                break;
        }
    }

    public void AddCardsToDeck(Enums.Card type, int amount)
    {
        for(int i = amount; i > 0; i--)
        {
            AddCardToDeck(type);
        }
    }

    private void AddCardToDeck(Enums.Card c)
    {
        Card card = new Card(c);
        deck.AddCardToBottom(card);
        if(card.isCombatCard() == true)
        {
            GetCurrentCombatDeck().AddCardToBottom(card);
        }
    }

    public void GainEquipment(Enums.Equipment e)
    {
        //TODO: Handle Gain Equipment
    }

     #region DamageHandlers
    public override void TakePhysicalDamage(int amount)
    {
        //UnityEngine.Debug.Log("Taking Physical Damage " + amount);
        int delta = amount;
        //Apply armor or other effects
        TakeDamage(delta);
    }

    public override void TakeMagicalDamage(int amount)
    {
        int delta = amount;
        //Effects
        TakeDamage(delta);
    }

    protected override bool TakeDamage(int delta)
    {
        isDead = base.TakeDamage(delta);

        MainUIController.instance.UpdateStatsUI(health, mana, action);

        return isDead;
    }
    #endregion

     #region AppliedEffectsHandlers
    public override void ApplyEffect(Enums.AppliedEffect effectType, int amount)
    {

        base.ApplyEffect(effectType, amount);
        //TODO: UI For player effects
    }
    #endregion
}
