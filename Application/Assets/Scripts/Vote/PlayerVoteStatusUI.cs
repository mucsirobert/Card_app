using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerVoteStatusUI : MonoBehaviour {

    [SerializeField]
    private Text playerNameField;

    [SerializeField]
    private GameObject yesImage;
    [SerializeField]
    private GameObject noImage;
    [SerializeField]
    private GameObject waitingImage;


    public void SetPlayerName(string name)
    {
        playerNameField.text = name;
    }

    public void SetVote(Vote.Options option)
    {
        if (option == Vote.Options.Yes)
        {
            yesImage.SetActive(true);
            waitingImage.SetActive(false);
            noImage.SetActive(false);
        } else if (option == Vote.Options.No)
        {
            yesImage.SetActive(false);
            waitingImage.SetActive(false);
            noImage.SetActive(true);
        }
    }
}
