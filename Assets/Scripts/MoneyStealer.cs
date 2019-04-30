using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyStealer : MonoBehaviour
{
    public PlayerStats playerStats;
    private const float cooldown = 0.4f;
    private float timeSince = 0.0f;
    private float maxMoneyToSteal = 5;
    private float totalStolen = 0;
    void Start()
    {
        maxMoneyToSteal = (float)GameData.Globals["maxMoneyToSteal"];
    }

    private bool ready()
    {
        if (timeSince >= cooldown)
        {
            return true;
        }
        return false;
    }
    void Update()
    {
        if (timeSince < cooldown)
        {
            timeSince += Time.deltaTime;
        }
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        steal(col);
    }
    void OnCollisionStay2D(Collision2D col)
    {
        steal(col);
    }
    private void steal(Collision2D col)
    {
        if(ready() && col.gameObject.tag == "Player"){
            timeSince = 0.0f;
            int stealVal = Random.Range(0, (int)maxMoneyToSteal);
            playerStats.stats.networth -= stealVal;
            totalStolen += stealVal;
        }
    }
}
