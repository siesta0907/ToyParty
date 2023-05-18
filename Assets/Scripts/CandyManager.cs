using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//터치와 스와이프 관련 스크립트

public class CandyManager : MonoBehaviour
{

    private Touch touch; //터치 입력을 받는 변수
   
    GameObject startCandy, endCandy; //스와이프할때 시작 캔디와 끝 캔디
    float swipeTime = 0.5f; //스와이프되는 시간

    private void Update()
    {
        TouchDetect();
    }


    //터치 감지하는 함수
    public void TouchDetect()
    {
        if (Input.touchCount > 0) //터치가 되었을 때 
        {
            touch = Input.touches[0]; // 첫 번째 터치만 처리
            if (touch.phase == TouchPhase.Began)
            {
                Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position); //스크린 좌표를 월드 좌표로
                Collider2D collider = Physics2D.OverlapPoint(touchPosition); //터치를 한 지점에 캔디 콜라이더가 있는지 검사, 
                if (collider != null) 
                {
                    startCandy = collider.gameObject; //터치를 한 캔디 오브젝트 저장 
                }
            }

            else if (touch.phase == TouchPhase.Ended) // 터치가 끝났을때 스와이프 판정
            {
                Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position); //스크린 좌표를 월드 좌표로
                Collider2D collider = Physics2D.OverlapPoint(touchPosition); 
                if (collider != null)
                {
                    endCandy = collider.gameObject; //터치가 끝난 지점의 캔디 오브젝트 저장

                    //만약 터치 그리고 스와이프한 두 캔디가 인접해있다면 두 캔디의 위치를 바꾼다
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
        //캔디가 생성중일때는 스와이프 못하게

        //둘중에 하나가 팽이면 스와이프 못함
        
        //둘다 팽이면 스와이프 가능 특수효과

        //인접해있을때만 스와이프 가능하게
        int deltaX = candy1.GetComponent<Candy>().currentBoardX - candy2.GetComponent<Candy>().currentBoardX;
        int deltaY = candy1.GetComponent<Candy>().currentBoardY - candy2.GetComponent<Candy>().currentBoardY;
        
        if (deltaX == deltaY && (deltaX == -1 || deltaX == 1)) return false;
        if (deltaX <= 1 && -1<=deltaX && deltaY <= 1 && -1 <= deltaY) return true;
        else return false;
    }

    // 두 캔디를 자연스럽게 스와이프하는 함수
    IEnumerator SwipeCandy(GameObject candy1, GameObject candy2)
    {
        Vector2 startpos = candy1.transform.position; //시작캔디의 위치 저장
        Vector2 endpos = candy2.transform.position; //끝 캔디의 위치저장
        float time = 0f;

        //자연스럽게 두 오브젝트 스왑
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
    //스왑했을 때 보드 좌표값을 업데이트해주는 코드
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
