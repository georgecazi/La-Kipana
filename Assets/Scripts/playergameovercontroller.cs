using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playergameovercontroller : MonoBehaviour
{ 




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
    public Animator transanim;
    private SpriteRenderer sprite;
    BoxCollider2D boxCollider;
    [SerializeField] private LayerMask groundLayer = ~0;
    [Range(0, 1)] [SerializeField] private float floorOffset = 0.4f;
    bool grounded = false;
    public Transform attackpoint;
    public float attackrange = 0.5f;
    public LayerMask enemylayers;
    public LayerMask bosslayers;
    AudioSource playeraudio;
    public AudioClip whip;
    public AudioClip hurt;
    private LevelLoader levelLoader;
    public bool hit = false;
    bool canmove = true;
    public AudioClip chadtheme;
   




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
        DontDestroyOnLoad(transform.gameObject);



    }
    private void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        if (canmove)
        {
            rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);

        }







    }
    IEnumerator Chadification()
    {
        canmove = false; 
        playeraudio.clip = chadtheme;
        playeraudio.Play();
        anim.Play("chadification");
        yield return new WaitForSecondsRealtime(2f);
        anim.Play("chadidle");
        yield return new WaitForSecondsRealtime(1f);
        transanim.SetTrigger("fade");
        yield return new WaitForSecondsRealtime(5f);
        SceneManager.LoadScene(7);

    }
  
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "CHADALICE")
        {
            rb.velocity = new Vector2(0, 0);
            Destroy(collision.gameObject);
            StartCoroutine(Chadification());
        }

        
    }
    void Update()
    {







        

        

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

        if (horizontalInput > 0.01f && canmove)
        {
            sprite.flipX = false;
        }

        else if (horizontalInput < -0.01f && canmove)
        {
            sprite.flipX = true;
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






