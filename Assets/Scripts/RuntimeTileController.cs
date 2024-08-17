using UnityEngine;
using UnityEngine.Tilemaps;

public class RuntimeTileController : MonoBehaviour
{
    [SerializeField] private Tilemap levelMap = null;
    [SerializeField] private Tilemap hoverMap = null;
    [SerializeField] private RuleTile levelTile = null;
    [SerializeField] private Tile hoverTile = null;

    private Grid grid;
    private Vector3Int m_PreviousMousePos = new Vector3Int();

    // Start is called before the first frame update
    void Start()
    {
        grid = gameObject.GetComponent<Grid>();
    }

    // Update is called once per frame
    void Update()
    {
        // check for click
        // TODO: check for click-up to see if holding and dragging sprites. 
        // check if click is in grid. 

        // Mouse over -> highlight tile
        Vector3Int mousePos = GetMousePosition();
        if (!Input.GetMouseButton(0) && !Input.GetMouseButton(1) && !mousePos.Equals(m_PreviousMousePos))
        {
            return;
        }

        if (!mousePos.Equals(m_PreviousMousePos))
        {
            hoverMap.SetTile(m_PreviousMousePos, null); // Remove old hoverTile
            hoverMap.SetTile(mousePos, hoverTile);
            m_PreviousMousePos = mousePos;
        }

        // Left mouse click -> add level tile
        if (Input.GetMouseButton(0))
        {
            levelMap.SetTile(mousePos, levelTile);
        }

        // Right mouse click -> remove level tile
        if (Input.GetMouseButton(1))
        {
            levelMap.SetTile(mousePos, null);
        }
    }

    Vector3Int GetMousePosition()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return grid.WorldToCell(mouseWorldPos);
    }
}