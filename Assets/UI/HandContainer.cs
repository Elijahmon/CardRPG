using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandContainer : MonoBehaviour {

    RectTransform myRect;

    List<HandCard> activeCards;
    List<HandCard> inactiveCards;

    HandCard selectedCard;
    float selectedBump = 20;

    int absMaxTilt = 30;
    int maxGap;
    float cardWidth;
    

    [SerializeField]
    GameObject inactiveCardHolder;
    [SerializeField]
    GameObject handCardObj;
    [SerializeField]
    ExaminedCardController examineCard;
    [SerializeField]
    ExaminedClassController examineClass;
    [SerializeField]
    TargetingLineController targetingLine;

    int initialCardPool = 15;

    float selectedExamineTimer = 2;

	void Start()
    {
        myRect = GetComponent<RectTransform>();

        activeCards = new List<HandCard>();
        inactiveCards = new List<HandCard>();
        cardWidth = handCardObj.GetComponent<RectTransform>().rect.width;
        maxGap = (int)cardWidth / 4;

        for(int i = initialCardPool; i >= 0; i--)
        {
            CreateInactiveCard();
        }

        targetingLine.Init(myRect.sizeDelta.y);
    }

    void Update()
    {

    }

    /// <summary>
    /// Re-Orders cards based on how many are in the active list
    /// </summary>
    public void UpdateCardPositions()
    {
        SortCards();
    }

    public void AddCardToHand(Card c)
    {
        ActivateCard(c);
    }

    public void RemoveCardFromHand(Card c)
    {
        foreach(var card in activeCards) //TODO: Fix this to remove exactly the card played
        {
            if(card.getCard().GetCard() == c.GetCard())
            {
                RemoveCardFromHand(card);
                break;
            }
        }
        
    }

    public void Clear()
    {
        for(int i = activeCards.Count -1; i >= 0 ; i--)
        {
            //Debug.Log("removing card " + activeCards[i].cardName.text + " - " + activeCards.Count);
            RemoveCardFromHand(activeCards[i]);
        }
    }

    public void RemoveCardFromHand(HandCard c)
    {
        Deselect(c);
        DeactivateCard(c);
    }

    private void SortCards()
    {
        int gap = Mathf.Max((int)cardWidth - (activeCards.Count * 5), maxGap);
        int j = 0;
        List<KeyValuePair<Vector3, float>> positions = new List<KeyValuePair<Vector3, float>>();

        for (int i = 0; i < activeCards.Count; i++)
        {
            int mygap = gap * j;
            float myRot = (absMaxTilt / Mathf.Min(activeCards.Count, absMaxTilt)) * j;
            if (i % 2 == 0)
            {
                mygap *= -1;
                myRot *= -1;
                j++;
            }
            positions.Add(new KeyValuePair<Vector3, float>(new Vector3(mygap, -Mathf.Abs(myRot), -mygap), -myRot));

        }
        positions.Sort((a, b) => (b.Key.z.CompareTo(a.Key.z)));
        for(int i = 0;i < activeCards.Count; i++)
        {
            if(activeCards[i] == selectedCard)
            {
                activeCards[i].SetTargetPosition(new Vector3(positions[i].Key.x, positions[i].Key.y + selectedBump, positions[i].Key.z));
                activeCards[i].SetTargetRotation(positions[i].Value);
            }
            else
            {
                activeCards[i].SetTargetPosition(positions[i].Key);
                activeCards[i].SetTargetRotation(positions[i].Value);
            }
            
        }
    }

    private void CreateInactiveCard()
    {
        HandCard newInactiveCard = GameObject.Instantiate<GameObject>(handCardObj, inactiveCardHolder.transform).GetComponent<HandCard>();
        inactiveCards.Add(newInactiveCard);
        newInactiveCard.gameObject.SetActive(false);
    }

    private void DeactivateCard(HandCard card)
    {
        inactiveCards.Add(card);
        activeCards.Remove(card);
        card.gameObject.SetActive(false);
        card.transform.SetParent(inactiveCardHolder.transform);
        UpdateCardPositions();
    }

    private void ActivateCard(Card c)
    {
        HandCard cardToActivate;
        if (inactiveCards.Count > 0)
        {
            cardToActivate = inactiveCards[inactiveCards.Count - 1];
        }
        else
        {
            CreateInactiveCard();
            cardToActivate = inactiveCards[inactiveCards.Count - 1];
        }
        cardToActivate.transform.SetParent(transform);
        cardToActivate.transform.localPosition = new Vector3(myRect.rect.width, 0, 0);
        inactiveCards.Remove(cardToActivate);
        activeCards.Add(cardToActivate);

        cardToActivate.Init(c);
        cardToActivate.gameObject.SetActive(true);
        SortCards();
    }

    public RectTransform GetRect()
    {
        return myRect;
    }

    public void SetSelected(HandCard card)
    {
        selectedCard = card;
        UpdateCardPositions();
        StartCoroutine(ExamineSelectedCard(card));
        
    }

    public void Deselect(HandCard card)
    {
        if (selectedCard == card)
        {
            selectedCard = null;
            UpdateCardPositions();
        }
        examineCard.Deativate();
        examineClass.Deativate();
        
    }

    IEnumerator ExamineSelectedCard(HandCard c)
    {
        yield return new WaitForSeconds(selectedExamineTimer);
        if(selectedCard == c && selectedCard.dragged == false)
        {
            if (selectedCard.getCard().GetCardType() == Enums.CardType.ClassSelect)
            {
                examineClass.ActivateExamined(selectedCard);
            }
            else
            {
                examineCard.ActivateExaminedCard(selectedCard);
            }
        }
        yield return new WaitForEndOfFrame();
    }

    public void UpdateTargetingArrowPos(Vector2 pos)
    {
        targetingLine.Activate();
        Canvas handCanvas = MainUIController.instance.GetHandUIController().GetCanvas();
        Vector2 mousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(handCanvas.transform as RectTransform, pos, handCanvas.worldCamera, out mousePos);
        targetingLine.UpdateTargetPosition(mousePos);
    }

    public void DeactivateTargetingArrow()
    {
        targetingLine.Deactivate();
    }

}
