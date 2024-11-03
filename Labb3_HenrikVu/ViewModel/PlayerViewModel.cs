using Labb3_HenrikVu.Command;
using Labb3_HenrikVu.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Labb3_HenrikVu.ViewModel
{
    internal class PlayerViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? mainWindowViewModel;
        private DispatcherTimer dispatchTimer;
        public QuestionPackViewModel ActivePack { get => mainWindowViewModel.ActivePack; }
        public DelegateCommand UpdateButtonOnCommand { get; }
        public DelegateCommand PlayQuestionsOnCommand { get; }
        public DelegateCommand SelectAnswerOnCommand { get; }

        private int temptimer = 30;
        private int questionIndex = 1;
        private int correctQuestionIndex = 0;
        private int _timer;
        private string _activePackQuestionCount;
        private bool _keepActiveWindow = false;
        private bool _button0 = false;
        private bool _button1 = false;
        private bool _button2 = false;
        private bool _button3 = false;
        private bool _button00 = false;
        private bool _button11 = false;
        private bool _button22 = false;
        private bool _button33 = false;
        private bool waitForNextQuestion = false;
        private bool _isQuestionSelected = false;
        private string _activePackCorrectAnswers;
        private TaskCompletionSource<bool> selectAnswerCompletionSource;
        public bool IsLastQuestion { get; set; } = true;
        public bool IsNotLastQuestion { get; set; } = false;
        public string ActivePackQuestionCount
        {
            get
            {
                return $"{questionIndex}/{mainWindowViewModel.ActivePack.Questions.Count.ToString()}";
            }
            set
            {
                _activePackQuestionCount = value;
                RaisePropertyChanged();
                RaisePropertyChanged("SelectedQuestion");
                RaisePropertyChanged("ActivePack");
            }
        }
        public string ActivePackCorrectAnswers
        {
            get
            {
                return $"{correctQuestionIndex}/{mainWindowViewModel.ActivePack.Questions.Count.ToString()}";
            }
            set
            {
                _activePackCorrectAnswers = value;
                RaisePropertyChanged();
                RaisePropertyChanged("SelectedQuestion");
                RaisePropertyChanged("ActivePack");
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
        public int Timer
        {
            get => _timer;
            set
            {
                _timer = value;
                RaisePropertyChanged();
            }
        }
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
        public PlayerViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
            dispatchTimer = new DispatcherTimer();
            dispatchTimer.Interval = TimeSpan.FromSeconds(1);
            dispatchTimer.Tick += Timer_Tick;

            UpdateButtonOnCommand = new DelegateCommand(UpdateData);
            PlayQuestionsOnCommand = new DelegateCommand(PlayQuestions);
            SelectAnswerOnCommand = new DelegateCommand(SelectAnswer, CheckAnswers);

            RandomizeSelectedQuestions();
            ResetButtonBooleans();
            ResetTimer();
            RaisePropertyChanged("ActivePack");
        }
        public bool Button0 { get => _button0; set { _button0 = value; RaisePropertyChanged(); } }
        public bool Button1 { get => _button1; set { _button1 = value; RaisePropertyChanged(); } }
        public bool Button2 { get => _button2; set { _button2 = value; RaisePropertyChanged(); } }
        public bool Button3 { get => _button3; set { _button3 = value; RaisePropertyChanged(); } }
        public bool Button00 { get => _button00; set { _button00 = value; RaisePropertyChanged(); } }
        public bool Button11 { get => _button11; set { _button11 = value; RaisePropertyChanged(); } }
        public bool Button22 { get => _button22; set { _button22 = value; RaisePropertyChanged(); } }
        public bool Button33 { get => _button33; set { _button33 = value; RaisePropertyChanged(); } }
        public void ResetButtonBooleans()
        {
            Button0 = false; Button1 = false; Button2 = false; Button3 = false;
            Button00 = false; Button11 = false; Button22 = false; Button33 = false;
            _isQuestionSelected = false;
        }
        bool hasCorrectAnswer = false;
        private bool CheckAnswers(object? arg)
        {
            if(arg is string && waitForNextQuestion == false || Timesup)
            {
                string tempString = (string)arg;
                int answerIndex = RandomizedAnswerList.IndexOf(tempString);
                int correctIndex = RandomizedAnswerList.IndexOf(SelectedQuestion.CorrectAnswer);
                if(SelectedQuestion.CorrectAnswer == tempString && hasCorrectAnswer == false || Timesup)
                {
                    hasCorrectAnswer = true;
                    correctQuestionIndex++;
                    if(Timesup)
                    {
                        answerIndex = correctIndex;
                        correctQuestionIndex--;
                    }
                    switch(answerIndex)
                    {
                        case 0: return Button0 = true;
                        case 1: return Button1 = true;
                        case 2: return Button2 = true;
                        case 3: return Button3 = true;
                    }
                }
                else
                {
                    if(_isQuestionSelected == false)
                    {
                        switch(answerIndex)
                        {
                            case 0: Button00 = true; _isQuestionSelected = true; break;
                            case 1: Button11 = true; _isQuestionSelected = true; break;
                            case 2: Button22 = true; _isQuestionSelected = true; break;
                            case 3: Button33 = true; _isQuestionSelected = true; break;
                        }
                        foreach(string answer in RandomizedAnswerList)
                        {
                            if(answer == SelectedQuestion.CorrectAnswer)
                            {
                                int Index = RandomizedAnswerList.IndexOf(answer);
                                switch(Index)
                                {
                                    case 0: return Button0 = true;
                                    case 1: return Button1 = true;
                                    case 2: return Button2 = true;
                                    case 3: return Button3 = true;
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }

        public ObservableCollection<string> RandomizedAnswerList { get; set; }
        public void RandomizeSelectedQuestions()
        {
            Random random = new Random();
            List<string> tempList = new List<string>
            {
                SelectedQuestion.CorrectAnswer, 
                SelectedQuestion.IncorrectAnswers[0],
                SelectedQuestion.IncorrectAnswers[1],
                SelectedQuestion.IncorrectAnswers[2]
            };

            tempList = tempList.OrderBy(x => random.Next()).ToList();
            RandomizedAnswerList = new ObservableCollection<string>(tempList);

            RaisePropertyChanged("RandomizedAnswerList");
        }

        public bool HandleLastQuestion
        {
            set 
            {
                IsLastQuestion = !IsLastQuestion; 
                IsNotLastQuestion = !IsNotLastQuestion; 
                RaisePropertyChanged("IsLastQuestion");
                RaisePropertyChanged("IsNotLastQuestion");
            }
        }
        bool Timesup = false;
        private async void SelectAnswer(object obj)
        {
            if(waitForNextQuestion == false || Timesup)
            {
                selectAnswerCompletionSource = new TaskCompletionSource<bool>();

                waitForNextQuestion = true;
                await Task.Delay(3000);
                if(questionIndex < ActivePack.Questions.Count)
                {
                    questionIndex++;
                    SelectedQuestion = ActivePack.Questions[ActivePack.Questions.Count-questionIndex];
                    ResetTimer();
                    ResetButtonBooleans();
                    RandomizeSelectedQuestions();
                    dispatchTimer.Start();
                }
                else
                {
                    HandleLastQuestion = true;
                    RaisePropertyChanged("IsLastQuestion");
                    RaisePropertyChanged("ActivePackCorrectAnswers");
                }
                waitForNextQuestion = false;
                hasCorrectAnswer = false;

                selectAnswerCompletionSource.SetResult(true);
            }
            RaisePropertySpam();
        }
        public void ResetTimer()
        {
            Timer = ActivePack.TimeLimitInSeconds;
            RaisePropertyChanged("Timer");
        }

        private void PlayQuestions(object obj)
        {
            ResetButtonBooleans();
            if(KeepActiveWindow == false)
            {
                mainWindowViewModel.ToggleSwapActiveWindow();
            }
            if(IsNotLastQuestion == false)
            {
                HandleLastQuestion = true;
                correctQuestionIndex = 0;
            }

            questionIndex = 1;
            ResetTimer();
            dispatchTimer.Start();

            SelectedQuestion = ActivePack.Questions.LastOrDefault();

            RandomizeSelectedQuestions();
            RaisePropertySpam();
        }

        private void UpdateData(object obj)
        {
            UpdateButtonOnCommand.RaiseCanExecuteChanged();
        }
        private async void Timer_Tick(object? sender, EventArgs e)
        {
            bool canStopTimer = true;
            if(Timer > 5) Timer -= 4;
            if(Timer > 0 && Timesup == false)
            {
                Timer--;
            } 
            if(waitForNextQuestion == true && canStopTimer == true)
            {
                dispatchTimer.Stop();
                canStopTimer = false;
            }
            if(Timer == 0 && waitForNextQuestion == false)
            {
                Timesup = true;
                CheckAnswers(null);
                SelectAnswer(null);
                Timesup = false;

                await selectAnswerCompletionSource.Task;
                canStopTimer = true;
            }
            RaisePropertyChanged("Timer");
        }
        private void RaisePropertySpam()
        {
            RaisePropertyChanged("KeepActiveWindow");
            RaisePropertyChanged("ActivePack");
            RaisePropertyChanged("ActivePackQuestionCount");
            RaisePropertyChanged("SelectedQuestion");
            RaisePropertyChanged("Timer");
            RaisePropertyChanged("RandomizedAnswerList");
        }
    }
}
