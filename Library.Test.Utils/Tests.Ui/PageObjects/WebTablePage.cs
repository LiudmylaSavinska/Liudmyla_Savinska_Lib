using Microsoft.Playwright;

namespace Library.Test.Utils.Tests.Ui.PageObjects;

public class WebTablePage: IBasePage
{
    public IPage? Page { get; set; }
    
    public string Url { get; } = "https://demoqa.com/webtables";
    public string ExpectedTitle { get; } = "Web Tables";
    
    public ILocator Title => Page!.Locator("xpath=//h1[contains(text(),'Web Tables')]");
    public ILocator Table => Page!.Locator("xpath=//div[@class='rt-tbody']");
    public ILocator AddButton => Page!.Locator("id=addNewRecordButton");
    public ILocator RegistrationForm => Page!.Locator("xpath=//div[@role='dialog']/div[@role='document']//div[@class='modal-header']");
    public ILocator FirstNameInput => Page!.Locator("xpath=/html//input[@id='firstName']");
    public ILocator LastNameInput => Page!.Locator("xpath=/html//input[@id='lastName']");
    public ILocator EmailInput => Page!.Locator("xpath=/html//input[@id='userEmail']");
    public ILocator AgeInput => Page!.Locator("xpath=/html//input[@id='age']");
    public ILocator SalaryInput => Page!.Locator("xpath=/html//input[@id='salary']");
    public ILocator DepartmentInput => Page!.Locator("xpath=/html//input[@id='department']");
    public ILocator SubmitButton => Page!.Locator("xpath=/html//button[@id='submit']");
    public ILocator Rows => Page!.Locator("xpath=//div/div[contains(@class,'rt-tr -odd') or contains(@class,'rt-tr -even')]");
    public ILocator Columns => Page!.Locator("xpath=//div[@class='rt-resizable-header-content']");
    public ILocator DeleteFirstRecordButton => Page!.Locator("id=delete-record-1");
    public ILocator AgeHeader => Page!.Locator("//div[@class='rt-resizable-header-content' and text()='Age']");
    
    public async Task<WebTablePage> Open()
    {
        await Page!.GotoAsync(Url);
        return this;
    }

    public async Task<WebTablePage> ClickAddButton()
    {
        await AddButton.ClickAsync();
        return this;
    }

    public async Task<WebTablePage> EnterFirstName(string firstName)
    {
        await FirstNameInput.FillAsync(firstName);
        return this;
    }
    
    public async Task<WebTablePage> EnterLastName(string lastName)
    {
        await LastNameInput.FillAsync(lastName);
        return this;
    }
    
    public async Task<WebTablePage> EnterEmail(string email)
    {
        await EmailInput.FillAsync(email);
        return this;
    }
    
    public async Task<WebTablePage> EnterAge(string age)
    {
        await AgeInput.FillAsync(age);
        return this;
    }
    
    public async Task<WebTablePage> EnterSalary(string salary)
    {
        await SalaryInput.FillAsync(salary);
        return this;
    }
    
    public async Task<WebTablePage> EnterDepartment(string department)
    {
        await DepartmentInput.FillAsync(department);
        return this;
    }
    
    public async Task<WebTablePage> ClickSubmitButton()
    {
        await SubmitButton.ClickAsync();
        return this;
    }

    public async Task<WebTablePage> SortByAge()
    {
        await AgeHeader.ClickAsync();
        return this;
    }

    public async Task<string[]> GetRowValues(int rowNumber)
    {
        var rowCells = Page!.Locator($"//div[@class='rt-tr-group'][{rowNumber}]//div[@role='gridcell']");

        var input = (await rowCells.AllTextContentsAsync()).ToArray();

        return input;
    }

    public async Task<List<string[]>> FindRows()
    {
        var rows = await Rows.AllAsync();
        var rowsWithInput = new List<string[]>();
        
        foreach (var row in rows)
        {
            var cellValues = await row.Locator(Rows).AllTextContentsAsync();

            if (cellValues.Any(cell => !string.IsNullOrWhiteSpace(cell)))
            {
                rowsWithInput.Add(cellValues.ToArray());
            }
        }

        return rowsWithInput;
    }
    
    public async Task<WebTablePage> ClosePage()
    {
        await Page!.CloseAsync();
        return this;
    }
}