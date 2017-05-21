using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private SpriteRenderer enemyRenderer;
    private Sprite[] enemySprite;
    private int i = 0;
    private float minY = -4f;
    private float maxY = 4f;
    private bool up = false;

    // Use this for initialization
    void Start()
    {
        enemyRenderer = this.GetComponent<SpriteRenderer>();
        enemySprite = Resources.LoadAll<Sprite>("fightbird");
        InvokeRepeating("animateObj", 0f, 0.15f);
    }

    void animateObj()
    {
        if (i < enemySprite.Length)
        {
            enemyRenderer.sprite = enemySprite[i];
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
        if (transform.position.y <= minY)
        {
            up = true;
            Vector3 position = this.transform.position;
            position.y = -4f;
            this.transform.position = position;
        }
        else if (transform.position.y >= maxY)
        {
            up = false;
            Vector3 position = this.transform.position;
            position.y = 4f;
            this.transform.position = position;
        }
        print(up);
        if (transform.position.y <= maxY && !up)
            transform.Translate(Time.maximumDeltaTime * 0, -0.2f, 0);
        if (transform.position.y >= minY && up)
            transform.Translate(Time.maximumDeltaTime * 0, 0.2f, 0);
    }    
}
