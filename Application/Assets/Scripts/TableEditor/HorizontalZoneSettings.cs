using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HorizontalZoneSettings : Dialog<HorizontalZoneSettings>
{

    public Dropdown ownerTakeAwayPermissionDropdown;
    public Dropdown ownerDropOntoPermissionDropdown;
    public Dropdown ownerViewPermissionDropdown;

    public Dropdown othersTakeAwayPermissionDropdown;
    public Dropdown othersDropOntoPermissionDropdown;
    public Dropdown othersViewPermissionDropdown;
    public InputField nameInputField;

    public Toggle flipCardsWhenDroppedToggle;

    private HorizontalZoneEditor zone;

    [SerializeField]
    private Button colorButton;
    private static Color[] Colors = new Color[] { Color.magenta, Color.red, Color.cyan, Color.blue, Color.green, Color.yellow };
    private int colorIdx;


    public static void Show(HorizontalZoneEditor zone)
    {
        var dialog = Create(MenuManager.Instance.horizontalZoneSettingsPrefab);
        dialog.zone = zone;
        dialog.nameInputField.text = zone.Name;
        dialog.colorButton.GetComponent<Image>().color = zone.Color;
        dialog.colorIdx = Array.FindIndex(Colors, (color) => color.Equals(zone.Color));

        dialog.ownerTakeAwayPermissionDropdown.AddOptions(new List<string>(Permission.permissionTypeNames));
        dialog.ownerDropOntoPermissionDropdown.AddOptions(new List<string>(Permission.permissionTypeNames));
        dialog.ownerViewPermissionDropdown.AddOptions(new List<string>(Permission.permissionTypeNames));

        dialog.othersTakeAwayPermissionDropdown.AddOptions(new List<string>(Permission.permissionTypeNames));
        dialog.othersDropOntoPermissionDropdown.AddOptions(new List<string>(Permission.permissionTypeNames));
        dialog.othersViewPermissionDropdown.AddOptions(new List<string>(Permission.permissionTypeNames));


        dialog.ownerTakeAwayPermissionDropdown.value = ((byte)zone.OwnerTakeAwayPermissionType);
        dialog.ownerDropOntoPermissionDropdown.value = ((byte)zone.OwnerDropOntoPermissionType);
        dialog.ownerViewPermissionDropdown.value = ((byte)zone.OwnerViewPermissionType);

        dialog.othersTakeAwayPermissionDropdown.value = ((byte)zone.OthersTakeAwayPermissionType);
        dialog.othersDropOntoPermissionDropdown.value = ((byte)zone.OthersDropOntoPermissionType);
        dialog.othersViewPermissionDropdown.value = ((byte)zone.OthersViewPermissionType);
        dialog.flipCardsWhenDroppedToggle.isOn = zone.FlipCardsWhenDropped;

    }

    public void OnCancelPressed()
    {
        Close();
    }

    public void OnOkButtonPressed()
    {
        zone.OwnerTakeAwayPermissionType = (Permission.PermissionType)ownerTakeAwayPermissionDropdown.value;
        zone.OwnerDropOntoPermissionType = (Permission.PermissionType)ownerDropOntoPermissionDropdown.value;
        zone.OwnerViewPermissionType = (Permission.PermissionType)ownerViewPermissionDropdown.value;

        zone.OthersTakeAwayPermissionType = (Permission.PermissionType)othersTakeAwayPermissionDropdown.value;
        zone.OthersDropOntoPermissionType = (Permission.PermissionType)othersDropOntoPermissionDropdown.value;
        zone.OthersViewPermissionType = (Permission.PermissionType)othersViewPermissionDropdown.value;
        zone.Name = nameInputField.text;
        zone.Color = Colors[colorIdx];
        zone.FlipCardsWhenDropped = flipCardsWhenDroppedToggle.isOn;

        Close();
    }

    public void OnColorButtonClicked()
    {
        colorIdx = (colorIdx + 1) % Colors.Length;

        colorButton.GetComponent<Image>().color = Colors[colorIdx];
    }
}
