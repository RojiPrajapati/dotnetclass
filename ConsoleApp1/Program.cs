using System;
using System.Collections.Generic;

abstract class Entity
{
    public int Id;
    public static int count = 1;

    public abstract void Display();
}

class Task : Entity
{
    public string Description = "";

    private string _status = "";

    public string Status
    {
        get => _status;

        set => _status = value == "pending" || value == "completed"
            ? value
            : throw new ArgumentException("Status must be Pending or Completed");
    }

    public override void Display()
    {
        Console.WriteLine($"ID: {Id}");
        Console.WriteLine($"Description: {Description}");
        Console.WriteLine($"Status: {Status}");
        Console.WriteLine();
    }
}

class Program
{
    private static List<Task> tasks = new List<Task>();

    static void AddTask()
    {
        Task task = new Task();

        try
        {
            Console.Write("Enter Description: ");
            task.Description = Console.ReadLine() ?? "";

            Console.Write("Enter Status (pending/completed): ");
            task.Status = (Console.ReadLine() ?? "").ToLower();

            task.Id = Entity.count++;

            tasks.Add(task);

            Console.WriteLine("Task added");
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

        foreach (Task task in tasks)
        {
            task.Display();
        }
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
                Console.WriteLine("Task completed");
                return;
            }
        }

        Console.WriteLine("Task not found");
    }

    static void DeleteTask()
    {
        Console.Write("Enter ID: ");

        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID.");
            return;
        }

        Task taskToDelete = null;

        foreach (Task task in tasks)
        {
            if (task.Id == id)
            {
                taskToDelete = task;
                break;
            }
        }

        if (taskToDelete != null)
        {
            tasks.Remove(taskToDelete);
            Console.WriteLine("Task deleted");
        }
        else
        {
            Console.WriteLine("Task not found");
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
                task.Display();
                found = true;
            }
        }

        if (!found)
        {
            Console.WriteLine("No matching tasks found!");
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

        Console.WriteLine("Task not found");
    }

    static void Main()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("1. Add");
            Console.WriteLine("2. List");
            Console.WriteLine("3. Complete");
            Console.WriteLine("4. Delete");
            Console.WriteLine("5. Exit");
            Console.WriteLine("6. Search by Status");
            Console.WriteLine("7. Find by Id");

            Console.Write("Enter choice: ");

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Invalid choice. Please enter a number.");
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
                    return;

                case 6:
                    SearchTasks();
                    break;

                case 7:
                    FindById();
                    break;

                default:
                    Console.WriteLine("Invalid choice");
                    break;
            }
        }
    }
}
