using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class TheseDAL
    {
        // attributes of a these 
        public int these_id { get; set; }
        public string titre { get; set; }
        public string sousTitre { get; set; }
        public string faculty { get; set; }
        public DateTime datePub { get; set; }
        public byte[] pdfData { get; set; }
        public string pdfPath { get; set; }
        public string description { get; set; }
        public string status { get; set; }
        public string university { get; set; }
        public int prof_id { get; set; }
        public byte[] imageData { get; set; }
        public string imagePath { get; set; }
        public DateTime dateSoutenance { get; set; }
        public string[] keywords { get; set; }
        public string domain { get; set; }
        public AuthorDAL author { get; set; }
        public ProfessorDAL prof { get; set; }

        public TheseDAL()
        {

        }

        // constructor with all the attributes
        public TheseDAL(int these_id, string titre, string sousTitre, string faculty, DateTime datePub, byte[] pdfData, string description, string status, string university, int prof_id, byte[] imageData, string imageName, DateTime dateSoutenance)
        {
            this.these_id = these_id;
            this.titre = titre;
            this.sousTitre = sousTitre;
            this.faculty = faculty;
            this.datePub = datePub;
            this.pdfData = pdfData;
            this.description = description;
            this.status = status;
            this.university = university;
            this.prof_id = prof_id;
            this.imageData = imageData;
            this.imagePath = imageName;
            this.dateSoutenance = dateSoutenance;
        }

        // this constructor is without the id
        public TheseDAL(string titre, string sousTitre, string faculty, string[] keywords, string domain, DateTime datePub,string pdfPath, byte[] pdfData, string description, string status, string university, byte[] imageData, string imagePath, DateTime dateSoutenance)
        {
            this.these_id = these_id;
            this.titre = titre;
            this.sousTitre = sousTitre;
            this.faculty = faculty;
            this.keywords = keywords;
            this.domain = domain;
            this.datePub = datePub;
            this.pdfPath = pdfPath;
            this.pdfData = pdfData;
            this.description = description;
            this.status = status;
            this.university = university;
            this.prof_id = prof_id;
            this.imageData = imageData;
            this.imagePath = imagePath;
            this.dateSoutenance = dateSoutenance;
        }
    }
}
