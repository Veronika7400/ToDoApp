
namespace ToDoApp.Application.TodoItems.Commands.CompleteTodoItems;
public class CompleteTodoItemCommandValidator : AbstractValidator<CompleteTodoItemCommand>
{
    /// <summary>
    /// This class validates the CompleteTodoItemCommand to ensure that the provided data is correct.
    /// Check if the IdList is not empty and ensure that all IDs in the IdList are positive integers
    /// </summary>
    public CompleteTodoItemCommandValidator()
    {
        RuleFor(v => v.IdList)
             .NotEmpty().WithMessage("You must select at least one task.")
             .Must(ids => ids != null && ids.All(id => id > 0)) 
             .WithMessage("Each ID in the list must be a positive number greater than zero.");
    }
}
