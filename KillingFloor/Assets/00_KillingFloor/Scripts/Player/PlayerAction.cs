//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.6.3
//     from Assets/00_KillingFloor/Scripts/Player/PlayerAction.inputactions
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

public partial class @PlayerAction: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerAction()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerAction"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""99391073-5063-42ad-8828-e313326257a2"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""bd05ab00-f6b6-4655-a79c-e662122684e6"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""c53404bd-d4b9-47b2-b4c4-a2df02c65427"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""d6815ecb-92e2-4cb9-96f7-5fe80a55abb0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Dash"",
                    ""type"": ""PassThrough"",
                    ""id"": ""495ac11d-2be8-4508-8e97-5ba349626516"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Crouch"",
                    ""type"": ""PassThrough"",
                    ""id"": ""95dfd024-be57-491b-a833-e091eb8d8256"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Reload"",
                    ""type"": ""Button"",
                    ""id"": ""9bf25a23-7a96-4c63-aff1-409069872106"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Melee"",
                    ""type"": ""Button"",
                    ""id"": ""681d31d9-7ad2-4df5-ac47-b597fd1fbdbd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Shoot"",
                    ""type"": ""PassThrough"",
                    ""id"": ""36407f72-dc9a-4f8e-81a3-0537aa900a28"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Aim"",
                    ""type"": ""PassThrough"",
                    ""id"": ""b823918c-087e-4ff4-bb4d-9e0a74c3fd18"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""WeaponSlot1"",
                    ""type"": ""Button"",
                    ""id"": ""bd73d223-76ea-4805-9d4c-a74efa090a5a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""WeaponSlot2"",
                    ""type"": ""Button"",
                    ""id"": ""b93a52ca-8273-4220-814f-0939a1b68d6d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""WeaponSlot3"",
                    ""type"": ""Button"",
                    ""id"": ""217a58c1-a1c8-4681-bb11-c5473a891643"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""WeaponSlot4"",
                    ""type"": ""Button"",
                    ""id"": ""93d89879-5371-4f7c-a362-10f31f32475a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Scroll"",
                    ""type"": ""Value"",
                    ""id"": ""904e7841-5889-4e51-8f8f-0a6b4ec1f82b"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Grenade"",
                    ""type"": ""Button"",
                    ""id"": ""19222c20-0558-459d-8a7c-82d18cb68c0d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Equip"",
                    ""type"": ""PassThrough"",
                    ""id"": ""93817ded-a93e-464e-b1f2-2fa2892cb3e2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Cancle"",
                    ""type"": ""PassThrough"",
                    ""id"": ""241b3356-95ae-4a4d-86f4-1adf0ac32e11"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ChangeCamera"",
                    ""type"": ""Button"",
                    ""id"": ""646a377b-ca3c-43a0-b6f5-f75a8e612d36"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Cheat"",
                    ""type"": ""Button"",
                    ""id"": ""b2f05880-fc64-4f92-8f36-cdacef84a794"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""b8f4c213-088a-4d31-ade9-a60c516f6fc3"",
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
                    ""id"": ""9c8d8cbb-749f-45b9-955b-65fd2d6132b4"",
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
                    ""id"": ""967c7440-e967-412f-b198-ccdbe49cd563"",
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
                    ""id"": ""d56a525e-b362-43b4-92ca-0af3ce67bc28"",
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
                    ""id"": ""c905bde5-656f-45a9-bb4e-a27b98e8335e"",
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
                    ""id"": ""d9106779-b434-4674-bab6-72fdf4fe32b9"",
                    ""path"": ""<XInputController>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""27bc4b06-71e8-4b70-97dd-96d2920e1947"",
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
                    ""id"": ""55e34a81-b5bc-4532-af28-6bb7fb3fc880"",
                    ""path"": ""<XInputController>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4ab0c5ee-4fcf-4bb1-a688-0f5189abeb98"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""789af24b-a515-407c-b95f-a0b0d0b8b94a"",
                    ""path"": ""<XInputController>/leftStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1bd0a982-97e2-4e8c-ac5c-0ab12921d584"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Crouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f951ce28-c086-4a65-9623-5a6432124ad5"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Reload"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e1ec4eab-27d9-47db-b554-8aa9f70acae3"",
                    ""path"": ""<Keyboard>/v"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Melee"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""53d730e4-d2bc-4ff5-8263-b19f8e9a69c6"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""788d0daf-54d3-4e40-b1ab-5b9a21222573"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""WeaponSlot1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""baaec203-bb2c-4652-880e-33e8b5c7c804"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""WeaponSlot2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4958f84e-4a91-4bd3-8bf7-d2c0d3798d92"",
                    ""path"": ""<Pointer>/delta"",
                    ""interactions"": """",
                    ""processors"": ""InvertVector2(invertX=false),ScaleVector2"",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""09747fcb-324d-4e3d-97c1-40f99303cc26"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6db3e24f-4f96-4ea2-8dbf-b80d205b5a6b"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""WeaponSlot3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fe0473c9-2065-472d-8dc3-d8dcec2cd16c"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""WeaponSlot4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6913d6a8-adf2-4a56-8673-b04e29f932f9"",
                    ""path"": ""<Mouse>/scroll/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Scroll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c14d88a6-c238-4cbb-b925-09657eea53e6"",
                    ""path"": ""<Keyboard>/g"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Grenade"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8d288b66-e775-427f-8a51-e81e7ec8cf10"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Equip"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f56b0266-c7e8-405e-b845-9c0d9329fcc6"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cancle"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c7779095-2879-4adb-bbd3-aa9f883a1469"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3c3f3934-3666-487a-90e8-b975be9cb262"",
                    ""path"": ""<Keyboard>/i"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ChangeCamera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fc102eb1-3858-4413-83cc-d46de5ee2c37"",
                    ""path"": ""<XInputController>/rightStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ChangeCamera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""93d9aa15-9b54-432b-a2b4-30c446919567"",
                    ""path"": ""<Keyboard>/f1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cheat"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Move = m_Player.FindAction("Move", throwIfNotFound: true);
        m_Player_Look = m_Player.FindAction("Look", throwIfNotFound: true);
        m_Player_Jump = m_Player.FindAction("Jump", throwIfNotFound: true);
        m_Player_Dash = m_Player.FindAction("Dash", throwIfNotFound: true);
        m_Player_Crouch = m_Player.FindAction("Crouch", throwIfNotFound: true);
        m_Player_Reload = m_Player.FindAction("Reload", throwIfNotFound: true);
        m_Player_Melee = m_Player.FindAction("Melee", throwIfNotFound: true);
        m_Player_Shoot = m_Player.FindAction("Shoot", throwIfNotFound: true);
        m_Player_Aim = m_Player.FindAction("Aim", throwIfNotFound: true);
        m_Player_WeaponSlot1 = m_Player.FindAction("WeaponSlot1", throwIfNotFound: true);
        m_Player_WeaponSlot2 = m_Player.FindAction("WeaponSlot2", throwIfNotFound: true);
        m_Player_WeaponSlot3 = m_Player.FindAction("WeaponSlot3", throwIfNotFound: true);
        m_Player_WeaponSlot4 = m_Player.FindAction("WeaponSlot4", throwIfNotFound: true);
        m_Player_Scroll = m_Player.FindAction("Scroll", throwIfNotFound: true);
        m_Player_Grenade = m_Player.FindAction("Grenade", throwIfNotFound: true);
        m_Player_Equip = m_Player.FindAction("Equip", throwIfNotFound: true);
        m_Player_Cancle = m_Player.FindAction("Cancle", throwIfNotFound: true);
        m_Player_ChangeCamera = m_Player.FindAction("ChangeCamera", throwIfNotFound: true);
        m_Player_Cheat = m_Player.FindAction("Cheat", throwIfNotFound: true);
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
    private List<IPlayerActions> m_PlayerActionsCallbackInterfaces = new List<IPlayerActions>();
    private readonly InputAction m_Player_Move;
    private readonly InputAction m_Player_Look;
    private readonly InputAction m_Player_Jump;
    private readonly InputAction m_Player_Dash;
    private readonly InputAction m_Player_Crouch;
    private readonly InputAction m_Player_Reload;
    private readonly InputAction m_Player_Melee;
    private readonly InputAction m_Player_Shoot;
    private readonly InputAction m_Player_Aim;
    private readonly InputAction m_Player_WeaponSlot1;
    private readonly InputAction m_Player_WeaponSlot2;
    private readonly InputAction m_Player_WeaponSlot3;
    private readonly InputAction m_Player_WeaponSlot4;
    private readonly InputAction m_Player_Scroll;
    private readonly InputAction m_Player_Grenade;
    private readonly InputAction m_Player_Equip;
    private readonly InputAction m_Player_Cancle;
    private readonly InputAction m_Player_ChangeCamera;
    private readonly InputAction m_Player_Cheat;
    public struct PlayerActions
    {
        private @PlayerAction m_Wrapper;
        public PlayerActions(@PlayerAction wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Player_Move;
        public InputAction @Look => m_Wrapper.m_Player_Look;
        public InputAction @Jump => m_Wrapper.m_Player_Jump;
        public InputAction @Dash => m_Wrapper.m_Player_Dash;
        public InputAction @Crouch => m_Wrapper.m_Player_Crouch;
        public InputAction @Reload => m_Wrapper.m_Player_Reload;
        public InputAction @Melee => m_Wrapper.m_Player_Melee;
        public InputAction @Shoot => m_Wrapper.m_Player_Shoot;
        public InputAction @Aim => m_Wrapper.m_Player_Aim;
        public InputAction @WeaponSlot1 => m_Wrapper.m_Player_WeaponSlot1;
        public InputAction @WeaponSlot2 => m_Wrapper.m_Player_WeaponSlot2;
        public InputAction @WeaponSlot3 => m_Wrapper.m_Player_WeaponSlot3;
        public InputAction @WeaponSlot4 => m_Wrapper.m_Player_WeaponSlot4;
        public InputAction @Scroll => m_Wrapper.m_Player_Scroll;
        public InputAction @Grenade => m_Wrapper.m_Player_Grenade;
        public InputAction @Equip => m_Wrapper.m_Player_Equip;
        public InputAction @Cancle => m_Wrapper.m_Player_Cancle;
        public InputAction @ChangeCamera => m_Wrapper.m_Player_ChangeCamera;
        public InputAction @Cheat => m_Wrapper.m_Player_Cheat;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Add(instance);
            @Move.started += instance.OnMove;
            @Move.performed += instance.OnMove;
            @Move.canceled += instance.OnMove;
            @Look.started += instance.OnLook;
            @Look.performed += instance.OnLook;
            @Look.canceled += instance.OnLook;
            @Jump.started += instance.OnJump;
            @Jump.performed += instance.OnJump;
            @Jump.canceled += instance.OnJump;
            @Dash.started += instance.OnDash;
            @Dash.performed += instance.OnDash;
            @Dash.canceled += instance.OnDash;
            @Crouch.started += instance.OnCrouch;
            @Crouch.performed += instance.OnCrouch;
            @Crouch.canceled += instance.OnCrouch;
            @Reload.started += instance.OnReload;
            @Reload.performed += instance.OnReload;
            @Reload.canceled += instance.OnReload;
            @Melee.started += instance.OnMelee;
            @Melee.performed += instance.OnMelee;
            @Melee.canceled += instance.OnMelee;
            @Shoot.started += instance.OnShoot;
            @Shoot.performed += instance.OnShoot;
            @Shoot.canceled += instance.OnShoot;
            @Aim.started += instance.OnAim;
            @Aim.performed += instance.OnAim;
            @Aim.canceled += instance.OnAim;
            @WeaponSlot1.started += instance.OnWeaponSlot1;
            @WeaponSlot1.performed += instance.OnWeaponSlot1;
            @WeaponSlot1.canceled += instance.OnWeaponSlot1;
            @WeaponSlot2.started += instance.OnWeaponSlot2;
            @WeaponSlot2.performed += instance.OnWeaponSlot2;
            @WeaponSlot2.canceled += instance.OnWeaponSlot2;
            @WeaponSlot3.started += instance.OnWeaponSlot3;
            @WeaponSlot3.performed += instance.OnWeaponSlot3;
            @WeaponSlot3.canceled += instance.OnWeaponSlot3;
            @WeaponSlot4.started += instance.OnWeaponSlot4;
            @WeaponSlot4.performed += instance.OnWeaponSlot4;
            @WeaponSlot4.canceled += instance.OnWeaponSlot4;
            @Scroll.started += instance.OnScroll;
            @Scroll.performed += instance.OnScroll;
            @Scroll.canceled += instance.OnScroll;
            @Grenade.started += instance.OnGrenade;
            @Grenade.performed += instance.OnGrenade;
            @Grenade.canceled += instance.OnGrenade;
            @Equip.started += instance.OnEquip;
            @Equip.performed += instance.OnEquip;
            @Equip.canceled += instance.OnEquip;
            @Cancle.started += instance.OnCancle;
            @Cancle.performed += instance.OnCancle;
            @Cancle.canceled += instance.OnCancle;
            @ChangeCamera.started += instance.OnChangeCamera;
            @ChangeCamera.performed += instance.OnChangeCamera;
            @ChangeCamera.canceled += instance.OnChangeCamera;
            @Cheat.started += instance.OnCheat;
            @Cheat.performed += instance.OnCheat;
            @Cheat.canceled += instance.OnCheat;
        }

        private void UnregisterCallbacks(IPlayerActions instance)
        {
            @Move.started -= instance.OnMove;
            @Move.performed -= instance.OnMove;
            @Move.canceled -= instance.OnMove;
            @Look.started -= instance.OnLook;
            @Look.performed -= instance.OnLook;
            @Look.canceled -= instance.OnLook;
            @Jump.started -= instance.OnJump;
            @Jump.performed -= instance.OnJump;
            @Jump.canceled -= instance.OnJump;
            @Dash.started -= instance.OnDash;
            @Dash.performed -= instance.OnDash;
            @Dash.canceled -= instance.OnDash;
            @Crouch.started -= instance.OnCrouch;
            @Crouch.performed -= instance.OnCrouch;
            @Crouch.canceled -= instance.OnCrouch;
            @Reload.started -= instance.OnReload;
            @Reload.performed -= instance.OnReload;
            @Reload.canceled -= instance.OnReload;
            @Melee.started -= instance.OnMelee;
            @Melee.performed -= instance.OnMelee;
            @Melee.canceled -= instance.OnMelee;
            @Shoot.started -= instance.OnShoot;
            @Shoot.performed -= instance.OnShoot;
            @Shoot.canceled -= instance.OnShoot;
            @Aim.started -= instance.OnAim;
            @Aim.performed -= instance.OnAim;
            @Aim.canceled -= instance.OnAim;
            @WeaponSlot1.started -= instance.OnWeaponSlot1;
            @WeaponSlot1.performed -= instance.OnWeaponSlot1;
            @WeaponSlot1.canceled -= instance.OnWeaponSlot1;
            @WeaponSlot2.started -= instance.OnWeaponSlot2;
            @WeaponSlot2.performed -= instance.OnWeaponSlot2;
            @WeaponSlot2.canceled -= instance.OnWeaponSlot2;
            @WeaponSlot3.started -= instance.OnWeaponSlot3;
            @WeaponSlot3.performed -= instance.OnWeaponSlot3;
            @WeaponSlot3.canceled -= instance.OnWeaponSlot3;
            @WeaponSlot4.started -= instance.OnWeaponSlot4;
            @WeaponSlot4.performed -= instance.OnWeaponSlot4;
            @WeaponSlot4.canceled -= instance.OnWeaponSlot4;
            @Scroll.started -= instance.OnScroll;
            @Scroll.performed -= instance.OnScroll;
            @Scroll.canceled -= instance.OnScroll;
            @Grenade.started -= instance.OnGrenade;
            @Grenade.performed -= instance.OnGrenade;
            @Grenade.canceled -= instance.OnGrenade;
            @Equip.started -= instance.OnEquip;
            @Equip.performed -= instance.OnEquip;
            @Equip.canceled -= instance.OnEquip;
            @Cancle.started -= instance.OnCancle;
            @Cancle.performed -= instance.OnCancle;
            @Cancle.canceled -= instance.OnCancle;
            @ChangeCamera.started -= instance.OnChangeCamera;
            @ChangeCamera.performed -= instance.OnChangeCamera;
            @ChangeCamera.canceled -= instance.OnChangeCamera;
            @Cheat.started -= instance.OnCheat;
            @Cheat.performed -= instance.OnCheat;
            @Cheat.canceled -= instance.OnCheat;
        }

        public void RemoveCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    public interface IPlayerActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnDash(InputAction.CallbackContext context);
        void OnCrouch(InputAction.CallbackContext context);
        void OnReload(InputAction.CallbackContext context);
        void OnMelee(InputAction.CallbackContext context);
        void OnShoot(InputAction.CallbackContext context);
        void OnAim(InputAction.CallbackContext context);
        void OnWeaponSlot1(InputAction.CallbackContext context);
        void OnWeaponSlot2(InputAction.CallbackContext context);
        void OnWeaponSlot3(InputAction.CallbackContext context);
        void OnWeaponSlot4(InputAction.CallbackContext context);
        void OnScroll(InputAction.CallbackContext context);
        void OnGrenade(InputAction.CallbackContext context);
        void OnEquip(InputAction.CallbackContext context);
        void OnCancle(InputAction.CallbackContext context);
        void OnChangeCamera(InputAction.CallbackContext context);
        void OnCheat(InputAction.CallbackContext context);
    }
}
