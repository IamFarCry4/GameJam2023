using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialHandler : MonoBehaviour
{
    private Transform player;
    public float offsetY;

    private Vector3 gestureInitialPos,gestureTargetPos;
    private SpriteRenderer sRenderer;
    private Color sColor;
    private bool isAlphaAnimating=false;

    void OnEnable()
    {
        player=GameObject.FindGameObjectWithTag("Player").transform;
        gestureInitialPos=new Vector2(0f,player.position.y-offsetY);
        gestureTargetPos=new Vector2(gestureInitialPos.x,gestureInitialPos.y-3f);
        sRenderer=GetComponent<SpriteRenderer>();
        sColor=sRenderer.color;
        StartGestureAnimation();
    }

    void StartGestureAnimation()
    {
        iTween.ValueTo(gameObject, iTween.Hash("name", "gestureAnimation","from", gestureInitialPos,"to",gestureTargetPos,"onupdate", 
        "UpdateGesturePosition","loopType", iTween.LoopType.none, "easetype", iTween.EaseType.linear, "time",1.8f));
    }

    void UpdateGesturePosition(Vector3 gestureNewPos)
    {
        transform.position=gestureNewPos;
        if(Vector3.Distance(gestureInitialPos,gestureNewPos)<0.2f&&!isAlphaAnimating)
        {
            AlphaAnimation();
            isAlphaAnimating=true;
        }
    }

    void AlphaAnimation()
    {
        iTween.ValueTo(gameObject,iTween.Hash("name","gestureAlpha","from", 1f,"to",0f,"onupdate", 
        "UpdateGestureAlpha","oncomplete","ResetAnimation","loopType", iTween.LoopType.none, "easetype", iTween.EaseType.linear, "time",1.5f));
    }

    IEnumerator ResetAnimation()
   {
        yield return new WaitForSeconds(0.5f);
        ShowTutorial();
   }

    void ShowTutorial()
    {
        isAlphaAnimating=false;
        StartGestureAnimation();
    }

   void UpdateGestureAlpha(float fAlpha)
   {
        sColor.a=fAlpha;
        sRenderer.color=sColor;
   }

   public void HideTutorial()
   {
    iTween.FadeTo(gameObject,0f,1f);
   }
}
