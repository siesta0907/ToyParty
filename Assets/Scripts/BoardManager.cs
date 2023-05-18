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
    public int[,] board =  { { -1, 0, 0, 0, 0 },  { 0, 0, 0, 0, 0 },   { 0, 0, 0, 0, 0 },   { 0, 0, 0, 0, 0 },   { 0, 0, 0, 0, -1 } };

    //ĵ����� ������Ʈ���� ���� ���߹迭
    public GameObject[,] candyObjectBoard = new GameObject[row,col];

    [SerializeField]
    public GameObject candy; //ĵ�� ������
    
    public static Vector3 top = new Vector3(0f, 2.5f,0f); //ĵ�� �����Ǵ� ĭ
    private float spawnCandyTime = 1f; //ĵ�𸮽����ð�
    private float spawnTimer, downTimer = 0f; //ĵ�� ������ Ÿ�� ��꿡 ���̴� ����

    private void Update()
    { 
        //CandiesDown();
    }
    private void Start()
    {
        if (!CheckBoardFull()) InvokeRepeating("makeCandy", 0f, 0.7f);

       // InvokeRepeating("CandiesDown", 0f, 1f);
    }

    //���尡 �� ���ִ��� Ȯ���Ѵ�.
    public bool CheckBoardFull()
    {
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                //���尡 �ִµ� object�� �� ���� �ʴٸ� false ��ȯ�Ѵ�.
                if (board[i, j] != -1 && candyObjectBoard[i, j] == null) return false;
            }
        }
        return true;
    }

    public void CandiesDown()
    {
        //Debug.Log("Ȯ����");
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

    //�Է¹��� ��ǥ�� 1�� ���� (�Է¹��� ��ǥ�� ������ �ִ�)
    public void BoardChecking(int i, int j, GameObject candy)
    {
        board[i, j] = 1;
        candyObjectBoard[i, j] = candy;
    }

    //�Է¹��� ��ǥ�� 0�� ���� (�Է¹��� ��ǥ�� ������ ����)
    public void BoardUnChecking(int i, int j)
    {
        board[i, j] = 0;
        candyObjectBoard[i, j] = null;
    }


    //ĵ����� �˻��� ���� ����� ���� �̻� ������ �������� ����� �Լ�
    public void CheckThreeCandy()
    {
      
            //���η� ���ӵ� ĵ�� �˻�
            for (int i = 0; i < row; i++)
            {
                int matchCount = 1; //���� ����� � �ִ��� �����ϴ� ����
                for (int j = 0; j < col - 1; j++)
                {
                    if (candyObjectBoard[i, j] != null && candyObjectBoard[i, j + 1] != null)
                    {
                        int currentCandyType = candyObjectBoard[i, j].GetComponent<Candy>().candyType; //���� �˻��ϴ� ĵ���� Ÿ��
                        int nextCandyType = candyObjectBoard[i, j + 1].GetComponent<Candy>().candyType; //���� �˻��ϴ� ĵ�� ������ Ÿ��

                        //���� ���� ĵ��� ����ĵ���� Ÿ���� ���ٸ� matchCount�� ������Ų��.
                        if (currentCandyType == nextCandyType)
                        {
                            matchCount += 1;
                            //���� ĵ�� �˻��� �������̰� matchCount�� 3�� �̻��̶��,  
                            if (j == col - 2 && matchCount >= 3)
                            {
                                //matchCount ��ŭ �ݺ��ϸ鼭 ĵ�� ���ش�. 
                                for (int c = 0; c < matchCount; c++)
                                {
                                    DestroyCandy(candyObjectBoard[i, j + 1 - c]);

                                }
                                //matchCount�� 1�� �ʱ�ȭ�Ѵ�. 
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


    //ĵ������Լ�
    public void makeCandy()
    {
        Instantiate(candy, top, Quaternion.identity);
    }
    
}
