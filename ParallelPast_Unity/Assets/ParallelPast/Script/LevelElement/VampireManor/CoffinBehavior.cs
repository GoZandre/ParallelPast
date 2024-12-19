using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffinBehavior : MonoBehaviour
{
    [SerializeField]
    private So_CharacterPower Power;

    public void CoffinInteraction()
    {
        InteractibleElement interactibleElem = GetComponent<InteractibleElement>();

        if( interactibleElem.CurrentActivator != null)
        {
            TransformInVampire(interactibleElem.CurrentActivator);
        }
    }

    public void TransformInVampire(PlayerController playerController)
    {
        playerController.OnCreatePower(Instantiate(Power));
    }
}
