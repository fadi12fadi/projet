using System;
using System.Collections.Generic;
using System.Linq;

public class User
{
    public static List<These> Search(
        List<These> theses,
        string title = null,
        string author = null,
        string keyword = null,
        string domaine = null,
        string university = null,
        int? anneePublication = null)
    {
        var result = theses
            .Where(t =>
                (title == null || (t.Title != null && t.Title.IndexOf(title, StringComparison.OrdinalIgnoreCase) >= 0)) &&
                (author == null || t.Authors.Any(a =>
                    (a.FirstName != null && a.FirstName.IndexOf(author, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (a.LastName != null && a.LastName.IndexOf(author, StringComparison.OrdinalIgnoreCase) >= 0))) &&
                (keyword == null || t.Keywords.Any(k =>
                    k != null && k.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)) &&
                (domaine == null || t.Domaines.Any(d =>
                    d != null && d.IndexOf(domaine, StringComparison.OrdinalIgnoreCase) >= 0)) &&
                (university == null || (t.University != null &&
                    t.University.IndexOf(university, StringComparison.OrdinalIgnoreCase) >= 0)) &&
                (anneePublication == null || t.AnneePublication == anneePublication.Value))
            .ToList();

        return result;
    }

    public static void DisplaySearchResults(List<These> results)
    {
        if (results == null || results.Count == 0)
        {
            Console.WriteLine("No theses found matching the search criteria.");
        }
        else if (results.Count == 1)
        {
            Console.WriteLine("1 thesis found:");
            DisplayThese(results[0]);
        }
        else
        {
            Console.WriteLine($"{results.Count} theses found:");
            foreach (var these in results)
            {
                DisplayThese(these);
                Console.WriteLine();
            }
        }
    }

    private static void DisplayThese(These these)
    {
        Console.WriteLine($"ID: {these.ID}");  // Added ID display
        Console.WriteLine($"Title: {these.Title}");
        Console.WriteLine($"Subtitle: {these.Subtitle}");
        Console.WriteLine($"Description: {these.Description}");
        Console.WriteLine($"Publication Date: {these.PublicationDate.ToShortDateString()}");
        Console.WriteLine($"PDF File: {these.PdfFile}");
        Console.WriteLine($"Picture File: {these.PictureFile}");
        Console.WriteLine($"State: {these.Etat}");
        Console.WriteLine($"Keywords: {string.Join(", ", these.Keywords)}");
        Console.WriteLine($"University: {these.University}");
        Console.WriteLine($"Domaines: {string.Join(", ", these.Domaines)}");
        Console.WriteLine($"Faculties: {string.Join(", ", these.Faculties)}");
        Console.WriteLine($"Publication Year: {these.AnneePublication}");
        Console.WriteLine("Authors:");
        foreach (var author in these.Authors)
        {
            Console.WriteLine($"- {author.FirstName} {author.LastName}");
        }
        Console.WriteLine($"Professor: {these.Prof}");
        Console.WriteLine($"Professor Email: {these.ProfEmail}");
        Console.WriteLine($"Professor Teaching Year: {these.ProfAnneeEnseignement}");
    }
}