using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextInputDialog : Dialog<TextInputDialog>
{

    [SerializeField]
    private InputField inputField;

    [SerializeField]
    private Text title;

    public delegate void TextEnteredDelegate(string text);

    private TextEnteredDelegate textEntered;


    public static void Show(string title, TextEnteredDelegate itemSelected)
    {
        var dialog = Create(MenuManager.Instance.textInputDialogPrefab);

        dialog.title.text = title;
        dialog.textEntered = itemSelected;

    }

    public void OnCancelPressed()
    {
        Close();
    }

    public void OnOkButtonPressed()
    {
        if (textEntered != null)
        {
            textEntered(inputField.text);
        }

        Close();
    }
}
