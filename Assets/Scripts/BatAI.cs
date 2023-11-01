using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BatAI : MonoBehaviour
{
    
    public float TargetRange;
    public Transform Player;
    private Vector2 movement;
    private Rigidbody2D rb;
    public float moveSpeed;
    private SpriteRenderer sprite;
    public Animator anim;
    BoxCollider2D boxCollider;





    private void FixedUpdate()
    {

        Vector3 direction = Player.position - transform.position;
        direction.Normalize();
        movement = direction;
        FindTarget();

        //flip

        if (direction.x > 0f)
        {
            sprite.flipX = true;
        }

        else if (direction.x < 0f)
        {

            sprite.flipX = false;


        }

    }
    private void Update()
    {

    






    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void FindTarget()
    {
        if (Vector3.Distance(transform.position, Player.position) < TargetRange)
        {
            moveCharacter(movement);
        }
       

    }
    void moveCharacter(Vector2 direction)
    {
        rb.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));
    }




}
