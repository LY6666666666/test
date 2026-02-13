using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBlockSelect : MonoBehaviour
{
    public GameObject prefab;
    public List<GameObject> showedBlocks = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        LoadUISelectData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void LoadUISelectData()
    {
        foreach(var i in UIBlockManager.Instance.indexList)
        {
            GameObject obj=GameObject.Instantiate(prefab, transform);
            obj.SetActive(true);
            obj.GetComponent<UIBlockItem>().index =i;
            obj.GetComponentInChildren<Image>().sprite=Resources.Load<Sprite>("Sprites/"+DataManager.Instance.blockDatas.blockDatas[i].sprite);
        }
    }
}
