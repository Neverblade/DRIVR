using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Vehicles.Car
{
    [RequireComponent(typeof (CarController))]
    public class CarUserControl : MonoBehaviour
    {
        public SteeringWheelControl steeringWheelControl;

        public OVRInput.Controller leftController = OVRInput.Controller.LTouch;
        public OVRInput.Controller rightController = OVRInput.Controller.RTouch;

        private CarController m_Car; // the car controller we want to use

        private void Awake()
        {
            // get the car controller
            m_Car = GetComponent<CarController>();
        }


        private void FixedUpdate()
        {
            // pass the input to the car!
            //float h = CrossPlatformInputManager.GetAxis("Horizontal");
            float h = steeringWheelControl.clampAngle();
            //float v = CrossPlatformInputManager.GetAxis("Vertical");
            float v = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, rightController);
            float handbrake = CrossPlatformInputManager.GetAxis("Jump");
            float b = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, leftController);
            m_Car.Move(h, v, -b, handbrake);
        }
    }
}
