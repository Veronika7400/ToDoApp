using static ToDoApp.Application.TodoItems.Commands.DeleteTodoItems.DeleteToDoItem;

namespace ToDoApp.Application.TodoItems.Commands.DeleteTodoItems;
public class DeleteTodoItemCommandValidator : AbstractValidator<DeleteTodoItemCommand>
{
    /// <summary>
    /// This class validates the CompleteTodoItemCommand to ensure that the provided data is correct.
    /// Check if the IdList is not empty and ensure that all IDs in the IdList are positive integers
    /// </summary>
    public DeleteTodoItemCommandValidator()
    {
        RuleFor(v => v.IdList)
             .NotEmpty().WithMessage("You must select at least one task.")
             .Must(ids => ids != null && ids.All(id => id > 0))
             .WithMessage("Each ID in the list must be a positive number greater than zero.");
    }
}
