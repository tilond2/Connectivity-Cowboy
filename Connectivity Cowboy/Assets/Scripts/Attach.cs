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
        if (collision.gameObject.GetComponent<Person>().canCatch)
        {
            roped = collision.gameObject;
            this.GetComponentInParent<Rope>().roped = roped;
            this.GetComponentInParent<Rope>().caught = true;
            collision.gameObject.GetComponent<Person>().ropeObject = this.GetComponentInParent<Rope>();
        }
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        
        //this.GetComponentInParent<Rope>().caught = false;

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
