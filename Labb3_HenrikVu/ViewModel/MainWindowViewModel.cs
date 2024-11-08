using Labb3_HenrikVu.Model;
using Labb3_HenrikVu.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
            ListOfQuestionPacks = new ObservableCollection<QuestionPackViewModel>();
            ActivePack = new QuestionPackViewModel(new QuestionPack("My Question Pack"));
            ActivePack.Questions.Add(new Question("Sample Question"));
            ListOfQuestionPacks.Add(ActivePack);
            SelectedQuestion = ActivePack.Questions.LastOrDefault();
            LoadJsonFileIfNotNull();
            ConfigurationViewModel = new ConfigurationViewModel(this);
            PlayerViewModel = new PlayerViewModel(this);
        }
        public bool isLoadingFile = false;
        private async Task LoadJsonFileIfNotNull()
        {
            string path = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\Labb3_HenrikVu\\ListOfActivePacks.json";
            if(File.Exists(path))
            {
                isLoadingFile = true;
                Debug.WriteLine("Json File Loading");
                string myJson = await File.ReadAllTextAsync(path);
                var hej = JsonSerializer.Deserialize<ObservableCollection<QuestionPackViewModel>>(myJson);
                ListOfQuestionPacks = hej;
                ActivePack = new QuestionPackViewModel(new QuestionPack("My Question Pack"));
                ActivePack = ListOfQuestionPacks.LastOrDefault();
                SelectedQuestion = ActivePack.Questions.LastOrDefault();

                Debug.WriteLine("Json File Loaded");
                RaisePropertyChanged("SelectedQuestion");
                RaisePropertyChanged("ActivePack");
                RaisePropertyChanged("ListOfQuestionPacks");
                isLoadingFile = false;
            }
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
