using UnityEngine;
using UnityEngine.Tilemaps;

public class PlatformCreator : I_ItemCreator
{
    private string m_ResourcePath = "Prefabs/Platform.prefab";
    
    public I_ItemDestroyer CreateItem(Vector3Int gridPos, Tilemap tilemap)
    {
        GameObject prefab = Resources.Load<GameObject>(m_ResourcePath);
        return Object.Instantiate(prefab, gridPos, Quaternion.identity).GetComponent<I_ItemDestroyer>();
    }
}
