using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIUpdater : MonoBehaviour
{
    public PlayerStats playerStats;
    public Text moneyText;
    public Text followersText;
    public Text gameOverText;
    public Text timerText;

    public void SetTimer(string txt)
    {
        timerText.text = txt;
    }

    public void SetGameOver(string txt)
    {

        gameOverText.text = txt;
    }
    private void SetMoney()
    {
        string text = "0";
        if (playerStats.stats.networth > 0){
            text = string.Format("Money: ${0}", playerStats.stats.networth);
        }
        moneyText.text = text;
    }

    private void SetFollowersText()
    {
        const string vampireColor = "c10000";
        const string batColor = "c15d00";
        const string humanColor = "00b0c1";
        int vampireFontSize = playerStats.stats.form == Form.Vampire ? 16 : 10;
        int batFontSize = playerStats.stats.form == Form.Bat ? 16 : 10;
        int humanFontSize = playerStats.stats.form == Form.Human ? 16 : 10;
        int vampireFollowerCount = playerStats.stats.vampireStats.followerCount;
        int batFollowerCount = playerStats.stats.batStats.followerCount;
        int humanFollowerCount = playerStats.stats.humanStats.followerCount;
        int bill = playerStats.stats.bill;
        float distance = playerStats.stats.distance;

        string text = string.Format(
// <size={0}><color=#{3}>Vampire Followers: {6}</color></size>
// <size={1}><color=#{4}>Bat Followers: {7}</color></size>
// <size={2}><color=#{5}>Human Followers: {8}</color></size>
@"<size={0}><color=#{3}>Vampire Distance: {9:0.##}</color></size>
<size={0}><color=#{3}>Distance Tax: {10}</color></size>",
            vampireFontSize, batFontSize, humanFontSize,
            vampireColor, batColor, humanColor,
            vampireFollowerCount, batFollowerCount, humanFollowerCount,
            distance, bill
        );

        followersText.text = text;
    }

    private void Start()
    {
        SetMoney();
        SetFollowersText();
    }

    void Update()
    {
        SetMoney();
        SetFollowersText();
    }
}
