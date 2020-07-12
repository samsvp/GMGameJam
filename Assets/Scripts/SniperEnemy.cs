using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperEnemy : MonoBehaviour
{
    private int HP = 0;
    private bool facingRight = true;
    private SpriteRenderer sRenderer;
    private float shootTime = 0;
    private Vector2 playerPos;
    private AudioSource aSource;
    public Transform transform;
    public Animator anim;
    public GameObject bullet;
    public float shootInterval = 10;
    public float searchRadius;
    void Start()
    {
        transform = GetComponent<Transform>();
        sRenderer = GetComponent<SpriteRenderer>();
        aSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        shootTime += Time.deltaTime;

        if (shootTime > shootInterval)
        {
            search();
            shootTime = 0;
        }
        else
        {
            anim.SetBool("shoot",false);
        }

        if (transform.position.x < playerPos.x)
        {
            sRenderer.flipX = true;
        }
        else
        {
            sRenderer.flipX = false;
        }
    }
    void search()
    {
        int RaysToShoot = 30;

        float angle = 0;
        for (int i = 0; i < RaysToShoot; i++)
        {
            float x = Mathf.Sin(angle);
            float y = Mathf.Cos(angle);
            angle += 2 * Mathf.PI / RaysToShoot;

            Vector2 dir = new Vector2(x, y);
            RaycastHit2D hit;
            hit = Physics2D.Raycast(transform.position, dir, searchRadius);
            if (hit)
            {
                if (hit.transform.tag == "Player")
                {
                    playerPos = hit.point;
                    var obj = GameObject.Instantiate(bullet, transform.position, transform.rotation);
                    obj.GetComponent<SniperBullet>().shoot(new Vector3(hit.point.x, hit.point.y + 2f, 0));
                    anim.SetBool("shoot",true);
                    break;
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, searchRadius);
    }
    private void TakeDamage()
    {
        if (--HP < 0) Destroy(gameObject);
    }
}
