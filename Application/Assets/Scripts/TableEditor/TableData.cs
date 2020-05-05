using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class TableData {
    //[SerializeField]
    [JsonProperty]
    private List<EntityData> entities = new List<EntityData>();


    public void Add(EntityData editorEntity)
    {
        entities.Add(editorEntity);
    }

    internal void Clear()
    {
        entities.Clear();
    }

    public void CloneZone(HorizontalZoneData data) {
        entities.Add(data);
    }

    public void SpawnEntities(GameObject parent, Player owner)
    {
        foreach (var entity in entities)
        {
            entity.SpawnEntity(parent, owner);
        }
    }

    public void SpawnEditorEntities(GameObject parent)
    {
        foreach (var entity in entities)
        {
            entity.SpawnEditorEntity(parent);
        }
    }
}
