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

        public void ChangeTypeTimePronoun()
        {
            var rnd = new Random();
            SentenceTime = (SentenceTime)rnd.Next(3);
            SentenceType = (SentenceType)rnd.Next(3);
            int random = rnd.Next(pronounsEng.Length);
            Pronoun = new string[] { pronounsRus[random], pronounsEng[random] };
        }

        public void ChangeVerb()
        {
            ChangeTypeTimePronoun();
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
        public int RightAnswersCount { get; private set; }
        public string GetTask(Task task)
        {
            string str = string.Format($"Время: {task.SentenceTime}\n" +
                $"Тип: {task.SentenceType}\n" +
                $"Местоимение: {task.Pronoun[0]}\n" +
                $"Глагол: {task.Verb[0]}");
            return str;
        }

        public string GetAnswer(Task task)
        {
            return "ANSWER";
        }

        public void IsAnwerRight(bool isRight)
        {
            if (isRight) 
                RightAnswersCount++;
        }
    }

    class GameMenu
    {
        private Task task;

        public GameMenu(Task task)
        {
            this.task = task;
        }
    }

    class Program
    {
        static void Main()
        {
            var task = new Task();
            var builder = new TaskBuilder();
            bool gameOver = false;
            while (!gameOver)
            {
                Console.WriteLine($"Количество верных ответов: {builder.RightAnswersCount}");
                Console.WriteLine(builder.GetTask(task));
                Console.WriteLine("\nНажмите любую клавишу, чтобы показать ответ");
                Console.ReadKey();
                Console.WriteLine("\n" + builder.GetAnswer(task));
                Console.WriteLine("Ответ был верный?");
                Console.WriteLine("1: да 2: нет");
                var isRight = Console.ReadKey().KeyChar;
                if (int.Parse(isRight.ToString()) == 1) builder.IsAnwerRight(true);
                Console.WriteLine("\n1: новое задание\t2:заменить глагол");
                var typedKey = Console.ReadKey();
                switch (typedKey.Key)
                {
                    case ConsoleKey.D1:
                        task.ChangeTypeTimePronoun();
                        break;
                    case ConsoleKey.D2:
                        task.ChangeVerb();
                        break;
                    default:
                        break;
                }
                Console.Clear();
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
