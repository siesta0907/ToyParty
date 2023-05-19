using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candy : MonoBehaviour
{
    //ĵ�� ����
    //none -1 blue_candy  0, green_candy  1, orange_candy  2, purple_candy  3, red_candy  4, yellow_cadny  5
    public int candyType;

    //public bool isCandyMoved = false;

    private Vector2 currentPos;     // ���� ��ġ
    private Vector2 targetPos;       // ��ǥ ��ġ

    [SerializeField]
    public int currentBoardX, currentBoardY; //���� ������ǥ

    private float moveTime = 0.1f;      // ĵ�� �ڿ������� �����̴� �ð�
    private float spawnTimer = 0f;    //ĵ�� �ڿ������� ������ ����� �� ����ϴ� ����
    private float moveCellTime = 0.2f; //ĵ�� ��ĭ�� �̵��ϴµ� �ɸ��� �ð�


    //������ü�� ���μ��� ���� 
    public static float cellWidth = 1.7f;  //��ĭ�� ���� ũ��
    public static float cellHeight = 1.5f;  //��ĭ�� ���� ũ��

    BoardManager boardManager;

    //�����̴���
    public bool isMoving = true;

    [SerializeField]
    Sprite[] sprites; //ĵ�� ��� ��������Ʈ

    private void Start()
    {
        boardManager = GameObject.FindGameObjectWithTag("BoardManager").GetComponent<BoardManager>();

        currentBoardX = 0;
        currentBoardY = BoardManager.col - 1;

        //�����ϰ� ĵ���� ����
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

    //�����üŷ�Ҷ����� ��������. �������� ���� ȣ���Ҷ����� �����.

    //�ؿ� ������ �ִ��� Ȯ��, ������ �ִٸ� �̵�
    public void CandyCheckAndDown()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= moveCellTime)
        {
            spawnTimer = 0f;
            if (CheckCandyDown()) CandyMove();
        }
    }

    //�Ʒ��� ������ ������ ������ true�� ��ȯ, �ƴ϶�� false�� ��ȯ

    public bool CheckCandyDown()
    {
        //���� ���ٿ� ������
        if (currentBoardX == BoardManager.row - 1 || currentBoardY == 0)
        {
            boardManager.BoardChecking(currentBoardX, currentBoardY, gameObject);
            return false;
        }
        //�ؿ� ������ ����ִ��� Ȯ��, 
        if (boardManager.board[currentBoardX + 1, currentBoardY - 1] == 0)
        {
            boardManager.BoardUnChecking(currentBoardX, currentBoardY);
            return true;
        }
        //   �����̳� ������ �Ʒ��� ������ ������
        else if (boardManager.board[currentBoardX + 1, currentBoardY] == 0 || boardManager.board[currentBoardX, currentBoardY - 1] == 0)
        {
            boardManager.BoardUnChecking(currentBoardX, currentBoardY);
            return true;

        }
        //������ ������ ���� ��
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
            //�Է¹���  ��ǥ�� �� ĵ�� �����Ѵٸ� true ��ȯ, �ƴϸ� false ��ȯ
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

    //ĵ�� �Ʒ��� �����δ�. 
    //�ٸ� ĵ�� ������, �����̳� ���������� ��������.
    public void CandyMove()
    {

        //�ؿ� ������ ��ٸ� ������ ��������.
        if (boardManager.board[currentBoardX + 1, currentBoardY - 1] == 0)
        {
            CandyMoveDown();
        }
        //���ʰ� ������ �Ѵ� ������ �ִٸ� ���� �����ϰ� ����. 

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
        //���ʾƷ��� ������ ���Ҵٸ� ���ʾƷ��� ����.
        else if (boardManager.board[currentBoardX, currentBoardY - 1] == 0)
        {

            if (!checkUpperAround(currentBoardX, currentBoardY - 1)) CandyMoveLeftDown();

        }
        //�����ʾƷ��� ������ ���Ҵٸ� �����ʾƷ��� ����.
        else if (boardManager.board[currentBoardX + 1, currentBoardY] == 0)
        {

            if (!checkUpperAround(currentBoardX + 1, currentBoardY)) CandyMoveRightDown();

        }
    }

    public void CandyMoveDown()
    {
        currentPos = transform.position; //���� ��ǥ ����
        targetPos = currentPos + new Vector2(0, cellWidth * -1);
        currentBoardX += 1;
        currentBoardY -= 1;
        StartCoroutine(CandyCellMoveCoroutine(targetPos));


    }
    public void CandyMoveLeftDown()
    {
        currentPos = transform.position; //���� ��ǥ ����
        targetPos = currentPos + new Vector2(-1 * cellHeight, -0.5f * cellWidth);
        currentBoardY -= 1;
        StartCoroutine(CandyCellMoveCoroutine(targetPos));

    }
    public void CandyMoveRightDown()
    {
        currentPos = transform.position; //���� ��ǥ ����
        targetPos = currentPos + new Vector2(cellHeight, -0.5f * cellWidth);
        currentBoardX += 1;
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

        transform.position = targetPos;
    }

}

