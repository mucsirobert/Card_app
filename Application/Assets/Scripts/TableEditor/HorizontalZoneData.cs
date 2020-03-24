using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class HorizontalZoneData : EntityData {
    [JsonProperty]
    private Permission.PermissionType ownerViewPermissionType;

    [JsonProperty]
    private Permission.PermissionType othersViewPermissionType;

    public HorizontalZoneData()
    {
         
    }

    public HorizontalZoneData(Vector3 position, string name, Color color, Permission.PermissionType ownerTakeAwayPermissionType, Permission.PermissionType ownerDropOntoPermissionType, Permission.PermissionType ownerViewPermissionType,
        Permission.PermissionType othersTakeAwayPermissionType, Permission.PermissionType othersDropOntoPermissionType, Permission.PermissionType othersViewPermissionType, bool flipCardsWhenDropped, int numberOfCards)
        : base(position, name, color, ownerTakeAwayPermissionType, ownerDropOntoPermissionType, othersTakeAwayPermissionType, othersDropOntoPermissionType, flipCardsWhenDropped, numberOfCards)
    {
        this.ownerViewPermissionType = ownerViewPermissionType;
        this.othersViewPermissionType = othersViewPermissionType;
    }

    protected override Zone GetEntityToSpawn(Transform parent)
    {
        var entity = UnityEngine.Object.Instantiate(TableEditorDataHolder.Instance.horizontalZonePrefab, parent);

        entity.ownerViewPermissionType = ownerViewPermissionType;
        entity.othersViewPermissionType = othersViewPermissionType;
        entity.numberOfCards = NumberOfCards;

        return entity;
    }

    protected override EditorEntity GetEditorEntityToSpawn(Transform parent)
    {
        var entityToSpawn = UnityEngine.Object.Instantiate(TableEditorDataHolder.Instance.horizontalZoneEditorPrefab, parent);

        entityToSpawn.OwnerViewPermissionType = ownerViewPermissionType;
        entityToSpawn.OthersViewPermissionType = othersViewPermissionType;
        entityToSpawn.numberOfCards = NumberOfCards;

        return entityToSpawn;
    }
}
