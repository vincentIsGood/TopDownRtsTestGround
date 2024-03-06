using System;
using System.Collections.Generic;

[Serializable]
public class GamePlayer{
    private List<Squad> squads = new List<Squad>();
    public string name;
    public ResourceStat resourceStat = new ResourceStat();
    public SquadFormationSolver formationSolver = new SquadFormationSolver();
    public bool isAlly = true;

    public GamePlayer(string name){
        this.name = name;
    }

    public void addSquad(Squad squad){
        squads.Add(squad);
    }
    public void removeSquad(Squad squad){
        squads.Remove(squad);
    }
}