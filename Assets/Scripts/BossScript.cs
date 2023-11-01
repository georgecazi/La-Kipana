using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class BossScript : MonoBehaviour
{
    public Transform Player;
    private SpriteRenderer sprite;
    public float timer;
    float jumptimer = 0.5f;
    Rigidbody2D rb;
    Transform GameObject;
    public float jumpvel;
    public int hp = 10;
    private Animator anim;
    AudioSource playeraudio;
    public AudioClip wurubudefeat;
    public AudioClip wurubugah;
    private bool bossdead;
    float deathtimer = 2.5f;
    bool invincible = false;
    public Transform hitbox;







    // Start is called before the first frame update
    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        GameObject = GetComponent<Transform>();
        anim = GetComponent<Animator>();
        playeraudio = GetComponent<AudioSource>();
    }
    private void FixedUpdate()
    {
        //flip
        Vector3 direction = Player.position - transform.position;
        direction.Normalize();

        if (direction.x > 0f && !bossdead)
        {
            sprite.flipX = true;
        }
        else if (direction.x < 0f && !bossdead)
        {
            sprite.flipX = false;
        }
        //timer, teleport then push rb up
        if (timer <= 0f && !bossdead)
        {
            //moveposition
            playeraudio.clip = wurubugah;
            playeraudio.Play();
            rb.velocity = new Vector2(0, jumpvel);
            anim.Play("bossidle");
            jumptimer -= Time.deltaTime;
            if (jumptimer <= 0f)
            {
                //tp
                transform.position = new Vector2(Player.position.x, 7f);
                rb.velocity = new Vector2(0, 0);
                invincible = false;
                //timer reset
                timer = 2.5f;
                jumptimer = 0.5f;
            }
            
        }
        else
        {
            timer -= Time.deltaTime;
            
        }
        if (hp <= 0f && !bossdead)
        {
            bossdead = true;
            deathtimer = 3f;
            gameObject.tag = "Untagged";
            Destroy(hitbox.gameObject);
            playeraudio.clip = wurubudefeat;
            playeraudio.Play();

        }
        deathtimer -= Time.deltaTime;
    }
    IEnumerator waitandload()
    {
        yield return new WaitForSecondsRealtime(5f);
        SceneManager.LoadScene(6);

    }
    void Update()
    {
        if (deathtimer <= 0f && bossdead)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Animator>().enabled = false;
            StartCoroutine(waitandload());


            //Destroy(gameObject);
        }
        if (bossdead)
            {
            anim.Play("bossdefeat");

        }

    }
    public void BossDamage()
    {
        if (hp != 0 && !invincible)
        //bossdamagetimer >= 0.3f && 
        {
            invincible = true; 
            this.hp -= 1;
            anim.Play("bossdefeat");
        }
        
    }


}
