using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PursuitBehavior : IMotionBehavior
{

    private static Vector2[] directions = new Vector2[] {
        Vector2.right, Vector2.up, Vector2.left,Vector2.down
    };
    RaycastHit2D lineOfSight;
    private Transform myPosition;
    private Transform target;
    private Rigidbody2D myRigidBody;
    private float speed;
    private int targetLayer;
    private bool wandering = false;



    public PursuitBehavior(Transform myPosition, Rigidbody2D myRigidBody, Transform target, int targetLayer, float speed)
    {
        this.myPosition = myPosition;
        this.myRigidBody = myRigidBody;
        this.target = target;
        this.targetLayer = targetLayer;
        this.speed = speed;


    }

    public void Scan()
    {

        lineOfSight = Physics2D.Raycast(myPosition.position, target.position-myPosition.position, Mathf.Infinity, 10111111);
       
        
        if (!lineOfSight.transform.tag.Equals("Player") && myRigidBody.velocity.magnitude==0 &&!wandering)
        {
            wandering = true;

            
        }
        else
        if(wandering && lineOfSight.transform.tag.Equals("Player")) 
        { 
            wandering=false;
            
        }

    }



    

    public void Wander()
    {
        if (myRigidBody.velocity.magnitude == 0)
        {
            int rand = Random.Range(0, directions.Length);
            myRigidBody.velocity = directions[rand] * speed;
        }

       
    }

    public void Pursue()
    {
        myRigidBody.velocity = ( target.position - myPosition.position).normalized * speed;
    }


    public void move()
    {
        if(myRigidBody.velocity.magnitude <speed)
        {
            myRigidBody.velocity.Equals(Vector2.zero);
        }
        
        Scan();
        if(wandering)
        {
            //Debug.Log("Wandering");
            Wander();
        }
        else
        {
            //Debug.Log("Pursuing");
            Pursue();
        }
    }
}
