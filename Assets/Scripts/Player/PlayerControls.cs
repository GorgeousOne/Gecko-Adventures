//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.3.0
//     from Assets/Scripts/Player/PlayerControls.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerControls : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""73adc155-e840-427f-8c76-3129608a224c"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Button"",
                    ""id"": ""6129f9f7-50b6-4e02-ac9c-ad3a4f39faec"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""4e2da69e-dda0-4072-9993-5661233598d9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""521f2abe-6fd3-4068-87f3-66684b9dcbe1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Crouch"",
                    ""type"": ""Button"",
                    ""id"": ""e2847d61-4502-4716-b557-35db99e81c40"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""TongueAim"",
                    ""type"": ""Value"",
                    ""id"": ""aaefe1d8-21e5-4aee-8ef7-bf156cb52a28"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""TongueShoot"",
                    ""type"": ""Button"",
                    ""id"": ""f817135c-95e9-45ba-898a-d6e3f1a29e82"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""TongueShootGamepad"",
                    ""type"": ""Value"",
                    ""id"": ""71ec0013-73b5-4b3e-9fe6-a95e9c6889b7"",
                    ""expectedControlType"": ""Stick"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""TongueRetract"",
                    ""type"": ""Button"",
                    ""id"": ""fdcce477-f2dc-4d87-aef4-b43eee4c9148"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""TongueExtend"",
                    ""type"": ""Button"",
                    ""id"": ""0f6aa197-337e-4d36-88e5-00329ce4a97a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Horizontal (WASD Keys)"",
                    ""id"": ""e0088bb8-1884-40f4-a2ea-343ff2247ac4"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""446408cf-d91d-4402-b8d6-c9715ee75156"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""d1a4a976-7502-4b31-860d-2c35fd0c2132"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Horizontal (Arrow Keys)"",
                    ""id"": ""00029959-908c-4068-ae0e-9c1ac1c0a4d0"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""955116a4-69d1-48d8-be08-b73ed9112809"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""04136b1e-5ede-4675-b54f-0b1a31b6b9de"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Horizontal (Gamepad)"",
                    ""id"": ""b24aa40a-d6f2-4bc6-a270-bd1a8ccebc68"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""609d19ff-1fb4-4ca4-acf4-e56ce5a9d93d"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""926a5da1-3710-4b0e-9e0e-65a00b13855f"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""5459a65c-953b-4933-aaf3-8e3bc84e1d68"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0e859e5d-bab4-4077-93df-61abe26df2a6"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e056116a-ab55-483d-b55e-e8442c7aa61b"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""affde894-6251-496a-9836-5243843c925e"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2476d0d9-af0c-4be9-8701-478a90888593"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""TongueShoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7d910a58-da67-4666-83f8-1ec9ab233b56"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TongueShootGamepad"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""43c1b392-1261-4150-872d-9022f557ff78"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""TongueAim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""63d9bb97-f2f4-4adc-ba05-141eabba953d"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Crouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c30a532c-c279-43ab-9139-3b04f1d54481"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Crouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2cd02344-3452-46cf-bd57-a913034aafd7"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Crouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a3853670-6ba3-43e8-a9d3-08151371b169"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""TongueRetract"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""11200629-5800-4146-8a7c-b062c828f79b"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""TongueRetract"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3b316bbd-f181-4133-b8c5-7fb35f7dab66"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""TongueExtend"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""784e7cc8-45e1-46b8-b867-5aa3e06414e1"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TongueExtend"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Controller"",
            ""bindingGroup"": ""Controller"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Move = m_Player.FindAction("Move", throwIfNotFound: true);
        m_Player_Interact = m_Player.FindAction("Interact", throwIfNotFound: true);
        m_Player_Jump = m_Player.FindAction("Jump", throwIfNotFound: true);
        m_Player_Crouch = m_Player.FindAction("Crouch", throwIfNotFound: true);
        m_Player_TongueAim = m_Player.FindAction("TongueAim", throwIfNotFound: true);
        m_Player_TongueShoot = m_Player.FindAction("TongueShoot", throwIfNotFound: true);
        m_Player_TongueShootGamepad = m_Player.FindAction("TongueShootGamepad", throwIfNotFound: true);
        m_Player_TongueRetract = m_Player.FindAction("TongueRetract", throwIfNotFound: true);
        m_Player_TongueExtend = m_Player.FindAction("TongueExtend", throwIfNotFound: true);
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
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_Move;
    private readonly InputAction m_Player_Interact;
    private readonly InputAction m_Player_Jump;
    private readonly InputAction m_Player_Crouch;
    private readonly InputAction m_Player_TongueAim;
    private readonly InputAction m_Player_TongueShoot;
    private readonly InputAction m_Player_TongueShootGamepad;
    private readonly InputAction m_Player_TongueRetract;
    private readonly InputAction m_Player_TongueExtend;
    public struct PlayerActions
    {
        private @PlayerControls m_Wrapper;
        public PlayerActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Player_Move;
        public InputAction @Interact => m_Wrapper.m_Player_Interact;
        public InputAction @Jump => m_Wrapper.m_Player_Jump;
        public InputAction @Crouch => m_Wrapper.m_Player_Crouch;
        public InputAction @TongueAim => m_Wrapper.m_Player_TongueAim;
        public InputAction @TongueShoot => m_Wrapper.m_Player_TongueShoot;
        public InputAction @TongueShootGamepad => m_Wrapper.m_Player_TongueShootGamepad;
        public InputAction @TongueRetract => m_Wrapper.m_Player_TongueRetract;
        public InputAction @TongueExtend => m_Wrapper.m_Player_TongueExtend;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @Interact.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteract;
                @Jump.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Crouch.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCrouch;
                @Crouch.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCrouch;
                @Crouch.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCrouch;
                @TongueAim.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTongueAim;
                @TongueAim.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTongueAim;
                @TongueAim.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTongueAim;
                @TongueShoot.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTongueShoot;
                @TongueShoot.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTongueShoot;
                @TongueShoot.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTongueShoot;
                @TongueShootGamepad.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTongueShootGamepad;
                @TongueShootGamepad.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTongueShootGamepad;
                @TongueShootGamepad.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTongueShootGamepad;
                @TongueRetract.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTongueRetract;
                @TongueRetract.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTongueRetract;
                @TongueRetract.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTongueRetract;
                @TongueExtend.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTongueExtend;
                @TongueExtend.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTongueExtend;
                @TongueExtend.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTongueExtend;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Crouch.started += instance.OnCrouch;
                @Crouch.performed += instance.OnCrouch;
                @Crouch.canceled += instance.OnCrouch;
                @TongueAim.started += instance.OnTongueAim;
                @TongueAim.performed += instance.OnTongueAim;
                @TongueAim.canceled += instance.OnTongueAim;
                @TongueShoot.started += instance.OnTongueShoot;
                @TongueShoot.performed += instance.OnTongueShoot;
                @TongueShoot.canceled += instance.OnTongueShoot;
                @TongueShootGamepad.started += instance.OnTongueShootGamepad;
                @TongueShootGamepad.performed += instance.OnTongueShootGamepad;
                @TongueShootGamepad.canceled += instance.OnTongueShootGamepad;
                @TongueRetract.started += instance.OnTongueRetract;
                @TongueRetract.performed += instance.OnTongueRetract;
                @TongueRetract.canceled += instance.OnTongueRetract;
                @TongueExtend.started += instance.OnTongueExtend;
                @TongueExtend.performed += instance.OnTongueExtend;
                @TongueExtend.canceled += instance.OnTongueExtend;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    private int m_KeyboardSchemeIndex = -1;
    public InputControlScheme KeyboardScheme
    {
        get
        {
            if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
            return asset.controlSchemes[m_KeyboardSchemeIndex];
        }
    }
    private int m_ControllerSchemeIndex = -1;
    public InputControlScheme ControllerScheme
    {
        get
        {
            if (m_ControllerSchemeIndex == -1) m_ControllerSchemeIndex = asset.FindControlSchemeIndex("Controller");
            return asset.controlSchemes[m_ControllerSchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnCrouch(InputAction.CallbackContext context);
        void OnTongueAim(InputAction.CallbackContext context);
        void OnTongueShoot(InputAction.CallbackContext context);
        void OnTongueShootGamepad(InputAction.CallbackContext context);
        void OnTongueRetract(InputAction.CallbackContext context);
        void OnTongueExtend(InputAction.CallbackContext context);
    }
}
