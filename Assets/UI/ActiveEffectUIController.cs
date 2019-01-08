using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActiveEffectUIController : UIController {

    [SerializeField]
    Image sprite;
    [SerializeField]
    TextMeshProUGUI label;

    public void Activate(KeyValuePair<Enums.AppliedEffect, int> effect)
    {
        //Get sprite from config
        label.text = effect.Value.ToString();
        gameObject.SetActive(true);
    }
}
