using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour
{
    [SerializeField]
    public GameObject walk_0;
    public GameObject walk_1;
    public GameObject roped_0;
    public GameObject roped_1;
    public GameObject sit;
    public Rope ropeObject;
    public Area bench;
    public bool roped = false;
    public int frameRate = 100;
    public int frame = 0;
    public bool state = true;
    public float direction = 1f;
    public float walkDist = 4f;
    public float traveled = 0f;
    public List<string> Characteristics;
    public string c1;
    public string c2;
    public string c3;
    public bool canCatch = true;
    public bool sitting;

    // Start is called before the first frame update
    void Start()
    {
        //roped = false;
        Characteristics = new List<string>(new string[3] { c1, c2, c3});
        walkDist = 10f;
        canCatch = true;
        if (this.transform.position.x < 0) direction = 1f; else { direction = -1f; transform.localScale = new Vector3(-0.5f, 0.5f, 1); }
    }

    public void rope() {
        roped = true;
    }

    public void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.name == "lassoCollison")rope();
    }
    public void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.name == "lassoCollison") rope();
    }

    private void FixedUpdate() {
       
    }
    public void Delete()
    {
        roped = false;
        if (ropeObject) ropeObject.caught = false;
        if (bench) bench.person1 = null;
        Destroy(this.gameObject);
    }


    // Update is called once per frame
    void Update()
    {
        if (this.transform.position.y < -13f || this.transform.position.y > 13f) {
            roped = false;
            if (ropeObject) ropeObject.caught = false;
            Delete();
        }
        if (this.transform.position.x < -13f || this.transform.position.x > 13f) {
            roped = false;
            if (ropeObject) ropeObject.caught = false;
            Delete();
        }
        //this.transform.position.Set(this.transform.position.x+1f,this.transform.position.y,this.transform.position.z);
        if (traveled >= walkDist) {
            direction = -direction;
            transform.localScale = new Vector3(-0.5f, 0.5f, 1);
            walk_0.GetComponent<SpriteRenderer>().flipX = !walk_0.GetComponent<SpriteRenderer>().flipX;
            walk_1.GetComponent<SpriteRenderer>().flipX = !walk_1.GetComponent<SpriteRenderer>().flipX;
            roped_0.GetComponent<SpriteRenderer>().flipX = !roped_0.GetComponent<SpriteRenderer>().flipX;
            roped_1.GetComponent<SpriteRenderer>().flipX = !roped_1.GetComponent<SpriteRenderer>().flipX;
            traveled = 0f;
            walkDist = Random.Range(1f,Mathf.Abs((direction*10f)-(this.transform.position.x)));
        }

        if (frame > frameRate) {
            if (roped)
            {
                walk_0.SetActive(false);
                walk_1.SetActive(false);
                roped_0.SetActive(state);
                roped_1.SetActive(!state);
                state = !state;

            }
            else if (sitting)
            {
                sit.SetActive(true);
                walk_0.SetActive(false);
                walk_1.SetActive(false);
                roped_0.SetActive(false);
                roped_1.SetActive(false);
            }
            else
            {
                walk_0.SetActive(state);
                walk_1.SetActive(!state);
                roped_0.SetActive(false);
                roped_1.SetActive(false);
                state = !state;
                this.transform.Translate(new Vector3(direction * .25f, 0f, 0f));
                traveled += .25f;
            
            }
            frame = 0;
        }
        frame++;
    }
}
