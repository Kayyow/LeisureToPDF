using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeisureToPDF.Database
{
    public partial class leisure
    {
        public float averageNote
        {
            get
            {
                List<evaluation> evaluations = this.evaluation.ToList();
                float averageNote = 0;
                foreach (var eval in evaluations)
                {
                    averageNote += eval.note;
                }
                return averageNote / evaluations.Count;
            }
        }
    }
}