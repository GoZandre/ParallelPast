using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpLocker : MonoBehaviour
{
    [SerializeField]
    private PlayerController _playerController;

    public void EnableJump()
    {
        _playerController.CanJump = true;
    }

    public void DisableJump()
    {
        _playerController.CanJump = true;
    }

}
