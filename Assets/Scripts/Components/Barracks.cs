using UnityEngine;

[RequireComponent (typeof(RequestProcessor))]
public class Barracks : MonoBehaviour
{
    [SerializeField]
    private RequestProcessor requestProcessing;

    [SerializeField]
    GameObject unitPrefab;

    public void GetUnitRequest(UnitRequest requestObject)
    {
        var queueRequest = new QueueUnitRequest(requestObject);
        queueRequest.CompleteUnit += SpawnUnit;

        requestProcessing.ReceiveRequest(queueRequest);
    }

    public void CancelUnitRequest(int pos = 0) 
    {
        requestProcessing.CancelQueuedRequest(pos);
    }

    public void SpawnUnit(UnitRequest unitData)
    {
        var unit = Instantiate(unitPrefab);
        unit.GetComponent<SpriteRenderer>().sprite = unitData.requestIcon;

        var cam = Camera.main;

        var x = Random.Range(0f, Screen.width);
        var y = Random.Range(0f, Screen.height);

        var worldPosition = cam.ScreenToWorldPoint(new Vector3(x, y, 1));
        unit.transform.position = worldPosition;
    }

}
