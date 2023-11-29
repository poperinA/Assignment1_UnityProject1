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
        public LayerMask collisionLayer = 1 << 9;

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
            //initialize variables
            RaycastHit hit;
            Vector3 playerToCamera = mCameraTransform.position - (mPlayerTransform.position + new Vector3(0, CameraConstants.CameraPositionOffset.y, 0)); //vector from player to the camera

            //checks and finds for the collision point of the camera and the object that is on the "collide" layer
            if (Physics.Raycast(mPlayerTransform.position + new Vector3(0,CameraConstants.CameraPositionOffset.y,0), playerToCamera.normalized, out hit, playerToCamera.magnitude, collisionLayer))
            {
                //adjust the camera position to the hit point
                mCameraTransform.position = hit.point;
            }
        }

        public abstract void Update();
    }
}
