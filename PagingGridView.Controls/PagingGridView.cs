// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PagingGridView.cs" company="Brightstar Corporation">
//   GroundTruth-Copyright (c) Brightstar Corporation. All rights reserved.
// </copyright>
// <summary>
//   Defines the PagingGridView type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BrightStar.Retail.PagingGridView.Controls
{
    using System;
    using System.ComponentModel;
    using System.Web.UI.WebControls;

    public class PagingGridView : GridView
    {
        #region Command Const

        /// <summary>
        /// First command name
        /// </summary>
        private const string First = "First";

        /// <summary>
        /// Previous command name
        /// </summary>
        private const string Previous = "Previous";

        /// <summary>
        /// Next command name
        /// </summary>
        private const string Next = "Next";

        /// <summary>
        /// Last command name
        /// </summary>
        private const string Last = "Last";

        /// <summary>
        /// The name of custom page index key
        /// </summary>
        private const string CustomPageIndexKey = "CustomPageIndexKey";

        /// <summary>
        /// Custom sort direction
        /// </summary>
        private const string CustomSortDirectionViewStateName = "CustomSortDirection";

        /// <summary>
        /// Custom sort expression
        /// </summary>
        private const string CustomSortExpressionViewStateName = "CustomSortExpression";

        #endregion

        #region Types and Properties

        /// <summary>
        /// Pager Types.
        /// </summary>
        public enum PagerTypes
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
        /// GoToPage Types.
        /// </summary>
        public enum GoToPageTypes
        {
            /// <summary>
            /// TextBox view.
            /// </summary>
            TextBoxView = 0,
            /// <summary>
            /// DropDown list view
            /// </summary>
            DropDownView = 1
        }

        /// <summary>
        /// Page index
        /// </summary>
        public override int PageIndex
        {
            get
            {
                if (!UseObjectDataSource)
                {
                    return this.CustomPageIndex;
                }
                return base.PageIndex;
            }
            set
            {
                base.PageIndex = value;
                CustomPageIndex = value;
            }
        }

        /// <summary>
        /// Page Index when no use object data source
        /// </summary>
        private int CustomPageIndex
        {
            get
            {
                return (int)(ViewState[CustomPageIndexKey] ?? 0);
            }
            set
            {
                ViewState[CustomPageIndexKey] = value;
            }
        }

        /// <summary>
        /// Default true, use objectdatasource for gridview
        /// set false if use List, DataTable, ... 
        /// </summary>
        [DefaultValue(true), Category("Paging"), Description("Indicates whether to use the object datasource or not")]
        public bool UseObjectDataSource
        {
            get
            {
                if (ViewState["UseObjectDataSource"] != null)
                    return (bool)ViewState["UseObjectDataSource"];
                else
                    return true;
            }
            set
            {
                ViewState["UseObjectDataSource"] = value;
            }
        }

        /// <summary>
        /// Get or set the CSS of Paging Row.
        /// </summary>
        public string PagingRowCssClass { get; set; }

        /// <summary>
        /// Get or Set Pager Types. Default value is Custom type.
        /// </summary>
        [DefaultValue(PagerTypes.Custom), Category("Paging"), Description("Indicates whether to use the built-in custom pager or not.")]
        public PagerTypes PagerType
        {
            get
            {
                if (ViewState["PagerType"] != null)
                    return (PagerTypes)ViewState["PagerType"];
                else
                    return PagerTypes.Custom;
            }
            set { ViewState["PagerType"] = value; }
        }

        /// <summary>
        /// Get or Set GoToPage view. Default value is TextBox view.
        /// </summary>
        [DefaultValue(GoToPageTypes.TextBoxView), Category("Paging"), Description("Indicates whether to use the text box number or drop down list")]
        public GoToPageTypes GoToPageType
        {
            get
            {
                if (ViewState["GoToPageType"] != null)
                    return (GoToPageTypes)ViewState["GoToPageType"];
                else
                    return GoToPageTypes.TextBoxView;
            }
            set { ViewState["GoToPageType"] = value; }
        }

        /// <summary>
        /// Get or Set Page total
        /// </summary>
        private int TotalPages
        {
            get;
            set;
        }

        /// <summary>
        /// Get or Set Page total
        /// </summary>
        public int RowCount
        {
            get
            {
                if (ViewState["RowCount"] != null)
                    return (int)ViewState["RowCount"];
                else
                    return 0;
            }
            set
            {
                ViewState["RowCount"] = value;
            }
        }

        /// <summary>
        /// Get or Set Custom sort direction
        /// </summary>
        public string CustomSortDirection
        {
            get
            {
                return ViewState[CustomSortDirectionViewStateName] as string ?? string.Empty;
            }
            set
            {
                ViewState[CustomSortDirectionViewStateName] = value;
            }
        }

        /// <summary>
        /// Get or Set Custom sort expression
        /// </summary>
        public string CustomSortExpression
        {
            get
            {
                return ViewState[CustomSortExpressionViewStateName] as string ?? string.Empty;
            }
            set
            {
                ViewState[CustomSortExpressionViewStateName] = value;
            }
        }
        #endregion

        /// <summary>
        /// Override GridView InittializePager function
        /// </summary>
        /// <param name="row">Grid View row</param>
        /// <param name="columnSpan">column span</param>
        /// <param name="pagedDataSource">Paged Data Source</param>
        protected override void InitializePager(System.Web.UI.WebControls.GridViewRow row, int columnSpan, System.Web.UI.WebControls.PagedDataSource pagedDataSource)
        {
            switch (this.PagerType)
            {
                case PagerTypes.Custom:
                    InitCustomPager(row, columnSpan, pagedDataSource);
                    break;
                default:
                    base.InitializePager(row, columnSpan, pagedDataSource);
                    break;
            }
        }

        /// <summary>
        /// Initialize custom pager
        /// </summary>
        /// <param name="row">Grid View row</param>
        /// <param name="columnSpan">column span</param>
        /// <param name="pagedDataSource">Paged Data Source</param>
        private void InitCustomPager(System.Web.UI.WebControls.GridViewRow row, int columnSpan, System.Web.UI.WebControls.PagedDataSource pagedDataSource)
        {
            int PageLinksToShow = 5;
            if (!UseObjectDataSource)
            {
                pagedDataSource.AllowPaging = true;
                pagedDataSource.CurrentPageIndex = this.CustomPageIndex;
                pagedDataSource.AllowCustomPaging = true;
                pagedDataSource.AllowServerPaging = true;
                pagedDataSource.VirtualCount = RowCount;
                TotalPages = (RowCount % this.PageSize) > 0 ? (RowCount / this.PageSize) + 1 : (RowCount / this.PageSize);
            }
            else
            {
                TotalPages = pagedDataSource.PageCount;
                RowCount = pagedDataSource.DataSourceCount;
            }
            int min = Math.Min(Math.Max(0, PageIndex - (PageLinksToShow / 2)),
                               Math.Max(0, TotalPages - PageLinksToShow + 1));
            int max = Math.Min(TotalPages, min + PageLinksToShow);
            Literal ltlPageIndex = new Literal();
            ltlPageIndex.ID = "ltlPageIndex";
            ltlPageIndex.Text = (this.PageIndex * PageSize + 1).ToString() + " - " + (this.PageIndex * PageSize + Rows.Count).ToString();
            Literal ltlPageCount = new Literal();
            ltlPageCount.ID = "ltlPageCount";
            ltlPageCount.Text = RowCount.ToString();
            string ShowingEntries = string.Format("Showing {0} of {1} entries", ltlPageIndex.Text, ltlPageCount.Text);

            Controls.Add(new Literal { Text = "<div class=\"row\">" });
            Controls.Add(new Literal { Text = "<div class=\"col-xs-5\">" });
            Controls.Add(new Literal { Text = "<div class=\"dataTables_info\">" + ShowingEntries + "</div></div>" });
            Controls.Add(new Literal { Text = "<div class=\"col-xs-7\">" });
            Controls.Add(new Literal { Text = "<div class=\"dataTables_paginate paging_bootstrap\"><ul class=\"pagination\">" });

            AddLink("Previous", "disabled", PageIndex != 0, (PageIndex-1).ToString());
            for (int i = min; i < max; i++)
            {
                AddLink((i + 1).ToString(), "active", PageIndex != i, i.ToString());
            }
            AddLink("Next", "disabled", PageIndex != TotalPages - 1, (PageIndex + 1).ToString());

            Controls.Add(new Literal { Text = "</ul></div></div></div>" });
            //Panel pnlPager = new Panel();
            //pnlPager.ID = "pnlPager";
            //pnlPager.CssClass = this.PagerStyle.CssClass;

            //Table tblPager = new Table();
            //tblPager.ID = "tblPager";
            //tblPager.CellPadding = 3;
            //tblPager.CellSpacing = 0;
            //tblPager.Style.Add("width", "100%");
            //tblPager.Style.Add("height", "100%");
            //tblPager.BorderStyle = BorderStyle.None;
            //tblPager.GridLines = GridLines.None;

            //TableRow trPager = new TableRow();
            //trPager.ID = "trPager";
            //trPager.CssClass = string.IsNullOrEmpty(PagingRowCssClass) ? "paging-row" : PagingRowCssClass;

            //Literal ltlPageIndex = new Literal();
            //ltlPageIndex.ID = "ltlPageIndex";
            //ltlPageIndex.Text = (this.PageIndex * PageSize + 1).ToString() + "-" + (this.PageIndex * PageSize + Rows.Count).ToString();


            //Literal ltlPageCount = new Literal();
            //ltlPageCount.ID = "ltlPageCount";
            //ltlPageCount.Text = RowCount.ToString();

            //TableCell tcPageXofY = new TableCell();
            //tcPageXofY.ID = "tcPageXofY";
            //tcPageXofY.Style.Add("width", "30%");
            //tcPageXofY.Style.Add("text-align", "left");
            //tcPageXofY.Style.Add("padding-left", "5px");
            //tcPageXofY.Style.Add("border", "0px");
            //tcPageXofY.Controls.Add(new LiteralControl("Showing "));
            //tcPageXofY.Controls.Add(ltlPageIndex);
            //tcPageXofY.Controls.Add(new LiteralControl(" of "));
            //tcPageXofY.Controls.Add(ltlPageCount);

            //ImageButton ibtnFirst = new ImageButton();
            //ibtnFirst.ID = "ibtnFirst";
            //ibtnFirst.CommandName = First;
            //ibtnFirst.ToolTip = Properties.Resources.FirstPageToolTip;
            //ibtnFirst.ImageAlign = ImageAlign.AbsMiddle;
            //ibtnFirst.Style.Add("cursor", "pointer");
            //ibtnFirst.CausesValidation = false;
            //ibtnFirst.Command += this.PagerCommand;

            //ImageButton ibtnPrevious = new ImageButton();
            //ibtnPrevious.ID = "ibtnPrevious";
            //ibtnPrevious.CommandName = Previous;
            //ibtnPrevious.ToolTip = Properties.Resources.PreviousPageToolTip;
            //ibtnPrevious.ImageAlign = ImageAlign.AbsMiddle;
            //ibtnPrevious.Style.Add("cursor", "pointer");
            //ibtnPrevious.CausesValidation = false;
            //ibtnPrevious.Command += this.PagerCommand;

            //ImageButton ibtnNext = new ImageButton();
            //ibtnNext.ID = "ibtnNext";
            //ibtnNext.CommandName = Next;
            //ibtnNext.ToolTip = Properties.Resources.NextPageToolTip;
            //ibtnNext.ImageAlign = ImageAlign.AbsMiddle;
            //ibtnNext.Style.Add("cursor", "pointer");
            //ibtnNext.CausesValidation = false;
            //ibtnNext.Command += this.PagerCommand;

            //ImageButton ibtnLast = new ImageButton();
            //ibtnLast.ID = "ibtnLast";
            //ibtnLast.CommandName = Last;
            //ibtnLast.ToolTip = Properties.Resources.LastPageToolTip;
            //ibtnLast.ImageAlign = ImageAlign.AbsMiddle;
            //ibtnLast.Style.Add("cursor", "pointer");
            //ibtnLast.CausesValidation = false;
            //ibtnLast.Command += this.PagerCommand;

            //string url = "~/Images/GridViewIcons/";
            //if (this.PageIndex > 0)
            //{
            //    ibtnFirst.ImageUrl = url + "page-first.gif";
            //    ibtnPrevious.ImageUrl = url + "page-prev.gif";
            //    ibtnFirst.Enabled = true;
            //    ibtnPrevious.Enabled = true;
            //}
            //else
            //{
            //    ibtnFirst.ImageUrl = url + "page-first-disabled.gif";
            //    ibtnPrevious.ImageUrl = url + "page-prev-disabled.gif";
            //    ibtnFirst.Enabled = false;
            //    ibtnPrevious.Enabled = false;
            //    ibtnFirst.Style.Add("cursor", "default");
            //    ibtnPrevious.Style.Add("cursor", "default");
            //}

            //if (this.PageIndex < TotalPages - 1)
            //{
            //    ibtnNext.ImageUrl = url + "page-next.gif";
            //    ibtnLast.ImageUrl = url + "page-last.gif";
            //    ibtnNext.Enabled = true;
            //    ibtnLast.Enabled = true;
            //}
            //else
            //{
            //    ibtnNext.ImageUrl = url + "page-next-disabled.gif";
            //    ibtnLast.ImageUrl = url + "page-last-disabled.gif";
            //    ibtnNext.Enabled = false;
            //    ibtnLast.Enabled = false;
            //    ibtnNext.Style.Add("cursor", "default");
            //    ibtnLast.Style.Add("cursor", "default");
            //}

            //TableCell tcPagerBtns = new TableCell();
            //tcPagerBtns.ID = "tcPagerBtns";
            //tcPagerBtns.Style.Add("width", "40%");
            //tcPagerBtns.Style.Add("text-align", "right");
            //tcPagerBtns.Style.Add("border", "0px");
            //tcPagerBtns.Controls.Add(ibtnFirst);
            //tcPagerBtns.Controls.Add(ibtnPrevious);
            //if (GoToPageType == GoToPageTypes.TextBoxView)
            //{
            //    TextBox tbxPages = new TextBox();
            //    tbxPages.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            //    tbxPages.ID = "PagesTextBox";
            //    tbxPages.AutoPostBack = true;
            //    tbxPages.Text = (this.PageIndex + 1).ToString();
            //    tbxPages.Width = Unit.Pixel(40);
            //    tbxPages.CausesValidation = false;
            //    tbxPages.TextChanged += this.SelectedIndexPagesChanged;

            //    FilteredTextBoxExtender filterNumeric = new FilteredTextBoxExtender();
            //    filterNumeric.FilterType = FilterTypes.Numbers;
            //    filterNumeric.TargetControlID = tbxPages.ClientID;

            //    tcPagerBtns.Controls.Add(tbxPages);
            //    tcPagerBtns.Controls.Add(filterNumeric);
            //    tcPagerBtns.Controls.Add(new LiteralControl(" / " + TotalPages.ToString()));
            //}
            //else
            //{
            //    DropDownList ddlPages = new DropDownList();
            //    ddlPages.ID = "ddlPages";
            //    ddlPages.AutoPostBack = true;
            //    for (int i = 1; i <= TotalPages; i++)
            //    {
            //        ddlPages.Items.Add(new ListItem(i.ToString(), i.ToString()));
            //    }
            //    ddlPages.SelectedIndex = this.PageIndex;
            //    ddlPages.CausesValidation = false;
            //    ddlPages.SelectedIndexChanged += this.SelectedIndexPagesChanged;
            //    tcPagerBtns.Controls.Add(ddlPages);
            //}
            //tcPagerBtns.Controls.Add(ibtnNext);
            //tcPagerBtns.Controls.Add(ibtnLast);

            ////add cells to row
            //trPager.Cells.Add(tcPageXofY);
            //trPager.Cells.Add(tcPagerBtns);

            ////add row to table
            //tblPager.Rows.Add(trPager);

            ////add table to div
            //pnlPager.Controls.Add(tblPager);

            ////add div to pager row
            //row.Controls.AddAt(0, new TableCell());
            //row.Cells[0].ColumnSpan = columnSpan;
            //row.Cells[0].Controls.Add(pnlPager);
            //row.Cells[0].CssClass = "paging-cell";
            //}
        }

        /// <summary>
        /// Adds the link for the page (and next/last etc) or a label if its a deactivated link
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="cssClass">The CSS class.</param>
        /// <param name="addAsLink">if set to <c>true</c> [add as link].</param>
        /// <param name="commandArgument">The command argument.</param>
        private void AddLink(String text, String cssClass, bool addAsLink, string commandArgument)
        {
            string content = "";
            if (addAsLink)
            {
                content = "<li>";
            }
            else
            {
                content = "<li class=\"" + cssClass + "\">";
            }
            Controls.Add(new Literal { Text = content });
            if (addAsLink)
            {
                LinkButton button = new LinkButton
                {
                    ID = "Page" + text,
                    CommandArgument = commandArgument,
                    Text = text,
                };

                button.Click += this.button_Click;

                Controls.Add(button);
            }
            else
            {
                Controls.Add(new Label { Text = text });
            }
            Controls.Add(new Literal { Text = "</li>" });
        }

        void button_Click(object sender, EventArgs e)
        {
            var linkButton = sender as LinkButton;
            var pageIndex = linkButton.CommandArgument;
            int newPageIndex = 0;
            bool numValid = true;
            
            numValid = int.TryParse(pageIndex, out newPageIndex);
            if (newPageIndex < 0)
                newPageIndex = 0;
            if ((newPageIndex > TotalPages) || (!numValid))
                newPageIndex = TotalPages;
            
            OnPageIndexChanging(new GridViewPageEventArgs(newPageIndex));
            PageIndex = newPageIndex;

        }

        #region Events handling
        /// <summary>
        /// Selected index changed event
        /// </summary>
        /// <param name="sender">Event source</param>
        /// <param name="e">Event which contain no data</param>
        protected void SelectedIndexPagesChanged(object sender, System.EventArgs e)
        {
            int newPageIndex = 0;
            bool numValid = true;
            if (sender is TextBox)
            {
                numValid = int.TryParse(((TextBox)sender).Text, out newPageIndex);
                if (newPageIndex < 1)
                    newPageIndex = 1;
                if ((newPageIndex > TotalPages) || (!numValid))
                    newPageIndex = TotalPages;
                newPageIndex--;
            }
            else
                newPageIndex = ((DropDownList)sender).SelectedIndex;

            OnPageIndexChanging(new GridViewPageEventArgs(newPageIndex));
        }

        /// <summary>
        /// First, Previous, Next and Last Command
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PagerCommand(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        {
            int curPageIndex = this.PageIndex;


            int newPageIndex = 0;

            switch (e.CommandName)
            {
                case First:
                    newPageIndex = 0;
                    break;
                case Previous:
                    if (curPageIndex > 0)
                        newPageIndex = curPageIndex - 1;
                    break;
                case Next:
                    if (!(curPageIndex == TotalPages))
                        newPageIndex = curPageIndex + 1;
                    break;
                case Last:
                    newPageIndex = TotalPages - 1;
                    break;
            }

            OnPageIndexChanging(new GridViewPageEventArgs(newPageIndex));
        }

        /// <summary>
        /// Raises the Custom GridView.Sorting event
        /// </summary>
        /// <param name="e">A System.Web.UI.WebControls.GridViewSortEventArgs that contains event data</param>
        protected override void OnSorting(GridViewSortEventArgs e)
        {
            if (ViewState[CustomSortDirectionViewStateName] == null || !CustomSortExpression.Contains(e.SortExpression))
            {
                ViewState[CustomSortDirectionViewStateName] = "ASC";
                CustomSortExpression = e.SortExpression + " " + ViewState[CustomSortDirectionViewStateName].ToString();
            }
            else if (ViewState[CustomSortDirectionViewStateName].ToString() == "DESC")
            {
                ViewState[CustomSortDirectionViewStateName] = null;
                CustomSortExpression = "";
            }
            else
            {
                ViewState[CustomSortDirectionViewStateName] = "DESC";
                CustomSortExpression = e.SortExpression + " " + ViewState[CustomSortDirectionViewStateName].ToString();
            }

            base.OnSorting(e);

            //Fixed: The data source 'ObjectDataSource' does not support sorting with IEnumerable data. Automatic sorting is only supported with DataView, DataTable, and DataSet. 
            e.Cancel = true;
        }

        /// <summary>
        /// Raises the custom RowDataBound event.
        /// </summary>
        /// <param name="e">A custom GridViewRowEventArgs that contains event data.</param>
        protected override void OnRowDataBound(GridViewRowEventArgs e)
        {
            base.OnRowDataBound(e);
            if (e.Row.RowType == DataControlRowType.Header)
            {
                string imgDes = @" <img class='sort-direction' src='../Images/GridViewIcons/ascending.png' title='" + Properties.Resources.DescendingToolTip + "' />";
                string imgAsc = @" <img class='sort-direction' src='../Images/GridViewIcons/descending.png' title='" + Properties.Resources.AscendingToolTip + "' />";
                string imgAscDes = @" <img class='sort-direction' src='../Images/GridViewIcons/ascdesc.png' title='" + Properties.Resources.SortingToolTip + "' />";

                foreach (TableCell cell in e.Row.Cells)
                {
                    if (cell.HasControls() && cell.Controls[0].GetType().BaseType == typeof(LinkButton))
                    {
                        
                        LinkButton lnkButton = (LinkButton)cell.Controls[0];
                        if (lnkButton != null)
                        {
                            Panel sortPanel = new Panel();
                            sortPanel.CssClass = "header-sort-container";
                            Literal sortIcon = new Literal();
                            
                            string temp = string.Empty;
                            int idx = CustomSortExpression.IndexOf(" ");
                            if (idx > 0)
                                temp = CustomSortExpression.Substring(0, idx);
                            if (lnkButton.CommandArgument == temp)
                            {
                                if (CustomSortDirection == "ASC")
                                {
                                    // lnkButton.Text += imgAsc;
                                    sortIcon.Text = imgAsc;
                                }
                                else if (CustomSortDirection == "DESC") 
                                { 
                                    // lnkButton.Text += imgDes;
                                    sortIcon.Text = imgDes;
                                }
                            }
                            else
                            {
                                // lnkButton.Text += imgAscDes;
                                sortIcon.Text = imgAscDes;
                            }

                            sortPanel.Controls.Add(lnkButton);
                            sortPanel.Controls.Add(sortIcon);
                            cell.Controls.Clear();
                            cell.Controls.Add(sortPanel);
                        }
                    }
                }
            }
        }

        #endregion
    }
}