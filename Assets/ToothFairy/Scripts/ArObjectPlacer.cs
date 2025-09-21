using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ArObjectPlacer : MonoBehaviour
{
    [SerializeField] private ARRaycastManager raycastManager;
    [SerializeField] private ARPlaneManager planeManager;
    [SerializeField] private SpriteRenderer arFrame;
    [SerializeField] private MainPage mainPage;

    private bool _isPlacing;
    private static readonly List<ARRaycastHit> RayHits = new List<ARRaycastHit>();
    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }
    
    private void Update()
    {
        if (_isPlacing) return;

        bool isTouch = Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
        bool isClick = Input.GetMouseButtonDown(0);

        if (!isTouch && !isClick) return;

        if (!mainPage.gameObject.activeInHierarchy) return;

        if (EventSystem.current != null)
        {
            if (isTouch && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                return;

            if (isClick && EventSystem.current.IsPointerOverGameObject())
                return;
        }
        
        StartCoroutine(HandlePlacement());
    }

    private IEnumerator HandlePlacement()
    {
        yield return new WaitForSeconds(0.1f);

        _isPlacing = true;

        Vector3 inputPos = Input.touchCount > 0 
            ? (Vector3)Input.GetTouch(0).position 
            : Input.mousePosition;

        PlaceObject(inputPos);

        yield return StartCoroutine(ResetPlacingFlag());
    }

    private void PlaceObject(Vector3 screenPosition)
    {
        if (!raycastManager.Raycast(screenPosition, RayHits, TrackableType.AllTypes))
            return;

        var hit = RayHits[0];
        Vector3 hitPos = hit.pose.position;

        arFrame.transform.position = hitPos;

        Vector3 lookDir = _mainCamera.transform.position - hitPos;
        lookDir.y = 0f;
        arFrame.transform.rotation = Quaternion.LookRotation(-lookDir, Vector3.up);

        ScaleToViewport(hitPos);

        arFrame.gameObject.SetActive(true);
        mainPage.ObjectPlaced();
    }

    private void ScaleToViewport(Vector3 worldPos)
    {
        float distance = Vector3.Distance(_mainCamera.transform.position, worldPos);

        Vector3 left = _mainCamera.ViewportToWorldPoint(new Vector3(0, 0.5f, distance));
        Vector3 right = _mainCamera.ViewportToWorldPoint(new Vector3(1, 0.5f, distance));
        float worldWidth = Vector3.Distance(left, right);

        float targetWidth = worldWidth * 0.5f;
        float spriteWidth = arFrame.sprite.bounds.size.x;

        float scaleFactor = targetWidth / spriteWidth;
        arFrame.transform.localScale = Vector3.one * scaleFactor;
    }

    private IEnumerator ResetPlacingFlag()
    {
        yield return new WaitForSeconds(0.25f);
        _isPlacing = false;
    }

    public void TogglePlaneDetection(bool enable)
    {
        if (planeManager == null) return;

        planeManager.enabled = enable;

        foreach (var plane in planeManager.trackables)
        {
            plane.gameObject.SetActive(enable);
        }
    }
}