using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    [SerializeField]
    private Transform _playerCharacter;

    public float InterpSpeed;

    public Vector3 CameraOffset;


    private void Update()
    {
        transform.position = Vector3.Slerp(transform.position, new Vector3(_playerCharacter.position.x, _playerCharacter.position.y, transform.position.z) + CameraOffset, Time.deltaTime * InterpSpeed);
    }



}
