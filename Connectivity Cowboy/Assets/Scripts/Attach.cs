using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attach : MonoBehaviour
{
    [SerializeField]
    public GameObject roped;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        roped = collision.gameObject;
    }

    private void OnCollisionEnter(Collision collision) {
        roped = collision.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
