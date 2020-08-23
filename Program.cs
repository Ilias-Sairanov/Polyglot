using System;
using System.Collections.Generic;
using System.IO;

namespace Polyglot
{
    class Task
    {
        public SentenceType SentenceType { get; private set; }
        public SentenceTime SentenceTime { get; private set; }
        public string[] Pronoun { get; private set; }
        public string[] Verb { get; private set; }

        private List<string[]> verbs;
        private string[] pronounsEng = { "I", "You", "We", "They", "He", "She", "It" };
        private string[] pronounsRus = { "Я", "Вы", "Мы", "Они", "Он", "Она", "Оно" };

        public Task()
        {
            verbs = new List<string[]>();
            var file = File.ReadAllText("Verbs.txt").Replace("\r", "");
            var splitVerbs = file.Split('\n');
            foreach (var verb in splitVerbs)
            {
                var splittedByFormsVerb = verb.Split('#');
                verbs.Add(splittedByFormsVerb);
            }

            var rnd = new Random();
            SentenceTime = (SentenceTime)rnd.Next(3);
            SentenceType = (SentenceType)rnd.Next(3);
            int random = rnd.Next(pronounsEng.Length);
            Pronoun = new string[] { pronounsRus[random], pronounsEng[random] };
            Verb = verbs[rnd.Next(verbs.Count)];
            for (int i = 0; i < Verb.Length; i++)
            {
                Verb[i] = Verb[i].Substring(0, 1).ToUpper() + Verb[i].Substring(1);
            }
        }

        public void NextTask()
        {
            var rnd = new Random();
            SentenceTime = (SentenceTime)rnd.Next(3);
            SentenceType = (SentenceType)rnd.Next(3);
            int random = rnd.Next(pronounsEng.Length);
            Pronoun = new string[] { pronounsRus[random], pronounsEng[random] };
        }

        public void NewVerb()
        {
            NextTask();
            var rnd = new Random();
            Verb = verbs[rnd.Next(verbs.Count)];
            for (int i = 0; i < Verb.Length; i++)
            {
                Verb[i] = Verb[i].Substring(0, 1).ToUpper() + Verb[i].Substring(1);
            }
        }
    }

    class TaskBuilder
    {
        public string GetTask(Task task)
        {
            string str = string.Format($"Время: {task.SentenceTime}\n" +
                $"Тип: {task.SentenceType}\n" +
                $"Местоимение: {task.Pronoun[0]}\n" +
                $"Глагол: {task.Verb[0]}");
            return str;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var task = new Task();
            var bilder = new TaskBuilder();
            bool gameOver = false;
            while (!gameOver)
            {
                Console.WriteLine(bilder.GetTask(task));
                Console.WriteLine("1: новое задание\t2:заменить глагол");
                var typedKey = Console.ReadKey();
                Console.WriteLine();
                switch (typedKey.Key)
                {
                    case ConsoleKey.D1:
                        task.NextTask();
                        break;
                    case ConsoleKey.D2:
                        task.NewVerb();
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
}
