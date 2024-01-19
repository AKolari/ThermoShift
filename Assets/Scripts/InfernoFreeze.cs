using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


public class InfernoFreeze : MonoBehaviour
{



    public GameObject Inferno;
    public Animator InfernoAnimator;
    public Collider2D InfernoCollider;
    public SpriteRenderer InfernoRenderer;
    public int Infernotype = 0;
    private int InfernoCurrentLevel;


    public GameObject Freeze;
    public Animator FreezeAnimator;
    public SpriteRenderer FreezeRenderer;
    public int Freezetype = 1;
    private int FreezeCurrentLevel;
    public Collider2D FreezeCollider;
    
    
    
    
    
    
    public int maxLevel = 8;
    private bool phaseTwo = false;
    private bool finished = false;
    protected Rigidbody2D rb;
    private IMotionBehavior motionBehavior;
    private IMotionBehavior currentMotion;
    public float speed = 3.6f;
    public bool stopMoving=false;
    private float nextCollisionTime=0;

    public GameObject finalKey;


    public Sprite InfernoFlame;
    public Sprite FreezeFlame;
    public Sprite InfernoDeath;
    public Sprite FreezeDeath;




    // Start is called before the first frame update
    void Start()
    {

        finalKey.SetActive(false);
        InfernoAnimator=Inferno.GetComponent<Animator>();
        FreezeAnimator=Freeze.GetComponent<Animator>();
        InfernoCurrentLevel = maxLevel;
        FreezeCurrentLevel = maxLevel;
        InfernoCollider=Inferno.GetComponent<Collider2D>();
        FreezeCollider=Freeze.GetComponent<Collider2D>();
        InfernoRenderer=Inferno.GetComponent<SpriteRenderer>();
        FreezeRenderer=Freeze.GetComponent<SpriteRenderer>();






        rb = gameObject.GetComponent<Rigidbody2D>();
        motionBehavior = new PursuitBehavior(this.transform, rb, GameManager.S.player.transform, 7, speed);
        currentMotion = motionBehavior;
        

        resetAnimation();
    }

    // Update is called once per frame
    void Update()
    {
        
            if(!stopMoving)
            currentMotion.move();

        
         
       

       


    }




    void resetAnimation()
    {
        if(Infernotype==0) {
            InfernoAnimator.Play("Inferno_Walk_" + (maxLevel - InfernoCurrentLevel), -1, 0);
        }
        else
        {
            InfernoAnimator.Play("Freeze_Walk_" + (maxLevel - InfernoCurrentLevel), -1, 0);
        }

        if(Freezetype==1) {
            FreezeAnimator.Play("Freeze_Walk_" + (maxLevel - FreezeCurrentLevel), -1, .4f);
        }
        else
        {
            FreezeAnimator.Play("Inferno_Walk_" + (maxLevel - FreezeCurrentLevel), -1, .4f);
        }




    }







    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Time.time >= nextCollisionTime)
        {
            nextCollisionTime = Time.time+.25f;


            if (collision.gameObject.tag == "Blast")
            {



                if (InfernoCollider.IsTouching(collision))
                {
                    if (Infernotype == 1 && GameManager.S.HeatOn)
                    {
                        InfernoCurrentLevel--;
                    }
                    //If enemy is fire, and gets hit with an ice attack, minus one level.
                    if (Infernotype == 0 && !GameManager.S.HeatOn)
                    {
                        InfernoCurrentLevel--;
                    }
                    //If enemy is ice, and gets hit with an ice attack, plus one level.
                    if (Infernotype == 1 && !GameManager.S.HeatOn)
                    {
                        InfernoCurrentLevel++;
                    }
                    //If enemy is fire, and gets hit with a fire attack, plus one level.
                    if (Infernotype == 0 && GameManager.S.HeatOn)
                    {
                        InfernoCurrentLevel++;
                    }
                    Debug.Log(InfernoCurrentLevel);



                }

                if (FreezeCollider.IsTouching(collision))
                {
                    if (Freezetype == 1 && GameManager.S.HeatOn)
                    {
                        FreezeCurrentLevel--;
                    }
                    //If enemy is fire, and gets hit with an ice attack, minus one level.
                    if (Freezetype == 0 && !GameManager.S.HeatOn)
                    {
                        FreezeCurrentLevel--;
                    }
                    //If enemy is ice, and gets hit with an ice attack, plus one level.
                    if (Freezetype == 1 && !GameManager.S.HeatOn)
                    {
                        FreezeCurrentLevel++;
                    }
                    //If enemy is fire, and gets hit with a fire attack, plus one level.
                    if (Freezetype == 0 && GameManager.S.HeatOn)
                    {
                        FreezeCurrentLevel++;
                    }
                    Debug.Log(FreezeCurrentLevel);
                }

                if (FreezeCurrentLevel > maxLevel)
                {
                    FreezeCurrentLevel = maxLevel;
                }
                if (InfernoCurrentLevel > maxLevel)
                {
                    InfernoCurrentLevel = maxLevel;
                }



                currentMotion = new KnockbackBehavior(rb, collision.transform.position, maxLevel - (InfernoCurrentLevel > FreezeCurrentLevel ? InfernoCurrentLevel : FreezeCurrentLevel) + 4);
                currentMotion.move();
                StartCoroutine(RestoreMotion(.5f));

                checkFightStatus();
                resetAnimation();

            }
        }

    }
    
    void checkFightStatus()
    {
        if (!phaseTwo)
        {
            if (FreezeCurrentLevel <= 0)
            {
                phaseTwo = true;
                enterPhase2("Freeze");
                GameManager.S.player.isImmobile = true;

            }
            else if (InfernoCurrentLevel <= 0)
            {
                phaseTwo = true;
                enterPhase2("Inferno");
                GameManager.S.player.isImmobile = true;
            }

        }
        else if (FreezeCurrentLevel <= 0 && InfernoCurrentLevel <= 0)
        {


            finished = true;
            speed = 0;
            Destroy(this.gameObject);
            EndPhaseTwoCutscene("");


        }
        else{
            if (FreezeCurrentLevel <= 0)
            {
                FreezeAnimator.enabled = false;
                FreezeRenderer.sprite = FreezeDeath;
                FreezeRenderer.flipX = false;

            }
            else if (InfernoCurrentLevel <= 0)
            {
                InfernoAnimator.enabled = false;
                InfernoRenderer.sprite = InfernoDeath;
                InfernoRenderer.flipX = false;
            }
        }
        


    }

    void enterPhase2(string down)
    {
        speed = 0;
        //currentMotion= new PursuitBehavior(this.transform, rb, GameManager.S.player.transform, 7, speed);
        stopMoving=true;
        




        if (down.Equals("Freeze"))
        {
            InfernoAnimator.Play("Inferno_Walk_" + (maxLevel - InfernoCurrentLevel), -1, 0);
            InfernoAnimator.speed = 0;
            FreezeAnimator.enabled = false;
            FreezeRenderer.sprite = FreezeDeath;
            


        }
        else
        {
            FreezeAnimator.Play("Freeze_Walk_" + (maxLevel - FreezeCurrentLevel), -1, 0);
            FreezeAnimator.speed = 0;
            InfernoAnimator.enabled = false;
            InfernoRenderer.sprite = InfernoDeath;
        }
        StartCoroutine(PhaseTwoCutsceneStart(down));



    }


    IEnumerator PhaseTwoCutsceneStart(string down)
    {
        yield return new WaitForSeconds(6);
        if (down.Equals("Freeze"))
        {
            FreezeRenderer.sprite = InfernoFlame;
            FreezeRenderer.flipX = true;
        }


        else
        {
            InfernoRenderer.sprite = FreezeFlame;
            InfernoRenderer.flipX = true;
        }
            

        StartCoroutine(EndPhaseTwoCutscene(down));

    }
     IEnumerator EndPhaseTwoCutscene(string down)
    {
        yield return new WaitForSeconds(3);
        GameManager.S.player.isImmobile = false;
        if (FreezeCurrentLevel <= 0 && InfernoCurrentLevel <= 0)
        {


            Destroy(gameObject);


        }
        if (down.Equals("Freeze"))
        {
            Freezetype = 0;
            
            FreezeAnimator.enabled = true;
            InfernoAnimator.speed = 1;
            FreezeCurrentLevel = InfernoCurrentLevel;
        }
        else
        {
            Infernotype = 1;
            
            InfernoAnimator.enabled = true;
            FreezeAnimator.speed = 1;
            InfernoCurrentLevel = FreezeCurrentLevel;
        }

        
        resetAnimation();
        speed = 3.3f; //Might be a little too fast for the player (originally 6)
        stopMoving = false;
        motionBehavior= new PursuitBehavior(this.transform, rb, GameManager.S.player.transform, 7, speed);
        currentMotion = motionBehavior;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null)
        {
            currentMotion = new KnockbackBehavior(rb, collision.transform.position, 0);
            currentMotion.move();
            StartCoroutine(RestoreMotion(.75f));

            /* if (collision.gameObject.GetComponent<Player>().isInvincible)
             {

                 currentMotion = new KnockbackBehavior(rb, collision.transform.position, collision.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude+1);
                 currentMotion.move();
                 StartCoroutine(RestoreMotion(2f - level / 2));

             }*/

        }
    }



    IEnumerator RestoreMotion(float s)
    {
        yield return new WaitForSeconds(s);
        rb.velocity = Vector2.zero;
        currentMotion = motionBehavior;
    }





    private void OnDestroy()
    {
        if (finished)
        {
            finalKey.SetActive(true);
            Destroy(gameObject);
        }
        
    }
}
