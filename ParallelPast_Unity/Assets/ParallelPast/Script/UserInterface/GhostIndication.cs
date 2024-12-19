using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GhostIndication : MonoBehaviour
{

    public Color UsedGhostColor;

    private List<Image> _ghostImages = new List<Image>();

    [SerializeField]
    private GameObject _ghostImagePrefab;
    [SerializeField]
    private Sprite _usedGhostSprite;

    private int _ghostCount;

    private int _ghostIndex;

    public void SetGhostCount(int ghostCount)
    {
        _ghostCount = ghostCount;

        for (int i = 0; i < ghostCount; i++)
        {
            GameObject ghostImage = Instantiate(_ghostImagePrefab, transform);
            ghostImage.transform.SetParent(transform, false);

            _ghostImages.Add(ghostImage.GetComponent<Image>());

            
        }

        _ghostIndex = _ghostCount - 1;
    }


    public void UseGhost()
    {
        _ghostImages[_ghostIndex].color = UsedGhostColor;
        _ghostImages[_ghostIndex].sprite = _usedGhostSprite;
        _ghostIndex--;
    }
}