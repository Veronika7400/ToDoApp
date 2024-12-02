using Microsoft.AspNetCore.Mvc.RazorPages;
using ToDoApp.Application.TodoItems.Queries.GetTodoItems;

namespace ToDoApp.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ISender _sender;

        public IEnumerable<TodoItemBriefDto> Todos { get; set; } = Enumerable.Empty<TodoItemBriefDto>();

        public IEnumerable<string> Errors { get; set; } = Enumerable.Empty<string>();

        public IndexModel(ISender sender)
        {
            _sender = sender;

            if (TempData is not null && TempData.TryGetValue("Errors", out var errors))
            {
                if (errors is not null)
                {
                    Errors = (errors as string[])!;
                }
            }
        }

        public async Task OnGetAsync(CancellationToken cancellationToken = default)
        {
            Todos = await _sender.Send(new GetTodoItemsQuery(), cancellationToken);
        }
    }
}
