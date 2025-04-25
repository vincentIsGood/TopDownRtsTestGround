using UnityEngine;

public class GameStartOption: MonoBehaviour{
    public static GameStartOption instance;
    void Awake(){if(instance == null)instance = this;}

    public GameCountries selectedCountry = GameCountries.German;
}