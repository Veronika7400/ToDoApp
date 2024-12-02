using ToDoApp.Application.Common.Interfaces;

namespace ToDoApp.Application.TodoItems.Queries.GetTodoItems;

public record GetTodoItemsQuery : IRequest<IEnumerable<TodoItemBriefDto>>
{
    public int ListId { get; set; } = 1;
}

public class GetTodoItemsQueryHandler : IRequestHandler<GetTodoItemsQuery, IEnumerable<TodoItemBriefDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetTodoItemsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TodoItemBriefDto>> Handle(GetTodoItemsQuery request, CancellationToken cancellationToken)
    {
        return await _context.TodoItems
            .Where(x => x.ListId == request.ListId)
            .OrderBy(x => x.Title)
            .ProjectTo<TodoItemBriefDto>(_mapper.ConfigurationProvider)
            .ToArrayAsync(cancellationToken);
    }
}
