using ASP.Net_Core_API_Exer.Models;
using ASP.Net_Core_API_Exer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Net_Core_API_Exer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaskController : ControllerBase
{
    private readonly ITaskService _taskService;
    public TaskController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet("list")]
    public async Task<IActionResult> Tasks()
    {
        return Ok(await _taskService.GetTaskListAsync());
    }

    [HttpGet("details")]
    public async Task<IActionResult> Task(int id)
    {
        var result = await _taskService.GetSpecificTaskAsync(id);
        if (result != null)
        {
            return Ok(result);
        }
        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Create(TaskModel newTask)
    {
        if (ModelState.IsValid)
        {
            return Ok(await _taskService.CreateNewTaskAsync(newTask));
        }
        return BadRequest("Something went wrong");
    }

    [HttpPut]
    public async Task<IActionResult> Edit(int id, TaskModel task)
    {
        if (ModelState.IsValid)
        {
            var currentTask = await _taskService.EditTaskAsync(id, task);
            if (currentTask != null)
            {
                return Ok(currentTask);
            }
            return BadRequest();
        }
        return BadRequest();
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _taskService.DeleteTaskAsync(id);
        if (result == true)
        {
            return Ok(result);
        }
        return BadRequest();
    }

    [HttpDelete("multiple-tasks")]
    public async Task<IActionResult> MultipleTasks(List<int> idList)
    {
        return Ok(await _taskService.DeleteMultipleTasksAsync(idList));
    }

    [HttpPost("multiple-tasks")]
    public async Task<IActionResult> MultipleTasks(List<TaskModel> tasks)
    {
        return Ok(await _taskService.CreateMultipleTasksAsync(tasks));
    }
}
