using System.Collections.Generic;
using SWOF.Api.Models;

namespace SWOF.Api.Repositories
{
public interface IEngineerRepository
{
IEnumerable<Engineer> GetAllActive();
}
}
