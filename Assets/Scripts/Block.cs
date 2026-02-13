using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDestroy()
    {
        DataManager.Instance.SetIndexByPos(new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z));
        Debug.Log("É¾³ý³É¹¦");
    }
}
