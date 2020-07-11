using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    private Animator anim;
    
    public float speed = 40;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
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
        col.SendMessage("TakeDamage", SendMessageOptions.DontRequireReceiver);
        Destroy(gameObject);
    }


    void OnBecameInvisible()
    {
        // Destroy when offscreen
        Destroy(gameObject);
    }
}
