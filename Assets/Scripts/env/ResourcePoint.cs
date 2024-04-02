using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ResourcePoint: MonoBehaviour{
    [Header("Resource")]
    public ResourceStat earningsPerTime;
    public ResourceType type;

    [Header("Capture Config")]
    public float captureRadius = 1.8f;
    public float captureTimeSec = 60;
    private LayerMask playersMask;

    [NonSerialized] public GamePlayer capturer = null;
    [NonSerialized] public GamePlayer capturing = null;
    [NonSerialized] public bool tie;
    private IntervalActionUtils resGenerationCounter;
    private float startCapTimeSec = -1;

    private int hitCount = 0;
    private Collider2D[] results = new Collider2D[5];
    private Dictionary<GamePlayer, int> playersInside = new Dictionary<GamePlayer, int>();
    private HashSet<Squad> squadsInside = new HashSet<Squad>();

    void Awake(){
        playersMask = LayerMask.GetMask("Ally", "Enemy");
        resGenerationCounter = new IntervalActionUtils(giveResourceToCapturer, 10);
    }

    void Update(){
        if(capturer != null)
            resGenerationCounter.tick();

        hitCount = Physics2D.OverlapCircleNonAlloc(transform.position, captureRadius, results, playersMask);
        GamePlayer newCapturer = findLargestPortionOfFaction();
        if(newCapturer == null || tie || capturer == newCapturer){
            startCapTimeSec = -1;
            return;
        }
        if(capturing != newCapturer){
            capturing = newCapturer;
            startCapTimeSec = Time.time;
        }
        if(Time.time - startCapTimeSec > captureTimeSec){
            capturer = newCapturer;
            capturing = null;
            startCapTimeSec = -1;
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmos(){
        Handles.color = getCurrentColor();
        if(startCapTimeSec != -1){
            Color newColor = Color.black;
            if(tie){
                newColor = Color.yellow;
            }else if(capturing != null){
                newColor = capturing.isAlly? Color.blue : Color.red;
            }
            Handles.color = Color.Lerp(Handles.color, newColor, (Time.time - startCapTimeSec) / captureTimeSec);
        }
        Handles.DrawWireDisc(transform.position, Vector3.forward, captureRadius);
    }
    private Color getCurrentColor(){
        Color resultColor = Color.black;
        if(tie){
            resultColor = Color.yellow;
        }else if(capturer != null){
            resultColor = capturer.isAlly? Color.blue : Color.red;
        }
        return resultColor;
    }
#endif

    private GamePlayer findLargestPortionOfFaction(){
        if(hitCount == 0) return null;
        playersInside.Clear();
        squadsInside.Clear();

        Soldier soldier;
        for(int i = 0; i < hitCount; i++){
            if(!results[i].TryGetComponent(out soldier)) continue;
            squadsInside.Add(soldier.getOwnSquad());
        }
        
        GamePlayer result = null;
        int largestAmount = -int.MaxValue;
        foreach(Squad squad in squadsInside){
            GamePlayer player = squad.player;
            int amount = 1;
            if(playersInside.ContainsKey(player))
                amount = playersInside[player]+1; 
            playersInside[player] = amount;

            if(amount > largestAmount){
                largestAmount = amount;
                result = player;
                tie = false;
            }else if(amount == largestAmount){
                largestAmount = amount;
                result = null;
                tie = true;
            }
        }
        return result;
    }

    private void giveResourceToCapturer(){
        if(capturer == null) return;
        capturer.resourceStat.add(earningsPerTime);
    }
}