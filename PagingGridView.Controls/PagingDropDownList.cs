// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PagingDropDownList.cs" company="Brightstar Corporation">
//   GroundTruth-Copyright (c) Brightstar Corporation. All rights reserved.
// </copyright>
// <summary>
//   Paging drop down list
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BrightStar.Retail.PagingGridView.Controls
{
    using System;
    using System.ComponentModel;
    using System.Web.UI.WebControls;

    /// <summary>
    /// Paging drop down list
    /// </summary>
    public class PagingDropDownList : DropDownList
    {
        #region Commands
        /// <summary>
        /// Previous command name
        /// </summary>
        private const string Previous = "PrevDropDownList";

        /// <summary>
        /// Next command name
        /// </summary>
        private const string Next = "NextDropDownList";

        /// <summary>
        /// Reassign data source handler
        /// </summary>
        public event EventHandler NeedDataSource;
        #endregion

        #region Types and Properties
        /// <summary>
        /// Pager types
        /// </summary>
        public enum PagerDDLTypes
        {
            /// <summary>
            /// Normal type
            /// </summary>
            Regular = 0,
            /// <summary>
            /// Custome type
            /// </summary>
            Custom = 1
        }

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(10), Category("Paging"), Description("Indicates whether to use page size is 10")]
        public int PageSize
        {
            get { return (ViewState["PageSize"] != null) ? (int)ViewState["PageSize"] : 10; }
            set { ViewState["PageSize"] = value; }
        }

        /// <summary>
        /// Get or Set Pager drop down list types. Default value is Custom type.
        /// </summary>
        [DefaultValue(PagerDDLTypes.Custom), Category("Paging"), Description("Indicates whether to use the built-in custom pager or not.")]
        public PagerDDLTypes PagerDDLType
        {
            get
            {
                return (ViewState["PagerDDLType"] != null) ? (PagerDDLTypes)ViewState["PagerDDLType"] : PagerDDLTypes.Custom;
            }
            set
            {
                ViewState["PagerDDLType"] = value;
            }
        }

        /// <summary>
        /// Get or Set Skip property
        /// </summary>
        public int Skip
        {
            get
            {
                return (ViewState["Skip"] != null) ? (int)ViewState["Skip"] : 0;
            }
            set
            {
                ViewState["Skip"] = value;
            }
        }
        #endregion

        #region Override events
        /// <summary>
        /// Raises the System.Web.UI.WebControls.ListControl.SelectedIndexChanged event.
        ///     This allows you to provide a custom handler for the event.
        /// </summary>
        /// <param name="e">A System.EventArgs that contains the event data.</param>
        protected override void OnSelectedIndexChanged(System.EventArgs e)
        {
            base.OnSelectedIndexChanged(e);

            if (PagerDDLType != PagerDDLTypes.Custom)
                return;

            switch (this.SelectedValue)
            {
                case Next:
                    Skip += PageSize;
                    FetchData();
                    break;

                case Previous:
                    Skip -= PageSize;
                    FetchData();
                    break;
            }
        }

        /// <summary>
        /// Fetch data base on new data suorce
        /// </summary>
        private void FetchData()
        {
            this.Items.Clear();

            if (NeedDataSource != null && this.DataSource == null)
                NeedDataSource(this, EventArgs.Empty);

            this.SelectedIndex = (Skip > 0) ? 1 : 0;
        }

        /// <summary>
        /// Binds a data source to the invoked server control and all its child controls.
        /// </summary>
        public override void DataBind()
        {
            this.Items.Clear();
            base.DataBind();
            if (PagerDDLType != PagerDDLTypes.Custom)
                return;

            if (Skip > 0 && this.Items.Count > 0)
            {
                ListItem firstItem = new ListItem("<< " + Properties.Resources.PreDropDownList + " " +
                    PageSize.ToString() + " " + Properties.Resources.RowsName, Previous);
                if (!this.Items.Contains(firstItem))
                {
                    this.Items.Insert(0, firstItem);
                    this.AppendDataBoundItems = true;
                }
            }
            if (this.Items.Count >= PageSize)
            {
                ListItem nextItem = new ListItem(Properties.Resources.NextDropDownList + " " +
                    PageSize.ToString() + " " + Properties.Resources.RowsName + " >>", Next);
                if (!this.Items.Contains(nextItem))
                {
                    this.Items.Add(nextItem);
                }
            }
            if (this.Items.Count > 1)
                this.SelectedIndex = (Skip > 0) ? 1 : 0;
        }
        #endregion
    }
}
