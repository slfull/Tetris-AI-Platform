using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTetromino : MonoBehaviour
{
    public GameObject[] Tetrominoes;
    // Start is called before the first frame update
    void Start()
    {
        NewTetromino();
    }

    public void NewTetromino() { //生新方塊
        Instantiate(Tetrominoes[Random.Range(0, Tetrominoes.Length)], transform.position, Quaternion.identity);
    }

}
