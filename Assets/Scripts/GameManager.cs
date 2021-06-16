using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameState GAME_STATE = GameState.INIT;
    public static bool jump = false;
    [SerializeField] private Text debugText;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            ScreenCapture.CaptureScreenshot("D:\\onemandrawing.png");
        }
        switch (GAME_STATE)
        {
            case GameState.INIT:
                //debugText.text = "Select a starting point";
                debugText.text = "";
                break;
            case GameState.START:
                debugText.text = "";
                break;
            case GameState.WIN:
                debugText.text = "Good Job!";
                break;
            case GameState.LOSE:
                debugText.text = "Oh No!";
                break;
        }
    }



    public static void StartGame()
    {
        GAME_STATE = GameState.START;
    }

    public static void Win()
    {
        GAME_STATE = GameState.WIN;
    }

    public static void Lose()
    {
        GAME_STATE = GameState.LOSE;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

public enum GameState { INIT, START, WIN, LOSE }