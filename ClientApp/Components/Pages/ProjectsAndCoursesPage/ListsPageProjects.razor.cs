using ClientApp.Components.TableView;
using ClientApp.Models;
using ClientApp.Services.ServiceModels;
using UILayer.Enums;

namespace ClientApp.Components.Pages.ProjectsAndCoursesPage;

public partial class ListsPage
{
    private async Task ReloadProjectsTableDataAsync(bool resetToFirstPage)
    {
        var page = resetToFirstPage ? 0 : _projectTableView.CurrentPage;
        var sortingOption = _projectTableView.SelectedSortedOption ?? _projectSortingOptions[0];
        var response = await ProjectService.GetFilteredAsync(new FilteredListRequest
        {
            Skip = page * _projectTableView.ItemsPerPage,
            Take = _projectTableView.ItemsPerPage,
            OrderBy = sortingOption.PropertyName,
            Ascending = !sortingOption.ByDescending,
            Search = _searchInput.Text,
            AdditionalQueryParams = []
        });
        if (!response.Completed)
        {
            _popup.ShowError("Возникла ошибка во время получения данных с сервера.");
            return;
        }
        
        _projectTableView.ReloadDisplayList(response.Item!.Items.ToArray(), response.Item.WithoutPagingCount, resetToFirstPage);
        SaveQueryParameters();
    }
    
    private Task DeleteProjectButtonClicked(ProbationProject obj)
    {
        if (obj.InternsCount > 0)
        {
            _popup.ShowError($"Выбранный проект \"{obj.Title}\" нельзя удалить, так как к нему привязаны стажеры.");
            return Task.CompletedTask;
        }
        _popup.ShowWithObject("Подтверждение", $"Вы уверены, что хотите удалить проект \"{obj.Title}\"?", 
            obj, OnAnsweredPopupDeleteProject);
        return Task.CompletedTask;
    }

    private async Task EditProjectButtonClicked(ProbationProject obj)
    {
        await _projectEditPopup.ShowAsync(obj);
    }
    
    private async Task SearchOnProjects(string obj)
    {
        await ReloadProjectsTableDataAsync(true);
        SaveQueryParameters();
    }

    private async Task OnAnsweredPopupDeleteProject(PopupAnswer answer, ProbationProject project)
    {
        if (answer != PopupAnswer.Yes)
        {
            return;
        }
        await ProjectService.DeleteAsync(project.Id);
        await ReloadProjectsTableDataAsync(false);
    }

    private async Task OnProjectSavePopup(ProbationProject obj)
    {
        await ReloadProjectsTableDataAsync(false);
    }

    private async Task CreateNewProjectBtnClicked()
    {
        await _projectEditPopup.ShowAsync();
        
    }
    
    private readonly TableSortingOption<ProbationProject>[] _projectSortingOptions = new[]
    {
        new TableSortingOption<ProbationProject>
        {
            Title = "Название \u25bc",
            ByDescending = true,
            PropertyName = "Title"
        },
        new TableSortingOption<ProbationProject>
        {
            Title = "Название \u25b2",
            ByDescending = false,
            PropertyName = "Title"
        },
        new TableSortingOption<ProbationProject>
        {
            Title = "Кол-во стажеров \u25bc",
            ByDescending = true,
            PropertyName = "InternsCount"
        },
        new TableSortingOption<ProbationProject>
        {
            Title = "Кол-во стажеров \u25b2",
            ByDescending = false,
            PropertyName = "InternsCount"
        },
    };
}