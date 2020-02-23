using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IInitializePotentialDragHandler {

    private Vector3 pointerOffset;

    public bool IsDraggable;

    public UnityEvent beforeDragEvent;
    public UnityEvent beginDragEvent;
    public UnityEvent dragEvent;
    public UnityEvent endDragEvent;


    public bool IsDragging { get; set; }

    // Use this for initialization
    void Start () {
		
	}

    protected virtual void OnBeforeDrag(PointerEventData eventData)
    {
        beforeDragEvent.Invoke();
    }
    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        OnBeforeDrag(eventData);
        if (IsDraggable)
        {
            pointerOffset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            pointerOffset.z = 0;
            IsDragging = true;
            beginDragEvent.Invoke();

        } else
        {
            eventData.pointerDrag = null;
        }
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        var screenMousePos = Input.mousePosition;
        screenMousePos.z = 10.0f;

        //Need to kill the exsisting animation, otherwise itt will try to move the object
        transform.DOKill();
        transform.position = Camera.main.ScreenToWorldPoint(screenMousePos) - pointerOffset;

        dragEvent.Invoke();
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        pointerOffset = Vector3.zero;

        IsDragging = false;

        endDragEvent.Invoke();
    }

    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
        eventData.useDragThreshold = false;
    }
}
