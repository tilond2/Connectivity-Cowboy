using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    [SerializeField]
    private int score;
    private Text scoreCount;
    public float timer;
    public int time;
    private Text timeDisplay;
    bool timerRunning;
    bool endGame;

    // Start is called before the first frame update
    void Start()
    {
        scoreCount = GameObject.Find("Score").GetComponent<Text>();
        timeDisplay = GameObject.Find("Timer").GetComponent<Text>();
        timer = 80f;
        timerRunning = true;
        GameObject.Find("SoundManager").GetComponent<SoundManager>().PlaySound("music", false);
    }

    // Update is called once per frame
    void Update()
    {
        scoreCount.text = score.ToString();
        timeDisplay.text = time.ToString();

        if (timerRunning)
        {
            timer -= Time.deltaTime;
            time = Mathf.FloorToInt(timer);
        }
        
        if (timer < 0) { 
            timerRunning = false;
            endGame = true;
        }
        if (endGame == true)
        {
            StartCoroutine(EndGame());
        }
        

    }

    public void AddScore(int s)
    {
        score += s;
    }
    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


}
