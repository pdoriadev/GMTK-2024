// using System.Collections.Generic;
// using Unity.VisualScripting;
// using UnityEngine;
// using UnityEngine.Tilemaps;
//
// public enum Items
// { 
//     Invalid = -1,
//     Platform,
//     NumberOfItems
//
// }
//
//
// public class RuntimeItemEditor : MonoBehaviour
// {
//     [SerializeField] private Tilemap m_EditableSpaceTilemap = null;
//     
//     private I_ItemCreator m_SelectedItemForPlacement = null;
//     //private Items m_SelectedItem = Items.Invalid;
//     private Dictionary<Vector3Int, I_ItemDestroyer> m_PlacedItems = new Dictionary<Vector3Int, I_ItemDestroyer>();
//     private Grid m_Grid;
//     
//     void Start()
//     {
//         Debug.Assert(m_EditableSpaceTilemap != null, "Missing reference to editable space tilemap.");
//         m_Grid = gameObject.GetComponent<Grid>();
//         Debug.Assert(m_Grid != null, "Missing reference.");
//     }
//
//     // Update is called once per frame
//     void Update()
//     {
//         Vector3Int mouseGridPos;
//         {
//             Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//             mouseGridPos = m_Grid.WorldToCell(mouseWorldPos);
//         }
//
//         if (!m_EditableSpaceTilemap.HasTile(mouseGridPos))
//         {
//             return;
//         }
//
//         // Attempt to Add Item
//         if (Input.GetMouseButtonDown(0))
//         {
//             if (m_PlacedItems.ContainsKey(mouseGridPos))
//             {
//                 Debug.Log("An item is already placed here.");
//                 return;
//             }
//
//             I_ItemDestroyer destroyer = null;
//             destroyer = m_SelectedItemForPlacement.CreateItem(mouseGridPos);
//             Debug.Assert(destroyer != null, "Destroyer is null");
//
//             m_PlacedItems.Add(mouseGridPos, destroyer);
//
//             return;
//         }
//
//         // Remove Item
//         if (Input.GetMouseButtonDown(1))
//         {
//             I_ItemDestroyer destroyer = m_PlacedItems[mouseGridPos];
//             if (destroyer == null)
//             {
//                 return;
//             }
//
//             m_PlacedItems.Remove(mouseGridPos);
//             destroyer.DestroyItem();
//         }
//     }
//
//     public void OnSelectItemForPlacement(Items item)
//     {
//         switch (item)
//         {
//             case Items.Invalid:
//                 return;
//             case Items.NumberOfItems:
//                 Debug.Assert(false, "This should never fire.");
//                 return;
//             case Items.Platform:
//                 m_SelectedItemForPlacement = new PlatformCreator();
//                 break;
//         }
//     }
// }