using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HandCard : MonoBehaviour {

    [SerializeField]
    public TextMeshProUGUI cardName;
    [SerializeField]
    public GameObject visualContainer;

    public bool dragged;

    RectTransform myRect;
    
    Vector3 targetPos;
    Vector3 targetRot;

    float lerpRate = 0.05f;
    float selectedOffset = 10;

    Card card;

    void Start()
    {
        myRect = GetComponent<RectTransform>();
    }

    public void Init(Card c)
    {
        card = c;
        cardName.text = card.GetDisplayName();
        visualContainer.gameObject.SetActive(true);
    }

    public void SetTargetPosition(Vector3 t)
    {
        //Debug.Log("Setting Target " + t);
        targetPos = t;
    }

    public void SetTargetRotation(float z)
    {
        //Debug.Log("Setting target " + z);
        targetRot = new Vector3(0,0, z);
    }

    void FixedUpdate()
    {
        if (dragged == false)
        {
            if (myRect.anchoredPosition3D != targetPos)
            {
                myRect.anchoredPosition3D = Vector3.Lerp(myRect.anchoredPosition3D, targetPos, lerpRate);
                //Debug.Log(myRect.anchoredPosition3D);
            }
            if (myRect.eulerAngles != targetRot)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(targetRot), lerpRate);
            }
        }
    }

    public RectTransform GetRect()
    {
        return myRect;
    }

    public Vector3 GetTargetPosition()
    {
        return targetPos;
    }

    public Card getCard()
    {
        return card;
    }
}
