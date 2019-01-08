using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using TMPro;
using System.Collections;

[Serializable]
public class DialogueControllerBehaviour : PlayableBehaviour {

    [SerializeField]
    string dialogueText;
    [SerializeField]
    float delay;

    int currentCharacter;
    float timer;

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        currentCharacter = 0;
    }

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        TextMeshProUGUI text = playerData as TextMeshProUGUI;

        if (text == null)
        {
            Debug.LogError("Dialogue Playable TextMesh Cast Error");
            return;
        }
        if(currentCharacter == 0)
        {
            text.text = "";
        
        }
        if (timer < delay)
        {
            timer += Time.deltaTime;
        }
        else
        {
            if (currentCharacter < dialogueText.Length)
            {
                text.text += dialogueText[currentCharacter];
                currentCharacter++;
            }
            timer = 0;
        }

    }
}
