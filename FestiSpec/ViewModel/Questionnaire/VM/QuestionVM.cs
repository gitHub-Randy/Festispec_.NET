using FestiSpec.Entities.Dal;
using FestiSpec.Entity;
using FestiSpec.ViewModel.Interfaces;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestiSpec.ViewModel.Questionnaire.VM
{
    public class QuestionVM : ViewModelBase, ICRUD
    {
        public Question _question;

        public QuestionVM()
        {
            _question = new Question();
        }

        public QuestionVM(Question question)
        {
            _question = question;
        }

        public int Id
        {
            get { return _question.Id; }
        }

        public string QuestionText
        {
            get { return _question.QuestionText; }
            set
            {
                _question.QuestionText = value;
                RaisePropertyChanged(() => QuestionText);
            }
        }

        public string Description
        {
            get { return _question.Description; }
            set
            {
                _question.Description = value;
                RaisePropertyChanged(() => Description);
            }
        }

        public int Index
        {
            get { return _question.Index; }
            set
            {
                _question.Index = value;
                RaisePropertyChanged(() => Index);
            }
        }

        public string Type
        {
            get { return _question.Type; }
            set
            {
                _question.Type = value;
                RaisePropertyChanged(() => Type);
            }
        }

        public string QuestionOptions
        {
            get { return _question.QuestionOptions; }
            set
            {
                _question.QuestionOptions = value;
                RaisePropertyChanged(() => QuestionOptions);
            }
        }

        public QuestionType QuestionType
        {
            get { return _question.QuestionType; }
            set
            {
                _question.QuestionType = value;
                RaisePropertyChanged(() => QuestionType);
            }
        }

        public virtual ICollection<Answer> Answers
        {
            get { return _question.Answers ?? (_question.Answers = new List<Answer>()); }
            set
            {
                _question.Answers = value;
                RaisePropertyChanged(() => Answers);
            }
        }

        public virtual ICollection<Attachment> Attachments
        {
            get { return _question.Attachments ?? (_question.Attachments = new List<Attachment>()); }
            set
            {
                _question.Attachments = value;
                RaisePropertyChanged(() => Attachments);
            }
        }

        public int AmountOfAnswers
        {
            get { return Answers.Count; }
        }

        public int AmountOfAttachments
        {
            get { return Attachments.Count; }
        }

        public void Add()
        {
            throw new NotImplementedException();
        }

        public void Remove()
        {
            var context = DBContext.Instance;
            context.Questions.Attach(_question);
            context.Questions.Remove(_question);
            context.SaveChanges();
        }

        public void Update()
        {
            var context = DBContext.Instance;
            context.Questions.Attach(_question);
            context.Questions.Remove(_question);
            context.SaveChanges();
        }

        public void View()
        {
            throw new NotImplementedException();
        }

        public Question ToQuestion()
        {
            return _question;
        }
    }
}
