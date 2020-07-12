using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperBullet : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator anim;
    private AudioSource aS;
    private CircleCollider2D cc2D;

    private Vector2 heading;
    private Rigidbody2D rb;
    public Transform playerTransform;

    public float speed = 40;

    public void shoot(Vector3 target)
    {
        aS = GetComponent<AudioSource>();
        cc2D = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        Vector3 heading = target - transform.position;
        heading *= speed;
        float distance = heading.magnitude;
        Vector3 direction = heading / distance; // This is now the normalized direction.

        if (transform.position.x > playerTransform.position.x)
        {
            rb.velocity = new Vector3(heading.x, heading.y - speed , 0);
        }
        else
        {
            rb.velocity = new Vector3(-heading.x, heading.y - speed , 0);
        }
    }

    private IEnumerator Hit()
    {
        aS.Play();
        cc2D.enabled = false;
        anim.enabled = true;
        rb.velocity = Vector3.zero;
        anim.SetTrigger("hit");
        yield return null;
        StartCoroutine(CountDown());
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f);
        Destroy(gameObject);
    }
    private IEnumerator CountDown()
    {
        yield return new WaitForSeconds(1f);
    }
    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Ignore")) return;
        if (col.CompareTag("Bomber")) return;
        StartCoroutine(Hit());
        col.SendMessage("TakeDamage", SendMessageOptions.DontRequireReceiver);
        //Destroy(gameObject);
    }

    void OnBecameInvisible()
    {
        // Destroy when offscreen
        Destroy(gameObject);
    }

}
