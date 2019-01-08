using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using TMPro;

public class TimelineScriptController : MonoBehaviour {

    [SerializeField]
    TimelineUIController uiController;
    [SerializeField]
    TextMeshProUGUI dialogueBox;

    [SerializeField]
    PlayableDirector director;

    TimelineAsset currentEncounter;

    EncounterConfig currentConfig;
    int currentSequenceIndex;

    bool waitingForInput = false;

    Enums.ScriptActionType currentScriptedAction;

    public void InitEncounter(Enums.EncounterID id)
    {
        //Debug.Log(id.ToString());
        currentConfig = ConfigHandler.encounterConfigs[id];
        currentSequenceIndex = 0;
        Main.instance.SetScriptedUIState(currentConfig.hideHandUI, currentConfig.hideStatsUI);
        if (currentConfig.type == Enums.EncounterType.TimelineSequence)
        {
            PlayEncounter(currentConfig.sequence[0].timelineName);
        }
    }

	public void PerformScriptedAction(Enums.ScriptActionType action,  params object[] data)
    {
        switch(action)
        {
            case Enums.ScriptActionType.PlayTimeline:
                int sequenceIndex = (int)data[0];
                PlayEncounter(currentConfig.sequence[sequenceIndex].timelineName);
                break;
            case Enums.ScriptActionType.PauseTimeline:
                director.Pause();
                break;
            case Enums.ScriptActionType.ReadDialogue:
                StartCoroutine(ReadDialogue((string)data[1], (float)data[2], (bool)data[3]));
                break;
            case Enums.ScriptActionType.ReadDialogueWaitForInput:
                StartCoroutine(ReadDialogue((string)data[1], (float)data[2], (bool)data[3]));
                waitingForInput = true;
                break;
            case Enums.ScriptActionType.WaitForChoice:
                director.Pause();
                break;
            case Enums.ScriptActionType.DrawEnemyDeck:
                Main.instance.GetCombatStateMachine().DrawEnemyDeck();
                break;
            case Enums.ScriptActionType.TransitionToEncounter:
                Main.instance.StartEncounter((Enums.EncounterID)data[4]);
                break;
            case Enums.ScriptActionType.EndCombat:
                EndEncounter();
                break;


        }
        currentScriptedAction = action;
        if((bool)data[5] == true)
        {
            Main.instance.ScriptedPlayerTurn();
        }
    }

    void PlayEncounter(string assetName)
    {
        uiController.Activate();
        currentEncounter = Resources.Load<TimelineAsset>("Timelines/" + assetName);
        director.playableAsset = currentEncounter;
        Object binding = null;
        foreach(var bind in currentEncounter.outputs)
        {
            if(bind.sourceObject.GetType() == typeof(TimelineScriptTrack))
            {
                binding = bind.sourceObject;
                break;
            }
        }
        director.SetGenericBinding(binding, this);
        director.Play();
    }

    void EndEncounter()
    {
        Main.instance.StopTimelineCombat();
    }

    IEnumerator ReadDialogue(string dialogue, float delay, bool clear)
    {
        if (clear)
        {
            dialogueBox.text = "";
        }
        for(int currentLetter = 0; currentLetter < dialogue.Length; currentLetter++)
        {
            dialogueBox.text += dialogue[currentLetter];
            yield return new WaitForSeconds(delay);
        }
        yield return new WaitForEndOfFrame();
    }

    void Update()
    {
    }

    public void ProcessInputFromCurrentState()
    {
        if(currentScriptedAction == Enums.ScriptActionType.ReadDialogueWaitForInput)
        {
            Debug.Log("Recieved Input While Waiting");
        }
        if(currentScriptedAction == Enums.ScriptActionType.WaitForChoice)
        {
            director.Resume();
        }
    }

    public Enums.ScriptActionType GetCurrentAction()
    {
        return currentScriptedAction;
    }
}
