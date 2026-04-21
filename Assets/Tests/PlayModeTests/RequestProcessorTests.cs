using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class RequestProcessorTests
{
    RequestProcessor processor;

    UnitRequest sampleUnitRequest;

    [SetUp]
    public void SetUp()
    {
        processor = new GameObject().AddComponent<RequestProcessor>();

        sampleUnitRequest = ScriptableObject.CreateInstance<UnitRequest>();
        sampleUnitRequest.requestTimeSpan = 1;
    }

    [UnityTest]
    public IEnumerator ReceiveRequest_AddRequest()
    {
        QueueRequest newRequest = new QueueUnitRequest(sampleUnitRequest);
        processor.ReceiveRequest(newRequest);

        Assert.AreEqual(1, processor.QueueCount);
        yield return null;
    }

    [UnityTest]
    public IEnumerator ReceiveRequest_AddRequestUnderQueueLimit() 
    {
        QueueRequest newRequest = new QueueUnitRequest(sampleUnitRequest);
        var receieved = processor.ReceiveRequest(newRequest);

        Assert.IsTrue(receieved);
        yield return null;
    }

    [UnityTest]
    public IEnumerator ReceiveRequest_AddRequestOverQueueLimit()
    {
        QueueRequest newRequest = new QueueUnitRequest(sampleUnitRequest);
        var queueLength = 4;

        for (int i = 0; i < queueLength; i++)
            processor.ReceiveRequest(newRequest);
        
        var receieved = processor.ReceiveRequest(newRequest);

        Assert.False(receieved);
        yield return null;
    }

    [UnityTest]
    public IEnumerator ProcessRequests_CheckProcessorRemovesCompleted() 
    {
        QueueRequest newRequest = new QueueUnitRequest(sampleUnitRequest);
        processor.ReceiveRequest(newRequest);

        yield return new WaitForSeconds(newRequest.requestTimeSpan);
        Assert.IsTrue(processor.TimeRemaining <= 0);

        yield return new WaitForSeconds(0.1f);
        Assert.IsTrue(processor.QueueCount == 0);
    }

    [UnityTest]
    public IEnumerator ProcessRequests_FillEmptySpaces() 
    {
        QueueRequest newRequest = new QueueUnitRequest(sampleUnitRequest);
        processor.ReceiveRequest(newRequest);
        processor.ReceiveRequest(newRequest);

        yield return new WaitForSeconds(newRequest.requestTimeSpan + 0.1f);

        Assert.AreEqual(1, processor.QueueCount);
        Assert.IsTrue(processor.TimeRemaining > 0);
    }


}
