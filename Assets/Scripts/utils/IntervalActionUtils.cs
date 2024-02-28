using System;
using UnityEngine;

public delegate void VoidAction();

public class IntervalActionUtils{
    private VoidAction action;
    private float intervalSec;

    private float startTimeSec = -1;
    private bool started = true;

    public IntervalActionUtils(VoidAction action, float intervalSec){
        this.action = action;
        this.intervalSec = intervalSec;
    }
    public IntervalActionUtils(VoidAction action, float intervalSec, bool started){
        this.action = action;
        this.intervalSec = intervalSec;
        this.started = started;
    }

    public void start(){
        startTimeSec = Time.time;
        started = true;
    }

    public void tick(){
        if(!started) return;
        if(Time.time - startTimeSec > intervalSec){
            action.Invoke();
            startTimeSec = Time.time;
        }
    }

    public void stop(){
        started = false;
    }

    public bool done(){
        return Time.time - startTimeSec > intervalSec;
    }
}