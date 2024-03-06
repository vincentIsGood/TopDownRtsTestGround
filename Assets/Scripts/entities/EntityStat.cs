using System;
using UnityEngine;

[Serializable]
public class EntityStat{
    public static EntityStat identity = new EntityStat(){damage = 1, defense = 1};

    public float health;
    public float maxHealth;
    public float damage;
    public float defense;

    private HpBar hpBar;

    public void takeDamage(float damage, EntityStat effectScaler = null){
        if(effectScaler == null) effectScaler = identity;

        float finalDamage = damage * effectScaler.damage - defense * effectScaler.defense;
        health = Mathf.Max(health - finalDamage, 0);
        updateUI();
    }

    public void heal(float amount){
        health = Mathf.Min(health + damage, maxHealth);
        updateUI();
    }

    public void setHpBar(HpBar hpBar){
        this.hpBar = hpBar;
    }

    private void updateUI(){
        hpBar?.setHp(health / maxHealth);
    }
}