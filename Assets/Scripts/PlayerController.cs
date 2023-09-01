using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Vector2 movement;
    Animator animator;
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;

    public ContactFilter2D movementFilter;
    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    private GameObject currentInteractable;
    public Interact interactScript;
    private bool isHoldingItem = false;
    private GameObject heldItem;
    private DoorController currentDoor;

    public AudioSource audioSource;
    public AudioClip footstepSound;
    public AudioClip doorSound;
    public AudioClip doorOpenSound;
    public AudioClip pickupSound;
    public AudioClip dropSound;
    

    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    public float interactionDistance = 1.5f;
    //bool canMove = true;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        interactScript = GetComponent<Interact>();

        if(audioSource == null)
        {
            audioSource = gameObject.GetComponent<AudioSource>();
            if(audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }
    }

    void FixedUpdate()
    {
        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        rb.velocity = movement.normalized * moveSpeed;

        //Boundary check
        Vector2 clampedPosition = rb.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minY, maxY);
        
        if(clampedPosition != rb.position)
        {
             rb.position = clampedPosition;
            rb.velocity = Vector2.zero;
        }
        

       if (movement == Vector2.zero)
        {
            animator.SetBool("isMoving", false);
        }
        else
        {
            // Flip the sprite if we're moving left
            spriteRenderer.flipX = movement.x < 0;
            animator.SetBool("isMoving", true);
            
            if(!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(footstepSound);
            }
        }

       /* if(canMove){
            if(movement != Vector2.zero){
                bool success = TryMove(movement);
                
                if(!success){
                    success = TryMove(new Vector2(movement.x, 0));
                }
                if(!success){
                    success = TryMove(new Vector2(0, movement.y));
                }
                animator.SetBool("isMoving",success);
            } else{
                animator.SetBool("isMoving",false);
            }
            //set direction of sprite to movement direction
            if(movement.x < 0){
                spriteRenderer.flipX = true;
                
            } else if (movement.x > 0){
                spriteRenderer.flipX = false;
                
            }
        }*/

        // Cast a ray in the direction of movement
        /*int count = checkCollisions(movement);
        if (count == 0)
        {
             Debug.Log("No collisions detected for movement: " + movement);
            transform.Translate(movement * Time.deltaTime);
            return;
        }
        else
            {
                for (int i = 0; i < count; i++)
                {
                    Debug.Log("Collided with: " + castCollisions[i].collider.gameObject.name);
                }
            }
        // If we hit something, try moving in the x direction
        count = checkCollisions(new Vector2(movement.x, 0));
        if (count == 0)
        {
            Debug.Log("No collisions detected for x movement: " + movement.x);
            transform.Translate(new Vector2(movement.x, 0) * Time.deltaTime);
            return;
        }
        else
            {
                for (int i = 0; i < count; i++)
                {
                    Debug.Log("Collided with: " + castCollisions[i].collider.gameObject.name);
                }
            }

        // If we hit something, try moving in the y direction
        count = checkCollisions(new Vector2(0, movement.y));
        if (count == 0)
        {
            Debug.Log("No collisions detected for y movement: " + movement.y);
            transform.Translate(new Vector2(0, movement.y) * Time.deltaTime);
            return;
        }
        else
        {
            for (int i = 0; i < count; i++)
            {
                Debug.Log("Collided with: " + castCollisions[i].collider.gameObject.name);
            }
        }*/
    }

    void OnMove(InputValue movementValue)
    {
        movement = movementValue.Get<Vector2>();
    }

    /*private bool TryMove(Vector2 direction) {

        if(direction != Vector2.zero) {
         int count = rb.Cast(
                direction,
                movementFilter,
                castCollisions,
                moveSpeed * Time.fixedDeltaTime + collisionOffset);
            Debug.DrawRay(rb.position, direction * (moveSpeed * Time.fixedDeltaTime + collisionOffset), Color.red);

            if(count == 0){
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
            return true;
        }else{
            return false;
        }
        }
        else{
            return false;
        }
    
    }*/

    int checkCollisions(Vector2 direction)
    {
        int count = rb.Cast(
            direction,
            movementFilter,
            castCollisions,
            moveSpeed * Time.deltaTime + collisionOffset
        );
        Debug.DrawRay(rb.position, direction * (moveSpeed * Time.fixedDeltaTime + collisionOffset), Color.red); 
        //return count;

            // Debug: Print information about the raycast
        Debug.Log("Raycast Direction: " + direction);
        Debug.Log("Raycast Origin: " + rb.position);
        Debug.Log("Raycast Hit Count: " + count);

        if (count > 0)
        {
            for (int i = 0; i < count; i++)
            {
                Debug.Log("Raycast Hit " + i + ": " + castCollisions[i].collider.gameObject.name);
            }
        }

        return count;
    }

   

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isHoldingItem)
            {    //if character is holding the key and interact with the door
                if (currentInteractable != null && currentInteractable.CompareTag("Door"))
                {
                    currentDoor = currentInteractable.GetComponent<DoorController>();
                    if (currentDoor != null)
                    {
                        if (currentDoor.RequiresKey())  
                        {
                            if (isHoldingItem && heldItem.CompareTag("Key")) //check if the key is with the player before opening the door and play sound
                            {
                                OpenDoorWithKey(currentDoor);
                                audioSource.PlayOneShot(doorOpenSound); 
                            }
                            else
                            {   //if player has no key
                                Debug.Log("You need a key to open this door.");
                            }
                        }
                        else //if player has no key
                        {
                            currentDoor.ToggleDoor();
                            audioSource.PlayOneShot(doorSound); 
                        }
                    }
                }
                else
                {
                    audioSource.PlayOneShot(dropSound);
                    interactScript.DropItem(ref isHoldingItem, ref heldItem);
                    
                }
            }
            else if (currentInteractable != null)
            {
                if (currentInteractable.CompareTag("Door"))
                {
                    interactScript.InteractWithObject(currentInteractable, ref isHoldingItem, ref heldItem);
                    audioSource.PlayOneShot(doorSound);
                }
                else if (currentInteractable.CompareTag("Key"))
                {
                    audioSource.PlayOneShot(pickupSound);
                    interactScript.InteractWithObject(currentInteractable, ref isHoldingItem, ref heldItem);
                }         
            }
            else
            {
                Debug.Log("No interactable object nearby.");
            }
        }
    }

    private void OpenDoorWithKey(DoorController door)
    {
        if (!door.isDoorOpen)
        {
            door.isDoorOpen = true;
            // Implement the door opening logic here.
            Debug.Log("Opening the door with the key.");
        }
        else
        {
            Debug.Log("The door is already open.");
        }
    }
 

    void InteractWithObject()
    {
        interactScript.InteractWithObject(currentInteractable, ref isHoldingItem, ref heldItem);
    }

    void DropItem()
    {
        interactScript.DropItem(ref isHoldingItem, ref heldItem);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Door") || other.CompareTag("Key"))
        {
            Debug.Log("Player entered the trigger of: " + other.gameObject.name);
            currentInteractable = other.gameObject;
           
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == currentInteractable)
        {
            Debug.Log("Player exit the trigger of: " + other.gameObject.name);
            currentInteractable = null;
        }
    }
}
