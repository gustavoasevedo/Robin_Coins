using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScript : MonoBehaviour
{
    [SerializeField] LevelLoader levelLoader;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Start"))
        {
            GetComponent<AudioSource>().Play();
            levelLoader.loadNextLevel(1);
        }
    }
}
