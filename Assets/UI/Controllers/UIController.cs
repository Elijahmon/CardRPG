using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour {

    protected Canvas myCanvas;

    public virtual void Init()
    {

    }

    public virtual void Init(Camera cam)
    {

    }

    public virtual void Tick()
    {

    }

	public virtual void Activate()
    {
        gameObject.SetActive(true);
    }

    public virtual void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public virtual Canvas GetCanvas()
    {
        return myCanvas;
    }
}
