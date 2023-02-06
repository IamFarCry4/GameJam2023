using UnityEngine;

public class Coin : MonoBehaviour
{
    private SpriteRenderer coinSprite;
    private Color coinColor;
    private float fscale=0f;
    private float bgAlpha=0f;
    private bool isCollected=false;
    private Collider2D col;

    void Start()
    {
        // coinSprite=GetComponent<SpriteRenderer>();
        // coinColor=coinSprite.color;
        // col=GetComponent<Collider2D>();
    }

    void OnEnable()
    {
        coinSprite=GetComponent<SpriteRenderer>();
        coinColor=coinSprite.color;
        col=GetComponent<Collider2D>();

        isCollected=false;
        //coinSprite.color=new Color(1f,1f,1f,1f);
        col.enabled=true;
    }

   public void Collected()
   {
        if(!isCollected)
        {
            col.enabled=false;
            iTween.ValueTo(gameObject, iTween.Hash("name", "coinAlpha","from", 0.8f,"to",0f ,"onupdate", 
            "UpdateCoinAlpha","oncomplete","HideThisCoin","loopType", iTween.LoopType.none, "easetype", iTween.EaseType.linear, "time",0.3f));
            isCollected=true;
        }
   }

   void UpdateCoinAlpha(float alpha)
   {
    coinColor.a=alpha;
    coinSprite.color=coinColor;
    fscale=1.5f-alpha;
    transform.localScale=new Vector3(fscale,fscale,fscale);
   }

   void HideThisCoin()
   {
    this.gameObject.SetActive(false);
   }
}
