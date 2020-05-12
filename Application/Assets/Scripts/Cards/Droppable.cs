using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[System.Serializable]
public class DropPlaceUnityEvent : UnityEvent<DropZone> { }

public class Droppable : Draggable {

    public UnityEvent onDropFailed;
    public DropPlaceUnityEvent dropPlaceEnterEvent;
    public DropPlaceUnityEvent dropPlaceExitEvent;

    public DropZone DropZoneBelow { get; set; }

    public string ignoreRaycastLayerName = "Ignore Raycast";

    private int defaultLayer;
    public bool AboveDropZone { get; set; }

    private bool droppedOnZone;

    

	// Use this for initialization
	void Start () {
        defaultLayer = gameObject.layer;
        if (DropZoneBelow != null) AboveDropZone = true;
	}

    protected override void OnBeforeDrag(PointerEventData eventData)
    {
        base.OnBeforeDrag(eventData);
        if (IsDraggable)
        {

            IsDraggable = false;
            if (DropZoneBelow != null)
            {
                DropZoneBelow.OnItemBeforeDrag(this);
            }
        }
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        if (IsDraggable)
        {
            droppedOnZone = false;
            if (DropZoneBelow != null)
            {
                DropZoneBelow.OnItemBeginDrag(this);
            }

            gameObject.layer = LayerMask.NameToLayer(ignoreRaycastLayerName);

            //This is needed, because otherwise after setting the parent, the zone's OnPointerExit will not be called
            eventData.pointerEnter = transform.parent.gameObject;

            transform.parent = Camera.main.transform;

            //This is needed, because sometimes OnDropPlaceExit called before OnbeginDrag
            if (!AboveDropZone) DropZoneBelow.OnDroppableAboveExit(this);

            //dropPlaceBelow = transform.parent.GetComponent<DropPlace>();
        }

    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);

        if (DropZoneBelow != null)
        {
            DropZoneBelow.OnItemDrag(this);
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);

        gameObject.layer = defaultLayer;

        if (DropZoneBelow != null)
        {
            DropZoneBelow.OnItemEndDrag(this);
        }

        if (!droppedOnZone)
        {
            onDropFailed.Invoke();
        }
    }

    public void OnDropPlaceEnter(DropZone dropZone)
    {

        DropZoneBelow = dropZone;

        AboveDropZone = true;

        dropPlaceEnterEvent.Invoke(dropZone);
    }

    public void OnDropPlaceExit(DropZone dropZone)
    {
        AboveDropZone = false;
        if (IsDragging)
        {
            DropZoneBelow = null;

            dropPlaceExitEvent.Invoke(dropZone);
        }
    }

    public void OnDrop(DropPlace dropPlace)
    {
        droppedOnZone = true;
        //dropPlaceBelow = dropPlace;

        //transform.parent = dropPlace.transform;
    }

    public void DroppedOn(DropZone dropZone)
    {
        droppedOnZone = true;
        DropZoneBelow = dropZone;

        AboveDropZone = true;
    }
}
