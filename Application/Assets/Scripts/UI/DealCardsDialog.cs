using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DealCardsDialog : Dialog<DealCardsDialog>
{

    [SerializeField]
    private NumberInputField numberInputField;

    [SerializeField]
    private ToggleScrollView toggleScrollView;

    [SerializeField]
    private Text title;

    public delegate void DealCardsDialogCallbackDelegate(int number, bool[] checkData);

    private DealCardsDialogCallbackDelegate callback;


    public static void Show(string title, int maxNumberOfCards, string[] playerNames, DealCardsDialogCallbackDelegate callback)
    {
        var dialog = Create(MenuManager.Instance.dealCardsDialogPrefab);

        dialog.title.text = title;
        dialog.callback = callback;
        dialog.numberInputField.maxNumber = maxNumberOfCards;
        dialog.numberInputField.Number = 0;
        dialog.toggleScrollView.Init(playerNames, true);
    }

  

    public void OnCancelPressed()
    {
        Close();
    }

    public void OnOkButtonPressed()
    {
        if (callback != null)
        {
            callback(numberInputField.Number, toggleScrollView.GetCheckData());
        }

        Close();
    }
}
