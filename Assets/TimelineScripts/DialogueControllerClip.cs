using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using TMPro;


[Serializable]
public class DialogueControllerClip : PlayableAsset, ITimelineClipAsset
{

    [SerializeField]
    DialogueControllerBehaviour template = new DialogueControllerBehaviour();

    public ClipCaps clipCaps
    {
        get
        {
            return ClipCaps.Extrapolation;
        }
    }

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        return ScriptPlayable<DialogueControllerBehaviour>.Create(graph, template);
    }
}


