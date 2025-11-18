using Gold_Five.Domain.Catalog;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace gold_five.Domain.Tests;

[TestClass]
public sealed class Test1
{
    [TestMethod]
    public void Can_Create_New_Rating()
    {
        var rating = new Rating(1, "Make", "Great fit!");

        Assert.AreEqual(1, rating.Stars);
        Assert.AreEqual("Make", rating.UserName);
        Assert.AreEqual("Great fit!", rating.Review);
    }
}
