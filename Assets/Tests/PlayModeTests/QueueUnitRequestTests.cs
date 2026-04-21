using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;


public class QueueUnitRequestTests
{
    /// <summary>
    /// This test ensures that the wrapper correctly preserves the injected data 
    /// </summary>
    /// <remarks>
    /// This does not currently test preservation of the requestIcon field. Might need to use Resources.Load functionality to facilitate.
    /// </remarks>
    /// <returns></returns>
    [UnityTest]
    public IEnumerator QueueUnitRequest_PreserveData()
    {
        var sampleUnitRequest = ScriptableObject.CreateInstance<UnitRequest>();
        sampleUnitRequest.requestTimeSpan = 1;
        sampleUnitRequest.requestName = "Test";
        sampleUnitRequest.requestCost = 1;
        
        QueueRequest newRequest = new QueueUnitRequest(sampleUnitRequest);

        Assert.AreEqual(sampleUnitRequest.requestName, newRequest.requestName);
        Assert.AreEqual(sampleUnitRequest.requestCost, newRequest.requestCost);
        Assert.AreEqual(sampleUnitRequest.requestTimeSpan, newRequest.requestTimeSpan);
        yield return null;
    }
}
