using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    //Pulls in the players health from Player
    public Player p;
    //public int health;
    private Image change;
    public GameObject Thermostat;
    public int heartNum; //The number of hearts you want to display
    public Image[] heartArr;//Heart Image array
    public Sprite heart; //sprite for heart
    public Sprite emptyHeart;//sprite for emptyheart
    //Thermostat Stuff
    public Sprite thermoHot;
    public Sprite thermoCold;

    private void Start()
    {
        change = Thermostat.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {

        if (p.health > heartNum)
        {
            p.health = heartNum;
        }

        for (int i = 0; i < heartArr.Length; i++)
        {
            if (i < p.health)
            {
                heartArr[i].sprite = heart;
            }
            else
            {
                heartArr[i].sprite = emptyHeart;
            }
            if (i < heartNum)
            {
                heartArr[i].enabled = true;
            }
            else
            {
                heartArr[i].enabled = false;
            }

        }

        if (GameManager.S.HeatOn)
        {
            change.sprite = thermoHot;
        }
        else { 
            change.sprite = thermoCold;
        }
    }

    
}
