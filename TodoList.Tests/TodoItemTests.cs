using Microsoft.VisualStudio.TestTools.UnitTesting;
using TodoList.Models;
using TodoList.API.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;

[TestClass]
public class ToDoItemTests
{
    
private TodoContext GetSQLiteTodoContext()
{
    var databasePath = "/workspaces/TodoList/TodoList.API/TodoList.db";
    var connectionString = $"Data Source={databasePath}";

    var options = new DbContextOptionsBuilder<TodoContext>()
        .UseSqlite(connectionString)
        .Options;

    var context = new TodoContext(options);
    return context;
}

// Test Adding a New ToDoItem
[TestMethod]
public void AddNewToDoItem()
{
    using var context = GetSQLiteTodoContext();
    var newItem = new ToDoItem { Title = "Go to gym", IsComplete = false };
    context.ToDoItems.Add(newItem);
    context.SaveChanges();

    var item = context.ToDoItems.FirstOrDefault(t => t.Title == "Go to gym");
    Assert.IsNotNull(item);
    Assert.AreEqual("Go to gym", item.Title);
    Assert.IsFalse(item.IsComplete);
}
// Test Retrieving a ToDoItem by ID
[TestMethod]
public void RetrieveToDoItemById()
{
    using var context = GetSQLiteTodoContext();
    var newItem = new ToDoItem { Title = "Learn Entity Framework", IsComplete = false };
    context.ToDoItems.Add(newItem);
    context.SaveChanges();

    var item = context.ToDoItems.Find(newItem.Id);
    Assert.IsNotNull(item);
    Assert.AreEqual("Learn Entity Framework", item.Title);
}
// Test Listing Incomplete ToDoItems
[TestMethod]
public void ListIncompleteToDoItems()
{
    using var context = GetSQLiteTodoContext();
    // Ensuring there's at least one incomplete item
    context.ToDoItems.AddRange(
        new ToDoItem { Title = "Incomplete Item", IsComplete = false },
        new ToDoItem { Title = "Completed Item", IsComplete = true, CompletedDate = DateTime.Now }
    );
    context.SaveChanges();

    var incompleteItems = context.ToDoItems.Where(t => !t.IsComplete).ToList();
    Assert.IsTrue(incompleteItems.All(t => !t.IsComplete));
}
// Test Marking a ToDoItem as Complete
[TestMethod]
public void MarkToDoItemAsComplete()
{
    using var context = GetSQLiteTodoContext();
    var newItem = new ToDoItem { Title = "Update unit tests", IsComplete = false };
    context.ToDoItems.Add(newItem);
    context.SaveChanges();

    var itemToUpdate = context.ToDoItems.FirstOrDefault(t => t.Title == "Update unit tests");
    if (itemToUpdate != null)
    {
        itemToUpdate.IsComplete = true;
        itemToUpdate.CompletedDate = DateTime.Now;
        context.SaveChanges();
    }

    var updatedItem = context.ToDoItems.Find(itemToUpdate?.Id);
    Assert.IsNotNull(updatedItem);
    Assert.IsTrue(updatedItem.IsComplete);
    Assert.IsNotNull(updatedItem.CompletedDate);
}


/*
I have commented this test method as this will clear the database after the above test are perfomed so that it will not
fill the database for every test. So i created this test method to clear the data and check if the data has been cleared 
or not. Feel free to try this.
*/
//    [TestMethod]
// public void ClearDatabase()
// {
//     using var context = GetSQLiteTodoContext();
    
//     // Remove all ToDoItems
//     var allItems = context.ToDoItems.ToList();
//     context.ToDoItems.RemoveRange(allItems);
//     context.SaveChanges();

//     // Verify the database is cleared
//     Assert.AreEqual(0, context.ToDoItems.Count());
// }

}
