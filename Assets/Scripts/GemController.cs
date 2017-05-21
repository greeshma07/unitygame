using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemController : MonoBehaviour {

    private SpriteRenderer gemRenderer;
    private Sprite[] gemSprite;
    private int i = 0;

    // Use this for initialization
    void Start()
    {
        GameObject gameObj = GameObject.Find("Gem");
        if (null != gameObj)
        {
            gemRenderer = this.GetComponent<SpriteRenderer>();
            gemSprite = Resources.LoadAll<Sprite>("gems");
            InvokeRepeating("animateObj", 0f, 0.15f);
        }            
    }

    void animateObj()
    {
        if (i < gemSprite.Length)
        {
            gemRenderer.sprite = gemSprite[i];
            i++;
        }
        else
        {
            i = 0;
        }
    }
}
