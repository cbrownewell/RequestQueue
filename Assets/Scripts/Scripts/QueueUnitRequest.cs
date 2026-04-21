public class QueueUnitRequest : QueueRequest
{
    UnitRequest data;

    public QueueUnitRequest(UnitRequest requestData) : base(requestData)
    {
        data = requestData;
    }

    public delegate void UnitCompleteDelegate(UnitRequest unitData);
    public event UnitCompleteDelegate CompleteUnit;

    public override void Complete()
    {
        CompleteUnit?.Invoke(data);
    }
}
