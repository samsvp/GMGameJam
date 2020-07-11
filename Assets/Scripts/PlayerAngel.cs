using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAngel : MonoBehaviour
{
    public static PlayerAngel instance;

    [SerializeField]
    private float speed;

    private SpriteRenderer spriteRenderer;

    private Rigidbody2D rb2D;
    private BoxCollider2D bc2D;

    // Movement variables
    private float x, y;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        bc2D = GetComponent<BoxCollider2D>();   
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Rotate();
    }

    private void FixedUpdate()
    {
        rb2D.velocity = new Vector2(x, y);
    }


    private void Move()
    {
        int x = (int)Input.GetAxisRaw("Horizontal");
        int y = (int)Input.GetAxisRaw("Vertical");
        
        Move(x, y);
    }

    private void Move(int _x, int _y)
    {
        x = _x * speed;
        y = _y * speed;
    }


    private void Rotate()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RotateTowards(mousePos);
    }


    private void RotateTowards(Vector3 position)
    {
        transform.rotation = Quaternion.LookRotation(Vector3.forward, position - transform.position);
    }
}
