using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaticCanvasUIController : UIController {

    [SerializeField]
    GameObject titleScreenBackground;

    public override void Init(Camera cam)
    {
        myCanvas = GetComponent<Canvas>();
        myCanvas.worldCamera = cam;
        titleScreenBackground.SetActive(true);
    }

    public void ToggleTitleScreen(bool toggle)
    {
        titleScreenBackground.SetActive(toggle);
    }
}
