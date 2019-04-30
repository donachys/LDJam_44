using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollisionPush : MonoBehaviour
{
    private int pushStrength = 15;
    private AudioPlayer audioPlayer;

    void Start()
    {
        audioPlayer = FindObjectOfType<AudioPlayer>();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player"){
            // disable the player's movement and push them back temporarily to indicate
            // something bad happened.
            Vector3 diff = transform.position - col.gameObject.transform.position;
            Rigidbody2D rbody = col.gameObject.GetComponent<Rigidbody2D>();
            PlayerController pcol = col.gameObject.GetComponent<PlayerController>();
            pcol.permitMovement = false;
            Vector2 diff2 = Vector2.ClampMagnitude(new Vector2(diff.x, diff.y), 1);
            diff2.Normalize();
            Vector2 movement = -diff2 * pushStrength;
            Vector2 currentPos = rbody.position;
            Vector2 newPos = currentPos + movement * Time.fixedDeltaTime;
            rbody.MovePosition(newPos);
            audioPlayer.playOuch();
        }
    }
}