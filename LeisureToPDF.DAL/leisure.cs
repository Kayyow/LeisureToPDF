//------------------------------------------------------------------------------
// <auto-generated>
//    Ce code a été généré à partir d'un modèle.
//
//    Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//    Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LeisureToPDF.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class leisure
    {
        public leisure()
        {
            this.evaluation = new HashSet<evaluation>();
        }
    
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string website { get; set; }
        public int id_address { get; set; }
        public int id_category { get; set; }
    
        public virtual address address { get; set; }
        public virtual category category { get; set; }
        public virtual ICollection<evaluation> evaluation { get; set; }
    }
}
