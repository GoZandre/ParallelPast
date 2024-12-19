using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using TMPro;
using UnityEngine.Events;

public class PremiumMenu : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI _priceText;

    [SerializeField]
    private UnityEvent _onBuyGame = new UnityEvent();

    private void Start()
    {
        _onBuyGame.RemoveAllListeners();
    }

    public void OnConfirmPurchase()
    {
        GameManager.Instance.userDataManager.BuyGame();
        _onBuyGame.Invoke();
        _onBuyGame.RemoveAllListeners();

        //GetComponent<Animator>().Play("PremiumMenu_FadeOut");
    }

    public void OnFailedPurchase()
    {

    }

    public void OnUpdateButton(Product product)
    {
        if (!product.hasReceipt)
        {
            string price = product.metadata.localizedPriceString.ToString();

            _priceText.text = price;
        }
        else
        {
            gameObject.SetActive(false);
        }

    }

    public void DisableMenu()
    {
        gameObject.SetActive(false);
    }
}
