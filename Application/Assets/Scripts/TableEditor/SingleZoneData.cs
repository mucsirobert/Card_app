using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class SingleZoneData : EntityData {
   
    [JsonProperty]
    private Permission.PermissionType ownerViewPermissionType;
    [JsonProperty]
    private Permission.PermissionType othersViewPermissionType;

    [JsonProperty]
    public bool canOnlyHoldOneCard;

    public SingleZoneData()
    {
    }

    public SingleZoneData(Vector3 position, string name, Color color, Permission.PermissionType ownerTakeAwayPermissionType, Permission.PermissionType ownerDropOntoPermissionType, Permission.PermissionType ownerViewPermissionType,
        Permission.PermissionType othersTakeAwayPermissionType, Permission.PermissionType othersDropOntoPermissionType, Permission.PermissionType othersViewPermissionType, bool flipCardsWhenDropped, bool canOnlyHoldOneCard, int numberOfCards, bool Collapse) 
        : base(position, name, color, ownerTakeAwayPermissionType, ownerDropOntoPermissionType, othersTakeAwayPermissionType, othersDropOntoPermissionType, flipCardsWhenDropped, numberOfCards)
    {

        
        this.ownerViewPermissionType = ownerViewPermissionType;
        this.othersViewPermissionType = othersViewPermissionType;
        this.canOnlyHoldOneCard = canOnlyHoldOneCard;
    }

    protected override Zone GetEntityToSpawn(Transform parent)
    {
        var entity = UnityEngine.Object.Instantiate(TableEditorDataHolder.Instance.singleZonePrefab, parent);

        
        entity.ownerViewPermissionType = ownerViewPermissionType;
        entity.othersViewPermissionType = othersViewPermissionType;
        entity.canOnlyHoldOneCard = canOnlyHoldOneCard;
        entity.defauldIsFacingUp = FlipCardsWhenDropped;

        return entity;
    }

    protected override EditorEntity GetEditorEntityToSpawn(Transform parent)
    {
        var entityToSpawn = UnityEngine.Object.Instantiate(TableEditorDataHolder.Instance.singleZoneEditorPrefab, parent);

        entityToSpawn.OwnerViewPermissionType = ownerViewPermissionType;
        entityToSpawn.OthersViewPermissionType = othersViewPermissionType;
        entityToSpawn.FlipCardsWhenDropped = FlipCardsWhenDropped;
        entityToSpawn.CanOnlyHoldOneCard = canOnlyHoldOneCard;

        return entityToSpawn;
    }


}
