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
    public class QuestionListVM : ViewModelBase, ICRUD
    {
        public QuestionList _questionlist;

        public QuestionListVM()
        {
            _questionlist = new QuestionList();
        }

        public QuestionListVM(Entity.Festival festival)
        {
            _questionlist = new QuestionList
            {
                Title = festival.Name + " " + DateTime.Now.Year,
                VersionNumber = festival.QuestionLists.Count() == 0 ? 1 : (festival.QuestionLists.Last().VersionNumber) + 0.1
            };
        }

        public QuestionListVM(QuestionList question)
        {
            _questionlist = question;
        }

        public int? Id
        {
            get { return _questionlist.Id; }
        }

        public string Title
        {
            get { return _questionlist.Title; }
            set
            {
                _questionlist.Title = value;
                RaisePropertyChanged(() => Title);
            }
        }

        public string Description
        {
            get { return _questionlist.Description; }
            set
            {
                _questionlist.Description = value;
                RaisePropertyChanged(() => Description);
            }
        }

        public double VersionNumber
        {
            get { return _questionlist.VersionNumber; }
            set
            {
                _questionlist.VersionNumber = value;
                RaisePropertyChanged(() => VersionNumber);
            }
        }

        public virtual ICollection<Question> Questions
        {
            get { return _questionlist.Questions ?? (_questionlist.Questions = new List<Question>()); }
            set
            {
                _questionlist.Questions = value;
                RaisePropertyChanged(() => Questions);
            }
        }

        public Entity.Festival Festival
        {
            get { return _questionlist.Festival; }
            set
            {
                _questionlist.Festival = value;
                RaisePropertyChanged(() => Festival);
            }
        }

        public int AmountOfQuestions
        {
            get { return Questions.Count; }
        }

        public void Add()
        {
            throw new NotImplementedException();
        }

        public void Remove()
        {
            var context = DBContext.Instance;
            foreach (var q in Questions.ToList())
            {
                new QuestionVM(q).Remove();
            }
            context.QuestionLists.Remove(_questionlist);
            context.SaveChanges();
        }

        public void Update()
        {
            throw new NotImplementedException();
        }

        public void View()
        {
            throw new NotImplementedException();
        }

        public QuestionList ToQuestionList()
        {
            return _questionlist;
        }
    }
}
