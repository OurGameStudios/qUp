// GENERATED AUTOMATICALLY FROM 'Assets/Inputs.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Inputs : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Inputs()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Inputs"",
    ""maps"": [
        {
            ""name"": ""CameraControls"",
            ""id"": ""25de74eb-ac79-46cf-8f9b-4df6ddad1055"",
            ""actions"": [
                {
                    ""name"": ""MouseDelta"",
                    ""type"": ""Value"",
                    ""id"": ""3e5f3abc-583a-46b3-b995-28a881a13eb7"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PointerPosition"",
                    ""type"": ""Value"",
                    ""id"": ""8f19de8b-cbe9-44b3-9bf7-4ea8684f79a8"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""CameraPan"",
                    ""type"": ""Value"",
                    ""id"": ""5f1fb2c6-e21b-43b1-9215-75d99eb0312a"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""CameraZoom"",
                    ""type"": ""Value"",
                    ""id"": ""1e39c189-4668-430c-aa72-80e0bf455e0c"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""CameraRotation"",
                    ""type"": ""Button"",
                    ""id"": ""947a07ad-1d5c-47ed-8f8b-99afb45b73da"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Hold""
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""01f4f44f-4776-45c4-a250-8ff491724afa"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": ""Hold"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraRotation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bcf4e0a2-974a-49d7-adc8-27a2394ee164"",
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
                    ""id"": ""4ae95260-84d7-4a82-bd80-0d313a70b771"",
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
                    ""id"": ""3293ed52-f64b-48e9-a549-72fafb9fb9a5"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseDelta"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ba738b56-3f8c-4adf-a968-25b5f4c49fc3"",
                    ""path"": ""<Pointer>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PointerPosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""NextPlayer"",
            ""id"": ""2496e54f-d236-4ceb-9051-e309b756f5cc"",
            ""actions"": [
                {
                    ""name"": ""Next"",
                    ""type"": ""Button"",
                    ""id"": ""c2b0e941-c260-4ca9-af28-8e8f5d808441"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""5adfb2b6-179f-443d-8684-6418f10425dd"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Next"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""PlanningPhase"",
            ""id"": ""42180ffc-520d-40af-a726-5263ab5bb153"",
            ""actions"": [
                {
                    ""name"": ""PointerPosition"",
                    ""type"": ""Value"",
                    ""id"": ""1b0fb7df-0bdb-4eb2-9275-0fdaae481aff"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SelectUnit"",
                    ""type"": ""Button"",
                    ""id"": ""6e686330-a4eb-4b64-8bde-bb77907a59a8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""d35d3069-70bd-4e7d-92e8-c55ca3ec9bc9"",
                    ""path"": ""<Pointer>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PointerPosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""43c4fbd5-a779-4c0a-9d7a-273cbcdb44fb"",
                    ""path"": ""<Mouse>/press"",
                    ""interactions"": ""Press"",
                    ""processors"": ""Normalize(max=1)"",
                    ""groups"": """",
                    ""action"": ""SelectUnit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""NoUnitSelected"",
            ""id"": ""ba4eb942-9cf5-45f2-9cc8-3ee68d593025"",
            ""actions"": [],
            ""bindings"": []
        },
        {
            ""name"": ""UnitSelected"",
            ""id"": ""cd337ba0-edc3-45a3-942e-ec872c3e87bd"",
            ""actions"": [
                {
                    ""name"": ""SelectPath"",
                    ""type"": ""Button"",
                    ""id"": ""4e94604a-15e6-4fca-9828-2c64fc908059"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""c0cb70a5-81dd-4019-b423-48dff7f143f8"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SelectPath"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""ExecutionInteractions"",
            ""id"": ""2c2d5aed-f471-4b8a-9709-5f64562fd8f1"",
            ""actions"": [],
            ""bindings"": []
        }
    ],
    ""controlSchemes"": []
}");
        // CameraControls
        m_CameraControls = asset.FindActionMap("CameraControls", throwIfNotFound: true);
        m_CameraControls_MouseDelta = m_CameraControls.FindAction("MouseDelta", throwIfNotFound: true);
        m_CameraControls_PointerPosition = m_CameraControls.FindAction("PointerPosition", throwIfNotFound: true);
        m_CameraControls_CameraPan = m_CameraControls.FindAction("CameraPan", throwIfNotFound: true);
        m_CameraControls_CameraZoom = m_CameraControls.FindAction("CameraZoom", throwIfNotFound: true);
        m_CameraControls_CameraRotation = m_CameraControls.FindAction("CameraRotation", throwIfNotFound: true);
        // NextPlayer
        m_NextPlayer = asset.FindActionMap("NextPlayer", throwIfNotFound: true);
        m_NextPlayer_Next = m_NextPlayer.FindAction("Next", throwIfNotFound: true);
        // PlanningPhase
        m_PlanningPhase = asset.FindActionMap("PlanningPhase", throwIfNotFound: true);
        m_PlanningPhase_PointerPosition = m_PlanningPhase.FindAction("PointerPosition", throwIfNotFound: true);
        m_PlanningPhase_SelectUnit = m_PlanningPhase.FindAction("SelectUnit", throwIfNotFound: true);
        // NoUnitSelected
        m_NoUnitSelected = asset.FindActionMap("NoUnitSelected", throwIfNotFound: true);
        // UnitSelected
        m_UnitSelected = asset.FindActionMap("UnitSelected", throwIfNotFound: true);
        m_UnitSelected_SelectPath = m_UnitSelected.FindAction("SelectPath", throwIfNotFound: true);
        // ExecutionInteractions
        m_ExecutionInteractions = asset.FindActionMap("ExecutionInteractions", throwIfNotFound: true);
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

    // CameraControls
    private readonly InputActionMap m_CameraControls;
    private ICameraControlsActions m_CameraControlsActionsCallbackInterface;
    private readonly InputAction m_CameraControls_MouseDelta;
    private readonly InputAction m_CameraControls_PointerPosition;
    private readonly InputAction m_CameraControls_CameraPan;
    private readonly InputAction m_CameraControls_CameraZoom;
    private readonly InputAction m_CameraControls_CameraRotation;
    public struct CameraControlsActions
    {
        private @Inputs m_Wrapper;
        public CameraControlsActions(@Inputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @MouseDelta => m_Wrapper.m_CameraControls_MouseDelta;
        public InputAction @PointerPosition => m_Wrapper.m_CameraControls_PointerPosition;
        public InputAction @CameraPan => m_Wrapper.m_CameraControls_CameraPan;
        public InputAction @CameraZoom => m_Wrapper.m_CameraControls_CameraZoom;
        public InputAction @CameraRotation => m_Wrapper.m_CameraControls_CameraRotation;
        public InputActionMap Get() { return m_Wrapper.m_CameraControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CameraControlsActions set) { return set.Get(); }
        public void SetCallbacks(ICameraControlsActions instance)
        {
            if (m_Wrapper.m_CameraControlsActionsCallbackInterface != null)
            {
                @MouseDelta.started -= m_Wrapper.m_CameraControlsActionsCallbackInterface.OnMouseDelta;
                @MouseDelta.performed -= m_Wrapper.m_CameraControlsActionsCallbackInterface.OnMouseDelta;
                @MouseDelta.canceled -= m_Wrapper.m_CameraControlsActionsCallbackInterface.OnMouseDelta;
                @PointerPosition.started -= m_Wrapper.m_CameraControlsActionsCallbackInterface.OnPointerPosition;
                @PointerPosition.performed -= m_Wrapper.m_CameraControlsActionsCallbackInterface.OnPointerPosition;
                @PointerPosition.canceled -= m_Wrapper.m_CameraControlsActionsCallbackInterface.OnPointerPosition;
                @CameraPan.started -= m_Wrapper.m_CameraControlsActionsCallbackInterface.OnCameraPan;
                @CameraPan.performed -= m_Wrapper.m_CameraControlsActionsCallbackInterface.OnCameraPan;
                @CameraPan.canceled -= m_Wrapper.m_CameraControlsActionsCallbackInterface.OnCameraPan;
                @CameraZoom.started -= m_Wrapper.m_CameraControlsActionsCallbackInterface.OnCameraZoom;
                @CameraZoom.performed -= m_Wrapper.m_CameraControlsActionsCallbackInterface.OnCameraZoom;
                @CameraZoom.canceled -= m_Wrapper.m_CameraControlsActionsCallbackInterface.OnCameraZoom;
                @CameraRotation.started -= m_Wrapper.m_CameraControlsActionsCallbackInterface.OnCameraRotation;
                @CameraRotation.performed -= m_Wrapper.m_CameraControlsActionsCallbackInterface.OnCameraRotation;
                @CameraRotation.canceled -= m_Wrapper.m_CameraControlsActionsCallbackInterface.OnCameraRotation;
            }
            m_Wrapper.m_CameraControlsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MouseDelta.started += instance.OnMouseDelta;
                @MouseDelta.performed += instance.OnMouseDelta;
                @MouseDelta.canceled += instance.OnMouseDelta;
                @PointerPosition.started += instance.OnPointerPosition;
                @PointerPosition.performed += instance.OnPointerPosition;
                @PointerPosition.canceled += instance.OnPointerPosition;
                @CameraPan.started += instance.OnCameraPan;
                @CameraPan.performed += instance.OnCameraPan;
                @CameraPan.canceled += instance.OnCameraPan;
                @CameraZoom.started += instance.OnCameraZoom;
                @CameraZoom.performed += instance.OnCameraZoom;
                @CameraZoom.canceled += instance.OnCameraZoom;
                @CameraRotation.started += instance.OnCameraRotation;
                @CameraRotation.performed += instance.OnCameraRotation;
                @CameraRotation.canceled += instance.OnCameraRotation;
            }
        }
    }
    public CameraControlsActions @CameraControls => new CameraControlsActions(this);

    // NextPlayer
    private readonly InputActionMap m_NextPlayer;
    private INextPlayerActions m_NextPlayerActionsCallbackInterface;
    private readonly InputAction m_NextPlayer_Next;
    public struct NextPlayerActions
    {
        private @Inputs m_Wrapper;
        public NextPlayerActions(@Inputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @Next => m_Wrapper.m_NextPlayer_Next;
        public InputActionMap Get() { return m_Wrapper.m_NextPlayer; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(NextPlayerActions set) { return set.Get(); }
        public void SetCallbacks(INextPlayerActions instance)
        {
            if (m_Wrapper.m_NextPlayerActionsCallbackInterface != null)
            {
                @Next.started -= m_Wrapper.m_NextPlayerActionsCallbackInterface.OnNext;
                @Next.performed -= m_Wrapper.m_NextPlayerActionsCallbackInterface.OnNext;
                @Next.canceled -= m_Wrapper.m_NextPlayerActionsCallbackInterface.OnNext;
            }
            m_Wrapper.m_NextPlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Next.started += instance.OnNext;
                @Next.performed += instance.OnNext;
                @Next.canceled += instance.OnNext;
            }
        }
    }
    public NextPlayerActions @NextPlayer => new NextPlayerActions(this);

    // PlanningPhase
    private readonly InputActionMap m_PlanningPhase;
    private IPlanningPhaseActions m_PlanningPhaseActionsCallbackInterface;
    private readonly InputAction m_PlanningPhase_PointerPosition;
    private readonly InputAction m_PlanningPhase_SelectUnit;
    public struct PlanningPhaseActions
    {
        private @Inputs m_Wrapper;
        public PlanningPhaseActions(@Inputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @PointerPosition => m_Wrapper.m_PlanningPhase_PointerPosition;
        public InputAction @SelectUnit => m_Wrapper.m_PlanningPhase_SelectUnit;
        public InputActionMap Get() { return m_Wrapper.m_PlanningPhase; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlanningPhaseActions set) { return set.Get(); }
        public void SetCallbacks(IPlanningPhaseActions instance)
        {
            if (m_Wrapper.m_PlanningPhaseActionsCallbackInterface != null)
            {
                @PointerPosition.started -= m_Wrapper.m_PlanningPhaseActionsCallbackInterface.OnPointerPosition;
                @PointerPosition.performed -= m_Wrapper.m_PlanningPhaseActionsCallbackInterface.OnPointerPosition;
                @PointerPosition.canceled -= m_Wrapper.m_PlanningPhaseActionsCallbackInterface.OnPointerPosition;
                @SelectUnit.started -= m_Wrapper.m_PlanningPhaseActionsCallbackInterface.OnSelectUnit;
                @SelectUnit.performed -= m_Wrapper.m_PlanningPhaseActionsCallbackInterface.OnSelectUnit;
                @SelectUnit.canceled -= m_Wrapper.m_PlanningPhaseActionsCallbackInterface.OnSelectUnit;
            }
            m_Wrapper.m_PlanningPhaseActionsCallbackInterface = instance;
            if (instance != null)
            {
                @PointerPosition.started += instance.OnPointerPosition;
                @PointerPosition.performed += instance.OnPointerPosition;
                @PointerPosition.canceled += instance.OnPointerPosition;
                @SelectUnit.started += instance.OnSelectUnit;
                @SelectUnit.performed += instance.OnSelectUnit;
                @SelectUnit.canceled += instance.OnSelectUnit;
            }
        }
    }
    public PlanningPhaseActions @PlanningPhase => new PlanningPhaseActions(this);

    // NoUnitSelected
    private readonly InputActionMap m_NoUnitSelected;
    private INoUnitSelectedActions m_NoUnitSelectedActionsCallbackInterface;
    public struct NoUnitSelectedActions
    {
        private @Inputs m_Wrapper;
        public NoUnitSelectedActions(@Inputs wrapper) { m_Wrapper = wrapper; }
        public InputActionMap Get() { return m_Wrapper.m_NoUnitSelected; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(NoUnitSelectedActions set) { return set.Get(); }
        public void SetCallbacks(INoUnitSelectedActions instance)
        {
            if (m_Wrapper.m_NoUnitSelectedActionsCallbackInterface != null)
            {
            }
            m_Wrapper.m_NoUnitSelectedActionsCallbackInterface = instance;
            if (instance != null)
            {
            }
        }
    }
    public NoUnitSelectedActions @NoUnitSelected => new NoUnitSelectedActions(this);

    // UnitSelected
    private readonly InputActionMap m_UnitSelected;
    private IUnitSelectedActions m_UnitSelectedActionsCallbackInterface;
    private readonly InputAction m_UnitSelected_SelectPath;
    public struct UnitSelectedActions
    {
        private @Inputs m_Wrapper;
        public UnitSelectedActions(@Inputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @SelectPath => m_Wrapper.m_UnitSelected_SelectPath;
        public InputActionMap Get() { return m_Wrapper.m_UnitSelected; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UnitSelectedActions set) { return set.Get(); }
        public void SetCallbacks(IUnitSelectedActions instance)
        {
            if (m_Wrapper.m_UnitSelectedActionsCallbackInterface != null)
            {
                @SelectPath.started -= m_Wrapper.m_UnitSelectedActionsCallbackInterface.OnSelectPath;
                @SelectPath.performed -= m_Wrapper.m_UnitSelectedActionsCallbackInterface.OnSelectPath;
                @SelectPath.canceled -= m_Wrapper.m_UnitSelectedActionsCallbackInterface.OnSelectPath;
            }
            m_Wrapper.m_UnitSelectedActionsCallbackInterface = instance;
            if (instance != null)
            {
                @SelectPath.started += instance.OnSelectPath;
                @SelectPath.performed += instance.OnSelectPath;
                @SelectPath.canceled += instance.OnSelectPath;
            }
        }
    }
    public UnitSelectedActions @UnitSelected => new UnitSelectedActions(this);

    // ExecutionInteractions
    private readonly InputActionMap m_ExecutionInteractions;
    private IExecutionInteractionsActions m_ExecutionInteractionsActionsCallbackInterface;
    public struct ExecutionInteractionsActions
    {
        private @Inputs m_Wrapper;
        public ExecutionInteractionsActions(@Inputs wrapper) { m_Wrapper = wrapper; }
        public InputActionMap Get() { return m_Wrapper.m_ExecutionInteractions; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ExecutionInteractionsActions set) { return set.Get(); }
        public void SetCallbacks(IExecutionInteractionsActions instance)
        {
            if (m_Wrapper.m_ExecutionInteractionsActionsCallbackInterface != null)
            {
            }
            m_Wrapper.m_ExecutionInteractionsActionsCallbackInterface = instance;
            if (instance != null)
            {
            }
        }
    }
    public ExecutionInteractionsActions @ExecutionInteractions => new ExecutionInteractionsActions(this);
    public interface ICameraControlsActions
    {
        void OnMouseDelta(InputAction.CallbackContext context);
        void OnPointerPosition(InputAction.CallbackContext context);
        void OnCameraPan(InputAction.CallbackContext context);
        void OnCameraZoom(InputAction.CallbackContext context);
        void OnCameraRotation(InputAction.CallbackContext context);
    }
    public interface INextPlayerActions
    {
        void OnNext(InputAction.CallbackContext context);
    }
    public interface IPlanningPhaseActions
    {
        void OnPointerPosition(InputAction.CallbackContext context);
        void OnSelectUnit(InputAction.CallbackContext context);
    }
    public interface INoUnitSelectedActions
    {
    }
    public interface IUnitSelectedActions
    {
        void OnSelectPath(InputAction.CallbackContext context);
    }
    public interface IExecutionInteractionsActions
    {
    }
}
