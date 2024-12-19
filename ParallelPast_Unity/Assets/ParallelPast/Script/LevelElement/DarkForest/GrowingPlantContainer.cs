using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowingPlantContainer : MonoBehaviour
{

    private GrowingPlants _selfGrowingPlant;

    [SerializeField]
    private So_ObjectHeight _objectHeight;

    private void Awake()
    {
        _selfGrowingPlant = transform.parent.GetComponent<GrowingPlants>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Lumberjack enter condition

        if(collision.gameObject.TryGetComponent<SleepingLumberjack>(out SleepingLumberjack lumberjack))
        {
            if (_selfGrowingPlant.IsGrowing)
            {

                Debug.Log("Lock lumberjack");

                _selfGrowingPlant.AddGrowingPlantMargin(_objectHeight.LumberjackHeight);
                _selfGrowingPlant.OnGrowingComplete.AddListener(lumberjack.RestartMovement);
                lumberjack.StopMovement();
            }        
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Lumberjack enter condition

        if (collision.gameObject.TryGetComponent<SleepingLumberjack>(out SleepingLumberjack lumberjack))
        {
            _selfGrowingPlant.RemoveGrowingPlantMargin(_objectHeight.LumberjackHeight);
        }
    }
}
