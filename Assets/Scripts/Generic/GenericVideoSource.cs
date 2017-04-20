using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GenericVideoSource : MonoBehaviour {
    
    // Should return a byte array filled with the pixel data
    public abstract byte[] GetPixels();

    // Should return the width of the video
    public abstract int GetWidth();

    // Should return the height of the video
    public abstract int GetHeight();

    // Should return true when the videosource is ready
    public abstract bool Ready();
}
