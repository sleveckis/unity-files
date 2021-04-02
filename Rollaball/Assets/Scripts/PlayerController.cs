using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 0;
  

    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    

    private Rigidbody rb;

    private float inputX;
    private float inputY;
    private float inputZ;
    private float distToGround;
    private float jumpHeight = 5;
    private float jumpCount;

    private int count;
    private float movementX;
    private float movementY;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
       
        count = 0;
        jumpCount = 0;

        SetCountText();
        winTextObject.SetActive(false);
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 10)
        {
            winTextObject.SetActive(true);
        }
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(inputX, 0.0f, inputY);

        rb.AddForce(movement * speed);
    }


    private bool IsGrounded()
    {
        if (rb.transform.position.y == 0.5)
        {
            jumpCount = 0;
            return true;
            
        } else
        {
            return false;
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        inputX = context.ReadValue<Vector2>().x;
        inputY = context.ReadValue<Vector2>().y;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (IsGrounded()&& context.performed)
        {
            // Do single jump, increase jump count to valid for double jump
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            jumpCount++;
        } else if (jumpCount == 1 && IsGrounded() == false && context.performed)
        {
            // Do doubule jump, increase jump count to invalid for another jump
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            jumpCount++;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;

            SetCountText();
        }
        
    }

}