using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildingSelector{
    private GameBuilding selectedBuilding;

    /**
     * <param name="mousePos">world position</param>
     * <returns>whether it is a hit</returns>
     */
    public bool leftClickOnBuilding(Vector3 mousePos){
        Collider2D[] colliders = Physics2D.OverlapCircleAll(mousePos, 0.1f, LayerMask.GetMask(new string[]{"Ally"}));
        if(colliders.Length == 1 && colliders[0].TryGetComponent(out GameBuilding building)){
            if(building == selectedBuilding) return true;
            if(building is Barracks || building is TankFactory){
                selectedBuilding = building;
                // SpawnerUI.instance.updateUI(selectedBuilding.spawnOptions.Select(o => {
                //     BuildingSpawnOption spawnOption = ScriptableObject.CreateInstance<BuildingSpawnOption>();
                //     spawnOption.init(selectedBuilding, o);
                //     return (CommandOption)spawnOption;
                // }).ToList());
            }
            return true;
        }
        return false;
    }

    public void clearSelection(){
        selectedBuilding = null;
    }
}