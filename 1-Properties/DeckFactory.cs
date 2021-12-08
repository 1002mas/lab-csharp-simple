namespace Properties
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A factory class for building <see cref="ISet{T}">decks</see> of <see cref="Card"/>s.
    /// </summary>
    public class DeckFactory
    {
        private string[] _seeds;

        private string[] _names;

        public IList<string> GetSeeds() => _seeds.ToList();
        
        public void SetSeeds(IList<string> seeds) => _seeds = seeds.ToArray();
        
        public IList<string> GetNames() => _names.ToList();

        public void SetNames(IList<string> names) => _names = names.ToArray();

        public int GetDeckSize() => _names.Length * _seeds.Length;

        public ISet<Card> GetDeck()
        {
            if (_names == null || _seeds == null)
            {
                throw new InvalidOperationException();
            }

            return new HashSet<Card>(Enumerable
                .Range(0, _names.Length)
                .SelectMany(i => Enumerable
                    .Repeat(i, _seeds.Length)
                    .Zip(
                        Enumerable.Range(0, _seeds.Length),
                        (n, s) => Tuple.Create(_names[n], _seeds[s], n)))
                .Select(tuple => new Card(tuple))
                .ToList());
        }
    }
}
