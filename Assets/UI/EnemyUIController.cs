using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyUIController : UIController {

    [SerializeField]
    EnemyCanvasUIController enemyCanvas;

    [SerializeField]
    public Image sprite;
    [SerializeField]
    public TextMeshProUGUI nameLabel;
    [SerializeField]
    public Slider healthBar;
    [SerializeField]
    public TextMeshProUGUI healthLabel;
    [SerializeField]
    public Slider manaBar;
    [SerializeField]
    public TextMeshProUGUI manaLabel;
    [SerializeField]
    public TextMeshProUGUI actionLabel;
    [SerializeField]
    public GridLayoutGroup activeEffectsContainer;
    [SerializeField]
    public ActiveEffectUIController activeEffectPrefab;

    List<ActiveEffectUIController> activeEffects;

    public Enemy myEnemy;
    public void Activate(Enemy e)
    {
        myEnemy = e;

        e.SetUIController(this);
        activeEffects = new List<ActiveEffectUIController>();

        EnemyConfig config = ConfigHandler.enemyConfigs[e.GetEnemyType()];
        //Set sprite
        nameLabel.text = e.GetName();

        UpdateHealth(e.GetCurrentHealth(), e.GetMaxHealth());

        manaBar.maxValue = e.GetMaxMana();
        manaBar.value = e.GetCurrentMana();
        manaLabel.text = manaBar.value + "/" + manaBar.maxValue;

        actionLabel.text = e.GetAction().ToString();

        InitActiveEffects();

        gameObject.SetActive(true);
    }

    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        Debug.Log("Updating Health");
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
        healthLabel.text = healthBar.value + "/" + healthBar.maxValue;
    }

    public void  UpdateActiveEffects(Dictionary<Enums.AppliedEffect, int> effects)
    {
        int index = 0;
        foreach (var effect in effects)
        {
            activeEffects[index].Activate(effect);
            index++;
        }

        for(;index < activeEffects.Count; index++)
        {
            activeEffects[index].Deactivate();
        }
    }

    void InitActiveEffects()
    {
        for(int i = 0; i < 20; i++)
        {
            GameObject g = Instantiate<GameObject>(activeEffectPrefab.gameObject, activeEffectsContainer.transform);
            activeEffects.Add(g.GetComponent<ActiveEffectUIController>());
        }
    }
    
    public override void Deactivate()
    {
        enemyCanvas.UpdateTargetedEnemy(null);
        base.Deactivate();
    }

}