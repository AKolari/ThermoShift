using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{ 

    static public GameManager S;


    public SpawnPoint spawnPoint; 

    public Player player;

    [Header("Inscribed")]
    public Text uitTemp;           //the Temperature Text...?   
    
    static public bool heatOn;  //True = Hot; False = Cold; 
    private Scene curLevel;
    private int Levels;


    //Sounds 
    

    /* Other Needed Variables
     Levels (list of scenes...?) 

     */

   
    void Awake()
    {
       // player = GameObject.FindAnyObjectByType<Player>();
      
       
        if (S == null)
        {
            S = this;
        }

        else { return; }

        heatOn = true;
        
        curLevel = SceneManager.GetActiveScene();
        Levels = SceneManager.sceneCount;
        //AudioManager.Instance.playMusic();
       
        //AudioManager.Instance.playMusic(); //Play music depending on scene
        

    }

    public void Start()
    {
        player.transform.position = spawnPoint.transform.position;
        
    }
    public bool HeatOn {  get { return heatOn; } }




    // Update is called once per frame
    void Update()
    {
        
    }

    static public void NEXT_LEVEL() 
    { 
        
        SceneManager.LoadScene(S.curLevel.buildIndex + 1);
        S.curLevel = SceneManager.GetActiveScene();
        Debug.Log(S.curLevel.name);
        if (S.curLevel.name.Equals("Level Mixed2"))
        {
            AudioManager.Instance.changeTrack();
            AudioManager.Instance.playMusic(true); //Play music depending on scene
        }
        //GameManager.DEATH(); 
        //AudioManager.Instance.playMusic();
    }




    static public void RESTART() 
    {
        SceneManager.LoadScene(S.curLevel.name);
        S.player.transform.position = S.spawnPoint.transform.position;
    }

    

    static public void RESPAWN()
    {
        //Check if player is dead first 
        if (S.player.alive == false) 
        {
            S.player.enabled = false;
            

            S.player.enabled = true;
            S.player.alive = true;
            S.player.isImmobile = false;
            RESTART();
           // S.player.transform = S.spawnPoint.transform;

        }

        //Reload scene?  
        
    } 

    //Kills the Player
    static public void DEATH()
    {
       S.player.Death();
       
    }

    
    
    static public void TEMP_CHANGE() {
        if (GameManager.heatOn == true) {

            GameManager.heatOn = false; 
        }
        
        else if (GameManager.heatOn == false) { //heatOn was false
            GameManager.heatOn = true;
        }
    }

    static public void KEY_GET(bool k = true)
    {
        //By default, KEY_GET() increments keys
        if (k == true)
        {
            S.player.hasKey += 1;
        } 

        else
        {
            S.player.hasKey -= 1;
        }
        
    }

    static public bool CHECK_KEY()
    {
        return S.player.hasKey >= 1;
    }


    static public bool IS_ALIVE()
    {
        return S.player.alive;
    }

}
