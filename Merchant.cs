using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Merchant : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _healthPotionCost;
    [SerializeField] private TextMeshProUGUI _lightPotionCost;
    [SerializeField] private TextMeshProUGUI _deathPotionCost;
    [SerializeField] private GameObject _shopUI;
    [SerializeField] private Sprite _swordSprite;
    private int _purchaseValue = 12;
    private Player _player;

    private void Start()
    {
        _player = FindObjectOfType<Player>();

        if (SceneManager.GetSceneByBuildIndex(2).isLoaded)
        {
            _healthPotionCost.text = _purchaseValue.ToString();
            _lightPotionCost.text = _purchaseValue.ToString();
            _deathPotionCost.text = 15.ToString();
        }
        else
        {
            _healthPotionCost.text = _purchaseValue.ToString();
            GameObject.Find("Light_Potion").GetComponent<Image>().sprite = _swordSprite;
            GameObject.Find("LightPotion_Text").GetComponent<TextMeshProUGUI>().text = "Советую взять этот меч. Ваш урон увеличится вдвое!";
            _lightPotionCost.text = 30.ToString();
            GameObject.Find("DP").gameObject.SetActive(false);
            GameObject.Find("Button_Death_Potion").gameObject.SetActive(false);
        }
        _shopUI.SetActive(false);
    }

    public void BuyLightPotion()
    {
        if (_player.Money >= _purchaseValue && SceneManager.GetSceneByBuildIndex(2).isLoaded)
        {
            _player.GiveMoneyToTheMerchant(_purchaseValue);
            GameObject.Find("Light_Potion_Effect").GetComponent<Light2D>().intensity = 0.52f;
            GameObject.Find("Button_Light_Potion").gameObject.SetActive(false);
        }
        else if (_player.Money >= _purchaseValue && SceneManager.GetSceneByBuildIndex(4).isLoaded)
        {
            _player.GiveMoneyToTheMerchant(30);
            FindObjectOfType<PlayerData>().GetDamage(50);
            GameObject.Find("Button_Light_Potion").gameObject.SetActive(false);
        }
        else FindObjectOfType<PlayerUI>().BlinkIfNotEnoughOfMoney();
    }

    public void BuyDeathPotion()
    {
        if (_player.Money >= 15)
        {
            _player.GiveMoneyToTheMerchant(15);
            _player.PlayerTakeDamage(100);
        }
        else FindObjectOfType<PlayerUI>().BlinkIfNotEnoughOfMoney();
    }

    public void BuyHealthPotion()
    {
        if (_player.Money >= _purchaseValue && _player.Health < 100)
        {
            _player.AddHealth();
            _player.GiveMoneyToTheMerchant(_purchaseValue);
        }

        else FindObjectOfType<PlayerUI>().BlinkIfHpIsFull();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _shopUI.SetActive(true);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        _shopUI.SetActive(false);
    }
}
