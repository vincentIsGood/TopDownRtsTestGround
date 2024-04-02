using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[DefaultExecutionOrder(0)]
[RequireComponent(typeof(GamePlayer))]
public class RtsController: MonoBehaviour{
    public static RtsController instance;

    [Header("Player")]
    public GamePlayer player;

    [Header("Debug")]
    public List<Squad> selectedSquads = new List<Squad>();

    private CommanderBehaviorTree commanderAi;
    private GameObject selectionBox;
    private UnitsSelector unitsSelector;

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

    private bool clicked = false;
    private Vector3 clickAnchorPos;
    public void leftClick(){
        if(Input.GetMouseButton(0)){
            clearSelectedSquadsAndPos();

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
            selectedSquads = unitsSelector.getSelectedSquads();
            player.formationSolver.clearLocalPos();
            player.formationSolver.updateMembersLocalPos(selectedSquads);
            
            selectionBox.SetActive(false);
        }
    }

    public void clearSelectedSquadsAndPos(){
        for(int i = selectedSquads.Count-1; i >= 0; i--){
            Squad squad = selectedSquads[i];
            squad.config.allowAiMovement = true;
            if(squad.getUnits().Count == 0){
                selectedSquads.RemoveAt(i);
            }
        }
        player.formationSolver.clearLocalPos();
        player.formationSolver.updateMembersLocalPos(selectedSquads);
    }
    public void disableAiMovementOfSelectedSquads(){
        foreach(Squad s in selectedSquads)
            s.config.allowAiMovement = false;
    }
}