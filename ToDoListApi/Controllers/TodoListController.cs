using Microsoft.AspNetCore.Mvc;
using ToDoListApi.Models;
using Microsoft.AspNetCore.Authorization;
using ToDoListApi.Models.Exceptions;
using Microsoft.Extensions.Localization;
using ToDoListApi.Data;
using Microsoft.EntityFrameworkCore;

namespace ToDoListApi.Controllers;

[ApiController]
[Route("api/toDoList")]
public class TodoListController : ControllerBase
{        
    private readonly IStringLocalizer _resourceLocalizer;
    private readonly DataContext _context;
    public TodoListController(IStringLocalizer resourceLocalizer, DataContext context)
    {        
        _resourceLocalizer = resourceLocalizer;
        _context = context;
    }

    [Authorize]
    [HttpPost("add-task")]
    public async Task<ActionResult<List<TodoItem>>> AddTodoItem(TodoItem newTodoItem)
    {        
        _context.TodoItems.Add(newTodoItem);
        await _context.SaveChangesAsync();
        return Ok(newTodoItem);
    }

    [Authorize]
    [HttpDelete("delete-task")]
    public async Task<ActionResult<List<TodoItem>>> RemoveTodoItem(int taskId)
    {
        var dbTasks = await _context.TodoItems.FindAsync(taskId);
        if (dbTasks == null)
        {
            throw new NotFoundException(_resourceLocalizer["NotFoundTask", taskId]);
        }
        _context.TodoItems.Remove(dbTasks);
        await _context.SaveChangesAsync();
        return Ok(await _context.TodoItems.ToListAsync());
    }

    [AllowAnonymous]
    [HttpGet("get-list-of-todoitems")]
    public async Task<ActionResult<List<TodoItem>>> Get()
    {        
        return Ok(await _context.TodoItems.ToListAsync());
    }

    [AllowAnonymous]
    [HttpGet("get-list-of-todoitems-by-id")]
    public async Task<ActionResult<List<TodoItem>>> Get(int id)
    {
        var tasks = await _context.TodoItems.FindAsync(id);
        if (tasks == null)
        {
            throw new NotFoundException(_resourceLocalizer["EmptyTodoList", id]);
        }
        return Ok(tasks);
    }

    [Authorize]
    [HttpPut("update-task")]
    public async Task<ActionResult<List<TodoItem>>> UpdateTask(TodoItem updatedTask)
    {
        var dbTasks = await _context.TodoItems.FindAsync(updatedTask.TaskId);       
        if (dbTasks == null)
        {
            throw new NotFoundException(_resourceLocalizer["NotFoundTask", updatedTask]);
        }
        dbTasks.TaskId = updatedTask.TaskId;
        dbTasks.TaskDescription = updatedTask.TaskDescription;
        dbTasks.IsComplete = updatedTask.IsComplete;
        await _context.SaveChangesAsync();
        return Ok(dbTasks);
    }
}
