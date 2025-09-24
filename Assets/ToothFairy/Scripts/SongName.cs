using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SongName : MonoBehaviour
{
    public GameObject highlightBg;
    public TMP_Text songNameText;

    public void Initialize(string songName)
    {
        songNameText.text = songName;
        highlightBg.SetActive(false);
    }
}
