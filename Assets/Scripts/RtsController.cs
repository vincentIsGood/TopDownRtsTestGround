using UnityEngine;

// Double click:
// https://forum.unity.com/threads/detect-double-click-on-something-what-is-the-best-way.476759/
public class RtsController: MonoBehaviour{
    public Squad selectedSquads;

    private RaycastHit2D[] hitInfos = new RaycastHit2D[1];
    void Update(){
        if(Input.GetMouseButtonDown(1)){
            Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int hitCount = Physics2D.RaycastNonAlloc(targetPos, Vector2.zero, hitInfos);
            if(hitCount > 0){
                if(hitInfos[0].collider.TryGetComponent(out Cover cover)){
                    selectedSquads.moveToPos(targetPos, cover);
                }else if(hitInfos[0].collider.TryGetComponent(out Soldier soldier)){
                    selectedSquads.moveToPos(targetPos, soldier.ownSquad);
                }else selectedSquads.moveToPos(targetPos);
            }else selectedSquads.moveToPos(targetPos);
        }
    }
}