using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class RecordingPage : MonoBehaviour
{
    public AudioSource musicBgSource;
    
    [Header("Fairy Details")]
    private SoFrameList _currentFrameList;
    
    [Header("Tooth Placement Details")]
    public SpriteRenderer spriteRenderer,arObjectSpriteRenderer;

    [SerializeField] private ExampleScreenRecorder recorder;
    public float frameRate = 12f;

    [SerializeField] private MainPage mainPage;
    [SerializeField] private GameObject resultPage;
    [SerializeField] private ArObjectPlacer arObjectPlacer;

    private CancellationTokenSource _cancellationTokenSource;
    
    public void ShowPage(SoFrameList frameList)
    {
        _currentFrameList = frameList;
        arObjectPlacer.TogglePlaneDetection(false);
        this.gameObject.SetActive(true);
        PlaySequence();
    }
    
    private async void PlaySequence()
    {
        if (_cancellationTokenSource != null)
        {
            _cancellationTokenSource?.Cancel();
        }

        _cancellationTokenSource = new CancellationTokenSource();
        CancellationToken token = _cancellationTokenSource.Token;
        
        musicBgSource.Play();
        CopyFrom(spriteRenderer.transform, arObjectSpriteRenderer.transform);
        spriteRenderer.gameObject.SetActive(true);
        arObjectSpriteRenderer.gameObject.SetActive(false);
        mainPage.gameObject.SetActive(false);
        recorder.StartRecording();

        int delay = Mathf.RoundToInt(1000f / frameRate);
        
        try
        {
            foreach (var t in _currentFrameList.frameList)
            {
                token.ThrowIfCancellationRequested();

                spriteRenderer.sprite = t;
                await Task.Delay(delay, token);
            }

            StopRecording();
        }
        catch (TaskCanceledException)
        {
            Debug.Log("Playback stopped.");
            //StopRecording();
        }
    }

    public void StopRecording()
    {
        _cancellationTokenSource?.Cancel();
        recorder.StopRecording();
        spriteRenderer.gameObject.SetActive(false);
        musicBgSource.Stop();
        resultPage.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
    }
    
    private static void CopyFrom(Transform target, Transform source)
    {
        target.position = source.position;
        target.rotation = source.rotation;
        target.localScale = source.localScale;
    }
}
