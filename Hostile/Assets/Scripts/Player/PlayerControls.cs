// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/Player/PlayerControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""InGame"",
            ""id"": ""edaa098d-25f6-403b-b7f1-5e09cb390b62"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Button"",
                    ""id"": ""925da7c0-76c1-43a1-8100-753190671fb8"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""129e9be6-b0e6-4015-8361-77a2a77bb7d4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""SpeedOnHold"",
                    ""type"": ""Button"",
                    ""id"": ""3c55c1b9-7476-46b3-a168-847baf9a1978"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SpeedSwap"",
                    ""type"": ""Button"",
                    ""id"": ""750a0c0f-4408-4c32-b346-01437a0b1ec0"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MouseMovement"",
                    ""type"": ""Value"",
                    ""id"": ""d9febb41-48b6-461f-afb7-089991747a6d"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Crouch"",
                    ""type"": ""Button"",
                    ""id"": ""cf134d64-8e0f-4f62-8c58-36d18d73d17f"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""ZQSD"",
                    ""id"": ""1308624b-f401-443b-a3ad-77579e0a6da2"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""f4114dc2-1087-4daf-b660-94efb41f3051"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""70ce7037-bea8-437c-a7bb-6348a8ae65c5"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""8b298c47-67ff-4976-bede-b4f2ac000882"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""dab4fb2b-ed78-44d4-b012-f88ef92c15ea"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""da59e4bb-9c56-4ae4-9f51-c4485b4ac7e7"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""31f2abdd-8b2d-4443-bb84-a7dd59b412b9"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": ""Hold"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SpeedOnHold"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b7e3ee85-2451-48e1-8047-77c28e44c528"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SpeedSwap"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""024926ee-82ca-4d65-95d5-e5196e1070bb"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f83799b3-2ebd-4db5-a5fa-d4d506471b92"",
                    ""path"": ""<Keyboard>/leftCtrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Crouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // InGame
        m_InGame = asset.FindActionMap("InGame", throwIfNotFound: true);
        m_InGame_Movement = m_InGame.FindAction("Movement", throwIfNotFound: true);
        m_InGame_Jump = m_InGame.FindAction("Jump", throwIfNotFound: true);
        m_InGame_SpeedOnHold = m_InGame.FindAction("SpeedOnHold", throwIfNotFound: true);
        m_InGame_SpeedSwap = m_InGame.FindAction("SpeedSwap", throwIfNotFound: true);
        m_InGame_MouseMovement = m_InGame.FindAction("MouseMovement", throwIfNotFound: true);
        m_InGame_Crouch = m_InGame.FindAction("Crouch", throwIfNotFound: true);
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

    // InGame
    private readonly InputActionMap m_InGame;
    private IInGameActions m_InGameActionsCallbackInterface;
    private readonly InputAction m_InGame_Movement;
    private readonly InputAction m_InGame_Jump;
    private readonly InputAction m_InGame_SpeedOnHold;
    private readonly InputAction m_InGame_SpeedSwap;
    private readonly InputAction m_InGame_MouseMovement;
    private readonly InputAction m_InGame_Crouch;
    public struct InGameActions
    {
        private @PlayerControls m_Wrapper;
        public InGameActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_InGame_Movement;
        public InputAction @Jump => m_Wrapper.m_InGame_Jump;
        public InputAction @SpeedOnHold => m_Wrapper.m_InGame_SpeedOnHold;
        public InputAction @SpeedSwap => m_Wrapper.m_InGame_SpeedSwap;
        public InputAction @MouseMovement => m_Wrapper.m_InGame_MouseMovement;
        public InputAction @Crouch => m_Wrapper.m_InGame_Crouch;
        public InputActionMap Get() { return m_Wrapper.m_InGame; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(InGameActions set) { return set.Get(); }
        public void SetCallbacks(IInGameActions instance)
        {
            if (m_Wrapper.m_InGameActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_InGameActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_InGameActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_InGameActionsCallbackInterface.OnMovement;
                @Jump.started -= m_Wrapper.m_InGameActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_InGameActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_InGameActionsCallbackInterface.OnJump;
                @SpeedOnHold.started -= m_Wrapper.m_InGameActionsCallbackInterface.OnSpeedOnHold;
                @SpeedOnHold.performed -= m_Wrapper.m_InGameActionsCallbackInterface.OnSpeedOnHold;
                @SpeedOnHold.canceled -= m_Wrapper.m_InGameActionsCallbackInterface.OnSpeedOnHold;
                @SpeedSwap.started -= m_Wrapper.m_InGameActionsCallbackInterface.OnSpeedSwap;
                @SpeedSwap.performed -= m_Wrapper.m_InGameActionsCallbackInterface.OnSpeedSwap;
                @SpeedSwap.canceled -= m_Wrapper.m_InGameActionsCallbackInterface.OnSpeedSwap;
                @MouseMovement.started -= m_Wrapper.m_InGameActionsCallbackInterface.OnMouseMovement;
                @MouseMovement.performed -= m_Wrapper.m_InGameActionsCallbackInterface.OnMouseMovement;
                @MouseMovement.canceled -= m_Wrapper.m_InGameActionsCallbackInterface.OnMouseMovement;
                @Crouch.started -= m_Wrapper.m_InGameActionsCallbackInterface.OnCrouch;
                @Crouch.performed -= m_Wrapper.m_InGameActionsCallbackInterface.OnCrouch;
                @Crouch.canceled -= m_Wrapper.m_InGameActionsCallbackInterface.OnCrouch;
            }
            m_Wrapper.m_InGameActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @SpeedOnHold.started += instance.OnSpeedOnHold;
                @SpeedOnHold.performed += instance.OnSpeedOnHold;
                @SpeedOnHold.canceled += instance.OnSpeedOnHold;
                @SpeedSwap.started += instance.OnSpeedSwap;
                @SpeedSwap.performed += instance.OnSpeedSwap;
                @SpeedSwap.canceled += instance.OnSpeedSwap;
                @MouseMovement.started += instance.OnMouseMovement;
                @MouseMovement.performed += instance.OnMouseMovement;
                @MouseMovement.canceled += instance.OnMouseMovement;
                @Crouch.started += instance.OnCrouch;
                @Crouch.performed += instance.OnCrouch;
                @Crouch.canceled += instance.OnCrouch;
            }
        }
    }
    public InGameActions @InGame => new InGameActions(this);
    public interface IInGameActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnSpeedOnHold(InputAction.CallbackContext context);
        void OnSpeedSwap(InputAction.CallbackContext context);
        void OnMouseMovement(InputAction.CallbackContext context);
        void OnCrouch(InputAction.CallbackContext context);
    }
}
