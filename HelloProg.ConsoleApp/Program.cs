using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HelloProgLib;
using HelloProgLib.Models;
using HelloProgLib.Services;

namespace HelloProg.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            HabbitService habbitService = new HabbitService();
            try
            {
                habbitService.AddHabbit("Sharp", "");
                habbitService.AddHabbit("C++", "Prosto plusy");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }

            List<Habit> habits = habbitService.AllHabits();

            foreach (Habit habit in habits) {
                Console.WriteLine(habit.title);
                Console.WriteLine(habit.description);
                Console.WriteLine();
            }

            Console.ReadLine();
        }

    }
}
