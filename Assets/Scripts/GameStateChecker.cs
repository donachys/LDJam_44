using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class GameStateChecker : MonoBehaviour
{
    public string loseText = "You ran out of money and died.";
    public string winText = "You are ready for the next level.";
    public PlayerStats playerStats;
    private float countDownTime = 4.0f;
    private GUIUpdater gui;

    void Start()
    {
        gui = FindObjectOfType<GUIUpdater>();
        countDownTime = (float)GameData.Globals["countDownTime"];
    }
    void Update()
    {

        if (playerStats.stats.networth <= 0 || countDownTime <= 0)
        {
            bool lose = playerStats.stats.networth <= 0;
            string gameovertext = lose ? loseText : winText;
            gui.SetGameOver(gameovertext);
            GameObject player = GameObject.FindGameObjectsWithTag("Player")[0];
            player.GetComponent<Animator>().Play("disappear");
        } else {
            gui.SetTimer(string.Format("{0:0.} Sec.", countDownTime));
        }
        countDownTime -= Time.deltaTime;
    }
    public void GameOver()
    {
        Debug.Log(GameData.Globals["level"]);
        bool lose = playerStats.stats.networth <= 0;
        Time.timeScale = 0;
        if (lose)
        {
            Debug.Log("Game over");
        } else
        {
            //load next level
            GameData.IncrementLevel();
            Application.LoadLevel(1);
            Time.timeScale = 1;
        }
        Debug.Log(GameData.Globals["level"]);
    }
    private void _loadLevel(object sender, ElapsedEventArgs e)
    {
        Debug.Log( e);
        Application.LoadLevel(1);
    }
}