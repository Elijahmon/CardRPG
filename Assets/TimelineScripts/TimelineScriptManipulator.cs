using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Playables;

[Serializable]
public class TimelineScriptManipulator : PlayableBehaviour
{
    [SerializeField]
    Enums.ScriptActionType action;

    [SerializeField]
    int sequenceIndex;
    [SerializeField]
    string dialogue;
    [SerializeField]
    float dialogueDelay;
    [SerializeField]
    bool clearDialogue;
    [SerializeField]
    Enums.EncounterID transitionID;
    [SerializeField]
    bool acceptInput;

    bool done = false;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        TimelineScriptController controller = playerData as TimelineScriptController;

        if(controller == null)
        {
            return;
        }

        if(!done)
        {
            controller.PerformScriptedAction(action, sequenceIndex, dialogue, dialogueDelay, clearDialogue, transitionID, acceptInput);
            done = true;
        }
    }

}
