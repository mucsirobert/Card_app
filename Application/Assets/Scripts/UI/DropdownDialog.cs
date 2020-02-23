using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownDialog : Dialog<DropdownDialog> {

    [SerializeField]
    private Dropdown dropdown;

    [SerializeField]
    private Text title;

    public delegate void ItemSelectedDelegate(int index);

    private ItemSelectedDelegate itemSelected;


    public static void Show(List<string> items, string title, ItemSelectedDelegate itemSelected)
    {
        var dialog = Create(MenuManager.Instance.dropdownDialogPrefab);

        dialog.title.text = title;
        dialog.dropdown.AddOptions(items);
        dialog.itemSelected = itemSelected;

    }

    public void OnCancelPressed()
    {
        Close();
    }

    public void OnOkButtonPressed()
    {
       if (itemSelected != null)
        {
            itemSelected(dropdown.value);
        }

        Close();
    }
}
