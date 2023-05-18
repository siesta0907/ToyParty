using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    //보드의 행과 열
    [SerializeField]
    public static int row = 5; //보드의 행
    public static int col = 5; //보드의 열

    //보드, 0이면 캔디가 없는 상태, 1이면 캔디가 있는 상태. -1은 보드가 없는 상태
    public int[,] board =  { { -1, 0, 0, 0, 0 },  { 0, 0, 0, 0, 0 },   { 0, 0, 0, 0, 0 },   { 0, 0, 0, 0, 0 },   { 0, 0, 0, 0, -1 } };

    //캔디들의 오브젝트들을 담은 이중배열
    public GameObject[,] candyObjectBoard = new GameObject[row,col];

    [SerializeField]
    public GameObject candy; //캔디 프리팹
    
    public static Vector3 top = new Vector3(0f, 2.5f,0f); //캔디가 생성되는 칸
    private float spawnCandyTime = 1f; //캔디리스폰시간
    private float spawnTimer, downTimer = 0f; //캔디 리스폰 타임 계산에 쓰이는 변수

    private void Update()
    { 
        //CandiesDown();
    }
    private void Start()
    {
        if (!CheckBoardFull()) InvokeRepeating("makeCandy", 0f, 0.7f);

       // InvokeRepeating("CandiesDown", 0f, 1f);
    }

    //보드가 다 차있는지 확인한다.
    public bool CheckBoardFull()
    {
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                //보드가 있는데 object가 들어가 있지 않다면 false 반환한다.
                if (board[i, j] != -1 && candyObjectBoard[i, j] == null) return false;
            }
        }
        return true;
    }

    public void CandiesDown()
    {
        //Debug.Log("확인중");
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    if (candyObjectBoard[i, j] != null )
                    {
                        if (candyObjectBoard[i, j].GetComponent<Candy>().CheckCandyDown()) candyObjectBoard[i, j].GetComponent<Candy>().CandyMove();   
                    }
                    
                }
            }
            
    }

    //입력받은 좌표를 1로 변경 (입력받은 좌표에 사탕이 있다)
    public void BoardChecking(int i, int j, GameObject candy)
    {
        board[i, j] = 1;
        candyObjectBoard[i, j] = candy;
    }

    //입력받은 좌표를 0로 변경 (입력받은 좌표에 사탕이 없다)
    public void BoardUnChecking(int i, int j)
    {
        board[i, j] = 0;
        candyObjectBoard[i, j] = null;
    }


    //캔디들을 검사해 같은 모양이 세줄 이상 있으면 없어지게 만드는 함수
    public void CheckThreeCandy()
    {
      
            //가로로 연속된 캔디 검사
            for (int i = 0; i < row; i++)
            {
                int matchCount = 1; //같은 모양이 몇개 있는지 저장하는 변수
                for (int j = 0; j < col - 1; j++)
                {
                    if (candyObjectBoard[i, j] != null && candyObjectBoard[i, j + 1] != null)
                    {
                        int currentCandyType = candyObjectBoard[i, j].GetComponent<Candy>().candyType; //현재 검사하는 캔디의 타입
                        int nextCandyType = candyObjectBoard[i, j + 1].GetComponent<Candy>().candyType; //현재 검사하는 캔디 다음의 타입

                        //만약 현재 캔디랑 다음캔디의 타입이 같다면 matchCount를 증가시킨다.
                        if (currentCandyType == nextCandyType)
                        {
                            matchCount += 1;
                            //현재 캔디가 검사의 마지막이고 matchCount가 3개 이상이라면,  
                            if (j == col - 2 && matchCount >= 3)
                            {
                                //matchCount 만큼 반복하면서 캔디를 없앤다. 
                                for (int c = 0; c < matchCount; c++)
                                {
                                    DestroyCandy(candyObjectBoard[i, j + 1 - c]);

                                }
                                //matchCount를 1로 초기화한다. 
                                matchCount = 1;
                            }
                        }
                        else
                        {
                            if (matchCount >= 3)
                            {
                                for (int c = 0; c < matchCount; c++) DestroyCandy(candyObjectBoard[i, j - c]);
                                matchCount = 1;
                            }
                        }
                    }
                }
            }
        
    }
            

    public void DestroyCandy( GameObject candy)
    {
        if (candy != null )
        {
            BoardUnChecking(candy.GetComponent<Candy>().currentBoardX, candy.GetComponent<Candy>().currentBoardY);
            Destroy(candy);
        }
        

    }


    //캔디생성함수
    public void makeCandy()
    {
        Instantiate(candy, top, Quaternion.identity);
    }
    
}
