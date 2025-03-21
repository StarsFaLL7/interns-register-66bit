using System.Net;
using ClientApp.Models;
using ClientApp.Utils;

namespace ClientApp.Components.Pages;

public partial class AddIntern
{
    protected override async Task OnInitializedAsync()
    {
        var allProjectsResult = await ProjectService.GetAllAsync();
        _ddProjectSelect.AddItemRange(allProjectsResult.Item ?? []);
        
        var allCoursesResult = await CourseService.GetAllAsync();
        _ddCourseSelect.AddItemRange(allCoursesResult.Item ?? []);
    }

    private async Task SaveInternBtnClicked()
    {
        _isShouldValidate = true;
        if (!ValidateFields())
        {
            return;
        }

        _editingModel.LastName = _editingModel.LastName;
        _editingModel.FirstName = _editingModel.FirstName;
        var createRes = await InternService.CreateAsync(_editingModel);
        if (createRes.Completed)
        {
            _popup.ShowSuccess("Новый стажер успешно добавлен");
            _editingModel = new Intern { BirthDate = DateTime.Today };
            _isShouldValidate = false;
        }
        else
        {
            _popup.ShowError($"Не удалось добавить нового стажера. {createRes.Message}");
        }
    }

    private bool ValidateFields()
    {
        if (string.IsNullOrWhiteSpace(_editingModel.FirstName) ||
            string.IsNullOrWhiteSpace(_editingModel.LastName) ||
            ValidationHelper.ValidatePhone(_editingModel.Phone!, true, true) == "is-invalid" ||
            ValidationHelper.ValidateEmail(_editingModel.Email, true) == "is-invalid" ||
            ValidationHelper.ValidateDateInBetween(_editingModel.BirthDate, new DateTime(1900, 1, 1),
                DateTime.Today) == "is-invalid")
        {
            return false;
        }

        return true;
    }

    private void ProjectSelectionChanged(ProbationProject? obj)
    {
        if (obj == null)
        {
            _editingModel.ProbationProject = null;
            return;
        }
        _editingModel.ProbationProject = new BriefProjectInfo
        {
            Id = obj.Id,
            Title = obj.Title
        };
    }

    private void CourseSelectionChanged(ProbationCourse? obj)
    {
        if (obj == null)
        {
            _editingModel.ProbationCourse = null;
            return;
        }
        _editingModel.ProbationCourse = new BriefCourseInfo()
        {
            Id = obj.Id,
            Title = obj.Title
        };
    }

    private void GenderSelectionChanged(bool obj)
    {
        _editingModel.IsMale = obj;
    }

    private async Task CreateNewCourseBtnClicked()
    {
        await _editCoursePopup.ShowAsync();
    }

    private async Task CreateNewProjectBtnClicked()
    {
        await _editProjectPopup.ShowAsync();
    }
    
    private Task CourseEditPopupOnSaveAction(ProbationCourse obj)
    {
        if (obj == null)
        {
            _popup.ShowError("Произошла ошибка во время сохранения направления.");
            return Task.CompletedTask;
        }
        _ddCourseSelect.AddItem(obj);
        _ddCourseSelect.SelectItem(obj, true);
        return Task.CompletedTask;
    }

    private Task ProjectEditPopupOnSaveAction(ProbationProject obj)
    {
        if (obj == null)
        {
            _popup.ShowError("Произошла ошибка во время сохранения проекта.");
            return Task.CompletedTask;
        }
        _ddProjectSelect.AddItem(obj);
        _ddProjectSelect.SelectItem(obj, true);
        return Task.CompletedTask;
    }
}