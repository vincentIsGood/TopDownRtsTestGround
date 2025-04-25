using System;
using System.Collections.Generic;

public class UnitTypes{
    public static List<Type> getAllUnitTypes(){
        return new List<Type>(){
            typeof(LightTankUnit), 
            typeof(HeavyTankUnit), 
            typeof(Soldier),
        };
    }
}