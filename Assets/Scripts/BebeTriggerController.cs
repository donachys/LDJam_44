using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BebeTriggerController : MonoBehaviour
{
    public PlayerStats playerStats;
    private float bebeValue;
    private AudioPlayer audioPlayer;
    void Start()
    {
         bebeValue = (float)GameData.Globals["bebeValue"];
         audioPlayer = FindObjectOfType<AudioPlayer>();
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Nursery")
        {
            playerStats.stats.networth += bebeValue;
            Destroy(gameObject);
            audioPlayer.playGotCash();
        }
    }
}