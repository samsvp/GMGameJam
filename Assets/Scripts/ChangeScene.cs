using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
            System.IO.File.WriteAllText(GameManager.path, nextScene.ToString());
            SceneManager.LoadScene(nextScene);
        }
            
    }
}
