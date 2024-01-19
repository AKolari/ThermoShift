using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{

    //Sounds 
    public AudioClip keyCollect;
    public bool hasEnemies=false;
    private int enemyCount; //Current Count
    private int enemiesInitial; //Amount on-screen when the scene is loaded
    public int enemiesNeeded=0; //Needed for key to appear

    private BoxCollider2D boxCollider; 
    private  SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        //Will this be called again whenever the scene reloads? 
        gameObject.SetActive(true);
        if (hasEnemies)
        {
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>(); 
            boxCollider = gameObject.GetComponent<BoxCollider2D>();

            //Key CAN be collected
            spriteRenderer.enabled = true;
            boxCollider.enabled = true;
            
            if (enemiesNeeded == 0) //If hasEnemies is True, but enemyCount is not already set
            {
                enemiesNeeded = FindObjectsOfType<Enemy>().Length;
            }

            enemiesInitial = FindObjectsOfType<Enemy>().Length;
            //Debug.Log("Initial enemies: " + enemiesNeeded);
        } 


        
    }

    // Update is called once per frame
    void Update()
    {
        //This is a horribly inefficient solution, but I no longer have time to optimize as I would like to :^(
        if (hasEnemies)
        {
            //Debug.Log("Current enemies: " + enemyCount);
            enemyCount = FindObjectsOfType<Enemy>().Length;
            if (enemiesInitial - enemyCount >= enemiesNeeded && hasEnemies == true) // 
            {
                //Key CAN be collected
                spriteRenderer.enabled = true;
                boxCollider.enabled = true; 
                
                AudioManager.Instance.playSound(keyCollect);
                hasEnemies = false;
                
            } 

            else
            {
                //Key CANNOT be collected
                spriteRenderer.enabled = false;
                boxCollider.enabled = false;
            }
        }
        
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Key Get");
            AudioManager.Instance.playSound(keyCollect);
            GameManager.KEY_GET(true);
            //Play sound
            gameObject.SetActive(false);
        } 
        
    }
}
