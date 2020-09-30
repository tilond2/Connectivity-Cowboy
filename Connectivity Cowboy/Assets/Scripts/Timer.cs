using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    [SerializeField]
    public GameObject Spawner;
    public int tick = 0;
    public int timer = 60;
    public Text text;
    public int maxPeople = 15;

    void Tick() {
        if (timer < 0) {
            //END
        } else {
            text.text = timer.ToString();
            /*if (PlayerPrefs.GetInt("person") < maxPeople) {
                GetComponentInChildren<Spawner>().spawn();
            }*/
        }

        tick++;
        timer--;
    }
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Tick", 1f, 1f);
        text.text = timer.ToString();
        /*PlayerPrefs.SetInt("person", 0);*/
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
