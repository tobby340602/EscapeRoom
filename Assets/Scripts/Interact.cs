using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
     

    public void InteractWithObject(GameObject currentInteractable, ref bool isHoldingItem, ref GameObject heldItem)
    {
        if (currentInteractable != null)
        {
            Debug.Log("Interacting with: " + currentInteractable.name);

            if (currentInteractable.CompareTag("Key"))
            {
                if(isHoldingItem)
                {
                    DropItem(ref isHoldingItem, ref heldItem);
                }
                else
                {
                    PickUpItem(currentInteractable, ref isHoldingItem, ref heldItem);
                }
            }
            else if(currentInteractable.CompareTag("Door"))
            {
                    DoorController doorController = currentInteractable.GetComponent<DoorController>();
                    if (doorController != null)
                {
                    if (doorController.RequiresKey())
                    {
                        if (isHoldingItem && heldItem.CompareTag("Key"))
                        {
                            doorController.ToggleDoor();
                        }
                        else
                        {
                            Debug.Log("You need a key to open this door.");
                        }
                    }
                    else
                    {
                        doorController.ToggleDoor();
                    }
                }
            }
            else
            {
                Debug.Log("currentInteractable not found");
            }
                
            
            }
        }

       

        private bool CheckDoorKeyRequirement(DoorController doorController, bool isHoldingItem, GameObject heldItem)
        {
            if(doorController.requiresKey && isHoldingItem && heldItem.CompareTag("Key"))
            {
                return true;
            }
            else if(!doorController.requiresKey)
            {
                return true;
            }
            else{
                Debug.Log("need key to open door");
                return false;
            }
        } 

    public void PickUpItem(GameObject item, ref bool isHoldingItem, ref GameObject heldItem)
    {
        if(item.CompareTag("Key"))
        {
            Debug.Log("Picking up the item!");
            item.SetActive(false); // Disable the item's GameObject or perform any other desired behavior.

            // Store the item that is being held.
            heldItem = item;
            isHoldingItem = true;
        }

    }

    public void DropItem(ref bool isHoldingItem, ref GameObject heldItem)
    {
        if(heldItem.CompareTag("Key"))
        {
        
            Debug.Log("Dropping the item!");
            heldItem.SetActive(true); // Enable the item's GameObject or perform any other desired behavior.

            // Clear the reference to the held item.
            heldItem = null;
            isHoldingItem = false;

       
        }
    }
}
