using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
//using System.Media;
//using System.Security.Policy;
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
