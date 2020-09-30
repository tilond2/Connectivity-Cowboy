using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    //12

    // 3 0

    [SerializeField]
    public GameObject person;
    public float xMax = 12f;
    public float xMin = -12f;
    public float yMax = 3f;
    public float yMin = -3f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void spawn() {
        float x = Random.Range(xMin, xMax);
        float y = Random.Range(yMin, yMax);

        Instantiate(person, new Vector3(x, y, 0), Quaternion.identity);
    }
}
