using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class TargetingUIHitbox : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	[SerializeField]
	EnemyCanvasUIController enemyCanvas;

	[SerializeField]
	EnemyUIController myController;
	public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("Stopped hovering enemey " + this.gameObject.name);
        enemyCanvas.UpdateTargetedEnemy(null);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("Started hovering enemy " + this.gameObject.name);
        enemyCanvas.UpdateTargetedEnemy(myController);
    }
}
