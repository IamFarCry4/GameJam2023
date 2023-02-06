using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [System.Serializable]
    public class levelData
    {
        public int levelId;
        public Transform[] levelPrefabs;
    }
    public List<levelData> levelList=new List<levelData>();
    private Transform lastBlock=null;
   
    [Header("Block Spawn Hide Store")]
    public Transform iModeBlockStorage; //stores the hidden blocks of infinite mode
    public Transform iModeblockSpawner;//adds block on top of existing blocks
    public Transform iModeblockHider;//point which is used to identify which block to hide

    [Header("Infinite Mode")]
    public int iModeTotalBlocks=15;//we will spawn blocks at the start and loop them for infinite mode
    public List<Transform> infiPrefabList=new List<Transform>();

    void Start()
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

    #region levels
    public void LoadLevels(int levelIndex)
    {
        lastBlock=transform.GetChild(0);//we always have one block at the start of the game
        for(int i=0;i<levelList[levelIndex].levelPrefabs.Length;i++)
        {
            Transform curBlock=Instantiate(levelList[levelIndex].levelPrefabs[i].transform,transform.position,Quaternion.identity);
            curBlock.position=new Vector3(0f,lastBlock.GetChild(1).position.y-curBlock.GetChild(2).position.y,0f);
            curBlock.SetParent(this.transform);
            lastBlock=curBlock;
        }
    }
    #endregion

    #region infinite mode
    public void LoadInfinteMode()//this method is only called once //at the start of the infinite mode
    {
        lastBlock=transform.GetChild(0);//we always have one block at the start of the game
        for(int i=0;i<iModeTotalBlocks;i++)
        {
            int randomIndex=Random.Range(0,infiPrefabList.Count);
            Transform curBlock=Instantiate(infiPrefabList[randomIndex],transform.position,Quaternion.identity);
            curBlock.position=new Vector3(0f,lastBlock.GetChild(1).position.y-curBlock.GetChild(2).position.y,0f);
            curBlock.SetParent(this.transform);
            lastBlock=curBlock;
        }
        //we will instantiate certain number of blocks at start and try to loop them accordingly
    }

    void Update()
    {
        if(GameController.instance.gameStates==GameController.states.ingame&&GameController.instance.gameMode==GameController.mode.infinite)
        {
            if(lastBlock.position.y<iModeblockSpawner.position.y)
            {
                AddBlocksOnTop();
            }
        }
    }

    void AddBlocksOnTop()
    {
        int blockIndex=Random.Range(0,iModeBlockStorage.childCount);
        Transform tempBlock=iModeBlockStorage.GetChild(blockIndex);
        tempBlock.position=new Vector3(0f,lastBlock.GetChild(1).position.y-tempBlock.GetChild(2).position.y,0f);
        tempBlock.SetParent(this.transform);
        tempBlock.gameObject.SetActive(true);
        lastBlock=tempBlock;
    }

    public void ResetBlocks(Transform block)
    {
        block.position=Vector3.zero;
        block.eulerAngles=Vector3.zero;
        block.gameObject.SetActive(false);
        //we will not loop initial block later so we are keeping it out of the storage
        if(block.gameObject.tag=="initialBlock")
        {
            block.parent=null;
        }
        else
        {
            block.SetParent(iModeBlockStorage.transform);
        }  
    }
    #endregion
}
