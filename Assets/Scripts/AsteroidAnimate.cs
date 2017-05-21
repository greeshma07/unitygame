using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AsteroidAnimate : MonoBehaviour {

    private SpriteRenderer asteroidRenderer;
    private Sprite[] astriodSprite;
    private int i = 0, low = 0, high;
    private bool visible = false;
    private float speed;

    private void Awake()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene == "scene1")
        {
            speed = 0.5f;
            high = 15;
        }
        else
        {
            speed = 0.9f;
            high = 12;
        }
    }

    // Use this for initialization
    IEnumerator Start()
    {
        asteroidRenderer = this.GetComponent<SpriteRenderer>();        
        astriodSprite = Resources.LoadAll<Sprite>("asteroidSpriteSheet");
        yield return new WaitForSeconds(Random.Range(low, high));
        InvokeRepeating("animateObj", 0f, 0.15f);
    }

    void animateObj()
    {
        visible = true;
        if (i < astriodSprite.Length)
        {
            asteroidRenderer.sprite = astriodSprite[i];
            i++;
        }
        else
        {
            i = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(visible)
            transform.Translate(Time.maximumDeltaTime * (-speed), 0, 0);
    }
}
