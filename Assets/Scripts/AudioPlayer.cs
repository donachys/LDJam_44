using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip[] clips = new AudioClip[10];
    public AudioClip testclip;
    void Start()
    {
        audioSource = GameObject.FindGameObjectsWithTag("Audio")[0].GetComponentInChildren<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void playOuch()
    {
        playSound(0);
    }
    public void playGotBaby()
    {
        playSound(1);
    }
    public void playAhYes()
    {
        playSound(2);
    }
    public void playNooo()
    {
        playSound(3);
    }
    public void playGotCash()
    {
        playSound(4);
    }    private void playSound(int soundNum){
        if (clips [soundNum]) {
            audioSource.clip = clips [soundNum];
            audioSource.Play();
        }
    }
    private void playSound(AudioClip clip){
        audioSource.clip = clip;
        audioSource.Play();
    }
}
