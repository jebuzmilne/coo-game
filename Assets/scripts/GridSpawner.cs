using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpawner : MonoBehaviour
{
    //height and width are number of grids, space size is size of each grid
   public int height = 5;
    public int width = 5;
   public float GridSpaceSize = 40f;
    //create grass prefabs array
    public GameObject[] grassprefabs;

    //create private object that still shows on inspector will make it a prefab (keeping here to remember about how to use serializefield)
    [SerializeField] private GameObject gridCellPrefab;
    // gamegrid
    private GameObject[,] gameGrid; 
  
    void Start()
    {
        //on startup run create grid 
        createGrid();    
    }

  
    void Update()
    {
        
    }

    //make grid
    private void createGrid()
    {
//gameGrid object created earlier with height and width variables
        gameGrid = new GameObject[height, width];

    

        // make the grid from 0 to height 
        for (int X =0; X < height; X++)
        {
            //make grid from 0 to width 
            for (int Z = 0; Z < width; Z++)
            {
                //create random grassindex variable for each iteration
                int grassindex = Random.Range(0, grassprefabs.Length + 1);
                //add grid based on for loop width and height.....object is prefab, position is spacesize * X , -0,  and spacesize * z (-80 currently so it extends to map negative maximum (80 + half grid size = -100) will fix later with variable)
                gameGrid[X, Z] = Instantiate(gridCellPrefab, new Vector3((X * GridSpaceSize) - 80, 0, (Z * GridSpaceSize) - 80), Quaternion.AngleAxis(-90f, Vector3.right));
                //set parent transform
                gameGrid[X, Z].transform.parent = transform;

                //label grids X0, Z0 to X4, Z4
                gameGrid[X, Z].gameObject.name = "Grid Space (X: " + X.ToString() + ", Z: " + Z.ToString() + ")";
                if ((Z * Z) + (X * X) != 8)
                {
                    //check if random int from grass index is less than prefab array length (because we want some grids not producing grass) 
                    if (grassindex < grassprefabs.Length)
                    {
                        //create grass prefab based on random grass index value (I want to look in to reducing likelihood of certain grassprefabs that I will make special in future)
                        gameGrid[X, Z] = Instantiate(grassprefabs[grassindex], new Vector3((X * GridSpaceSize) - 80, -2, (Z * GridSpaceSize) - 80), Quaternion.AngleAxis(-90f, Vector3.right));
                        //make grass child of grass spawner empty
                        gameGrid[X, Z].transform.parent = GameObject.Find("GrassSpawner").transform;
                        //rename grass so that other functions can find it
                        gameGrid[X, Z].gameObject.name = "Grass (X: " + X.ToString() + ", Z: " + Z.ToString() + ")";
                    }
                }
            }
        }
    }
   //gets grid position from world position
    public Vector3Int GetGridPosFromWorld (Vector3 worldPosition)
    {
        //variable x = floor of world position / gridspace size (i.e. grid number in x axis)
        int x = Mathf.FloorToInt(worldPosition.x / GridSpaceSize);
     
        int z = Mathf.FloorToInt(worldPosition.z / GridSpaceSize);

        //clamp to ensure x and z do not go lower than zero or abve the width/height
        x = Mathf.Clamp(x, 0, width);
        z = Mathf.Clamp(x, 0, height);

        return new Vector3Int(x, 0, z);
    }


    //gets world position from grid position
    public Vector3 GetWorldPosFromGridPos(Vector3Int GridPos)
    {
        float x =  (GridPos.x * GridSpaceSize);
        float z = (GridPos.z * GridSpaceSize);

        return new Vector3(x, 0, z);
    }
}
