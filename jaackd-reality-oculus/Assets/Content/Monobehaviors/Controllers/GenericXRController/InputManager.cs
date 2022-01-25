using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System;

namespace jaackd {
  namespace Input {
    public class InputManager : Singleton<InputManager> {
      GenericXRController inputActions;

      [SerializeField]
      private float triggerThreshhold = .1f;

      public UnityEvent OnLeftGripPressed = new UnityEvent();
      public UnityEvent<float> OnLeftGripUpdated = new UnityEvent<float>();
      public UnityEvent OnLeftGripReleased = new UnityEvent();

      public UnityEvent OnLeftTriggerPressed = new UnityEvent();
      public UnityEvent<float> OnLeftTriggerUpdated = new UnityEvent<float>();
      public UnityEvent OnLeftTriggerReleased = new UnityEvent();

      public UnityEvent OnRightGripPressed = new UnityEvent();
      public UnityEvent<float> OnRightGripUpdated = new UnityEvent<float>();
      public UnityEvent OnRightGripReleased = new UnityEvent();

      public UnityEvent OnRightTriggerPressed = new UnityEvent();
      public UnityEvent<float> OnRightTriggerUpdated = new UnityEvent<float>();
      public UnityEvent OnRightTriggerReleased = new UnityEvent();

      float leftGripValue, rightGripValue, leftTriggerValue, rightTriggerValue;

      bool leftGripPressed, leftGripReleased, rightGripPressed, rightGripReleased, leftTriggerPressed, lefTriggerReleased, rightTriggerPressed, rightTriggerReleased;



      private void Awake() {
        inputActions = new GenericXRController();

        inputActions.RightController.Grip.performed += PressRightGrip;
        inputActions.RightController.Trigger.performed += PressRightTrigger;
        inputActions.LeftController.Grip.performed += PressLeftGrip;
        inputActions.LeftController.Trigger.performed += PressLeftTrigger;

        inputActions.Enable();
      }

      public void OnEnable() {
        inputActions.Enable();
      }

      public void OnDisable() {
        inputActions.Disable();
      }

      protected override void OnDestroy() {
        inputActions.Dispose();
      }


      private void PressRightGrip(InputAction.CallbackContext obj) {
        rightGripValue = obj.ReadValue<float>();

        if (rightGripValue > triggerThreshhold && rightGripValue < (1 - triggerThreshhold)) {
          OnRightGripUpdated.Invoke(rightGripValue);
          rightGripPressed = false;
          rightGripReleased = false;
          //Utilities.PrintIfEditor("rightGripValue = " + rightGripValue);
        }

        if (rightGripValue > (1 - triggerThreshhold)) {
          if (!rightGripPressed) {
            OnRightGripPressed.Invoke();
            rightGripPressed = true;
          }
          //Utilities.PrintIfEditor("Right Grip Pressed");
        }
        if (rightGripValue < triggerThreshhold) {
          if (!rightGripReleased) {
            OnRightGripReleased.Invoke();
            rightGripReleased = false;
          }
          //Utilities.PrintIfEditor("Right Grip Released");
        }
      }

      private void PressLeftGrip(InputAction.CallbackContext obj) {
        leftGripValue = obj.ReadValue<float>();


        if (leftGripValue > triggerThreshhold && leftGripValue < (1 - triggerThreshhold)) {
          OnLeftGripUpdated.Invoke(leftGripValue);
          leftGripPressed = false;
          leftGripReleased = false;
          //Utilities.PrintIfEditor("leftGripValue = " + leftGripValue);
        }

        if (leftGripValue > (1 - triggerThreshhold)) {
          if (!leftGripPressed) {
            OnLeftGripPressed.Invoke();
            leftGripPressed = true;
          }
          //Utilities.PrintIfEditor("Left Grip Pressed");
        }
        if (leftGripValue < triggerThreshhold) {
          if (!leftGripReleased) {
            OnLeftGripReleased.Invoke();
            leftGripReleased = true;
          }
          ///Utilities.PrintIfEditor("Left Grip Released");
        }
      }

      private void PressRightTrigger(InputAction.CallbackContext obj) {
        rightTriggerValue = obj.ReadValue<float>();

        if (rightTriggerValue > triggerThreshhold && rightTriggerValue < (1 - triggerThreshhold)) {
          OnRightTriggerUpdated.Invoke(rightTriggerValue);
          rightTriggerPressed = false;
          //Utilities.PrintIfEditor("rightTriggerValue = " + rightTriggerValue);
        }

        if (!rightTriggerPressed && rightTriggerValue > (1 - triggerThreshhold)) {
          OnRightTriggerPressed.Invoke();
          rightTriggerPressed = true;
          //Utilities.PrintIfEditor("Right Trigger Pressed");
        }
        if (rightTriggerPressed && rightTriggerValue < triggerThreshhold) {
          OnRightTriggerReleased.Invoke();
          rightTriggerPressed = false;
          //Utilities.PrintIfEditor("Right Trigger Released");
        }
      }

      private void PressLeftTrigger(InputAction.CallbackContext obj) {
        leftTriggerValue = obj.ReadValue<float>();

        if (leftTriggerValue > triggerThreshhold && leftTriggerValue < (1 - triggerThreshhold)) {
          OnLeftTriggerUpdated.Invoke(leftTriggerValue);
          leftTriggerPressed = false;
          //Utilities.PrintIfEditor("leftTriggerValue = " + leftTriggerValue);
        }

        if (!leftTriggerPressed && leftTriggerValue > (1 - triggerThreshhold)) {
          OnLeftTriggerPressed.Invoke();
          leftTriggerPressed = true;
          //Utilities.PrintIfEditor("Left Trigger Pressed");
        }
        if (leftTriggerPressed && leftTriggerValue < triggerThreshhold) {
          OnLeftTriggerReleased.Invoke();
          leftTriggerPressed = false;
          //Utilities.PrintIfEditor("Left Trigger Released");
        }
      }


    }


  }
}