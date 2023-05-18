using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candy : MonoBehaviour
{
    //ĵ�� ����
    //blue_candy  0, green_candy  1, orange_candy  2, purple_candy  3, red_candy  4, yellow_cadny  5
    public int candyType;

    private Vector2 currentPos;     // ���� ��ġ
    private Vector2 targetPos;       // ��ǥ ��ġ

    [SerializeField]
    public int currentBoardX, currentBoardY; //���� ������ǥ

    private float moveTime = 0.1f;      // ĵ�� �ڿ������� �����̴� �ð�
    private float spawnTimer = 0f;    //ĵ�� �ڿ������� ������ ����� �� ����ϴ� ����
    private float moveCellTime = 0.2f; //ĵ�� ��ĭ�� �̵��ϴµ� �ɸ��� �ð�

    float downTime = 0.2f;
    float downTimer = 0f; 

    //������ü�� ���μ��� ���� 
    public static float candyHeight = 1.7f;  //��ĭ�� ���� ũ��
    public static float candyWidth = 1.5f;  //��ĭ�� ���� ũ��

    BoardManager boardManager;
    [SerializeField]
    public bool isMoving = true;

    [SerializeField]
    Sprite[] sprites; //ĵ�� ��� ��������Ʈ

    private void Start()
    {
        boardManager = GameObject.FindGameObjectWithTag("BoardManager").GetComponent<BoardManager>();

        currentBoardX = 0;
        currentBoardY = BoardManager.col - 1;
        boardManager.BoardChecking(currentBoardX, currentBoardY, gameObject);

        //�����ϰ� ĵ���� ����
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
   
               //���� ���ٿ� ������
       if (currentBoardX == 4 || currentBoardY == 0)
        {
            Debug.Log("���� ���ٿ� �־�");
            isMoving = false;
            return false;

        }
        //�ؿ� ������ ����ִ��� Ȯ��, 
        else if (boardManager.board[currentBoardX + 1, currentBoardY - 1] == 0)
        {
            //Debug.Log("�ؿ� �����־��");
            isMoving = true;
          //  boardManager.BoardUnChecking(currentBoardX, currentBoardY);
            return true;

        }
        //   �����̳� ������ �Ʒ��� ������ ������
        else if (boardManager.board[currentBoardX + 1, currentBoardY] == 0 || boardManager.board[currentBoardX, currentBoardY - 1] == 0)
        {
            //Debug.Log(" �����̳� ������ �Ʒ��� ������ ������");
            isMoving = true;
          
            return true;
        }
        //������ ������ ���� ��
        else
        {
            Debug.Log("���� ����");
            isMoving = false;
           // boardManager.BoardChecking(currentBoardX, currentBoardY, gameObject);
            return false;

        }
        
     
    }



    //ĵ�� �Ʒ��� �����δ�. 
    //�ٸ� ĵ�� ������, �����̳� ���������� ��������.
    public void CandyMove() {

         downTimer+=Time.deltaTime; 

        if (downTimer > 0.5f)
        {
            downTimer = 0f;
            boardManager.BoardUnChecking(currentBoardX, currentBoardY);
            if (boardManager.board[currentBoardX + 1, currentBoardY - 1] == 0)
            {
                Debug.Log("������ ��������");
                CandyMoveDown();
            }

            //���ʰ� ������ �Ѵ� ������ �ִٸ� ���� �����ϰ� ����. 
            else if (boardManager.board[currentBoardX + 1, currentBoardY] == 0 && boardManager.board[currentBoardX, currentBoardY - 1] == 0)
            {
                Debug.Log("�����ϰ� ������ ��������");
                int isLeft = Random.Range(0, 2);
                if (isLeft == 0) CandyMoveLeftDown();
                else CandyMoveRightDown();

            }
            //���ʾƷ��� ������ ���Ҵٸ� ���ʾƷ��� ����.
            else if (boardManager.board[currentBoardX, currentBoardY - 1] == 0)
            {
                Debug.Log("���� ������ ��������");
                CandyMoveLeftDown();
            }
            //�����ʾƷ��� ������ ���Ҵٸ� �����ʾƷ��� ����.
            else if (boardManager.board[currentBoardX + 1, currentBoardY] == 0)
            {
                Debug.Log("������ ������ ��������");
                CandyMoveRightDown();
            }
        }
    } 

    public void CandyMoveDown()
    {
        currentPos = transform.position; //���� ��ǥ ����
        targetPos = currentPos + new Vector2(0, candyHeight * -1);
        currentBoardX += 1;
        currentBoardY -= 1;
        boardManager.BoardChecking(currentBoardX, currentBoardY, gameObject);
        StartCoroutine(CandyCellMoveCoroutine(targetPos));

        
    }
    public void CandyMoveLeftDown()
    {
        currentPos = transform.position; //���� ��ǥ ����
        targetPos = currentPos + new Vector2(-1 * candyWidth, -0.5f * candyHeight);
        currentBoardY -= 1;
        boardManager.BoardChecking(currentBoardX, currentBoardY, gameObject);
        StartCoroutine(CandyCellMoveCoroutine(targetPos));
        
    }
    public void CandyMoveRightDown()
    {
        currentPos = transform.position; //���� ��ǥ ����
        targetPos = currentPos + new Vector2(candyWidth, -0.5f * candyHeight);
        currentBoardX += 1;
        boardManager.BoardChecking(currentBoardX, currentBoardY, gameObject);
        StartCoroutine(CandyCellMoveCoroutine(targetPos));
        
    }

    // ��ǥ��ġ���� �ڿ������� �̵��ϴ� �ڷ�ƾ �Լ�
    IEnumerator CandyCellMoveCoroutine(Vector3 targetPos)
    {
        float elapsedTime = 0;
        Vector3 startingPos = transform.position; //������ġ

        while (elapsedTime < moveTime)
        {
            transform.position = Vector3.Lerp(startingPos, targetPos, (elapsedTime / moveTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos  ;
    }

}

