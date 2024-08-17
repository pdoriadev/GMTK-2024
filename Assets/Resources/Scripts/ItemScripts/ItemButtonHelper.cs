using UnityEngine;
using UnityEngine.UI;

public class ItemButtonHelper : MonoBehaviour
{
    [SerializeField] string m_CreatorResourcePath = "";
    [SerializeField] GameObject m_RuntimeItemEditorObject = null;
    private I_ItemCreator m_ItemCreator = null;
    private RuntimeItemEditor m_RuntimeItemEditor = null;

    public void Awake()
    {
        m_ItemCreator = Resources.Load(m_CreatorResourcePath) as I_ItemCreator;
        Debug.Assert(m_ItemCreator != null, "Failed to load resource.");

        Debug.Assert(m_RuntimeItemEditorObject != null, "Missing reference");

        m_RuntimeItemEditor = m_RuntimeItemEditorObject.GetComponent<RuntimeItemEditor>();
        Debug.Assert(m_RuntimeItemEditor != null, "No item editor on game object");

        GetComponent<Button>().onClick.AddListener(OnItemButtonClick);
    }

    public void OnItemButtonClick()
    {
        m_RuntimeItemEditor.OnSelectItemForPlacement(m_ItemCreator);
    }
}
