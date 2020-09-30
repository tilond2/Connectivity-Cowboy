using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    [SerializeField]
    private int score;
    private Text scoreCount;
    // Start is called before the first frame update
    void Start()
    {
        scoreCount = GameObject.Find("Score").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddScore(int s)
    {
        score += s;
    }


}
