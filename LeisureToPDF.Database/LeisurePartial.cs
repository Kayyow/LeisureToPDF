using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeisureToPDF.Database
{
    public partial class leisure
    {
        public string averageNote
        {
            get
            {
                List<evaluation> evaluations = this.evaluation.ToList();
                float averageNote = 0;
                foreach (var eval in evaluations)
                {
                    averageNote += eval.note;
                }

                if (averageNote != 0) {
                    return (averageNote / evaluations.Count).ToString("0.#");
                } else {
                    return "0";
                }
                
            }
        }
    }
}