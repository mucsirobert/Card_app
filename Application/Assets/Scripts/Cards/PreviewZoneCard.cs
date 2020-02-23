using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(BoxCollider2D), typeof(SpriteRenderer))]
public class PreviewZoneCard : MonoBehaviour
{
    public CardView cardView;

    public Droppable DroppableComponent { get; private set; }

    public SpriteRenderer SpriteRenderer { get; set; }
    private BoxCollider2D BoxCollider2D { get; set; }

    private Vector2 originalBoxColliderSize;


    private void Awake()
    {
        DroppableComponent = GetComponent<Droppable>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        
        DroppableComponent.IsDraggable = true;
        DroppableComponent.DropZoneBelow = PreviewZone.Instance.GetComponent<DropZone>();
        BoxCollider2D = GetComponent<BoxCollider2D>();
        originalBoxColliderSize = BoxCollider2D.size;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        BoxCollider2D.size = originalBoxColliderSize;
        BoxCollider2D.offset = Vector2.zero;
        BoxCollider2D.enabled = true;
    }



    public void OnDrag(PointerEventData eventData)
    {
    }

    public void SetCardView(CardView cardView)
    {
        this.cardView = cardView;
        SpriteRenderer.sprite = cardView.Sprite;
    }


}
