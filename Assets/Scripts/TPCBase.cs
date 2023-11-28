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
        Player player;

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
            Vector3 playerToCamera = mCameraTransform.position - mPlayerTransform.position;

            if (Physics.Raycast(mPlayerTransform.position, playerToCamera.normalized, out hit, playerToCamera.magnitude, collisionLayer))
            {
                // Stores the camera height right when it collides with the wall and uses it throughout collision
                if (player != null)
                {
                    yPos = CameraConstants.CameraPositionOffset.y;
                }

                // Adjust the camera position to the hit point, using the original camera height
                mCameraTransform.position = new Vector3(hit.point.x, yPos, hit.point.z);
            }
        }

        public abstract void Update();
    }//helofldlasolso
}
