using FestiSpec.Entities.Dal;
using FestiSpec.Entity;
using FestiSpec.Helper;
using FestiSpec.PdfBuilder;
using FestiSpec.PdfBuilder.Node;
using FestiSpec.PdfBuilder.Styling.Tags;
using FestiSpec.ViewModel.Festival;
using FestiSpec.ViewModel.Interfaces;
using FestiSpec.ViewModel.Questionnaire.VM;
using FestiSpec.Views.Question;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using IronPdf;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI.DataVisualization.Charting;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace FestiSpec.ViewModel.Questionnaire
{
    public class QuestionnaireViewModel : ViewModelBase
    {
        #region Private fields
        private QuestionTypeVM _stype;
        private QuestionVM _squestion;
        private QuestionListVM _squestionlist;
        private FestivalViewModel _sfestival;
        private int _switch;
        private bool _caneditquestionlist = false;
        private bool _caneditquestion = false;
        #endregion

        #region Private ObservableCollections
        private ObservableCollection<QuestionListVM> _questionlist;
        private ObservableCollection<QuestionVM> _questions;
        private ObservableCollection<QuestionTypeVM> _qtype;
        #endregion

        #region ICommands
        // Create new questionlist & save
        public ICommand CreateQuestionListCMD { get; set; }
        public ICommand SaveQuestionListCMD { get; set; }
        // CRUD ICommands questionlist
        public ICommand ShowQuestionListCMD { get; set; }
        public ICommand EditQuestionListCMD { get; set; }
        public ICommand DeleteQuestionListCMD { get; set; }

        // TypChooser & Create new question
        public ICommand ShowTypeChooserCMD { get; set; }
        public ICommand CreateQuestionCMD { get; set; }
        // CRUD ICommands question
        public ICommand AddQuestionCMD { get; set; }
        public ICommand EditQuestionCMD { get; set; }
        public ICommand RemoveQuestionCMD { get; set; }

        //public ICommand ShowQuestionCMD { get; set; }

        // Helpers (switch & pdf)
        public ICommand BackCMD { get; set; }
        public ICommand QuestionlistToPdfCMD { get; set; }
        #endregion

        #region Const
        public QuestionnaireViewModel()
        {
            InitRelayComand();

            IEnumerable<QuestionTypeVM> qtype = DBContext.Instance.QuestionTypes.ToList().Select(q => new QuestionTypeVM(q));

            Qtype = new ObservableCollection<QuestionTypeVM>(qtype);
        }

        internal void LoadQuestionList(FestivalViewModel selectedFestival)
        {
            SwitchView = 0;
            _sfestival = selectedFestival ?? throw new ArgumentNullException(nameof(selectedFestival));
            QuestionList = new ObservableCollection<QuestionListVM>(selectedFestival.QuestionLists.Select(q => new QuestionListVM(q)));
        }
        #endregion

        #region ObservableCollections prop
        public ObservableCollection<QuestionListVM> QuestionList
        {
            get { return _questionlist; }
            set
            {
                _questionlist = value;
                RaisePropertyChanged(() => QuestionList);
            }
        }

        public ObservableCollection<QuestionVM> Questions
        {
            get { return _questions; }
            set
            {
                _questions = value;
                RaisePropertyChanged(() => Questions);
            }
        }

        public ObservableCollection<QuestionTypeVM> Qtype
        {
            get { return _qtype; }
            set
            {
                _qtype = value;
                RaisePropertyChanged(() => Qtype);
            }
        }
        #endregion

        #region Prop
        public QuestionVM SelectedQuestion
        {
            get { return _squestion; }
            set
            {
                _squestion = value;
                RaisePropertyChanged(() => SelectedQuestion);
            }
        }

        public QuestionListVM SelectedQuestionList
        {
            get { return _squestionlist; }
            set
            {
                _squestionlist = value;
                RaisePropertyChanged(() => SelectedQuestionList);
            }
        }

        public QuestionTypeVM SelectedType
        {
            get { return _stype; }
            set
            {
                _stype = value;
                RaisePropertyChanged(() => SelectedType);
            }
        }
        #endregion

        #region RelayCommands
        private void InitRelayComand()
        {
            // Create new questionlist & save
            CreateQuestionListCMD = new RelayCommand(CreateQuestionList, CanAddQuestionList);
            SaveQuestionListCMD = new RelayCommand(SaveQuestionList, CanSaveQuestionList);
            // CRUD ICommands questionlist
            ShowQuestionListCMD = new RelayCommand<int>(o => { ShowCreateEdit(o); CanEditQuestionList = false; });
            EditQuestionListCMD = new RelayCommand<int>(o => { ShowCreateEdit(o); CanEditQuestionList = true; });
            DeleteQuestionListCMD = new RelayCommand<int>(RemoveQuestionList);

            // TypChooser & Create new question
            ShowTypeChooserCMD = new RelayCommand(() => SwitchView = 2);
            CreateQuestionCMD = new RelayCommand<string>(o => { CreateQuestion(o); CanEditQuestion = true; });
            // CRUD ICommands question
            AddQuestionCMD = new RelayCommand(AddQuestion, CanSaveQuestion);
            EditQuestionCMD = new RelayCommand<int>(o => { EditQuestion(o); CanEditQuestion = true; });
            RemoveQuestionCMD = new RelayCommand<int>(RemoveQuestion);

            //ShowQuestionCMD = new RelayCommand<int>(o => { EditQuestion(o); CanEditQuestion = false; });

            // Helpers (switch & pdf)
            BackCMD = new RelayCommand<string>((o) => { if (o == null) SwitchView--; else if (o == "2") { SwitchView--; } else SwitchView = Convert.ToInt32(o); });
            QuestionlistToPdfCMD = new RelayCommand<int>(ToPDF);
        }
        #endregion

        #region Questionlist CRUD
        // Create empty questionlist
        private void CreateQuestionList()
        {
            CanEditQuestionList = true;
            SelectedQuestionList = new QuestionListVM(_sfestival.Model);
            Questions = new ObservableCollection<QuestionVM>();
            SwitchView = 1;
        }

        // Show/Edit a questionlist when CanEditQuestionList: false || true
        private void ShowCreateEdit(int id)
        {
            var t = _sfestival.QuestionLists.Single(i => i.Id == id);
            SelectedQuestionList = new QuestionListVM(t);
            Questions = new ObservableCollection<QuestionVM>(SelectedQuestionList.Questions.Select(q => new QuestionVM(q)));
            SwitchView = 1;
        }

        // Save questionlist
        public void SaveQuestionList()
        {
            var item = SelectedQuestionList.Id == 0 ? null : _sfestival.QuestionLists.Single(i => i.Id == SelectedQuestionList.Id);
            if (item == null)
            {
                try
                {
                    _sfestival.QuestionLists.Add(SelectedQuestionList.ToQuestionList());
                    QuestionList.Add(SelectedQuestionList);
                    SwitchView = 0;
                    DBContext.Instance.SaveChanges();
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                }
            }
            else
            {
                DBContext.Instance.SaveChanges();
                SwitchView = 0;
            }
        }

        // Remove questionlist
        public void RemoveQuestionList(int id)
        {
            if (MessageBox.Show("Weet je zeker dat je de vragenlijst wilt verwijderen?", "Verwijder vragenlijst?", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                QuestionList.Single(x => x.Id == id).Remove();
                LoadQuestionList(_sfestival);
            }
        }
        #endregion

        #region Question CRUD
        // TypeChooser To CreateEditQuestionView (Make new question)
        private void CreateQuestion(string type)
        {
            SelectedQuestion = new QuestionVM { Type = type };
            SelectedType = new QuestionTypeVM { Type = type };
            new ViewModelLocator().OptionHandlerViewModel.SetOptions(type);
            SwitchView = 3;
        }

        // Add/Update question to questionlist
        public void AddQuestion()
        {
            var item = SelectedQuestion.Index == 0 ? null : SelectedQuestionList.Questions.FirstOrDefault(i => i.Index == SelectedQuestion.Index);
            if (TypeConverter.type[SelectedQuestion.Type] == "Table")
            {
                var temp = new ViewModelLocator().OptionHandlerViewModel;
                temp.OptionTemp.Insert(0, temp.HeaderOne);
                temp.OptionTemp.Insert(1, temp.HeaderTwo);
                SelectedQuestion.QuestionOptions = temp.ListToString();
            }
            if (item == null)
            {
                try
                {
                    SelectedQuestionList.Questions.Add(SelectedQuestion.ToQuestion());
                    Questions.Add(SelectedQuestion);
                    SelectedQuestion = null;
                    SwitchView = 1;
                    UpdateIndex();
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                }
            }
            else
            {
                item = SelectedQuestion.ToQuestion();
                var t = Questions.Single(i => i.Index == SelectedQuestion.Index);
                t = SelectedQuestion;
                SelectedQuestion = null;
                SwitchView = 1;
                UpdateIndex();
            }
        }

        // Edit question
        private void EditQuestion(int index)
        {
            SelectedQuestion = new QuestionVM(SelectedQuestionList.Questions.FirstOrDefault(i => i.Index == index));
            SelectedType = new QuestionTypeVM { Type = SelectedQuestion.Type };

            if (SelectedQuestion.QuestionOptions != null) new ViewModelLocator().OptionHandlerViewModel.SetOptions(SelectedType.Type);
            SwitchView = 3;
        }

        // Remove question from questionlist
        public void RemoveQuestion(int index)
        {
            if (MessageBox.Show("Weet je zeker dat je de vraag uit de lijst wilt verwijderen?", "Verwijder vragenlijst?", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                var tQuestion = Questions.Single(x => x.Index == index);
                SelectedQuestionList.Questions.Remove(tQuestion.ToQuestion());
                Questions.Remove(tQuestion);
                if (tQuestion.Id != 0) tQuestion.Remove();
                UpdateIndex();
            }
        }
        #endregion

        #region canExecute
        private bool CanAddQuestionList()
        {
            return new ViewModelLocator().FestivalListViewModel.SelectedFestival != null;
        }

        private bool CanSaveQuestionList()
        {
            return SelectedQuestionList.AmountOfQuestions != 0 && !string.IsNullOrEmpty(SelectedQuestionList.Title) && !string.IsNullOrEmpty(SelectedQuestionList.Description) && !double.IsNaN(SelectedQuestionList.VersionNumber) && !double.IsInfinity(SelectedQuestionList.VersionNumber);
        }

        private bool CanSaveQuestion()
        {

            if (SelectedQuestion == null ? false : TypeConverter.type[SelectedQuestion.Type] == "Table")
            {
                var temp = new ViewModelLocator().OptionHandlerViewModel;
                return !string.IsNullOrEmpty(temp.HeaderOne) && !string.IsNullOrEmpty(temp.HeaderTwo) && temp.OptionTemp.Count != 0 && !string.IsNullOrEmpty(SelectedQuestion.QuestionText);
            }
            return SelectedQuestion == null ? false : !string.IsNullOrEmpty(SelectedQuestion.QuestionText);
        }

        public bool CanEditQuestionList
        {
            get { return _caneditquestionlist; }
            set
            {
                _caneditquestionlist = value;
                //RaisePropertyChanged(() => CanEditQuestionList);
            }
        }

        public bool CanEditQuestion
        {
            get { return _caneditquestion; }
            set
            {
                _caneditquestion = value;
                //RaisePropertyChanged(() => CanEditQuestion);
            }
        }
        #endregion

        #region Helpers (switch & update_index)
        // Switch UserControl
        public int SwitchView
        {
            get { return _switch; }
            set
            {
                _switch = value;
                RaisePropertyChanged(() => SwitchView);
            }
        }

        // Update index of questions
        public void UpdateIndex()
        {
            int t = 1;
            foreach (var selectedItem in Questions)
            {
                selectedItem.Index = t++;
            }
        }

        // PDF generator
        private void ToPDF(int id)
        {
            SelectedQuestionList = new QuestionListVM(DBContext.Instance.QuestionLists.FirstOrDefault(i => i.Id == id));
            CustomerCompany customer = SelectedQuestionList.Festival.CustomerCompany;

            Builder PDF = new Builder();
            var name = SelectedQuestionList.Festival.Name;
            var Startdate = SelectedQuestionList.Festival.StartDate;
            var EndDate = SelectedQuestionList.Festival.EndDate;
            var convert = Convert.ToDateTime(Startdate).Day + "-" + Convert.ToDateTime(EndDate).Day + " " + Convert.ToDateTime(Startdate).ToString("MMMM") + " " + Convert.ToDateTime(Startdate).Year;

            PDF.Renderer.PrintOptions.Header = new HtmlHeaderFooter() { HtmlFragment = "<a><em style='color:gray'>" + "Inspectie " + name + " " + convert + " – Festispec – Inspectie formulier v" + SelectedQuestionList.VersionNumber.ToString("F1", CultureInfo.InvariantCulture) + "</em></a>" };

            PDF.AddNode(new PdfHeaderNode($"Bedrijf: {customer.NameCompany}") { Size = new FontSizeTag("20px") });
            PDF.AddNode(new PdfHeaderNode($"Datum: {DateTime.Now.ToShortDateString()}") { Size = new FontSizeTag("20px") });
            PDF.AddNode(new PdfHeaderNode($"Locatie: {customer.City} - {customer.Street} {customer.HouseNumber}\n") { Size = new FontSizeTag("20px") });

            PDF.AddText("");
            PDF.AddHeader("Introductie");
            PDF.AddNode(new PdfTextNode(Interaction.InputBox("Introductie", "Vul hier je introducerend commentaar in", "")) { Size = new FontSizeTag("20px") });
            PDF.AddText("");

            PDF.AddHeader("Vragenlijst " + name + " " + Convert.ToDateTime(Startdate).Year);
            PDF.AddText("Instructies vragenlijst \n\r" + SelectedQuestionList.Description);

            foreach (var s in SelectedQuestionList.Questions)
            {
                PDF.AddNode(new PdfTextNode(s.QuestionText) { Weight = new FontWeightTag(FontWeightTag.Values.bold), Size = new FontSizeTag("22px") });
                PDF.AddNode(new PdfTextNode(s.Description) { Weight = new FontWeightTag(FontWeightTag.Values.lighter), Size = new FontSizeTag("15px") });

                if (s.Answers != null)
                {
                    foreach (var a in s.Answers)
                    {
                        string table = getTable(s, a.Inspection.Employee.Id);
                        if (table != "")
                        {
                            PDF.AddNode(new PdfTextNode(a.Inspection.Employee.Name + ":"));
                            PDF.AddNode(new PdfHtmlNode(table));
                        }
                        else
                        {
                            PDF.AddNode(new PdfTextNode(a.Inspection.Employee.Name + ": " + a.AnswerText));
                        }
                        if (s.Attachments.Count != 0)
                        {
                            var imageBack = @"data:image/png;base64," + s.Attachments.FirstOrDefault().FilePath;
                            var imageFront = @"data:image/png;base64," + a.Attachments.FirstOrDefault().FilePath;
                            var html = "<style>" +
                                        ".img {" +
                                                "position: absolute;" +
                                                "z-index:1;" +
                                        "}" +
                                        "#container {" +
                                        "display:inline-block;" +
                                        "}" +
                                        ".img2 {" +
                                        "position:relative;" +
                                        "z-index:20;" +
                                        "}" +
                                        "</style>" +
                                        "<div id='container'>" +
                                            "<img class='img' width='500' height='300' src='" + imageBack + "'/>" +
                                            "<img class='img2' width='500' height='300' src='" + imageFront + "'/>" +
                                        "</div>";
                            PDF.AddNode(new PdfHtmlNode(html));
                        }
                        else if (a.Attachments.Count != 0)
                        {
                            PDF.AddImageBase64(a.Attachments.FirstOrDefault().FilePath, "500", "300");
                        }
                        PDF.AddText("\n");
                    }

                    string chart = getChart(s);
                    if (chart != "")
                    {
                        PDF.AddImageBase64(chart, "300px", "300px");
                    }
                }
            }

            PDF.AddText("");
            PDF.AddHeader("Advies");
            PDF.AddNode(new PdfTextNode(Interaction.InputBox("Advies", "Vul hier je advies in", "")) { Size = new FontSizeTag("20px") });
            PDF.AddText("");

            PDF.Export("file.pdf");
            System.Diagnostics.Process.Start("file.pdf");
        }

        public string getTable(Question question, int id)
        {
            if (question.Type == "Tabel")
            {
                var t = "";
                var temp = "";

                var options = question.QuestionOptions.Split(';').ToList();
                var answer = question.Answers.FirstOrDefault(i => i.Inspection.EmployeeId == id).AnswerText.Split(';').ToList();
                for (int i = 2; i < options.Count(); i++)
                {
                    if (answer[i - 2] == null) { answer[i - 2] = ""; }
                    temp += string.Format(@"<tr>" +
                    "<td>" + options[i] + "</td>" +
                    "<td>" + answer[i - 2] + "</td>" +
                  "</tr>");
                }

                t += "<table border=1>" +
                 "<tr>" +
                   "<th>" + options[0] + "</th>" +
                   "<th>" + options[1] + "</th>" +
                 "</tr>" +
                 temp +
                "</table>";

                return t;
            }
            else
            {
                return "";
            }
        }

        public string getChart(Question question)
        {
            if (question.Type == "Meerkeuzevragen")
            {
                //Open file stream
                FileStream stream = new FileStream("chart.png", FileMode.Create);

                using (var Chart1 = new Chart())
                {
                    //Create chart and add points
                    var l = new Legend("Antwoorden");
                    Series pieSeries = new Series("Antwoorden") { ChartType = SeriesChartType.Pie, IsVisibleInLegend = true, Legend = "Antwoorden", BorderWidth = 3 };
                    Chart1.Series.Add(pieSeries);
                    Chart1.Legends.Add(l);

                    Dictionary<string, int> count = new Dictionary<string, int>();
                    foreach (Answer a in question.Answers)
                    {
                        if (a.AnswerText != null)
                        {
                            if (count.ContainsKey(a.AnswerText))
                            {
                                count[a.AnswerText] = count[a.AnswerText]++;
                            }
                            else
                            {
                                count[a.AnswerText] = 1;
                            }
                        }
                    }

                    foreach (string answer in count.Keys)
                    {
                        pieSeries.Points.AddXY(answer, count[answer]);
                    }

                    Chart1.Height = 300;
                    Chart1.Width = 300;


                    ChartArea chartArea = new ChartArea();
                    Chart1.ChartAreas.Add(chartArea);

                    Chart1.Series[0].ChartType = SeriesChartType.Pie;
                    Chart1.Legends[0].Enabled = true;
                    Chart1.ChartAreas[0].Area3DStyle.Enable3D = true;

                    Chart1.SaveImage(stream, ChartImageFormat.Png);
                }

                //Close file stream
                stream.Close();

                var pngBinaryData = File.ReadAllBytes("chart.png");
                var ImgDataURI = Convert.ToBase64String(pngBinaryData);

                return ImgDataURI;
            }
            return "";
        }
        #endregion
    }
}