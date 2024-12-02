namespace ToDoApp.Application.TodoItems.Commands.CreateTodoItem;

public record CreateTodoItemCommand : IRequest<int>
{
    public int ListId { get; init; }

    public string? Title { get; init; }
}

public class CreateTodoItemCommandHandler : IRequestHandler<CreateTodoItemCommand, int>
{
    public Task<int> Handle(CreateTodoItemCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(0);
    }
}
