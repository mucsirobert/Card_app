using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[System.Serializable]
public class Vector3Event : UnityEvent<Vector3> { }

public class LongClickable : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool pointerDown;
    private float pointerDownTimer;

    [SerializeField]
    private float requiredHoldTime = 0.5f;

    [SerializeField]
    private float mouseMoveThreshold = 0.8f;

    public Vector3Event onClick;
    public Vector3Event onLongClick;

    private bool longClicked;
    private bool mouseMoved;
    private Vector3 clickPos;

    private PointerEventData currentEventData;

    public void OnPointerDown(PointerEventData eventData)
    {
        pointerDown = true;
        currentEventData = eventData;
        clickPos = Input.mousePosition;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if ((clickPos - Input.mousePosition).magnitude < mouseMoveThreshold)
        {

            if (!longClicked)
            {
                if (onClick != null) onClick.Invoke(Input.mousePosition);
            }
        }
        currentEventData = null;
        longClicked = false;
        Reset();
    }

    private void Update()
    {
        if (pointerDown)
        {
            pointerDownTimer += Time.deltaTime;
            if (pointerDownTimer >= requiredHoldTime)
            {
                if ((clickPos - Input.mousePosition).magnitude < mouseMoveThreshold) {
                    longClicked = true;
                    currentEventData.pointerDrag = null;

                    if (onLongClick != null)
                        onLongClick.Invoke(Input.mousePosition);

                }
                Reset();
            }
        }
    }

    private void Reset()
    {
        pointerDown = false;
        pointerDownTimer = 0;
    }

}