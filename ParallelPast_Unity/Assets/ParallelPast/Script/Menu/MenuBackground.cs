using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBackground : MonoBehaviour
{
    [SerializeField]
    private GameObject _caveBackground;

    [SerializeField]
    private GameObject _forestBackground;

    GameManager gameManager;

    private void Start()
    {
        List<GameObject> list = new List<GameObject>();
        list.Add(_caveBackground);

        gameManager = GameManager.Instance;
        
        for(int i = 0; i < gameManager.SaveManager.state.CompletedLevel.Count; i++)
        {
            if (gameManager.SaveManager.state.CompletedLevel[i] >= 14)
            {
                list.Add(_forestBackground);
                break;
            }
        }

        list[Random.Range(0, list.Count)].SetActive(true);
    }
}
