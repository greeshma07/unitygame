using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class spriteAnimator : MonoBehaviour {
    private SpriteRenderer spriteRenderer,gemRenderer;
    private Sprite[] birdSprite;
    private GameObject gameObj1,gameObj2;
    private int i = 0,time=10;
    public float speed;
    private bool stopped = false, gameFinished = false, gameStart = false, gemStatus = false, shoot = false;
    Vector2 movement = new Vector2();
    public int score = 0;
    public string scoreText = "Score: ", timeText = "Time: ", result,points;
    public string lossText = "Sorry, better luck next time!";
    public string winText = "Congrats, you won!", currentScene;
    public Text canvasText, resultText, showScore, showTime;
    private float minY = -4.5f; 
    private float maxY = 3.5f;
    GameObject prefab;

    // Use this for initialization
    void Start()
    {
        currentScene = SceneManager.GetActiveScene().name;
        if (currentScene != "scene1")
        {
            score = PlayerPrefs.GetInt("pointsScored");
        }
        if(currentScene == "scene2")
        {
            if (score < 400)
            {
               gameObj2 = GameObject.Find("Gem");
               gameObj2.SetActive(false);
            }
        }
        if(currentScene == "scene3")
        {
            if (score < 1000)
            {
                gameObj2 = GameObject.Find("Gun");
                gameObj2.SetActive(false);
            }
            else
            {
                prefab = Resources.Load("projectile") as GameObject;
            }
        }
        gameObj1 = GameObject.Find("Bird");
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        gameObj1.transform.position = Camera.main.ViewportToWorldPoint(pos);
        resultText.text = "";
        spriteRenderer = gameObj1.GetComponent<SpriteRenderer>(); 
        canvasText.text = scoreText+score.ToString();
        birdSprite = Resources.LoadAll<Sprite>("bird");
        InvokeRepeating("animate", 0f, 0.1f);       
    }

    void animate()
    {
        if(null != spriteRenderer)
        {
            if (i < birdSprite.Length)
            {
                spriteRenderer.sprite = birdSprite[i];
                i++;
            }
            else
            {
                i = 0;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {        
        print("collision");
        if (collision.gameObject.CompareTag("asteroid") && !gemStatus)
        {
            spriteRenderer.color = new Color(0f, 0f, 0f);
            stopped = true;
            result = lossText;
        }
        else if (collision.gameObject.CompareTag("coin"))
        {
            Destroy(collision.gameObject);
            score += 100;
            canvasText.text = scoreText + score.ToString();
            print(score);
        }
        else if (collision.gameObject.CompareTag("gem") && !stopped)
        {
            Destroy(collision.gameObject);
            transform.localScale += new Vector3(2f, 2f, 2f);
            gemStatus = true;
            showTime.text = timeText + time.ToString();
            // start the timer ticking
            StartCoroutine(timerTick());
        }    
        else if (collision.gameObject.CompareTag("enemy"))
        {
            spriteRenderer.color = new Color(0f, 0f, 0f);
            stopped = true;
            result = lossText;
            shoot = false;
        }
        else if (collision.gameObject.CompareTag("gun"))
        {
            Destroy(collision.gameObject);
            shoot = true;
        }
    }

    // Update is called once per frame
    void Update()
    {        
        if (Input.GetKeyDown(KeyCode.Space) && !stopped && shoot)
        {
            GameObject projectile = Instantiate(prefab) as GameObject;
            projectile.transform.position = gameObj1.transform.position + Camera.main.transform.right * 1;
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            rb.velocity = Camera.main.transform.right * 40;            
        }
        if (null != spriteRenderer && spriteRenderer.isVisible)
        {
            gameStart = true;
        }
        if (null != spriteRenderer && !(spriteRenderer.isVisible) && !stopped && gameStart)
        {
            Destroy(spriteRenderer);
            PlayerPrefs.SetInt("pointsScored", score);
            if (currentScene == "scene1")
                SceneManager.LoadScene("scene2");
            else if (currentScene == "scene2")
                SceneManager.LoadScene("scene3");
            else
            {
                result = winText;
                gameOver();
            }
        }
        else
        {
            movement = new Vector2(1, speed);
            if (null != spriteRenderer && stopped)
            {
                movement = new Vector2(speed, 1);
                transform.Translate(-movement * 0.5f);
                StartCoroutine(executeAfterTime(1));
            }
            else if (null != spriteRenderer)
            {
                transform.Translate(movement * Time.deltaTime * 1.3f);
                if (Input.GetKeyDown(KeyCode.UpArrow) && transform.position.y < maxY)
                {
                    Vector3 position = this.transform.position;
                    position.y += 2;
                    this.transform.position = position;
                }
                if (Input.GetKeyDown(KeyCode.DownArrow) && transform.position.y > minY)
                {
                    Vector3 position = this.transform.position;
                    position.y -= 2;
                    this.transform.position = position;
                }
            }
        }
    }

    IEnumerator executeAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(spriteRenderer);
        gameOver();
    }

    void gameOver()
    {
        resultText.text = result;
        gameFinished = true;
        print("game over");
    }

    public IEnumerator timerTick()
    {
        while (time > -1)
        {
            yield return new WaitForSeconds(1.0f);
            updateTimeText();
        }
        transform.localScale -= new Vector3(2f, 2f, 2f);
        gemStatus = false;
        showTime.text = "";
    }

    void updateTimeText()
    {
        time--;
        showTime.text = timeText+time.ToString();
    }

    private void OnGUI()
    {
        if (gameFinished)
        {
            int res = result.CompareTo(winText);
            if (res == 0)
            {
                showScore.text = scoreText + score.ToString();
            }
            GUIStyle myStyle = new GUIStyle();
            myStyle.fontSize = 16;
            myStyle.normal.textColor = Color.red;
            if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2, 150, 35),
            "PLAY GAME", myStyle))
            {
                PlayerPrefs.SetInt("pointsScored",0);
                SceneManager.LoadScene("scene1");
            }
            if (GUI.Button(new Rect(Screen.width / 2 - 20, Screen.height / 2 + 30, 150, 35),
                "QUIT", myStyle))
            {
                Application.Quit();
            }
        }        
    }    
}