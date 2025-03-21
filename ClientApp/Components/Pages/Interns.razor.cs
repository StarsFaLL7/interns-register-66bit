using ClientApp.Components.TableView;
using ClientApp.Models;
using ClientApp.Services.ServiceModels;
using UILayer.Enums;

namespace ClientApp.Components.Pages;

public partial class Interns
{
    private async Task Search(string arg)
    {
        await ReloadTableDataAsync(true);
    }
    
    private Task DeleteInternBtnClicked(Intern arg)
    {
        _popup.ShowWithObject("Подтверждение", $"Вы уверены, что хотите удалить стажера {arg.LastName} {arg.FirstName}?", 
            arg, DeleteInternPopupAnswered);
        return Task.CompletedTask;
    }

    private async Task DeleteInternPopupAnswered(PopupAnswer answer, Intern intern)
    {
        if (answer != PopupAnswer.Yes)
        {
            return;
        }
        await InternService.DeleteAsync(intern.Id);
        await ReloadTableDataAsync(false);
    }
    
    private async Task ReloadTableDataAsync(bool resetToFirstPage)
    {
        var page = resetToFirstPage ? 0 : _tableView.CurrentPage;
        var sortingOption = _tableView.SelectedSortedOption ?? _internSortingOptions[0];
        var response = await InternService.GetFilteredAsync(new FilteredListRequest
        {
            Skip = page * _tableView.ItemsPerPage,
            Take = _tableView.ItemsPerPage,
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
        
        _tableView.ReloadDisplayList(response.Item!.Items.ToArray(), response.Item.WithoutPagingCount, resetToFirstPage);
    }

    private async Task EditInternBtnClicked(Intern arg)
    {
        await _editInternPopup.ShowAsync(arg);
    }

    private async Task EditPopupSaved(Intern arg)
    {
        await ReloadTableDataAsync(false);
    }
    
    private readonly TableSortingOption<Intern>[] _internSortingOptions = new []
    {
        new TableSortingOption<Intern>
        {
            Title = "Фамилия \u25bc",
            ByDescending = true,
            PropertyName = "LastName"
        },
        new TableSortingOption<Intern>
        {
            Title = "Фамилия \u25b2",
            ByDescending = false,
            PropertyName = "LastName"
        },
        new TableSortingOption<Intern>
        {
            Title = "Проект \u25bc",
            ByDescending = true,
            PropertyName = "ProbationProject.Title"
        },
        new TableSortingOption<Intern>
        {
            Title = "Проект \u25b2",
            ByDescending = false,
            PropertyName = "ProbationProject.Title"
        },
        new TableSortingOption<Intern>
        {
            Title = "Направление \u25bc",
            ByDescending = true,
            PropertyName = "ProbationCourse.Title"
        },
        new TableSortingOption<Intern>
        {
            Title = "Направление \u25b2",
            ByDescending = false,
            PropertyName = "ProbationCourse.Title"
        },
        new TableSortingOption<Intern>
        {
            Title = "Дата рождения \u25bc",
            ByDescending = true,
            PropertyName = "BirthDate"
        },
        new TableSortingOption<Intern>
        {
            Title = "Дата рождения \u25b2",
            ByDescending = false,
            PropertyName = "BirthDate"
        }
    };
}