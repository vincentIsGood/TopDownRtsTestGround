using System;
using UnityEngine;

[Serializable]
public class EntityStat{
    public static EntityStat identity = new EntityStat(){damage = 1, defense = 1};

    public float health;
    public float maxHealth;
    public float damage;
    public float defense;
    public int xpReward = 100;

    private HpBar hpBar;

    public void add(EntityStat entityStat){
        maxHealth += entityStat.maxHealth;
        heal(entityStat.health);
        damage += entityStat.damage;
        defense += entityStat.defense;
    }

    public void sub(EntityStat entityStat){
        maxHealth = Mathf.Max(maxHealth - entityStat.maxHealth, 0);
        health = Mathf.Max(health - entityStat.health, 0);
        damage = Mathf.Max(damage - entityStat.damage, 0);
        defense = Mathf.Max(defense - entityStat.defense, 0);
    }

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