using UnityEngine;

public abstract class QueueRequestBaseSO : ScriptableObject
{
    public Sprite requestIcon;

    public float requestTimeSpan;

    public string requestName;

    public int requestCost;

    public virtual void Complete() { throw new System.NotImplementedException(); }
}
