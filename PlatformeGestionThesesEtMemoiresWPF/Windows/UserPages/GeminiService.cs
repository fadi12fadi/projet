using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace PlatformeGestionThesesEtMemoiresWPF.Windows.UserPages
{
    public class GeminiService
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;
        private readonly string _geminiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent";

        public GeminiService(string apiKey)
        {
            _apiKey = apiKey;
            _httpClient = new HttpClient();
        }

        // Method to extract text from the first page of a PDF
        public string ExtractTextFromFirstPage(string pdfPath)
        {
            if (string.IsNullOrEmpty(pdfPath) || !File.Exists(pdfPath))
            {
                throw new FileNotFoundException("PDF file not found or path is empty.", pdfPath);
            }

            try
            {
                using (PdfReader reader = new PdfReader(pdfPath))
                {
                    if (reader.NumberOfPages > 0)
                    {
                        // Extract text from the first page only
                        return PdfTextExtractor.GetTextFromPage(reader, 1);
                    }
                    else
                    {
                        throw new Exception("The PDF document has no pages.");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error extracting text from PDF: {ex.Message}", ex);
            }
        }

        // Method to analyze the text using Gemini API
        public async Task<ThesisInfo> AnalyzeTextWithGeminiAsync(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentException("Text to analyze cannot be empty.");
            }

            try
            {
                string prompt = $@"
                You are a thesis information extraction assistant. Extract the following information from the text of a thesis cover page below. 
                The text is from the first page of a thesis PDF. 
                
                For each field, if you can find the information, provide it. If not, leave it blank.
                
                Text to analyze:
                {text}
                
                Please extract and return the following information in a JSON format:
                - Title (the main title of the thesis)
                - Subtitle (if any)
                - Author's first name
                - Author's last name
                - Author's email (if available)
                - University name
                - Faculty name
                - Domain or field of study
                - Professor's first name (supervisor/advisor)
                - Professor's last name (supervisor/advisor)
                - Publication date
                - Keywords (if available, as a comma-separated list)
                - Description or abstract (a short extract if available)
                
                Format your response ONLY as a valid JSON object with these fields.";

                var requestData = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new[]
                            {
                                new
                                {
                                    text = prompt
                                }
                            }
                        }
                    }
                };

                string jsonRequest = JsonSerializer.Serialize(requestData);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                // Add API key as a query parameter
                string urlWithApiKey = $"{_geminiUrl}?key={_apiKey}";
                HttpResponseMessage response = await _httpClient.PostAsync(urlWithApiKey, content);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    
                    // Parse the Gemini API response
                    JsonDocument doc = JsonSerializer.Deserialize<JsonDocument>(responseContent);
                    string extractedText = doc.RootElement
                        .GetProperty("candidates")[0]
                        .GetProperty("content")
                        .GetProperty("parts")[0]
                        .GetProperty("text")
                        .GetString();

                    // Extract the JSON part from the response
                    int startIndex = extractedText.IndexOf('{');
                    int endIndex = extractedText.LastIndexOf('}');
                    
                    if (startIndex >= 0 && endIndex > startIndex)
                    {
                        string jsonText = extractedText.Substring(startIndex, endIndex - startIndex + 1);
                        return JsonSerializer.Deserialize<ThesisInfo>(jsonText);
                    }
                    else
                    {
                        throw new Exception("Could not extract valid JSON from Gemini response.");
                    }
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Gemini API request failed: {response.StatusCode}, {errorContent}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error analyzing text with Gemini: {ex.Message}", ex);
            }
        }
    }

    // Class to hold the thesis information extracted by Gemini
    public class ThesisInfo
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string AuthorFirstName { get; set; }
        public string AuthorLastName { get; set; }
        public string AuthorEmail { get; set; }
        public string University { get; set; }
        public string Faculty { get; set; }
        public string Domain { get; set; }
        public string ProfessorFirstName { get; set; }
        public string ProfessorLastName { get; set; }
        public string PublicationDate { get; set; }
        public string Keywords { get; set; }
        public string Description { get; set; }
    }
} 