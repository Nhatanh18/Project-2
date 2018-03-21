using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour 
{
    public float levelStartDelay = 2f;
    public float turnDelay = 0.1f;
    public BoardManager boardScript;
    public static GameManager instance = null;
    public int playerFoodPoints = 100;
    [HideInInspector]
    public bool playersTurn = true;

    private Text Textlevel;
    private GameObject levelImage;
    private int level = 3;
    private List<Enemy> enemies;							//List of all Enemy units, used to issue them move commands.
    private bool enemiesMoving;
    private bool doingSetup;

	void Awake () 
    {
        if (instance == null)  
            instance = this;   
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        enemies = new List<Enemy>();
        boardScript = GetComponent<BoardManager>();
        InitGame();	
	}

    public void GameOver()
    {       
        //Disable this GameManager.
        enabled = false;
    }

    void InitGame()
    {

        doingSetup = true;
        levelImage = GameObject.Find("levelImage");
        Textlevel = GameObject.Find("Textlevel").GetComponent<Text>();
        Textlevel.text = "Level" + level;
        levelImage.SetActive(true);
        Invoke("HideLevelImage", levelStartDelay);

        enemies.Clear();
        boardScript.SetupScene(level);
    }

    private void HideLevelImage()
    {
        levelImage.SetActive(false);
        doingSetup = false;
    }

    public void GameOvers()
    {
        Textlevel.text = "After" + level + "levels, you start";
        levelImage.SetActive(true);
        enabled = false;
    }

    void Update()
    {
        //Check that playersTurn or enemiesMoving or doingSetup are not currently true.
        if (playersTurn || enemiesMoving || doingSetup)

            //If any of these are true, return and do not start MoveEnemies.
            return;

        //Start moving enemies.
        StartCoroutine(MoveEnemies());
    }

    public void AddEnemyToList(Enemy script)
    {
        //Add Enemy to List enemies.
        enemies.Add(script);
    }

    IEnumerator MoveEnemies()
    {
        //While enemiesMoving is true player is unable to move.
        enemiesMoving = true;

        //Wait for turnDelay seconds, defaults to .1 (100 ms).
        yield return new WaitForSeconds(turnDelay);

        //If there are no enemies spawned (IE in first level):
        if (enemies.Count == 0)
        {
            //Wait for turnDelay seconds between moves, replaces delay caused by enemies moving when there are none.
            yield return new WaitForSeconds(turnDelay);
        }

        //Loop through List of Enemy objects.
        for (int i = 0; i < enemies.Count; i++)
        {
            //Call the MoveEnemy function of Enemy at index i in the enemies List.
            enemies[i].MoveEnemy();

            //Wait for Enemy's moveTime before moving next Enemy, 
            yield return new WaitForSeconds(enemies[i].moveTime);
        }
        //Once Enemies are done moving, set playersTurn to true so player can move.
        playersTurn = true;

        //Enemies are done moving, set enemiesMoving to false.
        enemiesMoving = false;
    }
}
