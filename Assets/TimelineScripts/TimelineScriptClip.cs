using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using System;

[Serializable]
public class TimelineScriptClip : PlayableAsset, ITimelineClipAsset
{

    [SerializeField]
    TimelineScriptManipulator template = new TimelineScriptManipulator();

    public ClipCaps clipCaps
    {
        get
        {
            return ClipCaps.Extrapolation;
        }
    }

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        return ScriptPlayable<TimelineScriptManipulator>.Create(graph, template);
    }
}