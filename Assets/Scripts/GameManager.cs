using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    bool gameHasEnded = false;
    bool levelCompleted = false;

    public GameObject completeLevelUI;
    public GameObject gameOverUI;

    public void CompleteLevel()
    {
        completeLevelUI.SetActive(true);
        levelCompleted = true;
        Debug.Log("LEVEL COMPLETE!");
    }

    public void EndGame()
    {
        if (levelCompleted == true)
        {
            gameHasEnded = true;
        }

        if (gameHasEnded == false)
        {
            gameHasEnded = true;
            gameOverUI.SetActive(true);
            Debug.Log("GAME OVER!");
        }
    }
}
