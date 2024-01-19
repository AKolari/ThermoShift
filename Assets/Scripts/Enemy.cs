using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class Enemy : MonoBehaviour
{
    public int type=1; //0 or 1
    public int level=1;
    private int maxHealth =3;
    public float speed = 2f;
    private Vector2 knockback;
    private float knockbackSpeed = 6;
    private Animator enemyAnimator;
    private Rigidbody2D rb;
    private SpriteRenderer rbSprite;
    
    private IMotionBehavior motionBehavior;
    private IMotionBehavior currentMotion;

    //Sounds 
    public AudioClip enemyDamage; 
    public AudioClip enemyDefeat; 
    public AudioClip enemyPowerup;
   
    void Start()
    {
        motionBehavior = new MoveTillCollisionBehavior(this.gameObject, speed);
        currentMotion = motionBehavior;
        enemyAnimator = gameObject.GetComponent<Animator>();
        enemyAnimator.SetFloat("Type", type);
        rb=gameObject.GetComponent<Rigidbody2D>();
        rbSprite=gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        
        if(rb.velocity.magnitude<speed)
        {
            
            currentMotion.move();
           
        }

        if (level <= 0)
        {
            AudioManager.Instance.playSound(enemyDefeat);
            Destroy(this.gameObject);
        }



        
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<Player>() != null)
        {
            currentMotion = new KnockbackBehavior(rb, collision.transform.position, speed/2);
            currentMotion.move();
            StartCoroutine(RestoreMotion(.5f));

           /* if (collision.gameObject.GetComponent<Player>().isInvincible)
            {

                currentMotion = new KnockbackBehavior(rb, collision.transform.position, collision.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude+1);
                currentMotion.move();
                StartCoroutine(RestoreMotion(2f - level / 2));

            }*/

        }
    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Blast")
        {
            //If enemy is ice, and gets hit with a heat attack, minus one level.
            if (type == 1 && GameManager.S.HeatOn)
            {
                if (level - 1 != 0) { AudioManager.Instance.playSound(enemyDamage); } //If the next hit won't kill
                level--;
                
                
            }
            //If enemy is fire, and gets hit with an ice attack, minus one level.
            if (type == 0 && !GameManager.S.HeatOn)
            {
                if (level - 1 != 0) { AudioManager.Instance.playSound(enemyDamage); }
                level--;
                 //If the next hit won't kill
            }
            //If enemy is ice, and gets hit with an ice attack, plus one level.
            if (type == 1 && !GameManager.S.HeatOn)
            {
                if (level+1<maxHealth)
                level++;
                AudioManager.Instance.playSound(enemyPowerup);
            }
            //If enemy is fire, and gets hit with a fire attack, plus one level.
            if (type == 0 && GameManager.S.HeatOn)
            {
                if (level + 1 < maxHealth)
                    level++;
                AudioManager.Instance.playSound(enemyPowerup);
            }


            currentMotion = new KnockbackBehavior(rb, collision.transform.position, knockbackSpeed-level);
            currentMotion.move();
            StartCoroutine(RestoreMotion(2f-level/2));


            
           

            
        }
       
    }




    IEnumerator RestoreMotion(float s)
    {
        yield return new WaitForSeconds(s);
        rb.velocity = Vector2.zero;
        currentMotion = motionBehavior;
    }


}
