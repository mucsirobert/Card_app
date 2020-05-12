using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ContextMenu))]
public abstract class EditorEntity : MonoBehaviour
{
    public string Name { get; set; }
    public bool FlipCardsWhenDropped { get; set; }

    public Permission.PermissionType OwnerTakeAwayPermissionType { get; set; }
    public Permission.PermissionType OwnerDropOntoPermissionType { get; set; }

    public Permission.PermissionType OthersTakeAwayPermissionType { get; set; }
    public Permission.PermissionType OthersDropOntoPermissionType { get; set; }

    private ContextMenu contextMenuComponent;

    [SerializeField]
    private SpriteRenderer colorSprite;

    private Color color = Color.magenta;
    public Color Color
    {
        get
        {
            return color;
        }
        set
        {
            color = value;
            colorSprite.color = color;
        }
    }


    

    public abstract void Save(TableData tableData);

    protected virtual void Awake()
    {
        contextMenuComponent = GetComponent<ContextMenu>();
        colorSprite.color = color;
    }

    // Use this for initialization
    public virtual void Start()
    {
    }

    public virtual void OnMenuItemClicked(ContextMenuItem menuItem)
    {
        if (menuItem.id == 0)
        {
            //Remove
            Destroy(this.gameObject);
        }
    }

    public virtual void OnTouch(Vector3 touchPosition)
    {
    }

    public virtual void OnLongTouch(Vector3 touchPosition)
    {
        ContextMenuPanel.Instance.Show(touchPosition, contextMenuComponent);
    }


}
