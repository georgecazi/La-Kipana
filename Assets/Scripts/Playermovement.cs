using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.InputSystem;



public class Playermovement : MonoBehaviour
{
    PlayerControls controls;
    [SerializeField] private float speed;
    public float knockBackMultiplier = 200f;
    health health;
    //public static Playermovement Instance { get; private set; }
    public float jumpforce;
    private float jumptimecounter;
    public float jumptime;
    private bool isjumping;
    private Rigidbody2D rb;
    public Animator anim;
    private SpriteRenderer sprite;
    BoxCollider2D boxCollider;
    [SerializeField] private LayerMask groundLayer = ~0;
    [Range(0, 1)] [SerializeField] private float floorOffset = 0.4f;
    bool grounded = false;
    public Transform attackpoint;
    public float attackrange = 0.5f;
    public LayerMask enemylayers;
    public LayerMask bosslayers;
    public LayerMask batLayers;
    AudioSource playeraudio;
    public AudioClip whip;
    public AudioClip hurt;
    private LevelLoader levelLoader;
    public bool hit = false;
    public void Hit()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackpoint.position, attackrange, enemylayers);
        //Damage enemies oh yeah
        foreach (Collider2D enemy in hitEnemies)
        {
            health vida;
            enemy.gameObject.TryGetComponent<health>(out vida);
            vida.Damage();
            

        }
        Collider2D[] hitBoss = Physics2D.OverlapCircleAll(attackpoint.position, attackrange, bosslayers);
        foreach (Collider2D boss in hitBoss)
        {
            BossScript bossvida;
            boss.gameObject.TryGetComponent<BossScript>(out bossvida);
            bossvida.BossDamage();
        }

        Collider2D[] hitBats = Physics2D.OverlapCircleAll(attackpoint.position, attackrange, batLayers);
        //Damage enemies oh yeah
        foreach (Collider2D bat in hitBats)
        {
            healthBAT vidaBAT;
            bat.gameObject.TryGetComponent<healthBAT>(out vidaBAT);
            vidaBAT.Damage();


        }

        



    }




    private void Start()
    {
        levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        playeraudio = GetComponent<AudioSource>();
        //health = GetComponent<PlayerHealth>();
        controls = new PlayerControls();    


    }
    private void FixedUpdate()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
        jumptimecounter -= Time.deltaTime;








    }

    IEnumerator Knockback(Vector2 direction)
    {
        anim.SetBool("IsHit", true);
        rb.velocity = new Vector2(0, 0);
        rb.AddForce(direction.normalized * new Vector2(knockBackMultiplier * 15f, knockBackMultiplier));

        yield return new WaitForSeconds(0.15f);
        anim.SetBool("IsHit", false);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log(collision.gameObject.name);
        GameObject collisionObject = collision.gameObject;

        if (collisionObject.tag == "enemy" && anim.GetBool("IsHit") == false)
        {
            var healthComponent = GetComponent<PlayerHealth>();
            healthComponent.TakeDamage(2);
            playeraudio.clip = hurt;
            playeraudio.Play();



            Vector2 knockBack = (this.transform.root.position - collisionObject.transform.root.transform.position);
            knockBack.y = Mathf.Clamp(knockBack.y, 1, 1);
            StartCoroutine(Knockback(knockBack));
        }
    }
    void OnTriggerEnter2D(Collider2D trigger)
    {
        //Debug.Log(trigger.gameObject.name);
        GameObject collisionObject = trigger.gameObject;

        if (trigger.CompareTag("Boss") && anim.GetBool("IsHit") == false)
        {
            var healthComponent = GetComponent<PlayerHealth>();
            healthComponent.TakeDamage(2);
            playeraudio.clip = hurt;
            playeraudio.Play();



            Vector2 knockBack = (this.transform.root.position - collisionObject.transform.root.transform.position);
            knockBack.y = Mathf.Clamp(knockBack.y, 1, 1);
            StartCoroutine(Knockback(knockBack));
        }
    }
    public void Attack(InputAction.CallbackContext context)
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("whip"))
        {
            //play attack animation
            anim.SetBool("Attack", true);
            playeraudio.clip = whip;
            playeraudio.Play();






        }
    }
    void Update()
    {


        Debug.Log(jumptimecounter);




        if (hit)
        {
            Hit();
        }

        if (Input.GetKeyDown("r") || gameObject.transform.position.y <= -20)
        {
            levelLoader.ReloadLevel();
        }

        grounded = OnGround();
        anim.SetFloat("yvelocity", rb.velocity.y);

        float horizontalInput = Input.GetAxis("Horizontal");

        if (grounded)
        {

            anim.SetBool("walk", horizontalInput != 0);
        }
        else
        {
            anim.SetBool("jump", true);
        }



        anim.SetBool("Attack", false);



        //flip

        if (horizontalInput > 0.01f)
        {
            sprite.flipX = false;
            attackpoint.transform.localPosition = new Vector3(0.656f, -0.051f, 0f);
        }

        else if (horizontalInput < -0.01f)
        {
            sprite.flipX = true;
            attackpoint.transform.localPosition = new Vector3(-0.656f, -0.051f, 0f);
        }

        

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("whip"))
        {
            anim.SetBool("isattacking", true);
            //Detect enemies in range of attack

            //yield return new WaitForSeconds(0.15f);
            //Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackpoint.position, attackrange, enemylayers);
            ////Damage enemies
            //foreach (Collider2D enemy in hitEnemies)
            //{
            //    Debug.Log("We hit" + enemy.name);
            //    health vida;
            //    enemy.gameObject.TryGetComponent<health>(out vida);
            //    vida.Damage();
            //}

        }

        else
        {
            anim.SetBool("isattacking", false);
        }


        //Jump

        if (Input.GetButtonDown("Jump") && grounded)
        {
            isjumping = true;
            jumptimecounter = jumptime;
            rb.velocity = new Vector2(rb.velocity.x, jumpforce * 2);

        }
        if (Input.GetButtonDown("Jump") && isjumping == true)
        {
            if (jumptimecounter > 0)
            {
                rb.velocity = Vector2.up * jumpforce * 2;
            }
            else
            {
                isjumping = false;
            }
        }

        if (Input.GetButtonUp("Jump"))
        {
            isjumping = false;
        }









    }
    void OnDrawGizmosSelected()
    {
        if (attackpoint == null)
            return;
        Gizmos.DrawWireSphere(attackpoint.position, attackrange);
    }





    bool OnGround() //Check if Player is OnGround
    {
        RaycastHit2D hit0 = Physics2D.Raycast(boxCollider.bounds.center + new Vector3(boxCollider.bounds.extents.x, 0, 0), Vector2.down, boxCollider.bounds.extents.y + floorOffset, groundLayer);
        RaycastHit2D hit1 = Physics2D.Raycast(boxCollider.bounds.center - new Vector3(boxCollider.bounds.extents.x, 0, 0), Vector2.down, boxCollider.bounds.extents.y + floorOffset, groundLayer);

        if (hit0.collider != null || hit1.collider != null)
        {
            anim.SetBool("jump", false);
            return (true);
        }
        else
        {
            anim.SetBool("walk", false);
            return (false);
        }
    }


}





