using System.Collections.Generic;

namespace Relax.Model.Mapper.Interfaces
{
    public interface IMapper<TTO, TFROM>
    {
        ICollection<TTO> Map(ICollection<TFROM> entity);
        TTO Map(TFROM entity);
    }
}