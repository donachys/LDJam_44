using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PlayerStats", menuName = "Player Stats", order = 51)]
public class PlayerStats : ScriptableObject
{
    public Stats stats;
}

public enum Form
{
    Bat,
    Human,
    Vampire
}

[System.Serializable]
public class VampireStats
{
    public float timeAs;
    public int followerCount;
    public int moneyCount;
}

[System.Serializable]
public struct HumanStats
{
    public float timeAs;
    public int followerCount;
    public int moneyCount;
}

[System.Serializable]
public struct BatStats
{
    public float timeAs;
    public int followerCount;
    public int moneyCount;
}

[System.Serializable]
public struct Stats
{
    public float networth;
    public float distance;
    public int bill;

    public Form form;
    public VampireStats vampireStats;
    public HumanStats humanStats;
    public BatStats batStats;
}
