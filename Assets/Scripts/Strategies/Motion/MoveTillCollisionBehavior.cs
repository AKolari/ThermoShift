using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTillCollisionBehavior : IMotionBehavior
{
    
    private Rigidbody2D rb;
    private SpriteRenderer rbSprite;
    private float speed;
    private static Vector2[] directions = new Vector2[] {
        Vector2.right, Vector2.up, Vector2.left,Vector2.down
    };
    private bool directional;

    



    public MoveTillCollisionBehavior(GameObject m_gameObject, float speed, bool directional=true)
    {
        rb=m_gameObject.GetComponent<Rigidbody2D>();
        rbSprite=m_gameObject.GetComponent<SpriteRenderer>();
        this.speed = speed;
        this.directional = directional;
    }
   




    public void move()
    {

        int rand = Random.Range(0, directions.Length);
        rb.velocity = directions[rand] * speed;

        if (directional)
        {
            if (rb.velocity.x > 0)
            {
                rbSprite.flipX = true;
            }
            else
            {
                rbSprite.flipX = false;
            }
        }
    }
}
