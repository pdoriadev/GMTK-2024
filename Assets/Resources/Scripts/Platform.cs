using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour, I_ItemDestroyer
{
    [SerializeField] Collider2D collider = null;

    private void Awake()
    {
        collider = GetComponent<Collider2D>();
        Debug.Assert(collider != null, "collider is null");
    }

    public bool DestroyItem()
    {
        Object.Destroy(this);
        return true;
    }

    public void OnMouseDown()
    {
        

    }

}
