using System;
using System.Collections.Generic;
using Delegates;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DelegatesTest;

[TestClass]
public class QueryParserTest
{
    [TestMethod]
    public void TestParseQuery()
    {
        const string query = "One[0].Two[5].Three[10]";
        List<Tuple<string, int>> expectedQuery = new List<Tuple<string, int>>
        {
            new("One", 0),
            new("Two", 5),
            new("Three", 10)
        };

        var actualQuery = QueryParser.ParseQuery(query);

        var actualQueryCount = actualQuery.Count;
        Assert.AreEqual(expectedQuery.Count, actualQueryCount);

        for (int i = 0; i < actualQueryCount; i++)
        {
            Assert.AreEqual(expectedQuery[i].Item1, actualQuery[i].Item1);
            Assert.AreEqual(expectedQuery[i].Item2, actualQuery[i].Item2);
        }
    }
}