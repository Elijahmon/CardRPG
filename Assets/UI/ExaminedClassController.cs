using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ExaminedClassController : MonoBehaviour {

    [SerializeField]
    Image bg;
    [SerializeField]
    TextMeshProUGUI className;
    [SerializeField]
    TextMeshProUGUI description;
    [SerializeField]
    TextMeshProUGUI effects;

    public void ActivateExamined(HandCard cardToExamine)
    {
        Card c = cardToExamine.getCard();
        className.text = c.GetDisplayName();
        description.text = c.GetDescription();
        gameObject.SetActive(true);
    }

    public void Deativate()
    {
        gameObject.SetActive(false);
    }
}
