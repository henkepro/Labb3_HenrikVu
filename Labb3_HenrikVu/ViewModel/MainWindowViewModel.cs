using Labb3_HenrikVu.Model;
using Labb3_HenrikVu.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Shapes;

namespace Labb3_HenrikVu.ViewModel
{
    internal class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<QuestionPackViewModel> ListOfQuestionPacks { get; set; }
        public ConfigurationViewModel ConfigurationViewModel { get; set; }
        public PlayerViewModel PlayerViewModel { get; set; }
        private QuestionPackViewModel? _activePack;
        private Question _selectedQuestion;
        public bool CanClickPlay { get => ConfigurationViewModel.CanClickPlay; set => ConfigurationViewModel.CanClickPlay = value; }
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
            LoadJsonFileIfNotNull();
            ConfigurationViewModel = new ConfigurationViewModel(this);
            PlayerViewModel = new PlayerViewModel(this);
        }
        private async Task LoadJsonFileIfNotNull()
        {
            ListOfQuestionPacks = new ObservableCollection<QuestionPackViewModel>();
            string path = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\Labb3_HenrikVu\\ListOfActivePacks.json";
            if(File.Exists(path))
            {
                string myJson = File.ReadAllText(path);
                ListOfQuestionPacks = JsonSerializer.Deserialize<ObservableCollection<QuestionPackViewModel>>(myJson);
                ActivePack = ListOfQuestionPacks.LastOrDefault();
            }
            else
            {
                ActivePack = new QuestionPackViewModel(new QuestionPack("My Question Pack"));
                ActivePack.Questions.Add(new Question("Sample Question"));
                ListOfQuestionPacks.Add(ActivePack);
            }

            SelectedQuestion = ActivePack.Questions.LastOrDefault();

            RaisePropertyChanged("SelectedQuestion");
            RaisePropertyChanged("ActivePack");
            RaisePropertyChanged("ListOfQuestionPacks");
        }
        public void ToggleSwapActiveWindow()
        {
            PlayerViewModel.dispatchTimer.Stop();
            ConfigurationViewModel.KeepActiveWindow = !ConfigurationViewModel.KeepActiveWindow;
            PlayerViewModel.KeepActiveWindow = !PlayerViewModel.KeepActiveWindow;
            RaisePropertyChanged("KeepActiveWindow");
        }
    }
}
