using UnityEngine;

public class DashAfterimageScript : MonoBehaviour
{
    private float fadeDuration = 0.6f;

    private Renderer ballRenderer;
    private Material fadeMaterial;
    private float elapsedTime;
    private Color startColor;

    private void Awake()
    {
        ballRenderer = GetComponentInChildren<Renderer>();

        // Create a unique material instance for this afterimage.
        fadeMaterial = ballRenderer.material;
        startColor = fadeMaterial.color;
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        Color color = startColor;
        color.a = Mathf.Lerp(startColor.a, 0f, elapsedTime / fadeDuration);
        fadeMaterial.color = color;

        if (elapsedTime >= fadeDuration)
            Destroy(gameObject);
    }
}