using UnityEngine;

public class DirectionIndicator : MonoBehaviour
{
    private SpriteRenderer dirSprite;
    private Color dSpriteColor;
    private bool isFadingIn,isFadingOut=false;

    void Start()
    {
        dirSprite=transform.GetChild(0).GetComponent<SpriteRenderer>();
        dSpriteColor=dirSprite.color;
    }

    public void FadeInIndicator()
    {
        // if(!isFadingIn)
        // {   
        //     iTween.ValueTo(gameObject, iTween.Hash("name", "dirIndicatorAlpha","from", 0f,"to",1f,"onupdate", 
        //     "UpdateIndicatorAlpha","oncomplete","FadeInCompleted","loopType", iTween.LoopType.none, "easetype", iTween.EaseType.linear, "time",0.1f));
        // }
        UpdateIndicatorAlpha(1f);
    }

    public void FadeOutIndicator()
    {
        if(!isFadingOut)
        {
            iTween.ValueTo(gameObject, iTween.Hash("name", "dirIndicatorAlpha","from", 1f,"to",0f,"onupdate", 
            "UpdateIndicatorAlpha","oncomplete","FadeOutComplete","loopType", iTween.LoopType.none, "easetype", iTween.EaseType.linear, "time",0.1f));
        }
    }

    void UpdateIndicatorAlpha(float dAlpha)
    {
        dSpriteColor.a=dAlpha;
        dirSprite.color=dSpriteColor;
    }

    void FadeInCompleted()
    {
        isFadingIn=false;
    }   

    void FadeOutComplete()
    {
        isFadingOut=false;
        //this.gameObject.SetActive(false);
    }
}
