using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb3_HenrikVu.Model
{
    internal class Question
    {
        public Question(string query = "My Question", string correctAnswer = "Correct1", string incorrectAnswer1 = "Incorrect1", string incorrectAnswer2 = "Incorrect2", string incorrectAnswer3 = "Incorrect3")
        {
            Query = query;
            CorrectAnswer = correctAnswer;
            IncorrectAnswers = new string[3] { incorrectAnswer1, incorrectAnswer2, incorrectAnswer3 };
        }
        public string Query { get; set; }
        public string CorrectAnswer { get; set; }
        public string IncorrectAnswer1 { get; }
        public string IncorrectAnswer2 { get; }
        public string IncorrectAnswer3 { get; }
        public string[] IncorrectAnswers { get; set; }
    }
}
