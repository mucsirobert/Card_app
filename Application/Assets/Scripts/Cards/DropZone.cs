using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class DropZone : DropPlace, IPointerEnterHandler, IPointerExitHandler {

    public DroppableUnityEvent itemAboveBeforeDragEvent;
    public DroppableUnityEvent itemAboveBeginDragEvent;
    public DroppableUnityEvent itemAboveDragEvent;
    public DroppableUnityEvent itemAboveEndDragEvent;

    public DroppableUnityEvent droppableEnterEvent;
    public DroppableUnityEvent droppableExitEvent;


    // Use this for initialization
    void Start () {

	}

    public void OnItemBeforeDrag(Droppable droppable)
    {
        itemAboveBeforeDragEvent.Invoke(droppable);
    }

    public void OnItemBeginDrag(Droppable droppable)
    {
        itemAboveBeginDragEvent.Invoke(droppable);
    }
    public void OnItemDrag(Droppable droppable)
    {
        itemAboveDragEvent.Invoke(droppable);
    }


    public void OnItemEndDrag(Droppable droppable)
    {
        itemAboveEndDragEvent.Invoke(droppable);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;

        var droppable = eventData.pointerDrag.GetComponent<Droppable>();

        if (droppable.IsDragging)
        {
            droppableEnterEvent.Invoke(droppable);

            if (droppable != null)
            {
                droppable.OnDropPlaceEnter(this);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;

        var droppable = eventData.pointerDrag.GetComponent<Droppable>();

        OnDroppableAboveExit(droppable);
    }

    public void OnDroppableAboveExit(Droppable droppable) {
        /*if (droppable.IsDragging)
        {*/
            droppableExitEvent.Invoke(droppable);

            if (droppable != null)
            {
                droppable.OnDropPlaceExit(this);
            }
        //}
    }

}
