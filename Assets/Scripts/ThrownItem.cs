using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownItem : MonoBehaviour
{
    public enum State {Neutral, Thrown, Held};
    private State state;
    private PlayerController holder;
    public void SetHolder(PlayerController player)
    {
        state = State.Held;
        holder = player;
    }
    public void SetThrown()
    {
        state = State.Thrown;
    }
    void Start()
    {
        state = State.Neutral;
    }

    void Update()
    {

    }
    void OnCollisionEnter2D(Collision2D col)
    {
        switch(state) {
            case State.Thrown:
                if (gameObject.tag == "Weapon" && col.gameObject.tag == "Enemy")
                {
                    Destroy(gameObject);
                    CardinalRenderController other = col.gameObject.GetComponent<CardinalRenderController>();
                    other.Die();
                }
                break;
            case State.Held:
                holder.Drop();
                state = State.Neutral;
                break;
            case State.Neutral:
                break;
        }
    }
}
