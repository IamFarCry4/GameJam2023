using UnityEngine;

public class Blocks : MonoBehaviour
{
    private Transform blockHider;

    private float minXpos;

    public Transform[] sideBlocks;

    public Transform obstacleContainer;
    private int obstacleIndex=0;
    private Transform selectedObstacles=null;

    void Start()
    {
        blockHider=GameObject.FindGameObjectWithTag("blockHider").transform;
        AdjustPosition();
    }

    void OnEnable()
    {
        if(obstacleContainer==null)
        {
            return;
        }
        for(int i=0;i<obstacleContainer.childCount;i++)
        {
            obstacleContainer.GetChild(i).gameObject.SetActive(false);
        }
        //lets get the random index 
        obstacleIndex=Random.Range(0,obstacleContainer.childCount);
        selectedObstacles=obstacleContainer.GetChild(obstacleIndex);
        for(int i=0;i<selectedObstacles.childCount;i++)
        {
            selectedObstacles.GetChild(i).gameObject.SetActive(true);
        }
        selectedObstacles.gameObject.SetActive(true);
    }

   void Update()
   {
    if(transform.position.y<blockHider.position.y)
    {
        LevelManager.instance.ResetBlocks(this.transform);
    }
   }

   void AdjustPosition()
   {
        minXpos=Camera.main.ScreenToWorldPoint(new Vector3(0f,0f,0f)).x;
        if(sideBlocks.Length>0)
        {
            sideBlocks[0].localPosition=new Vector2(minXpos+2.3f,0f);
            sideBlocks[1].localPosition=new Vector2(Mathf.Abs(minXpos)-2.3f,0f);
        }
   }
}
