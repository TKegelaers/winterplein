using Winterplein.Domain.Entities;

namespace Winterplein.Application.Interfaces;

public interface ISeasonRepository
{
    IReadOnlyList<Season> GetAll();
    Season? GetById(int id);
    Season Add(Season season);
    bool Update(Season season);
    bool Delete(int id);
}
