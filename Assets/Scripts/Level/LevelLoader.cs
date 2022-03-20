using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{

    [SerializeField] Animator transition;
    [SerializeField] float transitionTime = 1;

    internal void loadNextLevel(int levelNumber)
    {
        string level = "";

        switch (levelNumber)
        {
            case 1:
                level = "Phase1";
                break;
            case 2:
                level = "Phase2";
                break;
            case 3:
                level = "Phase3";
                break;
            case 4:
                level = "Phase4";
                break;
            case 5:
                level = "Phase5";
                break;
            case 6:
                level = "Phase6";
                break;
            case 7:
                level = "Phase7";
                break;
            case 8:
                level = "Phase8";
                break;
            case 9:
                level = "Phase9";
                break;
            case 10:
                level = "Phase10";
                break;
        }

        StartCoroutine(loadLevel(level));
    }


    IEnumerator loadLevel(string level)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(level);
    }
}
