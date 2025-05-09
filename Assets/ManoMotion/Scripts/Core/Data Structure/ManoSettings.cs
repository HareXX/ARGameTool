﻿using System;

namespace ManoMotion
{
    /// <summary>
    /// Information sent to the SDK on Init() from ManomotionManager.
    /// </summary>
    [Serializable]
    public struct ManoSettings
    {
        public Platform platform;
        public ImageFormat imageFormat;
        public string serialKey;
        public int segmentationMode;
    };

    /// <summary>
    /// Provides information regarding the platform that the SDK is currently being deployed to.
    /// </summary>
    public enum Platform
    {
        UNITY_ANDROID,
        UNITY_IOS
    };

    /// <summary>
    /// Provides information regarding the format of the image ManoMotion tech is going to process. By default the WebcamTexture feed will provide a BGRA format so use that for initializing.
    /// </summary>
    public enum ImageFormat
    {
        GRAYSCALE_FORMAT = 5,
        BGRA_FORMAT = 4,
        RGBA_FORMAT = 3,
        RGB_FORMAT = 2,
        BGR_FORMAT = 1
    };
}