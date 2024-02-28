using UnityEditor;
using UnityEngine;

public class WeaponSuite{
    public static Bullet bulletForward(GameUnit owner, Vector3 forwardDir, float speed = 10){
        GameObject bulletPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Weapons/Bullet.prefab");
        Bullet bullet = GameObject.Instantiate(
            bulletPrefab, 
            owner.getTransform().position, 
            Quaternion.AngleAxis(AiNavigator.findAngle(forwardDir), Vector3.forward)).GetComponent<Bullet>();
        bullet.speed = speed;
        bullet.owner = owner;
        bullet.forward = forwardDir;
        return bullet;
    }
}