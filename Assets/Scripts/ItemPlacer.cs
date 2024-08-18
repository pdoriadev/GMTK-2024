using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ItemPlacer : MonoBehaviour
{
    public static ItemPlacer Instance;
    
    [SerializeField] private Tilemap m_EditableSpaceTilemap = null;
    [SerializeField] private Tilemap m_Default;
    [SerializeField] private Tilemap m_Wall;
    
    
    private Dictionary<Vector3Int, I_ItemDestroyer> m_PlacedItems = new Dictionary<Vector3Int, I_ItemDestroyer>();
    
    private Grid m_Grid;
    private GameObject m_HoverObject;
    private Item m_Item;

    public GameObject Platform;
    public GameObject PlatformHover;
    public GameObject Spring;
    public GameObject SpringHover;

    private bool isMouseDown = false;
    
    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void SelectObject(Item item)
    {
        if (!m_Item)
        {
            Debug.Log("Selected!");
            m_HoverObject = Instantiate(item.HoverObject);
            m_Item = item;
            m_Item.gameObject.SetActive(false);
            isMouseDown = true;
        }
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

        if (m_HoverObject)
        {
            m_HoverObject.transform.position = mouseGridPos;
        }

        if (!m_EditableSpaceTilemap.HasTile(mouseGridPos))
        {
            return;
        }
        
        if (m_Item && !isMouseDown && Input.GetMouseButtonDown(0))
        {
            bool canPlace = true;
            foreach (var pos in m_Item.ExtraTiles)
            {
                if (!m_EditableSpaceTilemap.HasTile(mouseGridPos + m_Grid.WorldToCell(pos)))
                {
                    canPlace = false;
                }
            }

            if (m_Item.mustBeGrounded)
            {
                if (!(m_Default.HasTile(mouseGridPos + m_Grid.WorldToCell(new Vector3Int(0, -1, 0)))
                        || m_Wall.HasTile(mouseGridPos + m_Grid.WorldToCell(new Vector3Int(0, -1, 0)))))
                {
                    canPlace = false;
                }
            }

            if (canPlace)
            {
                Debug.Log("Placed!");
                m_Item.gameObject.SetActive(true);
                m_Item.transform.position = mouseGridPos;
                m_Item.Place();
                if (m_HoverObject)
                    GameObject.Destroy(m_HoverObject);
                m_Item = null;
                m_HoverObject = null;
            }
        }

        if (Input.GetMouseButtonDown(0))
            isMouseDown = false;
    }
}