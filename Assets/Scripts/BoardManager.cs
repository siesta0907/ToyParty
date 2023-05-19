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
    public int[,] board =
        { { -1, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, -1 } };

    //캔디들의 오브젝트들을 담은 이중배열
    public GameObject[,] candyObjectBoard = new GameObject[row, col];

    [SerializeField]
    public GameObject candy; //캔디 프리팹


    public static Vector3 top = new Vector3(0f, 2.5f, 0f); //캔디가 생성되는 칸
    private float spawnCandyTime = 0.3f; //캔디리스폰시간
    private float destroyCandyTime = 0.4f; //캔디리스폰시간
    private float spawnTimer = 1f, destroyTimer;

    private void Update()
    {
        if (CheckEmptyCandy()) makeCandy();

    }
    private void Start()
    {
        InvokeRepeating("CheckThreeCandy", 0f, 1f);
    }

    //보드가 다 차있는지 확인
    public bool CheckBoardFull()
    {
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                //보드가 있는데 object가 들어가 있지 않다면 false 반환
                if (board[i, j] != -1 && candyObjectBoard[i, j] == null) return false;
            }
        }
        return true;
    }


    //빈 캔디 검사하는 함수
    //모든 캔디를 검사해서 캔디가 없다면 true를 반환, 캔디가 다 꽉 차있다면 false를 반환
    //보드가 비어있는지 확인
    public bool CheckEmptyCandy()
    {
        for (int i = 0; i < row; i++)
            for (int j = 0; j < col; j++)
                if (board[i, j] == 0) return true;
        return false;
    }


    //입력받은 좌표를 0로 변경 (입력받은 좌표에 사탕이 없다)
    public void BoardChecking(int i, int j, GameObject candy)
    {
        candy.GetComponent<Candy>().isMoving = false;
        board[i, j] = 1;
        candyObjectBoard[i, j] = candy;

    }

    //입력받은 좌표를 0로 변경 (입력받은 좌표에 사탕이 없다)
    public void BoardUnChecking(int i, int j)
    {
        candy.GetComponent<Candy>().isMoving = true;
        board[i, j] = 0;
        candyObjectBoard[i, j] = null;
    }


    //캔디들을 검사해 같은 모양이 세줄 이상 있으면 없어지게 만드는 함수
    public void CheckThreeCandy()
    {
        if (CheckBoardFull())
        {
            Debug.Log("확인중..");
            //가로로 연속된 캔디 검사
            for (int i = 0; i < row; i++)
            {
                int matchCount = 1; //같은 모양이 몇개 있는지 저장하는 변수
                for (int j = 0; j < col - 1; j++)
                {
                    if (candyObjectBoard[i, j] != null && candyObjectBoard[i, j + 1] != null)
                    {
                        int currentCandyType = candyObjectBoard[i, j].GetComponent<Candy>().candyType;
                        int nextCandyType = candyObjectBoard[i, j + 1].GetComponent<Candy>().candyType;

                        //만약 현재 캔디랑 다음캔디의 타입이 같다면 matchCount를 증가시킨다.
                        if (currentCandyType != -1 && currentCandyType == nextCandyType)
                        {
                            matchCount++;
                            //마지막이라면
                            if (j == col - 2 && matchCount >= 3)
                            {
                                for (int c = 0; c < matchCount; c++)
                                {
                                    DestroyCandy(candyObjectBoard[i, j + 1 - c]);
                                }
                                matchCount = 1;
                            }
                        }
                        else
                        {
                            //현재 캔디랑 다음캔디의 타입이 같지 않다면 matchCount의 개수를 확인하고
                            //3개 이상이라면 DestroyCandy()함수 호출, matchCount를 1로 초기화

                            if (currentCandyType != -1 && matchCount >= 3)
                            {
                                for (int c = 0; c < matchCount; c++)
                                {
                                    DestroyCandy(candyObjectBoard[i, j - c]);
                                }
                                matchCount = 1;
                            }
                        }
                    }
                }
            }

        }

    }

    public void DestroyCandy(GameObject candy)
    {
        if (candy != null)
        {
            Debug.Log("펑!");
            BoardUnChecking(candy.GetComponent<Candy>().currentBoardX, candy.GetComponent<Candy>().currentBoardY);
            Destroy(candy);
        }


    }


    //캔디생성함수
    public void makeCandy()
    {

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnCandyTime)
        {
            spawnTimer = 0f;
            if (board[0, col - 1] != 1)
                Instantiate(candy, top, Quaternion.identity);
        }


    }
}
