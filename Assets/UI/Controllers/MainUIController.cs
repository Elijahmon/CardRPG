using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUIController : UIController {

    public static MainUIController instance;

    [SerializeField]
    Camera uiCamera;

    StaticCanvasUIController staticCanvas;
    CombatStatsUIController combatCanvas;
    HandCanvasUIController handCanvas;
    InputUIController inputCanvas;
    TimelineUIController timelineCanvas;
    EnemyCanvasUIController enemyCanvas;

    public override void Init()
    {
        instance = this;

        GameObject g = Instantiate<GameObject>((GameObject)Resources.Load("UI/StaticCanvas"), transform);
        staticCanvas = g.GetComponent<StaticCanvasUIController>();
        staticCanvas.Init(uiCamera);
        staticCanvas.Activate();

        g = Instantiate<GameObject>((GameObject)Resources.Load("UI/CombatStatsCanvas"), transform);
        combatCanvas = g.GetComponent<CombatStatsUIController>();
        combatCanvas.Init(uiCamera);
        combatCanvas.Deactivate();

        g = Instantiate<GameObject>((GameObject)Resources.Load("UI/HandCanvas"), transform);
        handCanvas = g.GetComponent<HandCanvasUIController>();
        handCanvas.Init(uiCamera);
        handCanvas.Deactivate();

        g = Instantiate<GameObject>((GameObject)Resources.Load("UI/InputCanvas"), transform);
        inputCanvas = g.GetComponent<InputUIController>();
        inputCanvas.Init(uiCamera);
        inputCanvas.Activate();


        g = Instantiate<GameObject>((GameObject)Resources.Load("UI/TimelineCanvas"), transform);
        timelineCanvas = g.GetComponent<TimelineUIController>();
        timelineCanvas.Init(uiCamera);
        timelineCanvas.Activate();

        g = Instantiate<GameObject>((GameObject)Resources.Load("UI/EnemyCanvas"), transform);
        enemyCanvas = g.GetComponent<EnemyCanvasUIController>();
        enemyCanvas.Init(uiCamera);
        enemyCanvas.Deactivate();

    }

    public void LoadCombatUI(Player p)
    {
        UpdateStatsUI(p.GetHealth(), p.GetMana(), p.GetAction());
        combatCanvas.Activate();
        handCanvas.Activate();
        inputCanvas.Activate();
        inputCanvas.ActivateCombatInputUI();
    }

    public void ClearCombatUI()
    {
        combatCanvas.Deactivate();
        handCanvas.Deactivate();
        inputCanvas.Deactivate();
    }

    public void DrawCardsToHand(List<Card> cardsToDraw)
    {
        handCanvas.DrawCards(cardsToDraw);
    }

    public void DiscardCardsFromHand(List<Card> cardsToDiscard)
    {
        handCanvas.DiscardCards(cardsToDiscard);
    }

    public void ClearHands()
    {
        handCanvas.ClearPlayerHand();
    }

    public void UpdateStatsUI(Health h, Mana m, Action a)
    {
        combatCanvas.UpdateStats(h, m, a);
    }

    public HandCanvasUIController GetHandUIController()
    {
        return handCanvas;
    }

    public CombatStatsUIController GetStatsUIController()
    {
        return combatCanvas;
    }

    public void DeactivateTitleScreen()
    {
        staticCanvas.ToggleTitleScreen(false);
    }

    public TimelineUIController GetTimelineController()
    {
        return timelineCanvas;
    }

    public EnemyCanvasUIController GetEnemyCanvasUIController()
    {
        return enemyCanvas;
    }

    public void ActivateEnemyCanvas(List<Enemy> enemies)
    {
        enemyCanvas.Activate(enemies);
    }

    public void ToggleUIController(UIController uiElement, bool toggle)
    {
        if (toggle)
        {
            uiElement.Activate();
        }
        else
        {
            uiElement.Deactivate();
        }
    }
}
