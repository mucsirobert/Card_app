using System;
using UnityEngine;
using UnityEngine.UI;

public class ContextMenuPanel : MonoBehaviour {

    public RectTransform contextMenuTransform;

    private static ContextMenuPanel contextMenuPanel;

    public Button contextMenuItemToSpawn;

    public static ContextMenuPanel Instance
    {
        get
        {
            if (!contextMenuPanel)
            {
                contextMenuPanel = FindObjectOfType(typeof(ContextMenuPanel)) as ContextMenuPanel;
                if (!contextMenuPanel)
                    Debug.LogError("There needs to be one active ContextMenuPanel script on a GameObject in your scene.");
            }

            return contextMenuPanel;
        }
    }

    public void Show(Vector3 positon, ContextMenu contextMenu)
    {
        contextMenuTransform.transform.parent.gameObject.SetActive(true);
        

        //We check if the contextMenu is outside of the screen
        if (positon.x > Screen.width/2)
        {
            contextMenuTransform.pivot = new Vector2(1, 1);
        } else
        {
            contextMenuTransform.pivot = new Vector2(0, 1);
        }
        contextMenuTransform.position = positon;


        var menuItems = contextMenu.GetMenuItems();
        foreach (var menuItem in menuItems)
        {
            Button contextMenuItem = Instantiate(contextMenuItemToSpawn, contextMenuTransform.transform);
            //The first image is the Button's image
            contextMenuItem.GetComponentsInChildren<Image>()[1].sprite = menuItem.icon;
            contextMenuItem.GetComponentsInChildren<Image>()[1].SetNativeSize();
            contextMenuItem.onClick.AddListener(() => { contextMenu.MenuItemClicked(menuItem); });
            contextMenuItem.onClick.AddListener(Close);
        }

    }

    public void Close()
    {
        contextMenuTransform.transform.parent.gameObject.SetActive(false);
        foreach (Transform child in contextMenuTransform.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
