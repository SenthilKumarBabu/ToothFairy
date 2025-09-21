using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AlertPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text messageText;

    public void ShowAlert(string message)
    {
        messageText.text = message;
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

}
