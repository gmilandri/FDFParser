using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace FdfParser
{
    public class FDFParser : IFDFParser
    {

        private readonly string _filePath;

        public FDFParser(string filePath) { _filePath = filePath; }

        public string ParseComments()
        {
            ///Gets the content of the file.
            var fdfContent = File.ReadAllText(_filePath);

            ///Gets an unfiltered list of comments.
            var commentContent = GetComments(fdfContent);

            ///Divides the comments by page.
            var commentsByPage = GetCommentsByPage(commentContent);

            ///Returns them to a single ordered list.
            List<string> finalOrder = GetFinalOrderedList(commentsByPage);

            ///Finally returns the refactored comments.
            return ElaboratedComments(finalOrder);

        }

        private List<string> GetComments(string fileContent)
        {
            var answer = new List<string>();

            var entries = fileContent.Split("Contents(");

            foreach (var entry in entries)
            {
                if (entry == entries[0])
                    continue;
                var comment = entry.Split(")");
                answer.Add(comment[0]);
            }

            return answer;
        }

        private List<List<string>> GetCommentsByPage(List<string> comments)
        {

            var refactoredComments = new List<List<string>>();

            foreach (var comment in comments)
            {
                if (Comment.GetCommentType(comment) == CommentType.Page)
                    refactoredComments.Add(new List<string>());

                refactoredComments[refactoredComments.Count - 1].Add(comment);

                if (Comment.GetCommentType(comment) == CommentType.Error)
                    Console.WriteLine("I found an unusual entry at page {0}. Check the following entry in the converted file: ERROR: {1}", refactoredComments.Count.ToString(), comment);
            }

            return refactoredComments;
        }

        private List<string> OrderComments(List<string> comments)
        {
            var answer = new List<string> { comments[0] };

            var otherContent = new List<Comment>();

            for (int i = 1; i < comments.Count; i++)
            {
                otherContent.Add(new Comment(comments[i]));
            }

            otherContent = otherContent.OrderBy(c => c.OrderingNumber).ToList();

            foreach (var comment in otherContent)
            {
                answer.Add(comment.ToString());
            }

            return answer;
        }

        private List<string> GetFinalOrderedList (List<List<string>> commentsByPage)
        {
            var finalOrderedList = new List<string>();

            foreach (var page in commentsByPage)
            {
                {
                    foreach (var comment in OrderComments(page))
                    {
                        finalOrderedList.Add(comment);
                    }
                }
            }

            return finalOrderedList;
        }

        private string ElaboratedComments (List<string> finalOrder)
        {
            StringBuilder comments = new StringBuilder();

            for (int i = 0; i < finalOrder.Count; i++)
            {
                var comment = finalOrder[i];
                switch (Comment.GetCommentType(comment))
                {
                    case CommentType.Page:
                        if (i != 0)
                            comments.Append("\n\n");
                        comments.Append("pag. " + comment);
                        comments.Append("\n");
                        break;
                    case CommentType.Number:
                    case CommentType.NumberLetter:
                        comments.Append(comment + ")");
                        comments.Append("\n\n");
                        break;
                    case CommentType.Letter:
                    case CommentType.LetterNumber:
                        comments.Append(comment + ".");
                        comments.Append("\n\n");
                        break;
                    case CommentType.Error:
                        comments.Append("ERROR: " + comment);
                        comments.Append("\n\n");
                        break;
                }
            }

            return comments.ToString();
        }

    }

}
