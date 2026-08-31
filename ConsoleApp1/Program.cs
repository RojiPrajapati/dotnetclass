using System;
using System.Collections.Generic;
abstract class Entity
{
    public int Id;
    public static int count = 1;

    public abstract void Display();
}
class Task : Entity, IComparable<Task>
{
    public string Description = "";
    private string _status = "";

    public int Priority { get; set; }

    public string Status
    {
        get => _status;

        set => _status = value == "pending" || value == "completed" ? value : throw new ArgumentException("Status must be Pending or Completed");

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

            if (!int.TryParse(Console.ReadLine(), out int priority))
            {
                Console.WriteLine("Invalid Priority.");
                return;
            }

            task.Priority = priority;

            task.Id = Entity.count++;

            tasks.Add(task);

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
                    $"Task Id: {task.Id}\nTask Status: {task.Status}\nTask Description: {task.Description}\n");
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

    static void Main()
    {
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

                default:
                    Console.WriteLine("Invalid Choice.");
                    break;
            }
        }
    }
}
