using FestiSpec.Entity;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestiSpec.ViewModel.Questionnaire.VM
{
    class AttachmentVM : ViewModelBase
    {
        public Attachment _attachment;

        public AttachmentVM()
        {
            _attachment = new Attachment();
        }

        public AttachmentVM(Attachment attachment)
        {
            _attachment = attachment;
        }

        public int Id
        {
            get { return _attachment.Id; }
        }

        public string FilePath
        {
            get { return _attachment.FilePath; }
            set
            {
                _attachment.FilePath = value;
                RaisePropertyChanged(() => FilePath);
            }
        }

        public virtual ICollection<Answer> Answers
        {
            get { return _attachment.Answers ?? (_attachment.Answers = new List<Answer>()); }
            set
            {
                _attachment.Answers = value;
                RaisePropertyChanged(() => Answers);
            }
        }

        public virtual ICollection<Question> Questions
        {
            get { return _attachment.Questions; }
            set
            {
                _attachment.Questions = value;
                RaisePropertyChanged(() => Questions);
            }
        }

        public int AmountOfAnswers
        {
            get { return Answers.Count; }
        }

        public int AmountOfQuestions
        {
            get { return Questions.Count; }
        }

        public Attachment ToAttachment()
        {
            return _attachment;
        }
    }
}
