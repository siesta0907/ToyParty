using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    //������ ��� ��
    [SerializeField]
    public static int row = 5; //������ ��
    public static int col = 5; //������ ��

    //����, 0�̸� ĵ�� ���� ����, 1�̸� ĵ�� �ִ� ����. -1�� ���尡 ���� ����
    public int[,] board =
        { { -1, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, -1 } };

    //ĵ����� ������Ʈ���� ���� ���߹迭
    public GameObject[,] candyObjectBoard = new GameObject[row, col];

    [SerializeField]
    public GameObject candy; //ĵ�� ������


    public static Vector3 top = new Vector3(0f, 2.5f, 0f); //ĵ�� �����Ǵ� ĭ
    private float spawnCandyTime = 0.3f; //ĵ�𸮽����ð�
    private float destroyCandyTime = 0.4f; //ĵ�𸮽����ð�
    private float spawnTimer = 1f, destroyTimer;

    private void Update()
    {
        if (CheckEmptyCandy()) makeCandy();

    }
    private void Start()
    {
        InvokeRepeating("CheckThreeCandy", 0f, 1f);
    }

    //���尡 �� ���ִ��� Ȯ��
    public bool CheckBoardFull()
    {
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                //���尡 �ִµ� object�� �� ���� �ʴٸ� false ��ȯ
                if (board[i, j] != -1 && candyObjectBoard[i, j] == null) return false;
            }
        }
        return true;
    }


    //�� ĵ�� �˻��ϴ� �Լ�
    //��� ĵ�� �˻��ؼ� ĵ�� ���ٸ� true�� ��ȯ, ĵ�� �� �� ���ִٸ� false�� ��ȯ
    //���尡 ����ִ��� Ȯ��
    public bool CheckEmptyCandy()
    {
        for (int i = 0; i < row; i++)
            for (int j = 0; j < col; j++)
                if (board[i, j] == 0) return true;
        return false;
    }


    //�Է¹��� ��ǥ�� 0�� ���� (�Է¹��� ��ǥ�� ������ ����)
    public void BoardChecking(int i, int j, GameObject candy)
    {
        candy.GetComponent<Candy>().isMoving = false;
        board[i, j] = 1;
        candyObjectBoard[i, j] = candy;

    }

    //�Է¹��� ��ǥ�� 0�� ���� (�Է¹��� ��ǥ�� ������ ����)
    public void BoardUnChecking(int i, int j)
    {
        candy.GetComponent<Candy>().isMoving = true;
        board[i, j] = 0;
        candyObjectBoard[i, j] = null;
    }


    //ĵ����� �˻��� ���� ����� ���� �̻� ������ �������� ����� �Լ�
    public void CheckThreeCandy()
    {
        if (CheckBoardFull())
        {
            Debug.Log("Ȯ����..");
            //���η� ���ӵ� ĵ�� �˻�
            for (int i = 0; i < row; i++)
            {
                int matchCount = 1; //���� ����� � �ִ��� �����ϴ� ����
                for (int j = 0; j < col - 1; j++)
                {
                    if (candyObjectBoard[i, j] != null && candyObjectBoard[i, j + 1] != null)
                    {
                        int currentCandyType = candyObjectBoard[i, j].GetComponent<Candy>().candyType;
                        int nextCandyType = candyObjectBoard[i, j + 1].GetComponent<Candy>().candyType;

                        //���� ���� ĵ��� ����ĵ���� Ÿ���� ���ٸ� matchCount�� ������Ų��.
                        if (currentCandyType != -1 && currentCandyType == nextCandyType)
                        {
                            matchCount++;
                            //�������̶��
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
                            //���� ĵ��� ����ĵ���� Ÿ���� ���� �ʴٸ� matchCount�� ������ Ȯ���ϰ�
                            //3�� �̻��̶�� DestroyCandy()�Լ� ȣ��, matchCount�� 1�� �ʱ�ȭ

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
            Debug.Log("��!");
            BoardUnChecking(candy.GetComponent<Candy>().currentBoardX, candy.GetComponent<Candy>().currentBoardY);
            Destroy(candy);
        }


    }


    //ĵ������Լ�
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
