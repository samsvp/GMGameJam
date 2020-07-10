using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]
    protected float speed;

    protected Animator anim;
    protected SpriteRenderer spriteRenderer;

    protected Rigidbody2D rb2D;
    protected BoxCollider2D bc2D;

    // Movement variables
    protected float x, y;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        bc2D = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }


    protected virtual void FixedUpdate()
    {
        rb2D.velocity = new Vector2(x, y);
    }


    protected virtual void Move(int _x, int _y)
    {
        x = _x * speed;
        y = _y * speed;
    }
}
