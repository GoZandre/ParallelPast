using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeOVerTime : MonoBehaviour
{
    [SerializeField]
    private Vector3 _minVector;

    [SerializeField]
    private Vector3 _maxVector;

    [SerializeField]
    private float _duration;

    private Vector3 _newLocation;
    private Vector3 _basePosition;

    [SerializeField]
    private AnimationCurve _animationCurve;

    private void Start()
    {
        _basePosition = transform.position;
        CreateNewLocation();
    }

    IEnumerator Shake()
    {
        float elapsed = 0.0f;

        Vector3 basePosition = transform.position;

        while (elapsed < _duration)
        {

            transform.localPosition = Vector3.Lerp(basePosition, _newLocation, _animationCurve.Evaluate(elapsed / _duration));

            elapsed += Time.deltaTime;

            yield return null;
        }

        CreateNewLocation();
    }

    private void CreateNewLocation()
    {
        float randomX = Random.Range(_minVector.x, _maxVector.x);
        float randomY = Random.Range(_minVector.y, _maxVector.y);

        _newLocation = new Vector3(randomX, randomY,transform.position.z) + _basePosition;

        StartCoroutine(Shake());
    }
}
