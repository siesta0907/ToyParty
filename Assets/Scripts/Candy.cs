using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candy : MonoBehaviour
{
    //캔디 종류
    //blue_candy  0, green_candy  1, orange_candy  2, purple_candy  3, red_candy  4, yellow_cadny  5
    public int candyType;

    private Vector2 currentPos;     // 현재 위치
    private Vector2 targetPos;       // 목표 위치

    [SerializeField]
    public int currentBoardX, currentBoardY; //현재 보드좌표

    private float moveTime = 0.1f;      // 캔디가 자연스럽게 움직이는 시간
    private float spawnTimer = 0f;    //캔디가 자연스럽게 움직임 계산할 때 사용하는 변수
    private float moveCellTime = 0.2f; //캔디가 한칸을 이동하는데 걸리는 시간

    float downTime = 0.2f;
    float downTimer = 0f; 

    //정육면체의 가로세로 길이 
    public static float candyHeight = 1.7f;  //한칸의 세로 크기
    public static float candyWidth = 1.5f;  //한칸의 가로 크기

    BoardManager boardManager;
    [SerializeField]
    public bool isMoving = true;

    [SerializeField]
    Sprite[] sprites; //캔디 모양 스프라이트

    private void Start()
    {
        boardManager = GameObject.FindGameObjectWithTag("BoardManager").GetComponent<BoardManager>();

        currentBoardX = 0;
        currentBoardY = BoardManager.col - 1;
        boardManager.BoardChecking(currentBoardX, currentBoardY, gameObject);

        //랜덤하게 캔디모양 결정
        int randomIndex = Random.Range(0, sprites.Length);
        GetComponent<SpriteRenderer>().sprite = sprites[randomIndex];
        candyType = randomIndex;
    }
    private void Update()
    {
        if (CheckCandyDown()) CandyMove();
    }

    public bool CheckCandyDown()
    {
   
               //가장 밑줄에 있을때
       if (currentBoardX == 4 || currentBoardY == 0)
        {
            Debug.Log("가장 밑줄에 있엉");
            isMoving = false;
            return false;

        }
        //밑에 공간이 비어있는지 확인, 
        else if (boardManager.board[currentBoardX + 1, currentBoardY - 1] == 0)
        {
            //Debug.Log("밑에 공간있어요");
            isMoving = true;
          //  boardManager.BoardUnChecking(currentBoardX, currentBoardY);
            return true;

        }
        //   왼쪽이나 오른쪽 아래에 공간이 있을떄
        else if (boardManager.board[currentBoardX + 1, currentBoardY] == 0 || boardManager.board[currentBoardX, currentBoardY - 1] == 0)
        {
            //Debug.Log(" 왼쪽이나 오른쪽 아래에 공간이 있을떄");
            isMoving = true;
          
            return true;
        }
        //움직일 공간이 없을 때
        else
        {
            Debug.Log("공간 없음");
            isMoving = false;
           // boardManager.BoardChecking(currentBoardX, currentBoardY, gameObject);
            return false;

        }
        
     
    }



    //캔디를 아래로 움직인다. 
    //다른 캔디를 만나면, 왼쪽이나 오른쪽으로 내려간다.
    public void CandyMove() {

         downTimer+=Time.deltaTime; 

        if (downTimer > 0.5f)
        {
            downTimer = 0f;
            boardManager.BoardUnChecking(currentBoardX, currentBoardY);
            if (boardManager.board[currentBoardX + 1, currentBoardY - 1] == 0)
            {
                Debug.Log("밑으로 내려가요");
                CandyMoveDown();
            }

            //왼쪽과 오른쪽 둘다 공간이 있다면 둘중 랜덤하게 간다. 
            else if (boardManager.board[currentBoardX + 1, currentBoardY] == 0 && boardManager.board[currentBoardX, currentBoardY - 1] == 0)
            {
                Debug.Log("랜덤하게 밑으로 내려가요");
                int isLeft = Random.Range(0, 2);
                if (isLeft == 0) CandyMoveLeftDown();
                else CandyMoveRightDown();

            }
            //왼쪽아래에 공간이 남았다면 왼쪽아래로 간다.
            else if (boardManager.board[currentBoardX, currentBoardY - 1] == 0)
            {
                Debug.Log("왼쪽 밑으로 내려가요");
                CandyMoveLeftDown();
            }
            //오른쪽아래에 공간이 남았다면 오른쪽아래로 간다.
            else if (boardManager.board[currentBoardX + 1, currentBoardY] == 0)
            {
                Debug.Log("오른쪽 밑으로 내려가요");
                CandyMoveRightDown();
            }
        }
    } 

    public void CandyMoveDown()
    {
        currentPos = transform.position; //현재 좌표 저장
        targetPos = currentPos + new Vector2(0, candyHeight * -1);
        currentBoardX += 1;
        currentBoardY -= 1;
        boardManager.BoardChecking(currentBoardX, currentBoardY, gameObject);
        StartCoroutine(CandyCellMoveCoroutine(targetPos));

        
    }
    public void CandyMoveLeftDown()
    {
        currentPos = transform.position; //현재 좌표 저장
        targetPos = currentPos + new Vector2(-1 * candyWidth, -0.5f * candyHeight);
        currentBoardY -= 1;
        boardManager.BoardChecking(currentBoardX, currentBoardY, gameObject);
        StartCoroutine(CandyCellMoveCoroutine(targetPos));
        
    }
    public void CandyMoveRightDown()
    {
        currentPos = transform.position; //현재 좌표 저장
        targetPos = currentPos + new Vector2(candyWidth, -0.5f * candyHeight);
        currentBoardX += 1;
        boardManager.BoardChecking(currentBoardX, currentBoardY, gameObject);
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

        transform.position = targetPos  ;
    }

}

