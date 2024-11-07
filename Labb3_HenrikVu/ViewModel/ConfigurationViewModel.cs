using Labb3_HenrikVu.Command;
using Labb3_HenrikVu.Model;
using Labb3_HenrikVu.View;
using Labb3_HenrikVu.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;
using static System.Windows.Forms.Design.AxImporter;

namespace Labb3_HenrikVu.ViewModel
{
    internal class ConfigurationViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel mainWindowViewModel;
        public DelegateCommand AddQuestionOnCommand { get; }
        public DelegateCommand CreateQuestionPackOnCommand { get; }
        public DelegateCommand RemoveQuestionOnCommand { get; }
        public DelegateCommand EditPackSettingsOnCommand { get; }
        public DelegateCommand DeleteQuestionPackOnCommand { get; }
        public DelegateCommand UpdateButtonOnCommand { get; }
        public DelegateCommand SetDefaultPackOnCommand { get; }
        public DelegateCommand ToggleFullScreenCommand { get; }
        public DelegateCommand EditQuestionsOnCommand { get; }
        public DelegateCommand OpenQuestionPackOnCommand { get; }
        public ObservableCollection<QuestionPackViewModel> ListOfQuestionPacks { get => mainWindowViewModel.ListOfQuestionPacks; }
        public QuestionPackViewModel ActivePack { get => mainWindowViewModel.ActivePack; }
        private QuestionPackViewModel _targetReference;
        private bool _canClickPlay = true;
        private bool _isFullScreen;
        private bool _keepActiveWindow = true;
        public Question SelectedQuestion 
        {
            get => mainWindowViewModel.SelectedQuestion; 
            set 
            { 
                mainWindowViewModel.SelectedQuestion = value; 
                RaisePropertyChanged();
                RaisePropertyChanged("mainWindowViewModel.SelectedQuestion");
            } 
        }
        public bool IsFullScreen
        {
            get => _isFullScreen;
            set
            {
                _isFullScreen = value;
                RaisePropertyChanged();
            }
        }
        public QuestionPackViewModel TargetReference
        {
            get => _targetReference;
            set
            {
                _targetReference = value;
                RaisePropertyChanged();
            }
        }
        public bool KeepActiveWindow
        {
            get => _keepActiveWindow;
            set
            {
                _keepActiveWindow = value;
                RaisePropertyChanged();
            }
        }
        public bool CanClickPlay
        {
            get 
            {
                if(ListOfQuestionPacks.Count == 0)
                {
                    _canClickPlay = false;
                }
                if(ActivePack.Questions.Count == 0)
                {
                    _canClickPlay = false;
                }
                return _canClickPlay; 
            }
            set
            {
                _canClickPlay = value;
                RaisePropertyChanged();
            }
        }

        public ConfigurationViewModel(MainWindowViewModel mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
            TargetReference = new QuestionPackViewModel(new QuestionPack());
            ToggleFullScreenCommand = new DelegateCommand(ToggleFullScreen);

            EditQuestionsOnCommand = new DelegateCommand(EditQuestions);
            AddQuestionOnCommand = new DelegateCommand(AddQuestion);
            RemoveQuestionOnCommand = new DelegateCommand(RemoveQuestion);

            OpenQuestionPackOnCommand = new DelegateCommand(OpenQuestionPack);
            CreateQuestionPackOnCommand = new DelegateCommand(CreateQuestionPack);
            DeleteQuestionPackOnCommand = new DelegateCommand(DeleteQuestionPack);

            if(TargetReference.Questions.Count == 0)
            {
                TargetReference.Questions.Add(new Question());
            }

            dispatchTimer = new DispatcherTimer();
            dispatchTimer.Interval = TimeSpan.FromSeconds(2);
            dispatchTimer.Tick += async (sender, e) => await SavePacksToJsonAsync(sender, e);
            dispatchTimer.Start();

            RaisePropertySpam();
        }
        private async Task SavePacksToJsonAsync(object? sender, EventArgs e)
        {
            var options = new JsonSerializerOptions()
            {
                WriteIndented = true,
                IncludeFields = true,
                IgnoreReadOnlyProperties = false,
            };
            string directoryPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\Labb3_HenrikVu";
            Directory.CreateDirectory(directoryPath);
            string path = Path.Combine(directoryPath, "ListOfActivePacks.json");

            if(ListOfQuestionPacks.Count != 0)
            {
                string json = JsonSerializer.Serialize(ListOfQuestionPacks, options);
                await File.WriteAllTextAsync(path, json);
            }
            else
            {
                var tempPack = new QuestionPackViewModel(new QuestionPack("My Question Pack"));
                tempPack.Questions.Add(new Question("Sample Question"));
                string json = JsonSerializer.Serialize(tempPack, options);
                await File.WriteAllTextAsync(path, json);
            }
        }

        private DispatcherTimer dispatchTimer;
        private void EditQuestions(object obj)
        {
            mainWindowViewModel.ToggleSwapActiveWindow();

            RaisePropertySpam();
        }
    
        private void DeleteQuestionPack(object obj)
        {
            ActivePack.Name = null;
            ActivePack.Questions.Clear();
            ListOfQuestionPacks.Remove(ActivePack);
            if(ListOfQuestionPacks.Count != 0)
            {
                mainWindowViewModel.ActivePack = ListOfQuestionPacks.LastOrDefault();
            }

            SelectedQuestion = ActivePack.Questions.LastOrDefault();

            RaisePropertySpam();
        }

        private void OpenQuestionPack(object obj)
        {
            mainWindowViewModel.ActivePack = (QuestionPackViewModel)obj;
            SelectedQuestion = ActivePack.Questions.LastOrDefault();

            RaisePropertySpam();
        }

        private void CreateQuestionPack(object obj)
        {
            mainWindowViewModel.ActivePack = TargetReference;
            var questionPackViewModel = new QuestionPackViewModel(new QuestionPack());
            TargetReference = questionPackViewModel;

            questionPackViewModel.Questions.Add(new Question("Sample Question"));

            ListOfQuestionPacks.Add(mainWindowViewModel.ActivePack);
            SelectedQuestion = ActivePack.Questions.LastOrDefault();
            CanClickPlay = true;

            RaisePropertySpam();
        }
        private void ToggleFullScreen(object obj)
        {
            IsFullScreen = !IsFullScreen;
            SelectedQuestion = ActivePack.Questions.LastOrDefault();
        }
        private void RemoveQuestion(object obj)
        {
            ActivePack.Questions.Remove(SelectedQuestion);
            SelectedQuestion = ActivePack.Questions.LastOrDefault();

            if(ActivePack.Questions.Count == 0)
            {
                CanClickPlay = false;
            }

            RaisePropertySpam();
        }
        private void AddQuestion(object obj)
        {
            if(ListOfQuestionPacks.Count != 0)
            {
                ActivePack.Questions.Add(new Question());
            }

            CanClickPlay = true;
            SelectedQuestion = ActivePack.Questions.LastOrDefault();

            RaisePropertySpam();
        }
        private void RaisePropertySpam()
        {
            RaisePropertyChanged("ActivePack");
            RaisePropertyChanged("CanClickPlay");
            RaisePropertyChanged("KeepActiveWindow");
            RaisePropertyChanged("ListOfQuestionPacks");
            RaisePropertyChanged("SelectedQuestion");
            RaisePropertyChanged("ActivePack.Difficulty");
        }
    }
}