using Imobiliaria.Domain.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Imobiliaria.Domain
{
    public class Image : BaseImob
    {
        public String Url { get; set; }

        public int IdPredio { get; set; }

        public virtual Predio Predio { get; set; }

        
    }
}
