using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class Entity : NetworkBehaviour
{

    [SyncVar]
    public GameObject parent;

    //TODO use ethis everywhere!!!
    [SyncVar]
    public Vector3 localPosition;

    protected virtual void Awake() { }

    public override void OnStartClient()
    {
        if (parent != null)
            this.transform.SetParent(parent.transform, false);

        //TODO delete if
        if (!localPosition.Equals(Vector3.zero))
            this.transform.localPosition = localPosition;
    }

    protected virtual void Start()
    {
        /*if (parent == null)
            this.transform.SetParent(null);
        else*/

       
    }

    private void Update()
    {
        /*if (isPointerDown && !longPressTriggered)
        {
            if (Time.time - timePressStarted > longTouchTime)
            {
                longPressTriggered = true;
                OnLongTouch();
            }
        }*/
    }


    public virtual void OnTouch(Vector3 touchPosition)
    {
    }

    public virtual void OnLongTouch(Vector3 touchPosition)
    {
    }
}
