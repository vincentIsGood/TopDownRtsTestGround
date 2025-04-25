using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

[DefaultExecutionOrder(0)]
[RequireComponent(typeof(GamePlayer))]
public class RtsController: MonoBehaviour{
    public static RtsController instance;

    [Header("Player")]
    public GamePlayer player;

    [Header("Debug")]
    public List<Squad> selectedSquads = new List<Squad>();
    public Squad uniquelySelectedSquad;

    private CommanderBehaviorTree commanderAi;
    private GameObject selectionBox;
    private UnitsSelector unitsSelector;
    private BuildingSelector buildingSelector = new BuildingSelector();

    void Awake(){
        player = GetComponent<GamePlayer>();
        player.controller = this;

        if(instance == null) instance = this;
        GameObject selectionBoxPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/UI/SelectionBox.prefab");
        selectionBox = Instantiate(selectionBoxPrefab);
        selectionBox.SetActive(false);
        unitsSelector = selectionBox.GetComponentInChildren<UnitsSelector>();

        if(TryGetComponent(out commanderAi)){
            commanderAi.config = player.commanderConfig;
        }
    }

    void Update(){
        leftClick();
        rightClick();

        DebugMsgPanel.instance.showStats(player, EnemyController.instance.ai);
    }

    private RaycastHit2D[] hitInfos = new RaycastHit2D[1];
    public void rightClick(){
        if(selectedSquads.Count == 0) return;
        if(Input.GetMouseButton(1)){
            disableAiMovementOfSelectedSquads();

            Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPos.z = 0;
            int hitCount = Physics2D.RaycastNonAlloc(targetPos, Vector2.zero, hitInfos);
            if(hitCount > 0){
                if(hitInfos[0].collider.TryGetComponent(out Cover cover)){
                    foreach(Squad squad in selectedSquads){
                        squad.moveToPos(targetPos, cover);
                    }
                }else if(hitInfos[0].collider.TryGetComponent(out EnterableHouse house)){
                    foreach(Squad squad in selectedSquads){
                        squad.moveToPos(targetPos, house);
                    }
                }else if(hitInfos[0].collider.TryGetComponent(out GameBuilding building)){
                    moveToPos(targetPos);
                }else if(hitInfos[0].collider.TryGetComponent(out GameUnit unit)){
                    // Debug.Log("Targeting: " + unit.getOwner().name);
                    foreach(Squad squad in selectedSquads){
                        squad.moveToPos(targetPos, unit.getOwnSquad());
                    }
                }else moveToPos(targetPos);
            }else moveToPos(targetPos);
        }
    }
    private void moveToPos(Vector3 pos){
        foreach(Squad squad in selectedSquads){
            squad.resetStoppingDistance();
            squad.moveToPos(pos + player.formationSolver.getNoFormationPos(squad));
        }
    }

    private bool clickDrag = false;
    private Vector3 clickAnchorPos;
    public void leftClick(){
        if(EventSystem.current.IsPointerOverGameObject()){
            if(clickDrag){
                leftClickButtonUp();
            }
            return;
        }

        if(Input.GetMouseButton(0)){
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            if(buildingSelector.leftClickOnBuilding(mousePos)){
                return;
            }else clearSelectedSquadsAndPos();

            if(!clickDrag){
                selectionBox.SetActive(true);
                selectionBox.transform.position = mousePos;
                selectionBox.transform.localScale = Vector3.zero;
                clickDrag = true;
                clickAnchorPos = mousePos;
                return;
            }
            selectionBox.transform.localScale = mousePos - clickAnchorPos;
        }else if(Input.GetMouseButtonUp(0)){
            leftClickButtonUp();
        }
    }
    private void leftClickButtonUp(){
        clickDrag = false;
        selectedSquads = unitsSelector.getSelectedSquads();
        player.formationSolver.clearLocalPos();
        player.formationSolver.updateMembersLocalPos(selectedSquads);

        selectionBox.SetActive(false);
    }

    public void clearSelectedSquadsAndPos(){
        for(int i = selectedSquads.Count-1; i >= 0; i--){
            Squad squad = selectedSquads[i];
            squad.config.allowAiMovement = true;
            if(squad.getUnits().Count == 0){
                selectedSquads.RemoveAt(i);
            }
        }
        buildingSelector.clearSelection();

        player.formationSolver.clearLocalPos();
        player.formationSolver.updateMembersLocalPos(selectedSquads);
    }
    public void disableAiMovementOfSelectedSquads(){
        foreach(Squad s in selectedSquads)
            s.config.allowAiMovement = false;
    }
}
