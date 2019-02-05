using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    //Enemy stats
    int hp;
    int mp;
    public int maxHP = 100;
    public int maxMP = 100;
    public int atkPower = 10;
    public int defPower = 1;
    public int level = 0;
    public int minLvL = 1;
    public int maxLvL = 101;
    public string GUIText = "Enemy \nLevel: ";

    //Enemy rewards
    public int exp = 10;

    //RNG
    System.Random rng = new System.Random();

    // Use this for initialization
    void Start () {
        if (level <= 0)
            level = rng.Next(minLvL, maxLvL);
        GUIText += level;

        if (level > 1)
        {
            maxMP = (int)(1.5 * maxMP) ^ level;
            maxHP = (int)(1.5 * maxHP) ^ level;
            hp = maxHP;
            mp = maxMP;
            atkPower = (int)(1.3 * atkPower) ^ level;
            exp = (int)(1.25) ^ level;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void counterAttack(GameObject attacker, int dmg)
    {
        attacker.GetComponent<FirstPersonController>().attacked(atkPower);
    }

    public void damage(int dmg, GameObject attacker)
    {
        counterAttack(attacker, 20);
        hp -= dmg;
        if (hp <= 0)
            onDeath(attacker);
    }

    void onDeath(GameObject attacker)
    {
        attacker.GetComponent<FirstPersonController>().getExperience(100);
        Destroy(gameObject);
    }
}
