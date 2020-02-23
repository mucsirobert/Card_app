using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class QuestionPanel : Dialog<QuestionPanel> {
    public Text textField;
    public Button yesButton;
    public Button noButton;

    public static QuestionPanel Show(string question, UnityAction yesAction, UnityAction noAction)
    {
        var dialog = Create(MenuManager.Instance.questionPanelPrefab);
        dialog.yesButton.onClick.RemoveAllListeners();
        dialog.yesButton.onClick.AddListener(dialog.Close);
        if (yesAction != null)
            dialog.yesButton.onClick.AddListener(yesAction);

        dialog.noButton.onClick.RemoveAllListeners();
        dialog.noButton.onClick.AddListener(dialog.Close);
        if (noAction != null)
            dialog.noButton.onClick.AddListener(noAction);



        dialog.textField.text = question;

        dialog.yesButton.gameObject.SetActive(true);
        dialog.noButton.gameObject.SetActive(true);

        return dialog;
    }

}
