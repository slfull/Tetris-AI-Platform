using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SearchService;
using UnityEngine;

public class NextBlock : MonoBehaviour
{
    private SpawnTetromino spawnTetromino;
    private List<GameObject> tetrominoList;
    private Vector3 nextBarPos = new Vector3(15, 18, 0);
    // Start is called before the first frame update
    void Start()
    {
        spawnTetromino = GameObject.Find("SpawnManager").GetComponent<SpawnTetromino>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SpawnNextBlock() 
    {
        GameObject[] allTetromino = GameObject.FindGameObjectsWithTag("Tetromino");
        foreach(GameObject obj in allTetromino)
        {
            if(nextBarPos == obj.transform.position)
            {
                Destroy(obj);
            }
        }
        tetrominoList = new List<GameObject>(spawnTetromino.objectList);
        Instantiate(tetrominoList[0], transform.position, Quaternion.identity);
    }
}
