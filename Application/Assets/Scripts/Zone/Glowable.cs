using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Glowable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
{

    public SpriteRenderer glow;

    public void OnDrop(PointerEventData eventData)
    {
        glow.enabled = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            glow.enabled = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        glow.enabled = false;
    }
}
