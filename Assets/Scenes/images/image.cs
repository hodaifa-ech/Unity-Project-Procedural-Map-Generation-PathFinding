using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ImageController : MonoBehaviour
{
    public float transitionDuration = 1f;
    public float imageDuration = 5f;
    public Image[] images;
    private int currentIndex = 0;
    private int nextIndex = 1;
    private Color startColor;
    private Color endColor;

    void Start()
    {
        StartCoroutine(TransitionLoop());
    }

    IEnumerator TransitionLoop()
    {
        while (true)
        {
            // Transition from current image to next image
            yield return StartCoroutine(TransitionImage(images[currentIndex], images[nextIndex]));

            // Move to the next indices for transition
            currentIndex = (currentIndex + 1) % images.Length;
            nextIndex = (nextIndex + 1) % images.Length;
        }
    }

    IEnumerator TransitionImage(Image currentImage, Image nextImage)
    {
        // Transition from current image to next image
        startColor = currentImage.color;
        endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        float startTime = Time.time;
        while (Time.time - startTime < transitionDuration)
        {
            float normalizedTime = (Time.time - startTime) / transitionDuration;
            currentImage.color = Color.Lerp(startColor, endColor, normalizedTime);
            nextImage.color = Color.Lerp(endColor, startColor, normalizedTime); // Start transitioning next image simultaneously
            yield return null;
        }
        currentImage.color = endColor; // Ensure the final color is exactly the end color
        nextImage.color = startColor; // Ensure the final color is exactly the start color

        // Wait for the specified image duration
        yield return new WaitForSeconds(imageDuration - transitionDuration); // Adjust duration for smooth transition
    }
}
