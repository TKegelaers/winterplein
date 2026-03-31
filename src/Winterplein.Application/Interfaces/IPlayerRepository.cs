using Winterplein.Domain.Entities;
using Winterplein.Domain.Enums;
using Winterplein.Domain.ValueObjects;

namespace Winterplein.Application.Interfaces;

public interface IPlayerRepository
{
    IReadOnlyList<Player> GetAll();
    int Count { get; }
    Player Add(Name name, Gender gender);
    void Remove(int id);
}
