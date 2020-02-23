using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalServerEntry : MonoBehaviour {

    public Text serverInfoText;
    public Text slotInfo;
    public Button joinButton;

    public void Populate(LocalServerInfo server, Color c, JoinServerDelegate onJoinServer)
    {
        serverInfoText.text = server.ip;

        slotInfo.text = server.currentSize.ToString() + "/" + server.maxSize.ToString();

        joinButton.onClick.RemoveAllListeners();
        if (onJoinServer != null)
        {
            joinButton.onClick.AddListener(() => { onJoinServer(server.ip); });
        }

        GetComponent<Image>().color = c;
    }
}
