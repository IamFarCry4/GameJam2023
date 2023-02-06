using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public enum behaviour
    {
        none,
        falling,
        rotating,
        LeftRight,
        UpDown
    }
    public behaviour obstacleType;

    private Vector3 initialPosition;
    private Vector3 initialRotation;

    private int emittedChilds=0;

    [Header("Up down details")]
    public float minYpos;
    public float maxYpos;
    public bool startFromTop=false;
    private Vector3 targetPosition=Vector3.zero;

    [Header("Left right details")]
    public float minXpos;
    public float maxXpos;
    public bool startFromLeft=false;

    void Start()
    {
        initialPosition=transform.localPosition;
        initialRotation=transform.eulerAngles;
    }

    void OnEnable()
    {
        //set obstacle position to start position 
        transform.localPosition=initialPosition;
        transform.eulerAngles=initialRotation;
        emittedChilds=0;
        HandleObstacleEffects();
    }

    void HandleObstacleEffects()
    {
        switch(obstacleType)
        {
            case behaviour.none:
            break;
            case behaviour.falling:
            Falling();
            break;
            case behaviour.rotating:
            Rotating();
            break;
            case behaviour.LeftRight:
            MovingLeftRight();
            break;
            case behaviour.UpDown:
            MovingUpAndDown();
            break;
        }
    }

    #region falling
    void Falling()
    {
        if(transform.childCount==0)
        {
            return;
        }
        //lets reset all childs before setting them again
        for(int i=0;i<transform.childCount;i++)
        {
            transform.GetChild(i).GetComponent<Rigidbody2D>().bodyType=RigidbodyType2D.Static;
            transform.GetChild(i).gameObject.SetActive(false);
            transform.localPosition=Vector3.zero;
        }
        //we have child objects to fall
        StartCoroutine(SpawnChildBlocks());
    }

    IEnumerator SpawnChildBlocks()
    {
        while(emittedChilds<transform.childCount-1)
        {
            transform.GetChild(emittedChilds).gameObject.SetActive(true);
            transform.GetChild(emittedChilds).GetComponent<Rigidbody2D>().bodyType=RigidbodyType2D.Dynamic;
            yield return new WaitForSeconds(2f);
            emittedChilds++;
        }
    }
    #endregion

    #region rotation
    void Rotating()
    {
        iTween.RotateBy(gameObject, iTween.Hash("z", 1.0f, "time", 3f,"easetype", "linear","looptype", iTween.LoopType.loop));
    }
    #endregion

    #region movement
    void MovingLeftRight()
    {
        if(startFromTop)
        {
            transform.position=new Vector3(maxXpos,transform.position.y,transform.position.z);
            targetPosition=new Vector3(minXpos,transform.position.y,transform.position.z);
        }
        else
        {
            transform.position=new Vector3(minXpos,transform.position.y,transform.position.z);
            targetPosition=new Vector3(maxXpos,transform.position.y,transform.position.z);
        }
        iTween.ValueTo(gameObject, iTween.Hash("name", "leftRightMovement","from", transform.position,"to",targetPosition,"onupdate", 
        "UpdateBlockPosition","loopType", iTween.LoopType.pingPong, "easetype", iTween.EaseType.linear, "time",2f));
    }

    void MovingUpAndDown()
    {
        if(startFromTop)
        {
            transform.position=new Vector3(transform.position.x,maxYpos,transform.position.z);
            targetPosition=new Vector3(transform.position.x,minYpos,transform.position.z);
        }
        else
        {
            transform.position=new Vector3(transform.position.x,minYpos,transform.position.z);
            targetPosition=new Vector3(transform.position.x,maxYpos,transform.position.z);
        }
        iTween.ValueTo(gameObject, iTween.Hash("name", "upDownMovement","from", transform.position,"to",targetPosition,"onupdate", 
        "UpdateBlockPosition","loopType", iTween.LoopType.pingPong, "easetype", iTween.EaseType.linear, "time",2f));
    }

    void UpdateBlockPosition(Vector3 curPosition)
    {
        transform.localPosition=curPosition;
    }
    #endregion
}
