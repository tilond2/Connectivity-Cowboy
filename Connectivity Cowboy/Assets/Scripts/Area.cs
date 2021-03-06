﻿using System.Collections;
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
            person1.transform.position = new Vector2(transform.position.x - .6f, transform.position.y + .6f);
            if (person1.GetComponent<Person>().ropeObject) { person1.GetComponent<Person>().ropeObject.caught = false; 
                person1.GetComponent<Person>().ropeObject = null;
                person1.GetComponent<Person>().canCatch = false;
                person1.GetComponent<Person>().sitting = true;
            }

            

            if (person2) {
                person2.transform.position = new Vector2(transform.position.x + .6f, transform.position.y + .6f);
                if (person2.GetComponent<Person>().ropeObject) { person2.GetComponent<Person>().ropeObject.caught = false; person2.GetComponent<Person>().ropeObject = null; 

                person2.GetComponent<Person>().canCatch = false;
                person2.GetComponent<Person>().sitting = true;
                }
            };
            StartCoroutine(Checking());
            
            

        }
    }
    IEnumerator Checking()
    {
        for (int i = 0; i< 14; i++)
        {
            yield return new WaitForSeconds(1);
            if (!person2)
            {
                continue;
            }
            else
            {
                p1 = person1;
                p2 = person2;
                StartCoroutine(Talking());

                person1 = null;
                person2 = null;

            }
        }
        if (!person2)
        {
            if (person1)
            {
                person1.GetComponent<Person>()._Delete();
                person1 = null;
            }
        }
        
    }

    IEnumerator Talking()
    {
        
        p1.transform.position = new Vector2(transform.position.x - .6f, transform.position.y+.6f);
        p2.transform.position = new Vector2(transform.position.x + .6f, transform.position.y + .6f);

        Person firstPerson = p1.GetComponent<Person>();
        Person secondPerson = p2.GetComponent<Person>();

        //firstPerson.ropeObject.caught = false;
        //secondPerson.ropeObject.caught = false;
        //firstPerson.sitting = true;
        //secondPerson.sitting = true;

        int matchCount = 0;
        yield return new WaitForSeconds(3);
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
            GameObject clone = (GameObject)Instantiate(sad, new Vector2(transform.position.x, transform.position.y - .65f), Quaternion.identity);
            Destroy(clone, 1.8f);
        }
        else
        {
            gm.AddScore(matchCount*10);
            GameObject clone;
            if (matchCount == 3)
            { 
                clone = (GameObject)Instantiate(excited, new Vector2(transform.position.x, transform.position.y - .65f), Quaternion.identity);
            }
            else
            {
                clone = (GameObject)Instantiate(happy, new Vector2(transform.position.x, transform.position.y - .65f), Quaternion.identity);
            }
            Destroy(clone, 1.8f);
        }
        firstPerson._Delete();
        secondPerson._Delete();
        p1 = null;
        p2 = null;
        checking = false;
       
    }
}
