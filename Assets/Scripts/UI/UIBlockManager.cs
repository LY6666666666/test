using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBlockManager:MonoBehaviour
{
    public ushort curIndex;
    public static UIBlockManager Instance
    {
        get;
        private set;
    }
    public List<ushort> indexList = new List<ushort>();
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        
    }
}
