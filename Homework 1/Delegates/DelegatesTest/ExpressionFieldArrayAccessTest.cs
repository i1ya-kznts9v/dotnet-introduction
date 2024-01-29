using System;
using Delegates;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DelegatesTest;

[TestClass]
public class ExpressionFieldArrayAccessTest
{
    private class ATest
    {
        public BTest?[] One = {new(), null};
    }

    private class BTest
    {
        public string?[] Two = {"Dima", "Roma", null, "Egor"};
    }

    [TestMethod]
    public void TestCorrectResult()
    {
        string query = "One[0].Two[1]";
        string expectedResult = "Roma";

        ATest objectATest = new ATest();
        var func = ExpressionFieldArrayAccess.FieldArrayAccess<ATest>(query);

        var actualResult = func.Invoke(objectATest);

        Assert.AreEqual(expectedResult, actualResult);
    }

    [TestMethod]
    public void TestAnotherCorrectResult()
    {
        string query = "One[0]";

        ATest objectATest = new ATest();
        BTest expectedResult = objectATest.One[0]!;

        var func = ExpressionFieldArrayAccess.FieldArrayAccess<ATest>(query);

        var actualResult = func.Invoke(objectATest);

        Assert.AreEqual(expectedResult, actualResult);
    }

    [TestMethod]
    public void TestNullResult()
    {
        string query = "One[1].Two[2]";
        string expectedResult = null;

        ATest objectTest = new ATest();
        var func = ExpressionFieldArrayAccess.FieldArrayAccess<ATest>(query);

        var actualResult = func.Invoke(objectTest);

        Assert.AreEqual(expectedResult, actualResult);
    }

    [TestMethod]
    public void TestNoFieldResult()
    {
        string query = "Two[0].Two[1]";

        ATest objectTest = new ATest();

        Assert.ThrowsException<FieldAccessException>(() =>
            ExpressionFieldArrayAccess.FieldArrayAccess<ATest>(query).Invoke(objectTest));
    }

    private class CTest
    {
        private string?[] One = {"Dima", "Roma", null, "Egor"};
    }

    [TestMethod]
    public void TestPrivateFieldResult()
    {
        string query = "One[3]";

        CTest objectTest = new CTest();

        Assert.ThrowsException<FieldAccessException>(() =>
            ExpressionFieldArrayAccess.FieldArrayAccess<CTest>(query).Invoke(objectTest));
    }
}