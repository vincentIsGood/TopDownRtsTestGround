using UnityEditor;
using UnityEngine;

// Double click:
// https://forum.unity.com/threads/detect-double-click-on-something-what-is-the-best-way.476759/
public class RtsController: MonoBehaviour{
    public Squad selectedSquads;

    private GameObject selectionBox;

    void Awake(){
        GameObject selectionBoxPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/UI/SelectionBox.prefab");
        selectionBox = GameObject.Instantiate(selectionBoxPrefab);
        selectionBox.SetActive(false);
    }

    void Update(){
        leftClick();
        rightClick();
    }

    private RaycastHit2D[] hitInfos = new RaycastHit2D[1];
    public void rightClick(){
        if(Input.GetMouseButton(1)){
            Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPos.z = 0;
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

    private bool clicked = false;
    private Vector3 clickAnchorPos;
    public void leftClick(){
        if(Input.GetMouseButton(0)){
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            if(!clicked){
                selectionBox.SetActive(true);
                selectionBox.transform.position = mousePos;
                selectionBox.transform.localScale = Vector3.zero;
                clicked = true;
                clickAnchorPos = mousePos;
                return;
            }
            selectionBox.transform.localScale = mousePos - clickAnchorPos;
        }else if(Input.GetMouseButtonUp(0)){
            clicked = false;
            selectionBox.SetActive(false);
        }
    }
}