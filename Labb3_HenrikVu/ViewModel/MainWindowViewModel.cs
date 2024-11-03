using Labb3_HenrikVu.Model;
using Labb3_HenrikVu.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb3_HenrikVu.ViewModel
{
    internal class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<QuestionPackViewModel> Packs { get; set; }
        public ObservableCollection<QuestionPackViewModel> ListOfQuestionPacks { get; set; }
        public ConfigurationViewModel ConfigurationViewModel { get; }
        public PlayerViewModel PlayerViewModel { get; }
        private QuestionPackViewModel? _activePack;

        private Question _selectedQuestion;
        public Question SelectedQuestion
        {
            get => _selectedQuestion;
            set
            {
                _selectedQuestion = value;
                RaisePropertyChanged();
            }
        }
        public QuestionPackViewModel? ActivePack
		{
			get => _activePack;
			set 
			{ 
				_activePack = value;
				RaisePropertyChanged();
                ConfigurationViewModel?.RaisePropertyChanged("ActivePack");
			}
		}
        public MainWindowViewModel()
        {
            ListOfQuestionPacks = new ObservableCollection<QuestionPackViewModel>();
            ActivePack = new QuestionPackViewModel(new QuestionPack("My Question Pack"));
            ActivePack.Questions.Add(new Question("Sample Question"));
            ListOfQuestionPacks.Add(ActivePack);
            SelectedQuestion = ActivePack.Questions.LastOrDefault();

            ConfigurationViewModel = new ConfigurationViewModel(this);
            PlayerViewModel = new PlayerViewModel(this);

            RaisePropertyChanged("SelectedQuestion");
            RaisePropertyChanged("ActivePack");
            RaisePropertyChanged("ListOfQuestionPacks");
        }
        public void ToggleSwapActiveWindow()
        {
            ConfigurationViewModel.KeepActiveWindow = !ConfigurationViewModel.KeepActiveWindow;
            PlayerViewModel.KeepActiveWindow = !PlayerViewModel.KeepActiveWindow;
            RaisePropertyChanged("KeepActiveWindow");
        }
    }
}
