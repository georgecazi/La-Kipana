using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WarpZoneTrigger : MonoBehaviour
{
    bool nextlevel = true;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && nextlevel)
        {
            SceneManager.LoadScene(8);

            nextlevel = false;
        }
    }
}
