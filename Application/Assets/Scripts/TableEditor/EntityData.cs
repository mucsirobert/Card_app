using Newtonsoft.Json;
using System;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public abstract class EntityData {


    [JsonProperty]
    public Vector3 Position { get; set; }

    [JsonProperty]
    public string Name { get; set; }
    [JsonProperty]
    public int numberOfCards { get; set; }
    [JsonProperty]
    public string Color { get; set; }

    [JsonProperty]
    private Permission.PermissionType ownerTakeAwayPermissionType;
    [JsonProperty]
    private Permission.PermissionType ownerDropOntoPermissionType;

    [JsonProperty]
    private Permission.PermissionType othersTakeAwayPermissionType;
    [JsonProperty]
    private Permission.PermissionType othersDropOntoPermissionType;

    [JsonProperty]
    public bool FlipCardsWhenDropped { get; set; }

    public EntityData()
    {
        
    }

    public EntityData(Vector3 position, string name, Color color, Permission.PermissionType ownerTakeAwayPermissionType, Permission.PermissionType ownerDropOntoPermissionType,
        Permission.PermissionType othersTakeAwayPermissionType, Permission.PermissionType othersDropOntoPermissionType, bool flipCardsWhenDropped, int numberOfCards)
    {
        this.ownerTakeAwayPermissionType = ownerTakeAwayPermissionType;
        this.ownerDropOntoPermissionType = ownerDropOntoPermissionType;
        this.othersTakeAwayPermissionType = othersTakeAwayPermissionType;
        this.othersDropOntoPermissionType = othersDropOntoPermissionType;

        this.numberOfCards = numberOfCards;
        Position = position;
        Name = name;
        FlipCardsWhenDropped = flipCardsWhenDropped;

        Color = '#' + ColorUtility.ToHtmlStringRGBA(color);

    }

    public void SpawnEntity(GameObject parent, Player owner)
    {
        Zone zone = GetEntityToSpawn(parent.transform);

        zone.localPosition = Position;
        zone.transform.localPosition = Position;

        zone.ownerTakeAwayPermissionType = ownerTakeAwayPermissionType;
        zone.othersTakeAwayPermissionType = othersTakeAwayPermissionType;
        zone.ownerDropOntoPermissionType = ownerDropOntoPermissionType;
        zone.othersDropOntoPermissionType = othersDropOntoPermissionType;

        zone.parent = parent;
        zone.defauldIsFacingUp = FlipCardsWhenDropped;

        if (owner != null)
        {
            zone.zoneName = owner.playerName + "'s " + Name;
            zone.ownerNetId = owner.netId;
        } else
        {
            zone.zoneName = Name;
        }

        Color newCol;
        if (ColorUtility.TryParseHtmlString(Color, out newCol))
            zone.zoneColor = newCol;

        NetworkServer.Spawn(zone.gameObject);
    }

    protected abstract Zone GetEntityToSpawn(Transform parent);
    protected abstract EditorEntity GetEditorEntityToSpawn(Transform parent);


    public void SpawnEditorEntity(GameObject parent)
    {
        var entityToSpawn = GetEditorEntityToSpawn(parent.transform);
        entityToSpawn.OwnerTakeAwayPermissionType = ownerTakeAwayPermissionType;
        entityToSpawn.OthersTakeAwayPermissionType = othersTakeAwayPermissionType;
        entityToSpawn.OwnerDropOntoPermissionType = ownerDropOntoPermissionType;
        entityToSpawn.OthersDropOntoPermissionType = othersDropOntoPermissionType;

        entityToSpawn.transform.localPosition = Position;
        entityToSpawn.Name = Name;
        entityToSpawn.FlipCardsWhenDropped = FlipCardsWhenDropped;
        Color newCol;
        if (ColorUtility.TryParseHtmlString(Color, out newCol))
            entityToSpawn.Color = newCol;

    }

}
