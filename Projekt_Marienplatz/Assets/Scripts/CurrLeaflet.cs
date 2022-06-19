using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrLeaflet : MonoBehaviour
{
    public RawImage placeholder;
    public Texture test;
    // Start is called before the first frame update
    private void Start()
    {
        // placeholder.texture = Leaflets.leafletArray[currentLeafletIndex].spriteName;
        placeholder.texture = test;
    }
}
