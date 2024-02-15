
namespace TodoList.Models
{
    public class ToDoItem
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public bool IsComplete { get; set; }
        public DateTime? CompletedDate { get; set; }
    }
}
