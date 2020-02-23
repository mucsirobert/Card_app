using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UndoRedoUI : MonoBehaviour
{
    [SerializeField]
    private Button undoButton;

    [SerializeField]
    private Button redoButton;

    [SerializeField]
    private TableButton playerButtonPrefab;

    [SerializeField]
    private TableButton centerButton;

    [SerializeField]
    private TableButton ownPlayerButton;

    [SerializeField]
    private GameObject playerNamePanel;

    [SerializeField]
    private GameObject centerTable;

    public GameObject OwnTable { get; set; }

    private List<TableButton> buttons;


    public void OnUndoButtonClicked()
    {
        CommandProcessor.Instance.CmdUndoLastServerCommand();
    }

    public void OnRedoButtonClicked()
    {
        CommandProcessor.Instance.CmdRedoLastServerCommand();
    }

    private void Awake()
    {
        buttons = new List<TableButton>
        {
            centerButton
        };
        centerButton.Button.onClick.AddListener(MoveCameraToCenter);
        centerButton.Button.onClick.AddListener(() => { OnTableButtonClicked(centerButton); });
        //Make the centerButton the "highlighted" one
        centerButton.Toggle(false);
    }


    public void CreatePlayerButton(Player player)
    {
        if (player.isLocalPlayer)
        {
            ownPlayerButton.Button.GetComponentInChildren<Text>().text = player.playerName;
            ownPlayerButton.Button.GetComponent<Image>().color = player.playerColor;
            ownPlayerButton.Button.onClick.AddListener(MoveCameraToOwnTable);
            ownPlayerButton.Button.onClick.AddListener(() => { OnTableButtonClicked(ownPlayerButton); });
            buttons.Add(ownPlayerButton);
        }
        else
        {
            var playerButton = Instantiate(playerButtonPrefab, playerNamePanel.transform);
            playerButton.Button.onClick.AddListener(() => { MoveCameraToTable(player.Table.gameObject); });
            playerButton.Button.onClick.AddListener(() => { OnTableButtonClicked(playerButton); });
            playerButton.Button.GetComponentInChildren<Text>().text = player.playerName;
            playerButton.Button.GetComponent<Image>().color = player.playerColor;
            buttons.Add(playerButton);

        }
    }

    private void OnTableButtonClicked(TableButton tableButton)
    {
        //Highlight only the clicked button
        buttons.ForEach(button => { button.Toggle(true); });

        tableButton.Toggle(false);
    }

    private void MoveCameraToCenter()
    {
        Camera.main.transform.DOKill();
        Camera.main.transform.DOMoveY(centerTable.transform.position.y, 0.5f).OnComplete(() => {

            var players = Player.Players;

            foreach (var p in players)
            {
                p.Table.gameObject.SetActive(true);
            }
        });
    }

    private void MoveCameraToOwnTable()
    {
        Camera.main.transform.DOKill();
        Camera.main.transform.DOMoveY(OwnTable.transform.position.y, 0.5f).OnComplete(() => {

            var players = Player.Players;

            foreach (var p in players)
            {
                p.Table.gameObject.SetActive(true);
            }
        });
    }

    private void MoveCameraToTable(GameObject table)
    {
        var players = Player.Players;

        foreach (var p in players)
        {
            if (!p.isLocalPlayer)
                p.Table.gameObject.SetActive(false);
        }

        table.SetActive(true);

        Camera.main.transform.DOKill();
        Camera.main.transform.DOMoveY(table.transform.position.y, 0.5f);





    }

}
