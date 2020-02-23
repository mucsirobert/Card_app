using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ReassignableDroppable : Droppable {

    private GameObject currentDraggedItem;
    public GameObject CurrentDraggedItem { 
        get
        {
            return currentDraggedItem;
        }
        set
        {
            if (currentDraggedItem != value)
            {
                currentDraggedItem = value;
                draggedItemChanged = true;
            }
        }
    }
    private bool draggedItemChanged = false;

	// Use this for initialization
	void Start () {
        currentDraggedItem = gameObject;
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);

        if (draggedItemChanged)
        {
            draggedItemChanged = false;
            eventData.pointerDrag = CurrentDraggedItem;
            CurrentDraggedItem.transform.position = transform.position;
            CurrentDraggedItem.GetComponent<Droppable>().OnBeginDrag(eventData);
            Destroy(gameObject);
        }
    }

}
