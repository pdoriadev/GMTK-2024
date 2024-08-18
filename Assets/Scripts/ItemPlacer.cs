using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ItemPlacer : MonoBehaviour
{
    public static ItemPlacer Instance;
    
    [SerializeField] private Tilemap m_EditableSpaceTilemap = null;

    private Dictionary<Vector3Int, I_ItemDestroyer> m_PlacedItems = new Dictionary<Vector3Int, I_ItemDestroyer>();
    
    private Grid m_Grid;
    private GameObject m_HoverObject;
    private GameObject m_PlaceObject;

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
        Debug.Log("Selected!");
        m_HoverObject = Instantiate(item.HoverObject);
        m_PlaceObject = item.gameObject;
        m_PlaceObject.SetActive(false);
        isMouseDown = true;
    }
    
    public void SelectPlatform()
    {
        if (m_HoverObject) 
            GameObject.Destroy(m_HoverObject);
        m_HoverObject = Instantiate(PlatformHover);
        m_PlaceObject = Platform;
        isMouseDown = true;
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

        if (m_PlaceObject && !isMouseDown && Input.GetMouseButtonDown(0))
        {
            Debug.Log("Placed!");
            m_PlaceObject.SetActive(true);
            m_PlaceObject.transform.position = mouseGridPos;
            if (m_HoverObject)
                GameObject.Destroy(m_HoverObject);
            m_PlaceObject = null;
            m_HoverObject = null;
        }

        if (Input.GetMouseButtonDown(0))
            isMouseDown = false;
    }
}