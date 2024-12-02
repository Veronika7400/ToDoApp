using ToDoApp.Application.Common.Interfaces;
using ToDoApp.Domain.Entities;
using ToDoApp.Domain.Events;

namespace ToDoApp.Application.TodoItems.Commands.CreateTodoItem;

/// <summary>
/// Contains the properties necessary to create a Todo item in the system.
/// </summary>
public record CreateTodoItemCommand : IRequest<int>
{
    public int ListId { get; init; }

    public string? Title { get; init; }
}

public class CreateTodoItemCommandHandler : IRequestHandler<CreateTodoItemCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IValidator<CreateTodoItemCommand> _validator;

    public CreateTodoItemCommandHandler(IApplicationDbContext context, IValidator<CreateTodoItemCommand> validator)
    {
        _context = context;
        _validator = validator;
    }

    /// <summary>
    /// Handles the creation of a new Todo item. 
    /// Validates the request, creates a new Todo item in the database and triggers an event that a Todo item has been created.
    /// </summary>
    /// <param name="request">The command containing the Todo item details</param>
    /// <param name="cancellationToken">Token to cancel the operation if needed</param>
    /// <returns>A Task that returns an integer 0 indicating success.</returns>
    /// <exception cref="ValidationException">Thrown if validation of the request fails</exception>
    public async Task<int> Handle(CreateTodoItemCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var todoItem = new TodoItem
        {
            ListId = request.ListId,
            Title = request.Title,
            Done = false
        };

        _context.TodoItems.Add(todoItem);
        await _context.SaveChangesAsync(cancellationToken);

        todoItem.AddDomainEvent(new TodoItemCreatedEvent(todoItem));

        return await Task.FromResult(0);
    }

}
