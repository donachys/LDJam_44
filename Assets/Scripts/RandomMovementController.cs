using UnityEngine;
using System.Collections;

public class RandomMovementController : MonoBehaviour {

    public float movementSpeed = 1f;
    CardinalRenderController renderer;

    Rigidbody2D rbody;
    private float decisionCadence = 0.8f; //sec
    private float sinceLastDecision = 0.0f;
    private float stateChangeProbability = 0.2f;
    private float maxPressDuration = 2.5f;

    private float horizontalPressDuration = 0.0f;
    private float verticalPressDuration = 0.0f;
    protected InputBroker inputBroker = new InputBroker();

    private void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        renderer = GetComponentInChildren<CardinalRenderController>();
    }
    void FixedUpdate () {
        this.sendInput();
        Vector2 currentPos = rbody.position;
        float horizontalInput = inputBroker.GetAxis("Horizontal");
        float verticalInput = inputBroker.GetAxis("Vertical");
        Vector2 inputVector = new Vector2(horizontalInput, verticalInput);
        inputVector = Vector2.ClampMagnitude(inputVector, 1);
        Vector2 movement = inputVector * movementSpeed;
        Vector2 newPos = currentPos + movement * Time.fixedDeltaTime;
        renderer.SetDirection(movement);
        rbody.MovePosition(newPos);

    }

    protected virtual void sendInput()
    {
        randomInput();
    }
    protected void randomInput()
    {
        if (sinceLastDecision > decisionCadence) {
            bool horizontalChange = Random.Range(0.0f, 1.0f) > stateChangeProbability;
            if (horizontalChange) {
                if (!inputBroker.HorizontalPressed()) {
                    inputBroker.setHorizontal(Random.Range(-1.0f, 1.0f));
                } else {
                    inputBroker.setHorizontal(0.0f);
                }
            }
            bool verticalChange = Random.Range(0.0f, 1.0f) > stateChangeProbability;
            if (verticalChange) {
                if (!inputBroker.VerticalPressed()) {
                    inputBroker.setVertical(Random.Range(-1.0f, 1.0f));
                } else {
                    inputBroker.setVertical(0.0f);
                }
            }
            sinceLastDecision = 0.0f;
        }

        if (horizontalPressDuration > maxPressDuration) {
            inputBroker.setHorizontal(0.0f);
            sinceLastDecision = 0.0f;
        }
        if (verticalPressDuration > maxPressDuration) {
            inputBroker.setVertical(0.0f);
            sinceLastDecision = 0.0f;
        }
    }
    void Update () {
        sinceLastDecision += Time.deltaTime;
        if (inputBroker.HorizontalPressed()) {
            horizontalPressDuration += Time.deltaTime;
        } else {
            horizontalPressDuration = 0;
        }
        if (inputBroker.VerticalPressed()) {
            verticalPressDuration += Time.deltaTime;
        } else {
            verticalPressDuration = 0;
        }
    }
}