using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton : MonoBehaviour {

    public void ExitPressed()
    {
        var mainMenu = GameObject.FindGameObjectWithTag("MainMenuManager");
        if (mainMenu != null)
        {
            mainMenu.GetComponent<MainMenuManager>().OnIngameExitPressed();
        }

    }



}
