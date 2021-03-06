﻿using UnityEngine;
using UnityEngine.UI;

public class FirstPersonController : MonoBehaviour {
    //Player stats
    int hp;
    int mp;
    public int maxHP = 100;
    public int maxMP = 100;
    public int atkPower = 1;
    public int defPower = 1;
    public int level = 1;
    public int curExp = 0;
    int requiredExp = 100;
    int maxExp = 100000;
    int maxLevel = 99;
    public GameObject statusUI;

    //Movement variables
    public float speed = 10.0F;
    public float jumpHeight = 1.0F;
    public float sprintSpeed = 20.0F;

    //Camera variables
    bool mouseLocked = false;

    //Interaction variables
    public Camera camera;
    public float distance = 2F;
    public LayerMask layermask;
    public Text interactiveText;
    GameObject selectedObject;

    //Misc variables
    GameObject player;
    [SerializeField] Transform respawnPoint;

	// Use this for initialization
	void Start () {
        player = gameObject;
        mouseLocking();
        hp = maxHP;
        mp = maxMP;
        updateStatus();
	}

	// Update is called once per frame
	void Update () {

        //Jumping
        float jump = Input.GetAxis("Jump");
        jump *= Time.deltaTime;

        transform.Translate(0, jump*jumpHeight, 0);

        //Move character
        if (Input.GetKey("left shift"))
            sprint();
        else
            run();

        //Player actions
        if (Input.GetKeyDown("e"))
            destroyObject();
        if (Input.GetButtonDown("Fire1")) //(Input.GetMouseButtonDown(0))
            attack(10);

        //Change state of mouse lock and guided camera when ESC is pressed
        if (Input.GetKeyDown("escape"))
            mouseLocking();

        //Check for interactables
        selectObject();

        
	}

    //Locks / Unlocks mouse and enables / disables guided camera
    void mouseLocking()
    {
        if (mouseLocked)
            Cursor.lockState = CursorLockMode.None;
        else
            Cursor.lockState = CursorLockMode.Locked;
        mouseLocked = !mouseLocked;
        Cursor.visible = !mouseLocked;
        transform.Find("Main Camera").GetComponent<CameraController>().enabled = mouseLocked;
        print("Mouse locked: " + mouseLocked);
    }

	void run()
    {
        //Movement
        float moveVertical = Input.GetAxis("Vertical");
        float moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical *= Time.deltaTime;
        moveHorizontal *= Time.deltaTime;

        transform.Translate(moveHorizontal * speed, 0, moveVertical * speed);
    }

    void sprint()
    {
        //Movement
        float moveVertical = Input.GetAxis("Vertical");
        float moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical *= Time.deltaTime;
        moveHorizontal *= Time.deltaTime;

        transform.Translate(moveHorizontal * sprintSpeed, 0, moveVertical * sprintSpeed);
    }

    void damage(int dmg)
    {
        hp -= dmg;
        if (hp > maxHP)
            hp = maxHP;
        else if (hp <= 0)
            onDeath();
            print("You died..");
        updateStatus();
    }

    void drainMP(int drain)
    {
        mp -= drain;
        if (mp > maxMP)
            mp = maxMP;
        updateStatus();
    }

    public void getExperience(int amount)
    {
        curExp += amount;
        if (curExp >= requiredExp && level < maxLevel)
        {
            curExp -= requiredExp;
            levelUp();
            
        }
        updateStatus();
    }

    public void levelUp()
    {
        if (level >= maxLevel) return;
        level++;
        requiredExp = (int)(1.2F * requiredExp);
        maxMP = (int)(1.1 * maxMP);
        maxHP = (int)(1.1 * maxHP);
        mp = maxMP;
        hp = maxHP;
    }

    public void updateStatus()
    {
        statusUI.transform.Find("HP").GetChild(0).GetComponent <Text>().text = hp.ToString();
        statusUI.transform.Find("MP").GetChild(0).GetComponent<Text>().text = mp.ToString();
        statusUI.transform.Find("LvL").GetChild(0).GetComponent<Text>().text = level.ToString();
        statusUI.transform.Find("Exp").GetChild(0).GetComponent<Text>().text = curExp.ToString();

    }
    
    void selectObject()
    {
        Ray ray = camera.ViewportPointToRay(Vector3.one / 2F);
        Debug.DrawRay(ray.origin, ray.direction * distance, Color.red);

        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, distance, layermask))
        {
            var hitItem = hitInfo.transform.gameObject;
            if (hitItem == null)
                selectedObject = null;
            else if (hitItem != null && hitItem != selectedObject)
            {
                selectedObject = hitItem;
                if (selectedObject.tag == "Enemy")
                    interactiveText.text = selectedObject.GetComponent<EnemyController>().GUIText;
                else
                    interactiveText.text = "Press \"E\" to interact";
                interactiveText.enabled = true;
            }

        }
        else
        {
            selectedObject = null;
            interactiveText.enabled = false;
        }

    }

    void destroyObject()
    {

        if(selectedObject != null && selectedObject.tag == "Destructible")
            Destroy(selectedObject);
    }

    void attack(int dmg)
    {
        if(selectedObject != null)
        {
            selectedObject.GetComponent<EnemyController>().damage(dmg, player);
        }
    }

    public void attacked(int dmg)
    {
        damage(dmg);
    }

    void respawn()
    {
        hp = maxHP;
        mp = maxHP;
        player.transform.position = respawnPoint.transform.position;
    }
    
    void onDeath()
    {
        //TO-DO 
        //
        //Show "Death Screen"
        if (level > 50 || (curExp == 0 && level > 1))
            level--;
        else
            curExp = 0;
        respawn();
    }
}
