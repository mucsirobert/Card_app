using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckPermissionSettings : Dialog<DeckPermissionSettings> {
    [SerializeField]
    private Dropdown ownerTakeAwayPermissionDropdown;
    [SerializeField]
    private Dropdown ownerShufflePermissionDropdown;
    [SerializeField]
    private Dropdown ownerDealPermissionDropdown;

    [SerializeField]
    private Dropdown othersTakeAwayPermissionDropdown;
    [SerializeField]
    private Dropdown othersShufflePermissionDropdown;
    [SerializeField]
    private Dropdown othersDealPermissionDropdown;

    public InputField nameInputField;
    [SerializeField]
    private Button colorButton;

    private DeckEditor deck;

    private static Color[] Colors = new Color[] { Color.magenta, Color.red, Color.cyan, Color.blue, Color.green, Color.yellow };
    private int colorIdx;

    public static void Show(DeckEditor deck)
    {
        var dialog = Create(MenuManager.Instance.deckPermissionSettingsPrefab);
        dialog.deck = deck;
        dialog.nameInputField.text = deck.Name;
        dialog.colorButton.GetComponent<Image>().color = deck.Color;
        dialog.colorIdx = Array.FindIndex(Colors, (color) => color.Equals(deck.Color));

        dialog.ownerTakeAwayPermissionDropdown.AddOptions(new List<string>(Permission.permissionTypeNames));
        dialog.ownerShufflePermissionDropdown.AddOptions(new List<string>(Permission.permissionTypeNames));
        dialog.ownerDealPermissionDropdown.AddOptions(new List<string>(Permission.permissionTypeNames));

        dialog.othersTakeAwayPermissionDropdown.AddOptions(new List<string>(Permission.permissionTypeNames));
        dialog.othersShufflePermissionDropdown.AddOptions(new List<string>(Permission.permissionTypeNames));
        dialog.othersDealPermissionDropdown.AddOptions(new List<string>(Permission.permissionTypeNames));

        dialog.ownerTakeAwayPermissionDropdown.value = (byte)deck.OwnerTakeAwayPermissionType;
        dialog.ownerShufflePermissionDropdown.value = (byte)deck.OwnerShufflePermissionType;
        dialog.ownerDealPermissionDropdown.value = (byte)deck.OwnerDealPermissionType;

        dialog.othersTakeAwayPermissionDropdown.value = (byte)deck.OthersTakeAwayPermissionType;
        dialog.othersShufflePermissionDropdown.value = (byte)deck.OthersShufflePermissionType;
        dialog.othersDealPermissionDropdown.value = (byte)deck.OthersDealPermissionType;

    }

    public void OnCancelPressed()
    {
        Close();
    }

    public void OnOkButtonPressed()
    {
        deck.OwnerTakeAwayPermissionType = (Permission.PermissionType) ownerTakeAwayPermissionDropdown.value;
        deck.OwnerShufflePermissionType = (Permission.PermissionType)ownerShufflePermissionDropdown.value;
        deck.OwnerDealPermissionType = (Permission.PermissionType)ownerDealPermissionDropdown.value;

        deck.OthersTakeAwayPermissionType = (Permission.PermissionType)othersTakeAwayPermissionDropdown.value;
        deck.OthersShufflePermissionType = (Permission.PermissionType)othersShufflePermissionDropdown.value;
        deck.OthersDealPermissionType = (Permission.PermissionType)othersDealPermissionDropdown.value;
        deck.Name = nameInputField.text;
        deck.Color = Colors[colorIdx];

        Close();
    }

    public void OnColorButtonClicked()
    {
        colorIdx = (colorIdx + 1) % Colors.Length;

        colorButton.GetComponent<Image>().color = Colors[colorIdx];
    }
}
