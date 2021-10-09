using System;
using System.IO;

class css
{
    private static string clase;

    public static void Save()
    {
        File.WriteAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SkynetChat", "css", "class.css"), GetMyCSS());
    }
    public static string GetMyCSS()
    {
        clase = @"



            .message-call 
            {
                width: 670;
                color: #bdbdbf;
                padding: 0,5px 0px;
            }
            
            .callcall 
            {
                right: 30px;
                position: fixed;
                z-index: 1000;
                bottom: 50px; 
                display: none; 
                opacity: 0.8; 
                cursor: pointer; 
                border-radius: 50px
            }
            .call:hover { opacity: 1; }

            .message-data-time 
            {
                color: #a8aab1;
                padding-top: 3px;
                font-size: 9pt;
                width:30px;
            }
            .message-data-type 
            {
                color: #a8aab1;
                padding-top: 3px;
                font-size: 8pt;
                overflow: hidden;
                width:37px;
            }
            .message-data-name 
            {
                color: #bdbdbf;
                padding-left: -3px;
                white-space: nowrap;
                font-size: 10pt;
                overflow: hidden;
                text-overflow: ellipsis;
            }

            
            .text-replace
            {
	            overflow:hidden;
	            color:transparent;
	            text-indent:100%;
	            white-space:nowrap
            }


            /*GotoTop*/
            .text-replace
            {
	            overflow:hidden;
	            color:transparent;
	            text-indent:100%;
	            white-space:nowrap
            }



            .form-control[aria-invalid='true']:focus:focus{box-shadow:0 0 0 3px hsla(355, 90%, 61%, 0.2);box-shadow:0 0 0 3px var(--color-shadow)}
            .form-label{font-size:0.83333em;font-size:var(--text-sm)}:root{--cd-color-1:hsl(53, 29%, 95%);--cd-color-1-h:53;--cd-color-1-s:29%;--cd-color-1-l:95%;--cd-color-2:hsl(330, 13%, 42%);--cd-color-2-h:330;--cd-color-2-s:13%;--cd-color-2-l:42%;--cd-color-3:hsl(5, 76%, 62%);--cd-color-3-h:5;--cd-color-3-s:76%;--cd-color-3-l:62%;--cd-back-to-top-size: 40px;--cd-back-to-top-margin: 20px;--font-primary: 'Bitter', sans-serif;--font-secondary: 'Open Sans', sans-serif}@supports (--css: variables){@media (min-width: 64rem){:root{--cd-back-to-top-size: 60px;--cd-back-to-top-margin: 30px}}}
            .cd-top{position:fixed;bottom:20px;bottom:var(--cd-back-to-top-margin);right:20px;right:var(--cd-back-to-top-margin);display:inline-block;height:40px;height:var(--cd-back-to-top-size);width:40px;width:var(--cd-back-to-top-size);box-shadow:0 0 10px rgba(0,0,0,0.05);background:url(cd-top-arrow.svg) no-repeat center 50%;background-color:hsla(5, 76%, 62%, 0.8);background-color:hsla(var(--cd-color-3-h), var(--cd-color-3-s), var(--cd-color-3-l), 0.8)}
            .js .cd-top
            {
	            visibility:hidden;
	            opacity:0;
	            transition:opacity .3s, visibility .3s, background-color .3s
            }
            .js .cd-top--is-visible
            {
	            visibility:visible;opacity:1
            }
            .js .cd-top--fade-out{opacity:.5}.js .cd-top:hover{background-color:hsl(5, 76%, 62%);background-color:var(--cd-color-3);opacity:1}body{background-color:hsl(330, 13%, 42%);background-color:var(--cd-color-2);-webkit-font-smoothing:antialiased;-moz-osx-font-smoothing:grayscale}

        ";
        return clase;
    }
    public static string GetHTMLCSS()
    {
        clase = @"

            .navbar-fixed-top,.navbar-fixed-bottom{position:fixed;right:0;left:0;z-index:1030}@media (min-width:768px){.navbar-fixed-top,.navbar-fixed-bottom{border-radius:0}}
            .navbar-fixed-top{top:0;border-width:0 0 1px}.navbar-fixed-bottom{bottom:0;margin-bottom:0;border-width:1px 0 0}
            .navbar-brand:hover,.navbar-brand:focus{text-decoration:none}@media (min-width:768px){.navbar>.container .navbar-brand,.navbar>.container-fluid .navbar-brand{margin-left:-15px}}
            .navbar-form.navbar-right:last-child{margin-right:-15px}}.navbar-nav>li>.dropdown-menu{margin-top:0;border-top-right-radius:0;border-top-left-radius:0}.navbar-fixed-bottom .navbar-nav>li>.dropdown-menu{border-bottom-right-radius:0;border-bottom-left-radius:0}
            .panel{margin-bottom:20px;background-color:#fff;border:1px solid transparent;border-radius:4px;-webkit-box-shadow:0 1px 1px rgba(0,0,0,.05);box-shadow:0 1px 1px rgba(0,0,0,.05)}.panel-body{padding:15px}.panel-heading{padding:10px 15px;border-bottom:1px solid transparent;border-top-right-radius:3px;border-top-left-radius:3px}.panel-heading>.dropdown .dropdown-toggle{color:inherit}.panel-title{margin-top:0;margin-bottom:0;font-size:16px;color:inherit}.panel-title>a{color:inherit}.panel-footer{padding:10px 15px;background-color:#f5f5f5;border-top:1px solid #ddd;border-bottom-right-radius:3px;border-bottom-left-radius:3px}.panel>.list-group{margin-bottom:0}.panel>.list-group .list-group-item{border-width:1px 0;border-radius:0}.panel>.list-group:first-child .list-group-item:first-child{border-top:0;border-top-right-radius:3px;border-top-left-radius:3px}.panel>.list-group:last-child .list-group-item:last-child{border-bottom:0;border-bottom-right-radius:3px;border-bottom-left-radius:3px}.panel-heading+.list-group .list-group-item:first-child{border-top-width:0}.panel>.table,.panel>.table-responsive>.table{margin-bottom:0}.panel>.table:first-child,.panel>.table-responsive:first-child>.table:first-child{border-top-right-radius:3px;border-top-left-radius:3px}.panel>.table:first-child>thead:first-child>tr:first-child td:first-child,.panel>
            .panel-group .panel-footer+.panel-collapse .panel-body{border-bottom:1px solid #ddd}.panel-default{border-color:#ddd}.panel-default>
            .panel-heading{color:#333;background-color:#f5f5f5;border-color:#ddd}.panel-default>.panel-heading+.panel-collapse .panel-body{border-top-color:#ddd}


            /* header */
            .header {
	            margin: 0;
	            padding: 0;
	            border: 0;
	            margin-bottom: 0;
	            background: #318ce7;
            }
            .header .my-user-photo {
	            float: left;
	            height: 28px;
	            margin-right: 8px;
	            margin-top: -4px;
	            overflow: hidden;
	            width: 28px;
            }
            .header .caret {
	            margin-left: 5px;
            }
            .header-fixed {
	            padding-top: 44px;
            }
            .header.navbar {
	            border-radius: 0;
	            box-shadow: none;
	            min-height: 44px;
            }
            .header h3, .header h4 {
	            margin: 0;
	            padding: 0;
            }
            .header .navbar-toggle {
	            border: 0;
            }
            .header .header-nav {
	            float: right;
            }
            .header .header-nav .icon-nav {
	            float: right;
            }
            .header .header-nav .icon-nav > li {
	            float: left;
            }
            .header .header-nav .icon-nav > li > a {
	            padding: 12px;
            }
            .header .header-nav .icon-nav > li > a > i {
	            font-size: 20px;
            }
            .header .header-nav .icon-nav > li > a > .badge {
	            background: none repeat scroll 0 0 #FF0000;
	            border: 0 none;
	            border-radius: 100%;
	            box-shadow: none;
	            position: absolute;
	            right: 0;
	            top: 7px;
            }
            .header .header-nav .icon-nav > li > .dropdown-menu {
	            margin-top: 0;
            }
            .header .navbar-header {
	            float: none !important;
            }
            .header .navbar-brand {
	            height: 44px;
	            padding-top: 0px;
	            padding-bottom: 0px;
	            color: #f5f5f5;
            }
            .header .navbar-brand .logo {
	            background-image: url('../img/om_48.ico');
	            background-repeat: no-repeat;
	            background-size: 36px 36px;
	            float: left;
	            height: 36px;
	            margin-top: 5px;
	            width: 36px;
            }
            .header .navbar-brand .name {
	            float: left;
	            margin: 10px 0 0 10px;
            }
            .header .navbar-toggle.page-sidebar-toggle {
	            display: none;
            }
            .header .settings .dropdown-menu {
	            width: 300px;
            }
            .header .header-nav .icon-nav > li > a {
	            color: #f5f5f5;
            }
            .header .header-nav .icon-nav > li > a:hover, .header .header-nav .icon-nav > li > a:focus, .header .header-nav .nav .open > a, .header .header-nav .nav .open > a:hover, .header .header-nav .nav .open > a:focus, .header .navbar-toggle.page-sidebar-toggle:hover, .header .navbar-toggle.page-sidebar-toggle:focus {
	            background: #084C9F !important;
	            color: #fff !important;
            }
            .page-fluid .header .container {
	            width: auto;
            }
            .header .navbar-toggle.sidebar-toggle, .header .navbar-toggle.mobile-menu {
	            display: block;
	            border: 0 none;
	            border-radius: 0;
	            margin: 0;
	            padding: 15px 10px;
            }
            .header .user-detail-menu {
	            padding: 10px;
	            width: 250px;
            }
            

            /*ScrollBars*/
            body {
            scrollbar-face-color:#3d4145;  //barra
            scrollbar-highligh-color:#3d4145; 
            scrollbar-3dligh-color:#00FF00; 
            scrollbar-darkshadow-color:#00FFFF; 
            scrollbar-shadow-color:#4d5156; //Borde afuera
            scrollbar-track-color:#1c1d20;
            scrollbar-arrow-color:#fff}
            ";
        return clase;
    }

    internal static string GetJS()
    {
        string intHeigth = @"
        <script>
        function GetPageHeight()
        {
	        var body = document.body;
 	        var html = document.documentElement;
 	        var height = Math.max(body.scrollHeight ,body.offsetHeight, html.clientHeight, html.scrollHeight, html.offsetHeight);
 	        return height;
         }
        </script>
";

        return intHeigth;
    }
}

