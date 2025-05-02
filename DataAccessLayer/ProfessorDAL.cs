using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class ProfessorDAL
    {
        public int prof_id { get; set; }
        public string nom { get; set; }
        public string prenom { get; set; }
        public string fac_name { get; set; }
        public string university { get; set; }
        public int anneeEnseignement { get; set; }
        public int Doctorat_these_id { get; set; }

    }
}
