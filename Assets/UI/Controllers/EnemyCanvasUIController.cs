using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCanvasUIController : UIController {

    [SerializeField]
    EnemyUIController leftEnemy;
    [SerializeField]
    EnemyUIController rightEnemy;
    [SerializeField]
    EnemyUIController middleEnemy;

    Enemy targetedEnemy;
    public override void Init(Camera cam)
    {
        myCanvas = GetComponent<Canvas>();
        myCanvas.worldCamera = cam;
    }

    public void Activate(List<Enemy> enemies)
    {
        //Debug.Log(enemies.Count);
        switch(enemies.Count)
        {
            case 1:
                middleEnemy.Activate(enemies[0]);
                break;
            case 2:
                leftEnemy.Activate(enemies[0]);
                rightEnemy.Activate(enemies[1]);
                break;
            case 3:
                leftEnemy.Activate(enemies[0]);
                rightEnemy.Activate(enemies[1]);
                middleEnemy.Activate(enemies[2]);
                break;
        }
        gameObject.SetActive(true);
    }

    public void UpdateTargetedEnemy(EnemyUIController enemy)
    {
        if(enemy != null)
        {
            targetedEnemy = enemy.myEnemy;
        }
        else
        {
            targetedEnemy = null;
        }
    }

    public Enemy GetTargetedEnemy()
    {
        return targetedEnemy;
    }


}
