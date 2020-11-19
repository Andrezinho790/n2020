using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HalthBar : MonoBehaviour
{
	public Slider slider;
	public Gradient gradient;
	public Image fill;

	private GameManager gameManager;


    private void Start()
    {
		gameManager = FindObjectOfType<GameManager>();
    }
    private void Update()
    {
        if(gameManager.gamePhase == 0)
        {
			transform.GetChild(0).gameObject.SetActive(false);
			transform.GetChild(1).gameObject.SetActive(false);
		}
		else if(gameManager.gamePhase == 1)
        {
			transform.GetChild(0).gameObject.SetActive(true);
			transform.GetChild(1).gameObject.SetActive(true);
		}
    }
    public void SetMaxHealth(int health)
	{
		slider.maxValue = health;
		slider.value = health;

		fill.color = gradient.Evaluate(1f);
	}

	public void SetHealth(int health)
	{
		slider.value = health;

		fill.color = gradient.Evaluate(slider.normalizedValue);
	}
}
