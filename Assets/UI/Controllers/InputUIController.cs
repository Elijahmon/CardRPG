using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputUIController : UIController {

    [SerializeField]
    GameObject titleScreenInputContainer;
    [SerializeField]
    GameObject combatInputContainer;

    [SerializeField]
    Button endTurnButton;
    [SerializeField]
    Button startGameButon;

	// Use this for initialization
	public override void Init(Camera cam)
    {
        myCanvas = GetComponent<Canvas>();
        myCanvas.worldCamera = cam;
        combatInputContainer.SetActive(false);
        titleScreenInputContainer.gameObject.SetActive(true);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ActivateCombatInputUI()
    {
       combatInputContainer.SetActive(true);
       endTurnButton.gameObject.SetActive(true);
    }

    public void PressEndTurnButton()
    {
        CombatStateMachine c = Main.instance.GetCombatStateMachine();
        if (c.IsPlayerTurn() == true)
        {
            Main.instance.PlayerEndTurn();
        }
    }

    public void OnPressStartGameButton()
    {
        titleScreenInputContainer.SetActive(false);
        Main.instance.StartGame();
    }
}
