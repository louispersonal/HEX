using System;

public interface IDatabaseItem<TKey> where TKey : IEquatable<TKey>
{
    TKey Id { get; }
}