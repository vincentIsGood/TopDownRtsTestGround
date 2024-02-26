using UnityEngine;

public class HpBar: MonoBehaviour{
    public GameObject redBar;

    public void setHp(float percentage){
        transform.localScale = new Vector3(percentage, 1, 1);
    }
}