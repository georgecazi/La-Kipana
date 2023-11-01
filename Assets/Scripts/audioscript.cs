using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioscript : MonoBehaviour
{
    public PlayerHealth playerhealth;
    public AudioClip deathchant;
    AudioSource playeraudio;

    void Start()
    {
        
    }
    private void Awake()
    {
        playeraudio = GetComponent<AudioSource>();
        DontDestroyOnLoad(transform.gameObject);

    }
    // Update is called once per frame
    void Update()
    {
        if (playerhealth.playerdead == true)
        {
            playeraudio.clip = deathchant;
            playeraudio.Play();
        }
            
    }
}
