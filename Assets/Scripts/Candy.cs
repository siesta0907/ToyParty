using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candy : MonoBehaviour
{
    //캔디 종류
    //none -1 blue_candy  0, green_candy  1, orange_candy  2, purple_candy  3, red_candy  4, yellow_cadny  5
    public int candyType;

    //public bool isCandyMoved = false;

    private Vector2 currentPos;     // 현재 위치
    private Vector2 targetPos;       // 목표 위치

    [SerializeField]
    public int currentBoardX, currentBoardY; //현재 보드좌표

    private float moveTime = 0.1f;      // 캔디가 자연스럽게 움직이는 시간
    private float spawnTimer = 0f;    //캔디가 자연스럽게 움직임 계산할 때 사용하는 변수
    private float moveCellTime = 0.2f; //캔디가 한칸을 이동하는데 걸리는 시간


    //정육면체의 가로세로 길이 
    public static float cellWidth = 1.7f;  //한칸의 세로 크기
    public static float cellHeight = 1.5f;  //한칸의 가로 크기

    BoardManager boardManager;

    //움직이는중
    public bool isMoving = true;

    [SerializeField]
    Sprite[] sprites; //캔디 모양 스프라이트

    private void Start()
    {
        boardManager = GameObject.FindGameObjectWithTag("BoardManager").GetComponent<BoardManager>();

        currentBoardX = 0;
        currentBoardY = BoardManager.col - 1;

        //랜덤하게 캔디모양 결정
        int randomIndex = Random.Range(0, sprites.Length);
        GetComponent<SpriteRenderer>().sprite = sprites[randomIndex];
        candyType = randomIndex;


    }
    private void Update()
    {
       // if (isMoving)
      //  {
            CandyCheckAndDown();
       // }

    }

    //보드언체킹할때까지 내려간다. 내려가고 나면 호출할때까지 멈춘다.

    //밑에 공간이 있는지 확인, 공간이 있다면 이동
    public void CandyCheckAndDown()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= moveCellTime)
        {
            spawnTimer = 0f;
            if (CheckCandyDown()) CandyMove();
        }
    }

    //아래로 내려갈 공간이 있으면 true를 반환, 아니라면 false를 반환

    public bool CheckCandyDown()
    {
        //가장 밑줄에 있을때
        if (currentBoardX == BoardManager.row - 1 || currentBoardY == 0)
        {
            boardManager.BoardChecking(currentBoardX, currentBoardY, gameObject);
            return false;
        }
        //밑에 공간이 비어있는지 확인, 
        if (boardManager.board[currentBoardX + 1, currentBoardY - 1] == 0)
        {
            boardManager.BoardUnChecking(currentBoardX, currentBoardY);
            return true;
        }
        //   왼쪽이나 오른쪽 아래에 공간이 있을떄
        else if (boardManager.board[currentBoardX + 1, currentBoardY] == 0 || boardManager.board[currentBoardX, currentBoardY - 1] == 0)
        {
            boardManager.BoardUnChecking(currentBoardX, currentBoardY);
            return true;

        }
        //움직일 공간이 없을 때
        else
        {
            boardManager.BoardChecking(currentBoardX, currentBoardY, gameObject);
            return false;
        }
    }

    public bool checkUpperAround(int i, int j)
    {
        if (i >= 1 && j < 4)
        {
            //입력받은  좌표의 위 캔디가 존재한다면 true 반환, 아니면 false 반환
            if (boardManager.board[i - 1, j + 1] == 1)
                return true;
            else
                return false;
        }
        else return false;


    }
    public bool checkLeftAround(int i, int j)
    {
        if (i >= 1 && j >= 1)
        {
            if (boardManager.board[i - 1, j - 1] == 1)
                return true;
            else return false;
        }
        else return false;

    }
    public bool checkRightAround(int i, int j)
    {
        if (i < 4 && j < 4)
        {
            if (boardManager.board[i + 1, j + 1] == 1)
                return true;
            else return false;
        }
        else return false;

    }

    //캔디를 아래로 움직인다. 
    //다른 캔디를 만나면, 왼쪽이나 오른쪽으로 내려간다.
    public void CandyMove()
    {

        //밑에 공간이 빈다면 밑으로 내려간다.
        if (boardManager.board[currentBoardX + 1, currentBoardY - 1] == 0)
        {
            CandyMoveDown();
        }
        //왼쪽과 오른쪽 둘다 공간이 있다면 둘중 랜덤하게 간다. 

        else if (boardManager.board[currentBoardX + 1, currentBoardY] == 0 && boardManager.board[currentBoardX, currentBoardY - 1] == 0)
        {
            int isLeft = Random.Range(0, 2);
            if (isLeft == 0)
            {
                if (!checkUpperAround(currentBoardX, currentBoardY - 1)) CandyMoveLeftDown();
            }
            else
            {
                if (!checkUpperAround(currentBoardX + 1, currentBoardY)) CandyMoveRightDown();
            }
        }
        //왼쪽아래에 공간이 남았다면 왼쪽아래로 간다.
        else if (boardManager.board[currentBoardX, currentBoardY - 1] == 0)
        {

            if (!checkUpperAround(currentBoardX, currentBoardY - 1)) CandyMoveLeftDown();

        }
        //오른쪽아래에 공간이 남았다면 오른쪽아래로 간다.
        else if (boardManager.board[currentBoardX + 1, currentBoardY] == 0)
        {

            if (!checkUpperAround(currentBoardX + 1, currentBoardY)) CandyMoveRightDown();

        }
    }

    public void CandyMoveDown()
    {
        currentPos = transform.position; //현재 좌표 저장
        targetPos = currentPos + new Vector2(0, cellWidth * -1);
        currentBoardX += 1;
        currentBoardY -= 1;
        StartCoroutine(CandyCellMoveCoroutine(targetPos));


    }
    public void CandyMoveLeftDown()
    {
        currentPos = transform.position; //현재 좌표 저장
        targetPos = currentPos + new Vector2(-1 * cellHeight, -0.5f * cellWidth);
        currentBoardY -= 1;
        StartCoroutine(CandyCellMoveCoroutine(targetPos));

    }
    public void CandyMoveRightDown()
    {
        currentPos = transform.position; //현재 좌표 저장
        targetPos = currentPos + new Vector2(cellHeight, -0.5f * cellWidth);
        currentBoardX += 1;
        StartCoroutine(CandyCellMoveCoroutine(targetPos));

    }

    // 목표위치까지 자연스럽게 이동하는 코루틴 함수
    IEnumerator CandyCellMoveCoroutine(Vector3 targetPos)
    {
        float elapsedTime = 0;
        Vector3 startingPos = transform.position; //현재위치

        while (elapsedTime < moveTime)
        {
            transform.position = Vector3.Lerp(startingPos, targetPos, (elapsedTime / moveTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
    }

}

