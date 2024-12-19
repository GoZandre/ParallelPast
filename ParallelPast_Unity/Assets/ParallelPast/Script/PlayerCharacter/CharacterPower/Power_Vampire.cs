using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "VampirePower", menuName = "ScriptableObjects/Powers/VampirePower")]

public class Power_Vampire : So_CharacterPower
{
    [SerializeField]
    private SkullHead _skullHeadPrefab;

   
    private List<SkullHead> _skullHeadLaunched = new List<SkullHead>();
    public List<SkullHead> SkullHeadLaunched => _skullHeadLaunched;

    private int _skullHeadIndex;

    public override void EnablePower(PlayerController playerController, bool doTransform = true)
    {
        base.EnablePower(playerController, doTransform);
        _playerController.CanJump = false;

        _skullHeadIndex = 0;

        Debug.Log("Active vampire power");
    }

    public override void InvokePower()
    {
        if(_skullHeadIndex <= _skullHeadLaunched.Count)
        {
            _playerController.CanReceiveMovementInputs = false;

            _skullHeadLaunched.Add(Instantiate(_skullHeadPrefab));
            _skullHeadLaunched[_skullHeadIndex].transform.position = _playerController.transform.position;

            base.InvokePower();

            Time.timeScale = 0;
        }
        

    }
    public override void CancelPower()
    {
        base.CancelPower();

        if(_skullHeadIndex <= _skullHeadLaunched.Count)
        {
            if (_skullHeadLaunched[_skullHeadIndex].IsLaunched)
            {
                _playerController.transform.position = _skullHeadLaunched[_skullHeadIndex].transform.position;
                _skullHeadIndex++;
            }
            else
            {
                _skullHeadLaunched[_skullHeadIndex].LaunchSkull();
            }
        }

        _playerController.CanReceiveMovementInputs = true;

        Time.timeScale = 1;

    }
}
