using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonControls : MonoBehaviour
{
    [SerializeField]
    private GameObject balloon;

    [SerializeField]
    private int moveSpeed = 1;

    // Directions
    private bool isMovingRight = false;
    private bool isMovingUp = false;
    private bool isMovingDown = false;
    private bool isMovingLeft = false;
    private bool isMovingBack = false;
    private bool isMovingForward = false;


    void Update()
    {
        KeyboardControls();

        Vector3 moveDirection = Vector3.zero;
        if (isMovingRight) moveDirection += RIGHT;
        if (isMovingUp) moveDirection += Vector3.up;
        if (isMovingDown) moveDirection += Vector3.down;
        if (isMovingLeft) moveDirection += LEFT;
        if (isMovingBack) moveDirection += BACK;
        if (isMovingForward) moveDirection += FORWARD;

        Move(moveDirection);
    }

    public void StartMovingRight() => isMovingRight = true;
    public void StopMovingRight() => isMovingRight = false;

    public void StartMovingUp() => isMovingUp = true;
    public void StopMovingUp() => isMovingUp = false;

    public void StartMovingDown() => isMovingDown = true;
    public void StopMovingDown() => isMovingDown = false;

    public void StartMovingLeft() => isMovingLeft = true;
    public void StopMovingLeft() => isMovingLeft = false;

    public void StartMovingBack() => isMovingBack = true;
    public void StopMovingBack() => isMovingBack = false;

    public void StartMovingForward() => isMovingForward = true;
    public void StopMovingForward() => isMovingForward = false;

    // finish for all directions

    private void KeyboardControls()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow)) // Detect right arrow key press
        {
            isMovingRight = true;
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow)) // Detect right arrow key release
        {
            isMovingRight = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            isMovingLeft = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            isMovingLeft = false;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            isMovingUp = true;
        }
        else if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            isMovingUp = false;
        }

        // Detect down arrow key press and release
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            isMovingDown = true;
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            isMovingDown = false;
        }

        // Detect back arrow key press and release
        if (Input.GetKeyDown(KeyCode.S))
        {
            isMovingBack = true;
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            isMovingBack = false;
        }

        // Detect forward arrow key press and release
        if (Input.GetKeyDown(KeyCode.W))
        {
            isMovingForward = true;
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            isMovingForward = false;
        }
    }

    private void Move(Vector3 direction)
    {
        Vector3 moveDirection = direction * moveSpeed * Time.deltaTime;
        balloon.transform.Translate(moveDirection);
    }



    private static Vector3 FORWARD = Vector3.right;
    private static Vector3 BACK = Vector3.left;
    private static Vector3 RIGHT = Vector3.back;
    private static Vector3 LEFT = Vector3.forward;
}
