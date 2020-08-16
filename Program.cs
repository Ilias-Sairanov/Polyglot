using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polyglot
{
    class Task
    {

    }

    class TaskBuilder
    {

    }

    class GameHost
    {
        private List<string[]> verbs;

        public GameHost()
        {
            verbs = new List<string[]>();
            var file = File.ReadAllText("Verbs.txt").Replace("\r", "");
            var splitVerbs = file.Split('\n');
            foreach (var verb in splitVerbs)
            {
                var splittedByFormsVerb = verb.Split('#');
                verbs.Add(splittedByFormsVerb);
            }
        }

        public string[] GetVerb()
        {
            var rnd = new Random();
            string[] verb = verbs[rnd.Next(verbs.Count)];
            return verb;
        }
    }



    class Program
    {
        static void Main(string[] args)
        {
            var host = new GameHost();
        }
    }

    enum SentenceTime
    {
        Present,
        Past,
        Future
    }

    enum SentenceType
    {
        Affirmative,
        Negative,
        Question
    }
}
