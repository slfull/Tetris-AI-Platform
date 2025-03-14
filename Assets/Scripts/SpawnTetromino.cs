using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTetromino : MonoBehaviour
{
    public GameObject[] Tetrominoes;
    public List<GameObject> objectList;
    // Start is called before the first frame update
    private void Start()
    {
        NewTetrominoList();
        NewTetromino();
    }
    public void NewTetromino()
    {
        Instantiate(objectList[0], transform.position, Quaternion.identity);
        objectList.RemoveAt(0);
        if (objectList.Count == 0)
        {
            NewTetrominoList();
        }
        FindObjectOfType<NextBlock>().SpawnNextBlock();
    }



    private void NewTetrominoList()
    {
        objectList = new List<GameObject>(Tetrominoes);
        Shuffle(objectList);
    }
    private void Shuffle(List<GameObject> list)
    {
        int n = list.Count;
        System.Random rng = new System.Random();
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            GameObject value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

}

