using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using System;

public class BlockSwitch : MonoBehaviour
{

    //In the future, derive blocks based on this class (or just give the script to whoever needs it)

    static public GameManager S
    {
        get;
        private set;
    }

    

    public Sprite OnSprite;
    public Sprite OffSprite;
    protected GameObject curPlayer;
    protected BoxCollider2D colliderBlock;
    //public BoxCollider2D deathSquish; 

    public float squishRange = .4f; //How far from the center of a block, the player needs to be to die

    [Tooltip("All blocks are H(ot), C(old) or N(eutral)")]
    public string type; //Is H(ot), C(old) or N(eutral)
    //Hot is only on when Hot, Cold is only On when Cold, Neutral is ALWAYS on

    protected SpriteRenderer spriteRenderer;
    protected bool isOn;

    //Sounds  

    


    // Start is called before the first frame update
    void Start()
    {

        //Make need to make its own function
        spriteRenderer = GetComponent<SpriteRenderer>();
        colliderBlock = GetComponent<BoxCollider2D>();
        curPlayer = GameObject.FindWithTag("Player");

    }

    // Update is called once per frame
    void Update()
    {

        blockFunction();
    }

    void blockFunction() 
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
            colliderBlock.enabled = true;
            deathSquish();
            spriteRenderer.sprite = OnSprite;
        }

        else
        {
            colliderBlock.enabled = false;
            spriteRenderer.sprite = OffSprite;
        }
    }
    protected bool deathSquish()
    {
      
        if (Math.Abs(curPlayer.transform.position.x - this.transform.position.x) <= squishRange &&
            Math.Abs(curPlayer.transform.position.y - this.transform.position.y) <= squishRange && GameManager.IS_ALIVE())
        {
            //Debug.Log("Player Death");
            GameManager.DEATH();
            
            return true;
        }

        else return false;
    }
}
