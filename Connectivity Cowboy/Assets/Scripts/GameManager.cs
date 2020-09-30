using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    [SerializeField]
    private int score;
    private Text scoreCount;
    public float timer;
    public int time = 60;
    private Text timeDisplay;
    bool timerRunning;
    bool endGame;
    // Start is called before the first frame update
    void Start()
    {
        scoreCount = GameObject.Find("Score").GetComponent<Text>();
        timeDisplay = GameObject.Find("Timer").GetComponent<Text>();
        timer = 60f;
        time = 60;
        timerRunning = true;
        InvokeRepeating("Tick", 1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        scoreCount.text = score.ToString();
        timeDisplay.text = time.ToString();

        /*if (timerRunning)
        {
            timer -= Time.deltaTime;
            time = Mathf.FloorToInt(timer);
        }*/
        
        if (timer < 0) { 
            timerRunning = false;
            endGame = true;
        }
        if (endGame == true)
        {
            StartCoroutine(EndGame());
        }
    }

    void Tick() {
        time--;
    }

    public void AddScore(int s)
    {
        score += s;
    }
    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(2);
    }


}
