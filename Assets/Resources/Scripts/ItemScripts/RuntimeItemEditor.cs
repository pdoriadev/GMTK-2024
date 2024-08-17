using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RuntimeItemEditor : MonoBehaviour
{
    [SerializeField] private Tilemap EditableSpaceTilemap = null;
    
    private I_ItemCreator m_SelectedItemForPlacement = null;
    private Dictionary<Vector3Int, I_ItemDestroyer> m_PlacedItems = new Dictionary<Vector3Int, I_ItemDestroyer>();
    private Grid m_Grid;
    
    void Start()
    {
        Debug.Assert(EditableSpaceTilemap != null, "Missing reference to editable space tilemap.");
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

        if (!EditableSpaceTilemap.HasTile(mouseGridPos))
        {
            return;
        }

        // Attempt to Add Item
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
        {
            m_SelectedItemForPlacement.CreateItem(mouseGridPos, EditableSpaceTilemap);
            return;
        }

        // Remove Item
        if (Input.GetMouseButtonDown(1))
        {
            I_ItemDestroyer item = m_PlacedItems[mouseGridPos];
            if (item == null)
            {
                return;
            }

            m_PlacedItems.Remove(mouseGridPos);
            item.DestroyItem();
        }
    }

    public void OnSelectItemForPlacement(I_ItemCreator item)
    {
        m_SelectedItemForPlacement = item;
    }
}