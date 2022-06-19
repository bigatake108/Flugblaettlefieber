using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowLeaflet : MonoBehaviour
{
    Canvas flyer;

    private void Start()
    {
        flyer = gameObject.GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && flyer.enabled == false) {
            flyer.enabled = true;
        }
        else if (Input.GetKeyDown(KeyCode.F) && flyer.enabled == true) {
            flyer.enabled = false;
        }
        
    }
}