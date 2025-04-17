using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumKey : MonoBehaviour
{
    [SerializeField]
    public int num;

    public void press()
    {
        transform.parent.parent.GetComponent<NumpadManager>().SendMessage("press", num);
        Debug.Log("按下了" + '0' + num);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
