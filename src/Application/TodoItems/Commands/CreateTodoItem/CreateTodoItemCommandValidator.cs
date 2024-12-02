namespace ToDoApp.Application.TodoItems.Commands.CreateTodoItem;

public class CreateTodoItemCommandValidator : AbstractValidator<CreateTodoItemCommand>
{
    /// <summary>
    /// This class validates the CreateTodoItemCommand to ensure that the provided data is correct.
    /// Check if the title is not empty and ensure that title is not longer than 200 characters
    /// </summary>
    public CreateTodoItemCommandValidator()
    {
        RuleFor(v => v.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("The title must not be longer than 200 characters.");
    }
}
