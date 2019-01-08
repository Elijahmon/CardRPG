using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TargetingLineController : UIController {

    [SerializeField]
    LineRenderer lineRenderer;
    [SerializeField]
    Image arrowCap;

    Vector2 targetPos;

    int lengthDivisor = 40;
    float capOffset = 30;

    public void Init(float yPos)
    {
        Deactivate();
    }

    public void UpdateTargetPosition(Vector2 pos)
    {
        /*      TODO do line across points
        float length = Vector2.Distance(Vector2.zero, pos);
        int points = GetPointsFromLength(length);
        lineRenderer.positionCount = points;
        */

        lineRenderer.SetPosition(lineRenderer.positionCount - 1, pos);

        float angle = Mathf.Atan2(pos.y - Vector2.zero.y, pos.x - Vector2.zero.x) * Mathf.Rad2Deg - 90;
        //Debug.Log(angle);

        arrowCap.rectTransform.anchoredPosition = pos;
        arrowCap.transform.eulerAngles = new Vector3(0, 0, angle);
    }

    void Update()
    {

    }

    int GetPointsFromLength(float length)
    {
        return Mathf.RoundToInt(length / lengthDivisor);
    }
}
