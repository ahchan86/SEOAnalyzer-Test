using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Data;
using SEOAnalyzer.Models;
using SEOAnalyzer.Helper;

namespace SEOAnalyzer
{
    public partial class _Default : Page
    {

       

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                txtURL.Text = "";
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            
            DataTable dt = loadGrid();
            dgvWord1.DataSource = dt;
            dgvWord1.DataBind();

            bool isURL = Util.IsURLValidAsync(txtURL.Text);
            if(isURL)
            {
                spExternalURL.Visible = true;
                DataTable dtURL = LoadExternalURL();
                gvURL.DataSource = dtURL;
                gvURL.DataBind();

            }
            else
            {
                spExternalURL.Visible = false;
            }


            spanResult.Visible = true;
        }


        public DataTable LoadExternalURL()
        {
            DataTable dtURL = new DataTable();
            Dictionary<string, int> url = Util.GetAllExternalLinks(txtURL.Text);
            dtURL.Columns.AddRange(new DataColumn[2] { new DataColumn("url", typeof(string)),
                    new DataColumn("numURLOccur", typeof(int)) });

            foreach (var row in url)
            {
                dtURL.Rows.Add(row.Key, row.Value);
            }

            return dtURL;
        }
     

        public DataTable loadGrid()
        {
            DataTable dt = new DataTable();
            
            bool isURL = Util.IsURLValidAsync(txtURL.Text);


            Dictionary<string, int> text = Util.GetAllWordsInfo(txtURL.Text, chkFilterStopWord.Checked, isURL);
            dt.Columns.AddRange(new DataColumn[2] { new DataColumn("word", typeof(string)),
                    new DataColumn("numOccur", typeof(int)) });

            foreach (var row in text)
            {
                dt.Rows.Add(row.Key, row.Value);
            }

            return dt;
        }

      
        protected void dgvWord1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvWord1.PageIndex = e.NewPageIndex;
            dgvWord1.DataSource = loadGrid();
            dgvWord1.DataBind();
        }



        protected void dgvWord1_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortExpression = e.SortExpression;
            Session["Sortexpression"] = sortExpression;
            if (Session["SortDirection"] != null && Session["SortDirection"].ToString() == SortDirection.Descending.ToString())
            {
                Session["SortDirection"] = SortDirection.Ascending;
                Sort(sortExpression, "ASC");
            }
            else
            {
                Session["SortDirection"] = SortDirection.Descending;
                Sort(sortExpression, "DESC");
            }

          
        }

        private void Sort(string sortExpression, string Direction)
        {
            DataView dv = null;


            dv = new DataView(loadGrid());
            dv.Sort = sortExpression + " " + Direction;
            dgvWord1.DataSource = dv;
            dgvWord1.DataBind();
        }

        protected void gvURL_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvURL.PageIndex = e.NewPageIndex;
            gvURL.DataSource = LoadExternalURL();
            gvURL.DataBind();
        }

        protected void gvURL_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortExpression = e.SortExpression;
            Session["Sortexpression"] = sortExpression;
            if (Session["SortDirection"] != null && Session["SortDirection"].ToString() == SortDirection.Descending.ToString())
            {
                Session["SortDirection"] = SortDirection.Ascending;
                SortURL(sortExpression, "ASC");
            }
            else
            {
                Session["SortDirection"] = SortDirection.Descending;
                SortURL(sortExpression, "DESC");
            }
        }

        private void SortURL(string sortExpression, string Direction)
        {
            DataView dv = null;


            dv = new DataView(LoadExternalURL());
            dv.Sort = sortExpression + " " + Direction;
            gvURL.DataSource = dv;
            gvURL.DataBind();
        }
    }
}