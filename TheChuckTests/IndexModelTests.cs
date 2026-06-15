using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Logging.Abstractions;
using TheChuckTests.Fakes;
using TheChuck.Core;

namespace TheChuck.Pages.Tests
{
    [TestClass()]
    public class IndexModelTests
    {
        [TestMethod()]
        public async Task OnGet_ShouldDisplayTextFromService()
        {
            //Arrange
            var joke = new Joke() { Value = "Works" };
            var sut = new IndexModel(NullLogger<IndexModel>.Instance, new JokeServiceFake(joke));

            //Act
            await sut.OnGet();

            //Assert
            Assert.AreEqual("Works".ToUpper(), sut.DisplayText.ToUpper());
        }

        [TestMethod()]
        public async Task OnGet_ShouldDisplayTextTryAgainWhenApiIsNotWorking()
        {
            //Arrange
            var sut = new IndexModel(NullLogger<IndexModel>.Instance, new JokeServiceBrokenFake());

            //Act
            await sut.OnGet();

            //Assert
            Assert.AreEqual("Något gick fel. Försök igen lite senare.".ToUpper(), sut.DisplayText.ToUpper());
        }


        [TestMethod()]
        public async Task OnGet_ShouldBeUppecase()
        {
            //Arrange
            var joke = new Joke() { Value = "Works" };
            var pageModel = new IndexModel(NullLogger<IndexModel>.Instance, new JokeServiceFake(joke));

            //Act
            await pageModel.OnGet();

            //Assert
            Assert.AreEqual("WORKS", pageModel.DisplayText);
        }


        [TestMethod()]
        public async Task OnGetJokeFromCategory_ShouldFetchCategoricalTextFromService()
        {
            //Arrange
            var joke = new Joke() { Value = "Works" };
            var sut = new IndexModel(NullLogger<IndexModel>.Instance, new JokeServiceFake(joke)) { Category = "Violence" };

            //Act
            await sut.OnGet();

            //Assert
            Assert.AreEqual("Violence".ToUpper(), sut.DisplayText.ToUpper());
        }

        [DataRow("Billy Herrington")]
        [DataRow("Chuck E. Cheese")]
        [DataTestMethod]
        public async Task OnGet_ShouldReplaceChuckNorrisWithQueryParam(string name)
        {
            //Arrange
            var joke = new Joke() { Value = "Chuck Norris joke right here!" };
            var sut = new IndexModel(NullLogger<IndexModel>.Instance, new JokeServiceFake(joke)) { Who = name };

            //Act
            await sut.OnGet();

            //Assert
            Assert.IsTrue(sut.DisplayText.Contains(name.ToUpper()));
            Assert.IsFalse(sut.DisplayText.Contains("CHUCK NORRIS"));
        }

        [TestMethod()]
        public async Task OnGetShouldCountWordsInJoke()
        {
            //Arrange
            var joke = new Joke() { Value = "word1 word2 word3 word4 word5" };
            var sut = new IndexModel(NullLogger<IndexModel>.Instance, new JokeServiceFake(joke));

            //Act
            await sut.OnGet();

            //Assert
            Assert.AreEqual("5 words in this joke.", sut.WordCount);
        }

    }
}