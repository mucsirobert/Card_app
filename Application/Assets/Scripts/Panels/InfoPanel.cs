using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : Dialog<InfoPanel>
{
    public Text textField;


    public static InfoPanel Show(string question, float hideAfterSeconds = 0)
    {
        var dialog = Create(MenuManager.Instance.infoPanelPrefab);

        dialog.textField.text = question;

        if (hideAfterSeconds > 0)
        {
            dialog.StartCoroutine(dialog.ClosePanelAfter(hideAfterSeconds));
        }

        return dialog;
    }


    private IEnumerator ClosePanelAfter(float seconds)
    {
        float remainingTime = seconds;

        while (remainingTime > 0)
        {
            yield return null;

            remainingTime -= Time.deltaTime;
        }

        Close();
    }


}
