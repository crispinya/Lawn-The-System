using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {

    private double life; 
    private bool isAlive;
    private float moveSpeed; 
    private float planetJumpSpeed; 

    public Slider lifeSlider; //slider showing the life
    public GameObject deadPanel; //panel that shows up when the player dies
    public GameObject pausePanel;
    public GameObject startPanel;
 
    private int coins;
    public Text coinsText;

    public Joystick joystick; //joystick to control de character
    private Vector3 moveDirection;
    private Transform myTransform;

    public GameObject currentPlanet; //planet the player is currently at
    private Animator playerAnim;

    private float time; 
    public Text timeText;

    private float stopWatch;


    void Start()
    {
        SetStartingValues();
        SetRigidBodySettings();
        SetComponentAnimator();

        isAlive = true;
        myTransform = this.transform;

        SetInitialTime();
        SetInitialCoins();
        SetDeadPanel();
        SetPausePanel();

        DoStartPanel();
    }

    

    //joystick movement, and gravity attractor
    void FixedUpdate () 
	{
        //joystick movement
        if(isAlive)
            GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + transform.TransformDirection(moveDirection) * moveSpeed * Time.deltaTime);

        //player is attracted by the atracctor (current planet)
        currentPlanet.GetComponent<GravityAttractor>().Attract(myTransform);
    }

    void Update ()
    {
        if (isAlive)
        {
            UpdateTime();
            if(time - stopWatch >= 0.4)
            {
                DecreaseLifeWhenTimePasses();
            }
        }

        MoveAndAnimate();
        ClickOnPlanet();

        //checks if it is dead
        if (life <= 0)
        {
            Lose();
        }
    }

    //checks if something as been collected by the player
    void OnTriggerEnter(Collider other)
    {
        MeshRenderer otherMeshRend = other.gameObject.GetComponent<MeshRenderer>();
        
        //if grass is collected
        if (other.gameObject.CompareTag("Grass") && otherMeshRend.enabled)
        {
            otherMeshRend.enabled = false; //grass becomes invisible
            UpdateLife(+0.35);

            //sometimes there is a coin hidden in the grass
            if(Random.Range(0, 30) == 0)
            {
                UpdateCoins(1);
            } 
        }

        //if a coin is collected
        else if (other.gameObject.CompareTag("Coin"))
        {
            UpdateCoins(1);
            other.gameObject.SetActive(false);
        }

        //if mushrooms are collected
        else if(other.gameObject.CompareTag("PowerUp"))
        {
            if (other.gameObject.name.Contains("LifePW"))
            {
                UpdateLife(30);
                other.gameObject.SetActive(false);
                
            } else if (other.gameObject.name.Contains("DeadPW"))
            {
                UpdateLife(-this.life);
                other.gameObject.SetActive(false);     
            } else if (other.gameObject.name.Contains("Speed Meteor"))
            {
                Time.timeScale = 2f;
                UpdateMoveSpeed();
                other.gameObject.SetActive(false); 
            }
        }

        //if a normal meteor is collected (if it doesn't have a shield)
        else if (other.gameObject.name.Contains("Meteor"))
        {
            other.gameObject.SetActive(false);
            UpdateLife(-this.life);
        }
    }


    //********************** PRIVATE METHODS **********************//
    private void DoStartPanel()
    {
        startPanel.GetComponent<CanvasGroup>().alpha = 1;
        startPanel.GetComponent<CanvasGroup>().interactable = true;
        startPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        Time.timeScale = 0f;
    }

    private void SetStartingValues()
    {
        life = 15; 
        UpdateLife(0);

        lifeSlider.maxValue = 30;
        lifeSlider.minValue = 0;

        Time.timeScale = 1f;
        UpdateMoveSpeed();
        planetJumpSpeed = 500;
    }

    private void SetRigidBodySettings()
    {
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void SetInitialCoins()
    {
        coins = 0;
        coinsText.text = "";
    }

    private void SetInitialTime()
    {
        stopWatch = 0;
        this.time = 0;
        timeText.text = "P: " + (int)time;
    }

    private void SetDeadPanel()
    {
        deadPanel.GetComponent<CanvasGroup>().alpha = -1;
        deadPanel.GetComponent<CanvasGroup>().interactable = false;
        deadPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    private void SetPausePanel()
    {
        pausePanel.GetComponent<CanvasGroup>().alpha = -1;
        pausePanel.GetComponent<CanvasGroup>().interactable = false;
        pausePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    private void SetComponentAnimator()
    {
        playerAnim = this.GetComponent<Animator>();
        playerAnim.SetBool("isDead", false);
        playerAnim.SetBool("isWalking", false);
    }

    private void DecreaseLifeWhenTimePasses()
    {
        UpdateLife(-1);
        stopWatch = this.time; //updating stopwatch
    }

    private void MoveAndAnimate()
    {
        //vector for the joystick movement
        moveDirection = new Vector3(joystick.Horizontal, 0, joystick.Vertical);

        //animates depending on player's movement
        if (moveDirection.magnitude != 0)
        {
            playerAnim.SetFloat("Speed", moveDirection.magnitude);
            playerAnim.SetBool("isWalking", true);
        }
        else
        {
            playerAnim.SetBool("isWalking", false);
        }
    }

    private void UpdateLife(double extraLife)
    {
        this.life = this.life + extraLife;
        lifeSlider.value = (float)life;
    }

    private void UpdateTime()
    {
        this.time = Time.timeSinceLevelLoad; //updating time
        timeText.text = "P: " + (int)time; //updating time text
    }

    private void UpdateCoins(int cuantity)
    {
        this.coins = coins + cuantity;
        coinsText.text = "C: " + coins.ToString();
    }

    //transport to another planet by clicking
    private void ClickOnPlanet()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            //if the ray impacts with a planet
            if (Physics.Raycast(ray, out hitInfo))
            {
                var rig = hitInfo.collider.GetComponent<Rigidbody>();

                //checks that the rigidbody touched isn't the current planet, the player itself or the void
                if (rig != null && rig != this.GetComponent<Rigidbody>() && rig != currentPlanet.GetComponent<Rigidbody>())
                {
                    //moves the player into the new planet
                    this.transform.position = Vector3.MoveTowards(transform.position, rig.position, Time.deltaTime * planetJumpSpeed * 100);

                    //sets the new values for currentplanetrb and sets the new planet as the attractor
                    currentPlanet = rig.gameObject;

                }
            }
        }
    }

    //when the player dies
    private void Lose()
    {
        isAlive = false;
        playerAnim.SetBool("isDead", true);
        deadPanel.GetComponent<CanvasGroup>().alpha = 1;
        deadPanel.GetComponent<CanvasGroup>().interactable = true;
        deadPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    private void UpdateMoveSpeed()
    {
        moveSpeed = 5.3f * Time.timeScale;
    }
}