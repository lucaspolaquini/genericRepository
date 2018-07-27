using Imobiliaria.Domain.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Imobiliaria.Domain
{
    public class Predio : BaseImob
    {

        public String Nome { get; set; }

        public String Descricao { get; set; }

        public String Localizacao { get; set; }

        public virtual ICollection<Image>  Images { get; set; }

    }
}
