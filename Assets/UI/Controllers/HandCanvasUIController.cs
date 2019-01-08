using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCanvasUIController : UIController {

    [SerializeField]
    public HandContainer hand;
    [SerializeField]


    float drawCardDelay = 1;

    public override void Init(Camera cam)
    {
        myCanvas = GetComponent<Canvas>();
        myCanvas.worldCamera = cam;
        base.Init();
    }

    public void DrawCards(List<Card> cardsToDraw)
    {
        for(int i = 0; i < cardsToDraw.Count; i++)
        {
            StartCoroutine(DrawCardWithDelay(cardsToDraw[i], drawCardDelay * i));
        }
    }

    IEnumerator DrawCardWithDelay(Card c, float delay)
    {
        yield return new WaitForSeconds(delay);
        hand.AddCardToHand(c);
    }

    public RectTransform GetHandContainerRect()
    {
        return hand.GetRect();
    }

    public void DiscardCards(List<Card> cards)
    {
        foreach(Card c in cards)
        {
            hand.RemoveCardFromHand(c);
        }
    }

    public void ClearPlayerHand()
    {
        hand.Clear();
    }
}
