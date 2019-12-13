using System;
using System.Collections.Generic;
using System.Linq;

namespace FdfParser
{
    struct Comment
    {
        public readonly int OrderingNumber;
        private readonly string _comment;
        public readonly CommentType CommentType;


        public Comment(string comment)
        {
            _comment = comment;
            CommentType = GetCommentType(_comment);
            switch (CommentType)
            {
                case CommentType.NumberLetter:
                    OrderingNumber = Convert.ToInt32(comment.Substring(0, comment.Length - 1));
                    break;
                case CommentType.Number:
                    OrderingNumber = Convert.ToInt32(comment);
                    break;
                case CommentType.Letter:
                    OrderingNumber = comment[0] * 10;
                    break;
                case CommentType.LetterNumber:
                    OrderingNumber = Convert.ToInt32(comment.Substring(1)) + comment[0] * 10;
                    break;
                default:
                    OrderingNumber = int.MaxValue;
                    break;
            }
        }

        public override string ToString() => _comment;

        public static CommentType GetCommentType(string comment)
        {
            switch (comment.Length)
            {
                case 1:
                    if (char.IsLetter(comment[0]))
                        return CommentType.Letter;
                    else if (char.IsDigit(comment[0]))
                        return CommentType.Number;
                    else
                        return CommentType.Error;
                case 2:
                    if (char.IsDigit(comment[0]) && char.IsLetter(comment[1]))
                        return CommentType.NumberLetter;
                    else if (char.IsDigit(comment[0]) && Char.IsDigit(comment[1]))
                        return CommentType.Number;
                    else if (char.IsLetter(comment[0]) && char.IsDigit(comment[1]))
                        return CommentType.LetterNumber;
                    else
                        return CommentType.Error;
                case 3:
                    if (char.IsDigit(comment[0]) && char.IsDigit(comment[1]) && char.IsDigit(comment[2]))
                        return CommentType.Page;
                    else if (char.IsDigit(comment[0]) && char.IsDigit(comment[1]) && char.IsLetter(comment[2]))
                        return CommentType.NumberLetter;
                    else if (char.IsLetter(comment[0]) && char.IsDigit(comment[1]) && char.IsDigit(comment[2]))
                        return CommentType.LetterNumber;
                    else
                        return CommentType.Error;
                default:
                    return CommentType.Error;
            }

        }
    }

    public enum CommentType
    {
        Page,
        Number,
        NumberLetter,
        Letter,
        LetterNumber,
        Error
    }
}
