using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteOutline : MonoBehaviour
{
    public SpriteRenderer sprite;

    void Start()
    {
        sprite.enabled = false;
    }
    
    private void OnMouseEnter()
    {
        sprite.enabled = true;
    }
    
    private void OnMouseExit()
    {
        sprite.enabled = false;
    }
}
