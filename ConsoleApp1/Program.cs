public class Task
{
    public int Id;
    public string Description;
    public string Status;
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
        task.Description = Console.ReadLine();

        task.Status = "Pending";

        tasks.Add(task);

        Console.WriteLine("Task added");
    }

    static void ListTasks()
    {
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

    static void Main()
    {
        while (true)
        {
            Console.WriteLine("1. Add");
            Console.WriteLine("2. List");
            Console.WriteLine("3. Complete");
            Console.WriteLine("4. Delete");
            Console.WriteLine("5. Exit");

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

                default:
                    Console.WriteLine("Invalid choice");
                    break;
            }
        }
    }
}

