using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NurseryTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Bebe")
        {
        }
    }
}
