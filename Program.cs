using System;
using System.Collections.Generic;
using System.IO;

namespace Polyglot
{
    class Task
    {
        public SentenceType SentenceType { get; }
        public SentenceTime SentenceTime { get; }
        public string[] Pronoun { get; }
        public string[] Verb { get; }

        public Task(string[] verb, string[] pronoun)
        {
            var rnd = new Random();
            SentenceTime = (SentenceTime)rnd.Next(3);
            SentenceType = (SentenceType)rnd.Next(3);
            Pronoun = pronoun;
            Verb = verb;
            for (int i = 0; i < Verb.Length; i++)
            {
                Verb[i] = Verb[i].Substring(0, 1).ToUpper() + Verb[i].Substring(1);
            }
        }
    }

    class TaskBuilder
    {
        public string GetTask(Task task, TaskType taskType)
        {
            string str = string.Format($"Время: {task.SentenceTime}\n" +
                $"Тип: {task.SentenceType}\n" +
                $"Местоимение: {task.Pronoun[0]}\n" +
                $"Глагол: {task.Verb[0]}");
            return str;
        }
    }

    class GameHost
    {
        private List<string[]> verbs;
        private string[] pronounsEng = { "I", "You", "We", "They", "He", "She", "It" };
        private string[] pronounsRus = { "Я", "Вы", "Мы", "Они", "Он", "Она", "Оно" };

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

        public string[] GetPronoun()
        {
            var rnd = new Random();
            int i = rnd.Next(pronounsEng.Length);
            string[] pronoun = { pronounsRus[i], pronounsEng[i] };
            return pronoun;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var host = new GameHost();
            var task = new Task(host.GetVerb(), host.GetPronoun());
            var bilder = new TaskBuilder();
            bool gameOver = false;
            while (!gameOver)
            {
                Console.WriteLine(bilder.GetTask(task, TaskType.First));
                var typedKey = Console.ReadKey();
                switch (typedKey.Key)
                {
                    case ConsoleKey.D1:
                        Console.WriteLine("check");
                        break;
                    default:
                        break;
                }
                Console.WriteLine();
            }
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

    enum TaskType
    {
        First,
        Second
    }
}
