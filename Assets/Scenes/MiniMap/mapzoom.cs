using System.Collections;
using UnityEngine;

public class mapzoom : MonoBehaviour
{
    public Camera mainCamera;
    private bool isZoomed = false;
    private float startSize;
    private float endSize;
    private float zoomDuration = 0.5f;

    private void Start()
    {
        startSize = mainCamera.orthographicSize;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            isZoomed = !isZoomed;
            StopAllCoroutines();
            StartCoroutine(Zoom(isZoomed));
        }
    }

    private IEnumerator Zoom(bool isZoomIn)
    {
        float elapsedTime = 0f;
        float startZoom = mainCamera.orthographicSize;
        float targetZoom = isZoomIn ? 15f : startSize;

        while (elapsedTime < zoomDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / zoomDuration);
            mainCamera.orthographicSize = Mathf.Lerp(startZoom, targetZoom, t);
            yield return null;
        }
    }
}
