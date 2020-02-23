using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public DeckSettings deckSettingsPrefab;
    public SingleZoneSettings zoneSettingsPrefab;
    public HorizontalZoneSettings horizontalZoneSettingsPrefab;
    public DeckPermissionSettings deckPermissionSettingsPrefab;

    

    public LogPanel logPanelPrefab;
    public QuestionPanel questionPanelPrefab;
    public DropdownDialog dropdownDialogPrefab;
    public TextInputDialog textInputDialogPrefab;
    public DealCardsDialog dealCardsDialogPrefab;
    public VotePanel votePanelPrefab;


    public InfoPanel infoPanelPrefab;
    public GameObject backPanel;


    private Stack<Dialog> dialogStack = new Stack<Dialog>();

    public static MenuManager Instance { get; set; }

    private void Awake()
    {
        Instance = this;

        //MainMenu.Show();
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    public T CreateDialog<T>(T dialogPrefab) where T : Dialog
    {
        T newDialog = Instantiate(dialogPrefab, transform);

        return newDialog;
    }

    public void ShowDialog<T>(T newDialog) where T : Dialog
    {
        Debug.Log("ShowDialog " +newDialog);
        dialogStack.Push(newDialog);
        newDialog.transform.SetSiblingIndex(transform.childCount - 1);
        newDialog.gameObject.SetActive(true);

        backPanel.SetActive(true);
        backPanel.transform.SetSiblingIndex(dialogStack.Count - 1);
    }


    //public void OpenMenu(Dialog instance)
    //{
    //    // De-activate top menu
    //    if (dialogStack.Count > 0)
    //    {
    //        if (instance.DisableMenusUnderneath)
    //        {
    //            foreach (var menu in dialogStack)
    //            {
    //                menu.gameObject.SetActive(false);

    //                if (menu.DisableMenusUnderneath)
    //                    break;
    //            }
    //        }

    //        /*var topCanvas = instance.GetComponent<Canvas>();
    //        var previousCanvas = menuStack.Peek().GetComponent<Canvas>();
    //        topCanvas.sortingOrder = previousCanvas.sortingOrder + 1;*/
    //    }

    //    dialogStack.Push(instance);
    //    backPanel.SetActive(true);
    //}


    public void CloseDialog(Dialog dialog)
    {

        if (dialogStack.Count == 0)
        {
            Debug.LogErrorFormat(dialog, "{0} cannot be closed because menu stack is empty", dialog.GetType());
            return;
        }

        if (dialogStack.Peek() != dialog)
        {
            Debug.LogErrorFormat(dialog, "{0} cannot be closed because it is not on top of stack", dialog.GetType());
            return;
        }

        //CloseTopMenu();
        //backPanel.SetActive(false);
        dialogStack.Pop();

        if (dialogStack.Count > 0)
        {
            backPanel.transform.SetSiblingIndex(dialogStack.Count - 1);
        } else
        {
            backPanel.SetActive(false);
        }

        //Destroy(dialog.gameObject);
    }

  /*  public void CloseTopMenu()
    {
        var instance = dialogStack.Pop();

        if (instance.DestroyWhenClosed)
            Destroy(instance.gameObject);
        else
            instance.gameObject.SetActive(false);

        // Re-activate top menu
        // If a re-activated menu is an overlay we need to activate the menu under it
        foreach (var menu in dialogStack)
        {
            menu.gameObject.SetActive(true);

            if (menu.DisableMenusUnderneath)
                break;
        }
    }*/

    /*private void Update()
    {
        // On Android the back button is sent as Esc
        if (Input.GetKeyDown(KeyCode.Escape) && dialogStack.Count > 0)
        {
            dialogStack.Peek().OnBackPressed();
        }
    }*/

}