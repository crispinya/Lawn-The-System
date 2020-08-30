using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegeneratePlants : MonoBehaviour
{
    private float stopwatch;
    private bool isActive;
    private float regenerationTime = 35; 

    void Start()
    {
        stopwatch = 0;
        isActive = true;
    }

    void Update()
    {
        if (isActive && !this.gameObject.GetComponent<MeshRenderer>().enabled)
        {
            isActive = false;
            stopwatch = Time.time;
        }

        //the grass reapears after some seconds
        if (!isActive && ((Time.time - stopwatch) >= regenerationTime))
        {
            this.gameObject.GetComponent<MeshRenderer>().enabled = true;
            isActive = true;
        }
    }
}
