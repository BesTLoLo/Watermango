<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WaterPlants.aspx.cs" Inherits="Watermango.Pages.WaterPlants" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Watermango</title>
    <link href="../Images/1.png" rel="shortcut icon" type="image/x-icon" />
    <script src="../Scripts/Script.js"></script>
    <link href="../Style/GridStyle.css" rel="stylesheet" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.4.1/jquery.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div style="margin: 20px;">
                    <h1>Watermango</h1>
                    <asp:GridView ID="Plants_grd" runat="server" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false" CssClass="myGridClass" OnRowDataBound="Plants_grd_RowDataBound">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <input type="checkbox" id="selectAll_chk" runat="server" onclick="selectAllPlants(this.id)" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <input type="checkbox" id="select_chk" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ID">
                                <ItemTemplate>
                                    <asp:Label ID="plantId_lbl" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:ImageField HeaderText="Image" DataImageUrlField="Image" DataImageUrlFormatString="/Images/{0}"></asp:ImageField>
                            <asp:BoundField DataField="PlantName" HeaderText="Plant Name" />
                            <asp:TemplateField HeaderText="Start/Stop Watering">
                                <ItemTemplate>
                                    <input id="start_btn" type="button" runat="server" value="Start" class="button" />
                                    <input id="stop_btn" type="button" runat="server" value="Stop" hidden="hidden" class="buttonStop" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status" ItemStyle-Width="100">
                                <ItemTemplate>
                                    <span id="status_lbl" runat="server"></span>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Last watered time">
                                <ItemTemplate>
                                    <asp:Label ID="lastTime_txt" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Water Level">
                                <ItemTemplate>
                                    <progress value="0" max="10" id="waterBar_bar" runat="server"></progress>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Watering Count (last 24 hours)">
                                <ItemTemplate>
                                    <asp:Label ID="waterCount_lbl" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <input type="button" onclick="multiWatering()" value="Water multiple plants" class="button" />
                    <br />
                    <input type="hidden" id="multiWatering_txt" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
