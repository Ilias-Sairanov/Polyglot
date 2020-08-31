using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace Polyglot
{
    class Task
    {
        public SentenceType SentenceType { get; private set; }
        public SentenceTime SentenceTime { get; private set; }
        public PersonType PersonType { get; private set; }
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
            if (random < 4)
                PersonType = PersonType.FirstPerson;
            else
                PersonType = PersonType.ThirdPerson;
            Pronoun = new string[] { pronounsRus[random], pronounsEng[random] };
            Verb = verbs[rnd.Next(verbs.Count)];
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
        public int WrongAnswersCount { get; private set; }
        public string GetTask(Task task)
        {
            string str = string.Format($"Время: {task.SentenceTime}\n" +
                $"Тип: {task.SentenceType}\n" +
                $"Местоимение: {task.Pronoun[0]}\n" +
                $"Глагол: {task.Verb[0].Substring(0,1).ToUpper()+task.Verb[0].Substring(1)}");
            return str;
        }
        public string GetAnswer(Task task)
        {
            switch (string.Format($"{task.SentenceTime}{task.SentenceType}"))
            {
                case "FutureQuestion":
                    return string.Format($"Will {task.Pronoun[1]} {task.Verb[1]}?");
                case "FutureAffirmative":
                    return string.Format($"{task.Pronoun[1]} will {task.Verb[1]}");
                case "FutureNegative":
                    return string.Format($"{task.Pronoun[1]} will not {task.Verb[1]}");
                case "PresentQuestion":
                    if (task.PersonType == PersonType.FirstPerson)
                        return string.Format($"Do {task.Pronoun[1]} {task.Verb[1]}?");
                    else
                        return string.Format($"Does {task.Pronoun[1]} {task.Verb[1]}?");
                case "PresentAffirmative":
                    if (task.PersonType == PersonType.FirstPerson)
                        return string.Format($"{task.Pronoun[1]} {task.Verb[1]}");
                    else
                        return string.Format($"{task.Pronoun[1]} {task.Verb[1]}s");
                case "PresentNegative":
                    if (task.PersonType == PersonType.FirstPerson)
                        return string.Format($"{task.Pronoun[1]} don't {task.Verb[1]}");
                    else
                        return string.Format($"{task.Pronoun[1]} doesn't {task.Verb[1]}");
                case "PastQuestion":
                    return string.Format($"Did {task.Pronoun[1]} {task.Verb[1]}?");
                case "PastAffirmative":
                    return string.Format($"{task.Pronoun[1]} {task.Verb[2]}");
                case "PastNegative":
                    return string.Format($"{task.Pronoun[1]} did not {task.Verb[1]}");
                default:
                    throw new InvalidOperationException();
            }
        }

        public void IsAnwerRight(bool isRight)
        {
            if (isRight)
                RightAnswersCount++;
            else
                WrongAnswersCount++;
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
                Console.WriteLine($"Количество верных ответов: {builder.RightAnswersCount}," +
                    $" ошибочных: {builder.WrongAnswersCount}");
                Console.WriteLine(builder.GetTask(task));
                Console.WriteLine("\nНапиши ответ и нажми Enter, чтобы показать верный");
                Console.ReadLine();
                Console.WriteLine($"Верный ответ:\n{builder.GetAnswer(task)}");
                Console.WriteLine("Ответ был верный?");
                Console.WriteLine("1: да\tЛюбая другая клавиша: нет");
                var isRight = Console.ReadKey().KeyChar.ToString();
                if (int.Parse(isRight) == 1)
                    builder.IsAnwerRight(true);
                else
                    builder.IsAnwerRight(false);
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
        Future,
        Present,
        Past
    }

    enum SentenceType
    {
        Question,
        Affirmative,
        Negative
    }

    enum PersonType
    {
        FirstPerson,
        ThirdPerson
    }
}
