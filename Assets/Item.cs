using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] public List<Vector2> ExtraTiles = new List<Vector2>();
    public GameObject HoverObject;

    private void OnMouseDown()
    {
        ItemPlacer.Instance.SelectObject(this);
    }
}
