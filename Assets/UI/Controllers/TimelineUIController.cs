using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using TMPro;

public class TimelineUIController : UIController{

    [SerializeField]
    PlayableDirector director;

    TimelineAsset currentEncounter;

    Canvas bgCanvas;

    public override void Init(Camera cam)
    {
        myCanvas = GetComponent<Canvas>();
        myCanvas.worldCamera = cam;

        bgCanvas = GetComponentInChildren<Canvas>();
        bgCanvas.worldCamera = cam;
        base.Init();
    }
}
