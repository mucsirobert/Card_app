using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

 public delegate void JoinServerDelegate(string ip);
public class JoinGameMenu : MonoBehaviour {


    [SerializeField]
    private LocalServerList serverList;

    [SerializeField]
    private InputField ipInputField;

    private JoinServerDelegate onJoinServer;

    private UnityAction onBackPressed;

    public void OnJoinPressed()
    {
        if (onJoinServer != null)
        {
            onJoinServer(ipInputField.text);
        }
    }

    public void OnBackButtonPressed()
    {
        Hide();
        if (onBackPressed != null)
        {
            onBackPressed();
        }
    }

    public void Show(JoinServerDelegate onJoinServer, UnityAction onBackPressed)
    {
        gameObject.SetActive(true);
        this.onJoinServer = onJoinServer;
        this.onBackPressed = onBackPressed;

        serverList.Init(this.onJoinServer);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
