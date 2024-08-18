using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spring : MonoBehaviour
{
    public float force = 10.0f;
    
    void  OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("Ooble"))
        {
            var ooble = other.transform.GetComponent<Ooble2>();
            ooble.Jump(force);
        }
    }
}
