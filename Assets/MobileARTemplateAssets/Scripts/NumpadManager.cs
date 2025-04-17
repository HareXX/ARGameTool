using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NumpadManager : MonoBehaviour
{
    [SerializeField]
    public GameObject[] Num;

    public bool[] isPressed = new bool[10];


    int pressedNumCnt = 0;

    [SerializeField]
    public TMP_Text screenText;

    public void press(int num)
    {
        if (pressedNumCnt == 3) this.Invoke("clear", 3);
        if (isPressed[num] == true) return;
        Debug.Log("按下了" + '0' + num);

        isPressed[num] = true;
        ++pressedNumCnt;
        screenText.text += num;
    }


    void clear()
    {
        for (int i = 0; i < 10; ++i) isPressed[i] = false;
        screenText.text = "<mspace=0.8em>";
        pressedNumCnt = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 10; ++i) isPressed[i] = false;
        screenText.text = "<mspace=0.8em>";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
