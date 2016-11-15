<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="IMRegister.login" %>

<!DOCTYPE html>
<html dir="rtl">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
    <meta http-equiv="Content-Type" content="text/html; charset=ISO-8859-1">

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

    <title>Login</title>

    <style type="text/css">
         

        .alert-error {
            background-color: #fee5e2;
            border: 1px solid #fcaca5;
            color: #b50303;
            height: 30px;
            padding: 6px;
        }
    </style>
    <script type="text/javascript">
        function checkTextBoxes(){
            var chk=true;
            $("#login").removeClass("has-error");
            $("#password").removeClass("has-error");

            if ($("#login").val() == "") { 
                $("#login").addClass("has-error");
                chk=false;
            }

            if ($("#password").val() == "") { 
                $("#password").addClass("has-error");
                chk=false;
            }
            return chk;
        }

        function signInClick() {

            if(!checkTextBoxes())
                return;

            $(btnLogin).addClass('.disabled');
           
            $.ajax({
                type: "POST", 
                url: "ds/Sarif/Login/" + $("#login").val() + "ß" + $("#password").val(),
                success: function (data) {
                    if(data.Code === 0){
                        $(location).attr('href', 'Mulaqats.aspx?kxk=' + data.Token);
                    }
                    else{
                        alert(data.Description)
                    }
                },
                error:function () {
                    alert('error');
                }
            });  
        }

             
    </script>
</head>
<body>
    <div class="loginMainDiv">
        <div class="loginImageDiv"  >
            <img src="Images/chand.png" />
        </div>
        <div class="loginAreaCustom">
            <div class="control-group">
                <input class="form-control" id="login"
                    name="user" placeholder="یوزر" type="text" value=""
                    aria-required="true">
                <div class="form-validator-stack help-inline">
                     

                </div>
            </div>
            <div class="control-group" style="margin-top: 5px">
                <input class="form-control"
                    id="password" name="password" placeholder="پاس ورڈ"
                    type="password" value="" aria-required="true">
                <div class="form-validator-stack help-inline">
                     

                </div>
            </div>
            <div class="button-holder " style="margin-top: 5px">
                <button class="btn  btn-default col-lg-12" type="submit" runat="server" id="btnLogin"
                    onclick="signInClick()">
                    آغاز کریں
                </button>
                  </div>
        </div>
    </div>
</body>
</html>