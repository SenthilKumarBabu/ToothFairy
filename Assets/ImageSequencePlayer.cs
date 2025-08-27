using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ImageSequencePlayer : MonoBehaviour
{
    public AudioSource musicBgSource;
    public AudioClip[] musicBgClips;
    public TMP_Dropdown musicBgDropdown;
    public TMP_Text musicBgLabel;
    public Sprite[] frames;
    public float frameRate = 12f;
    public Image sr;
    public Button recordButton;

    public void ButtonClicked()
    {
        PlaySequence();
    }

    private async void PlaySequence()
    {
        musicBgSource.clip = musicBgClips[musicBgDropdown.value];
        musicBgSource.Play();
        recordButton.gameObject.SetActive(false);
        musicBgLabel.gameObject.SetActive(false);
        musicBgDropdown.gameObject.SetActive(false);
        sr.gameObject.SetActive(true);

        int delay = Mathf.RoundToInt(1000f / frameRate); // milliseconds per frame

        foreach (var t in frames)
        {
            sr.sprite = t;
            await Task.Delay(delay);
        }

        sr.gameObject.SetActive(false);
        musicBgDropdown.gameObject.SetActive(true);
        recordButton.gameObject.SetActive(true);
        musicBgLabel.gameObject.SetActive(true);
        musicBgSource.Stop();
    }
}