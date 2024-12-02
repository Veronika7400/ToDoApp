using ToDoApp.Application.Common.Interfaces;
using ToDoApp.Domain.Events;

namespace ToDoApp.Application.TodoItems.Commands.DeleteTodoItems;
public class DeleteToDoItem
{
    /// <summary>
    /// Record contains a list of item IDs to delete.
    /// </summary>
    public record DeleteTodoItemCommand : IRequest<int>
    {
        public List<int>? IdList { get; init; }
    }

    public class DeleteTodoItemCommandHandler : IRequestHandler<DeleteTodoItemCommand, int>
    {
        private readonly IApplicationDbContext _context;
        private readonly IValidator<DeleteTodoItemCommand> _validator;

        public DeleteTodoItemCommandHandler(IApplicationDbContext context, IValidator<DeleteTodoItemCommand> validator)
        {
            _context = context;
            _validator = validator;
        }

        /// <summary>
        /// Handles the command to delete todo items.
        /// It validates the request, checks if the items exist in the database, removes them if found, and raises an event for each deleted item.
        /// </summary>
        /// <param name="request">The command containing the list of IDs to delete.</param>
        /// <param name="cancellationToken">A cancellation token to handle task cancellation.</param>
        /// <returns>A Task that returns an integer 0 indicating success.</returns>
        /// <exception cref="ValidationException">Thrown if the validation of the request fails.</exception>
        public async Task<int> Handle(DeleteTodoItemCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            if (request.IdList != null && request.IdList.Any())
            {
                foreach (var id in request.IdList)
                {
                    var todoItem = await _context.TodoItems.FindAsync(id);
                    if (todoItem == null)
                    {
                        continue;
                    }

                    _context.TodoItems.Remove(todoItem);
                    await _context.SaveChangesAsync(cancellationToken);
                    todoItem.AddDomainEvent(new TodoItemDeletedEvent(todoItem));
                }
                await _context.SaveChangesAsync(cancellationToken);
            }
            return await Task.FromResult(0);
        }
    }
}
