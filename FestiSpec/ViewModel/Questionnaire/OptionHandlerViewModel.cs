using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;
using FestiSpec.ViewModel.Questionnaire.VM;
using GalaSoft.MvvmLight.CommandWpf;
using FestiSpec.Entity;
using Microsoft.Win32;
using FestiSpec.Helper;

namespace FestiSpec.ViewModel.Questionnaire
{
    public class OptionHandlerViewModel : ViewModelBase
    {
        private ObservableCollection<string> _optionTemp;

        private string _optionText;
        private string _headerOne;
        private string _headerTwo;
        private string _imagePath;
        private bool _canAddOption;

        public ICommand AddOptionCMD { get; set; }
        public ICommand RemoveOptionCMD { get; set; }
        public ICommand BrowseCMD { get; set; }


        public OptionHandlerViewModel()
        {
            AddOptionCMD = new RelayCommand(AddOption, CanAddOption);
            RemoveOptionCMD = new RelayCommand<int>(RemoveOption);
            OptionTemp = new ObservableCollection<string>();
            BrowseCMD = new RelayCommand(Browse);
            ImagePath = "Selecteer een foto";
            OptionText = _question.QuestionOptions;
        }

        private QuestionVM _question
        {
            get { return new ViewModelLocator().Questionnaire.SelectedQuestion; }
        }

        private void RemoveOption(int item)
        {
            OptionTemp.RemoveAt(item);
            if(OptionTemp.Count!= 0) _question.QuestionOptions = ListToString();
            OptionText = null;
        }

        public ObservableCollection<string> OptionTemp
        {
            get { return _optionTemp; }
            set
            {
                _optionTemp = value;
                RaisePropertyChanged(() => OptionTemp);
            }
        }

        public void SetOptions(string type)
        {
            switch (TypeConverter.type[type])
            {
                case "YesNo":
                    {
                        _question.QuestionOptions = "Ja;Nee";
                        break;
                    }
                case "MultipleChoice":
                    {
                        OptionTemp = new ObservableCollection<string>();

                        if (new ViewModelLocator().Questionnaire.SelectedQuestion.QuestionOptions != null)
                        {
                            List<string> temp = new ViewModelLocator().Questionnaire.SelectedQuestion.QuestionOptions.Split(';').ToList<string>();
                            OptionTemp = new ObservableCollection<string>(temp);
                        }
                        OptionText = null;
                        break;
                    }
                case "Table":
                    {
                        OptionTemp = new ObservableCollection<string>();
                        HeaderOne = null;
                        HeaderTwo = null;
                        if (new ViewModelLocator().Questionnaire.SelectedQuestion.QuestionOptions != null)
                        {
                            List<string> temp = new ViewModelLocator().Questionnaire.SelectedQuestion.QuestionOptions.Split(';').ToList<string>();

                            OptionTemp = new ObservableCollection<string>(temp);
                            HeaderOne = OptionTemp[0];
                            HeaderTwo = OptionTemp[1];
                            OptionTemp.RemoveAt(1);
                            OptionTemp.RemoveAt(0);
                        }
                        OptionText = null;
                        break;
                    }
            }
        }

        public string OptionText
        {
            get { return _optionText; }
            set
            {
                _optionText = value;
                RaisePropertyChanged(() => OptionText);
            }
        }
        public string HeaderOne
        {
            get { return _headerOne; }
            set
            {
                _headerOne = value;
                RaisePropertyChanged(() => HeaderOne);
            }
        }
        public string HeaderTwo
        {
            get { return _headerTwo; }
            set
            {
                _headerTwo = value;
                RaisePropertyChanged(() => HeaderTwo);
            }
        }

        private void Browse()
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                ImagePath = op.FileName;
            }
        }

        public string ImagePath
        {
            get { return _imagePath; }
            set
            {
                _imagePath = value;
                RaisePropertyChanged(() => ImagePath);
            }
        }

        private void AddOption()
        {
            switch (TypeConverter.type[new ViewModelLocator().Questionnaire.SelectedType.Type])
            {
                case "Image":
                    {
                        Browse();
                        byte[] imageBytes = System.IO.File.ReadAllBytes(ImagePath);
                        string base64String = Convert.ToBase64String(System.IO.File.ReadAllBytes(ImagePath));
                        if (_question.AmountOfAttachments == 1)
                        {
                            _question.Attachments.FirstOrDefault().FilePath = base64String;
                        }
                        else
                        {
                            _question.Attachments.Add(new Attachment { FilePath = base64String });
                        }
                        break;
                    }
                case "MultipleChoice":
                    {
                        //char c;
                        //if (OptionTemp.Count != 0)
                        //{
                        //    c = OptionTemp.Last()[0];
                        //    c++;
                        //}
                        //else c = 'A';
                        //OptionTemp.Add(c + ": " + OptionText);
                        OptionTemp.Add(OptionText);
                        _question.QuestionOptions = ListToString();
                        OptionText = null;
                        break;
                    }
                case "Table":
                    {
                        //char c;
                        //if (OptionTemp.Count != 0)
                        //{
                        //    c = OptionTemp.Last()[0];
                        //    c++;
                        //}
                        //else c = 'A';
                        //OptionTemp.Add(c + ": " + OptionText);
                        OptionTemp.Add(OptionText);
                        OptionText = null;
                        break;
                    }
            }
        }

        private bool CanAddOption()
        {
            var temp = new ViewModelLocator().Questionnaire.SelectedType;

            if (temp != null)
            {
                if (TypeConverter.type[temp.Type] == "Image")
                {
                    return true;
                }
            }
            return !string.IsNullOrEmpty(OptionText);
        }

        public string ListToString()
        {
            return OptionTemp.Aggregate((x, y) => x + ";" + y);
        }
    }
}
