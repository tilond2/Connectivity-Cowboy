using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Area : MonoBehaviour
{
    // Start is called before the first frame update
    List<GameObject> people;
    private GameManager gm;
    
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        people = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameObject person = collision.gameObject;
            people.Add(person);
        }
    }


    IEnumerator Talking()
    {
        yield return null;
    }
}
