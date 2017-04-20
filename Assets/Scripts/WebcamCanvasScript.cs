using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebcamCanvasScript : MonoBehaviour {
    
    private Canvas webcamTex;
    private Texture2D tex;
    private GenericVideoSource videoSource;
    private bool sizeSet;
    public bool canvasActive = true;

    // Use this for initialization
    void Start () {
        videoSource = GetComponent<GenericVideoSource>();
		webcamTex = GetComponent<Canvas>();
        webcamTex.GetComponent<RawImage>().texture = tex;
        sizeSet = false;

        Debug.Log(videoSource.GetType());
    }

    private void LateUpdate()
    {
        webcamTex.enabled = canvasActive;
        if (webcamTex.isActiveAndEnabled && videoSource.Ready())
        {
            byte[] texture = videoSource.GetPixels();
            SendFrameUpdate(texture, videoSource.GetWidth(), videoSource.GetHeight());
        }
    }

    private void SendFrameUpdate(byte[] pixels, int width, int height)
    {
        //TODO send frame to Frame Adapter Class

        var colorArray = new Color32[pixels.Length / 4];
        for (var i = 0; i < pixels.Length; i += 4)
        {
            var color = new Color32(pixels[i + 0], pixels[i + 1], pixels[i + 2], pixels[i + 3]);
            colorArray[i / 4] = color;
        }
        if (!sizeSet)
        {
            tex = new Texture2D(width, height);
            webcamTex.GetComponent<RawImage>().texture = tex;
            sizeSet = true;
        }
        tex.SetPixels32(colorArray);
        tex.Apply();
    }
}
