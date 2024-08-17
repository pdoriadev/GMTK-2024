using UnityEngine;
using UnityEngine.Tilemaps;

public class PlatformCreator : I_ItemCreator
{
    private string m_Resource = "Assets/Resources/Prefabs/Platform.prefab";
    public bool CreateItem(Vector3Int gridPos, Tilemap tilemap)
    {
        GameObject prefab = Resources.Load<GameObject>(m_Resource);
        return GameObject.Instantiate(prefab, gridPos, Quaternion.identity) != null;
    }
}
