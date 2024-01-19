using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{

    static public GameManager S
    {
        get;
        private set;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /*
    private void OnCollisionEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Debug.Log("Player Death");
            GameManager.NEXT_LEVEL();

        }
    }
    */


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            
            GameManager.NEXT_LEVEL();
            // Debug.Log("Player Death");


        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            StartCoroutine(NextLevelDelay());
            // Debug.Log("Player Death");
           

        }
    }

    IEnumerator NextLevelDelay()
    {
        yield return new WaitForSeconds(.25f);
        GameManager.NEXT_LEVEL();


    }


}
