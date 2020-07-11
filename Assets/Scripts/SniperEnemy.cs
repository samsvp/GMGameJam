using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperEnemy : MonoBehaviour
{
    private int HP = 1;
    private bool facingRight = true;
    private SpriteRenderer sRenderer;
    private Vector2 playerPos;
    private AudioSource aSource;
    public Transform transform;
    public GameObject bullet; 
    public float searchRadius;
    void Start()
    {
        transform = GetComponent<Transform>();
        sRenderer = GetComponent<SpriteRenderer>();
        aSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (facingRight)
        {
            sRenderer.flipX = true;
        }
        else
        {
            sRenderer.flipX = false;
        }
        search();
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
                    shoot(hit.transform.position);
                    return;
                }
            }
        }
    }
    void shoot(Vector3 target)
    {
        Vector3 heading = target - transform.position;
        GameObject.Instantiate(bullet, transform.position, transform.rotation);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, searchRadius);
    }
    private void shoot(Vector2 target)
    {

    }
    private void TakeDamage()
    {
        if (--HP < 0) Destroy(gameObject);
    }
}
