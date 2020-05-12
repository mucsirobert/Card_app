using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class HorizontalZoneEditor : EditorEntity
{

    private static int zoneNumber = 0;


    public int numberOfCards { get; set; }
    public bool Collapse { get; set; }

    public EditModeManager editModeManager;
    public List<HorizontalZoneEditor> clonedZones = new List<HorizontalZoneEditor>();
    

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

        tableData.Add(new HorizontalZoneData(this.transform.position, Name, Color, OwnerTakeAwayPermissionType, OwnerDropOntoPermissionType, OwnerViewPermissionType, OthersTakeAwayPermissionType, OthersDropOntoPermissionType, OthersViewPermissionType, FlipCardsWhenDropped, numberOfCards, Collapse));
        
        foreach (HorizontalZoneEditor item in clonedZones)
            {
                tableData.Add(new HorizontalZoneData(item.transform.position, Name, Color, OwnerTakeAwayPermissionType, OwnerDropOntoPermissionType, OwnerViewPermissionType, OthersTakeAwayPermissionType, OthersDropOntoPermissionType, OthersViewPermissionType, FlipCardsWhenDropped, numberOfCards, Collapse));
             
            }
     
       
    }

    public void copyProperties(HorizontalZoneEditor zone) {
        zone.OwnerTakeAwayPermissionType = this.OwnerTakeAwayPermissionType;
        zone.OthersTakeAwayPermissionType = this.OthersTakeAwayPermissionType;
        zone.OwnerViewPermissionType = this.OwnerViewPermissionType;
        zone.OthersViewPermissionType = this.OthersViewPermissionType;
        zone.FlipCardsWhenDropped = this.FlipCardsWhenDropped;
        zone.OwnerDropOntoPermissionType = this.OwnerDropOntoPermissionType;
        zone.OthersDropOntoPermissionType = this.OthersDropOntoPermissionType;
        zone.numberOfCards = this.numberOfCards;
        zone.Collapse = this.Collapse;

    }

    public override void OnMenuItemClicked(ContextMenuItem menuItem)
    {
        if (menuItem.id == 2) {
            
             HorizontalZoneEditor cloned = Instantiate(this);

             copyProperties(cloned);
            
             cloned.transform.position = new Vector3(0, 0, 0f);
             clonedZones.Add(cloned);
            
        }
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
