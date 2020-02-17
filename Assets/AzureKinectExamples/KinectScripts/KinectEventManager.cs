﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace com.rfilkov.kinect
{
    /// <summary>
    /// KinectEventManager provides sensor-frame events to the registered Unity event listeners.
    /// </summary>
    public class KinectEventManager : MonoBehaviour
    {
        [Tooltip("Depth sensor index - 0 is the 1st one, 1 - the 2nd one, etc.")]
        public int sensorIndex = 0;

        [Tooltip("Consider the sensor as disconnected, if no frames are received within this time period, in seconds.")]
        public float sensorTimeoutAfter = 2f;


        [System.Serializable]
        public class SensorStateEvent : UnityEvent<ulong> { }

        [System.Serializable]
        public class ImageTextureEvent : UnityEvent<Texture, ulong> { }

        [System.Serializable]
        public class UshortFrameEvent : UnityEvent<ushort[], ulong> { }

        [System.Serializable]
        public class ByteFrameEvent : UnityEvent<byte[], ulong> { }

        [System.Serializable]
        public class BodyFrameEvent : UnityEvent<KinectInterop.BodyData[], uint, ulong> { }


        [Header("Sensor Events")]

        /// <summary>
        /// Fired when the sensor is considered as disconnected.
        /// </summary>
        public SensorStateEvent OnSensorDisconnect;

        ///// <summary>
        ///// Fired when the sensor is considered as reconnected.
        ///// </summary>
        //public SensorStateEvent OnSensorReconnect;

        [Header("Sensor Data")]

        /// <summary>
        /// Fired when new color-camera frame is detected.
        /// </summary>
        public ImageTextureEvent OnNewColorImage;

        /// <summary>
        /// Fired when new depth frame is detected.
        /// </summary>
        public UshortFrameEvent OnNewDepthFrame;

        /// <summary>
        /// Fired when new infrared frame is detected.
        /// </summary>
        public UshortFrameEvent OnNewInfraredFrame;

        [Header("Body Tracking")]

        /// <summary>
        /// Fired when new body frame frame is detected.
        /// </summary>
        public BodyFrameEvent OnNewBodyFrame;

        /// <summary>
        /// Fired when new body-index frame is detected.
        /// </summary>
        public ByteFrameEvent OnNewBodyIndexFrame;

        [Header("Sensor Images")]

        /// <summary>
        /// Fired when new depth image is detected.
        /// </summary>
        public ImageTextureEvent OnNewDepthImage;

        /// <summary>
        /// Fired when new infrared image is detected.
        /// </summary>
        public ImageTextureEvent OnNewInfraredImage;

        /// <summary>
        /// Fired when new user body image is detected.
        /// </summary>
        public ImageTextureEvent OnNewBodyIndexImage;

        [Header("Transformed Frames")]

        /// <summary>
        /// Fired when new depth-camera transformed color image is detected.
        /// </summary>
        public ImageTextureEvent OnNewDepthCameraColorImage;

        /// <summary>
        /// Fired when new color-camera transformed depth frame is detected.
        /// </summary>
        public UshortFrameEvent OnNewColorCameraDepthFrame;

        /// <summary>
        /// Fired when new color-camera transformed body-index frame is detected.
        /// </summary>
        public ByteFrameEvent OnNewColorCameraBodyIndexFrame;


        // reference to KinectManager
        private KinectManager kinectManager = null;
        // reference to the respective sensor-data
        private KinectInterop.SensorData sensorData = null;

        // last frame times
        private ulong lastColorFrameTime = 0;
        private ulong lastDepthFrameTime = 0;
        private ulong lastInfraredFrameTime = 0;

        private ulong lastBodyFrameTime = 0;
        private ulong lastBodyIndexFrameTime = 0;

        private ulong lastDepthImageTime = 0;
        private ulong lastInfraredImageTime = 0;
        private ulong lastBodyIndexImageTime = 0;

        private ulong lastDepthCamColorFrameTime = 0;
        private ulong lastColorCamDepthFrameTime = 0;
        private ulong lastColorCamBodyIndexFrameTime = 0;

        // transformed frames enabled-flags
        private bool isDepthCamColorFramesEnabled = false;
        private bool isColorCamDepthFramesEnabled = false;
        private bool isColorCamBodyIndexFramesEnabled = false;

        // transformed depth cam color texture
        private Texture2D depthCamColorTex2D = null;

        // whether the sensor is currently disconnected
        private long lastFrameTime = 0;
        private bool isSensorDisconnected = false;


        void Start()
        {
            kinectManager = KinectManager.Instance;
            sensorData = kinectManager != null ? kinectManager.GetSensorData(sensorIndex) : null;
        }


        void OnDestroy()
        {
            if(isDepthCamColorFramesEnabled)
            {
                isDepthCamColorFramesEnabled = false;
                sensorData.sensorInterface.EnableDepthCameraColorFrame(sensorData, false);
                depthCamColorTex2D = null;
                Debug.Log("Disabled DepthCameraColorFrames.");
            }

            if (isColorCamDepthFramesEnabled)
            {
                isColorCamDepthFramesEnabled = false;
                sensorData.sensorInterface.EnableColorCameraDepthFrame(sensorData, false);
                Debug.Log("Disabled ColorCameraDepthFrames.");
            }

            if (isColorCamBodyIndexFramesEnabled)
            {
                isColorCamBodyIndexFramesEnabled = false;
                sensorData.sensorInterface.EnableColorCameraBodyIndexFrame(sensorData, false);
                Debug.Log("Disabled ColorCameraBodyIndexFrames.");
            }

        }


        void Update()
        {
            if (sensorData != null)
            {
                // color frame
                if(lastColorFrameTime != sensorData.lastColorFrameTime)
                {
                    lastFrameTime = System.DateTime.Now.Ticks;
                    lastColorFrameTime = sensorData.lastColorFrameTime;

                    OnNewColorImage?.Invoke(sensorData.colorImageTexture, sensorData.lastColorFrameTime);
                    //Debug.Log("OnNewColorFrame invoked. Time: " + lastColorFrameTime);
                }

                // depth frame
                if (lastDepthFrameTime != sensorData.lastDepthFrameTime)
                {
                    lastFrameTime = System.DateTime.Now.Ticks;
                    lastDepthFrameTime = sensorData.lastDepthFrameTime;

                    OnNewDepthFrame?.Invoke(sensorData.depthImage, sensorData.lastDepthFrameTime);
                    //Debug.Log("OnNewDepthFrame invoked. Time: " + lastDepthFrameTime);
                }

                // infrared frame
                if (lastInfraredFrameTime != sensorData.lastInfraredFrameTime)
                {
                    lastFrameTime = System.DateTime.Now.Ticks;
                    lastInfraredFrameTime = sensorData.lastInfraredFrameTime;

                    OnNewInfraredFrame?.Invoke(sensorData.infraredImage, sensorData.lastInfraredFrameTime);
                    //Debug.Log("OnNewInfraredFrame invoked. Time: " + lastInfraredFrameTime);
                }


                // body frame
                if (lastBodyFrameTime != sensorData.lastBodyFrameTime)
                {
                    lastFrameTime = System.DateTime.Now.Ticks;
                    lastBodyFrameTime = sensorData.lastBodyFrameTime;

                    OnNewBodyFrame?.Invoke(sensorData.alTrackedBodies, sensorData.trackedBodiesCount, sensorData.lastBodyFrameTime);
                    //Debug.Log("OnNewBodyFrame invoked. Time: " + lastBodyFrameTime);
                }

                // body-index frame
                if (lastBodyIndexFrameTime != sensorData.lastBodyIndexFrameTime)
                {
                    lastFrameTime = System.DateTime.Now.Ticks;
                    lastBodyIndexFrameTime = sensorData.lastBodyIndexFrameTime;

                    OnNewBodyIndexFrame?.Invoke(sensorData.bodyIndexImage, sensorData.lastBodyIndexFrameTime);
                    //Debug.Log("OnNewBodyIndexFrame invoked. Time: " + lastBodyIndexFrameTime);
                }


                // depth image
                if (OnNewDepthImage != null && OnNewDepthImage.GetPersistentEventCount() > 0 && lastDepthImageTime != sensorData.lastDepthImageTime)
                {
                    lastDepthImageTime = sensorData.lastDepthImageTime;
                    OnNewDepthImage?.Invoke(sensorData.depthImageTexture, sensorData.lastDepthImageTime);
                    //Debug.Log("OnNewDepthImage invoked. Time: " + lastDepthImageTime);
                }

                // infrared image
                if (OnNewInfraredImage != null && OnNewInfraredImage.GetPersistentEventCount() > 0 && lastInfraredImageTime != sensorData.lastInfraredImageTime)
                {
                    lastInfraredImageTime = sensorData.lastInfraredImageTime;
                    OnNewInfraredImage?.Invoke(sensorData.infraredImageTexture, sensorData.lastInfraredImageTime);
                    //Debug.Log("OnNewInfraredImage invoked. Time: " + lastInfraredImageTime);
                }

                // body-index image
                if (OnNewBodyIndexImage != null && OnNewBodyIndexImage.GetPersistentEventCount() > 0 && lastBodyIndexImageTime != sensorData.lastBodyImageTime)
                {
                    lastBodyIndexImageTime = sensorData.lastBodyImageTime;
                    OnNewBodyIndexImage?.Invoke(sensorData.bodyImageTexture, sensorData.lastBodyImageTime);
                    //Debug.Log("OnNewBodyIndexImage invoked. Time: " + lastBodyIndexImageTime);
                }

                // check for sensor events
                long currentTime = System.DateTime.Now.Ticks;
                long timeoutAfter = (long)(sensorTimeoutAfter * 10000000);

                if(!isSensorDisconnected && lastFrameTime != 0 && (currentTime - lastFrameTime) > timeoutAfter)
                {
                    isSensorDisconnected = true;
                    OnSensorDisconnect?.Invoke(GetLastFrameTime());
                    Debug.Log("Sensor disconnected. Time: " + GetLastFrameTime());
                }
                //else if(isSensorDisconnected && (currentTime - lastFrameTime) <= timeoutAfter)
                //{
                //    isSensorDisconnected = false;
                //    OnSensorReconnect?.Invoke(GetLastFrameTime());
                //    Debug.Log("Sensor reconnected. Time: " + GetLastFrameTime());
                //}

                // depth-cam color frame
                if (OnNewDepthCameraColorImage != null && OnNewDepthCameraColorImage.GetPersistentEventCount() > 0 && !isDepthCamColorFramesEnabled)
                {
                    isDepthCamColorFramesEnabled = true;
                    sensorData.sensorInterface.EnableDepthCameraColorFrame(sensorData, true);
                    depthCamColorTex2D = new Texture2D(sensorData.depthImageWidth, sensorData.depthImageHeight, TextureFormat.ARGB32, false);
                    Debug.Log("Enabled DepthCameraColorFrames.");
                }
                else if ((OnNewDepthCameraColorImage == null || OnNewDepthCameraColorImage.GetPersistentEventCount() == 0) && isDepthCamColorFramesEnabled)
                {
                    isDepthCamColorFramesEnabled = false;
                    sensorData.sensorInterface.EnableDepthCameraColorFrame(sensorData, false);
                    depthCamColorTex2D = null;
                    Debug.Log("Disabled DepthCameraColorFrames.");
                }

                if (isDepthCamColorFramesEnabled)
                {
                    ulong oldFrameTime = lastDepthCamColorFrameTime;
                    sensorData.sensorInterface.GetDepthCameraColorFrameTexture(ref depthCamColorTex2D, ref lastDepthCamColorFrameTime);

                    if(oldFrameTime != lastDepthCamColorFrameTime)
                    {
                        OnNewDepthCameraColorImage?.Invoke(depthCamColorTex2D, lastDepthCamColorFrameTime);
                        //Debug.Log("OnNewDepthCameraColorImage invoked. Time: " + lastDepthCamColorFrameTime);
                    }
                }

                // color-cam depth frame
                if (OnNewColorCameraDepthFrame != null && OnNewColorCameraDepthFrame.GetPersistentEventCount() > 0 && !isColorCamDepthFramesEnabled)
                {
                    isColorCamDepthFramesEnabled = true;
                    sensorData.sensorInterface.EnableColorCameraDepthFrame(sensorData, true);
                    Debug.Log("Enabled ColorCameraDepthFrames.");
                }
                else if ((OnNewColorCameraDepthFrame == null || OnNewColorCameraDepthFrame.GetPersistentEventCount() == 0) && isColorCamDepthFramesEnabled)
                {
                    isColorCamDepthFramesEnabled = false;
                    sensorData.sensorInterface.EnableColorCameraDepthFrame(sensorData, false);
                    Debug.Log("Disabled ColorCameraDepthFrames.");
                }

                if (isColorCamDepthFramesEnabled)
                {
                    ulong oldFrameTime = lastColorCamDepthFrameTime;
                    ushort[] depthFrame = sensorData.sensorInterface.GetColorCameraDepthFrame(ref lastColorCamDepthFrameTime);

                    if (depthFrame != null && oldFrameTime != lastColorCamDepthFrameTime)
                    {
                        OnNewColorCameraDepthFrame?.Invoke(depthFrame, lastColorCamDepthFrameTime);
                        //Debug.Log("OnNewColorCameraDepthFrame invoked. Time: " + lastColorCamDepthFrameTime);
                    }
                }

                // color-cam body-index frame
                if (OnNewColorCameraBodyIndexFrame != null && OnNewColorCameraBodyIndexFrame.GetPersistentEventCount() > 0 && !isColorCamBodyIndexFramesEnabled)
                {
                    isColorCamBodyIndexFramesEnabled = true;
                    sensorData.sensorInterface.EnableColorCameraBodyIndexFrame(sensorData, true);
                    Debug.Log("Enabled ColorCameraBodyIndexFrames.");
                }
                else if ((OnNewColorCameraBodyIndexFrame == null || OnNewColorCameraBodyIndexFrame.GetPersistentEventCount() == 0) && isColorCamBodyIndexFramesEnabled)
                {
                    isColorCamBodyIndexFramesEnabled = false;
                    sensorData.sensorInterface.EnableColorCameraBodyIndexFrame(sensorData, false);
                    Debug.Log("Disabled ColorCameraBodyIndexFrames.");
                }

                if (isColorCamBodyIndexFramesEnabled)
                {
                    ulong oldFrameTime = lastColorCamBodyIndexFrameTime;
                    byte[] bodyIndexFrame = sensorData.sensorInterface.GetColorCameraBodyIndexFrame(ref lastColorCamBodyIndexFrameTime);

                    if (bodyIndexFrame != null && oldFrameTime != lastColorCamBodyIndexFrameTime)
                    {
                        OnNewColorCameraBodyIndexFrame?.Invoke(bodyIndexFrame, lastColorCamBodyIndexFrameTime);
                        //Debug.Log("OnNewColorCameraBodyIndexFrame invoked. Time: " + lastColorCamBodyIndexFrameTime);
                    }
                }

            }
        }


        // returns the last frame time
        private ulong GetLastFrameTime()
        {
            ulong maxFrameTime = 0;

            if (lastColorFrameTime != 0 && maxFrameTime < lastColorFrameTime)
                maxFrameTime = lastColorFrameTime;
            if (lastDepthFrameTime != 0 && maxFrameTime < lastDepthFrameTime)
                maxFrameTime = lastDepthFrameTime;
            if (lastInfraredFrameTime != 0 && maxFrameTime < lastInfraredFrameTime)
                maxFrameTime = lastInfraredFrameTime;
            if (lastBodyFrameTime != 0 && maxFrameTime < lastBodyFrameTime)
                maxFrameTime = lastBodyFrameTime;
            if (lastBodyIndexFrameTime != 0 && maxFrameTime < lastBodyIndexFrameTime)
                maxFrameTime = lastBodyIndexFrameTime;

            return maxFrameTime;
        }

    }
}

