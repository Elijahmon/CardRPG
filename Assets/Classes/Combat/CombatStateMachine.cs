using System.Collections;
using System.Collections.Generic;

public class CombatStateMachine {

    CombatDeck playerSideDeck;
    CombatHand playerHand;
    List<Enemy> enemies;

    int turn;
    bool isPlayerTurn;
    bool isActive = false;

    CombatDeck playerDiscardPile;
    CombatDeck playerExilePile;

    EncounterConfig currentConfig;

    AIStateMachine aIStateMachine;

    bool useAI = false;
	public void Init()
    {
        aIStateMachine = new AIStateMachine();
        aIStateMachine.Init();
        enemies = new List<Enemy>();
        isActive = false;
    }

    public void StartCombatStateMachine(EncounterConfig combatConfig, Player p)
    {
        useAI = true;
        currentConfig = combatConfig;
        playerDiscardPile = new CombatDeck();
        playerExilePile = new CombatDeck();
        playerHand = new CombatHand();

        Main.instance.ShufflePlayerCurrentCombatDeck(p);
        playerSideDeck = new CombatDeck(p.GetCurrentCombatDeck());
        //TODO: Load Combat Configs and init enemies
        enemies.Clear();
        foreach (var enemy in combatConfig.enemies)
        {
            //UnityEngine.Debug.Log(ConfigHandler.enemyConfigs[enemy].type);
            Enemy e = new Enemy();
            e.Init(ConfigHandler.enemyConfigs[enemy]);
            enemies.Add(e);
        }
        
        MainUIController.instance.ActivateEnemyCanvas(enemies);
        DrawInitialHands(p.GetStartingCardCount(), 3); //TODO: Config Enemy Starting Hands
        turn = 0;
        isPlayerTurn = true;
        isActive = true;
    }
    
    public void StartTimelineStateMachine(EncounterConfig timelineConfig, Player p)
    {
        useAI = false;
        currentConfig = timelineConfig;
        playerDiscardPile = new CombatDeck();
        playerExilePile = new CombatDeck();
        playerHand = new CombatHand();
        playerSideDeck = new CombatDeck(p.GetCurrentCombatDeck());

        enemies.Clear();
        foreach(var enemy in timelineConfig.enemies)
        {
            //UnityEngine.Debug.Log(ConfigHandler.enemyConfigs[enemy].type);
            Enemy e = new Enemy();
            e.Init(ConfigHandler.enemyConfigs[enemy]);
            enemies.Add(e);
        }
        isPlayerTurn = false;
        isActive = true;
    }

    public void StopTimelineStateMachine(Player p)
    {
        if (isActive == true)
        {
            isActive = false;
            enemies.Clear();
            isPlayerTurn = false;
            ProcessEndOfCombat(currentConfig, p);
            currentConfig = null;
        }
    }

    public void ScriptedActionActivatePlayerTurn()
    {
        isPlayerTurn = true;
    }

    void ProcessEndOfCombat(EncounterConfig config, Player p)
    {
        ClearHands();
        UnityEngine.Debug.Log("Processing End of Combat");
    }

    public bool IsPlayerTurn()
    {
        return isPlayerTurn;
    }

    public bool IsCombatActive()
    {
        return isActive;
    }

    public void EndTurn(Player p)
    {
        isPlayerTurn = false;
        EnemyDrawTurnStart(p);
        ProcessEndOfTurnEffects();
    }

    public void EnemyEndTurn(Player p)
    {
        turn++;
        UnityEngine.Debug.Log("Ending Enemy Turn");
        ProcessEndOfTurnEffects();
        PlayerDrawTurnStart(p);
        isPlayerTurn = true;
    }

    public void Tick()
    {
        if(isActive == true)
        {
            if(useAI == true)
            {
                if(isPlayerTurn == false)
                {
                    aIStateMachine.Tick();
                }
            }
        }
    }

    public void DrawEnemyDeck()
    {
        List<Card> cardsDrawn = new List<Card>();
        foreach(Enemy e in enemies)
        {
            int amount = e.GetDeck().DeckCount();
            for (int i = 0; i < amount; i++)
            {
            cardsDrawn.Add(e.GetDeck().RemoveTopCard());
            playerHand.AddCardToBottom(cardsDrawn[i]);
            }
        }
        Main.instance.DrawCardsToPlayerHand(cardsDrawn); 
    }


    #region CardPlaying

    private void PlayCard(Entity source, Entity target, Card c)
    {
        if(source.GetType() == typeof(Player))
        {
            if(isPlayerTurn == false)
            {
                return;
            }
        } 

        foreach (var effect in c.GetEffects())
        {
            PlayCardEffect(source, target, effect);
        }  
    }

    private void PlayCard(Entity source, Card c, Entity[] targets)
    {
        foreach(Entity target in targets)
        {
            PlayCard(source, target, c);
        }
    }

    private void PlayCardEffect(Entity source, Entity target, CardEffectConfig effect)
    {
        //Player Unique Effects
        if(target.GetType() == typeof(Player))
        {
            Player p = (Player)target;
            switch(effect.effect)
            {
                case Enums.CardEffect.SelectClass:
                    p.SetClass(effect.selectedClass);
                    break;
                case Enums.CardEffect.StarterLevel:
                    p.SetPlayerLevel(effect.amount);
                    break;
                case Enums.CardEffect.GainStat:
                    p.GainStat(effect.stat, effect.amount);
                    break;
                case Enums.CardEffect.GainCard:
                    p.AddCardsToDeck(effect.card, effect.amount);
                    break;
                case Enums.CardEffect.GainEquipment:
                    p.GainEquipment(effect.equipment);
                    break;
                case Enums.CardEffect.GainTrinket:
                    //TODO Gain Trinket
                    break;
            }
        }
        
        switch(effect.effect)
        {
            case Enums.CardEffect.PhysicalDamage:
                target.TakePhysicalDamage(effect.amount);
                break;
            case Enums.CardEffect.MagicDamage:
                target.TakeMagicalDamage(effect.amount);
                break;
            case Enums.CardEffect.ApplyEffect:
                target.ApplyEffect(effect.appliedEffect, effect.amount);
                break;
        }

        if(target.IsDead() == true)
        {
            HandleDeadEntity(target);
        }
    }

    private void PlayCardEffect(Enemy e, CardEffectConfig effect)
    {
        switch(effect.effect)
        {
            /* TODO: Fix self cast effects for enemies
            case Enums.CardEffect.GainStat:
                e.GainStat(effect.stat, effect.amount);
                break;
            case Enums.CardEffect.GainCard:
                e.AddCardsToDeck(effect.card, effect.amount);
                break;
            case Enums.CardEffect.GainEquipment:
                e.GainEquipment(effect.equipment);
                break;
            case Enums.CardEffect.GainTrinket:
                //TODO Gain Trinket
                break;
                */
        }
    }

    public void PlayCardSingleTarget(Entity source, Entity target, Card c)
    {
        PlayCard(source, target, c);
        CleanUpPlayedCard(source, c);
    }

    public void PlayCardMultiTarget(Entity source, Card c)
    {
        Entity[] targets = new Entity[enemies.Count];
        for(int i = 0; i < targets.Length; i++)
        {

            targets[i] = (Entity)enemies[i];
        }
        PlayCard(source, c, targets);
        CleanUpPlayedCard(source, c);
    }

    private void CleanUpPlayedCard(Entity source, Card c)
    {
        //PLayer played card now discard
            if(source.GetType() == typeof(Player))
            {
                playerDiscardPile.AddCardToBottom(playerHand.Discard(c));
            }
            else //Enemy played now discard
            {
                Enemy e = (Enemy)source;
                e.GetDiscard().AddCardToBottom(e.GetCurrentHand().Discard(c));
            }
    }

    #endregion

    private void DrawInitialHands(int playerCards, int enemyCards)
    {
        List<Card> cardsDrawn = new List<Card>();
        for (int i = 0; i < playerCards; i++)
        {
            cardsDrawn.Add(playerSideDeck.RemoveTopCard());
            playerHand.AddCardToBottom(cardsDrawn[i]);
        }
        
        Main.instance.DrawCardsToPlayerHand(cardsDrawn);

        cardsDrawn.Clear();
        foreach(Enemy e in enemies)
        {
            for (int i = 0; i < enemyCards; i++)
            {
                cardsDrawn.Add(e.GetDeck().RemoveTopCard());
                e.GetCurrentHand().AddCardToBottom(cardsDrawn[i]);
                
            }
        }
        

        //Main.instance.DrawCardsToEnemyHand(cardsDrawn);
    }

    void ClearHands()
    {
        //UnityEngine.Debug.Log("clearing hand");
        playerHand.Clear();
        foreach(Enemy e in enemies)
        {
            e.GetCurrentHand().Clear();
        }
        Main.instance.ClearHands();
        playerSideDeck = null;
    }

    void PlayerDrawTurnStart(Player p)
    {
        List<Card> cardsDrawn = new List<Card>();
        if(playerSideDeck.DeckCount() < p.GetStartingCardCount()) //If they need to draw more than decksize draw what you can and then shuffle the discard back in
        {
            for (int i = 0; i < playerSideDeck.DeckCount(); i++) //TODO:? Draw per turn for player is seperate
            {
                cardsDrawn.Add(playerSideDeck.RemoveTopCard());
                playerHand.AddCardToBottom(cardsDrawn[i]);
            }
            for(int i = 0; i < playerDiscardPile.DeckCount(); i++)
            {
                playerSideDeck.AddCardToBottom(playerDiscardPile.RemoveTopCard());
            }
            playerSideDeck.Shuffle();

            int remainingCardsToDraw = p.GetStartingCardCount() - cardsDrawn.Count;
            for(int i = 0; i < remainingCardsToDraw; i++)
            {
                cardsDrawn.Add(playerSideDeck.RemoveTopCard());
                playerHand.AddCardToBottom(cardsDrawn[i]);
            }
        }
        else
        {
            for (int i = 0; i < p.GetStartingCardCount(); i++) //TODO:? Draw per turn for player is seperate
            {
                cardsDrawn.Add(playerSideDeck.RemoveTopCard());
                playerHand.AddCardToBottom(cardsDrawn[i]);
            }
        }
        
        Main.instance.DrawCardsToPlayerHand(cardsDrawn);
    }

    void EnemyDrawTurnStart(Player p)
    {
        if(turn >=1)
        {
            List<Card> cardsDrawn = new List<Card>();
            foreach(Enemy e in enemies)
            {
                if(e.GetDeck().DeckCount() < 3)
                {
                    for (int i = 0; i < e.GetDeck().DeckCount(); i++) //TODO:? Draw per turn for player is seperate
                    {
                        cardsDrawn.Add(e.GetDeck().RemoveTopCard());
                        playerHand.AddCardToBottom(cardsDrawn[i]);
                    }
                    for(int i = 0; i < e.GetDiscard().DeckCount(); i++)
                    {
                        e.GetDeck().AddCardToBottom(e.GetDiscard().RemoveTopCard());
                    }
                    e.GetDeck().Shuffle();
                    for(int i = 0; i < 3 - cardsDrawn.Count; i++)
                    {
                        cardsDrawn.Add(e.GetDeck().RemoveTopCard());
                        e.GetCurrentHand().AddCardToBottom(cardsDrawn[i]);
                    }
                }
                else
                {
                    for (int i = 0; i < 3; i++) //TODO: Config for enemies draw per turn
                    {
                        cardsDrawn.Add(e.GetDeck().RemoveTopCard());
                        e.GetCurrentHand().AddCardToBottom(cardsDrawn[i]);
                    }
                }
            } 
        }
    }

    void ProcessEndOfTurnEffects()
    {

    }

    public Enemy GetEnemyFromController(EnemyUIController uIController)
    {
        foreach(Enemy e in enemies)
        {
            UnityEngine.Debug.Log(e.GetName() + " " + uIController);
            if(e.GetUIController() == uIController)
            {
                return e;
            }
        }
        UnityEngine.Debug.LogError("Could not find UIController for Enemy");
        return null;
    }

    public List<Enemy> GetActiveEnemies()
    {
        List<Enemy> enemyList = new List<Enemy>();

        foreach(Enemy e in enemies)
        {
            if(e.IsDead() == false)
            {
                enemyList.Add(e);
            }
        }

        return enemyList;
    }

    void HandleDeadEntity(Entity e)
    {
        if(e.GetType() == typeof(Player))
        {
            //TODO: Game Over State
        }
        else
        {
            enemies.Remove((Enemy)e);
            CheckBattleCompleted();
        }
    }

    public void CheckBattleCompleted()
    {
        UnityEngine.Debug.Log("Enemies Left" + enemies.Count);
        if(enemies.Count == 0)
        {
            UnityEngine.Debug.Log("Battle End");//TODO: Battle Victory
        }
    }
}
