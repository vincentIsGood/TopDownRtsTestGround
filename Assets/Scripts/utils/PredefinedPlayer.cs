using UnityEngine;

[DefaultExecutionOrder(1)]
public class PredefinedPlayer: MonoBehaviour{
    public bool player = false;

    void Awake(){
        if(TryGetComponent(out Barracks barracks)){
            if(player)
                barracks.playerOwner();
            else barracks.enemyOwner();
        }
        
        if(TryGetComponent(out TankFactory tankFactory)){
            if(player)
                tankFactory.playerOwner();
            else tankFactory.enemyOwner();
        }
    }
}