using TheChuck.Core;

namespace TheChuckTests.Fakes
{
    internal class JokeServiceFake : IJokeService
    {
        private readonly Joke joke;

        public JokeServiceFake(Joke joke)
        {
            this.joke = joke;
        }

        public async Task<Joke?> GetJokeFromCategory(string category)
        {
            joke.Value = category;
            return await Task.FromResult(joke);
        }

        public async Task<Joke?> GetRandomJoke()
        {
            return await Task.FromResult(joke);
        }
    }
}


