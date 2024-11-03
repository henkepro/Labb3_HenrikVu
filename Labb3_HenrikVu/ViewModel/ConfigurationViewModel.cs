﻿using Labb3_HenrikVu.Command;
using Labb3_HenrikVu.Model;
using Labb3_HenrikVu.View;
using Labb3_HenrikVu.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;

namespace Labb3_HenrikVu.ViewModel
{
    internal class ConfigurationViewModel : ViewModelBase
    {
        public DelegateCommand AddQuestionOnCommand { get; }
        public DelegateCommand CreateQuestionPackOnCommand { get; }
        public DelegateCommand RemoveQuestionOnCommand { get; }
        public DelegateCommand EditPackSettingsOnCommand { get; }
        public DelegateCommand DeleteQuestionPackOnCommand { get; }
        public DelegateCommand SetDefaultPackOnCommand { get; }
        public DelegateCommand ToggleFullScreenCommand { get; }
        public DelegateCommand EditQuestionsOnCommand { get; }
        public DelegateCommand OpenQuestionPackOnCommand { get; }
        public ObservableCollection<QuestionPackViewModel> ListOfQuestionPacks { get => mainWindowViewModel.ListOfQuestionPacks; }
        public QuestionPackViewModel ActivePack { get => mainWindowViewModel.ActivePack; }
        private bool _canClickPlay = true;
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

        private readonly MainWindowViewModel mainWindowViewModel;

        private bool _isFullScreen;
        public bool IsFullScreen
        {
            get => _isFullScreen;
            set
            {
                _isFullScreen = value;
                RaisePropertyChanged();
            }
        }

        private QuestionPackViewModel _targetReference;
        public QuestionPackViewModel TargetReference
        {
            get => _targetReference;
            set
            {
                _targetReference = value;
                RaisePropertyChanged();
            }
        }
        private bool _keepActiveWindow = true;
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
            EditPackSettingsOnCommand = new DelegateCommand(EditPackSettings);
            DeleteQuestionPackOnCommand = new DelegateCommand(DeleteQuestionPack);

            if(TargetReference.Questions.Count == 0)
            {
                TargetReference.Questions.Add(new Question());
            }

            ActivePack.Difficulty = Difficulty.Medium;

            dispatchTimer = new DispatcherTimer();
            dispatchTimer.Interval = TimeSpan.FromSeconds(2);
            dispatchTimer.Tick += Timer_Tick;
            dispatchTimer.Start();

            RaisePropertySpam();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            Debug.WriteLine(ActivePack.Difficulty.ToString());
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

            if(ActivePack.Questions.Count != 0)
            {
                CanClickPlay = true;
            }

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

        private void EditPackSettings(object obj)
        {
            RaisePropertyChanged("ActivePack");
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