using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PatrolEnemy : MonoBehaviour
{
    // Start is called before the first frame update

    private Rigidbody2D rb2D;
    private Transform transform;
    public float patrolRange;
    public float speed;
    private Vector3 initialPosition;
    public bool facingLeft = true;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        transform = GetComponent<Transform>();
        initialPosition = transform.position;
        Debug.Log(initialPosition.x);
    }

    // Update is called once per frame
    void Update()
    {
        if ((initialPosition.x - patrolRange) > transform.position.x)
        {
            facingLeft = true;
        }
        if ((initialPosition.x + patrolRange) < transform.position.x)
        {
            facingLeft = false;
        }
        if (facingLeft)
        {
            transform.Translate(1 * Time.deltaTime, 0, 0);
        }
        else
        {
            transform.Translate(-1 * Time.deltaTime, 0, 0);
        }
    }
}
