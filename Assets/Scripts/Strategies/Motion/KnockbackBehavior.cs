using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class KnockbackBehavior :IMotionBehavior
{
    
    private Rigidbody2D myBody;
    private Vector2 knockbackPosition;
    private float knockbackSpeed;
    private Vector2 knockbackVelocity;





    public KnockbackBehavior(Rigidbody2D myBody, Vector2 knockbackPosition, float knockbackSpeed ) {
        this.myBody = myBody;
        this.knockbackPosition = knockbackPosition;
        this.knockbackSpeed = knockbackSpeed;
    
    
    }



    public void move()
    {
        Vector2 blastVelocity= (Vector2)myBody.transform.position-knockbackPosition;
       

        if (Mathf.Abs(blastVelocity.x) >= Mathf.Abs(blastVelocity.y))
        {
            // Knockback should be horizontal
            blastVelocity.x = (blastVelocity.x > 0) ? 1 : -1;
            blastVelocity.y = 0;
        }
        else
        {
            // Knockback should be vertical
            blastVelocity.x = 0;
            blastVelocity.y = (blastVelocity.y > 0) ? 1 : -1;
        }

        knockbackVelocity = blastVelocity * (knockbackSpeed);
        myBody.velocity = knockbackVelocity;
    }
   
}
