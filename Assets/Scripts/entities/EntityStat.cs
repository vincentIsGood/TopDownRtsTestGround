using System;
using UnityEngine;

[Serializable]
public class EntityStat{
    public float health;
    public float maxHealth;
    public float damage;

    private HpBar hpBar;

    public void setHpBar(HpBar hpBar){
        this.hpBar = hpBar;
    }

    public float calculateDamage(){
        return damage;
    }

    public void takeDamage(float damage){
        health = Mathf.Max(health - damage, 0);
        updateUI();
    }

    public void heal(float amount){
        health = Mathf.Min(health + damage, maxHealth);
        updateUI();
    }

    private void updateUI(){
        hpBar.setHp(health / maxHealth);
    }
}