using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
public abstract class Entity
{
    public int Id { get; set; }
    public string Description { get; set; } = "";
    public abstract void Display();
}

public class Task : Entity
{
    private string? status; 
    public string Status
    {
        get { return status; }
        set
        {
            if (value == "Pending" || value == "Completed")
            {
                status = value;
            }
            else
            {
                Console.WriteLine("Invalid status");
            }
        }
    }

    public override void Display()
    {
        Console.WriteLine($"Id: {Id}");
        Console.WriteLine($"Status: {status}");
        Console.WriteLine($"Description: {Description}");
    }

}
class Program
{
    static List<Task> tasks = new List<Task>();

    static void AddTask()
    {
        Task task = new Task();

        Console.Write("Enter ID");
        task.Id = int.Parse(Console.ReadLine());

        Console.Write("Enter Description");
        task.Description = Console.ReadLine() ?? "";
        task.Status = "Pending";

        tasks.Add(task);

        Console.WriteLine("Task added");
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
            Console.WriteLine("ID: " + task.Id);
            Console.WriteLine("Description: " + task.Description);
            Console.WriteLine("Status: " + task.Status);
        }
    }

    static void CompleteTask()
    {
        Console.Write("Enter ID: ");
        int id = int.Parse(Console.ReadLine());

        foreach (Task task in tasks)
        {
            if (task.Id == id)
            {
                task.Status = "Completed";
                Console.WriteLine("Task completed");
                return;
            }
        }

        Console.WriteLine("Task not found");
    }

    static void DeleteTask()
    {
        Console.Write("Enter ID: ");

        int id = int.Parse(Console.ReadLine());

        foreach (Task task in tasks)
        {
            if (task.Id == id)
            {
                tasks.Remove(task);
                Console.WriteLine("Task deleted");
                return;
            }
        }

        Console.WriteLine("Task not found");
    }

    public static void SearchTasks()
    {
        Console.WriteLine("Search for tasks with status:");
        Console.WriteLine("Enter 'p' for Pending tasks or 'c' for Completed tasks:");

        string choice = (Console.ReadLine() ?? "").ToLower(); bool found = false;
        foreach (Task task in tasks)
        {
            if (choice == "p" && task.Status.ToLower() == "pending")
            {
                Console.WriteLine(
                    $"Task Id: {task.Id}\nTask Status: {task.Status}\nTask Description: {task.Description}\n"
                );
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
            Console.WriteLine("No matching tasks found!");
        }
    }


    static void Main()
    {
        while (true)
        {
            Console.WriteLine("1. Add");
            Console.WriteLine("2. List");
            Console.WriteLine("3. Complete");
            Console.WriteLine("4. Delete");
            Console.WriteLine("5. Exit");
            Console.WriteLine("6. Search by Status");


            Console.Write("Enter choice: ");
            int choice = int.Parse(Console.ReadLine());

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

                default:
                    Console.WriteLine("Invalid choice");
                    break;
            }
        }
    }
}
