using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogEdit : MonoBehaviour
{
    public bool isEditingDialog;
    public static DialogEdit instance;

    private void Awake()
    {
        instance = this;
        isEditingDialog = false;
    }
    public GameObject DialogVariant;
    public TextMeshProUGUI TextMeshProUGUI;
    public TMP_InputField TMP_InputField;

    public TextMeshProUGUI currentIndex;
    public TextMeshProUGUI totalCount;

    public void setIsEditingDialogTrue()
    {
        isEditingDialog = true;
    }

    public void setIsEditingDialogFalse()
    {
        isEditingDialog = false;
    }
    public void OnSubmit()
    {
        DialogVariant.GetComponent<DialogContentAffordance>().editDiologContent(TextMeshProUGUI.text);
    }

    public void OnClickNext()
    {
        DialogVariant.GetComponent<DialogContentAffordance>().nextDialog();
    }

    public void OnClickDelete()
    {
        DialogVariant.GetComponent<DialogContentAffordance>().deleteDialog();
    }
    public void RefreshTextMeshPro()
    {
        TMP_InputField.text = DialogVariant.GetComponent<DialogContentAffordance>().m_DialogContent[DialogVariant.GetComponent<DialogContentAffordance>().m_CurrentDialogIndex];
        currentIndex.text = DialogVariant.GetComponent<DialogContentAffordance>().m_CurrentDialogIndex+1+"";
        totalCount.text = DialogVariant.GetComponent<DialogContentAffordance>().m_DialogCount+1+ "";
    }
    public void OnClickLast()
    {
        DialogVariant.GetComponent<DialogContentAffordance>().lastDialog();
   }

    public void OnClickAdd()
    {
        DialogVariant.GetComponent<DialogContentAffordance>().addDialog();
    }

    public void OnClickPreview()
    {
        DialogVariant.GetComponent<DialogContentAffordance>().play();
    }
    public void LogContent()
    {
        foreach(var s in DialogVariant.GetComponent<DialogContentAffordance>().m_DialogContent)
            Debug.Log(s);
    }
}
