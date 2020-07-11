using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    private bool facingRight = true;
    private SpriteRenderer sRenderer;
    private Vector2 playerPos;
    private AudioSource aSource;
    private int monsterCount = 0;
    public Transform transform;
    public GameObject enemy; 
    public float triggerRadius;
    public int maxMonsters = 10;
    void Start()
    {
        transform = GetComponent<Transform>();
        sRenderer = GetComponent<SpriteRenderer>();
        aSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (monsterCount < maxMonsters)
        {
            search();
        }
        else
        {
            GetComponent<Spawn>().enabled = false;
        }
    }
    void search()
    {
        // check if player is near
        int RaysToShoot = 30;

        float angle = 0;
        for (int i = 0; i < RaysToShoot; i++)
        {
            float x = Mathf.Sin(angle);
            float y = Mathf.Cos(angle);
            angle += 2 * Mathf.PI / RaysToShoot;

            Vector2 dir = new Vector2(x, y);
            RaycastHit2D hit;
            hit = Physics2D.Raycast(transform.position, dir, triggerRadius);
            if (hit)
            {
                if (hit.transform.tag == "Player")
                {
                    spawn_enemy();
                    monsterCount++;
                    return;
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, triggerRadius);
    }
   void spawn_enemy()
    {
        GameObject.Instantiate(enemy, transform.position, transform.rotation);
    }
}
