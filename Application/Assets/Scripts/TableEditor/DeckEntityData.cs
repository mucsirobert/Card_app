using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class DeckEntityData : EntityData
{
    [JsonProperty]
    private DeckLayout deckData;
    [JsonProperty]
    private DeckMeta deckInfo;
    [JsonProperty]
    private Permission.PermissionType ownerShufflePermissionType;
    [JsonProperty]
    private Permission.PermissionType ownerDealPermissionType;

    [JsonProperty]
    private Permission.PermissionType othersShufflePermissionType;
    [JsonProperty]
    private Permission.PermissionType othersDealPermissionType;

    /*//[SerializeField]
    [JsonProperty]
    private string deckDataFileName;*/

    [JsonConstructor]
    public DeckEntityData()
    {
        //this.deckDataFileName = deckDataFileName;
        /*deckData = DeckSettings.LoadDeckDataFromFile(deckDataFileName);
        deckInfo = DeckSettings.LoadDeckInfoFromFile(deckData.deckInfoPath);*/
    }

    public DeckEntityData(Vector3 position, string name, Color color, DeckMeta deckInfo, DeckLayout deckData, Permission.PermissionType ownerTakeAwayPermissionType, Permission.PermissionType ownerShufflePermissionType, Permission.PermissionType ownerDealPermissionType,
         Permission.PermissionType othersTakeAwayPermissionType, Permission.PermissionType othersShufflePermissionType, Permission.PermissionType othersDealPermissionType, bool flipCardsWhenDropped, int numberOfCards) 
        : base(position, name, color, ownerTakeAwayPermissionType, Permission.PermissionType.DENY, othersTakeAwayPermissionType, Permission.PermissionType.DENY, flipCardsWhenDropped, numberOfCards)
    {
        this.deckData = deckData;
        this.deckInfo = deckInfo;
        this.ownerShufflePermissionType = ownerShufflePermissionType;
        this.ownerDealPermissionType = ownerDealPermissionType;

        this.othersShufflePermissionType = othersShufflePermissionType;
        this.othersDealPermissionType = othersDealPermissionType;
        //this.deckDataFileName = deckDataFileName;
    }


    protected override Zone GetEntityToSpawn(Transform parent)
    {
        var deckEntity = UnityEngine.Object.Instantiate(TableEditorDataHolder.Instance.deckPrefab, parent);

        deckEntity.DeckData = deckData;
        deckEntity.DeckInfo = deckInfo;
        deckEntity.ownerShufflePermissionType = ownerShufflePermissionType;
        deckEntity.ownerDealPermissionType = ownerDealPermissionType;

        deckEntity.othersShufflePermissionType = othersShufflePermissionType;
        deckEntity.othersDealPermissionType = othersDealPermissionType;
        return deckEntity;
    }


    protected override EditorEntity GetEditorEntityToSpawn(Transform parent)
    {
        var entityToSpawn = UnityEngine.Object.Instantiate(TableEditorDataHolder.Instance.deckEditorPrefab, parent);

        //entityToSpawn.DeckDataFileName = deckDataFileName;

        entityToSpawn.DeckLayout = deckData;
        entityToSpawn.DeckMeta = deckInfo;

        entityToSpawn.OwnerShufflePermissionType = ownerShufflePermissionType;
        entityToSpawn.OwnerDealPermissionType = ownerDealPermissionType;
        entityToSpawn.OthersShufflePermissionType = othersShufflePermissionType;
        entityToSpawn.OthersDealPermissionType = othersDealPermissionType;
        return entityToSpawn;
    }
}
