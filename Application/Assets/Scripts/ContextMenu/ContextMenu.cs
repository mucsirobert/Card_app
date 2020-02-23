using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ContextMenuItemClickedEvent : UnityEvent<ContextMenuItem> { }

public class ContextMenu : MonoBehaviour {

    public ContextMenuItemClickedEvent itemClickedEvent;

    public List<ContextMenuItem> menuItems = new List<ContextMenuItem>();

    /*public void AddMenuItem(ContextMenuItem menuItem)
    {
        menuItems.Add(menuItem);
    }*/

    public List<ContextMenuItem> GetMenuItems()
    {
        return menuItems;
    }

    public void Show(Vector3 position)
    {
        ContextMenuPanel.Instance.Show(position, this);
    }

    public void MenuItemClicked(ContextMenuItem item)
    {
        itemClickedEvent.Invoke(item);
    }
}

[System.Serializable]
public class ContextMenuItem
{
    public int id;
    public string name;
    public Sprite icon;

    public ContextMenuItem(int id, string name, Sprite icon)
    {
        this.id = id;
        this.icon = icon;
        this.name = name;
    }
}
