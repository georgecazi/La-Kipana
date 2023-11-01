using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class healthBAT : MonoBehaviour
{
    ///ENEMYHEALTH
    public int hp = 1;
    private Animator anim;
    public bool dead = false;
    AudioSource playeraudio;
    public AudioClip enemydeathsound;
    private bool coroutine = false;

   
    private void Awake()
    {
        anim = GetComponent<Animator>();
        playeraudio = GetComponent<AudioSource>();

    }

    // Start is called before the first frame update
    void Start()
    {
    }
    IEnumerator EnemyDeath()
    {
        anim.Play("snakedead");
        gameObject.tag = "Untagged";
        playeraudio.clip = enemydeathsound;
        playeraudio.Play();
        yield return new WaitForSeconds(0.05f);
        GetComponent<BoxCollider2D>().enabled = false;
        Destroy(gameObject, anim.GetCurrentAnimatorStateInfo(0).length);
    }
    // Update is called once per frame
    void Update()
    {
        if (hp<=0 && !coroutine)
        {
            coroutine = true;
            StartCoroutine(EnemyDeath());
        }

    }
    public void Damage()
    {
        this.hp -= 1;
    }
}

