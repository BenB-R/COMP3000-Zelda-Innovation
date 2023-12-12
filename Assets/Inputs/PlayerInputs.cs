//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.4
//     from Assets/Inputs/PlayerInputs.inputactions
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

public partial class @PlayerInputs : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputs()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputs"",
    ""maps"": [
        {
            ""name"": ""BasicMovement"",
            ""id"": ""b69710c1-8f7a-4d68-a534-b747c2f2fc73"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""f05e5f1a-50a9-409f-a778-eeb4b42fb84b"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Sprint"",
                    ""type"": ""Button"",
                    ""id"": ""09529ce1-99c7-4650-98f2-d53ed58fa01f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""aa3f1cac-58f0-4dc3-8605-904ddaa93655"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""64fb4757-8e7e-41f1-82db-ce2627d4348d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Menu"",
                    ""type"": ""Button"",
                    ""id"": ""27befc16-08c5-4e0a-b5e6-de83b2037b51"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""1869b1ba-113e-432d-aba5-37074615a4c2"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""0105f585-74fd-4c09-9189-a0f40c0e6c92"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""224b4b24-e82a-46bf-b117-b640fc93671d"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""1dfd9830-e747-4ee8-9266-aa95aa0975cf"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""fd3373d2-ab90-4cd5-bc48-c4fcbe59659b"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""0cb35061-aad7-4da0-95e6-8466493f9d15"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""60bf4e8a-2bf6-4d70-880b-d59b93bd674b"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cb7a67a3-3877-4f17-85a5-8fc757acc7ec"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""62f01737-de57-4c6c-9d05-da5bc0c92e8e"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Menu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""InMenu"",
            ""id"": ""8d9daf19-fa20-46e0-9b7c-6e5c7b082e0f"",
            ""actions"": [
                {
                    ""name"": ""Click"",
                    ""type"": ""Button"",
                    ""id"": ""e7f7610d-20db-4771-ad8b-e34ec93b0750"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""f60cd7c6-e501-4bfc-b8c8-6ea6b3186830"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Combat"",
            ""id"": ""f2893be6-d5f3-4106-aaba-db465c5e794c"",
            ""actions"": [
                {
                    ""name"": ""New action"",
                    ""type"": ""Button"",
                    ""id"": ""95b75bac-70b2-4a74-8d7a-f9a5f7323b44"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""1cd8c4ca-0015-4917-bc11-5f427bda9e07"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""New action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Gliding"",
            ""id"": ""9984118a-76d1-4854-8504-1590aaad94aa"",
            ""actions"": [
                {
                    ""name"": ""Glide"",
                    ""type"": ""Button"",
                    ""id"": ""fd0e68a4-3630-452e-b048-825fcafa0c21"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""af09c61d-39c6-4622-ac84-51650109ba77"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Glide"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Player"",
            ""bindingGroup"": ""Player"",
            ""devices"": []
        }
    ]
}");
        // BasicMovement
        m_BasicMovement = asset.FindActionMap("BasicMovement", throwIfNotFound: true);
        m_BasicMovement_Move = m_BasicMovement.FindAction("Move", throwIfNotFound: true);
        m_BasicMovement_Sprint = m_BasicMovement.FindAction("Sprint", throwIfNotFound: true);
        m_BasicMovement_Jump = m_BasicMovement.FindAction("Jump", throwIfNotFound: true);
        m_BasicMovement_Interact = m_BasicMovement.FindAction("Interact", throwIfNotFound: true);
        m_BasicMovement_Menu = m_BasicMovement.FindAction("Menu", throwIfNotFound: true);
        // InMenu
        m_InMenu = asset.FindActionMap("InMenu", throwIfNotFound: true);
        m_InMenu_Click = m_InMenu.FindAction("Click", throwIfNotFound: true);
        // Combat
        m_Combat = asset.FindActionMap("Combat", throwIfNotFound: true);
        m_Combat_Newaction = m_Combat.FindAction("New action", throwIfNotFound: true);
        // Gliding
        m_Gliding = asset.FindActionMap("Gliding", throwIfNotFound: true);
        m_Gliding_Glide = m_Gliding.FindAction("Glide", throwIfNotFound: true);
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

    // BasicMovement
    private readonly InputActionMap m_BasicMovement;
    private IBasicMovementActions m_BasicMovementActionsCallbackInterface;
    private readonly InputAction m_BasicMovement_Move;
    private readonly InputAction m_BasicMovement_Sprint;
    private readonly InputAction m_BasicMovement_Jump;
    private readonly InputAction m_BasicMovement_Interact;
    private readonly InputAction m_BasicMovement_Menu;
    public struct BasicMovementActions
    {
        private @PlayerInputs m_Wrapper;
        public BasicMovementActions(@PlayerInputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_BasicMovement_Move;
        public InputAction @Sprint => m_Wrapper.m_BasicMovement_Sprint;
        public InputAction @Jump => m_Wrapper.m_BasicMovement_Jump;
        public InputAction @Interact => m_Wrapper.m_BasicMovement_Interact;
        public InputAction @Menu => m_Wrapper.m_BasicMovement_Menu;
        public InputActionMap Get() { return m_Wrapper.m_BasicMovement; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(BasicMovementActions set) { return set.Get(); }
        public void SetCallbacks(IBasicMovementActions instance)
        {
            if (m_Wrapper.m_BasicMovementActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_BasicMovementActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_BasicMovementActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_BasicMovementActionsCallbackInterface.OnMove;
                @Sprint.started -= m_Wrapper.m_BasicMovementActionsCallbackInterface.OnSprint;
                @Sprint.performed -= m_Wrapper.m_BasicMovementActionsCallbackInterface.OnSprint;
                @Sprint.canceled -= m_Wrapper.m_BasicMovementActionsCallbackInterface.OnSprint;
                @Jump.started -= m_Wrapper.m_BasicMovementActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_BasicMovementActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_BasicMovementActionsCallbackInterface.OnJump;
                @Interact.started -= m_Wrapper.m_BasicMovementActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_BasicMovementActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_BasicMovementActionsCallbackInterface.OnInteract;
                @Menu.started -= m_Wrapper.m_BasicMovementActionsCallbackInterface.OnMenu;
                @Menu.performed -= m_Wrapper.m_BasicMovementActionsCallbackInterface.OnMenu;
                @Menu.canceled -= m_Wrapper.m_BasicMovementActionsCallbackInterface.OnMenu;
            }
            m_Wrapper.m_BasicMovementActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Sprint.started += instance.OnSprint;
                @Sprint.performed += instance.OnSprint;
                @Sprint.canceled += instance.OnSprint;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
                @Menu.started += instance.OnMenu;
                @Menu.performed += instance.OnMenu;
                @Menu.canceled += instance.OnMenu;
            }
        }
    }
    public BasicMovementActions @BasicMovement => new BasicMovementActions(this);

    // InMenu
    private readonly InputActionMap m_InMenu;
    private IInMenuActions m_InMenuActionsCallbackInterface;
    private readonly InputAction m_InMenu_Click;
    public struct InMenuActions
    {
        private @PlayerInputs m_Wrapper;
        public InMenuActions(@PlayerInputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @Click => m_Wrapper.m_InMenu_Click;
        public InputActionMap Get() { return m_Wrapper.m_InMenu; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(InMenuActions set) { return set.Get(); }
        public void SetCallbacks(IInMenuActions instance)
        {
            if (m_Wrapper.m_InMenuActionsCallbackInterface != null)
            {
                @Click.started -= m_Wrapper.m_InMenuActionsCallbackInterface.OnClick;
                @Click.performed -= m_Wrapper.m_InMenuActionsCallbackInterface.OnClick;
                @Click.canceled -= m_Wrapper.m_InMenuActionsCallbackInterface.OnClick;
            }
            m_Wrapper.m_InMenuActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Click.started += instance.OnClick;
                @Click.performed += instance.OnClick;
                @Click.canceled += instance.OnClick;
            }
        }
    }
    public InMenuActions @InMenu => new InMenuActions(this);

    // Combat
    private readonly InputActionMap m_Combat;
    private ICombatActions m_CombatActionsCallbackInterface;
    private readonly InputAction m_Combat_Newaction;
    public struct CombatActions
    {
        private @PlayerInputs m_Wrapper;
        public CombatActions(@PlayerInputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @Newaction => m_Wrapper.m_Combat_Newaction;
        public InputActionMap Get() { return m_Wrapper.m_Combat; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CombatActions set) { return set.Get(); }
        public void SetCallbacks(ICombatActions instance)
        {
            if (m_Wrapper.m_CombatActionsCallbackInterface != null)
            {
                @Newaction.started -= m_Wrapper.m_CombatActionsCallbackInterface.OnNewaction;
                @Newaction.performed -= m_Wrapper.m_CombatActionsCallbackInterface.OnNewaction;
                @Newaction.canceled -= m_Wrapper.m_CombatActionsCallbackInterface.OnNewaction;
            }
            m_Wrapper.m_CombatActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Newaction.started += instance.OnNewaction;
                @Newaction.performed += instance.OnNewaction;
                @Newaction.canceled += instance.OnNewaction;
            }
        }
    }
    public CombatActions @Combat => new CombatActions(this);

    // Gliding
    private readonly InputActionMap m_Gliding;
    private IGlidingActions m_GlidingActionsCallbackInterface;
    private readonly InputAction m_Gliding_Glide;
    public struct GlidingActions
    {
        private @PlayerInputs m_Wrapper;
        public GlidingActions(@PlayerInputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @Glide => m_Wrapper.m_Gliding_Glide;
        public InputActionMap Get() { return m_Wrapper.m_Gliding; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GlidingActions set) { return set.Get(); }
        public void SetCallbacks(IGlidingActions instance)
        {
            if (m_Wrapper.m_GlidingActionsCallbackInterface != null)
            {
                @Glide.started -= m_Wrapper.m_GlidingActionsCallbackInterface.OnGlide;
                @Glide.performed -= m_Wrapper.m_GlidingActionsCallbackInterface.OnGlide;
                @Glide.canceled -= m_Wrapper.m_GlidingActionsCallbackInterface.OnGlide;
            }
            m_Wrapper.m_GlidingActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Glide.started += instance.OnGlide;
                @Glide.performed += instance.OnGlide;
                @Glide.canceled += instance.OnGlide;
            }
        }
    }
    public GlidingActions @Gliding => new GlidingActions(this);
    private int m_PlayerSchemeIndex = -1;
    public InputControlScheme PlayerScheme
    {
        get
        {
            if (m_PlayerSchemeIndex == -1) m_PlayerSchemeIndex = asset.FindControlSchemeIndex("Player");
            return asset.controlSchemes[m_PlayerSchemeIndex];
        }
    }
    public interface IBasicMovementActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnSprint(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnMenu(InputAction.CallbackContext context);
    }
    public interface IInMenuActions
    {
        void OnClick(InputAction.CallbackContext context);
    }
    public interface ICombatActions
    {
        void OnNewaction(InputAction.CallbackContext context);
    }
    public interface IGlidingActions
    {
        void OnGlide(InputAction.CallbackContext context);
    }
}
