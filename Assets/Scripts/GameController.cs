using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    //Action variables
    string printableText = "You clicked it...";
    public GameObject equipPrefab;
    Vector3 position = new Vector3(0, 10, 0);

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("f"))
            print("Respect your elders!");
        else if (Input.GetKeyDown("1"))
            getText();
        else if (Input.GetKeyDown("2")) ;
        // playMusic();
        else if (Input.GetKeyDown("3"))
            spawnObject();
    }

    public void getText()
    {
        print(printableText);
    }

    public void spawnObject()
    {
        GameObject spawned = (GameObject)Instantiate(equipPrefab, position, Quaternion.identity);
        spawned.name = "Purple sphere";
    }

}
