using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBorderHandler : MonoBehaviour
{
    private float minX,minY;
    private float xOffset=0.35f;
    private float screenWidth,screenHeight;

    public Transform deathZone=null;

    public Transform blockSpawner;
    public Transform blockHider;
   
    void Start()
    {
     InitializeBorder();   
    }

    void InitializeBorder()
    {
        minX=Camera.main.ScreenToWorldPoint(new Vector3(0f,0f,0f)).x;
        minY=Camera.main.ScreenToWorldPoint(new Vector3(0f,0f,0f)).y;
        screenWidth=Mathf.Abs(minX)*2f;
        screenHeight=Mathf.Abs(minY)*2f;
        transform.GetChild(0).position=new Vector2(minX-xOffset,0f);
        transform.GetChild(1).position=new Vector2(Mathf.Abs(minX)+xOffset,0f);
        SetBorderSize(screenHeight);
    }

    void SetBorderSize(float borderHeight)
    {
       for(int i=0;i<transform.childCount;i++)
       {
        transform.GetChild(i).GetComponent<BoxCollider2D>().size=new Vector2(1f,borderHeight);
        transform.GetChild(i).gameObject.SetActive(true);
       }

       deathZone.position=new Vector2(0f,minY-0.5f);
       deathZone.GetComponent<BoxCollider2D>().size=new Vector2(screenWidth,1f);
       deathZone.gameObject.SetActive(true);

       blockSpawner.position=new Vector2(0f,Mathf.Abs(minY)+30f);
       blockHider.position=new Vector2(0f,minY-30f);//for hiding the blocks
    }
}
