using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
public class BarracksTests
{
    GameObject barracksObject;

    [SetUp]
    public void SetUp() 
    {
        barracksObject = new();
        barracksObject.AddComponent<Barracks>();
    }

    [UnityTest]
    public IEnumerator BarracksHasRequiredComponents()
    {
        var hasRequestProcessor = barracksObject.TryGetComponent<RequestProcessor>(out _);

        Assert.IsTrue(hasRequestProcessor);
        yield return null;
    }
}
