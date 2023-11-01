using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    [Range (0, 30)] public int maxHealth = 10;
    [Range (0, 10)] public float currentHealth;
    public bool maxed;
    public AudioClip healsfx;
    AudioSource playeraudio;
    private LevelLoader levelLoader;
    public AudioClip deathchant;
    public Animator anim;
    public bool playerdead = false;

    private void Update()
    {
        
    }
    private void Start()
    {
        currentHealth = maxHealth;
        levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
        


    }
    private void Awake()
    {
        anim = GetComponent<Animator>();
        playeraudio = GetComponent<AudioSource>();

    }

    IEnumerator Death()
    {
        Time.timeScale = 0;
        playerdead = true;
        levelLoader.ReloadLevel();
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 1;

        playerdead = false;
        gameObject.SetActive(false);


    }
    public void TakeDamage(int amount)
    { 
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            anim.SetBool("IsHit", true);
            StartCoroutine(Death());

            
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
            maxed = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "HEAL")
        {
            if (currentHealth < 10f)
            {
                playeraudio.clip = healsfx;
                playeraudio.Play();
                Heal(6);
                Destroy(collision.gameObject);

            }

        }
    }

    }
