using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class TertisBlock : MonoBehaviour
{
    public Vector3 pivot; //方塊軸心
    private float previousTime = 0.0f;
    public float fallTime = 0.8f;
    public float realFallTime = 0.8f;
    private bool canMove = true;
    public static int height = 23; //長
    public static int width = 10; //寬
    private static Transform[,] grid = new Transform[width, height]; //存方塊屬於哪個格子的矩陣 且所有方塊能存取到的矩陣值需要相同


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        TertrisFallDown();
        
    }


    public void HorizontalMoveLeft(InputAction.CallbackContext context) //控制方塊左右移動
    {
        if (context.performed && canMove) { 
            transform.position += new Vector3(-1, 0, 0);
            if (!VaildMove())
            { //碰到邊界還原原本移動
                transform.position += new Vector3(1, 0, 0);
            }
        }
    }

    public void HorizontalMoveRight(InputAction.CallbackContext context) //控制方塊左右移動
    {
        if (context.performed && canMove)
        {
            transform.position += new Vector3(1, 0, 0);
            if (!VaildMove())
            { //碰到邊界還原原本移動
                transform.position += new Vector3(-1, 0, 0);
            }
        }
    }

    void TertrisFallDown() //控制方塊落下
    {
        realFallTime = Input.GetKey(KeyCode.DownArrow) ? fallTime / 10 : fallTime; //按著下 falltime會除10 沒按就維持原本的值
        if(Time.time - previousTime > realFallTime) 
        {
            transform.position += new Vector3(0, -1, 0);
            previousTime = Time.time;
            if(!VaildMove()) 
            { //碰到邊界還原原本移動
                transform.position += new Vector3(0, 1, 0);
                AddToGrid();
                CheckForLines();
                canMove = false;
                this.enabled = false;
                FindObjectOfType<SpawnTetromino>().NewTetromino(); //碰到底部讓此物件無法操作並生成新的方塊
            }
        }
    }

    public void TertrisRotateClockWise(InputAction.CallbackContext context) { //控制旋轉 之後需要做逆時針轉
        if(context.performed && canMove) {
            transform.RotateAround(transform.TransformPoint(pivot), new Vector3(0, 0, 1), 90);
            if(!VaildMove())
            { //之後要做wallkick要改
                transform.RotateAround(transform.TransformPoint(pivot), new Vector3(0, 0, 1), -90);
            }
        }
    }
    public void TertrisRotateClockCounterWise(InputAction.CallbackContext context)
    { //控制旋轉 之後需要做逆時針轉
        if (context.performed && canMove)
        {
            transform.RotateAround(transform.TransformPoint(pivot), new Vector3(0, 0, 1), -90);
            if (!VaildMove())
            { //之後要做wallkick要改
                transform.RotateAround(transform.TransformPoint(pivot), new Vector3(0, 0, 1), 90);
            }
        }
    }

    void AddToGrid() //Tetromino碰到底部把所有方塊的存進grid
    {
        foreach(Transform children in transform) //所有Tetromino的square
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);
            grid[roundedX, roundedY] = children;
        }
    }

    void CheckForLines() //如果連線執行消線，並把方塊往下移
    {
        for(int i = height - 1; i >= 0; i--)
        {
            if(HasLine(i))
            {
                DeleteLine(i);
                RowDown(i);
            }
        }
    }

    bool HasLine(int i) //檢查i行是否連線
    {
        for(int j = 0; j < width; j++) 
        {
            if(grid[j, i] == null) 
            {
                return false;
            }
        }
        return true;
    }

    void DeleteLine(int i) //消除連線的所有square
    {
        for(int j = 0; j < width; j++)
        {
            Destroy(grid[j, i].gameObject);
            grid[j, i] = null;
        }
    }

    void RowDown(int i) //消線後把方塊往下移，同時調整grid中的資料
    {
        for(int y = i; y < height; y++)
        {
            for(int x = 0; x < width; x++)
            {
                if(grid[x, y] != null)
                {
                    grid[x, y - 1] = grid[x, y];
                    grid[x, y] = null;
                    grid[x, y - 1].transform.position -= new Vector3(0, 1, 0);
                }
            }
        }
    }

    bool VaildMove() //檢查是否是有效的移動 
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);
            if(roundedX < 0 || roundedX >= width || roundedY < 0 || roundedY >= height)
            {
                return false;
            }
            if(grid[roundedX, roundedY] != null) //檢查該欄位是否已經有方塊
            {
                return false;
            }
        }
        return true;
    }
}
