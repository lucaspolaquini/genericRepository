using Imobiliaria.Domain;
using Imobiliaria.Repository.Base;
using Imobiliaria.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Imobiliaria.Repository
{
    public class PredioRepository : RepositoryBase<Predio>, IPredioRepository
    {
        public PredioRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {            
        }
    }
}
