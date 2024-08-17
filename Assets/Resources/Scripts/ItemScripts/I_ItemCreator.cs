using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;


public interface I_ItemCreator 
{
    public I_ItemDestroyer CreateItem(Vector3Int gridPos, Tilemap tilemap = null);
}
