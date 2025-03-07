using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Input;
using System.Xml;
using C4Net.Framework.Core.Utils;
using C4Net.Framework.MVVM;
using C4Net.Framework.Templates;
using MetaTool.Business;
using MetaTool.Business.Entities;
using MetaTool.ViewModels.Flyouts;
using MetaTool.ViewModels.Trees;

namespace MetaTool.ViewModels
{
    /// <summary>
    /// View model for the Shell View.
    /// </summary>
    [Export(typeof(IShell))]
    public class ShellViewModel : FlyoutScreen, IShell
    {
        #region - Fields -

        /// <summary>
        /// The selected tab index
        /// </summary>
        private int selectedTabIndex = 0;

        /// <summary>
        /// The selected path
        /// </summary>
        private string selectedPath = string.Empty;

        /// <summary>
        /// The selected output paht
        /// </summary>
        private string selectedOutputPath = string.Empty;

        /// <summary>
        /// The templates path.
        /// </summary>
        private string templatesPath = string.Empty;

        /// <summary>
        /// The template list
        /// </summary>
        private ObservableCollection<string> templateList = new ObservableCollection<string>();

        /// <summary>
        /// The selected template
        /// </summary>
        private string selectedTemplate = string.Empty;

        /// <summary>
        /// Indicates if the metamodel entities must be included.
        /// </summary>
        private bool includeMeta = true;

        /// <summary>
        /// Indicates if the application is during the loading tables task.
        /// </summary>
        private bool isLoadingTables = false;

        /// <summary>
        /// The metamodel loader
        /// </summary>
        private ModelLoader metamodelLoader = new ModelLoader();

        /// <summary>
        /// The move to page tables command
        /// </summary>
        private RelayAsyncCommand moveToPageTablesCommand;

        /// <summary>
        /// The move to page build command
        /// </summary>
        private RelayAsyncCommand moveToPageBuildCommand;

        /// <summary>
        /// The entities
        /// </summary>
        private Dictionary<double, EntityViewModel> entities = new Dictionary<double, EntityViewModel>();

        /// <summary>
        /// The root entities
        /// </summary>
        private ObservableCollection<EntityViewModel> rootEntities = new ObservableCollection<EntityViewModel>();

        /// <summary>
        /// The parameters
        /// </summary>
        private ObservableKeyValue<string, string> parameters = new ObservableKeyValue<string, string>();

        /// <summary>
        /// The selected parameter
        /// </summary>
        private ObservableKeyValueItem<string, string> selectedParameter;

        /// <summary>
        /// The progress message
        /// </summary>
        private string progressMessage;

        /// <summary>
        /// The progress value
        /// </summary>
        private int progressValue;

        #endregion

        #region - Properties -

        /// <summary>
        /// Gets or sets the index of the selected tab.
        /// </summary>
        /// <value>
        /// The index of the selected tab.
        /// </value>
        public int SelectedTabIndex
        {
            get { return this.selectedTabIndex; }
            set { this.SetProperty<int>(ref this.selectedTabIndex, value); }
        }

        /// <summary>
        /// Gets or sets the selected path.
        /// </summary>
        /// <value>
        /// The selected path.
        /// </value>
        public string SelectedPath
        {
            get { return this.selectedPath; }
            set { this.SetProperty<string>(ref this.selectedPath, value); }
        }

        /// <summary>
        /// Gets or sets the selected output path.
        /// </summary>
        /// <value>
        /// The selected output path.
        /// </value>
        public string SelectedOutputPath
        {
            get { return this.selectedOutputPath; }
            set { this.SetProperty<string>(ref this.selectedOutputPath, value); }
        }

        /// <summary>
        /// Gets the browse file command.
        /// </summary>
        /// <value>
        /// The browse file command.
        /// </value>
        public ICommand BrowseFileCommand
        {
            get { return new RelayCommand(BrowseFileAction); }
        }

        /// <summary>
        /// Gets the browse folder command.
        /// </summary>
        /// <value>
        /// The browse folder command.
        /// </value>
        public ICommand BrowseFolderCommand
        {
            get { return new RelayCommand(BrowseFolderAction); }
        }

        /// <summary>
        /// Gets the command to move to the Tables tab.
        /// </summary>
        /// <value>
        /// The move to page tables command.
        /// </value>
        public ICommand MoveToPageTablesCommand
        {
            get { return this.moveToPageTablesCommand; }
        }

        /// <summary>
        /// Gets the move to page build command.
        /// </summary>
        /// <value>
        /// The move to page build command.
        /// </value>
        public ICommand MoveToPageBuildCommand
        {
            get { return this.moveToPageBuildCommand; }
        }

        /// <summary>
        /// Gets or sets the templates path.
        /// </summary>
        /// <value>
        /// The templates path.
        /// </value>
        public string TemplatesPath
        {
            get { return this.templatesPath; }
            set { this.templatesPath = value; }
        }

        /// <summary>
        /// Gets the template list.
        /// </summary>
        /// <value>
        /// The template list.
        /// </value>
        public ObservableCollection<string> TemplateList
        {
            get { return this.templateList; }
        }

        /// <summary>
        /// Gets or sets the selected template.
        /// </summary>
        /// <value>
        /// The selected template.
        /// </value>
        public string SelectedTemplate
        {
            get { return this.selectedTemplate; }
            set { this.SetProperty<string>(ref this.selectedTemplate, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [include metamodel].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [include metamodel]; otherwise, <c>false</c>.
        /// </value>
        public bool IncludeMeta
        {
            get { return this.includeMeta; }
            set { this.SetProperty<bool>(ref this.includeMeta, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is loading tables.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is loading tables; otherwise, <c>false</c>.
        /// </value>
        public bool IsLoadingTables
        {
            get { return this.isLoadingTables; }
            set
            {
                this.SetProperty<bool>(ref this.isLoadingTables, value);
                this.NotifyOfPropertyChange("IsNotLoadingTables");
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is not loading tables.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is not loading tables; otherwise, <c>false</c>.
        /// </value>
        public bool IsNotLoadingTables
        {
            get { return !this.isLoadingTables; }
        }

        /// <summary>
        /// Gets the root entities.
        /// </summary>
        /// <value>
        /// The root entities.
        /// </value>
        public ObservableCollection<EntityViewModel> RootEntities
        {
            get { return this.rootEntities; }
        }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        public ObservableKeyValue<string, string> Parameters
        {
            get { return this.parameters; }
        }

        /// <summary>
        /// Gets or sets the selected parameter.
        /// </summary>
        /// <value>
        /// The selected parameter.
        /// </value>
        public ObservableKeyValueItem<string, string> SelectedParameter
        {
            get { return this.selectedParameter; }
            set { this.SetProperty<ObservableKeyValueItem<string, string>>(ref this.selectedParameter, value); }
        }

        /// <summary>
        /// Gets the edit key value command.
        /// </summary>
        /// <value>
        /// The edit key value command.
        /// </value>
        public RelayCommand EditKeyValueCommand
        {
            get { return new RelayCommand(this.EditKeyValue, this.CanEditOrRemoveKeyValue); }
        }

        /// <summary>
        /// Gets the remove key value command.
        /// </summary>
        /// <value>
        /// The remove key value command.
        /// </value>
        public RelayCommand RemoveKeyValueCommand
        {
            get { return new RelayCommand(this.RemoveKeyValue, this.CanEditOrRemoveKeyValue); }
        }

        /// <summary>
        /// Gets or sets the progress message.
        /// </summary>
        /// <value>
        /// The progress message.
        /// </value>
        public string ProgressMessage
        {
            get { return this.progressMessage; }
            set { this.SetProperty<string>(ref this.progressMessage, value); }
        }

        /// <summary>
        /// Gets or sets the progress value.
        /// </summary>
        /// <value>
        /// The progress value.
        /// </value>
        public int ProgressValue
        {
            get { return this.progressValue; }
            set { this.SetProperty<int>(ref this.progressValue, value); }
        }

        #endregion

        #region - Methods -

        private void LoadParameters(string fileName)
        {
            XmlDocument document = XmlResources.GetFromResource(fileName);
            if (document != null)
            {
                foreach (XmlNode node in document.SelectNodes("parameters/parameter"))
                {
                    NodeAttributes attr = new NodeAttributes(node);
                    this.Parameters.Add(attr.AsString("name"), attr.AsString("value"));
                }
            }
        }

        /// <summary>
        /// Called when initializing.
        /// </summary>
        protected override void OnInitialize()
        {
            base.OnInitialize();
            this.moveToPageTablesCommand = new RelayAsyncCommand(this.MoveToPageTables, this.GetIsValidated);
            this.moveToPageTablesCommand.Started += this.MoveToPageTablesCommandStarted;
            this.moveToPageTablesCommand.Ended += this.MoveToPageTablesCommandEnded;

            this.moveToPageBuildCommand = new RelayAsyncCommand(this.MoveToPageBuild);
            this.moveToPageBuildCommand.Started += this.MoveToPageBuildCommandStarted;
            this.moveToPageBuildCommand.Ended += this.MoveToPageBuildCommandEnded;

            //this.Parameters.Add("Author", Environment.UserName);
            //this.Parameters.Add("AuthorName", "Jesús Seijas");
            //this.Parameters.Add("Namespace", "C4Net.JC3IEDM");
            //this.Parameters.Add("ProductName", "C4Net");
            //this.Parameters.Add("DatabaseName", "Test");
            //this.Parameters.Add("DatabaseSchema", "JC3IEDM");

            this.DisplayName = "Metamodel Tool";
            this.templatesPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Templates";
            this.LoadTemplateList();
        }

        /// <summary>
        /// Abstract method to initialize the flyouts.
        /// </summary>
        protected override void InitializeFlyouts()
        {
            this.AddFlyout(new FlyoutKeyValueViewModel());
        }

        /// <summary>
        /// Action for browse for a file.
        /// </summary>
        private void BrowseFileAction()
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Access files|*.mdb;*.accdb";
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.SelectedPath = dialog.FileName;
            }
        }

        /// <summary>
        /// Browses the folder action.
        /// </summary>
        private void BrowseFolderAction()
        {
            var dialog = new FolderBrowserDialog();
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.SelectedOutputPath = dialog.SelectedPath;
            }
        }

        /// <summary>
        /// Gets the value of the IsValidated property.
        /// </summary>
        /// <returns></returns>
        private bool GetIsValidated()
        {
            return this.IsValidated;
        }

        /// <summary>
        /// Moves to page configuration.
        /// </summary>
        public void MoveToPageConfiguration()
        {
            this.SelectedTabIndex = 0;
        }

        /// <summary>
        /// Changes the active tab of the tab control to the Tables page.
        /// </summary>
        private void MoveToPageTables()
        {
            this.SelectedTabIndex = 1;
            this.metamodelLoader.LoadTablesFrom(this.SelectedPath, this.IncludeMeta);
        }

        /// <summary>
        /// Moves back to page tables.
        /// </summary>
        public void MoveToPageTablesBack()
        {
            this.SelectedTabIndex = 1;
        }

        /// <summary>
        /// Moves to page parameters.
        /// </summary>
        public void MoveToPageParameters()
        {
            this.SelectedTabIndex = 2;
            this.LoadParameters(this.templatesPath + "\\" + this.SelectedTemplate + "\\Parameters.xml");
        }

        /// <summary>
        /// Moves to page build.
        /// </summary>
        public void MoveToPageBuild()
        {
            this.SelectedTabIndex = 3;
            this.Build();
        }

        /// <summary>
        /// Start the command move to page tables.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void MoveToPageTablesCommandStarted(object sender, EventArgs e)
        {
            this.IsLoadingTables = true;
        }

        /// <summary>
        /// Ends the command move to page tables.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void MoveToPageTablesCommandEnded(object sender, EventArgs e)
        {
            this.IsLoadingTables = false;
            this.rootEntities.Clear();
            this.entities.Clear();
            foreach (Entity entity in this.metamodelLoader.Entities.Values)
            {
                if (entity.DepthCnt == 0)
                {
                    EntityViewModel entityVM = new EntityViewModel(null, entity);
                    this.entities.Add(entityVM.EntId, entityVM);
                    this.rootEntities.Add(entityVM);
                }
                else
                {
                    SubtypeRelationship rel = this.metamodelLoader.GetSubtypeWhereSonIs(entity);
                    if (rel == null)
                    {
                        EntityViewModel entityVM = new EntityViewModel(null, entity);
                        this.entities.Add(entityVM.EntId, entityVM);
                        this.rootEntities.Add(entityVM);
                    }
                    else
                    {
                        EntityViewModel parentEntity = this.entities[rel.SupEntId];
                        EntityViewModel entityVM = parentEntity.AddSon(entity);
                        this.entities.Add(entityVM.EntId, entityVM);
                    }
                }
            }
            this.SelectAllEntities();
        }

        /// <summary>
        /// Moves to page build command started.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void MoveToPageBuildCommandStarted(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Moves to page build command ended.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void MoveToPageBuildCommandEnded(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Validates the column.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        protected override string ValidateColumn(string columnName)
        {
            if (columnName.Equals("SelectedPath"))
            {
                if (string.IsNullOrEmpty(this.SelectedPath))
                {
                    return "You must select a file";
                }
                if (!File.Exists(this.SelectedPath))
                {
                    return "File must exists";
                }
            }
            if (columnName.Equals("SelectedOutputPath"))
            {
                if (string.IsNullOrEmpty(this.SelectedOutputPath))
                {
                    return "You must select a path";
                }
            }
            return base.ValidateColumn(columnName);
        }

        /// <summary>
        /// Loads the template list.
        /// </summary>
        private void LoadTemplateList()
        {
            this.templateList.Clear();
            string[] folders = Directory.GetDirectories(this.templatesPath);
            foreach (string folder in folders)
            {
                if (File.Exists(folder + "\\TemplateRules.xml"))
                {
                    string[] directories = folder.Split(Path.DirectorySeparatorChar);
                    this.templateList.Add(directories[directories.Length - 1]);
                }
            }
            this.SelectedTemplate = this.templateList[0];
        }

        /// <summary>
        /// Selects all entities.
        /// </summary>
        public void SelectAllEntities()
        {
            foreach (EntityViewModel entity in this.entities.Values)
            {
                entity.IsSelected = true;
            }
        }

        /// <summary>
        /// Unselects all entities.
        /// </summary>
        public void UnselectAllEntities()
        {
            foreach (EntityViewModel entity in this.entities.Values)
            {
                entity.IsSelected = false;
            }
        }

        /// <summary>
        /// Determines whether this instance [can edit or remove key value].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance [can edit or remove key value]; otherwise, <c>false</c>.
        /// </returns>
        public bool CanEditOrRemoveKeyValue()
        {
            return this.SelectedParameter != null;
        }

        /// <summary>
        /// Adds a new parameter.
        /// </summary>
        public void AddNewKeyValue()
        {
            FlyoutKeyValueViewModel flyout = (FlyoutKeyValueViewModel)this.GetFlyout("Parameter");
            flyout.Add();
        }

        /// <summary>
        /// Edits the selected parameter.
        /// </summary>
        public void EditKeyValue()
        {
            FlyoutKeyValueViewModel flyout = (FlyoutKeyValueViewModel)this.GetFlyout("Parameter");
            flyout.Edit(this.SelectedParameter);
        }

        /// <summary>
        /// Removes the selected parameter.
        /// </summary>
        public void RemoveKeyValue()
        {
            this.Parameters.Remove(this.SelectedParameter);
        }

        /// <summary>
        /// Sets the progress.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="value">The value.</param>
        private void SetProgress(string message, int value)
        {
            this.ProgressMessage = message;
            this.ProgressValue = value;
        }

        /// <summary>
        /// Builds this instance.
        /// </summary>
        private void Build()
        {
            this.SetProgress("Loading metamodel from database...", 0);
            this.metamodelLoader.LoadMetamodel();
            this.SetProgress("Generating metamodel structures...", 10);
            this.SetProgress("Selecting structures...", 20);
            foreach (EntityViewModel entityVM in this.entities.Values)
            {
                if ((!entityVM.IsSelected.HasValue) || (entityVM.IsSelected.Value))
                {
                    this.metamodelLoader.SelectEntity(entityVM.EntId);
                }
            }
            this.SetProgress("Generating template container...", 30);
            TemplateContainer container = this.metamodelLoader.BuildContainer();
            // Add own variables.
            container.AddAttribute("OutputPath", this.SelectedOutputPath);
            foreach (ObservableKeyValueItem<string, string> okv in this.Parameters)
            {
                container.AddAttribute(okv.Key, okv.Value);
            }
            this.SetProgress("Generating template...", 40);
            //// 60% all for the generation.

            TemplateEngine engine = new TemplateEngine();
            engine.LoadRulesFromXml(this.templatesPath + "\\" + this.SelectedTemplate + "\\TemplateRules.xml", this.templatesPath+"\\"+this.selectedTemplate);
            engine.Iterate(container);
            this.SetProgress("Finish", 100);
        }

        public void DoClose()
        {
            this.TryClose();
        }

        #endregion
    }
}
