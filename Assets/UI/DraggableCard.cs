using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler {

    [SerializeField]
    HandCard handCard;
    [SerializeField]
    HandContainer container;

    Animator myAnimator; 

    HandCanvasUIController handCanvas;

    float playareaBuffer;

    bool inPlayable = false;

    Entity targetedEnemy;

    public void OnBeginDrag(PointerEventData eventData)
    {
        handCard.dragged = true;
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void OnDrag(PointerEventData eventData)
    {
        SetDraggedPosition(eventData);
        RectTransform cardRect = handCard.GetRect();
        if (cardRect.anchoredPosition.y > playareaBuffer)
        {
            inPlayable = true;
            if (handCard.getCard().isCombatCard() == true && handCard.getCard().GetTargetType() == Enums.CardTargetType.SingleEnemy)
            {
                Vector2 targetPos = eventData.position;
                handCard.visualContainer.SetActive(false);
                if(handCard.getCard().GetTargetType() == Enums.CardTargetType.SingleEnemy)
                {//TODO: Remove or replace
                }

                container.UpdateTargetingArrowPos(targetPos);
            }
        }
        else
        {
            inPlayable = false;
            if (handCard.getCard().isCombatCard() == true)
            {
                container.DeactivateTargetingArrow();
                handCard.visualContainer.SetActive(true);
            }
        }
        //Change into Arrow if in playable
        myAnimator.SetBool("isPlayable", inPlayable);
        //Scale with distance from bottom if not in playable
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        handCard.dragged = false;

        if (inPlayable == false)
        {
            myAnimator.SetBool("isPlayable", inPlayable);
        }
        else
        {
            if (Main.instance.GetCombatStateMachine().IsPlayerTurn() == true)
            {
                switch(handCard.getCard().GetTargetType())
                {
                    case Enums.CardTargetType.Self:
                        Main.instance.PlayerPlayCard(handCard.getCard());
                        container.RemoveCardFromHand(handCard);
                        Main.instance.ProcessCardChoiceInput();
                        container.DeactivateTargetingArrow();
                        break;
                    case Enums.CardTargetType.SingleEnemy:
                        targetedEnemy = MainUIController.instance.GetEnemyCanvasUIController().GetTargetedEnemy(); //TODO: Clean this up
                        if(targetedEnemy != null)
                        {
                            Main.instance.PlayerPlayCard(handCard.getCard(), targetedEnemy);
                            container.RemoveCardFromHand(handCard);
                            Main.instance.ProcessCardChoiceInput();
                            container.DeactivateTargetingArrow();
                        }
                        else
                        {
                            container.DeactivateTargetingArrow();
                            handCard.visualContainer.SetActive(true);
                            myAnimator.SetBool("isPlayable", false);
                        }
                        break;
                    case Enums.CardTargetType.AllEnemies:
                        Main.instance.PlayerPlayCard(handCard.getCard());
                        container.RemoveCardFromHand(handCard);
                        Main.instance.ProcessCardChoiceInput();
                        container.DeactivateTargetingArrow();
                        break;
                }
                
                
            }
        }
    }

    private void SetDraggedPosition(PointerEventData data)
    {
        Vector2 mousePos;
        
        RectTransformUtility.ScreenPointToLocalPointInRectangle(handCanvas.transform as RectTransform, data.position, handCanvas.GetCanvas().worldCamera, out mousePos);
        transform.position = handCanvas.transform.TransformPoint(mousePos);

    }

    void Start()
    {
        handCanvas = MainUIController.instance.GetHandUIController();
        myAnimator = GetComponent<Animator>();
        playareaBuffer = (handCard.GetRect().sizeDelta.y / 3) * 2;
    }

	// Update is called once per frame
	void Update () {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (handCard.dragged == false)
        {
            container.SetSelected(this.handCard);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        container.Deselect(this.handCard);
    }
}
