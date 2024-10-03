using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoPlay : MonoBehaviour
{
    public GameObject player; // Assign your player object in the inspector
    public float moveSpeed = 5f; // Speed at which the player moves
    public float safeDistance = 2f; // The safe distance to avoid bombs, but stay close enough for balls

    // Update is called once per frame
    void Update()
    {
        // Find the nearest falling object with the tag "Bomb" or "Ball"
        GameObject fallingObject = FindClosestFallingObject();

        if (fallingObject != null)
        {
            if (fallingObject.CompareTag("Bomb"))
            {
                AvoidBomb(fallingObject);
            }
            else if (fallingObject.CompareTag("Ball"))
            {
                MoveTowards(fallingObject);
            }
        }
    }

    // Function to find the closest object with the "Bomb" or "Ball" tag
    GameObject FindClosestFallingObject()
    {
        GameObject[] bombs = GameObject.FindGameObjectsWithTag("Bomb");
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");

        GameObject closest = null;
        float minDistance = Mathf.Infinity;
        Vector3 currentPosition = player.transform.position;

        // Check for the closest bomb
        foreach (GameObject bomb in bombs)
        {
            float distance = Vector3.Distance(bomb.transform.position, currentPosition);
            if (distance < minDistance)
            {
                closest = bomb;
                minDistance = distance;
            }
        }

        // Check for the closest ball
        foreach (GameObject ball in balls)
        {
            float distance = Vector3.Distance(ball.transform.position, currentPosition);
            if (distance < minDistance)
            {
                closest = ball;
                minDistance = distance;
            }
        }

        return closest;
    }

    // Move the player to avoid the bomb but stay within a "safe distance" for catching balls
    void AvoidBomb(GameObject bomb)
    {
        Vector3 bombPosition = bomb.transform.position;
        Vector3 playerPosition = player.transform.position;

        // Calculate the horizontal distance between the player and the bomb
        float distanceToBomb = Mathf.Abs(playerPosition.x - bombPosition.x);

        // Only move if we are closer than the "safe distance"
        if (distanceToBomb < safeDistance)
        {
            // Move left if the bomb is to the right, or move right if the bomb is to the left
            float moveDirection = playerPosition.x > bombPosition.x ? 1 : -1;
            player.transform.Translate(Vector3.right * moveDirection * moveSpeed * Time.deltaTime);
        }
    }

    // Move the player under the ball (Horizontally)
    void MoveTowards(GameObject ball)
    {
        Vector3 ballPosition = ball.transform.position;
        Vector3 playerPosition = player.transform.position;

        // Only move in the x direction
        float moveDirection = playerPosition.x < ballPosition.x ? 1 : -1;
        player.transform.Translate(Vector3.right * moveDirection * moveSpeed * Time.deltaTime);
    }
}
