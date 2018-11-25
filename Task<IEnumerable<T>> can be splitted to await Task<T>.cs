        [Test]
        public async Task Cannot_yield_return_awaited_values_But_you_can_do_this()
        {
            IEnumerable<int> range = await GetAsync();
            Check.That(range).ContainsExactly(Enumerable.Range(1, 100));
        }

        private async Task<IEnumerable<int>> GetAsync()
        {
            // instead of yield return a bunch of awaited values

            //yield return Task.FromResult(1); => Task<IEnumerable<int>> is not an iterate interface

            return await Task.WhenAll(from number in Enumerable.Range(1, 100) select Task.FromResult(number));
        }
