using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorScript : MonoBehaviour
{
    public GameObject currentPlanet;
    private bool isOnGround = false;
    private float hitTime;
    private float maxTimeInGround = 4;

    Vector3 tempSize;

    void Update()
    {
        if (isOnGround && Time.timeScale != 0f)
        {
            float timeInGround = Time.timeSinceLevelLoad - hitTime;

            //the size gets reduced each time
            float factor = 0.00007F * timeInGround;
            tempSize.x = tempSize.x - factor;
            tempSize.y = tempSize.y - factor;
            tempSize.z = tempSize.z - factor;
            transform.localScale = tempSize;
            
            
            
            //the meteor finally dessapears
            if (timeInGround >= maxTimeInGround)
            {
                Destroy(this.gameObject);
            }
        }
    }

    //The planet attracts the meteor
    void FixedUpdate()
    {
        if (!isOnGround) {
            currentPlanet.GetComponent<GravityAttractor>().Attract(this.transform);
        }
        
    }

    //checks if the meteor has hit the ground
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Planeta") && !isOnGround){
            this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            this.isOnGround = true;
            hitTime = Time.timeSinceLevelLoad;

            tempSize = transform.localScale;
        }
    }
}
