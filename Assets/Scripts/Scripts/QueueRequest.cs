using UnityEngine;

public abstract class QueueRequest
{
    readonly QueueRequestBaseSO baseData;
    public QueueRequest(QueueRequestBaseSO requestData)
    {
        baseData = requestData;
    }

    public float requestTimeSpan => baseData.requestTimeSpan;
    public Sprite requestIcon => baseData.requestIcon;
    public string requestName => baseData.requestName;
    public int requestCost => baseData.requestCost;
    public virtual void Complete() { throw new System.NotImplementedException(); }
}
