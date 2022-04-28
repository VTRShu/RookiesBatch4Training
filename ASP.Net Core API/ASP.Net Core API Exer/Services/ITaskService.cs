using ASP.Net_Core_API_Exer.Models;

namespace ASP.Net_Core_API_Exer.Services;
public interface ITaskService
{
  Task<List<TaskModel>> GetTaskListAsync();
  Task<TaskModel> CreateNewTaskAsync(TaskModel newTask);
  Task<TaskModel> EditTaskAsync(int id, TaskModel task);
  Task<bool> DeleteTaskAsync(int id);
  Task<TaskModel> GetSpecificTaskAsync(int id);
  Task<List<TaskModel>> CreateMultipleTasksAsync(List<TaskModel> tasks);
  Task<List<string>> DeleteMultipleTasksAsync(List<int> id);
}