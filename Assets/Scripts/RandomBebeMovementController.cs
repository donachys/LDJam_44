using UnityEngine;
using System.Collections;

public class RandomBebeMovementController : RandomMovementController {
    private float sightRadius = 2.25f;
    private float moveThreshhold = 0.5f;
    protected override void sendInput()
    {
        GameObject player = GameObject.FindGameObjectsWithTag("Player")[0];
        Vector3 diff = player.transform.position - gameObject.transform.position;
        float dist = diff.magnitude;
        if (dist <= sightRadius)
        {
            naiveFleeInput(diff);
        }
        else {
            randomInput();
        }
    }
    protected void naiveFleeInput(Vector3 diff)
    {
        if (Mathf.Abs(diff.x) >= moveThreshhold)
        {
            float x;
            x = diff.x < 0 ? -1 : 1;
            this.inputBroker.setHorizontal(-x);
        } else {
            this.inputBroker.setHorizontal(0);
        }
        if (Mathf.Abs(diff.y) >= moveThreshhold)
        {
            float y;
            y = diff.y < 0 ? -1 :1;
            this.inputBroker.setVertical(-y);
        } else {
            this.inputBroker.setVertical(0);
        }
    }
}