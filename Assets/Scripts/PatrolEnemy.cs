using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public enum EnemyStates
{

    playerFound
}
public class PatrolEnemy : MonoBehaviour
{
    private Rigidbody2D rb2D;
    private Transform transform;
    private Vector3 initialPosition;
    private EnemyStates currentState;
    private Vector2 playerPosition;
    public float walkRange = 2;
    public float viewDistance = 5;
    public Vector2 walkGoal;

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
        /*
        switch (currentState)
        {
            case EnemyStates.playerFound:
                chase();
                break;
            default:
                patrol();
                if (searchPlayer())
                {
                    currentState = EnemyStates.playerFound;
                }
                break;
        }
        */
        walk(walkGoal);
    }
    void walk(Vector2 goal)
    {
        if (transform.position.x <= Mathf.Min(goal.x - 0.1f,goal.x + 0.1f) || transform.position.x >= Mathf.Max(goal.x - 0.1f,goal.x + 0.1f))
        {
            if (transform.position.x < goal.x)
            {
                facingRight = true;
            }
            if (transform.position.x > goal.x)
            {
                facingRight = false;
            }

            if (facingRight)
            {
                transform.Translate(1 * Time.deltaTime * speed, 0, 0);
            }
            else
            {
                transform.Translate(-1 * Time.deltaTime * speed, 0, 0);
            }
        }
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
            if (hit2D.transform.tag == "Player")
            {
                playerPosition = hit2D.point;
                return true;
            }
        }
        return false;
    }
    void patrol()
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
            transform.Translate(1 * Time.deltaTime * speed, 0, 0);
        }
        else
        {
            transform.Translate(-1 * Time.deltaTime * speed, 0, 0);
        }
    }
}
