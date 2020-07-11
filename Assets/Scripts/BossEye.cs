using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEye : MonoBehaviour
{

    [SerializeField]
    private int HP = 50;
    private int maxHP;

    [SerializeField]
    private float speed = 5;

    private SpriteRenderer sR;

    // Lighting
    [SerializeField]
    private Sprite reticule;
    [SerializeField]
    private GameObject[] lightingSpots;
    [SerializeField]
    private GameObject[] lightingCircles;

    [SerializeField]
    private GameObject bullet;

    private Vector3 initialPosition;

    // Start is called before the first frame update
    void Start()
    {
        sR = GetComponent<SpriteRenderer>();

        initialPosition = transform.position;
        maxHP = HP;
        StartCoroutine(StateMachine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private IEnumerator StateMachine()
    {
        while (true)
        {
            float m = Random.Range(0.0f, 1.0f);
            if (m < 0.33f) yield return StartCoroutine(Shoot());
            else if (m < 0.66f) yield return StartCoroutine(LightingAttack());
            else yield return StartCoroutine(DashAttack());
            yield return StartCoroutine(GoToPosition(initialPosition, speed));
            yield return null;
        }
        
    }


    private IEnumerator Shoot()
    {
        int numberOfBullets = Random.Range(4, 7);

        float m = Random.Range(0.0f, 1.0f);
        float x = m > 0.5f ? 11.5f : -11.5f;
        float y = -2.7f;

        Vector3 target = new Vector3(x, y);
        yield return StartCoroutine(GoToPosition(target, speed));

        for (int i = 0; i < numberOfBullets; i++)
        {
            var offset = new Vector3(1, Random.Range(-0.4f, 0.4f));
            if (x > 0)
            {
                GameObject mBullet = Instantiate(bullet, transform.position - 3f * offset, Quaternion.identity);
                mBullet.GetComponent<Bullet>().speed *= -1;
            }
            else Instantiate(bullet, transform.position + 2 * offset, Quaternion.identity);
            yield return new WaitForSeconds(1f);
        }
        
        yield return null;
    }


    private int[] PickLightingSpots()
    {
        int numberOfSpots = Random.Range(5, 9);
        var pickedSpots = new int[numberOfSpots];

        List<GameObject> mLightingSpots = new List<GameObject>(lightingSpots);
        float y = -3.55f;
        for (int i = 0; i < numberOfSpots; i++)
        {
            pickedSpots[i] = mLightingSpots[Random.Range(0, mLightingSpots.Count)].transform.GetSiblingIndex();
        }

        return pickedSpots;
    }


    private IEnumerator LightingAttack()
    {
        var indexes = PickLightingSpots();
        List<GameObject> gOs = new List<GameObject>();

        foreach (var index in indexes)
        {
            StartCoroutine(SpawnLighting(index));
            yield return new WaitForSeconds(0.3f);
        }     

        yield return null;
    }


    private IEnumerator SpawnLighting(int index)
    {
        print(index);
        lightingCircles[index].SetActive(true);
        for (int i = 0; i < 360; i++)
        {
            lightingCircles[index].transform.eulerAngles = new Vector3(0, 0, i);
            yield return new WaitForSeconds(0.0005f);
        }
        lightingCircles[index].SetActive(false);


        lightingSpots[index].SetActive(true);
        yield return new WaitUntil(() =>
            lightingSpots[index].GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f);
        lightingSpots[index].SetActive(false);

    }


    private IEnumerator GoToPosition(Vector3 target, float speed)
    {
        while (Vector3.Distance(transform.position, target) > 0.05f)
        {
            transform.position = Vector3.Lerp(transform.position, target, speed * Time.deltaTime);
            yield return null;
        }
    }


    private IEnumerator DashAttack()
    {
        float m = Random.Range(0.0f, 1.0f);
        float x = m > 0.5f ? 11.5f : -11.5f;
        float y = -2.7f;

        Vector3 target = new Vector3(x, y);
        yield return StartCoroutine(GoToPosition(target, speed));
        yield return new WaitForSeconds(0.5f);

        target = new Vector3(-x, y);
        yield return StartCoroutine(GoToPosition(target, speed / 2));
        yield return new WaitForSeconds(0.5f);

        target = initialPosition;
        yield return StartCoroutine(GoToPosition(target, speed));

    }


    private void TakeDamage()
    {
        print(HP);
        float healthPercentage = HP / (float)maxHP;
        sR.color = new Color(1, healthPercentage, healthPercentage);
        if (--HP < 0) Destroy(gameObject);
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        col.SendMessage("TakeDamage", SendMessageOptions.DontRequireReceiver);
    }
}
