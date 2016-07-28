using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Characters.FirstPerson
{
    [Serializable]
    public class MouseLook
    {
        public float XSensitivity = 2f;
        public float YSensitivity = 2f;
        public bool clampVerticalRotation = true;
        public float MinimumX = -90F;
        public float MaximumX = 90F;
        public bool smooth;
        public float smoothTime = 5f;


        private Quaternion m_CharacterTargetRot;
        private Quaternion m_CameraTargetRot;


        //private bool wave = false;
        private float decayTime = 0.85f;
        private float processDecayTime = 0.0f;

        public void Init(Transform character, Transform camera)
        {
            m_CharacterTargetRot = character.localRotation;
            m_CameraTargetRot = camera.localRotation;

            processDecayTime = decayTime;
        }

        public void InitProcessDecayTime()
        {
            processDecayTime = 0.0f;
        }

        public void LookRotation(Transform character, Transform camera)
        {
            float yRot = CrossPlatformInputManager.GetAxis("Mouse X") * XSensitivity;
            float xRot = CrossPlatformInputManager.GetAxis("Mouse Y") * YSensitivity;


            //if (Input.GetKeyDown(KeyCode.H))
            //{
            //    wave = true;
            //    processDecayTime = 0.0f;
            //}
            //if (wave)
            //{
            //    processDecayTime += Time.deltaTime;
            float y = 0.0f;
            if (processDecayTime < decayTime)
            {
                processDecayTime += Time.deltaTime;
                y = (Mathf.Sin(2.0f * 3.14159f * processDecayTime * 3) * 6.0f +
                     Mathf.Sin(2.0f * 3.14159f * processDecayTime * 6 + 0.2f) * 3.1f +
                     Mathf.Sin(2.0f * 3.14159f * processDecayTime * 10 + 0.5f) * 1.1f) * (decayTime - processDecayTime) / decayTime;
            }
            xRot += y;
            //}


            m_CharacterTargetRot *= Quaternion.Euler (0f, yRot, 0f);
            m_CameraTargetRot *= Quaternion.Euler (-xRot, 0f, 0f);

            if(clampVerticalRotation)
                m_CameraTargetRot = ClampRotationAroundXAxis (m_CameraTargetRot);

            if(smooth)
            {
                character.localRotation = Quaternion.Slerp (character.localRotation, m_CharacterTargetRot,
                    smoothTime * Time.deltaTime);
                camera.localRotation = Quaternion.Slerp (camera.localRotation, m_CameraTargetRot,
                    smoothTime * Time.deltaTime);
            }
            else
            {
                character.localRotation = m_CharacterTargetRot;
                camera.localRotation = m_CameraTargetRot;
            }
        }


        Quaternion ClampRotationAroundXAxis(Quaternion q)
        {
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1.0f;

            float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan (q.x);

            angleX = Mathf.Clamp (angleX, MinimumX, MaximumX);

            q.x = Mathf.Tan (0.5f * Mathf.Deg2Rad * angleX);

            return q;
        }

    }
}
