<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Mulaqats.aspx.cs" Inherits="IMRegister.Mulaqats" %>

<!DOCTYPE html>

<html dir="rtl" xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ملاقات رجسٹر </title>
    
    <script src="Scripts/jquery-1.10.2.min.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
    <script src="Scripts/angular.min.js"></script>
    <% if (Request.Browser.IsMobileDevice)
    {%>
        <link href="Styles/M.Bootstrap.min.css" rel="stylesheet" />
    <%}
    else {%>
        <link href="Styles/Bootstrap.min.css" rel="stylesheet" />
    <% }%>

    <style>
        
        .Cell {
            cursor: pointer;
        }

        .TextBoxError {
            background-color: #f7c2c2;
            border-color: red;
        }

        #dtGrid {
            width: 100%;
            overflow: auto;
        } 

        #DataGrid { 
            color: #333333;
            border-width: 1px;
            border-color: #999999;
            border-collapse: collapse;
        }

        #DataGrid th {                


                background: #d5e3e4;
               
        }

        #DataGrid td {          
                background: #ebecda;
                background: url(data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIiA/Pgo8c3ZnIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyIgd2lkdGg9IjEwMCUiIGhlaWdodD0iMTAwJSIgdmlld0JveD0iMCAwIDEgMSIgcHJlc2VydmVBc3BlY3RSYXRpbz0ibm9uZSI+CiAgPGxpbmVhckdyYWRpZW50IGlkPSJncmFkLXVjZ2ctZ2VuZXJhdGVkIiBncmFkaWVudFVuaXRzPSJ1c2VyU3BhY2VPblVzZSIgeDE9IjAlIiB5MT0iMCUiIHgyPSIwJSIgeTI9IjEwMCUiPgogICAgPHN0b3Agb2Zmc2V0PSIwJSIgc3RvcC1jb2xvcj0iI2ViZWNkYSIgc3RvcC1vcGFjaXR5PSIxIi8+CiAgICA8c3RvcCBvZmZzZXQ9IjQwJSIgc3RvcC1jb2xvcj0iI2UwZTBjNiIgc3RvcC1vcGFjaXR5PSIxIi8+CiAgICA8c3RvcCBvZmZzZXQ9IjEwMCUiIHN0b3AtY29sb3I9IiNjZWNlYjciIHN0b3Atb3BhY2l0eT0iMSIvPgogIDwvbGluZWFyR3JhZGllbnQ+CiAgPHJlY3QgeD0iMCIgeT0iMCIgd2lkdGg9IjEiIGhlaWdodD0iMSIgZmlsbD0idXJsKCNncmFkLXVjZ2ctZ2VuZXJhdGVkKSIgLz4KPC9zdmc+);
                background: -moz-linear-gradient(top, #ebecda 0%, #e0e0c6 40%, #ceceb7 100%);
                background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,#ebecda), color-stop(40%,#e0e0c6), color-stop(100%,#ceceb7));
                background: -webkit-linear-gradient(top, #ebecda 0%,#e0e0c6 40%,#ceceb7 100%);
                background: -o-linear-gradient(top, #ebecda 0%,#e0e0c6 40%,#ceceb7 100%);
                background: -ms-linear-gradient(top, #ebecda 0%,#e0e0c6 40%,#ceceb7 100%);
                background: linear-gradient(to bottom, #ebecda 0%,#e0e0c6 40%,#ceceb7 100%);
                border: 1px solid #999999;
            }
    </style>

    <script type="text/javascript">
        var GridData = '';

        $(document).ready(function () {
            $.get('ds/DostMulaqt')
                     .success(function (data, status, headers, config) {
                         if (data.Code == 0) {
                             GridData = data.Data;
                             LoadData();
                         }
                         else {
                             $(divAlert).addClass("");
                             $(lblAlertMessage).val('')
                         }
                     })
                     .error(function (data, status, header, config) {
                         alert('error')
                     });

            $('#txtNaam').blur(function () {
                if ($(txtNaam).val() == '') {
                    $(txtNaam).addClass("TextBoxError");
                }
                else {
                    $('#txtNaam').removeClass("TextBoxError");
                }
            });
        });

        function saveData() {
            $(btnMahfoozAll).addClass('.disabled');
            $('#txtNaam').prop("readonly", true);
            $.ajax({
                type: "POST",
                data: JSON.stringify(GridData),
                url: "ds/DostMulaqt",
                contentType: "application/json",
                success: function (data) {
                    GridData = data.Data;
                    LoadData();
                    $(btnMahfoozAll).removeClass('.disabled');
                    $('#txtNaam').removeAttr("readonly");
                },
                error:function () {
                    alert('error');
                }
             });             
        }

        function LoadData() {
            $('#divSarifNaam').text(GridData.Naam);
            //$('#dtGrid').append("<div id='colHeaders'/>");
            //$('#dtGrid').append("<div id='Rows'/>");
            $('#dtGrid').text('');
            $('#dtGrid').append("<table id='DataGrid' style='width:" + GridData.Columns * 180 + "px'/>");
            $('#DataGrid').append("<tr>")
            $('#DataGrid tr:last').append("<th class='ColHeader' >   دوست کا نام</th>");
            for (i = 0; i < GridData.Columns; i++) {
                $('#DataGrid tr:last').append("<th class='ColHeader'> ملاقات   " + (i + 1) + "</th>");
            }

            for (i = 0; i <= GridData.lstDost.length ; i++) {
                $('#DataGrid').append("<tr>");
                if (GridData.lstDost[i] != null) {
                    addDataRow();
                } else {
                    addEmptyRow();
                    i++;
                }
            }
        }
        function addDataRow() {
            $('#DataGrid tr:last').append("<td class='Cell'>" + GridData.lstDost[i].Naam + "</td>");
            for (j = 0; j < GridData.Columns; j++) { // data cel to display
                if (GridData.lstDost[i].lstMulaqat[j] != null) {
                    $('#DataGrid tr:last').append("<td id='" + i + ":" + j + "'class='Cell' onclick='Cel_Click(this)'  data-toggle='modal' data-target='#divMulaqat' >" + getFormatedDate(GridData.lstDost[i].lstMulaqat[j].Tarekh) + "</td>");
                } else { // empty cel for table format
                    GridData.lstDost[i].lstMulaqat.push(getNewMulaqat(0, '', '', 0));
                    $('#DataGrid tr:last').append("<td id='" + i + ":" + j + "' class='Cell'  onclick='Cel_Click(this)'  data-toggle='modal' data-target='#divMulaqat'></td>");
                }
            }
        }
        function addEmptyRow() {
            GridData.lstDost.push(getNewDost(0, '', '', 0,0));
            var i = $('#DataGrid tr').length - 2;
            $('#DataGrid tr:last').append("<td id='" + i + "' class='Cell' ><input onblur='txtNayaDost_FocusLost(this," + i + ")' type='text' class='form-control'/></td>");
            for (j = 0; j < GridData.Columns; j++) {
                $('#DataGrid tr:last').append("<td  id='" + i + ":" + j + "' class='Cell'  onclick='Cel_Click(this)' data-toggle='modal' data-target='#divMulaqat'></td>");
            }
        }
        function Cel_Click(obj) {
            var cel = $(obj).attr('id').split(":");
            $(txtNaam).val(GridData.lstDost[cel[0]].Naam);
            $(txtIndex).val($(obj).attr('id'));
            if (GridData.lstDost[cel[0]].lstMulaqat[cel[1]].Halath == 3) {

                $(dtTareekh).val(getDateForCal(GridData.lstDost[cel[0]].lstMulaqat[cel[1]].Tarekh));
                $(txtTafseel).val(GridData.lstDost[cel[0]].lstMulaqat[cel[1]].Tafseel);
            }
            else {

                var d = new Date();
                var strDate = d.getFullYear() + "-" + padDate(d.getMonth() + 1) + "-" + padDate(d.getDate());

                $(dtTareekh).val(strDate);
                $(txtTafseel).val(GridData.lstDost[cel[0]].lstMulaqat[cel[1]].Tafseel);
            }
            if (GridData.lstDost[cel[0]].Halath == 3) {
                $('#txtNaam').prop("readonly", true);
            }
            else {
                $('#txtNaam').removeAttr("readonly");
            }
        }
        
        function txtNayaDost_FocusLost(obj, index) {
            if (GridData.lstDost[index].Naam != $(obj).val()) {
                GridData.lstDost[index].Naam = $(obj).val();
                if (GridData.lstDost[index].Halath == 0 || GridData.lstDost[index].Halath == 1) {
                    GridData.lstDost[index].Halath = 1;
                }
                else {
                    GridData.lstDost[index].Halath = 2;
                }
            }

            if ($('#DataGrid tr').length - 2 == index && obj.value != '') {
                GridData.lstDost[index].Naam = obj.value;
                $('#DataGrid').append("<tr>");
                addEmptyRow();
            }
        }

        function btnShamil_Click() {
            if ($(txtNaam).val() == '') {
                $(txtNaam).addClass("TextBoxError");
                return;
            }

            var cel = $(txtIndex).val().split(":");
            if (GridData.lstDost[cel[0]].lstMulaqat[cel[1]].Halath == 0) {
                for (i = 0; i < GridData.Columns; i++) {
                    if (GridData.lstDost[cel[0]].lstMulaqat[i].Halath == 0) {
                        break;
                    }
                }
            } else {
                i = cel[1];
            }
            if (GridData.lstDost[cel[0]].Naam != $(txtNaam).val()) {
                GridData.lstDost[cel[0]].Naam = $(txtNaam).val();
                if (GridData.lstDost[cel[0]].Halath == 0 || GridData.lstDost[cel[0]].Halath == 1) {
                    GridData.lstDost[cel[0]].Halath = 1;
                }
                else {
                    GridData.lstDost[cel[0]].Halath = 2;
                }
            }
            GridData.lstDost[cel[0]].lstMulaqat[i].Tarekh = $(dtTareekh).val();
            GridData.lstDost[cel[0]].lstMulaqat[i].Tafseel = $(txtTafseel).val();

            if (GridData.lstDost[cel[0]].lstMulaqat[cel[1]].Halath == 0 || GridData.lstDost[cel[0]].lstMulaqat[cel[1]].Halath == 1) {
                GridData.lstDost[cel[0]].lstMulaqat[i].Halath = 1; if (GridData.lstDost[cel[0]].lstMulaqat[cel[1]].Halath == 0 || GridData.lstDost[cel[0]].lstMulaqat[cel[1]].Halath == 1) {
                }
                else {
                    GridData.lstDost[cel[0]].lstMulaqat[i].Halath = 2;
                }

                $('#DataGrid tr').eq(Number(cel[0]) + 1).find('td').eq(Number(i) + 1).text(getFormatedDate($(dtTareekh).val(),1));
                $('#DataGrid tr').eq(Number(cel[0]) + 1).find('td').eq(0).find('input').val($(txtNaam).val());
                $('#DataGrid tr').eq(Number(cel[0]) + 1).find('td').eq(0).find('input').focus();

                $('#divMulaqat').modal('toggle');
            }
        }

        function btnHazaf_Click()
        {
            var cel = $(txtIndex).val().split(":");
            if (GridData.lstDost[cel[0]].lstMulaqat[cel[1]].Halath == 3 || GridData.lstDost[cel[0]].lstMulaqat[cel[1]].Halath == 2) {
                GridData.lstDost[cel[0]].lstMulaqat[cel[1]].Halath = 4;
            }
            else {
                GridData.lstDost[cel[0]].lstMulaqat[cel[1]].Halath = 0;
            }
            $('#DataGrid tr').eq(Number(cel[0]) + 1).find('td').eq(Number(cel[1]) + 1).text(''); 

            $('#divMulaqat').modal('toggle');

        }

        function getNewDost(id, naam, tafseel,hasiath, halat) {
            var newDost = new Object();
            newDost.Id = id;
            newDost.Naam = naam;
            newDost.Tafseel = tafseel;
            newDost.Hasiath = hasiath;
            newDost.Halath = halat;
            newDost.lstMulaqat = [];
            for (i = 0; i < GridData.Columns; i++) {
                newDost.lstMulaqat.push(getNewMulaqat(0, '', '', 0));
            }
            return (newDost);
        }
        function getNewMulaqat(id, tarekh, tafseel, halath) {
            var newMulaqat = new Object();
            newMulaqat.Id = id;
            newMulaqat.Tarekh = tarekh;
            newMulaqat.Tafseel = tafseel;
            newMulaqat.Halath = halath;
            return newMulaqat;
        }
        function padDate(s) {
            s = s + '';
            if (s.length === 1) s = '0' + s;
            return s;
        }
        function getFormatedDate(dt,type) {
            if (type==1){
                dtArry = dt.split('-');
                return (dtArry[2] + '-' + dtArry[1] + '-' + dtArry[0]);
            }
            else{
                dtArry = dt.split(' ')[0].split('/');
                return (dtArry[0] + '-' + dtArry[1] + '-' + dtArry[2]);
            }
            
        }

        function getDateForCal(dt) {
            dtArry = dt.split(' ')[0].split('/');
            return (dtArry[2] + '-' + padDate(dtArry[1]) + '-' + padDate(dtArry[0]));
        }


    </script>
</head>
<body>    
    <div class="">
        <div class="btn-group btn_title" role="group" aria-label="..."  >
            <button type="button" id="btnMahfoozAll" class="btn btn-default" onclick="saveData()"> محفوظ کریں </button> 

            <div class="btn-group" role="group">
                <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" >
                   خوش آمدید <strong><lable id='divSarifNaam' style="margin-right:5px; margin-left:10px"></lable> </strong>
                    <span class="caret"></span>
                </button>
                <ul class="dropdown-menu">
                    <li><a href="#">لاگ آوٹ</a></li> 
                </ul>
            </div>
        </div>

        <div class="title">   ملاقات رجسٹر </div>

       <!-- <div id="divAlert" class="alert alert-dismissible" role="alert">
            <button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>
            <lable id="lblAlertMessage"/>
        </div>
        -->

        <div id="dtGrid" style="margin-top:45px auto;"></div>

        <div id="divMulaqat" class="modal fade" role="dialog">
            <div class="modal-dialog" style='width: 400px'>

                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <div class="btn-group btn-group-justified" role="group" aria-label="...">
                            <div class="btn-group" role="group">
                                <button type="button" class="btn btn-default" onclick="btnShamil_Click()" >شامل کریں </button>
                            </div>
                            <div class="btn-group" role="group">
                                <button type="button" class="btn btn-default" onclick="btnHazaf_Click()" >حذف کریں </button>
                            </div>
                            <div class="btn-group" role="group">
                                <button type="button" class="btn btn-default" data-dismiss="modal" > منسوخ کریں </button>
                            </div>
                        </div>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <div class="form-group form-group-lg">
                                <div class="col-sm-10">
                                    <input type="hidden" id="txtIndex" />
                                    <input type="text" id="txtNaam" class="form-control" placeholder="دوست" />
                                </div>
                                <label class="col-sm-2 control-label" for="formGroupInputLarge">دوست</label>
                            </div>
                            <div class="form-group form-group-lg">
                                <div class="col-sm-10">
                                    <input type="date" id="dtTareekh" class="form-control" placeholder="تاریخ" />
                                </div>
                                <label class="col-sm-2 control-label" for="formGroupInputLarge">تاریخ</label>
                            </div>
                            <div class="form-group form-group-lg">
                                <div class="col-sm-10">
                                    <textarea id="txtTafseel" class="form-control" placeholder="تفصیل"></textarea>
                                </div>
                                <label class="col-sm-2 control-label" for="formGroupInputLarge">تفصیل</label>
                            </div>
                        </div>
                    </div>
                     
                </div>
            </div>
        </div>
    </div>
</body>
</html>
