using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExaminedCardController : MonoBehaviour {

    [SerializeField]
    TextMeshProUGUI cardName;
    [SerializeField]
    Image bg;

    public void ActivateExaminedCard(HandCard cardToExamine)
    {
        cardName.text = cardToExamine.cardName.text;
        gameObject.SetActive(true);
    }

    public void Deativate()
    {
        gameObject.SetActive(false);
    }
}
