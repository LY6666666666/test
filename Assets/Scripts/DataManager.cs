using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance
    {
        get;
        private set;
    }
    private void Awake()
    {
        Instance = this;
    }

    public BlockDatas blockDatas = new BlockDatas();
    BlockPlatte blockPlatte;
    //特殊方块索引如0代表空气
    public ushort specialBlockIndex=0;
    public ushort defaultBlockIndex = 1;

    //方块数据
    public BlockArchiveDatas curBlockArchiveDatas;
    //预制体数据

    public Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        blockPlatte = new BlockPlatte(blockDatas);

        //保存数据

        //从文件中加载数据
        curBlockArchiveDatas=LoadArchiveData(Application.streamingAssetsPath + "/1.json");

        //加载方块数据
        //LoadArchiveData(blockArchiveDatas,new Vector3Int(1,0,1));
        LoadArchiveDatas(curBlockArchiveDatas);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            SaveArchiveData(Application.streamingAssetsPath + "/1.json",curBlockArchiveDatas);
            Debug.Log("保存成功");
        }
    }
    public BlockArchiveDatas LoadArchiveData(string file)
    {
        string json = File.ReadAllText(file);
        BlockArchiveDatas datas=JsonUtility.FromJson<BlockArchiveDatas>(json);
        
        //foreach(var i in datas.blockArchiveDatas)
        //{
        //    LoadArchiveData(i.blockArchiveData,new Vector3Int(i.x,i.y,i.z));
        //}
        return datas;
    }
    public void SaveArchiveData(string file,BlockArchiveDatas blockArchiveDatas)
    {
        string json=JsonUtility.ToJson(blockArchiveDatas);
        File.WriteAllText(file, json);
    }

    public void LoadArchiveData(ushort[] arr,Vector3Int areaIndex,byte areaSize=16)
    {
        BlockData blockData = null;
        GameObject block = null;
        int x=0;
        int y=0;
        int z=0;

        for (int i=0;i<arr.Length;i++)
        {
            if (arr[i] == specialBlockIndex)
                continue;
            blockData=blockPlatte.GetBlockDataByIndex(arr[i]);
            Debug.Log(blockData);
            block=InstantiteBlock(blockData);
            z = i/ areaSize % areaSize+ areaIndex.z * areaSize;
            x = i % areaSize+areaIndex.x*areaSize;
            y = i / areaSize / areaSize+ areaIndex.y * areaSize;
            block.transform.position=new Vector3(x, y, z);
        }
    }
    public void LoadArchiveDatas(BlockArchiveDatas blockArchiveDatas, byte areaSize = 16)
    {
        for(int i=0;i<blockArchiveDatas.blockArchiveDatas.Count;i++)
        {
            LoadArchiveData(blockArchiveDatas.blockArchiveDatas[i].blockArchiveData, new Vector3Int(blockArchiveDatas.blockArchiveDatas[i].x, blockArchiveDatas.blockArchiveDatas[i].y, blockArchiveDatas.blockArchiveDatas[i].z),areaSize);
        }
    }
    public GameObject InstantiteBlock(BlockData blockData)
    {
        if(prefabs.ContainsKey(blockData.res))
        {
            return GameObject.Instantiate(prefabs[blockData.res]);
        }
        else
        {
            GameObject prefab=Resources.Load<GameObject>("Blocks\\" + blockData.res);
            prefabs.Add(blockData.res, prefab);
            return GameObject.Instantiate(prefabs[blockData.res]);
        }
    }

    public void SetIndexByPos(Vector3Int pos,int index=-1, byte areaSize = 16)
    {
        //确认区块
        Vector3Int areaIndex = new Vector3Int(pos.x / areaSize, pos.y/ areaSize, pos.z/ areaSize);

        BlockArchiveData blockArchiveData = null;
        foreach(var j in curBlockArchiveDatas.blockArchiveDatas)
        {
            if (j.x == areaIndex.x && j.y == areaIndex.y && j.z == areaIndex.z)
            {
                blockArchiveData = j;
                break;
            }

        }
        blockArchiveData.blockArchiveData[(pos.y % areaSize) * areaSize * areaSize + (pos.z % areaSize) * areaSize + pos.x % areaSize]= index < 0 ? specialBlockIndex : (ushort)index; ;
    }
}
[System.Serializable]
public class BlockData
{
    public int id;
    public string name;
    public string res;
    public string sprite;
}
[System.Serializable]
public class BlockDatas
{
    public List<BlockData> blockDatas = new List<BlockData>();
}
public class BlockPlatte
{
    public BlockDatas blockDatas;
    public BlockPlatte(BlockDatas blockDatas)
    {
        this.blockDatas = blockDatas;
    }
    public BlockData GetBlockDataByIndex(ushort index)
    {
        if (blockDatas.blockDatas.Count <= index)
        {
            Debug.LogError("GetBlockDataByIndex:索引超出范围");
            return null;
        }
        return blockDatas.blockDatas[index];
    }
    public ushort ParseIndexByID(int id)
    {
        //blockDatas.blockDatas.Find((i) => { return i.id == id; });
        for(ushort i=0;i<blockDatas.blockDatas.Count;i++)
        {
            if(blockDatas.blockDatas[i].id==id)
            {
                return i;
            }
        }
        //默认方块
        return 1;
    }
}
[System.Serializable]
public class BlockArchiveData
{
    public int x;
    public int y;
    public int z;
    public ushort[] blockArchiveData;
}
[System.Serializable]
public class BlockArchiveDatas
{
    public List<BlockArchiveData> blockArchiveDatas = new List<BlockArchiveData>();
}