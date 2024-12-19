using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "GrowingPlantPower", menuName = "ScriptableObjects/Powers/GrowingPlantPower")]
public class Power_GrowingPlants : So_CharacterPower
{
    [SerializeField]
    private GrowingPlants _growingPlantsPrefab;
    public GrowingPlants GrowingPlants => _growingPlantsPrefab;

    public override void EnablePower(PlayerController playerController, bool doTransform = true)
    {
        base.EnablePower(playerController, doTransform);
        _playerController.CanJump = false;
    }

    private GrowingPlants _currentGrowingPlant;

    public override void InvokePower()
    {
        _playerController.CanReceiveMovementInputs = false;

        base.InvokePower();

        float offsetX = 0;
        float h = 0;
        
        _playerController.RigidBody2D.velocity = new Vector2(0, _playerController.RigidBody2D.velocity.y);
        Vector2 roundPlayerPos = new Vector2(Mathf.Round(_playerController.transform.position.x),_playerController.transform.position.y);

        RaycastHit2D rayHit2D = Physics2D.Raycast(roundPlayerPos, Vector2.down, Mathf.Infinity, LayerMask.GetMask("World"));

        if (rayHit2D.collider.TryGetComponent<GrowingPlantLocker>(out GrowingPlantLocker locker))
        {
            return;
        }
        else
        {
            h = rayHit2D.distance + 0.05f;
            DebugHeight = h;

            for (int i = -2; i <= 2; i++)
            {
                Vector2 raycastOffset = roundPlayerPos + new Vector2(i, 0);
                RaycastHit2D rayHitOffset2D = Physics2D.Raycast(raycastOffset, Vector2.down, h, LayerMask.GetMask("World"));

                if (rayHitOffset2D.collider != null)
                {
                    if(i < 0)
                    {
                        offsetX --;
                    }
                    else if(i > 0)
                    {
                        offsetX ++;
                    }
                }
            }
        }

        Debug.Log(offsetX);

        _currentGrowingPlant = Instantiate(_growingPlantsPrefab, _playerController.transform);

        _currentGrowingPlant.transform.parent = null;
        _currentGrowingPlant.transform.position = new Vector2(rayHit2D.point.x + offsetX, rayHit2D.point.y);
       

        _currentGrowingPlant.DisableCharacterCollision(_playerController.transform);

    }

    public override void CancelPower()
    {
        base.CancelPower();

        Debug.Log(this.name);

        _playerController.CanReceiveMovementInputs = true;

        if(_currentGrowingPlant != null )
        {
            _currentGrowingPlant.DestroyGrowingPlant();    
        }
    }

}
