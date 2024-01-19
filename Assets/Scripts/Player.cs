
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    static public GameManager S
    {
        get;
        private set;
    }




   

    public float movementSpeed = 5f;
    private Rigidbody2D playerBody;
    private Vector2 motion;
    private Animator playerAnimator;
    private SpriteRenderer spriteRenderer; 
    private BoxCollider2D boxCollider;
    public ThermoBlast thermoBlast;

    private bool pressDelay = false;
    public int hasKey = 0;
    public int health=2;
    public bool isInvincible {  get; private set; }
    public bool isImmobile;
    private bool isKnocked=false;

    private float invincibleEndTime;
    private float knockedEndTime;
    private float respawnDelay = 1.0f;

    private IMotionBehavior motionBehavior; 

    //Sounds 
    public AudioClip death_SFX;
    public AudioClip Switch_1_SFX; 
    public AudioClip Switch_2_SFX;
    private bool soundSwitch = true; //Changes between Switch_1 and Switch_2



    public Sprite deathSprite;
    public bool alive {
        get; 
        set;
    }
    
    
    void Start()
    {
        playerBody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>(); 
        boxCollider = GetComponent<BoxCollider2D>();
        alive = true; //Might remove when a Start enum state is added to GameManager 
        thermoBlast = this.transform.Find("Blast").gameObject.GetComponent<ThermoBlast>();
        isImmobile = false;
        //Start game 
        GameManager.RESPAWN();
    }

    // Update is called once per frame
    void Update()
    {
        

        if(isInvincible&& Time.time > invincibleEndTime)
        {
            isInvincible = false;
        }
        if(isKnocked && Time.time>knockedEndTime ) { 
         isKnocked = false;
        }

        if(health<=0)
        {
            Death();
        }
        //Detect Axis from arrow key input
        motion.x = Input.GetAxisRaw("Horizontal"); 
        //Debug.Log("Motion X: " + motion.x);
        motion.y = Input.GetAxisRaw("Vertical");
        //Debug.Log("Motion Y: " + motion.y);

        if (Input.GetKey(KeyCode.Space) && !pressDelay && !isImmobile)
        {
            SwitchSound();
            thermoBlast.gameObject.SetActive(true);
            thermoBlast.doBlast();
            GameManager.TEMP_CHANGE();
            StartCoroutine(PressDelay(.25f)); //Amount of time to wait until next press
        }

        if (Input.GetKey(KeyCode.R))
        {
            Death();
        }

        if (isImmobile)
        {
            playerAnimator.enabled = false;
        }
        else
        {
            playerAnimator.enabled=true;
        }


    }

    private void SwitchSound()
    {
        if (soundSwitch) { 
            AudioManager.Instance.playSound(Switch_1_SFX);
            soundSwitch = false;
        } 
        else { AudioManager.Instance.playSound(Switch_2_SFX);
            soundSwitch = true;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        

        if (col.gameObject.GetComponent<Enemy>() || col.gameObject.GetComponent<InfernoFreeze>())
        {
            if (!isInvincible)
            {
                health--;
                isKnocked = true;
                isInvincible = true;
                motionBehavior= new KnockbackBehavior(playerBody, col.transform.position, 4f);
                motionBehavior.move();
                invincibleEndTime = Time.time+.1f;
                knockedEndTime = Time.time+.5f;

                

            }
            else if (isKnocked)
            {
                motionBehavior = new KnockbackBehavior(playerBody, col.transform.position, 3f);
                motionBehavior.move();
            }
            
            
            

        }
        else
        {
           
            if (isKnocked && !isInvincible)
            {
                health--;
            }
        }
        
    }

    private void FixedUpdate()
    {
        if(playerBody.velocity.magnitude !=0  && !isKnocked) {
            playerBody.velocity = new Vector2(0, 0);
            
        }
        if (alive && !isKnocked &&!isImmobile )
        {
            boxCollider.enabled = true;
            playerAnimator.SetFloat("X", motion.x);
            playerAnimator.SetFloat("Y", motion.y);
            playerAnimator.SetFloat("Velocity", motion.sqrMagnitude);

            if (Math.Abs(motion.x) == 1 && Math.Abs(motion.y) == 1)
            {

                //takes the square root of (.5) to make diagonal (or hypothenuse) the exact movementSpeed 
                playerBody.transform.position = playerBody.position + (motion * (float)Math.Sqrt(.5) * movementSpeed * Time.deltaTime);
            }

            else
            {
                //Apply normal movement speed
                playerBody.transform.position = playerBody.position + (motion * movementSpeed * Time.deltaTime);
            }
          //GameManager will envoke Death() method when needed 




    } //GameManager will envoke Death() method when needed 
        else 
        {
            if (!alive)
            {
                boxCollider.enabled = false;
                isKnocked = false;

            }
            
        }

 
        

        

    } 

   

    public void Death() 
    {
        Debug.Log("DEATH");
        if (alive) { AudioManager.Instance.playSound(death_SFX); }
        
        alive = false;
        isImmobile = true;
        playerAnimator.enabled = false;
        spriteRenderer.sprite = deathSprite;
        
        StartCoroutine(DeathDelay()); //Delay until respawn  
    } 

   /* private void deathSound()
    {
        AudioManager.Instance.playSound(death_SFX);
        StartCoroutine(genericDelay(respawnDelay)); //Should line up with respawn delay
    }*/


    IEnumerator PressDelay(float d)
    {
        pressDelay = true;
        // Wait for half a second before next temperature switch.
        yield return new WaitForSeconds(d);
        pressDelay = false;

    } 

    IEnumerator DeathDelay()
    {
        
        yield return new WaitForSeconds(respawnDelay);
        GameManager.RESPAWN();
    } 

    IEnumerator genericDelay(float d)
    {
        yield return new WaitForSeconds(d);
    }
}
