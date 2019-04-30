using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector3 moveDirection = Vector3.zero;
    private float carryDistanceX = 0.5f; //weapon carry proximity X
    private float carryDistanceY = 0.7f; //weapon carry proximity Y
    private float moveThreshhold = 0.1f; //help determine where player is facing
    public float movementSpeed = 3f; //how fast would you like to go
    public float torque = 0.5f; //toss weapons with a little torque
    private GameObject carryItem = null; //open carry?
    private float pickupDistance = 0.6f; //proximity to a weapon/pickup item

    public bool permitMovement = true; //false after bumping into an enemy
    private float timeElapsed = 0.0f; //cooldown timer
    private float cooldown = 0.4f; //before re-enabling movement

    // movement tax
    private float baseRate = 10;
    private float taxRate = 0.30f;
    private float taxMod = 1.0f;
    private float distanceMovedSinceTax = 0.0f;
    private float taxTimeElapsed = 0.0f;
    private float taxCooldown = 10.0f;
    private float startingWorth = 100;
    Rigidbody2D rbody;
    public enum Side{N, S, E, W, NW, SW, NE, SE}
    private Side side;
    private AudioPlayer audioPlayer;
    public PlayerStats playerStats;
    void Awake()
    {
        playerStats.stats.networth = startingWorth;
        playerStats.stats.distance = 0;
        rbody = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        baseRate = (float)GameData.Globals["baseRate"];
        taxRate = (float)GameData.Globals["taxRate"];
        taxMod = (float)GameData.Globals["taxMod"];
        startingWorth = (float)GameData.Globals["startingWorth"];
        side = Side.W;
        audioPlayer = FindObjectOfType<AudioPlayer>();
    }
    public void Drop()
    {
        Debug.Log("dropped");
        if (carryItem)
        {
            Collider2D cicc = carryItem.GetComponent<Collider2D>();
            cicc.enabled = true;
            Rigidbody2D rb = carryItem.GetComponent<Rigidbody2D>();
            if (carryItem.tag == "Weapon") rb.freezeRotation = false;
            carryItem = null;
        }
    }
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector2 inputVector = new Vector2(horizontalInput, verticalInput);
        inputVector = Vector2.ClampMagnitude(inputVector, 1);
        moveDirection = new Vector3(horizontalInput, verticalInput, 0.0f);
        if (moveDirection.x > moveThreshhold)
        {
            if (moveDirection.y > moveThreshhold)
            {
                side = Side.NE;
            } else if (moveDirection.y < -moveThreshhold)
            {
                side = Side.SE;
            } else
            {
                side = Side.E;
            }
        } else if (moveDirection.x < -moveThreshhold)
        {
            if (moveDirection.y > moveThreshhold)
            {
                side = Side.NW;
            } else if (moveDirection.y < -moveThreshhold)
            {
                side = Side.SW;
            } else
            {
                side = Side.W;
            }
        } else if (moveDirection.y > moveThreshhold)
        {
            side = Side.N;
        } else if (moveDirection.y < -moveThreshhold)
        {
            side = Side.S;
        }
        SpriteRenderer srenderer = GetComponent<SpriteRenderer>();
        if (horizontalInput < 0)
        {
            srenderer.flipX = false;
        } else if (horizontalInput > 0)
        {
            srenderer.flipX = true;
        }
        if (Input.GetButtonDown("Fire1") && carryItem == null)
        {
            GameObject[] weapons = GameObject.FindGameObjectsWithTag("Weapon");
            GameObject[] bebes = GameObject.FindGameObjectsWithTag("Bebe");
            GameObject[] pickups = new GameObject[weapons.Length + bebes.Length];
            weapons.CopyTo(pickups, 0);
            bebes.CopyTo(pickups, weapons.Length);

            if (pickups.Length > 0)
            {
                int nearestIndex = 0;
                float minDistSoFar = float.MaxValue;
                for (int i = 0; i < pickups.Length; i++)
                {
                    GameObject pickup = pickups[i];
                    float dist = Vector3.Distance(transform.position, pickup.transform.position);
                    if (dist < minDistSoFar)
                    {
                        minDistSoFar = dist;
                        nearestIndex = i;
                    }
                }
                if (minDistSoFar <= pickupDistance) {
                    Debug.Log("picking up");
                    carryItem = pickups[nearestIndex];
                    carryItem.GetComponent<ThrownItem>().SetHolder(this);
                    Rigidbody2D rb = carryItem.GetComponent<Rigidbody2D>();
                    rb.freezeRotation = true;
                    if (carryItem.tag == "Bebe")
                    {
                        audioPlayer.playGotBaby();
                    } else if (carryItem.tag == "Weapon")
                    {
                        audioPlayer.playAhYes();
                    }
                }
            }
        } else if (Input.GetButtonDown("Fire1") && carryItem != null) {
            Debug.Log("released");
            Collider2D cicc = carryItem.GetComponent<Collider2D>();
            cicc.enabled = true;
            carryItem.GetComponent<ThrownItem>().SetThrown();
            Rigidbody2D rb = carryItem.GetComponent<Rigidbody2D>();
            if (carryItem.tag == "Weapon")
            {
                rb.AddTorque(torque);
                rb.freezeRotation = false;
            }
            rb.AddForce(Vec2() * 5, ForceMode2D.Impulse);
            carryItem = null;
        }
        if (taxTimeElapsed > taxCooldown)
        {
            taxTimeElapsed -= taxCooldown;
            playerStats.stats.networth -= tax();
            distanceMovedSinceTax = 0.0f;
        }
        taxTimeElapsed += Time.deltaTime;
    }
    void FixedUpdate()
    {
        if (permitMovement) {
            Vector2 currentPos = rbody.position;
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector2 inputVector = new Vector2(horizontalInput, verticalInput);
            inputVector = Vector2.ClampMagnitude(inputVector, 1);
            Vector2 movement = inputVector * movementSpeed;
            Vector2 newPos = currentPos + movement * Time.fixedDeltaTime;
            distanceMovedSinceTax += (movement * Time.fixedDeltaTime).magnitude;
            playerStats.stats.distance = distanceMovedSinceTax;
            playerStats.stats.bill = tax();
            rbody.MovePosition(newPos);
        } else if (timeElapsed > cooldown){
            timeElapsed -= cooldown;
            permitMovement = true;
        }
        timeElapsed += Time.fixedDeltaTime;
    }
    int tax()
    {
        return Mathf.FloorToInt(baseRate + distanceMovedSinceTax * taxRate * taxMod);
    }
    void LateUpdate()
    {
        if (carryItem != null) {
            Vector3 offset;
            switch (side)
            {
                case Side.SE:
                    offset = new Vector3(carryDistanceX, -carryDistanceY, 0.0f);
                    break;
                case Side.SW:
                    offset = new Vector3(-carryDistanceX, -carryDistanceY, 0.0f);
                    break;
                case Side.NE:
                    offset = new Vector3(carryDistanceX, carryDistanceY, 0.0f);
                    break;
                case Side.NW:
                    offset = new Vector3(-carryDistanceX, carryDistanceY, 0.0f);
                    break;
                case Side.N:
                    offset = new Vector3(0.0f, carryDistanceY, 0.0f);
                    break;
                case Side.S:
                    offset = new Vector3(0.0f, -carryDistanceY, 0.0f);
                    break;
                case Side.W:
                    offset = new Vector3(-carryDistanceX, 0.0f, 0.0f);
                    break;
                case Side.E:
                    offset = new Vector3(carryDistanceX, 0.0f, 0.0f);
                    break;
                default:
                    offset = new Vector3(carryDistanceX, 0.0f, 0.0f);
                    break;
            }
            carryItem.transform.position = transform.position + offset;
        }
    }
    private Vector2 Vec2()
    {
        Vector2 outvec = Vector2.zero;
        switch(side)
        {
            case Side.N:
                outvec = new Vector2(0, 1);
                break;
            case Side.S:
                outvec = new Vector2(0, -1);
                break;
            case Side.E:
                outvec = new Vector2(1, 0);
                break;
            case Side.W:
                outvec = new Vector2(-1, 0);
                break;
            case Side.SW:
                outvec = new Vector2(-1, -1);
                break;
            case Side.SE:
                outvec = new Vector2(1, -1);
                break;
            case Side.NW:
                outvec = new Vector2(-1, 1);
                break;
            case Side.NE:
                outvec = new Vector2(1, 1);
                break;
        }
        return outvec;
    }
}
