using UnityEngine;
using System.Collections;
using System.IO;
using System;

namespace vertigoGames.hexColorGame
{
    public class CameraManager : MonoBehaviour
    {
        public float minWidth, minHeight, maxWidth, maxHeight;

        [HideInInspector]
        public float virtualWidth;
        [HideInInspector]
        public float virtualHeight;

        private int screenWidth, screenHeight;
        private float aspect;
        private Camera cam;
        private bool matchWidth;
        private float width;
        private float height;

        private float minAspect, maxAspect;

        public Action onScreenSizeChanged;

        protected virtual void Awake()
        {
            cam = GetComponent<Camera>();
            matchWidth = minWidth == maxWidth;

            minAspect = minHeight / Mathf.Max(minWidth, maxWidth);
            maxAspect = maxHeight / Mathf.Min(minWidth, maxWidth);
            Debug.Log(minAspect);
            Debug.Log(maxAspect);

            UpdateCamera();
        }

        private void Update()
        {
            if (screenWidth != Screen.width || screenHeight != Screen.height)
            {
                UpdateCamera();
            }
        }

        private void UpdateCamera()
        {
            screenWidth = Screen.width;
            screenHeight = Screen.height;

            aspect = Screen.height / (float)Screen.width;
            float targetAspect = Mathf.Clamp(aspect, minAspect, maxAspect);
            if (matchWidth)
            {
                width = minWidth / 17f;
                height = width * targetAspect;
            }
            else
            {
                height = minHeight / 17f;
                width = height / targetAspect;
            }
            cam.orthographicSize = aspect < maxAspect ? height : width * aspect;

            virtualHeight = cam.orthographicSize * 200;
            virtualWidth = virtualHeight / aspect;

            if (onScreenSizeChanged != null) onScreenSizeChanged();
        }

        public float GetHeight()
        {
            return height;
        }

        public float GetWidth()
        {
            return width;
        }
    }

}