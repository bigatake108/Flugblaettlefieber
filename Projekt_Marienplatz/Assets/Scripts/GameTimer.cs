using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameTimer : MonoBehaviour
{
    [SerializeField]
    private float delay = 10f;

    [SerializeField]
    private string sceneToLoad;
    private float timeElapsed;

    
    // Update is called once per frame
    private void Update()
    {
        timeElapsed += Time.deltaTime;

        // Temporary:
        if (timeElapsed > delay) {
            SceneManager.LoadScene("EndScreen1");
        }


        /*
        if (timeElapsed > delay && 1000 > playerScript.score) 
        {
            SceneManager.LoadScene("EndScreen1");
        } 
        else if (timeElapsed > delay && playerScript.score > 1000 && 2000 > playerScript.score) 
        {
            SceneManager.LoadScene("EndScreen2");
        }
        else if (timeElapsed > delay && playerScript.score > 2000) 
        {
            SceneManager.LoadScene("EndScreen3");
        }
        */

    }
}
