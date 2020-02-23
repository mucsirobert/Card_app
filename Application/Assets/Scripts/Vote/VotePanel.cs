using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class VotePanel : Dialog<VotePanel> {
    [SerializeField]
    private PlayerVoteStatusUI playerVoteStatusPrefab;

    [SerializeField]
    private GameObject playerVoteStatusHolder;

    private Dictionary<Player, PlayerVoteStatusUI> playerStatusDictionary;

    public Text textField;
    public Button yesButton;
    public Button noButton;

    public static VotePanel Show(string question, UnityAction yesAction, UnityAction noAction)
    {
        var dialog = Create(MenuManager.Instance.votePanelPrefab);
        dialog.yesButton.onClick.RemoveAllListeners();
        dialog.yesButton.onClick.AddListener(yesAction);

        dialog.noButton.onClick.RemoveAllListeners();
        dialog.noButton.onClick.AddListener(noAction);



        dialog.textField.text = question;

        dialog.yesButton.gameObject.SetActive(true);
        dialog.noButton.gameObject.SetActive(true);

        dialog.playerStatusDictionary = new Dictionary<Player, PlayerVoteStatusUI>();
        for (int i = 0; i < Player.Players.Count; i++)
        {
            var playerVoteStatus = Instantiate(dialog.playerVoteStatusPrefab, dialog.playerVoteStatusHolder.transform);
            dialog.playerStatusDictionary.Add(Player.Players[i], playerVoteStatus);
            playerVoteStatus.SetPlayerName(Player.Players[i].playerName);
        }

        return dialog;
    }

    public void LocalPlayerVoted()
    {
        textField.text = "Waiting for other players to vote...";
        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);
    }

    public void PlayerVoted(Player player, Vote.Options option)
    {
        PlayerVoteStatusUI playerVoteStatus;
        if (playerStatusDictionary.TryGetValue(player, out playerVoteStatus))
        {
            playerVoteStatus.SetVote(option);
        }
    }

    public void VotePassed()
    {
        textField.text = "Passed";
        StartCoroutine(ClosePanelAfter(1.5f));
    }

    public void VoteFailed()
    {
        textField.text = "Failed";
        StartCoroutine(ClosePanelAfter(1.5f));
    }

    private System.Collections.IEnumerator ClosePanelAfter(float seconds)
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
