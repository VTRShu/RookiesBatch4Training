using ASP.Net_Core_API_Exer.Models;

namespace ASP.Net_Core_API_Exer.Services.Implement;

public class TaskService : ITaskService
{
    public static List<TaskModel> taskList = new List<TaskModel>()
    {
        new TaskModel(){Id = 1, Title = "Task 1", IsCompleted = false},
        new TaskModel(){Id = 2, Title = "Task 2", IsCompleted = false},
        new TaskModel(){Id = 3, Title = "Task 3", IsCompleted = false},
    };
    public TaskModel GetTaskById(int id) => taskList.FirstOrDefault(x => x.Id == id);
    public async Task<TaskModel> CreateNewTaskAsync(TaskModel newTask)
    {
        newTask.Id = taskList.OrderBy(x => x.Id).Select(x => x.Id).LastOrDefault() + 1;
        taskList.Add(newTask);
        return await Task.FromResult(newTask);
    }
    public async Task<List<TaskModel>> CreateMultipleTasksAsync(List<TaskModel> tasks)
    {
        List<TaskModel> newTaskList = new List<TaskModel>();
        foreach (TaskModel task in tasks)
        {
            task.Id = taskList.OrderBy(x => x.Id).Select(x => x.Id).LastOrDefault() + 1;
            taskList.Add(task);
            newTaskList.Add(task);
        }
        return newTaskList;
    }

    public async Task<List<string>> DeleteMultipleTasksAsync(List<int> id)
    {
        List<string> result = new List<string>();
        foreach (var taskId in id)
        {
            var currentTask = GetTaskById(taskId);
            if (currentTask != null)
            {
                await Task.FromResult(taskList.Remove(currentTask));
                result.Add("Delete succesfully");
            }else{
                result.Add("The chosen task is already deleted or doesn't exist!");
            }
        }
        return result;
    }

    public async Task<bool> DeleteTaskAsync(int id)
    {
        var task = GetTaskById(id);
        if (task != null)
        {
            return await Task.FromResult(taskList.Remove(task));
        }
        return false;
    }

    public async Task<TaskModel> EditTaskAsync(int id, TaskModel task)
    {
        var currentTask = GetTaskById(id);
        if (currentTask != null)
        {
            currentTask.Title = task.Title;
            currentTask.IsCompleted = task.IsCompleted;
            return currentTask;
        }
        return null;
    }

    public async Task<TaskModel> GetSpecificTaskAsync(int id)
    {
        var task = await Task.FromResult(GetTaskById(id));
        if (task != null)
        {
            return task;
        }
        return null;
    }

    public async Task<List<TaskModel>> GetTaskListAsync()
    {
        return await Task.FromResult(taskList.ToList());
    }
}