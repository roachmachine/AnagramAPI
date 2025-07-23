using Anagram.MobileUI.Models;
using CommunityToolkit.Mvvm.Input;

namespace Anagram.MobileUI.PageModels
{
    public interface IProjectTaskPageModel
    {
        IAsyncRelayCommand<ProjectTask> NavigateToTaskCommand { get; }
        bool IsBusy { get; }
    }
}