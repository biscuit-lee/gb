// director is a sciprt that determines where and what the AI would do.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
 
public class Director : MonoBehaviour
{
 
    private enum State
    {
        Hunting,
        Investigate,
        Scatter,
    }
 
    private State state;
 
    public Transform player;
    public float moveSpeed = 3f;
    private Rigidbody2D rb;
    private Vector3 movement;
    private double distance;
    float x_pos;
    float y_pos;
    float esx_pos;
    float esy_pos;
    Vector3 randPos;
    Vector3 randPos2;
    // roaming position for ai 
    private Vector3 roamingPos;
    bool moving;
    bool moving2;
    bool isGauge;
    bool roaming = true;
    // prioritize the run away function
    float gaugeCountdown = 0;
    bool spotted; // if the player is spotted
 
 
    // time to measure how long have ai and player been near each other
    float gaugeTime = 0;
 
 
    // Start is called before the first frame update
    void Start()
    {
        // declare the ai rigidbody
        rb = this.GetComponent<Rigidbody2D>();
        roamingPos = Getroamingposition();
 
 
    }
 
    void Awake()
    {
        // hunting is the default state
       // state = State.Hunting;
    }
 
    // Update is called once per frame
    void Update()
    {
 
        // random position near the player 
        x_pos = UnityEngine.Random.Range(player.position.x - 10, player.position.x + 10);
        y_pos = UnityEngine.Random.Range(player.position.z - 10, player.position.z + 10);
 
        //random position away from player
        esx_pos = UnityEngine.Random.Range(player.position.x + 15, player.position.x + 25);
        esy_pos = UnityEngine.Random.Range(player.position.z + 15, player.position.z + 25);
 
        //calcualte distance between player and AI
        distance = System.Math.Sqrt(System.Math.Pow(System.Math.Abs(player.position.z - transform.position.z), 2) + System.Math.Pow(System.Math.Abs(player.position.x - transform.position.x), 2));
        Vector3 direction = player.position - transform.position;
        //angle determine where the ai will be facing
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //rb.rotation = angle;
        direction.Normalize();
        movement = direction;
 
        // state machine
        /*
        switch (State)
        {
            default:
            case state.Hunting:
                Debug.Log("Hunting");
                break;
            case state.Investigate:
                Debug.Log("Investigating");
                break;
        }
 
        */
        // roaming around funciton
 
        if (roaming && (Vector3)transform.position != roamingPos && !moving &&!moving2 &&!isGauge &&!spotted)
        {
            transform.position = Vector3.MoveTowards(transform.position, roamingPos, moveSpeed * Time.deltaTime);
 
 
        }
 
        else
        {
 
            roamingPos = Getroamingposition();
        }
 
 
        // if ai see the player 
 
        if (distance < 2)
        {
            spotted = true;
        }
 
        // prioritize 1; spotted
        if (spotted && (Vector3)transform.position != (Vector3)player.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
        else
        {
            spotted = false;
        }
 
 
 
 
 
        // if player and ai are too far, get a random point around the player for the ai to go to
        if (distance > 30 )//&& gaugeCountdown <= 0)
        {
            randPos = new Vector3(x_pos, 0,y_pos);
            moving = true;
            //isGauge = false;
            //gaugeCountdown = 10;
 
        }
        // move ai to that position
        if (moving && (Vector3)transform.position != randPos &&!moving2 &&!isGauge &&!spotted)
        {
            transform.position = Vector3.MoveTowards(transform.position, randPos, moveSpeed * Time.deltaTime);
        }
        else
        {
            moving = false;
        }
 
 
 
 
 
 
        // MEANCE GAUGE
 
        // if player and ai is in close proximity, set a timer
        if (distance <= 10)
        {
            randPos2 = new Vector3(esx_pos, 0,esy_pos);
            gaugeTime += Time.deltaTime;
 
        }
 
        // gauge time measure how long player and ai been near each other
        if (gaugeTime > 10)
        {
            moving2 = true;
            gaugeTime = 0;
            isGauge = true;
        }
 
 
 
 
 
        // if the timer (gaugeTime) is more than 20 sec, then send the ai to a further 
        if (moving2 && (Vector3)transform.position != randPos2 &&!spotted)
        {
 
            //Debug.Log("RUN");
 
            transform.position = Vector3.MoveTowards(transform.position, randPos2, moveSpeed * Time.deltaTime);
            //reset timer
 
 
        }
 
        else
        {
            moving2 = false;
 
        }
 
        if (isGauge)
        {
            gaugeCountdown += Time.deltaTime;
        }
 
        // give player 20s rest
 
        if (gaugeCountdown > 20){
            isGauge = false;
            gaugeCountdown = 0;
        }
 
        //Debug.Log(gaugeCountdown);
 
 
 
 
 
 
 
        //giveHints(distance);
        //Debug.Log(distance);
    }
 
    /*  void giveHints(double distance)
      {
          // check if the ai and player is too far away from each other
 
          if (distance > 10)
          {
 
 
              Debug.Log("HE'S GETTING AWAY BOI");
          }
      }
    */
    /*
      private void FixedUpdate()
      {
          // check if the ai and player is too far away from each other
 
          if (distance > 20)
          {
 
 
          Debug.Log("HE'S GETTING AWAY BOI");
          }
 
          moveCharacter();
 
      }
    */
    void moveCharacter(Vector3 destination)
    {
        // get random position in the radius of the player
 
        // move to the random generated position around the player
        rb.MovePosition((Vector3)transform.position + (destination * moveSpeed * Time.deltaTime));
 
    }
 
    Vector3 Getroamingposition()
    {
        Vector3 randDir = new Vector3(UnityEngine.Random.Range(-1f, 1f), 0,UnityEngine.Random.Range(-1f, 1f)).normalized;    
        return (Vector3)transform.position + randDir * UnityEngine.Random.Range(0f,10f);
 
    }
 
 
}