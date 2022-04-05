namespace Shop.Automation.Framework
{
    public class TestExecutionReport
    {
        //public string filename { get; set; }

        //public void createexcel(string currentclassname)
        //{
        //    excel.application xlapp;
        //    excel.workbook xlworkbook;
        //    excel.worksheet xlworksheet;
        //    object misvalue = system.reflection.missing.value;
        //    xlapp = new excel.application();
        //    if (filename == null)
        //    {
        //        xlworkbook = xlapp.workbooks.add(misvalue);
        //        xlworksheet = (excel.worksheet)xlworkbook.worksheets.get_item(1);
        //        xlworksheet.cells[1, 1] = "testcase name";
        //        xlworksheet.cells[1, 2] = "test result";
        //        xlworksheet.cells[1, 3] = "comments";
        //        string filename = currentclassname;
        //        filename = filename + datetime.now.year.tostring() + datetime.now.month.tostring() + datetime.now.day.tostring() + datetime.now.hour.tostring() + datetime.now.minute.tostring();
        //        string filelocation = "c:\\testresult\\" + filename + ".xls";

        //        xlworkbook.saveas(filelocation, excel.xlfileformat.xlworkbooknormal, misvalue, misvalue, misvalue, misvalue, excel.xlsaveasaccessmode.xlshared, misvalue, misvalue, misvalue, misvalue, misvalue);
        //        xlworkbook.close(true, misvalue, misvalue);
        //    }

        //    xlapp.quit();
        //}

        //public void updateexcel(string testcase, string result, string comments)
        //{
        //    excel.application xlapp = new excel.application();
        //    object misvalue = system.reflection.missing.value;

        //    excel.workbook xlworkbook = xlapp.workbooks.open("c:\\testresult\\" + filename + ".xls", 0, false, 5, "", "", false,
        //    excel.xlplatform.xlwindows, "", true, false, 0, true, false, false);
        //    excel.worksheet xlworksheet = (excel.worksheet)xlworkbook.worksheets.get_item(1);

        //    excel.range xlrange = xlworksheet.usedrange;
        //    int rownumber = xlrange.rows.count + 1;

        //    xlworksheet.cells[rownumber, 1] = testcase;
        //    xlworksheet.cells[rownumber, 2] = result;
        //    xlworksheet.cells[rownumber, 3] = comments;

        //    xlworkbook.saveas("c:\\testresult\\" + filename + ".xls", excel.xlfileformat.xlworkbooknormal, misvalue, misvalue, misvalue, misvalue, excel.xlsaveasaccessmode.xlshared, misvalue, misvalue, misvalue, misvalue, misvalue);
        //    xlworkbook.close(true, misvalue, misvalue);
        //    xlapp.quit();
        //}
    }
}

