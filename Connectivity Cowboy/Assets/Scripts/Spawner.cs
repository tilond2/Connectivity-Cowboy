using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    //12

    // 3 0

    [SerializeField]
    public List<GameObject> people;
    public GameObject person;
    GameManager gm;
    public GameObject leftSpawn;
    public GameObject rightSpawn;
    bool spawnCheck;
    bool s = false;
    public float xMax = 12f;
    public float xMin = -12f;
    public float yMax = 3f;
    public float yMin = -2f;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        spawn();
        spawn();
        spawn();
        spawn();

        
    }

    // Update is called once per frame
    void Update()
    {
        if (gm.time%6 == 0 && s == false)
        {
            spawnCheck = true;
        }
        else if(gm.time % 6 == 0 && s == true)
        {
            spawnCheck = false;
        }
        else
        {
            s = false;
        }
        if (spawnCheck)
        {
            spawn();
            spawnCheck = false;
            s = true;
        }

    }

    public void spawn() {
        int i = Random.Range(0, people.Count);
        float x = Random.Range(xMin, xMax);
        //float y = Random.Range(yMin, yMax);

        float y = Random.Range(-1.5f, 1.5f);

        Instantiate(people[i], new Vector3(x, y, 0), Quaternion.identity);
    }
}
