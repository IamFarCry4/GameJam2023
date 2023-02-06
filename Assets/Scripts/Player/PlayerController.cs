using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [Header("Controls")]
    private Rigidbody2D rb2d;
    private bool isDragging=false;
    private Vector2 initialMousePos,curMousePos,dirVector,differenceVector;
    private Vector2 lastPlayerPosition=Vector2.zero;
    private Vector2 throwForce;
    public float dragForceMultiplier=5f;
    private float fixedDeltaTime;

    [Header("direction indicator")]
    public Transform directionIndicator;
    [Header("force indicator")]
    public Transform dragForceIndicator;
    public float maxDragLength;
    //collision
    private Vector3 hitPoint=Vector3.zero;

    [Header("Sprites")]
    private SpriteRenderer playerSprite;

    [Header("Cinemachine")]
    public float shakeIntensity=5f;
    public CinemachineVirtualCamera cinemachinegameCam;
    private CinemachineBasicMultiChannelPerlin camShaker;
    public CinemachineBrain brain;

    [Header("Pulse")]
    public Animator pulseAnimator;

    [Header("Particles")]
    public Transform deathParticle;

    ///score
    private Vector3 playerInitialPos=Vector3.zero;

    #region initialization
    void Awake()
    {
        if(instance!=null)
        {
            return;
        }
        else
        {
            instance=this;
        }
    }

    void Start()
    {
        rb2d=GetComponent<Rigidbody2D>();
        playerSprite=GetComponent<SpriteRenderer>();
        playerSprite.enabled=false;
        fixedDeltaTime=Time.fixedDeltaTime;
        directionIndicator.gameObject.SetActive(false);
        dragForceIndicator.gameObject.SetActive(false);
        camShaker=cinemachinegameCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        if(deathParticle!=null)
        {
            deathParticle.gameObject.SetActive(false);
        }
    }
    #endregion

    #region play
    public void EnablePlayer()
    {
        if(!GameController.instance.isTutorialActive)
        {
            rb2d.bodyType=RigidbodyType2D.Dynamic;
        }
        playerSprite.enabled=true;
        playerInitialPos=transform.position;
    }
    #endregion

    #region controls
    void Update()
    {
        if(GameController.instance.gameStates==GameController.states.ingame)
        {
            if(Input.GetMouseButtonDown(0))
            {
                directionIndicator.gameObject.SetActive(true);
                dragForceIndicator.gameObject.SetActive(true);
                Time.timeScale=0.2f;
                isDragging=true;
                initialMousePos=Camera.main.ScreenToWorldPoint(Input.mousePosition);
                //pulse animation
                PulseAnimation(true);
                if(GameController.instance.isTutorialActive)
                {
                    GameController.instance.DisableTutorial();
                }
            }

            if(Input.GetMouseButtonUp(0)&&isDragging)
            {
                Time.timeScale=1f;
                Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
                isDragging=false;
                Shoot();
            }

            if(isDragging)
            {
               curMousePos=Camera.main.ScreenToWorldPoint(Input.mousePosition); 
                differenceVector=curMousePos-initialMousePos;
                Vector3 newPos=transform.position+Vector3.ClampMagnitude(differenceVector,maxDragLength); 

                dragForceIndicator.position=newPos;
                float distanceFrom=Vector3.Distance(transform.position,newPos)/maxDragLength;
                dragForceIndicator.localScale=new Vector2(distanceFrom,distanceFrom);

                Vector3 dragDir=(transform.position-newPos);
                float dragAngle = Mathf.Atan2(dragDir.y,dragDir.x)* Mathf.Rad2Deg;
                dragForceIndicator.eulerAngles=new Vector3(0f,0f,dragAngle);
                throwForce=dragDir*dragForceMultiplier;

                //directionIndicator.position=transform.position;
                directionIndicator.position=transform.position+(dragDir/9f);
                directionIndicator.eulerAngles=new Vector3(0f,0f,dragAngle-90f);
            }
            else
            {
                if(transform.position.y<lastPlayerPosition.y)
                {
                    //do not follow
                    cinemachinegameCam.Follow=null;
                    lastScore=curScore;
                }
                else
                {
                    //follow player
                    cinemachinegameCam.Follow=this.transform;
                    lastPlayerPosition=transform.position;
                }
            }

            if(cinemachinegameCam.Follow!=null&&!GameController.instance.isTutorialActive)
            {
                Vector2 distDifference=transform.position-playerInitialPos;
                float heightReached=Vector2.Distance(playerInitialPos,transform.position)/4f;
                curScore=(int)heightReached;
                if(lastScore<curScore)
                {
                    GameController.instance.AddScore(curScore);
                }
            }
        }
    }

    private int curScore=0;
    private int lastScore=0;

    void Shoot()
    {
        // rb2d.AddForce(throwForce,ForceMode2D.Impulse);
        if(rb2d.bodyType==RigidbodyType2D.Static)
        {
            rb2d.bodyType=RigidbodyType2D.Dynamic;
        }
        rb2d.velocity=throwForce;
        directionIndicator.gameObject.SetActive(false);
        dragForceIndicator.gameObject.SetActive(false);
        PulseAnimation(false);
        if(GameAudioHandler.instance!=null)
        {
            GameAudioHandler.instance.PlayThrowSound();
        }
    }
    #endregion

    #region collision
    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.layer==LayerMask.NameToLayer("border"))
        {
            foreach (ContactPoint2D colPoints in col.contacts)
            {
                hitPoint=colPoints.point;
            }
           Dead();
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.layer==LayerMask.NameToLayer("coin"))
        {
          //lets play coin particle effects here
          GameController.instance.collectedCoins+=1;
          col.gameObject.GetComponent<Coin>().Collected();
          if(GameAudioHandler.instance!=null)
          {
            GameAudioHandler.instance.PlayCoinSound();
          }
          //col.gameObject.SetActive(false);
          SaveSystemHandler.instance.UpdateData();
        }
    }

    void Dead()
    {
        rb2d.bodyType=RigidbodyType2D.Static;
        GameController.instance.GameOver();
        playerSprite.enabled=false;
        deathParticle.position=hitPoint;
        deathParticle.gameObject.SetActive(true);
        deathParticle.GetComponent<ParticleSystem>().Play();
        ShakeCamera();
        directionIndicator.gameObject.SetActive(false);
        dragForceIndicator.gameObject.SetActive(false);
        PulseAnimation(false);
        if(GameAudioHandler.instance!=null)
        {
            GameAudioHandler.instance.PlayDeathSound();
        }
    }
    #endregion

    #region camera shake
    void ShakeCamera()
    {
        iTween.ValueTo(gameObject, iTween.Hash("name", "camShake","from", 0f,"to",shakeIntensity,"onupdate", 
        "UpdateShake","oncomplete","ResetShake","loopType", iTween.LoopType.none, "easetype", iTween.EaseType.easeInCirc, "time",0.01f));
    }

    void UpdateShake(float intensity)
    {
       camShaker.m_AmplitudeGain = intensity;
    }

    void ResetShake()
    {
        iTween.ValueTo(gameObject, iTween.Hash("name", "resetShake","from", shakeIntensity,"to",0f,"onupdate", 
        "UpdateShake","loopType", iTween.LoopType.none, "easetype", iTween.EaseType.easeInCirc, "time",0.15f));
    }
    #endregion

    #region pulse
    void  PulseAnimation(bool pulse)
    {
        if(pulseAnimator!=null)
        {
            pulseAnimator.speed=0.1f;
            pulseAnimator.SetBool("pulse",pulse);
        }
    }
    #endregion
}
