/*
 *  Kod för att köra/skapa/editera en Todolista sparad till text fil
 *  sorteras med hjälp av LINQ
 * 
 * 
 * 
 * 
 */







// Create empty List for file to fill
List<Task> tasks = new();


// ----------------- program start




// skapar input för todolist

while (true)
{

    // Start() returnerar 1-4 (show todolist, add new task, edit task(update,mark as done, remove), and exit
    int waschosen = Start();

    // show tasks
    if (waschosen == 1)
    {
        int sorting = choseSorting();  
        
        ShowTasks(sorting);

        // Keep the console window open.
        Console.WriteLine("-------------------------------");
        Console.WriteLine("Press any key to continue.");
        Console.ReadKey();
        continue;

    }

    // add task
    else if (waschosen == 2)
    {
        (string Ret, string title, DateTime dt, bool done, string project) = TodoListAdd(1);

        if (Ret == "add")
        {
            
            tasks.Add(new Task(title, dt, done, project));
            // add to file now
            using StreamWriter file = new(@"./todolist.txt", append: true);
            await file.WriteLineAsync(title + "|||" + dt.ToString("yyyy-MM-dd") + "|||" + done.ToString() + "|||" + project);

            Console.WriteLine("New task was added.");
            // Keep the console window open.
            Console.WriteLine("-------------------------------");
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
            continue;

        }      
        else if (Ret == "ended")
        {
            Console.WriteLine("Todolist was unaltered.");
            break;
        }
        
    }
    else if (waschosen == 3)
    {
        // edit or remove or done
        int editing = editMode();

        int tasktoedit = 0;

        if (editing == 1)
        {
            // edit one of these tasks
            tasktoedit = showTaskEdit();

            // edit the line
            // get new line
            (string Ret, string title, DateTime dt, bool done, string project) = TodoListAdd(0);

            if (Ret == "add" && tasktoedit > -1)
            {

                if (editThis("EDIT", title, dt, done, project, tasktoedit))
                {
                    Console.WriteLine("Edit was a success");
                }
                else
                {
                    Console.WriteLine("TodoList was NOT altered");
                }
            }
            else
            {
                Console.WriteLine("TodoList was NOT altered");
            }

        }
        else if (editing == 2)
        {
            // toggle to done status
            // edit one of these tasks
            tasktoedit = showTaskEdit();

            if (tasktoedit > -1)
            {

                if (editThis("DONE", "", DateTime.Now, false, "", tasktoedit))
                {
                    Console.WriteLine("Edit was a success");
                }
                else
                {
                    Console.WriteLine("TodoList was NOT altered");
                }
            }

        }
        else if (editing == 3)
        {
            // remove one of the tasks
            // edit one of these tasks
            tasktoedit = showTaskEdit();

            if (tasktoedit > -1)
            {

                if (editThis("REM", "", DateTime.Now, false, "", tasktoedit))
                {
                    Console.WriteLine("Edit was a success");
                }
                else
                {
                    Console.WriteLine("TodoList was NOT altered");
                }
            }

        }
        // Keep the console window open.
        Console.WriteLine("-------------------------------");
        Console.WriteLine("Press any key to continue.");
        Console.ReadKey();
        continue;

    }
    else if (waschosen == 4)
    {
        Console.WriteLine("Todolist Exit.");
        break;
    }


}


// ----------------- end program



// Methods

static int Start()
{

    string firstinput = "";
    bool Success = false;
    int choice = 0;

    while (true)
    {

        Console.ResetColor();
        Console.WriteLine("Welcome to TodoList");

        (int taskTot, int taskDone) = CountTask();

        Console.WriteLine("You have " + taskTot + " tasks and " + taskDone + " tasks are done.");
        Console.WriteLine("-----------------------------");
        Console.WriteLine("[1] Show all tasks.");
        Console.WriteLine("[2] Add new task.");
        Console.WriteLine("[3] Edit Task(Edit, mark as done, remove)");
        Console.WriteLine("[4] Save and exit.");

        //Console.WriteLine("q ends input.");
        Console.WriteLine("");
        Console.Write("Enter [1-4]: ");

        try
        {
            firstinput = Console.ReadLine();

            if (firstinput.ToLower().Trim() == "q")
            {
                Console.WriteLine("Ends it.");
                break;
            }

            Success = int.TryParse(firstinput, out choice);

        }
        catch (Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Wrong Input. Error: " + e);
            Console.WriteLine("Try again.");
            continue;
        }
        if (Success)
        {
            if (choice > 0 && choice < 5)
            {
                break;
            }
            else
            {
                Console.WriteLine("Must be between 1-4. Try again.");
                continue;
            }
        }
    }
    if (Success)
    {
        return choice;
    }
    else
    {
        return 0;
    }

}


static int choseSorting()
{

    int newsort = 0;
    bool Success = false;
    var firstinput = "";

    Console.ResetColor();
    Console.WriteLine("Chose Sorting");
    Console.WriteLine("-----------------------------");
    Console.WriteLine("[1] Sort by Project.");
    Console.WriteLine("[2] Sort by Due Date.");
    Console.Write("Enter Choice: ");

    while (true)
    {

        try
        {
            firstinput = Console.ReadLine();

            if (firstinput.ToLower().Trim() == "q")
            {
                Console.WriteLine("Ends it.");
                break;
            }

            Success = int.TryParse(firstinput, out newsort);

        }
        catch (Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Wrong Input. Error: " + e);
            Console.WriteLine("Try again.");
            continue;
        }
        if (Success)
        {
            if (newsort > 0 && newsort < 3)
            {
                break;
            }
            else
            {
                Console.WriteLine("Must be [1-2]. Try again.");
                continue;
            }
        }
    }
    if (Success)
    {
        return newsort;
    }
    else
    {
        return 0;
    }

}


static int editMode()
{
    int choice = 0;
    bool Success = false;
    var firstinput = "";

    Console.ResetColor();
    Console.WriteLine("Chose Sorting");
    Console.WriteLine("-----------------------------");
    Console.WriteLine("[1] Edit a Task.");
    Console.WriteLine("[2] Alter Done status.");
    Console.WriteLine("[3] Remove Task.");
    Console.Write("Enter Choice[1-3]: ");

    while (true)
    {

        try
        {
            firstinput = Console.ReadLine();

            if (firstinput.ToLower().Trim() == "q")
            {
                Console.WriteLine("Ends it.");
                break;
            }

            Success = int.TryParse(firstinput, out choice);

        }
        catch (Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Wrong Input. Error: " + e);
            Console.WriteLine("Try again.");
            continue;
        }
        if (Success)
        {
            if (choice > 0 && choice < 4)
            {
                break;
            }
            else
            {
                Console.WriteLine("Must be [1-3]. Try again.");
                continue;
            }
        }
    }
    if (Success)
    {
        return choice;
    }
    else
    {
        return 0;
    }

}




static int showTaskEdit() {

    string input = "";
    int choice = 0;
    bool Success = false;

    Console.ResetColor();
    Console.WriteLine("Todo List");
    Console.WriteLine("-----------------------------");


    // get file data
    string[] lines = File.ReadAllLines(@".\todolist.txt");

    // Display the file contents by using a foreach loop.
    int i = 0;
    foreach (string line in lines)
    {
        if (line.Trim() != null)
        {
            //for debugging
            // Use a tab to indent each line of the file.
            //Console.WriteLine("\t" + line);

            // count tasks
            // I use ||| to split each file lines into an array to use
            var lineArr = line.Split("|||");
            i++;
            Console.WriteLine("[" + i + "] Edit: " + lineArr[0]);
        }
    }
    Console.Write("Chose Task to edit: ");
    input = Console.ReadLine();
    Success = int.TryParse(input, out choice);

    if (Success && choice > 0)
    {
        choice = choice - 1;
        return choice;
    } 
    else
    {
        return -1;
    }

}



static bool editThis(string action, string title, DateTime dt, bool done, string project, int taskNR=0)
{
    bool success = false;

    // get file data
    string[] oldlines = File.ReadAllLines(@".\todolist.txt");

    List<string> editedlist = new List<string>();
    
    //String[] str = editedlist.ToArray();



    // go through the file contents by using a foreach loop.
    foreach (string line in oldlines)
    {
        //Console.WriteLine(taskNR); //for debugging
        if (line == oldlines[taskNR] && action == "EDIT")
        {
            // add edited line instead to newlist
            string editedline = title + "|||" + dt.ToString("yyyy-MM-dd") + "|||" + done.ToString() + "|||" + project;
            editedlist.Add(editedline);
            success = true;
        } 
        else if (line == oldlines[taskNR] && action == "DONE")
        {
            var lineArr = line.Split("|||");
            //  edit done status and add to newlist
            string editedline = lineArr[0] + "|||" + lineArr[1] + "|||true|||" + lineArr[3];
            editedlist.Add(editedline);
            success = true;
        }
        else if (line == oldlines[taskNR] && action == "REM")
        {
            //dont add line to editedList cuz we are removing it
            success = true;
            continue;
        }
        else
        {
            // add old data to new list
            editedlist.Add(line);
        }

    }

    // and now write the edited data to the file
    using StreamWriter writer = new StreamWriter(@".\todolist.txt");
    foreach (string newline in editedlist)
    {
        writer.WriteLine(newline);
    }

    if (success)
    {
        return true;
    }
    else
    {
        return false;
    }

}




// add new task
static (string, string, DateTime, bool, string) TodoListAdd(int editmod)
{

    // Återanvänder kod från min asset-tracking.cs

    // declare empty strings
    string newInput = "";
    string dateInput = "";
    string projectInput = "";

    DateTime truedate = DateTime.Now;
    bool thisDone = false;
    
    // reset color
    Console.ResetColor();
    Console.WriteLine("");
    if (editmod == 1)
    {
        Console.WriteLine("Add new Task.");
    }
    else
    {
        Console.WriteLine("Edit Task.");
    }
    Console.WriteLine("q to cancel new Task and exit.");
    Console.WriteLine("");


    // input new task
    while (true)
    {

        try
        {
            Console.ResetColor();
            Console.Write("Enter Task Title: ");
            newInput = Console.ReadLine();

            if (newInput.ToLower().Trim() == "q")
            {
                Console.WriteLine("Cancels input.");
                break;
            }

        }
        catch (Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine("Error in Input. Error: " + e);
            Console.WriteLine("Try again.");
            continue;

        }

        if (newInput.Trim() == "")
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error in Input. Seems to be empty.");
            Console.WriteLine("Try again.");
            continue;
        }
        else
        {
            break;
        }
    }
    if (newInput.ToLower().Trim() == "q")
    {
        return ("ended", "none", DateTime.Now, false, "");
    }



    // input due date
    while (true)
    {

        try
        {
            Console.ResetColor();
            Console.Write("Enter due date(like this, 2022-06-06): ");
            dateInput = Console.ReadLine();

            if (dateInput.ToLower().Trim() == "q")
            {
                Console.WriteLine("Cancels input.");
                break;
            }

            truedate = DateTime.Parse(dateInput);

        }
        catch (Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error in Input. Error: " + e);
            Console.WriteLine("Try again.");
            continue;

        }

        if (dateInput.Trim() == "")
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error in Input. seems to be empty");
            Console.WriteLine("Try again.");
            continue;
        }
        else
        {
            break;
        }

    }
    if (dateInput.ToLower().Trim() == "q")
    {
        return ("ended", "none", DateTime.Now, false, "");
    }


    // input project
    string project = "";
    while (true)
    {

        try
        {
            Console.ResetColor();
            Console.Write("Enter your projekt: ");
            projectInput = Console.ReadLine();

            if (projectInput.ToLower().Trim() == "q")
            {
                Console.WriteLine("Ends Input.");
                break;
            }

        }
        catch (Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine("Error in Input. Error: " + e);
            Console.WriteLine("Try again.");
            continue;

        }

        if (projectInput.Trim() == "")
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Wrong input! Seems to be empty. Try again.");
            continue;
        }
        else
        {
            project = projectInput.ToLower().Trim();
            break;
        }

       
    }
    if (newInput.ToLower().Trim() == "q")
    {
        return ("ended", "none", DateTime.Now, false, "FEL");
    }
    else
    {
        // Return data for new entry in todolist
        return ("add", newInput, truedate, thisDone, project);
    }

}





static void ShowTasks(int sortstyle)
{

    // reset color
    Console.ResetColor();

    // init list
    string line;
    List<Task> TodoList = new();

    // get file data
    StreamReader file = new StreamReader(@".\todolist.txt");
    while ((line = file.ReadLine()) != null)
    {
        string[] data = line.Split("|||");
        var newdt = DateTime.Parse(data[1]);
        bool mydone = Convert.ToBoolean(data[2]);
        TodoList.Add(new Task(data[0], newdt, mydone, data[3]));
    }
    file.Close();


    if (sortstyle == 1)
    {
        // Sort by project

        //Due date how long ago?
        DateTime now = DateTime.Now;
        DateTime months6away = now.AddMonths(-30);
        DateTime months3away = now.AddMonths(-33);
        DateTime threeyearsago = now.AddMonths(-36);

        Console.WriteLine("----------------------------------------");
        Console.WriteLine("Tasks sorted by Project.");
        Console.WriteLine("");
        Console.WriteLine("Task".PadRight(20) + "Due Date".PadRight(15) + "Status".PadRight(10) + "Project".PadRight(20));

        //  order by Project
        var orderedResultProject = TodoList.OrderBy(q => q.Project);

        foreach (var obj in orderedResultProject)
        {
            if (obj.DueDate > threeyearsago)
            {
                if (obj.DueDate < months3away)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.Red;
                    obj.ShowData();
                    Console.ResetColor();
                }
                else if (obj.DueDate < months6away)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    obj.ShowData();
                    Console.ResetColor();
                }
                else
                {
                    Console.ResetColor();
                    obj.ShowData();
                }
            }
            Console.ResetColor();
        }
    }


    else if (sortstyle == 2) {
        // Sort by due date

        Console.WriteLine("----------------------------------------");
        Console.WriteLine("Task sorted by Due date.");
        Console.WriteLine("");
        Console.WriteLine("Task".PadRight(20) + "Due Date".PadRight(15) + "Status".PadRight(10) + "Project".PadRight(20));

        // order by Due Date
        var orderedResultDate = TodoList.OrderBy(q => q.DueDate);

        //Due date how long ago?
        DateTime now = DateTime.Now;
        DateTime months6away = now.AddMonths(-30);
        DateTime months3away = now.AddMonths(-33);
        DateTime threeyearsago = now.AddMonths(-36);

        foreach (var obj in orderedResultDate)
        {

            if (obj.DueDate > threeyearsago)
            {
                if (obj.DueDate < months3away)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.Red;
                    obj.ShowData();
                    Console.ResetColor();
                }
                else if (obj.DueDate < months6away)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    obj.ShowData();
                    Console.ResetColor();
                }
                else
                {
                    Console.ResetColor();
                    obj.ShowData();
                }
            }
            Console.ResetColor();
        }
    }

}


static (int, int) CountTask() {

    int tottask = 0;
    int donetask = 0;

    try {

        //for debugging
        //Console.WriteLine("Contents of todolist.txt = ");

        // get file data
        string[] lines = File.ReadAllLines(@".\todolist.txt");
        // total amount of tasks
        tottask = lines.Length;

        // Display the file contents by using a foreach loop.
        foreach (string line in lines)
        {
            if (line.Trim() != null) {
                //for debugging
                // Use a tab to indent each line of the file.
                //Console.WriteLine("\t" + line);

                // count tasks
                // I use ||| to split each file lines into an array to use
                var lineArr = line.Split("|||");
                foreach (string s in lineArr)
                {
                    if (s == "true")
                    {
                        donetask++;
                    }
                }
            }
        }
    }
    catch (Exception e){
        Console.WriteLine(e);
    }
   
    return (tottask, donetask);

}




// CLASSES


class Task
{
    public Task(string taskTitle, DateTime dueDate, bool status, string project)
    {
        TaskTitle = taskTitle;
        DueDate = dueDate;
        Status = status;
        Project = project;
    }

    public string TaskTitle { get; set; }
    public DateTime DueDate { get; set; }
    public bool Status { get; set; }
    public string Project { set; get; }


    public void ShowData()
    {
        string myStatus = "";
        if (Status == true)
        {
            myStatus = "Done";
        }
       else
        {
            myStatus = "Pending";
        }        
        
        Console.WriteLine(TaskTitle.PadRight(20) + DueDate.ToString("yyyy/MM/dd").PadRight(15) + myStatus.PadRight(10) + Project.PadRight(20));

    }


}


