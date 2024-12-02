using Microsoft.AspNetCore.Mvc;
using ToDoApp.Application.TodoItems.Commands.CreateTodoItem;
using ToDoApp.Application.Common.Exceptions;
using static ToDoApp.Application.TodoItems.Commands.DeleteTodoItems.DeleteToDoItem;
using ToDoApp.Application.TodoItems.Commands.CompleteTodoItems;

namespace ToDoApp.Web.Controllers;

public class ToDoItemController : Controller
{
    private readonly ISender _sender;

    public ToDoItemController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Handles the creation of a new ToDo item.
    /// </summary>
    /// <param name="command">The command containing details for the new ToDo item</param>
    /// <param name="cancellationToken">Token for cancellation of the operation if needed</param>
    /// <returns>Redirect to the home page</returns>
    [HttpPost]
    public async Task<IActionResult> Create(CreateTodoItemCommand command, CancellationToken cancellationToken)
    {
        try
        {
            await _sender.Send(command, cancellationToken);
        }
        catch (ValidationException ex)
        {
            printErrors(ex);
            return Redirect("/");
        }
        return Redirect("/");
    }

    /// <summary>
    /// Handles the completion of one or more ToDo items.
    /// </summary>
    /// <param name="id">List of IDs of the ToDo items to be marked as completed</param>
    /// <param name="cancellationToken">Token for cancellation of the operation if needed</param>
    /// <returns>Redirect to the home page</returns>
    [HttpPost]
    public async Task<IActionResult> Complete(List<int> id, CancellationToken cancellationToken)
    {
        try
        {
            var command = new CompleteTodoItemCommand { IdList = id };
            await _sender.Send(command, cancellationToken);
        }
        catch (ValidationException ex)
        {
            printErrors(ex);
            return Redirect("/");
        }
        return Redirect("/");
    }

    /// <summary>
    /// Handles the deletion of one or more ToDo items.
    /// </summary>
    /// <param name="id">List of IDs of the ToDo items to be deleted</param>
    /// <param name="cancellationToken">Token for cancellation of the operation if needed</param>
    /// <returns>Redirect to the home page</returns>
    [HttpPost]
    public async Task<IActionResult> Delete(List<int> id, CancellationToken cancellationToken)
    {
        try
        {
            var command = new DeleteTodoItemCommand { IdList = id };
            await _sender.Send(command, cancellationToken);
        }
        catch (ValidationException ex)
        {
            printErrors(ex);
            return Redirect("/");
        }

        return Redirect("/");
    }

    /// <summary>
    /// Method to handle printing errors and storing them in TempData.
    /// </summary>
    /// <param name="ex">The validation exception containing the errors</param>
    private void printErrors(ValidationException ex)
    {
        var allErrors = ex.Errors
                        .SelectMany(kv => kv.Value)
                        .ToList();

        TempData["Errors"] = string.Join(", ", allErrors);
    }
}
