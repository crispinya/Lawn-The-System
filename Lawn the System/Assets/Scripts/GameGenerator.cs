using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGenerator : MonoBehaviour
{

    public GameObject planet;

    private void Awake()
    {
        //Creates the planets in the game
        int cuantity = Random.Range(5, 10);
        for (int i = 0; i < cuantity; i++)
        {
            Vector3 spawnVector = Random.onUnitSphere * (120) + this.transform.position;
            Instantiate(planet, spawnVector, Random.rotation, this.transform);
        }

    }
}
