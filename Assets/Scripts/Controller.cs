using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour
{
    public Button exit;
    public Button play;
    public GameObject carvas;
    public GameObject main;
    public Text result;
    public static bool BlueLose = false;
    public static bool RedLose = false;
    private string text;
    private bool start = true;

    void Start()
    {
        if (text != null) result.text = text;
        play.onClick.AddListener(Play);
        exit.onClick.AddListener(Exit);
    }

    void Update()
    {
        if (BlueLose) Win();
        if (RedLose) Lose();
    }

    void Play()
    {
        if (start == true) SceneManager.LoadScene("Scene_2"); 
        start = true;
        Time.timeScale = 1;
    }

    void Exit()
    {
        Application.Quit();
    }

    public void Win()
    {
        carvas.SetActive(true);
        main.SetActive(false);
        text = "You win";
        BlueLose = false;
        RedLose = false;
        result.text = text;
        Time.timeScale = 0;
    }

    public void Lose()
    {
        carvas.SetActive(true);
        main.SetActive(false);
        text = "You lose";
        BlueLose = false;
        RedLose = false;
        result.text = text;
        Time.timeScale = 0;
    }
}
