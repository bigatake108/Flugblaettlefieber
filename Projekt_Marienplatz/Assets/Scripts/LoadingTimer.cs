using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingTimer : MonoBehaviour
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

        if (timeElapsed > delay) {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
