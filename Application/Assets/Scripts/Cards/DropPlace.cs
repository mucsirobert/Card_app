using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


[System.Serializable]
public class DroppableUnityEvent : UnityEvent<Droppable> { }

public class DropPlace : MonoBehaviour, IDropHandler
{
    public DroppableUnityEvent dropEvent;


    public virtual void OnDrop(PointerEventData eventData)
    {

        //eventData.pointerDrag.transform.parent = transform;

        var droppable = eventData.pointerDrag.GetComponent<Droppable>();


        if (droppable != null)
        {
            droppable.OnDrop(this);
        }
        dropEvent.Invoke(droppable);
    }

}
