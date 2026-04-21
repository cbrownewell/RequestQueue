using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RequestProcessor: MonoBehaviour
{
    Queue<QueueRequest> requestQueue = new();
    QueueRequest activeRequest;
    [SerializeField]
    private int QueueSize = 4;

    /// <summary>
    /// Current amount of requests in queue.
    /// </summary>
    public int QueueCount => requestQueue.Count; 

    private float processTimeMax;
    private float processTimeCurrent;
    public float TimeRemaining { get { return processTimeMax - processTimeCurrent; } }
    public bool IsProcessing { get { return TimeRemaining <= 0; } }

    /// <summary>
    /// Triggers whenever a new request is added to the queue.
    /// </summary>
    /// <param name="activeRequest">The newly added request</param>
    /// <param name="position">the request's position in the queue</param>
    public delegate void NewRequest(QueueRequest activeRequest);
    public static event NewRequest NewRequestAdded;

    /// <summary>
    /// Triggers whenever a request is removed from the queue
    /// </summary>
    /// <param name="request">The canceled request</param>
    public delegate void CancelRequest(QueueRequest request);
    public static event CancelRequest CancelExistingRequest;

    /// <summary>
    /// Triggers every frame to
    /// </summary>
    /// <param name="timeleft"></param>
    public delegate void ProcessingTime(float timeleft);
    public static event ProcessingTime ProcessingTimeLeft;

    /// <summary>
    /// Triggers when the queue moves due to a request being completed
    /// </summary>
    public delegate void QueueStatus(QueueRequest request, int position);
    public static event QueueStatus QueueChange;

    /// <summary>
    /// Cancels a Request at specified location.
    /// </summary>
    /// <remarks>
    ///  0 targets active request
    /// </remarks>
    /// <param name="location"></param>
    public void CancelQueuedRequest(int location)
    {
        if (requestQueue.Count is 0)
            return;

        QueueRequest refund;
        if (location is 0)
        {
            refund = requestQueue.Dequeue();
            activeRequest = null;
        }
        else
        {
            var requestList = requestQueue.ToList();
            refund = requestList[location];
            requestList.RemoveAt(location);
            requestQueue = new Queue<QueueRequest>(requestList);
        }
        CancelExistingRequest?.Invoke(refund);
        SendIconUpdates();
    }

    /// <summary>
    /// Receives request and adds to the queue
    /// </summary>
    /// <param name="request"></param>
    public bool ReceiveRequest(QueueRequest request) 
    {
        var futureSize = requestQueue.Count() + 1;
        if (futureSize <= QueueSize) 
        {
            requestQueue.Enqueue(request);
            NewRequestAdded?.Invoke(request);
            SendIconUpdates();
            return true;
        }
        else
            Debug.Log($"Queue is full for {gameObject.name}");
        return false;
    }

    /// <summary>
    /// Updates all icons
    /// </summary>
    private void SendIconUpdates()
    {
        var requestList = requestQueue.ToList();
        for (int i = 0; i < QueueSize; i++)
        {
            if (i < requestQueue.Count)//Queue items are filled
            {
                SendIconUpdate(requestList[i], i);
            }
            else//Send blanks for the rest
            {
                SendIconUpdate(null,i);
            }
        }
    }

    private void SendIconUpdate(QueueRequest request, int position)
        => QueueChange?.Invoke(request, position);

    private void ProcessRequests()
    {
        //If queue is empty skip
        if (requestQueue.Count is 0)
        {
            //Debug.Log("Request Queue Empty");
            return;
        }

        //Get new request
        if (activeRequest is null)
        {
            //Debug.Log("Getting new Request");
            activeRequest = requestQueue.Peek();
            processTimeMax = activeRequest.requestTimeSpan;
            processTimeCurrent = 0;
            
        }

        //If the request is complete, execute, then release from queue
        if (processTimeCurrent >= processTimeMax)
        {
            //Debug.Log("Request Done");
            requestQueue.Dequeue();
            activeRequest.Complete();
            activeRequest = null;
            SendIconUpdates();
            return;
        }

        //Otherwise continue processing
        processTimeCurrent += Time.deltaTime;
        ProcessingTimeLeft?.Invoke(processTimeCurrent / processTimeMax);
        //Debug.Log("Processing");
    }

    private void Update()
    {
        ProcessRequests();
    }
}
