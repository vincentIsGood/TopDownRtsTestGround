using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[DefaultExecutionOrder(0)]
public class RtsController: MonoBehaviour{
    public static RtsController instance;
    
    [Header("Player")]
    public GamePlayer player = new GamePlayer("player");

    [Header("Enemy")]
    public GamePlayer enemy = new GamePlayer("enemy"){isAlly = false};
    public CommanderBTData enemyCommanderConfig = new CommanderBTData();

    [Header("Debug")]
    public List<Squad> selectedSquads = new List<Squad>();

    private GameObject selectionBox;
    private UnitsSelector unitsSelector;

    void Awake(){
        if(instance == null) instance = this;
        GameObject selectionBoxPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/UI/SelectionBox.prefab");
        selectionBox = Instantiate(selectionBoxPrefab);
        selectionBox.SetActive(false);
        unitsSelector = selectionBox.GetComponentInChildren<UnitsSelector>();
    }

    void Update(){
        leftClick();
        rightClick();

        DebugMsgPanel.instance.showStats(player.resourceStat, enemy.resourceStat);
    }

    private RaycastHit2D[] hitInfos = new RaycastHit2D[1];
    public void rightClick(){
        if(Input.GetMouseButton(1) && selectedSquads.Count > 0){
            Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPos.z = 0;
            int hitCount = Physics2D.RaycastNonAlloc(targetPos, Vector2.zero, hitInfos);
            if(hitCount > 0){
                if(hitInfos[0].collider.TryGetComponent(out Cover cover)){
                    foreach(Squad squad in selectedSquads){
                        squad.moveToPos(targetPos, cover);
                    }
                }else if(hitInfos[0].collider.TryGetComponent(out Soldier soldier)){
                    foreach(Squad squad in selectedSquads){
                        squad.moveToPos(targetPos, soldier.getOwnSquad());
                    }
                }else if(hitInfos[0].collider.TryGetComponent(out EnterableHouse house)){
                    foreach(Squad squad in selectedSquads){
                        squad.moveToPos(targetPos, house);
                    }
                }else moveToPos(targetPos);
            }else moveToPos(targetPos);
        }
    }
    private void moveToPos(Vector3 pos){
        foreach(Squad squad in selectedSquads){
            squad.moveToPos(pos + player.formationSolver.getNoFormationPos(squad));
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
            selectedSquads = unitsSelector.getSelectedSquads();
            player.formationSolver.clearLocalPos();
            player.formationSolver.updateMembersLocalPos(selectedSquads);
            
            selectionBox.SetActive(false);
        }
    }
}