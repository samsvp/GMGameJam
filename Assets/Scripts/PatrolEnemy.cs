using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public enum EnemyStates
{

    chasing,
    searching
}
public class PatrolEnemy : MonoBehaviour
{
    private Rigidbody2D rb2D;
    private Transform transform;
    private Vector3 initialPosition;
    private EnemyStates currentState = EnemyStates.searching;
    private Animator anim;
    private SpriteRenderer sRenderer;
    private AudioSource aSource;
    private float soundTime;
    private float HP = 0; 
    public Transform playerTransform;
    public float audioInterval;
    public float walkRange = 2;
    public float viewDistance = 1;
    public Vector2 walkGoal;

    public AudioClip[] sounds;
 
    public float searchSpeed, chaseSpeed;
    public bool facingRight = true;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        transform = GetComponent<Transform>();
        initialPosition = transform.position;
        anim = GetComponent<Animator>();
        sRenderer = GetComponent<SpriteRenderer>();
        aSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if(facingRight)
        {
            sRenderer.flipX = true;
        }
        else
        {
            sRenderer.flipX = false;
        }
     
        soundTime += Time.deltaTime;
        switch (currentState)
        {
            case EnemyStates.chasing:
                chase(playerTransform.position);
                if(soundTime > audioInterval)
                {
                soundTime = 0;
                aSource.clip = sounds[Random.Range(0, sounds.Length - 1)];
                aSource.Play();
                }
                if (Mathf.Abs(playerTransform.position.y - transform.position.y) > 5)
                {
                    currentState = EnemyStates.searching;
                    initialPosition = transform.position;
                }

                break;
            case EnemyStates.searching:

                patrol();
                if (searchPlayer())
                {
                    currentState = EnemyStates.chasing;
                }
                break;
            default: 
                currentState = EnemyStates.searching;
                break;
        }
    }
    void chase(Vector2 goal)
    {
        anim.SetBool("run", true);
        anim.SetFloat("runSpeed", 1.0f);
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
                transform.Translate(1 * Time.deltaTime * chaseSpeed, 0, 0);
            }
            else
            {
                transform.Translate(-1 * Time.deltaTime * chaseSpeed, 0, 0);
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
                return true;
            }
        }
        return false;
    }
    void patrol()
    {
        anim.SetBool("run", true);
        anim.SetFloat("runSpeed", 0.5f);
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
            transform.Translate(1 * Time.deltaTime * searchSpeed, 0, 0);
        }
        else
        {
            transform.Translate(-1 * Time.deltaTime * searchSpeed, 0, 0);
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.tag == "Player")
        {
            col.SendMessage("TakeDamage", SendMessageOptions.DontRequireReceiver);
        }
    }
    private void TakeDamage()
    {
        if (--HP < 0) Destroy(gameObject);
    }

}
