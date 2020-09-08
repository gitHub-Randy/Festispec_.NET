using FestiSpec.Entity;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestiSpec.ViewModel.Questionnaire.VM
{
    public class QuestionTypeVM : ViewModelBase
    {
        public QuestionType _questionType;

        public QuestionTypeVM()
        {
            _questionType = new QuestionType();
        }

        public QuestionTypeVM(QuestionType questionType)
        {
            _questionType = questionType;
        }

        public string Type
        {
            get { return _questionType.Type; }
            set
            {
                _questionType.Type = value;
                RaisePropertyChanged(() => Type);
            }
        }

        public string Description
        {
            get { return _questionType.Description; }
            set
            {
                _questionType.Description = value;
                RaisePropertyChanged(() => Description);
            }
        }

        public QuestionType ToQuestionType()
        {
            return _questionType;
        }
    }
}
