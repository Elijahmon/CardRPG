using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

    public static Main instance;

    [SerializeField]
    MainUIController mainUIController;

    Player player;
    CombatStateMachine combatStateMachine;

    TimelineScriptController timelineScriptController;

	// Use this for initialization
	void Start ()
    {
        Init();
	}
	
	// Update is called once per frame
	void Update ()
    {
        Tick();
	}

    private void Init()
    {
        string configPath = Application.dataPath + "/Resources/Configs";

        ConfigHandler.LoadConfigs(configPath);
        instance = this;
        mainUIController.Init();
        player = LoadOrCreatePlayer();
        combatStateMachine = new CombatStateMachine();
        combatStateMachine.Init();
        timelineScriptController = MainUIController.instance.GetTimelineController().GetComponent<TimelineScriptController>();
    }

    private void Tick()
    {
        combatStateMachine.Tick();
        mainUIController.Tick();
    }

    private Player LoadOrCreatePlayer()
    {
        player = new Player();
        if (false) // if a save exists
        {

        }
        else //Create a new player
        {
            player.Init();
        }

        return player;
    }

    public void StartGame()
    {
        mainUIController.DeactivateTitleScreen();
        //TODO: Fix this to use a controller or something
        StartEncounter(Enums.EncounterID.StartSequence);
        
        //Main.instance.StartCombat(0);
    }

    public void StartEncounter(Enums.EncounterID encounter)
    {
        //Debug.Log(encounter.ToString());
        EncounterConfig config = ConfigHandler.encounterConfigs[encounter];
        switch(config.type)
        {
            case Enums.EncounterType.TimelineSequence:
                StartTimelineCombat(config);
                timelineScriptController.InitEncounter(config.id);
                break;
            case Enums.EncounterType.Combat:
                StartCombat(config);
                break;
        }
    }

    void StartTimelineCombat(EncounterConfig config)
    {
        combatStateMachine.StartTimelineStateMachine(config, player);
    }

    void StartCombat(EncounterConfig config)
    {
        mainUIController.LoadCombatUI(player);
        combatStateMachine.StartCombatStateMachine(config, player);
    }

    public void StopTimelineCombat()
    {
        mainUIController.ClearCombatUI();
        combatStateMachine.StopTimelineStateMachine(player);
    }

    public void ShufflePlayerCurrentCombatDeck(Player p)
    {
        p.GetCurrentCombatDeck().Shuffle();
    }

    public void DrawCardsToPlayerHand(List<Card> cardsDrawn)
    {
        mainUIController.DrawCardsToHand(cardsDrawn);
    }

    public void DiscardCardsFromPlayerHand(List<Card> cardsDiscarded)
    {
        mainUIController.DiscardCardsFromHand(cardsDiscarded);
    }

    public void ClearHands()
    {
        mainUIController.ClearHands();
    }

    public void UpdateUI(Health h, Mana m, Action a)
    {
        mainUIController.UpdateStatsUI(h, m, a);
    }

    public CombatStateMachine GetCombatStateMachine()
    {
        return combatStateMachine;
    }

    public void SetScriptedUIState(bool hideHand, bool hideStats)
    {
        mainUIController.ToggleUIController(mainUIController.GetHandUIController(), !hideHand);
        mainUIController.ToggleUIController(mainUIController.GetStatsUIController(), !hideStats);
    }

    public void EnemyPlayCard(Card c, Enemy source)
    {
        if(c.GetTargetType() == Enums.CardTargetType.Self)
        {
            combatStateMachine.PlayCardSingleTarget(source, source, c);
        }
        else
        {
            combatStateMachine.PlayCardSingleTarget(source, player, c);
        }
    }

    public void PlayerPlayCard(Card c, Entity target = null)
    {
        if(c.GetTargetType() == Enums.CardTargetType.Self)
        {
            combatStateMachine.PlayCardSingleTarget(player, player, c);
        }
        else if(c.GetTargetType() == Enums.CardTargetType.AllEnemies)
        {
            combatStateMachine.PlayCardMultiTarget(player, c);
        }
        else
        {
            combatStateMachine.PlayCardSingleTarget(player, target, c);
        }
    }
    
    public void ProcessCardChoiceInput()
    {
        if (timelineScriptController.GetCurrentAction() == Enums.ScriptActionType.WaitForChoice)
        {
            timelineScriptController.ProcessInputFromCurrentState();
        }
    }

    public void ScriptedPlayerTurn()
    {
        combatStateMachine.ScriptedActionActivatePlayerTurn();
    }

    public void AIEndTurn()
    {
        combatStateMachine.EnemyEndTurn(player);
    }

    public void PlayerEndTurn()
    {
        combatStateMachine.EndTurn(player);
    }
}
