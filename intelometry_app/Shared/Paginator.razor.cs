using intelometry_app.Models;
using Microsoft.AspNetCore.Components;

namespace intelometry_app.Shared
{
    public class PaginatorBase : ComponentBase
    {
        protected List<PageLinkModel> links = new();

        [Parameter]
        public int CurrentPage { get; set; }

        [Parameter]
        public int CurrentPageSize { get; set; }

        [Parameter]
        public int TotalPages { get; set; }

        [Parameter]
        public int TotalRecords { get; set; }

        [Parameter]
        public int Range { get; set; } = 2;

        [Parameter]
        public EventCallback<int> OnSelectPage { get; set; }

        [Parameter]
        public EventCallback<int> OnSelectPageSize { get; set; }

        protected override void OnParametersSet()
        {
            LoadPaginator();
        }

        private void LoadPaginator()
        {
            links = new List<PageLinkModel>();

            int firstPage = 1;
            int lastPage = TotalPages;

            int nextPage = CurrentPage + 1;
            int previousPage = CurrentPage - 1;

            bool hasPreviousPage = CurrentPage > 1;
            bool hasNextPage = CurrentPage < TotalPages;

            links.Add(new PageLinkModel(firstPage, hasPreviousPage, $"\u00AB"));
            links.Add(new PageLinkModel(previousPage, hasPreviousPage, $"\u2039"));

            for (int i = 1; i <= TotalPages; i++)
            {
                if (i >= CurrentPage - Range && i <= CurrentPage + Range)
                {
                    links.Add(new PageLinkModel(i) { Active = CurrentPage == i });
                }
            }

            links.Add(new PageLinkModel(nextPage, hasNextPage, $"\u203A"));
            links.Add(new PageLinkModel(lastPage, hasNextPage, $"\u00BB"));
        }

        protected async Task SelectPage(PageLinkModel link)
        {
            if (link.Page == CurrentPage) return;
            if (!link.Enabled) return;

            CurrentPage = link.Page;
            await OnSelectPage.InvokeAsync(link.Page);
        }

        protected async Task SelectPageSize(ChangeEventArgs e)
        {
            if (e.Value is not null)
            {
                if (CurrentPageSize == Convert.ToInt32(e.Value)) return;

                CurrentPageSize = Convert.ToInt32(e.Value);
                await OnSelectPageSize.InvokeAsync(Convert.ToInt32(e.Value));
            }
        }
    }
}
