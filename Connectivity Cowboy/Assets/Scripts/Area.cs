using System.Collections;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Area : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public GameObject person1, person2;
    [SerializeField]
    GameObject p1, p2;
    public GameObject sad;
    public GameObject happy;
    public GameObject excited;
    private GameManager gm;
    private bool checking;
    
    public bool isCoroutineReady;


    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Person")
        {
            collision.gameObject.GetComponent<Person>().bench = this;
            if(!ReferenceEquals(person1, collision.gameObject)){
                if (p1) {return; }
                if (person1) { person2 = collision.gameObject;  }
                else { person1 = collision.gameObject; }
            }
            person1.transform.position = new Vector2(transform.position.x - 1f, transform.position.y - 1f);
            if (person1.GetComponent<Person>().ropeObject) { person1.GetComponent<Person>().ropeObject.caught = false; person1.GetComponent<Person>().ropeObject = null; }

                person1.GetComponent<Person>().canCatch = false;
            if (person2) {
                person2.transform.position = new Vector2(transform.position.x + 1f, transform.position.y - 1f);
                if (person2.GetComponent<Person>().ropeObject) { person2.GetComponent<Person>().ropeObject.caught = false; person2.GetComponent<Person>().ropeObject = null; }

                person2.GetComponent<Person>().canCatch = false;
            };
            StartCoroutine(Checking());
            
            

        }
    }
    IEnumerator Checking()
    {

        yield return new WaitForSeconds(12);
        if (!person2)
        {
            if (person1)
            {
                person1.GetComponent<Person>()._Delete();
                person1 = null;
            }
        }
        else
        {
            p1 = person1;
            p2 = person2;
            Talking();

            person1 = null;
            person2 = null;
            
        }
    }

    void Talking()
    {
        
        p1.transform.position = new Vector2(transform.position.x - 1f, transform.position.y-1f);
        p2.transform.position = new Vector2(transform.position.x + 1f, transform.position.y - 1f);

        Person firstPerson = p1.GetComponent<Person>();
        Person secondPerson = p2.GetComponent<Person>();

        //firstPerson.ropeObject.caught = false;
        //secondPerson.ropeObject.caught = false;
        //firstPerson.sitting = true;
        //secondPerson.sitting = true;

        int matchCount = 0;
        
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (firstPerson.Characteristics[j] == secondPerson.Characteristics[i])
                {
                    matchCount += 1;
                    break;
                }
            }
        }
        if (matchCount == 0)
        {
            gm.AddScore(-10);
            GameObject clone = (GameObject)Instantiate(sad, transform.position, Quaternion.identity);
            Destroy(clone, 1.0f);
        }
        else
        {
            gm.AddScore(matchCount*10);
            GameObject clone;
            if (matchCount == 3)
            { 
                clone = (GameObject)Instantiate(excited, transform.position, Quaternion.identity);
            }
            else
            {
                clone = (GameObject)Instantiate(happy, transform.position, Quaternion.identity);
            }
            Destroy(clone, 1.0f);
        }
        firstPerson._Delete();
        secondPerson._Delete();
        p1 = null;
        p2 = null;
        checking = false;
       
    }
}
