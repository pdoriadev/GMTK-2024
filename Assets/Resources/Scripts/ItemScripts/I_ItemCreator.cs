using UnityEngine;
using UnityEngine.Tilemaps;
public interface I_ItemCreator 
{
    public bool CreateItem(Vector3Int gridPos, Tilemap tilemap);
}
