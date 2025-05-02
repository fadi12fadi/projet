using System;
using System.Collections.Generic;
class These{
    public string Title { get; set; }
    public string Subtitle { get; set; }
    public string Description { get; set; }
    public DateTime PublicationDate { get; set; }
    public string PdfFile { get; set; }
    public string PictureFile { get; set; }
    public string Etat { get; set; }
    public List<string> Keywords { get; set; }
    public string University { get; set; }
    public List<string> Domaines { get; set; }
    public List<string> Faculties { get; set; }
    public int AnneePublication { get; set; }
    public List<Author> Authors { get; set; }
    public string Prof { get; set; }
    public string ProfEmail { get; set; }
    public int ProfAnneeEnseignement { get; set; }
    public long ID { get ; set; }

    public These(
        string title, string subtitle, string description, DateTime publicationDate,
        string pdfFile, string pictureFile, string etat, List<string> keywords,
        string university, List<string> domaines, List<string> faculties,
        int anneePublication, List<Author> authors, string prof,
        string profEmail, int profAnneeEnseignement, long id)
    {
        Title = title;
        Subtitle = subtitle;
        Description = description;
        PublicationDate = publicationDate;
        PdfFile = pdfFile;
        PictureFile = pictureFile;
        Etat = etat;
        Keywords = keywords ?? new List<string>();
        University = university;
        Domaines = domaines ?? new List<string>();
        Faculties = faculties ?? new List<string>();
        AnneePublication = anneePublication;
        Authors = authors ?? new List<Author>();
        Prof = prof;
        ProfEmail = profEmail;
        ProfAnneeEnseignement = profAnneeEnseignement;
        ID = id;
    }
    
}