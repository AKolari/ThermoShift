using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBlock : BlockSwitch
{
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        colliderBlock = GetComponent<BoxCollider2D>();
        curPlayer = GameObject.FindWithTag("Player");

    }

    // Update is called once per frame
    void Update()
    {
        blockFunction();
    }

    private void blockFunction()
    {
        if (type == "H")
        {
            //On when Hot
            if (GameManager.heatOn == true)
            {
                isOn = true;
            }

            else
            {
                isOn = false;
            }
        }

        else if (type == "C")
        {
            //On when Cold
            if (GameManager.heatOn == false)
            {
                isOn = true;
            }

            else
            {
                isOn = false;
            }
        }

        else { isOn = true; } //On when Neutral

        //Change collider and sprite 
        if (isOn)
        {
            colliderBlock.enabled = false;
            spriteRenderer.sprite = OnSprite;
        }

        else
        {
            colliderBlock.enabled = true;
            spriteRenderer.sprite = OffSprite;
        } 

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player Death");
            GameManager.DEATH();

        }
    }
}
