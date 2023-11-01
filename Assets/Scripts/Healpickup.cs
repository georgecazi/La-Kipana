using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healpickup : MonoBehaviour
{
    AudioSource playeraudio;
    //public AudioClip healsfx;
    public AudioSource healsfx;
    PlayerHealth playerHealth;


    public void Playhealsfx()
    {
        healsfx.Play();
    }
    // Start is called before the first frame update
    void Awake()
    {
        playeraudio = GetComponent<AudioSource>();
        playerHealth = FindObjectOfType<PlayerHealth>();

    }
    
    
    
}
