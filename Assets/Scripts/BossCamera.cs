using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCamera : MonoBehaviour
{

    [SerializeField]
    private float radius = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var pos = new Vector2(transform.position.x + radius * Mathf.Sin(Time.time),
                              transform.position.y + radius * Mathf.Cos(Time.time));
        transform.position = new Vector3(pos.x, pos.y, transform.position.z);
    }
}
