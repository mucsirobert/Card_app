using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleZoneEditor : EditorEntity
{
    private static int zoneNumber = 0;
    public bool CanOnlyHoldOneCard { get; set; }
    public List<SingleZoneEditor> clonedZones = new List<SingleZoneEditor>();
   
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
        tableData.Add(new SingleZoneData(this.transform.position, Name, Color, OwnerTakeAwayPermissionType, OwnerDropOntoPermissionType, OwnerViewPermissionType, OthersTakeAwayPermissionType, OthersDropOntoPermissionType, OthersViewPermissionType, FlipCardsWhenDropped, CanOnlyHoldOneCard));
        foreach (SingleZoneEditor item in clonedZones)
        {
            tableData.Add(new SingleZoneData(item.transform.position, Name, Color, OwnerTakeAwayPermissionType, OwnerDropOntoPermissionType, OwnerViewPermissionType, OthersTakeAwayPermissionType, OthersDropOntoPermissionType, OthersViewPermissionType, FlipCardsWhenDropped, CanOnlyHoldOneCard));

        }
    }

    public void copyProperties(SingleZoneEditor singlezone)
    {
        singlezone.OwnerTakeAwayPermissionType = this.OwnerTakeAwayPermissionType;
        singlezone.OthersTakeAwayPermissionType = this.OthersTakeAwayPermissionType;
        singlezone.OwnerViewPermissionType = this.OwnerViewPermissionType;
        singlezone.OthersViewPermissionType = this.OthersViewPermissionType;
        singlezone.FlipCardsWhenDropped = this.FlipCardsWhenDropped;
        singlezone.OwnerDropOntoPermissionType = this.OwnerDropOntoPermissionType;
        singlezone.OthersDropOntoPermissionType = this.OthersDropOntoPermissionType;
        singlezone.CanOnlyHoldOneCard = this.CanOnlyHoldOneCard;
    }

    public override void OnMenuItemClicked(ContextMenuItem menuItem)
    {
        if (menuItem.id == 2)
        {
            SingleZoneEditor cloned = Instantiate(this);
            copyProperties(cloned);
            cloned.transform.position = new Vector3(0, 0, 0f);
            clonedZones.Add(cloned);
        }
        if (menuItem.id == 1) { 
            //Settings
            SingleZoneSettings.Show(this);
        } else {
            base.OnMenuItemClicked(menuItem);
        };
    }
}
