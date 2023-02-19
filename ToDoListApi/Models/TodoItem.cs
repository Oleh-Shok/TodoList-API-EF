using System.ComponentModel.DataAnnotations;

namespace ToDoListApi.Models;
public class TodoItem
{
    [Key]
    public int TaskId { get; set ; }
    public string? TaskDescription { get; set; }
    public bool IsComplete { get; set; }
}





