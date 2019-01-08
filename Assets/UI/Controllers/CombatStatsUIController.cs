using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatStatsUIController : UIController {

    [SerializeField]
    Slider healthbarSlider;
    [SerializeField]
    TextMeshProUGUI healthbarLabel;
    [SerializeField]
    Slider manabarSlider;
    [SerializeField]
    TextMeshProUGUI manabarLabel;
    [SerializeField]
    Image actionImage;
    [SerializeField]
    TextMeshProUGUI actionLabel;

    public override void Init(Camera cam)
    {
        myCanvas = GetComponent<Canvas>();
        myCanvas.worldCamera = cam;
        base.Init();
    }

    public void UpdateStats(Health h, Mana m, Action a)
    {
        healthbarSlider.maxValue = h.GetMaxHealth();
        healthbarSlider.value = h.GetCurrentHealth();
        healthbarLabel.text = healthbarSlider.value + "/" + healthbarSlider.maxValue;

        manabarSlider.maxValue = m.GetMaxMana();
        manabarSlider.value = m.GetCurrentMana();
        manabarLabel.text = manabarSlider.value + "/" + manabarSlider.maxValue;

        actionLabel.text = a.GetCurrentAction().ToString();

    }
    
    
}
