using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
    public Text playerName;
    public Text healthText;
    public Text mpText;
    public Slider healthSlider;
    public Slider mpSlider;
    public Slider limitSlider;

    public void StartHUD(Humanoid humanoid)
    {
        playerName.text = humanoid.charName;

        healthText.text = humanoid.currentHealth + "/ " + humanoid.maxHealth;
        healthSlider.maxValue = humanoid.maxHealth;
        healthSlider.value = humanoid.currentHealth;

        mpText.text = humanoid.currentMP + "/ " + humanoid.maxMP;
        mpSlider.maxValue = humanoid.maxMP;
        mpSlider.value = humanoid.currentMP;

        limitSlider.maxValue = humanoid.maxLimit;
        limitSlider.value = humanoid.currentLimit;
    }

    public void UpdateHP(Humanoid humanoid)
    {
        healthText.text = humanoid.currentHealth + "/ " + humanoid.maxHealth;
        healthSlider.value = humanoid.currentHealth;
    }


}
