using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class DragActivateButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    [SerializeField]
    [Tooltip("The amount of time the dragged object has to wait above the button before it activates.")]
    private float hoverTime;

    private Button button;
    private bool hovered = false;

    private float hoverStart;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.dragging)
        {
            hovered = true;
            hoverStart = Time.time;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovered = false;
    }

    private void Update()
    {
        if (hovered && (Time.time - hoverStart >= hoverTime))
        {
            hovered = false;
            //Fire the button onclick
            var pointer = new PointerEventData(EventSystem.current);
            button.OnSubmit(pointer);
            //ExecuteEvents.Execute(button.gameObject, pointer, ExecuteEvents.submitHandler);
        }
    }
}
