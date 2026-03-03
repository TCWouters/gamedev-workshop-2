using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float turnSpeed = 180f;

    private float moveInput;
    private float turnInput;

    void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();

        moveInput = input.y; 
        turnInput = input.x;   
    }

    void FixedUpdate()
    {
        transform.Rotate(0f, turnInput * turnSpeed * Time.deltaTime, 0f);

        transform.position += transform.forward * moveInput * moveSpeed * Time.deltaTime;
    }
}
