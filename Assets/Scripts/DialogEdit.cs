using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogEdit : MonoBehaviour
{
    public static DialogEdit instance;

    private void Awake()
    {
        instance = this;
    }
    public GameObject DialogVariant;
    public TextMeshProUGUI TextMeshProUGUI;
    public void OnSubmit()
    {
        DialogVariant.GetComponentInChildren<DialogContentAffordance>().editDiologContent(TextMeshProUGUI.text);
    }
}
