using UnityEngine;

public class PickUpWateringCan : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        // hi
        if (other.name == "Capsule")
        {
            transform.parent = other.transform;
            
            // Position on the left side of the player
            transform.localPosition = new Vector3(-0.5f, 0, 0);
            
            // Face forward
            transform.localRotation = Quaternion.identity;
        }
    }
    
}
