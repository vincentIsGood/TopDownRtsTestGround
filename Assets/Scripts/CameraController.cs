using UnityEngine;

public class CameraController: MonoBehaviour{
    public float speed = 10f;

    void Update(){
        float horiValue = Input.GetAxis("Horizontal");
        float vertValue = Input.GetAxis("Vertical");
        Vector3 movement = Vector3.zero;
        if(horiValue != 0){
            movement.x += horiValue * Time.deltaTime * speed;
        }
        if(vertValue != 0){
            movement.y += vertValue * Time.deltaTime * speed;
        }
        transform.position = transform.position + movement;
    }
}