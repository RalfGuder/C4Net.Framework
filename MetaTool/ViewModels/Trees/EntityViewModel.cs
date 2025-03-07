using System.Collections.ObjectModel;
using C4Net.Framework.MVVM;
using MetaTool.Business.Entities;

namespace MetaTool.ViewModels.Trees
{
    /// <summary>
    /// View model for an entity tree representation 
    /// </summary>
    public class EntityViewModel : NotifyObject
    {
        #region - Fields -

        /// <summary>
        /// The parent entity view model, null for root.
        /// </summary>
        private EntityViewModel parent = null;

        /// <summary>
        /// The attached ent.
        /// </summary>
        private Entity ent = null;

        /// <summary>
        /// The sons
        /// </summary>
        private ObservableCollection<EntityViewModel> sons = new ObservableCollection<EntityViewModel>();

        /// <summary>
        /// Indicates if is full selected, full unselected or partially selected.
        /// </summary>
        private bool? isSelected = false;

        #endregion

        #region - Properties -

        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        /// <value>
        /// The parent.
        /// </value>
        public EntityViewModel Parent
        {
            get { return this.parent; }
            set { this.SetProperty<EntityViewModel>(ref this.parent, value); }
        }

        /// <summary>
        /// Gets the sons.
        /// </summary>
        /// <value>
        /// The sons.
        /// </value>
        public ObservableCollection<EntityViewModel> Sons
        {
            get { return this.sons; }
        }

        /// <summary>
        /// Gets the ent.
        /// </summary>
        /// <value>
        /// The ent.
        /// </value>
        public Entity Ent
        {
            get { return this.ent; }
        }

        /// <summary>
        /// Gets the entity id.
        /// </summary>
        /// <value>
        /// The ent id.
        /// </value>
        public double EntId
        {
            get { return this.ent.EntId; }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get { return this.ent.NameTxt; }
        }

        /// <summary>
        /// Gets or sets the selected status.
        /// </summary>
        /// <value>
        /// The is selected.
        /// </value>
        public bool? IsSelected
        {
            get { return this.isSelected; }
            set
            {
                if (this.isSelected != value)
                {
                    this.isSelected = value;
                    if (value.HasValue)
                    {
                        if (value.Value)
                        {
                            this.SelectAllSons();
                        }
                        else
                        {
                            this.UnselectAllSons();
                        }
                    }
                    if (this.Parent != null)
                    {
                        this.Parent.RecalculateValue();
                    }
                    this.NotifyOfPropertyChange();
                }
            }
        }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityViewModel"/> class.
        /// </summary>
        public EntityViewModel()
        {
            this.Parent = null;
            this.ent = new Entity();
            ent.NameTxt = "-- ROOT --";
            ent.EntId = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityViewModel"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="ent">The ent.</param>
        public EntityViewModel(EntityViewModel parent, Entity ent)
        {
            this.Parent = parent;
            this.ent = ent;
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Adds the son.
        /// </summary>
        /// <param name="ent">The ent.</param>
        /// <returns></returns>
        public EntityViewModel AddSon(Entity ent)
        {
            EntityViewModel result = new EntityViewModel(this, ent);
            this.Sons.Add(result);
            return result;
        }

        /// <summary>
        /// Selects all sons.
        /// </summary>
        private void SelectAllSons()
        {
            foreach (EntityViewModel son in this.Sons)
            {
                son.IsSelected = true;
            }
        }

        /// <summary>
        /// Unselects all sons.
        /// </summary>
        private void UnselectAllSons()
        {
            foreach (EntityViewModel son in this.Sons)
            {
                son.IsSelected = false;
            }
        }

        /// <summary>
        /// Indicates if alls the sons are selected.
        /// </summary>
        /// <returns></returns>
        private bool AllSonsSelected()
        {
            foreach (EntityViewModel son in this.Sons)
            {
                if ((!(son.IsSelected.HasValue && son.IsSelected.Value)) || (!son.AllSonsSelected()))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Indicates if alls the sons are unselected.
        /// </summary>
        /// <returns></returns>
        private bool AllSonsUnselected()
        {
            foreach (EntityViewModel son in this.Sons)
            {
                if ((!(son.IsSelected.HasValue && (!son.IsSelected.Value))) || (!son.AllSonsUnselected()))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Recalculates the value.
        /// </summary>
        private void RecalculateValue()
        {
            if (this.AllSonsSelected())
            {
                this.IsSelected = true;
            }
            else if (this.AllSonsUnselected())
            {
                this.IsSelected = false;
            }
            else
            {
                this.IsSelected = null;
            }
            if (this.Parent != null)
            {
                this.Parent.RecalculateValue();
            }
        }

        #endregion
    }
}
