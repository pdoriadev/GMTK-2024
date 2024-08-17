using UnityEngine;
using UnityEngine.UI;

public class ItemButtonHelper : MonoBehaviour
{
    [SerializeField] private GameObject m_RuntimeItemEditorObject = null;
    [SerializeField] private Items m_ItemCorrespondingToButton = Items.Invalid;
    private RuntimeItemEditor m_RuntimeItemEditor = null;

    public void Awake()
    {
        //Debug.Assert(m_ItemCreatorScript != null, "Missing reference");
        //m_ItemCreator = m_ItemCreatorScript as I_ItemCreator;
        //Debug.Assert(m_ItemCreator != null, "Failed to get item creator.");

        Debug.Assert(m_ItemCorrespondingToButton != Items.Invalid && m_ItemCorrespondingToButton != Items.NumberOfItems, "Missing reference");
        
        Debug.Assert(m_RuntimeItemEditorObject != null, "Missing reference");
        m_RuntimeItemEditor = m_RuntimeItemEditorObject.GetComponent<RuntimeItemEditor>();
        Debug.Assert(m_RuntimeItemEditor != null, "No item editor on game object");

        GetComponent<Button>().onClick.AddListener(OnItemButtonClick);
    }

    public void OnItemButtonClick()
    {
        m_RuntimeItemEditor.OnSelectItemForPlacement(m_ItemCorrespondingToButton);
    }
}
