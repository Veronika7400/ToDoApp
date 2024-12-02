using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.Application.Common.Interfaces;
using ToDoApp.Application.TodoItems.Commands.CreateTodoItem;
using ToDoApp.Domain.Entities;
using ToDoApp.Domain.Events;


namespace ToDoApp.Application.TodoItems.Commands.CompleteTodoItems
{
    /// <summary>
    /// Record contains a list of item IDs to mark as completed.
    /// </summary>
    public record CompleteTodoItemCommand : IRequest<int>
    {
        public List<int>? IdList { get; init; }
    }

    public class CompleteTodoItemCommandHandler : IRequestHandler<CompleteTodoItemCommand, int>
    {
        private readonly IApplicationDbContext _context;
        private readonly IValidator<CompleteTodoItemCommand> _validator;

        public CompleteTodoItemCommandHandler(IApplicationDbContext context, IValidator<CompleteTodoItemCommand> validator)
        {
            _context = context;
            _validator = validator;
        }


        /// <summary>
        /// Handles the command to complete todo items.
        /// It validates the request, updates todo items, and triggers domain events for each completed item.
        /// </summary>
        /// <param name="request">The command containing the list of IDs to complete.</param>
        /// <param name="cancellationToken">A cancellation token to handle task cancellation.</param>
        /// <returns>A Task that returns an integer 0 indicating success.</returns>
        /// <exception cref="ValidationException">Thrown if the validation of the request fails.</exception>
        public async Task<int> Handle(CompleteTodoItemCommand request, CancellationToken cancellationToken)
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

                    todoItem.Done = true;
                    await _context.SaveChangesAsync(cancellationToken);
                    todoItem.AddDomainEvent(new TodoItemCompletedEvent(todoItem));
                }
            }
            return await Task.FromResult(0); 
        }
    }
}
