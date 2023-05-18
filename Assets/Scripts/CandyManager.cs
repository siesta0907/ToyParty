using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��ġ�� �������� ���� ��ũ��Ʈ

public class CandyManager : MonoBehaviour
{

    private Touch touch; //��ġ �Է��� �޴� ����
   
    GameObject startCandy, endCandy; //���������Ҷ� ���� ĵ��� �� ĵ��
    float swipeTime = 0.5f; //���������Ǵ� �ð�

    private void Update()
    {
        TouchDetect();
    }


    //��ġ �����ϴ� �Լ�
    public void TouchDetect()
    {
        if (Input.touchCount > 0) //��ġ�� �Ǿ��� �� 
        {
            touch = Input.touches[0]; // ù ��° ��ġ�� ó��
            if (touch.phase == TouchPhase.Began)
            {
                Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position); //��ũ�� ��ǥ�� ���� ��ǥ��
                Collider2D collider = Physics2D.OverlapPoint(touchPosition); //��ġ�� �� ������ ĵ�� �ݶ��̴��� �ִ��� �˻�, 
                if (collider != null) 
                {
                    startCandy = collider.gameObject; //��ġ�� �� ĵ�� ������Ʈ ���� 
                }
            }

            else if (touch.phase == TouchPhase.Ended) // ��ġ�� �������� �������� ����
            {
                Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position); //��ũ�� ��ǥ�� ���� ��ǥ��
                Collider2D collider = Physics2D.OverlapPoint(touchPosition); 
                if (collider != null)
                {
                    endCandy = collider.gameObject; //��ġ�� ���� ������ ĵ�� ������Ʈ ����

                    //���� ��ġ �׸��� ���������� �� ĵ�� �������ִٸ� �� ĵ���� ��ġ�� �ٲ۴�
                    if (CanSwipe(startCandy, endCandy))
                    {
                        StartCoroutine(SwipeCandy(startCandy, endCandy));
                        
                    }
                }
            }
        }
    }

    public bool CanSwipe (GameObject candy1, GameObject candy2)
    {
        //ĵ�� �������϶��� �������� ���ϰ�

        //���߿� �ϳ��� ���̸� �������� ����
        
        //�Ѵ� ���̸� �������� ���� Ư��ȿ��

        //�������������� �������� �����ϰ�
        int deltaX = candy1.GetComponent<Candy>().currentBoardX - candy2.GetComponent<Candy>().currentBoardX;
        int deltaY = candy1.GetComponent<Candy>().currentBoardY - candy2.GetComponent<Candy>().currentBoardY;
        
        if (deltaX == deltaY && (deltaX == -1 || deltaX == 1)) return false;
        if (deltaX <= 1 && -1<=deltaX && deltaY <= 1 && -1 <= deltaY) return true;
        else return false;
    }

    // �� ĵ�� �ڿ������� ���������ϴ� �Լ�
    IEnumerator SwipeCandy(GameObject candy1, GameObject candy2)
    {
        Vector2 startpos = candy1.transform.position; //����ĵ���� ��ġ ����
        Vector2 endpos = candy2.transform.position; //�� ĵ���� ��ġ����
        float time = 0f;

        //�ڿ������� �� ������Ʈ ����
        while (time < swipeTime)
        {
            float t = time / swipeTime;
            candy1.transform.position = Vector2.Lerp(startpos, endpos, t); 
            candy2.transform.position = Vector2.Lerp(endpos, startpos, t); 
            time += Time.deltaTime;
            yield return null;
        }

        candy1.transform.position = endpos;
        candy2.transform.position = startpos;

        SwipeXY(startCandy, endCandy);
    }
    //�������� �� ���� ��ǥ���� ������Ʈ���ִ� �ڵ�
    public void SwipeXY(GameObject candy1, GameObject candy2)
    {
        int tmpX, tmpY;
        tmpX = candy1.GetComponent<Candy>().currentBoardX;
        tmpY = candy1.GetComponent<Candy>().currentBoardY;
        candy1.GetComponent<Candy>().currentBoardX = candy2.GetComponent<Candy>().currentBoardX;
        candy1.GetComponent<Candy>().currentBoardY = candy2.GetComponent<Candy>().currentBoardY;
        candy2.GetComponent<Candy>().currentBoardX = tmpX;
        candy2.GetComponent<Candy>().currentBoardY = tmpY;
    }


}
