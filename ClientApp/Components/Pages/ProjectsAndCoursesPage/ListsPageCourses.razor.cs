using ClientApp.Components.TableView;
using ClientApp.Models;
using ClientApp.Services.ServiceModels;
using UILayer.Enums;

namespace ClientApp.Components.Pages.ProjectsAndCoursesPage;

public partial class ListsPage
{
    private async Task ReloadCoursesTableDataAsync(bool resetToFirstPage)
    {
        var page = resetToFirstPage ? 0 : _courseTableView.CurrentPage;
        var sortingOption = _courseTableView.SelectedSortedOption ?? _courseSortingOptions[0];
        var response = await CourseService.GetFilteredAsync(new FilteredListRequest
        {
            Skip = page * _courseTableView.ItemsPerPage,
            Take = _courseTableView.ItemsPerPage,
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
        
        _courseTableView.ReloadDisplayList(response.Item!.Items.ToArray(), response.Item.WithoutPagingCount, resetToFirstPage);
        SaveQueryParameters();
    }
    
    private Task DeleteCourseButtonClicked(ProbationCourse obj)
    {
        if (obj.InternsCount > 0)
        {
            _popup.ShowError($"Выбранное направление \"{obj.Title}\" нельзя удалить, так как к нему привязаны стажеры.");
            return Task.CompletedTask;
        }
        _popup.ShowWithObject("Подтверждение", $"Вы уверены, что хотите удалить направление \"{obj.Title}\"?", 
            obj, OnAnsweredPopupDeleteCourse);
        return Task.CompletedTask;
    }

    private async Task EditCourseButtonClicked(ProbationCourse obj)
    {
        await _courseEditPopup.ShowAsync(obj);
    }
    
    private async Task SearchOnCourses(string obj)
    {
        await ReloadCoursesTableDataAsync(true);
        SaveQueryParameters();
    }

    private async Task OnAnsweredPopupDeleteCourse(PopupAnswer answer, ProbationCourse course)
    {
        if (answer != PopupAnswer.Yes)
        {
            return;
        }

        await CourseService.DeleteAsync(course.Id);
        await ReloadCoursesTableDataAsync(false);
    }

    private async Task OnCourseSavePopup(ProbationCourse obj)
    {
        await ReloadCoursesTableDataAsync(false);
    }

    private async Task CreateNewCourseBtnClicked()
    {
        await _courseEditPopup.ShowAsync();
        
    }
    
    private readonly TableSortingOption<ProbationCourse>[] _courseSortingOptions = new[]
    {
        new TableSortingOption<ProbationCourse>
        {
            Title = "Название \u25bc",
            ByDescending = true,
            PropertyName = "Title"
        },
        new TableSortingOption<ProbationCourse>
        {
            Title = "Название \u25b2",
            ByDescending = false,
            PropertyName = "Title"
        },
        new TableSortingOption<ProbationCourse>
        {
            Title = "Кол-во стажеров \u25bc",
            ByDescending = true,
            PropertyName = "InternsCount"
        },
        new TableSortingOption<ProbationCourse>
        {
            Title = "Кол-во стажеров \u25b2",
            ByDescending = false,
            PropertyName = "InternsCount"
        },
    };
}