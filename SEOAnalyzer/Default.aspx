<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SEOAnalyzer._Default" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">


    <div class="row">
        <div class="col-md-10">
            <h1>Please enter URL / Text</h1>
            <p>
                <asp:TextBox runat="server" Width="700px" ID="txtURL"></asp:TextBox><asp:Button runat="server" ID="btnSubmit" Text="Submit" OnClick="btnSubmit_Click" />
                <br />
                <span><asp:CheckBox ID="chkFilterStopWord" runat="server" Text=" Filters out stop-words"/></span>
            </p>
        </div>

    </div>
<%--    <asp:DataGrid ID="dgvWord" runat="server" ShowFooter="True" Width="500px"
        CssClass="table table-hover table-striped" 
        AutoGenerateColumns="False" OnPageIndexChanged="dgvWord_PageIndexChanged" AllowPaging="True">
        
        <HeaderStyle BackColor="#aaaadd"></HeaderStyle>
        <PagerStyle Mode="NumericPages">
      </PagerStyle> 
        <FooterStyle BackColor="#aaaadd"></FooterStyle>
        <Columns>
            <asp:BoundColumn DataField="word" HeaderText="Word" HeaderStyle-Width="30px" SortExpression="Word">
<HeaderStyle Width="30px"></HeaderStyle>
            </asp:BoundColumn>
            <asp:BoundColumn DataField="numOccur" HeaderText="Number of Occurence" HeaderStyle-Width="10px" HeaderStyle-HorizontalAlign="Center" SortExpression="NumOccur">
<HeaderStyle HorizontalAlign="Center" Width="10px"></HeaderStyle>
            </asp:BoundColumn>
        </Columns>
       
    </asp:DataGrid>--%>
    <span id="spanResult" runat="server" visible="false" style="width:800px">
     <h2>Number of word occurences on the page</h2>
    <asp:GridView runat="server" AllowPaging="True" AllowSorting="true" ID="dgvWord1" CssClass="table table-hover table-striped" OnPageIndexChanging="dgvWord1_PageIndexChanging" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" OnSorting="dgvWord1_Sorting">
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White"></HeaderStyle>
    
            <EditRowStyle BackColor="#2461BF" />
    
        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White"></FooterStyle>
            <AlternatingRowStyle BackColor="White" />
        <Columns>
            <asp:TemplateField HeaderText="Word" SortExpression="word" >
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("word") %>'></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("word") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Number of Occurence" SortExpression="NumOccur">
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("numOccur") %>'></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label2" runat="server" Text='<%# Bind("numOccur") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
            <PagerStyle CssClass="cssPager" BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#EFF3FB" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <SortedAscendingCellStyle BackColor="#F5F7FB" />
            <SortedAscendingHeaderStyle BackColor="#6D95E1" />
            <SortedDescendingCellStyle BackColor="#E9EBEF" />
            <SortedDescendingHeaderStyle BackColor="#4870BE" />
    </asp:GridView>
        <span id="spExternalURL" runat="server">
        <br />
        <h2>Number of External URL Link</h2>
    <asp:GridView runat="server" AllowPaging="True" ID="gvURL" AllowSorting="true" CssClass="table table-hover table-striped" OnPageIndexChanging="gvURL_PageIndexChanging" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" OnSorting="gvURL_Sorting" >
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White"></HeaderStyle>
    
            <EditRowStyle BackColor="#2461BF" />
    
        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White"></FooterStyle>
            <AlternatingRowStyle BackColor="White" />
        <Columns>
            <asp:TemplateField HeaderText="URL" SortExpression="url" >
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("url") %>'></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("url") %>'></asp:Label>
                </ItemTemplate>
                <HeaderStyle Width="500px" />
            </asp:TemplateField>
              <asp:TemplateField HeaderText="Number of Occurence" SortExpression="numURLOccur">
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("numURLOccur") %>'></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label2" runat="server" Text='<%# Bind("numURLOccur") %>'></asp:Label>
                </ItemTemplate>
                  <HeaderStyle Width="50px" />
            </asp:TemplateField>
        </Columns>
            <PagerStyle CssClass="cssPager" BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#EFF3FB" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <SortedAscendingCellStyle BackColor="#F5F7FB" />
            <SortedAscendingHeaderStyle BackColor="#6D95E1" />
            <SortedDescendingCellStyle BackColor="#E9EBEF" />
            <SortedDescendingHeaderStyle BackColor="#4870BE" />
    </asp:GridView>
</span>

        </span>

    <style>
               .cssPager td
        {
              padding-left: 4px;     
              padding-right: 4px;    
          }
    </style> 
  
</asp:Content>


