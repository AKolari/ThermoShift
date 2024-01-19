using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThermoBlast : MonoBehaviour
{


    private bool pressDelay = false;
    private SpriteRenderer mySpriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
       
        pressDelay = false;
        mySpriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        if(GameManager.S.HeatOn)
        {
            mySpriteRenderer.color = Color.red;
        }
        else
        {
            mySpriteRenderer.color = Color.blue;
        }
        gameObject.SetActive(false);
    }

    
    public void doBlast()
    {
        if (Input.GetKey(KeyCode.Space) && !pressDelay)
        {
        
            colorSwitch();
           
            
            StartCoroutine(BlastIsActive(.25f));
        }
    }


    private void Update()
    {
        this.gameObject.transform.position=this.gameObject.transform.parent.transform.position;
    }


    IEnumerator BlastIsActive(float d)
    {
        yield return new WaitForSeconds(d);
        gameObject.SetActive(false);
    }

    private void colorSwitch()
    {
        if(mySpriteRenderer.color==Color.red)
        {
            mySpriteRenderer.color = Color.blue;
        }
        else
        {
            mySpriteRenderer.color= Color.red;
        }
    }
}
