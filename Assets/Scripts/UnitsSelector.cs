using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitsSelector: MonoBehaviour{
    public LayerMask allyMask;

    private HashSet<Squad> selectedSquads = new HashSet<Squad>();
    private GameUnit gameUnit;
    
    void OnTriggerEnter2D(Collider2D collider){
        if(collider.TryGetComponent(out gameUnit) && ((gameUnit.getOwner().layer & 1 << allyMask) > 0)){
            selectedSquads.Add(gameUnit.getOwnSquad());
        }
    }
    void OnTriggerExit2D(Collider2D collider){
        if(collider.TryGetComponent(out gameUnit) && ((gameUnit.getOwner().layer & 1 << allyMask) > 0)){
            selectedSquads.Remove(gameUnit.getOwnSquad());
        }
    }

    public List<Squad> getSelectedSquads(){
        List<Squad> result = selectedSquads.ToList();
        selectedSquads.Clear();
        return result;
    }
}