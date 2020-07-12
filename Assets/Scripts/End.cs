using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class End : MonoBehaviour
{

    private Animator anim;

    [SerializeField]
    private GameObject canvas;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.92f)
        {
            anim.enabled = false;
            StartCoroutine(CanvasShow());
        }
    }

    public void LoadMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }


    private IEnumerator CanvasShow()
    {
        yield return new WaitForSeconds(1);
        canvas.SetActive(true);
    }
}
