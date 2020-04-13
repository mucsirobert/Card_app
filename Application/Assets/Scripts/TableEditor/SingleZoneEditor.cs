using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleZoneEditor : EditorEntity
{
    private static int zoneNumber = 0;
    public bool CanOnlyHoldOneCard { get; set; }


    public int numberOfCards { get; set; }
    public bool Collapse { get; set; }
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
        tableData.Add(new SingleZoneData(this.transform.position, Name, Color, OwnerTakeAwayPermissionType, OwnerDropOntoPermissionType, OwnerViewPermissionType, OthersTakeAwayPermissionType, OthersDropOntoPermissionType, OthersViewPermissionType, FlipCardsWhenDropped, CanOnlyHoldOneCard, numberOfCards, Collapse));
    }

    public override void OnMenuItemClicked(ContextMenuItem menuItem)
    {
        if (menuItem.id == 1) { 
            //Settings
            SingleZoneSettings.Show(this);
        } else {
            base.OnMenuItemClicked(menuItem);
        };
    }
}
