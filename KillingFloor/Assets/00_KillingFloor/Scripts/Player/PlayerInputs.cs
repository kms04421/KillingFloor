using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviourPun
{
    [Header("Player Input Values")]
    public Vector2 move;
    public Vector2 look;
    public bool jump;
    public bool dash;
    public bool shoot;
    public bool aim;
    public bool reload;
    public bool changeCamera;
    public bool weaponSlot1;
    public bool weaponSlot2;
    public bool weaponSlot3;
    public bool weaponSlot4;
    public float scroll;
    public bool grenade;
    public bool equip;
    public bool cancle;
    

    [Header("Mouse Cursor Settings")]
    public bool cursorLocked = true;
    public bool cursorInputForLook = true;

    // ��ǲ�ý��� �Է� ================================================

    // �÷��̾� �̵� �Է�
    public void OnMove(InputValue value)
    {
        MoveInput(value.Get<Vector2>());
    }
    // �÷��̾� �� ���콺 ��Ÿ�� �Է�
    public void OnLook(InputValue value)
    {
        if (cursorInputForLook)
        {
            LookInput(value.Get<Vector2>());
        }
    }
    // ���� �Է�
    public void OnJump(InputValue value)
    {
        JumpInput(value.isPressed);
    }

    // ��� �Է�
    public void OnDash(InputValue value)
    {
        dashInput(value.isPressed);
    }
 
    // ��� �Է�
    public void OnShoot(InputValue value)
    {
        ShootInput(value.isPressed);
    }
    public void OnAim(InputValue value)
    {
        AimInput(value.isPressed);
    }
    // ������ �Է�
    public void OnReload(InputValue value)
    {
        ReloadInput(value.isPressed);
    }
    public void OnGrenade(InputValue value)
    {
        GrenadeInput(value.isPressed);
    }

    // ī�޶� ����
    public void OnChangeCamera(InputValue value)
    {
        ChangeCameraInput(value.isPressed);
    }

    public void OnWeaponSlot1(InputValue value)
    {
        WeaponSlot1Input(value.isPressed);
    }
    public void OnWeaponSlot2(InputValue value)
    {
        WeaponSlot2Input(value.isPressed);
    }
    public void OnWeaponSlot3(InputValue value)
    {
        WeaponSlot3Input(value.isPressed);
    }
    public void OnWeaponSlot4(InputValue value)
    {
        WeaponSlot4Input(value.isPressed);
    }
    public void OnScroll(InputValue value)
    {
        ScrollInput(value.Get<float>()); ;
    }
    public void OnEquip(InputValue value)
    {
        EquipInput(value.isPressed);
    }
    public void OnCancle(InputValue value)
    {
        CancleInput(value.isPressed);
    }

    // �Է��� ��ȯ ================================================
    public void MoveInput(Vector2 newMoveDirection)
    {
        move = newMoveDirection;
    }
    public void LookInput(Vector2 newLookDirection)
    {
        look = newLookDirection;
    }

    public void JumpInput(bool newJumpState)
    {
        jump = newJumpState;
    }

    public void dashInput(bool newDashState)
    {
        dash = newDashState;
    }
   
    public void ShootInput(bool newShoot)
    {
        shoot = newShoot;
    }

    public void AimInput(bool newAim)
    {
        aim = newAim;
    }
    public void ReloadInput(bool newReload)
    {
        reload = newReload;
    }
    public void ChangeCameraInput(bool newCameraState)
    {
        changeCamera = newCameraState;
    }
    public void WeaponSlot1Input(bool newSlot)
    {
        weaponSlot1 = newSlot;
    }
    public void WeaponSlot2Input(bool newSlot)
    {
        weaponSlot2 = newSlot;
    }
    public void WeaponSlot3Input(bool newSlot)
    {
        weaponSlot3 = newSlot;
    }
    public void WeaponSlot4Input(bool newSlot)
    {
        weaponSlot4 = newSlot;
    }
    public void ScrollInput(float newScroll)
    {
        scroll = newScroll;
    }
    public void GrenadeInput(bool newGrenade)
    {
        grenade = newGrenade;
    }
    public void EquipInput(bool newEquip)
    {
        equip = newEquip;
    }
    public void CancleInput(bool newCancle)
    {
        cancle = newCancle;
    }
    // ī�޶� ���콺 ���� ================================================
    private void OnApplicationFocus(bool hasFocus)
    {
        SetCursorState(cursorLocked);
    }
    private void SetCursorState(bool newState)
    {
        if (!photonView.IsMine) { return; } // ���� �÷��̾ �ƴ� ��� �Է��� ���� �ʴ´�.

        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }

}
