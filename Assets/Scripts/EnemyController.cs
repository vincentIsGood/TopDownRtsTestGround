using System;
using UnityEngine;

[RequireComponent(typeof(CommanderBehaviorTree), typeof(GamePlayer))]
public class EnemyController: MonoBehaviour{
    public static EnemyController instance;

    [NonSerialized] public GamePlayer ai;
    private CommanderBehaviorTree commanderAi;

    void Awake(){
        if(instance == null) instance = this;

        ai = GetComponent<GamePlayer>();
        commanderAi = GetComponent<CommanderBehaviorTree>();
        commanderAi.config = ai.commanderConfig;
    }
}