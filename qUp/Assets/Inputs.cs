// GENERATED AUTOMATICALLY FROM 'Assets/Inputs.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace InputControlls
{
    public class @Inputs : IInputActionCollection, IDisposable
    {
        public InputActionAsset asset { get; }
        public @Inputs()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""Inputs"",
    ""maps"": [
        {
            ""name"": ""NoUnitSelected"",
            ""id"": ""ba4eb942-9cf5-45f2-9cc8-3ee68d593025"",
            ""actions"": [
                {
                    ""name"": ""SelectUnit"",
                    ""type"": ""Button"",
                    ""id"": ""9473a6b6-c5b7-4c71-b469-dfd5edcba00e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""CameraRotation"",
                    ""type"": ""Button"",
                    ""id"": ""d6037670-8c32-46af-959c-9f3fe1f7248b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Hold""
                },
                {
                    ""name"": ""CameraZoom"",
                    ""type"": ""Value"",
                    ""id"": ""f3fecfb8-e708-48be-823d-4bc17f86bc9b"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""CameraPan"",
                    ""type"": ""Value"",
                    ""id"": ""e43c5166-5909-4152-876d-0ca8a29b6441"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""13d7d1ed-310f-458b-9a00-e8068c769610"",
                    ""path"": ""<Mouse>/press"",
                    ""interactions"": ""Press"",
                    ""processors"": ""Normalize(max=1)"",
                    ""groups"": """",
                    ""action"": ""SelectUnit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b54dabff-4d3e-4e28-ab40-5f3a8fda6ebd"",
                    ""path"": ""<Mouse>/scroll/y"",
                    ""interactions"": """",
                    ""processors"": ""Normalize(min=-1,max=1)"",
                    ""groups"": """",
                    ""action"": ""CameraZoom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""453d2945-cfbc-4bc7-8c62-555332070b90"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraPan"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""066caa6c-d949-43bc-9981-df2ff7f60382"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": ""Hold"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraRotation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // NoUnitSelected
            m_NoUnitSelected = asset.FindActionMap("NoUnitSelected", throwIfNotFound: true);
            m_NoUnitSelected_SelectUnit = m_NoUnitSelected.FindAction("SelectUnit", throwIfNotFound: true);
            m_NoUnitSelected_CameraRotation = m_NoUnitSelected.FindAction("CameraRotation", throwIfNotFound: true);
            m_NoUnitSelected_CameraZoom = m_NoUnitSelected.FindAction("CameraZoom", throwIfNotFound: true);
            m_NoUnitSelected_CameraPan = m_NoUnitSelected.FindAction("CameraPan", throwIfNotFound: true);
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(asset);
        }

        public InputBinding? bindingMask
        {
            get => asset.bindingMask;
            set => asset.bindingMask = value;
        }

        public ReadOnlyArray<InputDevice>? devices
        {
            get => asset.devices;
            set => asset.devices = value;
        }

        public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

        public bool Contains(InputAction action)
        {
            return asset.Contains(action);
        }

        public IEnumerator<InputAction> GetEnumerator()
        {
            return asset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Enable()
        {
            asset.Enable();
        }

        public void Disable()
        {
            asset.Disable();
        }

        // NoUnitSelected
        private readonly InputActionMap m_NoUnitSelected;
        private INoUnitSelectedActions m_NoUnitSelectedActionsCallbackInterface;
        private readonly InputAction m_NoUnitSelected_SelectUnit;
        private readonly InputAction m_NoUnitSelected_CameraRotation;
        private readonly InputAction m_NoUnitSelected_CameraZoom;
        private readonly InputAction m_NoUnitSelected_CameraPan;
        public struct NoUnitSelectedActions
        {
            private @Inputs m_Wrapper;
            public NoUnitSelectedActions(@Inputs wrapper) { m_Wrapper = wrapper; }
            public InputAction @SelectUnit => m_Wrapper.m_NoUnitSelected_SelectUnit;
            public InputAction @CameraRotation => m_Wrapper.m_NoUnitSelected_CameraRotation;
            public InputAction @CameraZoom => m_Wrapper.m_NoUnitSelected_CameraZoom;
            public InputAction @CameraPan => m_Wrapper.m_NoUnitSelected_CameraPan;
            public InputActionMap Get() { return m_Wrapper.m_NoUnitSelected; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(NoUnitSelectedActions set) { return set.Get(); }
            public void SetCallbacks(INoUnitSelectedActions instance)
            {
                if (m_Wrapper.m_NoUnitSelectedActionsCallbackInterface != null)
                {
                    @SelectUnit.started -= m_Wrapper.m_NoUnitSelectedActionsCallbackInterface.OnSelectUnit;
                    @SelectUnit.performed -= m_Wrapper.m_NoUnitSelectedActionsCallbackInterface.OnSelectUnit;
                    @SelectUnit.canceled -= m_Wrapper.m_NoUnitSelectedActionsCallbackInterface.OnSelectUnit;
                    @CameraRotation.started -= m_Wrapper.m_NoUnitSelectedActionsCallbackInterface.OnCameraRotation;
                    @CameraRotation.performed -= m_Wrapper.m_NoUnitSelectedActionsCallbackInterface.OnCameraRotation;
                    @CameraRotation.canceled -= m_Wrapper.m_NoUnitSelectedActionsCallbackInterface.OnCameraRotation;
                    @CameraZoom.started -= m_Wrapper.m_NoUnitSelectedActionsCallbackInterface.OnCameraZoom;
                    @CameraZoom.performed -= m_Wrapper.m_NoUnitSelectedActionsCallbackInterface.OnCameraZoom;
                    @CameraZoom.canceled -= m_Wrapper.m_NoUnitSelectedActionsCallbackInterface.OnCameraZoom;
                    @CameraPan.started -= m_Wrapper.m_NoUnitSelectedActionsCallbackInterface.OnCameraPan;
                    @CameraPan.performed -= m_Wrapper.m_NoUnitSelectedActionsCallbackInterface.OnCameraPan;
                    @CameraPan.canceled -= m_Wrapper.m_NoUnitSelectedActionsCallbackInterface.OnCameraPan;
                }
                m_Wrapper.m_NoUnitSelectedActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @SelectUnit.started += instance.OnSelectUnit;
                    @SelectUnit.performed += instance.OnSelectUnit;
                    @SelectUnit.canceled += instance.OnSelectUnit;
                    @CameraRotation.started += instance.OnCameraRotation;
                    @CameraRotation.performed += instance.OnCameraRotation;
                    @CameraRotation.canceled += instance.OnCameraRotation;
                    @CameraZoom.started += instance.OnCameraZoom;
                    @CameraZoom.performed += instance.OnCameraZoom;
                    @CameraZoom.canceled += instance.OnCameraZoom;
                    @CameraPan.started += instance.OnCameraPan;
                    @CameraPan.performed += instance.OnCameraPan;
                    @CameraPan.canceled += instance.OnCameraPan;
                }
            }
        }
        public NoUnitSelectedActions @NoUnitSelected => new NoUnitSelectedActions(this);
        public interface INoUnitSelectedActions
        {
            void OnSelectUnit(InputAction.CallbackContext context);
            void OnCameraRotation(InputAction.CallbackContext context);
            void OnCameraZoom(InputAction.CallbackContext context);
            void OnCameraPan(InputAction.CallbackContext context);
        }
    }
}
