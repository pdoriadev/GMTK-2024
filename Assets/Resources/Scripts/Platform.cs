using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour, I_ItemDestroyer
{
    public bool DestroyItem()
    {
        Object.Destroy(this);
        return true;
    }

}
