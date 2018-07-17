using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SiteNumberParser
{
    /// <summary>
    /// A class that parses the names from the website nemaloknig.com
    /// </summary>
    public class Parser
    {
        /// <summary>
        /// A function that parses the files to the new names
        /// </summary>
        /// <param name="path"></param>
        public static void ParseFiles(string path)
        {
            try
            {
                //A folder with parsed files
                string parsedFilesPath = path + "\\parsed";

                //If the directory doesn't exist, create one
                if (!Directory.Exists(parsedFilesPath) && Directory.Exists(path))
                    Directory.CreateDirectory(parsedFilesPath);

                string[] FileNames = GetFileNames(path);


                //The length of the sentence before the index name
                var lengthOfBeforeSentence = "<!-- Mirrored from nemaloknig.com/read-281449/?page=".Length;

                foreach (var fileName in FileNames)
                {
                    var lines = System.IO.File.ReadAllLines(path + "\\" + fileName, Encoding.UTF8);

                    //A line that contains the page number (in the link)
                    var lineWithNumber = lines[3];

                    //Index of the sentence that is after the site number
                    var indexOfAfterSentence = lineWithNumber.IndexOf
                        (" by HTTrack Website Copier/3.x");

                    //Page number string length = total length - length of before sentence - length of after sentence
                    //2 for f.e. 13, 
                    //1 for f.e. 4 etc.
                    var pageNumberStringLength = indexOfAfterSentence - lengthOfBeforeSentence;

                    //A string with the page number
                    var pageNumberString = lineWithNumber.Substring(lengthOfBeforeSentence, pageNumberStringLength);

                    int pageNumber;
                    Int32.TryParse(pageNumberString, out pageNumber);

                    string pageContent = RemoveHtmlMarks(lines);

                    //Create a new file with the name of the page number and fill it with the content of the page
                    System.IO.File.WriteAllText(parsedFilesPath + "\\" + pageNumber + ".txt", pageContent);
                }
                MessageBox.Show("The files have been parsed succesfully! You can find them in " + parsedFilesPath + "\\");
            }
            catch (Exception e)
            {
                MessageBox.Show("An error occured: " + e.Message);
            }
        }

        /// <summary>
        ///  /// Gets the names of the files and saves them to an array
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>    
        public static string[] GetFileNames(string path)
        {
            string[] htmlFiles = Directory.GetFiles(path, "*.html")
                .Select(Path.GetFileName)
                .ToArray();

            return htmlFiles;
        }

        /// <summary>
        /// A function that extracts the title from the line that contains it
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static string ExtractTheTitle(string line)
        {
            return null;
        }

        /// <summary>
        /// A function that removes from the string all html markups
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static string RemoveHtmlMarks(string[] lines)
        {
            //After "<!--noindex-->" there are always html markups, so +2
            var beginningIndex = lines.ToList().FindIndex(s => s.Contains("<!--noindex-->")) + 2;
            var endIndex = lines.ToList().FindIndex(s => s.Contains("<!--/noindex-->"));

            StringBuilder clearStringBuilder = new StringBuilder();

            //Append all the lines between end and beginning
            for (int i = beginningIndex; i < endIndex; i++)
            {
                clearStringBuilder.Append(lines[i]);
            }

            //Two new lines to distinguish a new chapter
            for (int i = 1; i < 10; i++)
            {
                clearStringBuilder.Replace("<h" + i + " class=\"book\">", "\n\n");
                clearStringBuilder.Replace("</h" + i + ">", "\n\n");
            }

            //Replace <i> and </i> with '
            clearStringBuilder.Replace("</i>", "'");
            clearStringBuilder.Replace("<i>", "'");

            //Remove html markups
            clearStringBuilder.Replace(
                "<div class=\"bookcontent\" id=\"bookcontent\" " +
                "style=\"font-size:14px;font-family:tahoma;line-height:19.6px;\">",
                String.Empty);
            clearStringBuilder.Replace("<p class=\"book\">", String.Empty);
            clearStringBuilder.Replace("</p>", String.Empty);
            clearStringBuilder.Replace("</a>", String.Empty);
            clearStringBuilder.Replace("\t", String.Empty);

            for (int i = 0; i <= 57; i++)
            {
                clearStringBuilder.Replace("<a name=t" + i + ">", String.Empty);
            }

            clearStringBuilder.Replace("<br>", String.Empty);

            return clearStringBuilder.ToString();
        }

        /// <summary>
        /// A function that connects the small pieces into a book.
        /// </summary>
        /// <param name="path">A path to the folder containing the files to concatenate.</param>
        public static void connect(string path, string bookName)
        {
            try
            {
                //Appending every line to the book
                StringBuilder book = new StringBuilder();
                int fileNumber = 1;
                while (System.IO.File.Exists(path + "\\" + fileNumber + ".txt"))
                {
                    var lines = System.IO.File.ReadAllLines(path + "\\" + fileNumber + ".txt", Encoding.UTF8);
                    for (int i = 0; i < lines.Length; i++)
                    {
                        book.AppendLine(lines[i]);
                    }

                    fileNumber++;
                }

                //A directory of the book
                string bookPath = path + "\\book";

                //Saving the book to a file
                if (!Directory.Exists(bookPath) && Directory.Exists(path))
                    Directory.CreateDirectory(bookPath);

                System.IO.File.WriteAllText(bookPath+"\\"+bookName+".txt", book.ToString(0,book.Length),Encoding.UTF8);

                MessageBox.Show("The concatenation was succesfull. You can find the book here: " + bookPath + "\\" + bookName + ".txt");
            }
            catch (Exception e)
            {
                MessageBox.Show("An error occured: " + e.Message);
            }
        }
    }
}
