using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    public AudioClip doorOpen;
    // Start is called before the first frame update
    void Start()
    {
        //Will this be called again whenever the scene reloads? 
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {


            if (GameManager.CHECK_KEY() == true)
            {
                Debug.Log("Key Used");
                GameManager.KEY_GET(false);
                //Play sound
                AudioManager.Instance.playSound(doorOpen);
                gameObject.SetActive(false);
            }

        }
        
    }
}
