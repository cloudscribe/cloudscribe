# SSRS (Sql Server Reporting Services)

https://docs.microsoft.com/en-us/sql/reporting-services/reporting-services-tutorials-ssrs?view=sql-server-2017

There is a vsix in the markletplace that adds a project tempalte for reports.

But there is a standalone Microsoft Sql Server Report Builder app that seems to work better and can be launched from 
http://desktop-l3pe8ab:8080/Reports/browse/

https://www.microsoft.com/en-us/download/confirmation.aspx?id=53613

https://marketplace.visualstudio.com/items?itemName=ProBITools.MicrosoftReportProjectsforVisualStudio

Using the vs project you deploy to a specified reportserver, whereas the standalone tool you publish report parts after edits.

In Report Designer, after you create tables, charts, and other paginated report items in a project, you can publish them as report parts to a report server 

http://desktop-l3pe8ab:8080/Reports/browse/

the base path for reports in viewer control then /folderifany/reportname
http://desktop-l3pe8ab:8080/ReportServer

biggest issue is the reportviewer control seems to be webforms only

https://docs.microsoft.com/en-us/sql/reporting-services/application-integration/using-the-webforms-reportviewer-control?view=sql-server-2017

There are a few tutorials how to show reports in mvc but they use 3rd party nugets not the viewercontrol from microsoft.

## Power BI

better for responsive web design vs ssrs which is more pixel perfect (good for printing) but not responsive.

desktop app
https://powerbi.microsoft.com/en-us/

self hosted
https://powerbi.microsoft.com/en-us/report-server/

javascript api
https://powerbi.microsoft.com/en-us/blog/intro-pbi-js-api/

https://microsoft.github.io/PowerBI-JavaScript/

https://github.com/microsoft/PowerBI-JavaScript

Power BI - Sample - Client - Angular
http://azure-samples.github.io/powerbi-angular-client/#/scenario3


https://spiderlogic.com/2018/07/20/embedding-power-bi-report-into-net-core-angular-js-application-with-app-owned-data/

https://blog.bennymichielsen.be/2017/11/09/powerbi-with-net-core/

