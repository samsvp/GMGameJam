using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    private Animator anim;
    private AudioSource aS;
    private BoxCollider2D bc2D;

    public float speed = 40;

    private bool destroy = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        aS = GetComponent<AudioSource>();
        bc2D = GetComponent<BoxCollider2D>();
        GetComponent<Rigidbody2D>().velocity = new Vector3(speed, 0, 0);
        StartCoroutine(EndAnimation());
    }

    // Update is called once per frame
    void Update()
    {
            
    }


    private IEnumerator EndAnimation()
    {
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f);
        anim.enabled = false;
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if (destroy) return;
        if (col.CompareTag("Ignore")) return;
        col.SendMessage("TakeDamage", SendMessageOptions.DontRequireReceiver);
        StartCoroutine(Hit());
    }


    private IEnumerator Hit()
    {
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        aS.Play();
        bc2D.enabled = false;
        anim.enabled = true;
        anim.SetTrigger("hit");
        yield return null;
        StartCoroutine(CountDown());
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f || destroy);
        Destroy(gameObject);
    }


    private IEnumerator CountDown()
    {
        yield return new WaitForSeconds(0.9f);
        destroy = true;
    }


    void OnBecameInvisible()
    {
        // Destroy when offscreen
        Destroy(gameObject);
    }
}
