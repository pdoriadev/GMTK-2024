using UnityEngine;
using UnityEngine.Tilemaps;

public class PlatformCreator : I_ItemCreator
{
    private string m_ResourcePath = "Prefabs/Platform";
    private string m_HoverPath = "Prefabs/PlatformHover";
    
    public I_ItemDestroyer CreateItem(Vector3Int gridPos, Tilemap tilemap)
    {
        GameObject prefab = Resources.Load<GameObject>(m_ResourcePath);
        I_ItemDestroyer destroyer = Object.Instantiate(prefab, gridPos, Quaternion.identity).GetComponent<I_ItemDestroyer>();
        return destroyer;
    }
}
