using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour 
{
    [Serializable]
	public class Count
    {
        public int minimun;
        public int maximun;

        public Count (int min, int max)
        {
            minimun = min;
            maximun = max;
        }
    }

    public int column = 8;
    public int row = 8;
    public Count wallcount = new Count(5, 9);
    public Count foodcount = new Count(1, 5);
    public GameObject exit;
    public GameObject[] foortiles;
    public GameObject[] walltiles;
    public GameObject[] foodtiles;
    public GameObject[] enemytiles;
    public GameObject[] outwalltiles;

    public Transform boardHolder;
    private List<Vector3> gridPosition = new List<Vector3>();

    void InitialiseList()
    {
        gridPosition.Clear();

        for (int x = 1; x < column - 1; x++ )
        {
            for(int y =1;y<row-1;y++)
            {
                gridPosition.Add(new Vector3(x,y,0f));
            }
        }
    }

    void BoardSetup()
    {
        
        boardHolder = new GameObject("Board").transform;

        
        for (int x = -1; x < column + 1; x++)
        {
            
            for (int y = -1; y < row + 1; y++)
            {

                GameObject toInstantiate = foortiles[Random.Range(0, foortiles.Length)];

                
                if (x == -1 || x == column || y == -1 || y == row)
                    toInstantiate = outwalltiles[Random.Range(0, outwalltiles.Length)];

                
                GameObject instance =
                    Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                
                instance.transform.SetParent(boardHolder);
            }
        }
    }

    Vector3 RandomPosition()
    {

        int randomIndex = Random.Range(0, gridPosition.Count);

        Vector3 randomPosition = gridPosition[randomIndex];

        gridPosition.RemoveAt(randomIndex);
      
        return randomPosition;
    }

    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        
        int objectCount = Random.Range(minimum, maximum + 1);

       
        for (int i = 0; i < objectCount; i++)
        {
            
            Vector3 randomPosition = RandomPosition();

           
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];

            
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }

    public void SetupScene(int level)
    {
        
        BoardSetup();

        
        InitialiseList();

        
        LayoutObjectAtRandom(walltiles, wallcount.minimun, wallcount.maximun);


        LayoutObjectAtRandom(foodtiles, foodcount.minimun, foodcount.maximun);

        
        int enemyCount = (int)Mathf.Log(level, 2f);

        
        LayoutObjectAtRandom(enemytiles, enemyCount, enemyCount);

        
        Instantiate(exit, new Vector3(column - 1, row - 1, 0f), Quaternion.identity);
    }

	void Start () 
    {
		
	}
	
	
	void Update () {
		
	}
}
