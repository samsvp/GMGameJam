using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PatrolEnemy : MonoBehaviour
{
    // Start is called before the first frame update

    private Rigidbody2D rb2D;
    private Transform transform;
    private Vector3 initialPosition;
    public float walkRange = 2;
    public float viewDistance = 5;

    public float speed;
    public bool facingRight = true;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        transform = GetComponent<Transform>();
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        walk();
        Debug.Log(searchPlayer());
    }
    bool searchPlayer()
    {
        RaycastHit2D hit2D;
        if (facingRight)
        {
            hit2D = Physics2D.Raycast(transform.position, Vector2.right, viewDistance);
        }
        else
        {
            hit2D = Physics2D.Raycast(transform.position, Vector2.left, viewDistance);
        }
        if (hit2D)
        {
            if(hit2D.transform.tag == "Player")
            {
                return true;
            }
        }
        return false;
    }
    void walk()
    {
        if ((initialPosition.x - walkRange) > transform.position.x)
        {
            facingRight = true;
        }
        if ((initialPosition.x + walkRange) < transform.position.x)
        {
            facingRight = false;
        }
        if (facingRight)
        {
            transform.Translate(1 * Time.deltaTime, 0, 0);
        }
        else
        {
            transform.Translate(-1 * Time.deltaTime, 0, 0);
        }
    }
}
