using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PGGE
{
    // The base class for all third-person camera controllers
    public abstract class TPCBase
    {
        protected Transform mCameraTransform;
        protected Transform mPlayerTransform;
        private float yPos;
        public LayerMask collisionLayer = 1 << 9;
        Animator m_Animator;
        float m_CurrentClip;
        float m_ClipName;

        public Transform CameraTransform
        {
            get
            {
                return mCameraTransform;
            }
        }
        public Transform PlayerTransform
        {
            get
            {
                return mPlayerTransform;
            }
        }

        public TPCBase(Transform cameraTransform, Transform playerTransform)
        {
            mCameraTransform = cameraTransform;
            mPlayerTransform = playerTransform;
        }

        public void RepositionCamera()
        {
            RaycastHit hit;
            Vector3 playerToCamera = mCameraTransform.position - (mPlayerTransform.position + new Vector3(0, CameraConstants.CameraPositionOffset.y, 0));

            if (Physics.Raycast(mPlayerTransform.position + new Vector3(0,CameraConstants.CameraPositionOffset.y,0), playerToCamera.normalized, out hit, playerToCamera.magnitude, collisionLayer))
            {
                // Adjust the camera position to the hit point, using the original camera height
                mCameraTransform.position = hit.point;
            }
        }

        public abstract void Update();
    }
}
