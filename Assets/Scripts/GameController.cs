using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    public int maxSize;
    public int currentSize;
    public GameObject snakeprefab;
    public Snake head;
    public Snake tail;
    public int NESW;
    public int score;
    public Vector2 nexPos;
    public int xBound;
    public int yBound;
    public GameObject foodprefab;
    public GameObject currentFood;
    public Text scoreText;
    public float deltaTimer;
    private void OnEnable()
    {
        Snake.hit += hit;
    }
    private void OnDisable()
    {
        Snake.hit -= hit;
    }
    // Use this for initialization
    void Start () {
        InvokeRepeating("TimerInvoke", 0, deltaTimer);
        FoodFunc();
	}
	
	// Update is called once per frame
	void Update () {
        ComChangeD();

    }
    void TimerInvoke()
    {
        Movement();
        StartCoroutine("CheckVisible");
        if(currentSize>=maxSize)
        {
            TailFunc();
        }
        else
        {
            currentSize++;
        }
     
    }
    void Movement()
    {
        GameObject temp;
        nexPos = head.transform.position;
        switch (NESW)
        {
            case 0:
                nexPos = new Vector2(nexPos.x, nexPos.y+0.5f);
                break;
            case 1:
                nexPos = new Vector2(nexPos.x+0.5f, nexPos.y);
                break;
            case 2:
                nexPos = new Vector2(nexPos.x, nexPos.y - 0.5f);
                break;
            case 3:
                nexPos = new Vector2(nexPos.x-0.5f, nexPos.y );
                break;
        }
        temp = (GameObject)Instantiate(snakeprefab, nexPos, transform.rotation);
        head.SetNext(temp.GetComponent<Snake>());
        head=temp.GetComponent<Snake>();
        return;
    }
    void ComChangeD()
    {
        if(NESW !=2 && Input.GetKeyDown(KeyCode.W))
        {
            NESW = 0;
        }
        if (NESW !=3 && Input.GetKeyDown(KeyCode.D))
        {
            NESW = 1;
        }
        if (NESW !=0 && Input.GetKeyDown(KeyCode.S))
        {
            NESW = 2;
        }
        if (NESW !=1 && Input.GetKeyDown(KeyCode.A))
        {
            NESW = 3;
        }
    }
    void TailFunc()
    {
        Snake tempSnake=tail;
        tail = tail.GetNext();
        tempSnake.RemoveTail();
    }
    void FoodFunc()
    {
        int xPos = Random.Range(-xBound, xBound);
        int yPos = Random.Range(-yBound, yBound);
        currentFood = (GameObject)Instantiate(foodprefab,new Vector2(xPos,yPos),transform.rotation );
        StartCoroutine(CheckRender(currentFood));

    }
    IEnumerator CheckRender(GameObject IN)
    {
        yield return new WaitForEndOfFrame();
        if(IN.GetComponent<Renderer>().isVisible==false)
        {
            if (IN.tag =="Food")
            {
                Destroy(IN);
                FoodFunc();
            }
        }
    }
    void hit(string WhatWasSent)
    {
        if (WhatWasSent == "Food")
        {
            if (deltaTimer >= .1f)
            {
                deltaTimer -= .05f;
                CancelInvoke("TimerInvoke");
                InvokeRepeating("TimerInvoke", 0, deltaTimer);
            }
            FoodFunc();
            maxSize++;
            score++;
            scoreText.text = score.ToString();
            int temp = PlayerPrefs.GetInt("HighScore");
            if (score > temp)
            {
                PlayerPrefs.SetInt("HighScore", score);
            }
        }
        if (WhatWasSent == "Snake")
        {
            CancelInvoke("TimerInvoke");
            Exit();
        }
    }
    public void Exit()
    {
        SceneManager.LoadScene(0);
    }
    void Wrap()
    {
        if (NESW == 0)
        {
            head.transform.position = new Vector2(head.transform.position.x, -(float)(head.transform.position.y - 0.5));
        }
        else if (NESW == 1)
        {
            head.transform.position=new Vector2((float)-(head.transform.position.x-0.5),head.transform.position.y);
        }
        else if (NESW == 2)
        {
            head.transform.position = new Vector2(head.transform.position.x,(float) -(head.transform.position.y + 1));
        }
        else if (NESW == 3)
        {
            head.transform.position = new Vector2((float)-(head.transform.position.x + 1), head.transform.position.y);
        }
    }
    IEnumerator CheckVisible()
    {
        yield return new WaitForEndOfFrame();
        if (head.GetComponent<Renderer>().isVisible==false)
        {
            Wrap();
        }
    }
}
