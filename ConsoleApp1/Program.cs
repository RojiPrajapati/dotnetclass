using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text.Json;
using System.IO;
using System.Linq;

abstract class Entity
{
    public int Id { get; set; }
    public static int count = 1;

    public abstract void Display();
}

class Task : Entity, IComparable<Task>
{
    public string Description { get; set; } = "";
    private string _status = "";

    public DateTime CreatedDate { get; set; }

    public DateOnly EffectiveDate { get; set; }

    public DateTime UpdatedDate { get; set; }

    public int Priority { get; set; }



    public string Status
    {
        get => _status;

        set => _status = value == "pending" || value == "completed"
            ? value
            : throw new ArgumentException("Status must be Pending or Completed");

    }

    public int CompareTo(Task? other)
    {
        if (other == null)
            return 1;

        return Priority.CompareTo(other.Priority);
    }

    public override void Display()
    {
        Console.WriteLine($"ID: {Id}");
        Console.WriteLine($"Description: {Description}");
        Console.WriteLine($"Status: {Status}");
        Console.WriteLine($"Priority: {Priority}");
        Console.WriteLine($"Created Date: {CreatedDate:dd/MM/yyyy HH:mm:ss}\n");
        Console.WriteLine($"Effective Date: {EffectiveDate:dd/MM/yyyy}\n");
        Console.WriteLine($"Updated Date: {UpdatedDate:dd/MM/yyyy HH:mm:ss}\n");

    }
}

class Program
{
    private static List<Task> tasks = new List<Task>();

    private static Queue<Task> reviewQueue = new Queue<Task>();

    static void AddTask()
    {
        Task task = new Task();

        try
        {
            Console.Write("Enter Description: ");
            task.Description = Console.ReadLine() ?? "";

            Console.Write("Enter Status (pending/completed): ");
            task.Status = (Console.ReadLine() ?? "").ToLower();

            Console.Write("Enter Priority (1-5): ");

            if (!int.TryParse(Console.ReadLine(), out int priority) || priority < 1 || priority > 5)
            {
                Console.WriteLine("Priority must be between 1 and 5.");
                return;
            }

            task.Priority = priority;

            // Date validation logic
            DateOnly effectiveDate;

            while (true)
            {
                Console.Write("Enter Effective Date (dd/MM/yyyy): ");
                string dateInput = Console.ReadLine() ?? "";

                string datePattern =
                    @"^(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[0-2])/\d{4}$";

                if (!Regex.IsMatch(dateInput, datePattern))
                {
                    Console.WriteLine("Invalid date format! Please use dd/MM/yyyy.");
                    continue;
                }

                if (!DateOnly.TryParseExact(
                     dateInput,
                     "dd/MM/yyyy",
                     out effectiveDate))
                {
                    Console.WriteLine("Invalid date! Please enter a valid calendar date.");
                    continue;
                }

                // Reject future dates
                if (effectiveDate > DateOnly.FromDateTime(DateTime.Today))
                {
                    Console.WriteLine("Future dates are not allowed.");
                    Console.WriteLine("Please enter today's date or a past date.");
                    continue;
                }

                break;
            }

            DateTime now = DateTime.Now;

            task.CreatedDate = now;
            task.UpdatedDate = now;
            task.EffectiveDate = effectiveDate;

            task.Id = Entity.count++;

            tasks.Add(task);

            SaveTasks();

            Console.WriteLine("Task added successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }




    static void ListTasks()
    {
        if (tasks.Count == 0)
        {
            Console.WriteLine("No tasks available.");
            return;
        }

        bool found = false;

        DateOnly today = DateOnly.FromDateTime(DateTime.Today);


        foreach (Task task in tasks)
        {
            if (task.EffectiveDate <= today)
            {
                task.Display();
                found = true;
            }
        }
        if (!found)
        {
            Console.WriteLine("No effective tasks found!");
        }
    }

    static void SaveTasks()
    {
        string json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        File.WriteAllText("./tasks.json", json);
    }


    static void CompleteTask()
    {
        Console.Write("Enter ID: ");

        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID.");
            return;
        }

        foreach (Task task in tasks)
        {
            if (task.Id == id)
            {
                task.Status = "completed";
                task.UpdatedDate = DateTime.Now;
                SaveTasks();
                Console.WriteLine("Task completed.");
                return;
            }
        }

        Console.WriteLine("Task not found.");
    }

    static void DeleteTask()
    {
        Console.Write("Enter ID: ");

        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID.");
            return;
        }

        Task? taskToRemove = null;

        foreach (Task task in tasks)
        {
            if (task.Id == id)
            {
                taskToRemove = task;
                break;
            }
        }

        if (taskToRemove != null)
        {
            tasks.Remove(taskToRemove);
            SaveTasks();
            Console.WriteLine("Task deleted.");
        }
        else
        {
            Console.WriteLine("Task not found.");
        }
    }

    static void SearchTasks()
    {
        Console.WriteLine("Search for tasks with status:");
        Console.WriteLine("Enter 'p' for Pending tasks or 'c' for Completed tasks:");

        string choice = (Console.ReadLine() ?? "").ToLower();

        bool found = false;
        foreach (Task task in tasks)
        {
            if (choice == "p" && task.Status.ToLower() == "pending")
            {
                task.Display();
                found = true;
            }
            else if (choice == "c" && task.Status.ToLower() == "completed")
            {
                Console.WriteLine(
                    $"Task Id: {task.Id}\nTask Status: {task.Status}\nTask Description: {task.Description}\n"
                );
                found = true;
            }
        }

        if (!found)
        {
            Console.WriteLine("No matching tasks found.");
        }
    }

    static void FindById()
    {
        Console.Write("Enter ID: ");

        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID.");
            return;
        }

        foreach (Task task in tasks)
        {
            if (task.Id == id)
            {
                Console.WriteLine("\nTask Found:");
                task.Display();
                return;
            }
        }

        Console.WriteLine("Task not found.");
    }

    static void SortTasks()
    {
        if (tasks.Count == 0)
        {
            Console.WriteLine("No tasks available.");
            return;
        }

        tasks.Sort();

        Console.WriteLine("\nTasks Sorted By Priority:\n");

        foreach (Task task in tasks)
        {
            task.Display();
        }
    }

    static void AddToReviewQueue()
    {
        Console.Write("Enter Task ID: ");

        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID.");
            return;
        }

        foreach (Task task in tasks)
        {
            if (task.Id == id)
            {
                reviewQueue.Enqueue(task);

                Console.WriteLine("Task added to review queue.");

                return;
            }
        }

        Console.WriteLine("Task not found.");
    }

    static void ReviewNextTask()
    {
        if (reviewQueue.Count == 0)
        {
            Console.WriteLine("Review queue is empty.");
            return;
        }

        Task task = reviewQueue.Dequeue();

        Console.WriteLine("\nReviewing Task:");
        task.Display();
    }

    static void ViewReviewQueue()
    {
        if (reviewQueue.Count == 0)
        {
            Console.WriteLine("Review queue is empty.");
            return;
        }

        Console.WriteLine("\nTasks In Review Queue:\n");

        foreach (Task task in reviewQueue)
        {
            task.Display();
        }
    }

    static void LoadTasks()
    {
        if (!File.Exists("tasks.json"))
            return;

        string json = File.ReadAllText("tasks.json");

        List<Task>? loadedTasks =
            JsonSerializer.Deserialize<List<Task>>(json);

        if (loadedTasks != null)
        {
            tasks = loadedTasks;

            if (tasks.Count > 0)
            {
                Entity.count = tasks.Max(t => t.Id) + 1;
            }
        }
    }




    static void Main()
    {
        LoadTasks();
        while (true)
        {
            Console.WriteLine("1. Add Task");
            Console.WriteLine("2. List Tasks");
            Console.WriteLine("3. Complete Task");
            Console.WriteLine("4. Delete Task");
            Console.WriteLine("5. Exit");
            Console.WriteLine("6. Search By Status");
            Console.WriteLine("7. Find By ID");
            Console.WriteLine("8. Sort Tasks By Priority");
            Console.WriteLine("9. Add Task To Review Queue");
            Console.WriteLine("10. Review Next Task");
            Console.WriteLine("11. View Review Queue");
            Console.WriteLine("12. Save Tasks");



            Console.Write("\nEnter Choice: ");

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Invalid Choice.");
                continue;
            }

            switch (choice)
            {
                case 1:
                    AddTask();
                    break;

                case 2:
                    ListTasks();
                    break;

                case 3:
                    CompleteTask();
                    break;

                case 4:
                    DeleteTask();
                    break;

                case 5:
                    Console.WriteLine("Exiting...");
                    return;

                case 6:
                    SearchTasks();
                    break;

                case 7:
                    FindById();
                    break;

                case 8:
                    SortTasks();
                    break;

                case 9:
                    AddToReviewQueue();
                    break;

                case 10:
                    ReviewNextTask();
                    break;

                case 11:
                    ViewReviewQueue();
                    break;

                case 12:
                    SaveTasks();
                    Console.WriteLine(Path.GetFullPath("tasks.json"));
                    Console.WriteLine("Tasks saved successfully to tasks.json");
                    break;


                default:
                    Console.WriteLine("Invalid Choice.");
                    break;
            }
        }
    }
}