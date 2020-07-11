using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
            col.SendMessage("TakeDamage", SendMessageOptions.DontRequireReceiver);
    }
}
