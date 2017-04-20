using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VuforiaVideoSource : GenericVideoSource {

    private int width;
    private int height;

	// Use this for initialization
	void Start () {
        width = 0;
        height = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override bool Ready()
    {
        bool sizeSet = width != 0 && height != 0;
        if(!sizeSet)
        {
            GetPixels();
        }
        return sizeSet && Vuforia.CameraDevice.Instance.SetFrameFormat(Vuforia.Image.PIXEL_FORMAT.RGBA8888, true);
    }

    public override byte[] GetPixels()
    {
        var image = Vuforia.CameraDevice.Instance.GetCameraImage(Vuforia.Image.PIXEL_FORMAT.RGBA8888);
        if(image != null)
        {
            width = image.BufferWidth;
            height = image.BufferHeight;
            return image.Pixels;
        }
        return new byte[0];
    }

    public override int GetWidth()
    {
        return width;
    }

    public override int GetHeight()
    {
        return height;
    }
}
