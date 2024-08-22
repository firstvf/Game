using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Gradient _gradientOfSlider;
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private Text _amountOfMoney;
    [SerializeField] private Image _fill;
    private Player _player;
    private Color _defaultMoneyColor;

    private void Start()
    {
        _player = FindObjectOfType<Player>();

        _amountOfMoney.text = _player.Money.ToString();
        _healthSlider.value = _player.Health;

        _defaultMoneyColor = _amountOfMoney.color;       
        _fill.color = _gradientOfSlider.Evaluate(_healthSlider.normalizedValue);
    }

    private IEnumerator BlinkMoneyCooldown(float time)
    {
        yield return new WaitForSeconds(time);
        _amountOfMoney.color = _defaultMoneyColor;
    }
    private IEnumerator BlinkHpCooldown(float time)
    {
        yield return new WaitForSeconds(time);
        GameObject.Find("Fill").GetComponent<Image>().color = Color.green;
    }
    public void BlinkIfHpIsFull()
    {
        GameObject.Find("Fill").GetComponent<Image>().color = Color.red;
        StartCoroutine(BlinkHpCooldown(0.1f));
    }

    public void BlinkIfNotEnoughOfMoney()
    {
        _amountOfMoney.color = Color.red;
        StartCoroutine(BlinkMoneyCooldown(0.1f));

    }

    public void SetMoneyUI(int money)
    {
        _amountOfMoney.text = money.ToString();
    }

    public void MoveSliderIfPlayerGotDamage(int health)
    {
        _healthSlider.value = health;

        _fill.color = _gradientOfSlider.Evaluate(1f);
        _fill.color = _gradientOfSlider.Evaluate(_healthSlider.normalizedValue);
    }
}
