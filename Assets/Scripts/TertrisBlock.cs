using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TertisBlock : MonoBehaviour
{
    public Vector3 pivot;
    private float previousTime = 0.0f;
    public float fallTime = 0.8f;
    public float realFallTime = 0.8f;
    public static int height = 20;
    public static int width = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HorizontalMove();
        TertrisFallDown();
        TertrisRotate();
        
    }

    void HorizontalMove() //控制方塊左右移動
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1, 0, 0);
            if(!VaildMove())
            { //碰到邊界還原原本移動
                transform.position += new Vector3(1, 0, 0);
            }
        }
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1, 0, 0);
            if(!VaildMove()) 
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
                this.enabled = false;
                FindObjectOfType<SpawnTetromino>().NewTetromino(); //碰到底部讓此物件無法操作並生成新的方塊
            }
        }
    }
    
    void TertrisRotate() {
        if(Input.GetKeyDown(KeyCode.UpArrow)) {
            transform.RotateAround(transform.TransformPoint(pivot), new Vector3(0, 0, 1), 90);
            if(!VaildMove())
            { //之後要做wallkick要改
                transform.RotateAround(transform.TransformPoint(pivot), new Vector3(0, 0, 1), -90);
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
        }
        return true;
    }
}
