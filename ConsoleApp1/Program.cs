// Console.WriteLine("Hello, World!");
//
// string description = "This is a sample description.";
// int priority = 1;
// string dueDate = "2026-8-24";
// string title = "Dotnet Class";
// bool isComplete = false;
// decimal cost = 200.45m;
// DateTime dueDate2 = DateTime.Now;
//
// Console.WriteLine("The title is: " + title);    
// Console.WriteLine(description);
// Console.WriteLine("The priority level: "+ priority);
// Console.WriteLine("This was written on: " + dueDate);
// Console.WriteLine("The due date is: " + dueDate2);
// Console.WriteLine("The class completed? " + isComplete);
// Console.WriteLine("Cost for the course: " + cost);

// Day 2 - Control Flow, Loops and Methods
/*
 * Add, List, Complete, Delete methods
 * In a loop
 * SearchTask method
 */

public class Task
{
    public int Id { get; set; }
    public string Status { get; set; }
    public string Description { get; set; }
}
public class Program
{
    static List<Task> tasks = new List<Task>();
    static int nextId = 1;
    public static void AddTask()
    {
        Console.Write("Enter task description: ");
        string description = Console.ReadLine();
        Task task = new Task
        {
            Id = nextId++,
            Status = "Pending",
            Description = description
        };
        tasks.Add(task);
        Console.WriteLine("Task added successfully!");
    }
    public static void ListTask()
    {
        if (tasks.Count == 0)
        {
            Console.WriteLine("No tasks found!");
            return;
        }
        foreach (Task task in tasks)
        {
            Console.WriteLine("Task id:" + task.Id + "\nTask status:" + task.Status + " Task description:" + task.Description);
        }

    }

    public static void CompleteTask()
    {
        Console.Write("Enter task id: ");
        if (int.TryParse(Console.ReadLine(), out int check_id))
        {
            Console.WriteLine($"Valid ID: {check_id}");
        }
        foreach (Task task in tasks)
        {
            if (task.Id == check_id)
            {
                Console.WriteLine("Task completed successfully!");
                task.Status = "Completed";
                return;
            }
            else
            {
                Console.WriteLine("Invalid task id!");
            }
        }
    }
    public static void DeleteTask()
    {
        Console.Write("Enter task id: ");
        if (!int.TryParse(Console.ReadLine(), out int check_id))
        {
            Console.WriteLine("Invalid input!");
            return;
        }
        Task taskToDelete = tasks.Find(task => task.Id == check_id);
        if (taskToDelete != null)
        {
            tasks.Remove(taskToDelete);
            Console.WriteLine("Task deleted successfully!");
        }
        else
        {
            Console.WriteLine("Task not found!");
        }
    }
    public static void SearchTasks()
    {
        Console.WriteLine("Search for tasks with status:");
        Console.WriteLine("Enter 'p' for Pending tasks or 'c' for Completed tasks:");
        string choice = Console.ReadLine().ToLower();
        bool found = false;
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
    public static void Main()
    {
        bool running = true;
        while (running)
        {
            Console.WriteLine("Choose an option: \n 1.Add a Task\n2.List All Tasks\n3.Complete Task by Id\n4.Delete Task by Id\n5.Search Task by Status \n");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1": AddTask(); break;
                case "2": ListTask(); break;
                case "3": CompleteTask(); break;
                case "4": DeleteTask(); break;
                case "5": SearchTasks(); break;

                default: running = false; break;
            }

        }
    }
}
