using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinAnimate : MonoBehaviour {

    private SpriteRenderer coinRenderer;
    private Sprite[] coinSprite;
    private int i = 0;

    // Use this for initialization
    void Start()
    {
        coinRenderer = this.GetComponent<SpriteRenderer>();
        coinSprite = Resources.LoadAll<Sprite>("coin_sheet");
        InvokeRepeating("animateObj", 0f, 0.15f);
    }

    void animateObj()
    {
        if (i < coinSprite.Length)
        {
            coinRenderer.sprite = coinSprite[i];
            i++;
        }
        else
        {
            i = 0;
        }
    }
}
