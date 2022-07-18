using System.Collections;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    public int FramesPerSec { get; protected set; }

    [SerializeField] private float frequency = 0.5f;

    private void Awake()
    {
        StartCoroutine(FPS());
    }

    private IEnumerator FPS()
    {
        while(true)
        {
            int lastFrameCount = Time.frameCount;
            float lastTime = Time.realtimeSinceStartup;
            
            yield return new WaitForSeconds(frequency);

            float timeSpan = Time.realtimeSinceStartup - lastTime;
            int frameCount = Time.frameCount - lastFrameCount;

            FramesPerSec = Mathf.RoundToInt(frameCount / timeSpan);
        }
    }
}