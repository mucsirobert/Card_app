using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalZoneEditor : EditorEntity
{

    private static int zoneNumber = 0;

    public int numberOfCards { get; set; }
    public Permission.PermissionType OwnerViewPermissionType { get; set; }
    public Permission.PermissionType OthersViewPermissionType { get; set; }

    protected override void Awake()
    {
        base.Awake();
    }



    public override void Start()
    {
        base.Start();

        if (string.IsNullOrEmpty(Name))
        {
            Name = "Zone" + (++zoneNumber);
        }
    }


    public override void Save(TableData tableData)
    {
        tableData.Add(new HorizontalZoneData(this.transform.position, Name, Color, OwnerTakeAwayPermissionType, OwnerDropOntoPermissionType, OwnerViewPermissionType, OthersTakeAwayPermissionType, OthersDropOntoPermissionType, OthersViewPermissionType, FlipCardsWhenDropped, numberOfCards));
    }

    public override void OnMenuItemClicked(ContextMenuItem menuItem)
    {
        if (menuItem.id == 1)
        {
            //Settings
            HorizontalZoneSettings.Show(this);
        }
        else
        {
            base.OnMenuItemClicked(menuItem);
        };
    }

}
