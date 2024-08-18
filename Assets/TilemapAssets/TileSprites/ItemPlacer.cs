using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ItemPlacer : MonoBehaviour
{
    [SerializeField] private Tilemap m_EditableSpaceTilemap = null;

    private Dictionary<Vector3Int, I_ItemDestroyer> m_PlacedItems = new Dictionary<Vector3Int, I_ItemDestroyer>();
    
    private Grid m_Grid;
    private GameObject m_HoverObject;
    private GameObject m_PlaceObject;

    public GameObject Platform;
    public GameObject PlatformHover;
    public GameObject Spring;
    public GameObject SpringHover;

    public void SelectPlatform()
    {
        if (m_HoverObject) 
            GameObject.Destroy(m_HoverObject);
        m_HoverObject = Instantiate(PlatformHover);
        m_PlaceObject = Platform;
    }
    
    public void SelectSpring()
    {
        if (m_HoverObject) 
            GameObject.Destroy(m_HoverObject);
        m_HoverObject = Instantiate(SpringHover);
        m_PlaceObject = Spring;
    }
    
    void Start()
    {
        Debug.Assert(m_EditableSpaceTilemap != null, "Missing reference to editable space tilemap.");
        m_Grid = gameObject.GetComponent<Grid>();
        Debug.Assert(m_Grid != null, "Missing reference.");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3Int mouseGridPos;
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseGridPos = m_Grid.WorldToCell(mouseWorldPos);
        }

        if (!m_EditableSpaceTilemap.HasTile(mouseGridPos))
        {
            return;
        }

        if (m_HoverObject)
        {
            m_HoverObject.transform.position = mouseGridPos;
        }
        
        // Attempt to Add Item
        if (Input.GetMouseButtonDown(0))
        {
            if (m_PlaceObject)
            {
                var instance = Instantiate(m_PlaceObject);
                instance.transform.position = mouseGridPos;
                GameObject.Destroy(m_HoverObject);
                m_HoverObject = null;
                m_PlaceObject = null;
            }

            // if (m_PlacedItems.ContainsKey(mouseGridPos))
            // {
            //     Debug.Log("An item is already placed here.");
            //     return;
            // }
            return;
        }

        // Remove Item
        // if (Input.GetMouseButtonDown(1))
        // {
        //     I_ItemDestroyer destroyer = m_PlacedItems[mouseGridPos];
        //     if (destroyer == null)
        //     {
        //         return;
        //     }
        //
        //     m_PlacedItems.Remove(mouseGridPos);
        //     destroyer.DestroyItem();
        // }
    }
}