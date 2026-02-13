using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBlockItem : MonoBehaviour
{
    public ushort index;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => {
            UIBlockManager.Instance.curIndex = index;
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
