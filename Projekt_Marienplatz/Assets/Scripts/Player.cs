using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rbody;
    float speed;

    public bool isTalkingToWalker = false;
    public GameObject talkingWaker;

    GameObject leafletsObject;
    Leaflets leafletsScript;

    public static int score = 0;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        speed = 50f;
        rbody = gameObject.GetComponent<Rigidbody2D>();

        leafletsObject = GameObject.Find("Leaflets");
        leafletsScript = leafletsObject.GetComponent<Leaflets>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.Space))
        {
            interactWithWalker();
        }
    }

    private void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector2 translationVector = new Vector2(horizontalInput * speed, verticalInput * speed) * Time.fixedDeltaTime;
        
        rbody.MovePosition(rbody.position + translationVector);
       
    }

    void interactWithWalker()
    {
        Leaflets.Leaflet currentLeaflet = leafletsScript.leafletArray[leafletsScript.currentLeafletIndex];

        if (talkingWaker != null)
        {
            Walker walkerScript = talkingWaker.GetComponent<Walker>();

            walkerScript.respondToPlayer(currentLeaflet);

            // TODO wenn reward gleich null -> Zeitmalus
            // score += walkerScript.rewardLeaflet(currentLeaflet);
        }
    }

    public void applyReward(int reward)
    {
        score += reward;
    }
}