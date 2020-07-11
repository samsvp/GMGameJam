using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{


    [SerializeField]
    private float speed = 5;

    private Animator anim;
    private BoxCollider2D[] bcs2D;

    private bool isDead = false;
    private bool canDestroy = false; 

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        bcs2D = GetComponents<BoxCollider2D>();
    }
    

    private IEnumerator Move()
    {
        while(true)
        {
            if (isDead) yield break;
            var target = new Vector3(transform.position.x - 1, transform.position.y + 0.5f * Mathf.Sin(speed * 2 * Time.time));
            transform.position = Vector3.Lerp(transform.position, target, speed * Time.deltaTime);
            yield return null;
        }
    }


    private void TakeDamage()
    {
        StartCoroutine(Death());
    }


    private IEnumerator Death()
    {
        isDead = true;
        foreach (var bc2D in bcs2D) bc2D.enabled = false;
        
        anim.SetTrigger("death");
        yield return null;
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f);
        Destroy(gameObject);

    }

    void OnBecameVisible()
    {
        StartCoroutine(Move());
    }

}
