using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetCreation : MonoBehaviour
{
    void Start()
    {
        if (Random.Range(0, 10) == 0)
        {
            createRedCoinPlanet();
        }
        else
        {
            createNormalPlanet();
        }
    }

    void Update()
    {
        //if the game is not paused
        if(Time.timeScale != 0f)
        {
            createNormalMeteors();
            createMeteorPowerUps();
        }
    }

    private void createNormalPlanet()
    {
        //sets random soil
        switch (Random.Range(0, 2))
        {
            case 0: createSoil("Soil1"); break;
            case 1: createSoil("Soil2"); break;
            case 2: createSoil("Soil3"); break;
        }
        
        createGrass("GrassPlanet", 120, 150);

        //creates random rocks in the planet
        switch (Random.Range(0, 6))
        {
            case 0:
            case 1: break;
            case 2:
                createRocks("rock_b", "rock_d", "rock_g"); break;
            case 3:
                createRocks("rock_h", "rock_k", "rock_n"); break;
            case 4:
                createRocks("rock_f", "rock_i", "rock_l"); break;
            case 5:
                createRocks("rock_e", "rock_o", "rock_j"); break;
            case 6:
                createRocks("rock_c", "rock_m", "rock_p"); break;
        }

        //creates random trees in the planet
        switch (Random.Range(0, 7))
        {
            case 0: break;
            case 1:
                createTrees("Bush01", 3, 6); break;
            case 2:
                createTrees("Bush02", 2, 6); break;
            case 3:
                createTrees("Bush03", 2, 6); break;
            case 4:
                createTrees("Bush04", 2, 6); break;
            case 5:
                createTrees("Bush05", 2, 6); break;
            case 6:
                createTrees("Bush06", 2, 6); break;
            case 7:
                createTrees("Bush07", 2, 6); break;
        }

        //creates the powerups
        if (Random.Range(0, 5) == 0)
        {
            createClones(Resources.Load("PowerUps/LifePW") as GameObject, 1);
        }
        if (Random.Range(0, 2) == 0)
        {
            createClones(Resources.Load("PowerUps/DeadPW") as GameObject, 1);
        }
        
        createCoins(2, 6);
    }

    private void createRedCoinPlanet()
    {
        createSoil("Red");
        createGrass("GrassPlanet", 35, 60);
        //no rocks

        //trees
        createTrees("Red3", 4, 7);
        createTrees("Red4", 3, 7);
    
        if (Random.Range(0, 1) == 0)
        {
            createTrees("Red1", 2, 5);
        }
        else
        {
            createTrees("Red2", 0, 5);
        }

        createCoins(30, 60);
    }

    private void createGrass(string resource, int min, int max)
    {
        createClones(Resources.Load(resource) as GameObject, Random.Range(min, max));
    }

    private void createSoil(string resource)
    {
        this.GetComponent<Renderer>().material = Resources.Load("Soil/" + resource) as Material;
    }

    private void createRocks(string rock1, string rock2, string rock3)
    {
        createClones(Resources.Load("Rocks/" + rock1) as GameObject, Random.Range(0, 3));
        createClones(Resources.Load("Rocks/" + rock2) as GameObject, Random.Range(1, 3));
        createClones(Resources.Load("Rocks/" + rock3) as GameObject, Random.Range(1, 3));
    }

    private void createTrees(string treeType, int min, int max)
    {
        createClones(Resources.Load("Bushes/" + treeType) as GameObject, Random.Range(min, max));
    }

    private void createCoins(int min, int max)
    {
        createClones(Resources.Load("coin") as GameObject, Random.Range(min, max));
    }

    //clones a giving object a given number of times
    private void createClones(GameObject objectToClone, int cuantity)
    {
        for (int i = 0; i < cuantity; i++)
        {
            Vector3 spawnVector = Random.onUnitSphere * ((this.transform.localScale.x / 2)) + this.transform.position;
            GameObject newObject = Instantiate(objectToClone, spawnVector, Quaternion.identity, this.transform);
            newObject.transform.LookAt(this.transform.position);
            newObject.transform.rotation = newObject.transform.rotation * Quaternion.Euler(-90, 0, 0);
        }
    }

    private void createNormalMeteors()
    {
        if (Random.Range(0, 100) == 0)
        {
            createMeteor("Meteor");
        }
    }
    

    private void createMeteorPowerUps()
    {
        int rNumber = Random.Range(0, 500);
        if(rNumber == 0)
        { //Fast Speed Meteor
            createMeteor("Speed Meteor");
        }
    }

    private void createMeteor(string type)
    {
        Vector3 spawnVector = Random.onUnitSphere * ((this.transform.localScale.x * 3)) + this.transform.position;
        GameObject newObject = Instantiate(Resources.Load(type) as GameObject, spawnVector, Quaternion.identity, this.transform);

        //sets the current planet of the meteor as the one where it has been created
        MeteorScript meteorScript = newObject.GetComponent<MeteorScript>();
        meteorScript.currentPlanet = this.gameObject;
    }
}
